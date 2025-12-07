classdef WeibullAnalysisLib
    %% ================================================
    %  WeibullAnalysisLib - Weibull分析类库
    %  用于编译为 .NET DLL 供 C# 调用
    %  基于 Weibull实验分析系统 V4.8 改造
    %  版本: 1.0.0
    %  更新日期: 2025-12-04
    %  ================================================
    
    properties (Constant)
        VERSION = '1.0.0'
        DEFAULT_CONFIDENCE_LEVEL = 0.95
    end
    
    properties (Access = private)
        conn            % 数据库连接
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
            % 默认数据库配置
            obj.dbHost = 'localhost';
            obj.dbPort = 3306;
            obj.dbName = 'SysHardTestDB';
            obj.dbUser = 'root';
            obj.dbPassword = '*hml2023PLTKIB*';
            obj.jdbcDriverPath = 'C:/Tools/mysql-connector-j-9.4.0/mysql-connector-j-9.4.0.jar';
            obj.conn = [];
            obj.connSuccess = false;
        end
        
        %% 设置数据库连接参数
        function SetConnectionParams(obj, host, port, database, user, password, jdbcPath)
            % 设置数据库连接参数
            % 参数:
            %   host - 数据库服务器地址
            %   port - 端口号
            %   database - 数据库名称
            %   user - 用户名
            %   password - 密码
            %   jdbcPath - JDBC驱动路径 (可选)
            
            if nargin >= 2 && ~isempty(host)
                obj.dbHost = host;
            end
            if nargin >= 3 && ~isempty(port)
                obj.dbPort = port;
            end
            if nargin >= 4 && ~isempty(database)
                obj.dbName = database;
            end
            if nargin >= 5 && ~isempty(user)
                obj.dbUser = user;
            end
            if nargin >= 6 && ~isempty(password)
                obj.dbPassword = password;
            end
            if nargin >= 7 && ~isempty(jdbcPath)
                obj.jdbcDriverPath = jdbcPath;
            end
        end
        
        %% 连接数据库
        function [success, message] = Connect(obj)
            % 连接数据库
            % 返回:
            %   success - 是否成功
            %   message - 状态消息
            
            success = false;
            message = '';
            
            warning('off', 'all');
            
            % 添加JDBC驱动
            if exist(obj.jdbcDriverPath, 'file')
                javaaddpath(obj.jdbcDriverPath);
            end
            
            % 尝试JDBC连接
            try
                jdbcURL = sprintf('jdbc:mysql://%s:%d/%s?useSSL=false&allowPublicKeyRetrieval=true&serverTimezone=UTC&characterEncoding=UTF-8', ...
                    obj.dbHost, obj.dbPort, obj.dbName);
                obj.conn = database('', obj.dbUser, obj.dbPassword, 'com.mysql.cj.jdbc.Driver', jdbcURL);
                if isopen(obj.conn)
                    obj.connSuccess = true;
                    success = true;
                    message = 'JDBC连接成功';
                    return;
                end
            catch ME
                message = ME.message;
            end
            
            % 尝试Database Toolbox连接
            try
                obj.conn = database(obj.dbName, obj.dbUser, obj.dbPassword, 'Vendor', 'MySQL', 'Server', obj.dbHost, 'PortNumber', obj.dbPort);
                if isopen(obj.conn)
                    obj.connSuccess = true;
                    success = true;
                    message = 'Database Toolbox连接成功';
                    return;
                end
            catch ME
                message = ['连接失败: ' ME.message];
            end
            
            obj.connSuccess = false;
        end
        
        %% 断开数据库连接
        function Disconnect(obj)
            if ~isempty(obj.conn) && isopen(obj.conn)
                close(obj.conn);
            end
            obj.connSuccess = false;
        end
        
        %% 按模组ID进行Weibull分析 (主要接口函数)
        function result = WeibullAnalysisByModuleID(obj, moduleID)
            % 对指定模组ID进行Weibull分析
            % 参数:
            %   moduleID - 模组ID (数字或字符串)
            % 返回:
            %   result - 结构体，包含分析结果
            
            result = obj.CreateEmptyResult();
            
            % 确保连接
            if ~obj.connSuccess
                [success, ~] = obj.Connect();
                if ~success
                    result.success = false;
                    result.message = '数据库连接失败';
                    return;
                end
            end
            
            % 转换moduleID为数字
            if ischar(moduleID) || isstring(moduleID)
                moduleID = str2double(moduleID);
            end
            
            % 查询数据
            sqlQuery = sprintf(['SELECT m.module_id, m.module_code, m.module_name, td.test_id, ' ...
                'td.failure_time, ' ...
                'COALESCE(td.last_inspection_time, 0) AS last_inspection_time, ' ...
                'COALESCE(td.quantity, 1) AS quantity, ' ...
                'COALESCE(td.censoring_type, td.is_censored) AS censoring_type, ' ...
                'td.is_censored, td.failure_mode ' ...
                'FROM tb_module m INNER JOIN tb_test_data td ON m.module_id = td.module_id ' ...
                'WHERE m.module_id = %d AND td.failure_time IS NOT NULL AND td.failure_time > 0 ' ...
                'ORDER BY td.failure_time'], moduleID);
            
            try
                rawData = fetch(obj.conn, sqlQuery);
                
                if isempty(rawData) || height(rawData) == 0
                    result.success = false;
                    result.message = sprintf('模组ID=%d 没有找到数据', moduleID);
                    return;
                end
                
                % 执行分析
                result = obj.AnalyzeModuleData(rawData);
                
            catch ME
                result.success = false;
                result.message = ['数据查询失败: ' ME.message];
            end
        end
        
        %% 按模组代码进行Weibull分析
        function result = WeibullAnalysisByModuleCode(obj, moduleCode)
            % 对指定模组代码进行Weibull分析
            % 参数:
            %   moduleCode - 模组代码 (字符串)
            % 返回:
            %   result - 结构体，包含分析结果
            
            result = obj.CreateEmptyResult();
            
            % 确保连接
            if ~obj.connSuccess
                [success, ~] = obj.Connect();
                if ~success
                    result.success = false;
                    result.message = '数据库连接失败';
                    return;
                end
            end
            
            % 查询数据
            sqlQuery = sprintf(['SELECT m.module_id, m.module_code, m.module_name, td.test_id, ' ...
                'td.failure_time, ' ...
                'COALESCE(td.last_inspection_time, 0) AS last_inspection_time, ' ...
                'COALESCE(td.quantity, 1) AS quantity, ' ...
                'COALESCE(td.censoring_type, td.is_censored) AS censoring_type, ' ...
                'td.is_censored, td.failure_mode ' ...
                'FROM tb_module m INNER JOIN tb_test_data td ON m.module_id = td.module_id ' ...
                'WHERE m.module_code = ''%s'' AND td.failure_time IS NOT NULL AND td.failure_time > 0 ' ...
                'ORDER BY td.failure_time'], moduleCode);
            
            try
                rawData = fetch(obj.conn, sqlQuery);
                
                if isempty(rawData) || height(rawData) == 0
                    result.success = false;
                    result.message = sprintf('模组代码=%s 没有找到数据', moduleCode);
                    return;
                end
                
                % 执行分析
                result = obj.AnalyzeModuleData(rawData);
                
            catch ME
                result.success = false;
                result.message = ['数据查询失败: ' ME.message];
            end
        end
        
        %% 对所有模组进行批量分析
        function results = WeibullAnalysisAll(obj)
            % 对所有活跃模组进行Weibull分析
            % 返回:
            %   results - 结构体数组，包含所有模组的分析结果
            
            results = [];
            
            % 确保连接
            if ~obj.connSuccess
                [success, ~] = obj.Connect();
                if ~success
                    return;
                end
            end
            
            % 查询所有数据
            sqlQuery = ['SELECT m.module_id, m.module_code, m.module_name, td.test_id, ' ...
                'td.failure_time, ' ...
                'COALESCE(td.last_inspection_time, 0) AS last_inspection_time, ' ...
                'COALESCE(td.quantity, 1) AS quantity, ' ...
                'COALESCE(td.censoring_type, td.is_censored) AS censoring_type, ' ...
                'td.is_censored, td.failure_mode ' ...
                'FROM tb_module m INNER JOIN tb_test_data td ON m.module_id = td.module_id ' ...
                'WHERE m.is_active = 1 AND td.failure_time IS NOT NULL AND td.failure_time > 0 ' ...
                'ORDER BY m.module_id, td.failure_time'];
            
            try
                rawData = fetch(obj.conn, sqlQuery);
                
                if isempty(rawData) || height(rawData) == 0
                    return;
                end
                
                % 获取所有模组ID
                moduleIDs = unique(rawData.module_id);
                nModules = length(moduleIDs);
                
                % 预分配结果数组
                results = repmat(obj.CreateEmptyResult(), nModules, 1);
                
                % 逐个模组分析
                for i = 1:nModules
                    modID = moduleIDs(i);
                    idx = rawData.module_id == modID;
                    moduleData = rawData(idx, :);
                    results(i) = obj.AnalyzeModuleData(moduleData);
                end
                
            catch ME
                % 返回空结果
                results = [];
            end
        end
        
        %% 将分析结果保存到数据库
        function [success, message] = SaveResultToDatabase(obj, result)
            % 将分析结果保存到数据库
            % 参数:
            %   result - 分析结果结构体
            % 返回:
            %   success - 是否成功
            %   message - 状态消息
            
            success = false;
            message = '';
            
            if ~result.success
                message = '分析结果无效，无法保存';
                return;
            end
            
            if ~obj.connSuccess
                message = '数据库未连接';
                return;
            end
            
            analysisNotes = sprintf('DLL接口:完全=%d,右删失=%d,区间=%d,左删失=%d', ...
                result.completeN, result.rightCensN, result.intervalCensN, result.leftCensN);
            
            insertSQL = sprintf(['INSERT INTO tb_weibull_analysis ' ...
                '(module_id, analysis_time, data_count, beta, eta, gamma, ' ...
                'mttf, median_life, b10_life, b50_life, b90_life, r_squared, ' ...
                'confidence_level, lower_beta, upper_beta, lower_eta, upper_eta, ' ...
                'analysis_method, analysis_notes, analyst) ' ...
                'VALUES (%d, NOW(), %d, %.6f, %.4f, 0, ' ...
                '%.4f, %.4f, %.4f, %.4f, %.4f, %.6f, ' ...
                '95.00, %.6f, %.6f, %.4f, %.4f, ' ...
                '''MLE'', ''%s'', ''DLL_Interface'')'], ...
                result.moduleID, result.totalN, result.beta, result.eta, ...
                result.mttf, result.median, result.b10, result.b50, result.b90, result.r2, ...
                result.lowerBeta, result.upperBeta, result.lowerEta, result.upperEta, analysisNotes);
            
            try
                execute(obj.conn, insertSQL);
                success = true;
                message = '保存成功';
            catch ME
                message = ['保存失败: ' ME.message];
            end
        end
        
        %% 直接使用数据数组进行分析（不访问数据库）
        function result = WeibullAnalysisFromData(~, failureTimes, censoringTypes, quantities, lastInspectionTimes, moduleCode, moduleName)
            % 直接使用数据数组进行Weibull分析
            % 参数:
            %   failureTimes - 失效时间数组 (double[])
            %   censoringTypes - 删失类型数组 (double[]) 0=完全, 1=右删失, 2=区间, 3=左删失
            %   quantities - 数量数组 (double[])
            %   lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时使用
            %   moduleCode - 模组代码 (可选)
            %   moduleName - 模组名称 (可选)
            % 返回:
            %   result - 结构体，包含分析结果
            
            result = WeibullAnalysisLib.CreateEmptyResultStatic();
            
            % 参数验证
            if isempty(failureTimes) || length(failureTimes) < 2
                result.success = false;
                result.message = '数据点不足，至少需要2个数据点';
                return;
            end
            
            % 确保是列向量
            failureTimes = failureTimes(:);
            n = length(failureTimes);
            
            % 处理可选参数
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
            
            % 执行MLE估计
            [beta, eta, msg] = WeibullAnalysisLib.WeibullMLEAllTypes(...
                failureTimes, lastInspectionTimes, censoringTypes, quantities);
            
            if isnan(beta)
                result.success = false;
                result.message = msg;
                return;
            end
            
            % 计算各项指标
            [mttf, median_life, b10, b50, b90] = WeibullAnalysisLib.CalcLifeMetrics(beta, eta);
            r2 = WeibullAnalysisLib.CalcR2AllTypes(failureTimes, lastInspectionTimes, censoringTypes);
            [lowerBeta, upperBeta, lowerEta, upperEta] = WeibullAnalysisLib.CalcConfidenceIntervals(...
                censoringTypes, quantities, beta, eta, 0.95);
            
            % 统计数据类型
            completeN = sum((censoringTypes == 0) .* quantities);
            rightCensN = sum((censoringTypes == 1) .* quantities);
            intervalCensN = sum((censoringTypes == 2) .* quantities);
            leftCensN = sum((censoringTypes == 3) .* quantities);
            
            % 填充结果
            result.success = true;
            result.message = 'OK';
            result.moduleID = 0;
            result.moduleCode = string(moduleCode);
            result.moduleName = string(moduleName);
            result.beta = beta;
            result.eta = eta;
            result.mttf = mttf;
            result.median = median_life;
            result.b10 = b10;
            result.b50 = b50;
            result.b90 = b90;
            result.totalN = sum(quantities);
            result.completeN = completeN;
            result.rightCensN = rightCensN;
            result.intervalCensN = intervalCensN;
            result.leftCensN = leftCensN;
            result.r2 = r2;
            result.lowerBeta = lowerBeta;
            result.upperBeta = upperBeta;
            result.lowerEta = lowerEta;
            result.upperEta = upperEta;
        end
        
        %% 获取版本信息
        function ver = GetVersion(~)
            ver = WeibullAnalysisLib.VERSION;
        end
    end
    
    %% 私有方法
    methods (Access = private)
        
        %% 分析单个模组数据
        function result = AnalyzeModuleData(obj, moduleData)
            result = obj.CreateEmptyResult();
            
            moduleID = moduleData.module_id(1);
            moduleCode = string(moduleData.module_code(1));
            moduleName = string(moduleData.module_name(1));
            
            times = moduleData.failure_time;
            n = length(times);
            
            lastInspTimes = zeros(n, 1);
            censoringTypes = zeros(n, 1);
            quantities = ones(n, 1);
            
            for j = 1:n
                lastInspTimes(j) = WeibullAnalysisLib.ConvertToNumeric(moduleData.last_inspection_time(j));
                censoringTypes(j) = WeibullAnalysisLib.ConvertToNumeric(moduleData.censoring_type(j));
                q = WeibullAnalysisLib.ConvertToNumeric(moduleData.quantity(j));
                if q > 0
                    quantities(j) = q;
                end
            end
            
            % 执行MLE估计
            [beta, eta, msg] = WeibullAnalysisLib.WeibullMLEAllTypes(...
                times, lastInspTimes, censoringTypes, quantities);
            
            if isnan(beta)
                result.success = false;
                result.message = msg;
                result.moduleID = moduleID;
                result.moduleCode = moduleCode;
                result.moduleName = moduleName;
                return;
            end
            
            % 计算各项指标
            [mttf, median_life, b10, b50, b90] = WeibullAnalysisLib.CalcLifeMetrics(beta, eta);
            r2 = WeibullAnalysisLib.CalcR2AllTypes(times, lastInspTimes, censoringTypes);
            [lowerBeta, upperBeta, lowerEta, upperEta] = WeibullAnalysisLib.CalcConfidenceIntervals(...
                censoringTypes, quantities, beta, eta, 0.95);
            
            % 统计数据类型
            completeN = sum((censoringTypes == 0) .* quantities);
            rightCensN = sum((censoringTypes == 1) .* quantities);
            intervalCensN = sum((censoringTypes == 2) .* quantities);
            leftCensN = sum((censoringTypes == 3) .* quantities);
            
            % 填充结果
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
            result.totalN = sum(quantities);
            result.completeN = completeN;
            result.rightCensN = rightCensN;
            result.intervalCensN = intervalCensN;
            result.leftCensN = leftCensN;
            result.r2 = r2;
            result.lowerBeta = lowerBeta;
            result.upperBeta = upperBeta;
            result.lowerEta = lowerEta;
            result.upperEta = upperEta;
        end
        
        %% 创建空结果结构体
        function result = CreateEmptyResult(~)
            result = WeibullAnalysisLib.CreateEmptyResultStatic();
        end
    end
    
    %% 静态方法
    methods (Static)
        
        %% 创建空结果结构体（静态版本）
        function result = CreateEmptyResultStatic()
            result = struct(...
                'success', false, ...
                'message', '', ...
                'moduleID', 0, ...
                'moduleCode', '', ...
                'moduleName', '', ...
                'beta', NaN, ...
                'eta', NaN, ...
                'mttf', NaN, ...
                'median', NaN, ...
                'b10', NaN, ...
                'b50', NaN, ...
                'b90', NaN, ...
                'totalN', 0, ...
                'completeN', 0, ...
                'rightCensN', 0, ...
                'intervalCensN', 0, ...
                'leftCensN', 0, ...
                'r2', NaN, ...
                'lowerBeta', NaN, ...
                'upperBeta', NaN, ...
                'lowerEta', NaN, ...
                'upperEta', NaN);
        end
        
        %% 数值转换
        function val = ConvertToNumeric(input)
            if isempty(input) || (isstring(input) && input == "") || (ischar(input) && isempty(input))
                val = 0;
            elseif islogical(input)
                val = double(input);
            elseif isnumeric(input)
                val = double(input);
            elseif ischar(input) || isstring(input)
                numVal = str2double(input);
                if isnan(numVal)
                    val = 0;
                else
                    val = numVal;
                end
            elseif iscell(input)
                val = WeibullAnalysisLib.ConvertToNumeric(input{1});
            else
                val = 0;
            end
            if isnan(val)
                val = 0;
            end
        end
        
        %% Weibull MLE估计（支持所有删失类型）
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
                beta = NaN;
                eta = NaN;
                msg = '有效失效数据不足';
                return;
            end
            
            allFailTimes = [completeTimes; (intervalUpper + intervalLower)/2; leftCensTimes];
            allFailTimes = allFailTimes(allFailTimes > 0 & isfinite(allFailTimes));
            
            if length(allFailTimes) < 2
                beta = NaN;
                eta = NaN;
                msg = '无有效失效数据';
                return;
            end
            
            logFailTimes = log(allFailTimes);
            meanLog = mean(logFailTimes);
            stdLog = std(logFailTimes);
            if stdLog <= 0.01 || ~isfinite(stdLog)
                stdLog = 0.5;
            end
            
            beta0 = min(max(1.0/stdLog, 0.5), 4.0);
            eta0 = exp(meanLog);
            
            negLogLike = @(params) WeibullAnalysisLib.NegLogLikelihoodAllTypes(params, ...
                completeTimes, completeQty, rightCensTimes, rightCensQty, ...
                intervalLower, intervalUpper, intervalQty, leftCensTimes, leftCensQty);
            
            bestBeta = NaN;
            bestEta = NaN;
            bestNLL = Inf;
            options = optimset('Display', 'off', 'MaxIter', 1000, 'TolFun', 1e-8, 'TolX', 1e-8);
            
            initPoints = [beta0, eta0; beta0*1.5, eta0*1.1; beta0*0.7, eta0*0.9; ...
                1.0, eta0; 2.0, eta0; beta0, eta0*1.5; beta0, eta0*0.7];
            
            for i = 1:size(initPoints, 1)
                try
                    [params, nll] = fminsearch(negLogLike, initPoints(i,:), options);
                    if nll < bestNLL && params(1) > 0.1 && params(1) < 20 && params(2) > 0
                        bestBeta = params(1);
                        bestEta = params(2);
                        bestNLL = nll;
                    end
                catch
                end
            end
            
            if isnan(bestBeta)
                beta = NaN;
                eta = NaN;
                msg = '优化失败';
                return;
            end
            
            beta = bestBeta;
            eta = bestEta;
            msg = 'OK';
        end
        
        %% 负对数似然函数
        function nll = NegLogLikelihoodAllTypes(params, completeTimes, completeQty, ...
                rightCensTimes, rightCensQty, intervalLower, intervalUpper, intervalQty, ...
                leftCensTimes, leftCensQty)
            
            b = params(1);
            e = params(2);
            if b <= 0.1 || b > 20 || e <= 0
                nll = 1e10;
                return;
            end
            
            ll = 0;
            
            % 完全数据
            if ~isempty(completeTimes) && any(completeTimes > 0)
                validIdx = completeTimes > 0;
                t = completeTimes(validIdx);
                q = completeQty(validIdx);
                ll = ll + sum(q .* (log(b) - b*log(e) + (b-1)*log(t) - (t/e).^b));
            end
            
            % 右删失
            if ~isempty(rightCensTimes) && any(rightCensTimes > 0)
                validIdx = rightCensTimes > 0;
                t = rightCensTimes(validIdx);
                q = rightCensQty(validIdx);
                ll = ll + sum(q .* (-(t/e).^b));
            end
            
            % 区间删失
            if ~isempty(intervalLower) && any(intervalUpper > 0)
                validIdx = (intervalUpper > 0) & (intervalLower >= 0) & (intervalUpper > intervalLower);
                L = intervalLower(validIdx);
                U = intervalUpper(validIdx);
                q = intervalQty(validIdx);
                if ~isempty(L)
                    S_L = exp(-(L/e).^b);
                    S_U = exp(-(U/e).^b);
                    prob = S_L - S_U;
                    prob(prob <= 0) = 1e-10;
                    ll = ll + sum(q .* log(prob));
                end
            end
            
            % 左删失
            if ~isempty(leftCensTimes) && any(leftCensTimes > 0)
                validIdx = leftCensTimes > 0;
                t = leftCensTimes(validIdx);
                q = leftCensQty(validIdx);
                F = 1 - exp(-(t/e).^b);
                F(F <= 0) = 1e-10;
                F(F >= 1) = 1 - 1e-10;
                ll = ll + sum(q .* log(F));
            end
            
            nll = -ll;
            if ~isfinite(nll)
                nll = 1e10;
            end
        end
        
        %% 计算R²
        function r2 = CalcR2AllTypes(times, lastInspTimes, censoringTypes)
            completeIdx = (censoringTypes == 0);
            intervalIdx = (censoringTypes == 2);
            
            completeTimes = times(completeIdx);
            if any(intervalIdx)
                intervalMid = (times(intervalIdx) + lastInspTimes(intervalIdx)) / 2;
            else
                intervalMid = [];
            end
            
            allFailTimes = [completeTimes; intervalMid];
            allFailTimes = allFailTimes(allFailTimes > 0);
            allFailTimes = sort(allFailTimes);
            
            n = length(allFailTimes);
            if n < 2
                r2 = 0;
                return;
            end
            
            ranks = (1:n)';
            medianRanks = (ranks - 0.3) / (n + 0.4);
            x = log(allFailTimes);
            y = log(-log(1 - medianRanks));
            R = corrcoef(x, y);
            r2 = R(1,2)^2;
            if isnan(r2)
                r2 = 0;
            end
        end
        
        %% 计算置信区间
        function [lowerBeta, upperBeta, lowerEta, upperEta] = CalcConfidenceIntervals(...
                censoringTypes, quantities, beta, eta, confLevel)
            
            if nargin < 5
                confLevel = 0.95;
            end
            
            completeIdx = (censoringTypes == 0);
            intervalIdx = (censoringTypes == 2);
            leftCensIdx = (censoringTypes == 3);
            
            effectiveN = sum(quantities(completeIdx)) + sum(quantities(intervalIdx)) + sum(quantities(leftCensIdx));
            
            if effectiveN < 3
                lowerBeta = NaN;
                upperBeta = NaN;
                lowerEta = NaN;
                upperEta = NaN;
                return;
            end
            
            varBeta = 1.109 * beta^2 / effectiveN;
            varEta = 0.608 * eta^2 / effectiveN;
            seBeta = sqrt(varBeta);
            seEta = sqrt(varEta);
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
