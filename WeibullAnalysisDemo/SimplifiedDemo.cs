/*
 * SimplifiedDemo.cs
 * 
 * 使用 WeibullAnalysisWrapper 封装类的简化示例
 * 展示更友好的 C# 调用方式
 */

using System;
using WeibullAnalysisDemo;

namespace SimplifiedUsage
{
    class SimplifiedDemo
    {
        static void RunDemo()
        {
            Console.WriteLine("=== 使用封装类的简化示例 ===\n");

            // 使用 using 语句自动管理资源
            using var weibull = new WeibullAnalysisWrapper();

            // 示例1: 按模组ID分析
            Console.WriteLine("1. 按模组ID分析:");
            var result1 = weibull.AnalyzeByModuleID(1);
            PrintResult(result1);

            // 示例2: 按模组代码分析 (使用自定义数据库配置)
            Console.WriteLine("\n2. 按模组代码分析 (自定义数据库):");
            var result2 = weibull.AnalyzeByModuleCode(
                moduleCode: "MOD001",
                dbHost: "192.168.1.100",
                dbPort: 3306,
                dbName: "TestDB",
                dbUser: "admin",
                dbPassword: "password123"
            );
            PrintResult(result2);

            // 示例3: 直接数据分析 - 最常用的场景
            Console.WriteLine("\n3. 直接数据分析 (不需要数据库):");
            
            // 准备测试数据
            double[] times = { 150, 280, 320, 450, 520, 680, 750, 890, 1020, 1200 };
            
            // 混合删失类型: 大部分是完全数据，有2个右删失
            CensoringType[] censoring = {
                CensoringType.Complete,
                CensoringType.Complete,
                CensoringType.Complete,
                CensoringType.RightCensored,  // 在450小时还未失效
                CensoringType.Complete,
                CensoringType.Complete,
                CensoringType.Complete,
                CensoringType.RightCensored,  // 在890小时还未失效
                CensoringType.Complete,
                CensoringType.Complete
            };

            var result3 = weibull.AnalyzeFromData(
                failureTimes: times,
                censoringTypes: censoring,
                moduleCode: "TEST001",
                moduleName: "测试组件"
            );
            PrintResult(result3);

            // 示例4: 区间删失数据
            Console.WriteLine("\n4. 区间删失数据分析:");
            double[] intervalTimes = { 100, 200, 300, 400, 500 };
            double[] lastInspection = { 0, 0, 250, 0, 400 };  // 第3和第5个是区间删失
            CensoringType[] intervalCensoring = {
                CensoringType.Complete,
                CensoringType.Complete,
                CensoringType.IntervalCensored,  // 失效在250-300之间
                CensoringType.Complete,
                CensoringType.IntervalCensored   // 失效在400-500之间
            };

            var result4 = weibull.AnalyzeFromData(
                failureTimes: intervalTimes,
                censoringTypes: intervalCensoring,
                lastInspectionTimes: lastInspection,
                moduleCode: "INTERVAL01",
                moduleName: "区间检测组件"
            );
            PrintResult(result4);

            // 示例5: 批量分析
            Console.WriteLine("\n5. 批量分析所有模组:");
            var allResults = weibull.AnalyzeAll();
            
            Console.WriteLine($"共 {allResults.Count} 个结果:\n");
            Console.WriteLine($"{"编码",-10} {"β",-8} {"η",-10} {"MTTF",-10} {"失效模式"}");
            Console.WriteLine(new string('-', 60));
            
            foreach (var r in allResults)
            {
                if (r.Success)
                {
                    Console.WriteLine($"{r.ModuleCode,-10} {r.Beta,-8:F3} {r.Eta,-10:F1} {r.MTTF,-10:F1} {r.FailureMode}");
                }
            }
        }

        static void PrintResult(WeibullResult result)
        {
            if (!result.Success)
            {
                Console.WriteLine($"  失败: {result.Message}");
                return;
            }

            Console.WriteLine($"  模组: {result.ModuleCode} ({result.ModuleName})");
            Console.WriteLine($"  β = {result.Beta:F4} (95% CI: [{result.LowerBeta:F4}, {result.UpperBeta:F4}])");
            Console.WriteLine($"  η = {result.Eta:F2} (95% CI: [{result.LowerEta:F2}, {result.UpperEta:F2}])");
            Console.WriteLine($"  R² = {result.R2:F4}");
            Console.WriteLine($"  MTTF = {result.MTTF:F2}");
            Console.WriteLine($"  失效模式: {result.FailureMode}");
            Console.WriteLine($"  数据: 总计{result.TotalN}, 完全{result.CompleteN}, " +
                            $"右删失{result.RightCensN}, 区间{result.IntervalCensN}, 左删失{result.LeftCensN}");
        }
    }
}
