/*
 * WeibullAnalysisDemo - C# 调用 MATLAB Weibull 分析 DLL 示例
 * 
 * 使用前准备:
 * 1. 安装 MATLAB Runtime R2025b
 * 2. 将 WeibullAnalysis.dll 放入 libs 文件夹
 * 3. 配置 MWArray.dll 引用路径
 * 
 * .NET 8.0 项目
 */

using System;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using WeibullAnalysisNative; // MATLAB 生成的命名空间

namespace WeibullAnalysisDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔══════════════════════════════════════════════════╗");
            Console.WriteLine("║     Weibull 分析系统 - C# 调用示例              ║");
            Console.WriteLine("║     基于 MATLAB R2025b 编译的 DLL               ║");
            Console.WriteLine("╚══════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                // 创建分析器实例
                using var analyzer = new WeibullAnalyzer();

                // 示例1: 按模组ID分析
                Console.WriteLine("=== 示例1: 按模组ID分析 ===");
                AnalyzeByModuleID(analyzer, 1);

                Console.WriteLine();

                // 示例2: 按模组代码分析
                Console.WriteLine("=== 示例2: 按模组代码分析 ===");
                AnalyzeByModuleCode(analyzer, "MOD001");

                Console.WriteLine();

                // 示例3: 直接使用数据数组分析
                Console.WriteLine("=== 示例3: 直接数据分析 ===");
                AnalyzeFromData(analyzer);

                Console.WriteLine();

                // 示例4: 批量分析所有模组
                Console.WriteLine("=== 示例4: 批量分析 ===");
                AnalyzeAll(analyzer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
                Console.WriteLine($"详情: {ex.StackTrace}");
            }

            Console.WriteLine();
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }

        /// <summary>
        /// 按模组ID进行分析
        /// </summary>
        static void AnalyzeByModuleID(WeibullAnalyzer analyzer, int moduleID)
        {
            Console.WriteLine($"分析模组ID: {moduleID}");

            // 调用 MATLAB 函数
            MWStructArray result = (MWStructArray)analyzer.WeibullAnalysisByModuleID(
                new MWNumericArray(moduleID)
            );

            // 解析结果
            PrintWeibullResult(result);
        }

        /// <summary>
        /// 按模组代码进行分析
        /// </summary>
        static void AnalyzeByModuleCode(WeibullAnalyzer analyzer, string moduleCode)
        {
            Console.WriteLine($"分析模组代码: {moduleCode}");

            MWStructArray result = (MWStructArray)analyzer.WeibullAnalysisByModuleCode(
                new MWCharArray(moduleCode)
            );

            PrintWeibullResult(result);
        }

        /// <summary>
        /// 直接使用数据数组分析
        /// </summary>
        static void AnalyzeFromData(WeibullAnalyzer analyzer)
        {
            // 准备测试数据
            double[] failureTimes = { 100, 200, 300, 400, 500, 600, 750, 900, 1100, 1500 };
            double[] censoringTypes = { 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 };  // 0=完全, 1=右删失
            double[] quantities = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            double[] lastInspTimes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            Console.WriteLine($"分析 {failureTimes.Length} 个数据点");
            Console.WriteLine($"  失效时间: [{string.Join(", ", failureTimes)}]");

            // 转换为 MATLAB 数组
            MWNumericArray mwTimes = new MWNumericArray(failureTimes);
            MWNumericArray mwCensoring = new MWNumericArray(censoringTypes);
            MWNumericArray mwQuantities = new MWNumericArray(quantities);
            MWNumericArray mwLastInsp = new MWNumericArray(lastInspTimes);

            MWStructArray result = (MWStructArray)analyzer.WeibullAnalysisFromData(
                mwTimes,
                mwCensoring,
                mwQuantities,
                mwLastInsp,
                new MWCharArray("TestModule"),
                new MWCharArray("测试模组")
            );

            PrintWeibullResult(result);
        }

        /// <summary>
        /// 批量分析所有模组
        /// </summary>
        static void AnalyzeAll(WeibullAnalyzer analyzer)
        {
            Console.WriteLine("分析所有活跃模组...");

            MWStructArray results = (MWStructArray)analyzer.WeibullAnalysisAll();

            if (results == null || results.NumberOfElements == 0)
            {
                Console.WriteLine("  没有找到数据或分析失败");
                return;
            }

            int count = results.NumberOfElements;
            Console.WriteLine($"  共 {count} 个模组的分析结果:");
            Console.WriteLine();

            // 打印表头
            Console.WriteLine($"{"编码",-12} {"名称",-15} {"Beta",8} {"Eta",10} {"R²",8} {"MTTF",10}");
            Console.WriteLine(new string('-', 70));

            // 遍历结果
            for (int i = 1; i <= count; i++)
            {
                bool success = GetBoolField(results, "success", i);
                if (success)
                {
                    string code = GetStringField(results, "moduleCode", i);
                    string name = GetStringField(results, "moduleName", i);
                    double beta = GetDoubleField(results, "beta", i);
                    double eta = GetDoubleField(results, "eta", i);
                    double r2 = GetDoubleField(results, "r2", i);
                    double mttf = GetDoubleField(results, "mttf", i);

                    Console.WriteLine($"{code,-12} {name,-15} {beta,8:F3} {eta,10:F1} {r2,8:F3} {mttf,10:F1}");
                }
            }
        }

        /// <summary>
        /// 打印 Weibull 分析结果
        /// </summary>
        static void PrintWeibullResult(MWStructArray result)
        {
            if (result == null)
            {
                Console.WriteLine("  结果为空");
                return;
            }

            bool success = GetBoolField(result, "success");
            string message = GetStringField(result, "message");

            if (!success)
            {
                Console.WriteLine($"  分析失败: {message}");
                return;
            }

            // 获取各个字段
            string moduleCode = GetStringField(result, "moduleCode");
            string moduleName = GetStringField(result, "moduleName");
            double beta = GetDoubleField(result, "beta");
            double eta = GetDoubleField(result, "eta");
            double mttf = GetDoubleField(result, "mttf");
            double median = GetDoubleField(result, "median");
            double b10 = GetDoubleField(result, "b10");
            double b50 = GetDoubleField(result, "b50");
            double b90 = GetDoubleField(result, "b90");
            double r2 = GetDoubleField(result, "r2");
            double lowerBeta = GetDoubleField(result, "lowerBeta");
            double upperBeta = GetDoubleField(result, "upperBeta");
            double lowerEta = GetDoubleField(result, "lowerEta");
            double upperEta = GetDoubleField(result, "upperEta");
            int totalN = (int)GetDoubleField(result, "totalN");
            int completeN = (int)GetDoubleField(result, "completeN");
            int rightCensN = (int)GetDoubleField(result, "rightCensN");
            int intervalCensN = (int)GetDoubleField(result, "intervalCensN");
            int leftCensN = (int)GetDoubleField(result, "leftCensN");

            // 打印结果
            Console.WriteLine($"  模组: {moduleCode} ({moduleName})");
            Console.WriteLine($"  ────────────────────────────────────");
            Console.WriteLine($"  Weibull 参数:");
            Console.WriteLine($"    β (形状参数): {beta:F4} [{lowerBeta:F4}, {upperBeta:F4}]");
            Console.WriteLine($"    η (尺度参数): {eta:F2} [{lowerEta:F2}, {upperEta:F2}]");
            Console.WriteLine($"    R² 拟合度:    {r2:F4}");
            Console.WriteLine();
            Console.WriteLine($"  寿命指标:");
            Console.WriteLine($"    MTTF (平均失效时间): {mttf:F2}");
            Console.WriteLine($"    中位寿命:            {median:F2}");
            Console.WriteLine($"    B10 寿命:            {b10:F2}");
            Console.WriteLine($"    B50 寿命:            {b50:F2}");
            Console.WriteLine($"    B90 寿命:            {b90:F2}");
            Console.WriteLine();
            Console.WriteLine($"  数据统计:");
            Console.WriteLine($"    总样本:   {totalN}");
            Console.WriteLine($"    完全数据: {completeN}");
            Console.WriteLine($"    右删失:   {rightCensN}");
            Console.WriteLine($"    区间删失: {intervalCensN}");
            Console.WriteLine($"    左删失:   {leftCensN}");
        }

        #region 辅助方法 - 从 MWStructArray 获取字段值

        static double GetDoubleField(MWStructArray arr, string fieldName, int index = 1)
        {
            try
            {
                MWArray field = arr[fieldName, index];
                if (field is MWNumericArray numArray)
                {
                    return (double)numArray;
                }
                return double.NaN;
            }
            catch
            {
                return double.NaN;
            }
        }

        static string GetStringField(MWStructArray arr, string fieldName, int index = 1)
        {
            try
            {
                MWArray field = arr[fieldName, index];
                if (field is MWCharArray charArray)
                {
                    return charArray.ToString();
                }
                if (field is MWCellArray cellArray)
                {
                    return cellArray[1].ToString();
                }
                return field?.ToString() ?? "";
            }
            catch
            {
                return "";
            }
        }

        static bool GetBoolField(MWStructArray arr, string fieldName, int index = 1)
        {
            try
            {
                MWArray field = arr[fieldName, index];
                if (field is MWLogicalArray logArray)
                {
                    return (bool)logArray;
                }
                if (field is MWNumericArray numArray)
                {
                    return (double)numArray != 0;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
