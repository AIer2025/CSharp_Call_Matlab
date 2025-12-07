function results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath)
%WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
%   results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
%   results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath)
%       使用指定的数据库连接参数
%
%   输入参数:
%       dbHost - 数据库服务器地址 (可选，默认'localhost')
%       dbPort - 端口号 (可选，默认3306)
%       dbName - 数据库名称 (可选，默认'SysHardTestDB')
%       dbUser - 用户名 (可选，默认'root')
%       dbPassword - 密码 (可选)
%       jdbcPath - JDBC驱动路径 (可选)
%
%   输出参数:
%       results - 结构体数组，每个元素包含一个模组的分析结果

    lib = WeibullAnalysisLib();
    
    if nargin >= 1 && ~isempty(dbHost)
        if nargin < 2, dbPort = []; end
        if nargin < 3, dbName = []; end
        if nargin < 4, dbUser = []; end
        if nargin < 5, dbPassword = []; end
        if nargin < 6, jdbcPath = []; end
        lib.SetConnectionParams(dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath);
    end
    
    results = lib.WeibullAnalysisAll();
    lib.Disconnect();
end
