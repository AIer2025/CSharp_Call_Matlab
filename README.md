# MATLAB Weibull 分析库 - 编译为 .NET DLL 指南

## 目录
1. [概述](#概述)
2. [环境要求](#环境要求)
3. [MATLAB 端准备](#matlab-端准备)
4. [编译方法](#编译方法)
5. [C# 端配置](#c-端配置)
6. [API 说明](#api-说明)
7. [常见问题](#常见问题)

---

## 概述

本项目将 MATLAB Weibull 分析程序编译为 .NET DLL，供 C# (.NET 8.0) 调用。

### 功能特性
- 支持四种删失数据类型（完全数据、右删失、区间删失、左删失）
- 使用 MLE（最大似然估计）方法
- 自动计算置信区间
- 支持数据库连接和直接数据输入两种模式

### 文件结构
```
WeibullAnalysis/
├── WeibullAnalysisLib.m          # 主类库（核心算法）
├── WeibullAnalysisByModuleID.m   # 按模组ID分析接口
├── WeibullAnalysisByModuleCode.m # 按模组代码分析接口
├── WeibullAnalysisFromData.m     # 直接数据分析接口
├── WeibullAnalysisAll.m          # 批量分析接口
├── CompileToDLL.m                # 编译脚本
└── CSharpExample/                # C# 示例项目
    ├── WeibullAnalysisDemo.csproj
    ├── Program.cs
    ├── WeibullAnalysisWrapper.cs
    └── SimplifiedDemo.cs
```

---

## 环境要求

### MATLAB 端
- **MATLAB R2025b** (或更高版本)
- **MATLAB Compiler** (许可证)
- **MATLAB Compiler SDK** (许可证，用于生成 .NET 程序集)
- **Database Toolbox** (如果需要数据库功能)

### 目标机器 (运行 C# 程序)
- **MATLAB Runtime R2025b** (免费，从 MathWorks 官网下载)
- **.NET 8.0 SDK/Runtime**
- **Windows 10/11 x64**

### 检查工具箱
在 MATLAB 中运行：
```matlab
license('test', 'Compiler')              % 应返回 1
license('test', 'MATLAB_Builder_for_dotNET')  % 应返回 1
```

---

## MATLAB 端准备

### 1. 复制文件
将以下文件放入同一目录：
- `WeibullAnalysisLib.m`
- `WeibullAnalysisByModuleID.m`
- `WeibullAnalysisByModuleCode.m`
- `WeibullAnalysisFromData.m`
- `WeibullAnalysisAll.m`

### 2. 测试函数
```matlab
% 测试直接数据分析
times = [100, 200, 300, 400, 500, 600, 700, 800];
censor = [0, 0, 0, 0, 0, 1, 0, 0];  % 第6个是右删失
result = WeibullAnalysisFromData(times, censor);
disp(result);

% 测试数据库分析 (需要数据库配置)
% result = WeibullAnalysisByModuleID(1);
```

---

## 编译方法

### 方法一：使用 Library Compiler App（推荐）

1. **启动 Library Compiler**
   ```matlab
   libraryCompiler
   ```

2. **配置项目**
   - 类型：选择 **".NET Assembly"**
   - 程序集名称：`WeibullAnalysis`
   - 类名：`WeibullAnalyzer`

3. **添加导出函数**
   点击 "+" 添加以下文件：
   - `WeibullAnalysisByModuleID.m`
   - `WeibullAnalysisByModuleCode.m`
   - `WeibullAnalysisFromData.m`
   - `WeibullAnalysisAll.m`

4. **添加依赖文件**
   在 "Files required for your library to run" 中添加：
   - `WeibullAnalysisLib.m`

5. **设置选项**
   - .NET Framework: 选择 4.0 或更高 (兼容 .NET 8.0)
   - 平台: 64-bit

6. **编译**
   点击 **"Package"** 按钮，等待编译完成。

### 方法二：使用命令行

```matlab
% 切换到文件目录
cd('C:\path\to\WeibullAnalysis');

% 创建输出目录
if ~exist('output', 'dir'), mkdir('output'); end

% 执行编译
mcc -W 'dotnet:WeibullAnalysis,WeibullAnalyzer,4.0' -T link:lib ...
    -d './output' ...
    WeibullAnalysisByModuleID.m ...
    WeibullAnalysisByModuleCode.m ...
    WeibullAnalysisFromData.m ...
    WeibullAnalysisAll.m ...
    -a WeibullAnalysisLib.m ...
    -v
```

### 编译输出
编译成功后，`output` 文件夹包含：
```
output/
├── WeibullAnalysis.dll              # 主程序集 ★
├── WeibullAnalysisNative.dll        # 本地代码
├── for_redistribution/              # 可分发包
│   └── MyAppInstaller_web.exe
├── for_redistribution_files_only/   # 仅文件分发
│   ├── WeibullAnalysis.dll
│   └── ...
└── for_testing/                     # 测试用
```

---

## C# 端配置

### 1. 安装 MATLAB Runtime
从 MathWorks 官网下载并安装 MATLAB Runtime R2025b：
https://www.mathworks.com/products/compiler/matlab-runtime.html

### 2. 创建 .NET 8.0 项目
```bash
dotnet new console -n WeibullAnalysisDemo -f net8.0
cd WeibullAnalysisDemo
```

### 3. 添加 DLL 引用
编辑 `.csproj` 文件：
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <PlatformTarget>x64</PlatformTarget>
  </PropertyGroup>

  <ItemGroup>
    <!-- MATLAB 生成的 DLL -->
    <Reference Include="WeibullAnalysis">
      <HintPath>.\libs\WeibullAnalysis.dll</HintPath>
    </Reference>
    
    <!-- MATLAB Runtime 的 MWArray.dll -->
    <Reference Include="MWArray">
      <HintPath>C:\Program Files\MATLAB\MATLAB Runtime\R2025b\toolbox\dotnetbuilder\bin\win64\v4.0\MWArray.dll</HintPath>
    </Reference>
  </ItemGroup>
</Project>
```

### 4. 复制 DLL 文件
创建 `libs` 文件夹，复制 `WeibullAnalysis.dll`。

---

## API 说明

### WeibullAnalysisByModuleID
```csharp
// 按模组ID分析
MWStructArray result = analyzer.WeibullAnalysisByModuleID(
    new MWNumericArray(moduleID),           // 必需：模组ID
    new MWCharArray(dbHost),                // 可选：数据库主机
    new MWNumericArray(dbPort),             // 可选：端口
    new MWCharArray(dbName),                // 可选：数据库名
    new MWCharArray(dbUser),                // 可选：用户名
    new MWCharArray(dbPassword),            // 可选：密码
    new MWCharArray(jdbcPath)               // 可选：JDBC驱动路径
);
```

### WeibullAnalysisByModuleCode
```csharp
// 按模组代码分析
MWStructArray result = analyzer.WeibullAnalysisByModuleCode(
    new MWCharArray(moduleCode),            // 必需：模组代码
    // ... 其他可选数据库参数
);
```

### WeibullAnalysisFromData
```csharp
// 直接数据分析（不需要数据库）
double[] times = {100, 200, 300, 400, 500};
double[] censorTypes = {0, 0, 1, 0, 0};     // 0=完全, 1=右删失, 2=区间, 3=左删失
double[] quantities = {1, 1, 1, 1, 1};
double[] lastInspTimes = {0, 0, 0, 0, 0};

MWStructArray result = analyzer.WeibullAnalysisFromData(
    new MWNumericArray(times),              // 必需：失效时间
    new MWNumericArray(censorTypes),        // 可选：删失类型
    new MWNumericArray(quantities),         // 可选：数量
    new MWNumericArray(lastInspTimes),      // 可选：上次检查时间
    new MWCharArray(moduleCode),            // 可选：模组代码
    new MWCharArray(moduleName)             // 可选：模组名称
);
```

### 返回结果字段
| 字段名 | 类型 | 说明 |
|--------|------|------|
| success | bool | 分析是否成功 |
| message | string | 状态消息 |
| moduleID | int | 模组ID |
| moduleCode | string | 模组代码 |
| moduleName | string | 模组名称 |
| beta | double | 形状参数 β |
| eta | double | 尺度参数 η |
| mttf | double | 平均失效时间 |
| median | double | 中位寿命 |
| b10, b50, b90 | double | B寿命 |
| r2 | double | R² 拟合度 |
| lowerBeta, upperBeta | double | β 置信区间 |
| lowerEta, upperEta | double | η 置信区间 |
| totalN | int | 总样本数 |
| completeN | int | 完全数据数 |
| rightCensN | int | 右删失数 |
| intervalCensN | int | 区间删失数 |
| leftCensN | int | 左删失数 |

---

## 常见问题

### Q1: 编译时提示缺少工具箱
**A:** 确保安装了 MATLAB Compiler 和 MATLAB Compiler SDK，并且许可证有效。

### Q2: C# 运行时提示找不到 DLL
**A:** 检查以下几点：
1. MATLAB Runtime 是否已安装
2. 环境变量 PATH 是否包含 MATLAB Runtime 路径
3. DLL 路径是否正确
4. 平台是否匹配 (x64)

### Q3: MWArray.dll 在哪里？
**A:** 位于 MATLAB Runtime 安装目录：
```
C:\Program Files\MATLAB\MATLAB Runtime\R2025b\toolbox\dotnetbuilder\bin\win64\v4.0\MWArray.dll
```

### Q4: .NET 8.0 兼容性问题
**A:** MATLAB Compiler SDK 生成的 .NET 4.0 程序集可以被 .NET 8.0 加载（向下兼容）。如果遇到问题，可以尝试：
1. 在 `app.config` 中添加运行时绑定重定向
2. 使用 `<SupportedRuntime>` 配置

### Q5: 数据库连接失败
**A:** 检查：
1. MySQL JDBC 驱动是否正确配置
2. 数据库服务是否运行
3. 防火墙是否允许连接
4. 用户名密码是否正确

---

## 技术支持

如有问题，请检查：
1. MATLAB 命令窗口的错误信息
2. C# 的异常堆栈
3. Windows 事件查看器

---

*文档版本: 1.0.0*
*最后更新: 2025-12-04*
