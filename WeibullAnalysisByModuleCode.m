function result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath)
%WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
%   result = WeibullAnalysisByModuleCode(moduleCode) 对指定模组代码进行分析
%   result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath)
%       使用指定的数据库连接参数
%
%   输入参数:
%       moduleCode - 模组代码 (字符串)
%       dbHost - 数据库服务器地址 (可选)
%       dbPort - 端口号 (可选)
%       dbName - 数据库名称 (可选)
%       dbUser - 用户名 (可选)
%       dbPassword - 密码 (可选)
%       jdbcPath - JDBC驱动路径 (可选)
%
%   输出参数:
%       result - 结构体，包含分析结果

    lib = WeibullAnalysisLib();
    
    if nargin >= 2 && ~isempty(dbHost)
        if nargin < 3, dbPort = []; end
        if nargin < 4, dbName = []; end
        if nargin < 5, dbUser = []; end
        if nargin < 6, dbPassword = []; end
        if nargin < 7, jdbcPath = []; end
        lib.SetConnectionParams(dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath);
    end
    
    result = lib.WeibullAnalysisByModuleCode(moduleCode);
    lib.Disconnect();
end
