%% CompileToDLL_v1.2.m - 编译 Weibull 分析库（纯 JDBC 版本）
%  版本: 1.2.0
%  重要：此版本使用纯 Java JDBC API，不依赖 Database Toolbox

clc;
fprintf('========================================\n');
fprintf('  Weibull 分析 DLL 编译工具 v1.2.0\n');
fprintf('  (纯 JDBC 版本 - 不依赖 Database Toolbox)\n');
fprintf('========================================\n\n');

%% ========== 配置区 ==========

% JDBC 驱动路径 - 请修改为你的实际路径！
jdbcDriverPath = 'C:/Tools/mysql-connector-j-9.4.0/mysql-connector-j-9.4.0.jar';

% 输出目录
outputDir = './output_v1.2';

%% ========== 检查 JDBC 驱动 ==========
fprintf('【步骤1】检查 JDBC 驱动...\n');

if ~exist(jdbcDriverPath, 'file')
    localJars = dir('*.jar');
    if ~isempty(localJars)
        jdbcDriverPath = fullfile(pwd, localJars(1).name);
        fprintf('  在当前目录找到: %s\n', jdbcDriverPath);
    else
        error('JDBC 驱动未找到！请下载并设置路径');
    end
else
    fprintf('  ✓ JDBC 驱动: %s\n', jdbcDriverPath);
end

%% ========== 重命名文件（如果需要）==========
fprintf('\n【步骤2】准备源文件...\n');

% 检查是否需要重命名
if exist('WeibullAnalysisLib_v1.2.m', 'file') && ~exist('WeibullAnalysisLib.m', 'file')
    copyfile('WeibullAnalysisLib_v1.2.m', 'WeibullAnalysisLib.m');
    fprintf('  已复制 WeibullAnalysisLib_v1.2.m -> WeibullAnalysisLib.m\n');
end

% 检查源文件
sourceFiles = {
    'WeibullAnalysisByModuleID.m'
    'WeibullAnalysisByModuleCode.m'
    'WeibullAnalysisFromData.m'
    'WeibullAnalysisAll.m'
    'WeibullAnalysisLib.m'
};

allExist = true;
for i = 1:length(sourceFiles)
    if exist(sourceFiles{i}, 'file')
        fprintf('  ✓ %s\n', sourceFiles{i});
    else
        fprintf('  ✗ 缺少: %s\n', sourceFiles{i});
        allExist = false;
    end
end

if ~allExist
    error('缺少源文件');
end

%% ========== 创建输出目录 ==========
fprintf('\n【步骤3】准备输出目录...\n');

if ~exist(outputDir, 'dir')
    mkdir(outputDir);
end
fprintf('  输出目录: %s\n', fullfile(pwd, outputDir));

%% ========== 执行编译 ==========
fprintf('\n【步骤4】开始编译...\n');
fprintf('  此版本使用纯 Java JDBC API，不需要 Database Toolbox\n\n');

try
    mccCmd = sprintf([...
        'mcc -W ''dotnet:WeibullAnalysis,WeibullAnalyzer,4.0'' ' ...
        '-T link:lib ' ...
        '-d ''%s'' ' ...
        '-v ' ...
        'WeibullAnalysisByModuleID.m ' ...
        'WeibullAnalysisByModuleCode.m ' ...
        'WeibullAnalysisFromData.m ' ...
        'WeibullAnalysisAll.m ' ...
        '-a WeibullAnalysisLib.m ' ...
        '-a ''%s'''], ...
        outputDir, jdbcDriverPath);
    
    fprintf('执行命令:\n%s\n\n', mccCmd);
    fprintf('编译中，请等待 2-5 分钟...\n\n');
    
    eval(mccCmd);
    
    fprintf('\n========================================\n');
    fprintf('  ✓ 编译成功！\n');
    fprintf('========================================\n\n');
    
catch ME
    fprintf('\n编译失败: %s\n', ME.message);
    return;
end

%% ========== 显示结果 ==========
fprintf('【步骤5】生成的文件:\n\n');

outputFiles = dir(fullfile(outputDir, '*.*'));
for i = 1:length(outputFiles)
    if ~outputFiles(i).isdir
        fprintf('  %s (%.1f KB)\n', outputFiles(i).name, outputFiles(i).bytes/1024);
    end
end

%% ========== 使用说明 ==========
fprintf('\n========================================\n');
fprintf('  下一步\n');
fprintf('========================================\n');
fprintf('1. 复制到 C# 项目:\n');
fprintf('   - %s/WeibullAnalysis.dll\n', outputDir);
fprintf('   - %s/WeibullAnalysisNative.dll\n', outputDir);
fprintf('   - %s/WeibullAnalysis.ctf\n', outputDir);
fprintf('\n');
fprintf('2. 添加 MWArray.dll 引用\n');
fprintf('\n');
fprintf('3. 目标机器安装 MATLAB Runtime R2025b\n');
fprintf('\n');
fprintf('此版本优势:\n');
fprintf('  - 使用纯 Java JDBC API\n');
fprintf('  - 不依赖 Database Toolbox\n');
fprintf('  - 解决 fetch 函数不可用问题\n');
fprintf('\n完成！\n');
