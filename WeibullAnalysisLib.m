classdef WeibullAnalysisLib
    %% ================================================
    %  WeibullAnalysisLib - Weibull分析类库
    %  版本: 1.2.0 (使用纯 JDBC API，不依赖 Database Toolbox)
    %  更新日期: 2025-12-07
    %  
    %  重要改动：使用 Java JDBC API 直接连接数据库
    %  完全绕过 Database Toolbox，解决部署环境 fetch 函数不可用问题
    %  ================================================
    
    properties (Constant)
        VERSION = '1.2.0'
        DEFAULT_CONFIDENCE_LEVEL = 0.95
    end
    
    properties (Access = private)
        javaConn        % Java JDBC 连接对象
        connSuccess     % 连接状态
        dbHost
        dbPort
        dbName
        dbUser
        dbPassword
        jdbcDriverPath
    end
    
    methods
        %% 构造函数
        function obj = WeibullAnalysisLib()
            obj.dbHost = 'localhost';
            obj.dbPort = 3306;
            obj.dbName = 'SysHardTestDB';
            obj.dbUser = 'root';
            obj.dbPassword = '*hml2023PLTKIB*';
            obj.jdbcDriverPath = WeibullAnalysisLib.FindJdbcDriver();
            obj.javaConn = [];
            obj.connSuccess = false;
        end
        
        %% 设置数据库连接参数
        function SetConnectionParams(obj, host, port, database, user, password, jdbcPath)
            if nargin >= 2 && ~isempty(host), obj.dbHost = host; end
            if nargin >= 3 && ~isempty(port), obj.dbPort = port; end
            if nargin >= 4 && ~isempty(database), obj.dbName = database; end
            if nargin >= 5 && ~isempty(user), obj.dbUser = user; end
            if nargin >= 6 && ~isempty(password), obj.dbPassword = password; end
            if nargin >= 7 && ~isempty(jdbcPath), obj.jdbcDriverPath = jdbcPath; end
        end
        
        %% 连接数据库 (使用纯 Java JDBC API)
        function [success, message] = Connect(obj)
            success = false;
            message = '';
            
            % 加载 JDBC 驱动
            if ~isempty(obj.jdbcDriverPath) && exist(obj.jdbcDriverPath, 'file')
                try
                    javaaddpath(obj.jdbcDriverPath);
                catch
                end
            else
                message = ['JDBC 驱动未找到: ' obj.jdbcDriverPath];
                return;
            end
            
            % 使用 Java JDBC API 连接
            try
                % 加载驱动类
                java.lang.Class.forName('com.mysql.cj.jdbc.Driver');
                
                % 构建连接 URL
                jdbcURL = sprintf('jdbc:mysql://%s:%d/%s?useSSL=false&allowPublicKeyRetrieval=true&serverTimezone=UTC&characterEncoding=UTF-8', ...
                    obj.dbHost, obj.dbPort, obj.dbName);
                
                % 获取连接
                obj.javaConn = java.sql.DriverManager.getConnection(jdbcURL, obj.dbUser, obj.dbPassword);
                
                if ~isempty(obj.javaConn) && ~obj.javaConn.isClosed()
                    obj.connSuccess = true;
                    success = true;
                    message = 'JDBC 连接成功';
                else
                    message = 'JDBC 连接返回空或已关闭';
                end
            catch ME
                message = ['JDBC 连接失败: ' ME.message];
                obj.connSuccess = false;
            end
        end
        
        %% 断开数据库连接
        function Disconnect(obj)
            if ~isempty(obj.javaConn)
                try
                    if ~obj.javaConn.isClosed()
                        obj.javaConn.close();
                    end
                catch
                end
            end
            obj.javaConn = [];
            obj.connSuccess = false;
        end
        
        %% 执行 SQL 查询 (使用纯 Java JDBC API)
        function data = ExecuteQuery(obj, sqlQuery)
            data = [];
            
            if ~obj.connSuccess || isempty(obj.javaConn)
                error('数据库未连接');
            end
            
            try
                % 创建 Statement
                stmt = obj.javaConn.createStatement();
                
                % 执行查询
                rs = stmt.executeQuery(sqlQuery);
                
                % 获取元数据
                metaData = rs.getMetaData();
                numCols = metaData.getColumnCount();
                
                % 获取列名
                colNames = cell(1, numCols);
                for i = 1:numCols
                    colNames{i} = char(metaData.getColumnLabel(i));
                end
                
                % 读取数据
                rows = {};
                rowIdx = 0;
                while rs.next()
                    rowIdx = rowIdx + 1;
                    rowData = cell(1, numCols);
                    for i = 1:numCols
                        val = rs.getObject(i);
                        if isempty(val) || rs.wasNull()
                            rowData{i} = [];
                        elseif isjava(val)
                            % 转换 Java 对象为 MATLAB 类型
                            className = class(val);
                            if contains(className, 'String')
                                rowData{i} = char(val);
                            elseif contains(className, 'Integer') || contains(className, 'Long')
                                rowData{i} = double(val);
                            elseif contains(className, 'Double') || contains(className, 'Float')
                                rowData{i} = double(val);
                            elseif contains(className, 'BigDecimal')
                                rowData{i} = double(val.doubleValue());
                            else
                                rowData{i} = double(val);
                            end
                        else
                            rowData{i} = val;
                        end
                    end
                    rows{rowIdx} = rowData; %#ok<AGROW>
                end
                
                % 关闭资源
                rs.close();
                stmt.close();
                
                % 转换为表格
                if rowIdx > 0
                    % 构建表格数据
                    tableData = cell(rowIdx, numCols);
                    for r = 1:rowIdx
                        tableData(r, :) = rows{r};
                    end
                    data = cell2table(tableData, 'VariableNames', colNames);
                end
                
            catch ME
                error(['SQL 查询失败: ' ME.message]);
            end
        end
        
        %% 按模组ID进行Weibull分析
        function result = WeibullAnalysisByModuleID(obj, moduleID)
            result = obj.CreateEmptyResult();
            
            % 确保连接
            if ~obj.connSuccess
                [success, msg] = obj.Connect();
                if ~success
                    result.success = false;
                    result.message = ['数据库连接失败: ' msg];
                    return;
                end
            end
            
            % 转换 moduleID
            if ischar(moduleID) || isstring(moduleID)
                moduleID = str2double(moduleID);
            end
            
            % 构建 SQL
            sqlQuery = sprintf(['SELECT m.module_id, m.module_code, m.module_name, ' ...
                'td.failure_time, ' ...
                'COALESCE(td.last_inspection_time, 0) AS last_inspection_time, ' ...
                'COALESCE(td.quantity, 1) AS quantity, ' ...
                'COALESCE(td.censoring_type, td.is_censored, 0) AS censoring_type ' ...
                'FROM tb_module m INNER JOIN tb_test_data td ON m.module_id = td.module_id ' ...
                'WHERE m.module_id = %d AND td.failure_time IS NOT NULL AND td.failure_time > 0 ' ...
                'ORDER BY td.failure_time'], moduleID);
            
            try
                rawData = obj.ExecuteQuery(sqlQuery);
                
                if isempty(rawData) || height(rawData) == 0
                    result.success = false;
                    result.message = sprintf('模组ID=%d 没有找到数据', moduleID);
                    return;
                end
                
                result = obj.AnalyzeModuleData(rawData);
            catch ME
                result.success = false;
                result.message = ['数据查询失败: ' ME.message];
            end
        end
        
        %% 按模组代码进行Weibull分析
        function result = WeibullAnalysisByModuleCode(obj, moduleCode)
            result = obj.CreateEmptyResult();
            
            if ~obj.connSuccess
                [success, msg] = obj.Connect();
                if ~success
                    result.success = false;
                    result.message = ['数据库连接失败: ' msg];
                    return;
                end
            end
            
            sqlQuery = sprintf(['SELECT m.module_id, m.module_code, m.module_name, ' ...
                'td.failure_time, ' ...
                'COALESCE(td.last_inspection_time, 0) AS last_inspection_time, ' ...
                'COALESCE(td.quantity, 1) AS quantity, ' ...
                'COALESCE(td.censoring_type, td.is_censored, 0) AS censoring_type ' ...
                'FROM tb_module m INNER JOIN tb_test_data td ON m.module_id = td.module_id ' ...
                'WHERE m.module_code = ''%s'' AND td.failure_time IS NOT NULL AND td.failure_time > 0 ' ...
                'ORDER BY td.failure_time'], moduleCode);
            
            try
                rawData = obj.ExecuteQuery(sqlQuery);
                
                if isempty(rawData) || height(rawData) == 0
                    result.success = false;
                    result.message = sprintf('模组代码=%s 没有找到数据', moduleCode);
                    return;
                end
                
                result = obj.AnalyzeModuleData(rawData);
            catch ME
                result.success = false;
                result.message = ['数据查询失败: ' ME.message];
            end
        end
        
        %% 直接使用数据进行分析 (不需要数据库)
        function result = WeibullAnalysisFromData(obj, failureTimes, censoringTypes, quantities, lastInspectionTimes, moduleCode, moduleName)
            result = obj.CreateEmptyResult();
            
            if nargin < 2 || isempty(failureTimes)
                result.success = false;
                result.message = '缺少失效时间数据';
                return;
            end
            
            failureTimes = failureTimes(:);
            n = length(failureTimes);
            
            if nargin < 3 || isempty(censoringTypes)
                censoringTypes = zeros(n, 1);
            else
                censoringTypes = censoringTypes(:);
            end
            
            if nargin < 4 || isempty(quantities)
                quantities = ones(n, 1);
            else
                quantities = quantities(:);
            end
            
            if nargin < 5 || isempty(lastInspectionTimes)
                lastInspectionTimes = zeros(n, 1);
            else
                lastInspectionTimes = lastInspectionTimes(:);
            end
            
            if nargin < 6 || isempty(moduleCode)
                moduleCode = 'DirectInput';
            end
            
            if nargin < 7 || isempty(moduleName)
                moduleName = '直接输入数据';
            end
            
            validIdx = failureTimes > 0 & isfinite(failureTimes);
            if sum(validIdx) < 2
                result.success = false;
                result.message = '有效数据不足（至少需要2个）';
                return;
            end
            
            try
                times = failureTimes(validIdx);
                censoring = censoringTypes(validIdx);
                qty = quantities(validIdx);
                lastInsp = lastInspectionTimes(validIdx);
                
                [beta, eta, mleMsg] = WeibullAnalysisLib.WeibullMLEAllTypes(times, lastInsp, censoring, qty);
                
                if isnan(beta) || isnan(eta)
                    result.success = false;
                    result.message = ['参数估计失败: ' mleMsg];
                    return;
                end
                
                r2 = WeibullAnalysisLib.CalcR2AllTypes(times, lastInsp, censoring);
                [lowerBeta, upperBeta, lowerEta, upperEta] = WeibullAnalysisLib.CalcConfidenceIntervals(censoring, qty, beta, eta);
                [mttf, median_life, b10, b50, b90] = WeibullAnalysisLib.CalcLifeMetrics(beta, eta);
                
                result.success = true;
                result.message = 'OK';
                result.moduleID = 0;
                result.moduleCode = moduleCode;
                result.moduleName = moduleName;
                result.beta = beta;
                result.eta = eta;
                result.mttf = mttf;
                result.median = median_life;
                result.b10 = b10;
                result.b50 = b50;
                result.b90 = b90;
                result.r2 = r2;
                result.lowerBeta = lowerBeta;
                result.upperBeta = upperBeta;
                result.lowerEta = lowerEta;
                result.upperEta = upperEta;
                result.totalN = sum(qty);
                result.completeN = sum(qty(censoring == 0));
                result.rightCensN = sum(qty(censoring == 1));
                result.intervalCensN = sum(qty(censoring == 2));
                result.leftCensN = sum(qty(censoring == 3));
            catch ME
                result.success = false;
                result.message = ['分析过程出错: ' ME.message];
            end
        end
        
        %% 批量分析所有模组
        function results = WeibullAnalysisAll(obj)
            if ~obj.connSuccess
                [success, msg] = obj.Connect();
                if ~success
                    results = obj.CreateEmptyResult();
                    results.message = ['数据库连接失败: ' msg];
                    return;
                end
            end
            
            try
                sqlQuery = ['SELECT DISTINCT m.module_id FROM tb_module m ' ...
                    'INNER JOIN tb_test_data td ON m.module_id = td.module_id ' ...
                    'WHERE m.is_active = 1 AND td.failure_time > 0'];
                modules = obj.ExecuteQuery(sqlQuery);
                
                if isempty(modules) || height(modules) == 0
                    results = obj.CreateEmptyResult();
                    results.message = '没有找到活跃模组';
                    return;
                end
                
                nModules = height(modules);
                results = repmat(obj.CreateEmptyResult(), nModules, 1);
                
                for i = 1:nModules
                    moduleID = modules.module_id(i);
                    if iscell(moduleID)
                        moduleID = moduleID{1};
                    end
                    results(i) = obj.WeibullAnalysisByModuleID(moduleID);
                end
            catch ME
                results = obj.CreateEmptyResult();
                results.message = ['批量分析失败: ' ME.message];
            end
        end
    end
    
    methods (Access = private)
        function result = CreateEmptyResult(~)
            result.success = false;
            result.message = '';
            result.moduleID = 0;
            result.moduleCode = '';
            result.moduleName = '';
            result.beta = NaN;
            result.eta = NaN;
            result.mttf = NaN;
            result.median = NaN;
            result.b10 = NaN;
            result.b50 = NaN;
            result.b90 = NaN;
            result.totalN = 0;
            result.completeN = 0;
            result.rightCensN = 0;
            result.intervalCensN = 0;
            result.leftCensN = 0;
            result.r2 = NaN;
            result.lowerBeta = NaN;
            result.upperBeta = NaN;
            result.lowerEta = NaN;
            result.upperEta = NaN;
        end
        
        function result = AnalyzeModuleData(obj, rawData)
            result = obj.CreateEmptyResult();
            try
                % 提取数据
                moduleID = obj.SafeGetValue(rawData, 'module_id', 1);
                moduleCode = obj.SafeGetString(rawData, 'module_code', 1);
                moduleName = obj.SafeGetString(rawData, 'module_name', 1);
                
                times = obj.SafeGetColumn(rawData, 'failure_time');
                lastInspTimes = obj.SafeGetColumn(rawData, 'last_inspection_time');
                quantities = obj.SafeGetColumn(rawData, 'quantity');
                censoringTypes = obj.SafeGetColumn(rawData, 'censoring_type');
                
                validIdx = times > 0 & isfinite(times);
                if sum(validIdx) < 2
                    result.success = false;
                    result.message = '有效数据不足';
                    result.moduleID = moduleID;
                    result.moduleCode = moduleCode;
                    result.moduleName = moduleName;
                    return;
                end
                
                times = times(validIdx);
                lastInspTimes = lastInspTimes(validIdx);
                quantities = quantities(validIdx);
                censoringTypes = censoringTypes(validIdx);
                
                [beta, eta, mleMsg] = WeibullAnalysisLib.WeibullMLEAllTypes(times, lastInspTimes, censoringTypes, quantities);
                
                if isnan(beta) || isnan(eta)
                    result.success = false;
                    result.message = ['参数估计失败: ' mleMsg];
                    result.moduleID = moduleID;
                    result.moduleCode = moduleCode;
                    result.moduleName = moduleName;
                    return;
                end
                
                r2 = WeibullAnalysisLib.CalcR2AllTypes(times, lastInspTimes, censoringTypes);
                [lowerBeta, upperBeta, lowerEta, upperEta] = WeibullAnalysisLib.CalcConfidenceIntervals(censoringTypes, quantities, beta, eta);
                [mttf, median_life, b10, b50, b90] = WeibullAnalysisLib.CalcLifeMetrics(beta, eta);
                
                result.success = true;
                result.message = 'OK';
                result.moduleID = moduleID;
                result.moduleCode = moduleCode;
                result.moduleName = moduleName;
                result.beta = beta;
                result.eta = eta;
                result.mttf = mttf;
                result.median = median_life;
                result.b10 = b10;
                result.b50 = b50;
                result.b90 = b90;
                result.r2 = r2;
                result.lowerBeta = lowerBeta;
                result.upperBeta = upperBeta;
                result.lowerEta = lowerEta;
                result.upperEta = upperEta;
                result.totalN = sum(quantities);
                result.completeN = sum(quantities(censoringTypes == 0));
                result.rightCensN = sum(quantities(censoringTypes == 1));
                result.intervalCensN = sum(quantities(censoringTypes == 2));
                result.leftCensN = sum(quantities(censoringTypes == 3));
            catch ME
                result.success = false;
                result.message = ['分析过程出错: ' ME.message];
            end
        end
        
        function col = SafeGetColumn(~, tbl, colName)
            try
                if istable(tbl) && ismember(colName, tbl.Properties.VariableNames)
                    col = tbl.(colName);
                    if iscell(col)
                        col = cellfun(@(x) double(x), col);
                    end
                else
                    col = [];
                end
            catch
                col = [];
            end
            if isempty(col)
                col = 0;
            end
            col = double(col(:));
        end
        
        function str = SafeGetString(~, tbl, colName, rowIdx)
            try
                if istable(tbl) && ismember(colName, tbl.Properties.VariableNames)
                    val = tbl.(colName)(rowIdx);
                    if iscell(val)
                        str = char(val{1});
                    elseif isstring(val)
                        str = char(val);
                    else
                        str = char(val);
                    end
                else
                    str = '';
                end
            catch
                str = '';
            end
        end
        
        function val = SafeGetValue(~, tbl, colName, rowIdx)
            try
                if istable(tbl) && ismember(colName, tbl.Properties.VariableNames)
                    val = tbl.(colName)(rowIdx);
                    if iscell(val)
                        val = val{1};
                    end
                    val = double(val);
                else
                    val = 0;
                end
            catch
                val = 0;
            end
        end
    end
    
    methods (Static)
        %% 查找 JDBC 驱动
        function jdbcPath = FindJdbcDriver()
            jdbcPath = '';
            
            fileNames = {
                'mysql-connector-j-9.1.0.jar',
                'mysql-connector-j-9.0.0.jar',
                'mysql-connector-j-8.4.0.jar',
                'mysql-connector-j-8.0.33.jar',
                'mysql-connector-java.jar'
            };
            
            % 1. 部署环境
            if isdeployed
                ctfDir = ctfroot;
                for i = 1:length(fileNames)
                    testPath = fullfile(ctfDir, fileNames{i});
                    if exist(testPath, 'file')
                        jdbcPath = testPath;
                        return;
                    end
                end
            end
            
            % 2. 当前目录
            for i = 1:length(fileNames)
                if exist(fileNames{i}, 'file')
                    jdbcPath = fullfile(pwd, fileNames{i});
                    return;
                end
            end
            
            % 3. 预设路径
            presetPaths = {
                'C:/Tools/mysql-connector-j-9.1.0',
                'C:/Tools/mysql-connector-j-9.0.0',
                'C:/Tools/mysql-connector-j-8.0.33',
                'C:/jdbc'
            };
            
            for j = 1:length(presetPaths)
                for i = 1:length(fileNames)
                    testPath = fullfile(presetPaths{j}, fileNames{i});
                    if exist(testPath, 'file')
                        jdbcPath = testPath;
                        return;
                    end
                end
            end
        end
    end
    
    methods (Static, Access = private)
        %% Weibull MLE 估计
        function [beta, eta, msg] = WeibullMLEAllTypes(times, lastInspTimes, censoringTypes, quantities)
            completeIdx = (censoringTypes == 0);
            rightCensIdx = (censoringTypes == 1);
            intervalIdx = (censoringTypes == 2);
            leftCensIdx = (censoringTypes == 3);
            
            completeTimes = times(completeIdx);
            completeQty = quantities(completeIdx);
            rightCensTimes = times(rightCensIdx);
            rightCensQty = quantities(rightCensIdx);
            intervalUpper = times(intervalIdx);
            intervalLower = lastInspTimes(intervalIdx);
            intervalQty = quantities(intervalIdx);
            leftCensTimes = times(leftCensIdx);
            leftCensQty = quantities(leftCensIdx);
            
            effectiveFailures = sum(completeQty) + sum(intervalQty) + sum(leftCensQty);
            
            if effectiveFailures < 2
                beta = NaN; eta = NaN; msg = '有效失效数据不足';
                return;
            end
            
            allFailTimes = [completeTimes; (intervalUpper + intervalLower)/2; leftCensTimes];
            allFailTimes = allFailTimes(allFailTimes > 0 & isfinite(allFailTimes));
            
            if length(allFailTimes) < 2
                beta = NaN; eta = NaN; msg = '无有效失效数据';
                return;
            end
            
            logFailTimes = log(allFailTimes);
            meanLog = mean(logFailTimes);
            stdLog = std(logFailTimes);
            if stdLog <= 0.01, stdLog = 0.5; end
            
            beta0 = min(max(1.0/stdLog, 0.5), 4.0);
            eta0 = exp(meanLog);
            
            negLogLike = @(params) WeibullAnalysisLib.NegLogLikelihoodAllTypes(params, ...
                completeTimes, completeQty, rightCensTimes, rightCensQty, ...
                intervalLower, intervalUpper, intervalQty, leftCensTimes, leftCensQty);
            
            bestBeta = NaN; bestEta = NaN; bestNLL = Inf;
            options = optimset('Display', 'off', 'MaxIter', 1000, 'TolFun', 1e-8);
            
            initPoints = [beta0, eta0; beta0*1.5, eta0*1.1; beta0*0.7, eta0*0.9; 1.0, eta0; 2.0, eta0];
            
            for i = 1:size(initPoints, 1)
                try
                    [params, nll] = fminsearch(negLogLike, initPoints(i,:), options);
                    if nll < bestNLL && params(1) > 0.1 && params(1) < 20 && params(2) > 0
                        bestBeta = params(1); bestEta = params(2); bestNLL = nll;
                    end
                catch
                end
            end
            
            if isnan(bestBeta)
                beta = NaN; eta = NaN; msg = '优化失败';
            else
                beta = bestBeta; eta = bestEta; msg = 'OK';
            end
        end
        
        %% 负对数似然函数
        function nll = NegLogLikelihoodAllTypes(params, completeTimes, completeQty, ...
                rightCensTimes, rightCensQty, intervalLower, intervalUpper, intervalQty, ...
                leftCensTimes, leftCensQty)
            
            b = params(1); e = params(2);
            if b <= 0.1 || b > 20 || e <= 0
                nll = 1e10; return;
            end
            
            ll = 0;
            
            if ~isempty(completeTimes) && any(completeTimes > 0)
                idx = completeTimes > 0;
                t = completeTimes(idx); q = completeQty(idx);
                ll = ll + sum(q .* (log(b) - b*log(e) + (b-1)*log(t) - (t/e).^b));
            end
            
            if ~isempty(rightCensTimes) && any(rightCensTimes > 0)
                idx = rightCensTimes > 0;
                t = rightCensTimes(idx); q = rightCensQty(idx);
                ll = ll + sum(q .* (-(t/e).^b));
            end
            
            if ~isempty(intervalLower) && any(intervalUpper > 0)
                idx = (intervalUpper > 0) & (intervalLower >= 0) & (intervalUpper > intervalLower);
                L = intervalLower(idx); U = intervalUpper(idx); q = intervalQty(idx);
                if ~isempty(L)
                    prob = exp(-(L/e).^b) - exp(-(U/e).^b);
                    prob(prob <= 0) = 1e-10;
                    ll = ll + sum(q .* log(prob));
                end
            end
            
            if ~isempty(leftCensTimes) && any(leftCensTimes > 0)
                idx = leftCensTimes > 0;
                t = leftCensTimes(idx); q = leftCensQty(idx);
                F = 1 - exp(-(t/e).^b);
                F(F <= 0) = 1e-10; F(F >= 1) = 1 - 1e-10;
                ll = ll + sum(q .* log(F));
            end
            
            nll = -ll;
            if ~isfinite(nll), nll = 1e10; end
        end
        
        %% 计算 R²
        function r2 = CalcR2AllTypes(times, lastInspTimes, censoringTypes)
            completeIdx = (censoringTypes == 0);
            intervalIdx = (censoringTypes == 2);
            
            completeTimes = times(completeIdx);
            if any(intervalIdx)
                intervalMid = (times(intervalIdx) + lastInspTimes(intervalIdx)) / 2;
            else
                intervalMid = [];
            end
            
            allFailTimes = sort([completeTimes; intervalMid]);
            allFailTimes = allFailTimes(allFailTimes > 0);
            
            n = length(allFailTimes);
            if n < 2, r2 = 0; return; end
            
            ranks = (1:n)';
            medianRanks = (ranks - 0.3) / (n + 0.4);
            x = log(allFailTimes);
            y = log(-log(1 - medianRanks));
            R = corrcoef(x, y);
            r2 = R(1,2)^2;
            if isnan(r2), r2 = 0; end
        end
        
        %% 计算置信区间
        function [lowerBeta, upperBeta, lowerEta, upperEta] = CalcConfidenceIntervals(censoringTypes, quantities, beta, eta, confLevel)
            if nargin < 5, confLevel = 0.95; end
            
            effectiveN = sum(quantities(censoringTypes == 0)) + sum(quantities(censoringTypes == 2)) + sum(quantities(censoringTypes == 3));
            
            if effectiveN < 3
                lowerBeta = NaN; upperBeta = NaN; lowerEta = NaN; upperEta = NaN;
                return;
            end
            
            seBeta = sqrt(1.109 * beta^2 / effectiveN);
            seEta = sqrt(0.608 * eta^2 / effectiveN);
            zValue = sqrt(2) * erfinv(confLevel);
            
            lowerBeta = max(0.1, beta - zValue * seBeta);
            upperBeta = beta + zValue * seBeta;
            lowerEta = max(1, eta - zValue * seEta);
            upperEta = eta + zValue * seEta;
        end
        
        %% 计算寿命指标
        function [mttf, median_life, b10, b50, b90] = CalcLifeMetrics(beta, eta)
            mttf = eta * gamma(1 + 1/beta);
            median_life = eta * (log(2))^(1/beta);
            b10 = eta * (log(10/9))^(1/beta);
            b50 = median_life;
            b90 = eta * (log(10))^(1/beta);
        end
    end
end
