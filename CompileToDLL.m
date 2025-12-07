%% ================================================
%  MATLAB 编译脚本 - 生成 .NET DLL
%  将 Weibull 分析函数编译为 C# 可调用的 DLL
%  ================================================

%% 检查工具箱
fprintf('检查所需工具箱...\n');

% 检查 MATLAB Compiler
if ~license('test', 'Compiler')
    error('需要 MATLAB Compiler 工具箱');
end

% 检查 MATLAB Compiler SDK
if ~license('test', 'MATLAB_Builder_for_dotNET')
    error('需要 MATLAB Compiler SDK (.NET) 工具箱');
end

fprintf('  ✓ MATLAB Compiler 已安装\n');
fprintf('  ✓ MATLAB Compiler SDK 已安装\n\n');

%% 设置编译选项
fprintf('配置编译选项...\n');

% 输出目录
outputDir = fullfile(pwd, 'output');
if ~exist(outputDir, 'dir')
    mkdir(outputDir);
end

% 程序集名称
assemblyName = 'WeibullAnalysis';
className = 'WeibullAnalyzer';

% .NET Framework 版本 (对于 .NET 8.0，需要选择 .NET Standard 或 .NET Core 兼容)
% MATLAB R2025b 支持 .NET 6.0+
dotNetVersion = '6.0';  % .NET 8.0 向下兼容 .NET 6.0

fprintf('  程序集名称: %s\n', assemblyName);
fprintf('  类名: %s\n', className);
fprintf('  输出目录: %s\n', outputDir);
fprintf('  .NET 版本: %s\n\n', dotNetVersion);

%% 方法1: 使用 Library Compiler App (推荐)
fprintf('=== 方法1: 使用 Library Compiler App ===\n');
fprintf('在 MATLAB 命令窗口运行:\n');
fprintf('  >> libraryCompiler\n\n');
fprintf('然后:\n');
fprintf('  1. 选择 ".NET Assembly"\n');
fprintf('  2. 设置程序集名称为: %s\n', assemblyName);
fprintf('  3. 设置类名为: %s\n', className);
fprintf('  4. 添加以下导出函数:\n');
fprintf('     - WeibullAnalysisByModuleID.m\n');
fprintf('     - WeibullAnalysisByModuleCode.m\n');
fprintf('     - WeibullAnalysisFromData.m\n');
fprintf('     - WeibullAnalysisAll.m\n');
fprintf('  5. 添加依赖文件: WeibullAnalysisLib.m\n');
fprintf('  6. 点击 "Package" 进行编译\n\n');

%% 方法2: 使用命令行编译
fprintf('=== 方法2: 使用命令行编译 ===\n');

% 构建 mcc 命令
mccCommand = sprintf(['mcc -W ''dotnet:%s,%s,4.0'' -T link:lib ' ...
    '-d ''%s'' ' ...
    'WeibullAnalysisByModuleID.m ' ...
    'WeibullAnalysisByModuleCode.m ' ...
    'WeibullAnalysisFromData.m ' ...
    'WeibullAnalysisAll.m ' ...
    '-a WeibullAnalysisLib.m ' ...
    '-v'], ...
    assemblyName, className, outputDir);

fprintf('编译命令:\n');
fprintf('  %s\n\n', mccCommand);

%% 询问是否执行编译
response = input('是否立即执行命令行编译? (y/n): ', 's');

if strcmpi(response, 'y')
    fprintf('\n开始编译...\n');
    fprintf('这可能需要几分钟，请耐心等待...\n\n');
    
    try
        eval(mccCommand);
        fprintf('\n编译成功！\n');
        fprintf('输出文件位于: %s\n', outputDir);
        
        % 列出生成的文件
        fprintf('\n生成的文件:\n');
        files = dir(fullfile(outputDir, '*.*'));
        for i = 1:length(files)
            if ~files(i).isdir
                fprintf('  %s\n', files(i).name);
            end
        end
        
    catch ME
        fprintf('编译失败: %s\n', ME.message);
    end
else
    fprintf('\n已取消编译。\n');
    fprintf('您可以稍后手动运行上述命令或使用 Library Compiler App。\n');
end

%% 编译后说明
fprintf('\n=== 编译后配置说明 ===\n');
fprintf('1. 将生成的 DLL 文件复制到 C# 项目中\n');
fprintf('2. 在目标机器上安装 MATLAB Runtime (R2025b)\n');
fprintf('3. 在 C# 项目中添加以下引用:\n');
fprintf('   - %s.dll (生成的程序集)\n', assemblyName);
fprintf('   - MWArray.dll (MATLAB 数据类型)\n');
fprintf('4. MWArray.dll 位于:\n');
fprintf('   %%MATLAB_RUNTIME%%\\toolbox\\dotnetbuilder\\bin\\win64\\v4.0\\\n\n');
