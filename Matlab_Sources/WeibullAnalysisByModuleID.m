function result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath)
%WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
%   result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
%   result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath)
%       使用指定的数据库连接参数
%
%   输入参数:
%       moduleID - 模组ID (数字或字符串)
%       dbHost - 数据库服务器地址 (可选，默认'localhost')
%       dbPort - 端口号 (可选，默认3306)
%       dbName - 数据库名称 (可选，默认'SysHardTestDB')
%       dbUser - 用户名 (可选，默认'root')
%       dbPassword - 密码 (可选)
%       jdbcPath - JDBC驱动路径 (可选)
%
%   输出参数:
%       result - 结构体，包含以下字段:
%           success - 是否成功 (logical)
%           message - 状态消息 (string)
%           moduleID - 模组ID (double)
%           moduleCode - 模组代码 (string)
%           moduleName - 模组名称 (string)
%           beta - 形状参数 (double)
%           eta - 尺度参数 (double)
%           mttf - 平均失效时间 (double)
%           median - 中位寿命 (double)
%           b10, b50, b90 - B寿命 (double)
%           r2 - R²拟合度 (double)
%           lowerBeta, upperBeta - beta置信区间 (double)
%           lowerEta, upperEta - eta置信区间 (double)
%           totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)

    % 创建分析对象
    lib = WeibullAnalysisLib();
    
    % 设置连接参数
    if nargin >= 2 && ~isempty(dbHost)
        if nargin < 3, dbPort = []; end
        if nargin < 4, dbName = []; end
        if nargin < 5, dbUser = []; end
        if nargin < 6, dbPassword = []; end
        if nargin < 7, jdbcPath = []; end
        lib.SetConnectionParams(dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath);
    end
    
    % 执行分析
    result = lib.WeibullAnalysisByModuleID(moduleID);
    
    % 断开连接
    lib.Disconnect();
end
