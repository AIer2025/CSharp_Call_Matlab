%% CompileToDLL.m - 编译 Weibull 分析库为 .NET DLL
%  版本: 1.3.1
%  JDBC: mysql-connector-j-9.4.0
%  更新日期: 2025-12-07

clc;
fprintf('========================================\n');
fprintf('  Weibull 分析 DLL 编译工具 v1.3.1\n');
fprintf('  JDBC: mysql-connector-j-9.4.0\n');
fprintf('========================================\n\n');

%% ========== 配置 ==========

% JDBC 驱动路径 (已更新为 9.4.0)
jdbcDriverPath = 'C:/Tools/mysql-connector-j-9.4.0/mysql-connector-j-9.4.0.jar';

% 输出目录
outputDir = './output';

%% ========== 检查 JDBC ==========
fprintf('【步骤1】检查 JDBC 驱动...\n');

if ~exist(jdbcDriverPath, 'file')
    % 尝试查找本地 jar
    localJars = dir('mysql-connector-j-*.jar');
    if ~isempty(localJars)
        jdbcDriverPath = fullfile(pwd, localJars(1).name);
    else
        error(['JDBC 驱动未找到: %s\n' ...
               '请从 https://dev.mysql.com/downloads/connector/j/ 下载'], jdbcDriverPath);
    end
end
fprintf('  ✓ %s\n', jdbcDriverPath);

%% ========== 检查源文件 ==========
fprintf('\n【步骤2】检查源文件...\n');

sourceFiles = {'WeibullAnalysisByModuleID.m', 'WeibullAnalysisByModuleCode.m', ...
               'WeibullAnalysisFromData.m', 'WeibullAnalysisAll.m', 'WeibullAnalysisLib.m'};

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
    error('缺少源文件，请确保所有 .m 文件在当前目录');
end

%% ========== 创建输出目录 ==========
fprintf('\n【步骤3】准备输出目录...\n');

if ~exist(outputDir, 'dir')
    mkdir(outputDir);
end
fprintf('  输出目录: %s\n', fullfile(pwd, outputDir));

%% ========== 执行编译 ==========
fprintf('\n【步骤4】开始编译（约2-5分钟）...\n\n');

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
    fprintf('编译中...\n\n');
    
    eval(mccCmd);
    
    fprintf('\n========================================\n');
    fprintf('  ✓ 编译成功！\n');
    fprintf('========================================\n\n');
    
catch ME
    fprintf('\n编译失败: %s\n', ME.message);
    return;
end

%% ========== 显示输出文件 ==========
fprintf('【步骤5】生成的文件:\n\n');

outputFiles = dir(fullfile(outputDir, '*.*'));
for i = 1:length(outputFiles)
    if ~outputFiles(i).isdir
        fprintf('  %s (%.1f KB)\n', outputFiles(i).name, outputFiles(i).bytes/1024);
    end
end

%% ========== 部署说明 ==========
fprintf('\n========================================\n');
fprintf('  【重要】部署步骤\n');
fprintf('========================================\n\n');

fprintf('1. 复制以下文件到 C# 项目 bin 目录:\n');
fprintf('   - %s/WeibullAnalysis.dll\n', outputDir);
fprintf('   - %s/WeibullAnalysisNative.dll\n', outputDir);
fprintf('   - %s/WeibullAnalysis.ctf\n', outputDir);
fprintf('\n');

fprintf('2. 【关键】复制 JDBC 驱动到 exe 同目录:\n');
fprintf('   将 mysql-connector-j-9.4.0.jar 复制到:\n');
fprintf('   bin/Debug/net8.0/mysql-connector-j-9.4.0.jar\n');
fprintf('\n');

fprintf('3. 添加 MWArray.dll 引用:\n');
fprintf('   C:\\Program Files\\MATLAB\\MATLAB Runtime\\R2025b\\toolbox\\dotnetbuilder\\bin\\win64\\v4.0\\MWArray.dll\n');
fprintf('\n');

fprintf('4. 目标机器安装 MATLAB Runtime R2025b\n');
fprintf('\n');

fprintf('部署目录结构:\n');
fprintf('  bin/Debug/net8.0/\n');
fprintf('  ├── YourApp.exe\n');
fprintf('  ├── WeibullAnalysis.dll\n');
fprintf('  ├── WeibullAnalysisNative.dll\n');
fprintf('  ├── WeibullAnalysis.ctf\n');
fprintf('  └── mysql-connector-j-9.4.0.jar  ← 必须！\n');
fprintf('\n');
fprintf('完成！\n');
