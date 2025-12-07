/*
 * WeibullAnalysisDemo - C# 调用 MATLAB Weibull 分析 DLL
 * 版本: 1.1.0
 * 日期: 2025-12-07
 * 
 * MATLAB R2025b Native API 返回类型说明:
 * - 数值: double[,] (二维数组)
 * - 字符串: char[,] 或 string[]
 * - 逻辑值: bool[,] (二维数组)
 */

using System;
using WeibullAnalysisNative;  // 使用 Native 命名空间

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
                using var analyzer = new WeibullAnalyzer();

                // 示例1: 按模组ID分析 (需要数据库连接)
                Console.WriteLine("=== 示例1: 按模组ID分析 ===");
                AnalyzeByModuleID(analyzer, 1);
                Console.WriteLine();

                // 示例2: 按模组代码分析 (需要数据库连接)
                Console.WriteLine("=== 示例2: 按模组代码分析 ===");
                AnalyzeByModuleCode(analyzer, "MOD001");
                Console.WriteLine();

                // 示例3: 直接数据分析 (不需要数据库)
                Console.WriteLine("=== 示例3: 直接数据分析 ===");
                AnalyzeFromData(analyzer);
                Console.WriteLine();

                // 示例4: 批量分析 (需要数据库连接)
                Console.WriteLine("=== 示例4: 批量分析 ===");
                AnalyzeAll(analyzer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
                Console.WriteLine($"堆栈: {ex.StackTrace}");
            }

            Console.WriteLine();
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }

        /// <summary>
        /// 按模组ID分析
        /// </summary>
        static void AnalyzeByModuleID(WeibullAnalyzer analyzer, int moduleID)
        {
            Console.WriteLine($"分析模组ID: {moduleID}");
            
            // 重要：参数必须转换为 (object)，否则会匹配到错误的重载
            dynamic result = analyzer.WeibullAnalysisByModuleID((object)moduleID);
            PrintResult(result);
        }

        /// <summary>
        /// 按模组代码分析
        /// </summary>
        static void AnalyzeByModuleCode(WeibullAnalyzer analyzer, string moduleCode)
        {
            Console.WriteLine($"分析模组代码: {moduleCode}");
            dynamic result = analyzer.WeibullAnalysisByModuleCode((object)moduleCode);
            PrintResult(result);
        }

        /// <summary>
        /// 直接使用数据分析 (不需要数据库连接)
        /// </summary>
        static void AnalyzeFromData(WeibullAnalyzer analyzer)
        {
            // 准备测试数据
            double[] failureTimes = { 100, 200, 300, 400, 500, 600, 750, 900, 1100, 1500 };
            double[] censoringTypes = { 0, 0, 0, 0, 0, 0, 1, 0, 0, 1 };  // 0=完全失效, 1=右删失
            double[] quantities = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            double[] lastInspTimes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            Console.WriteLine($"分析 {failureTimes.Length} 个数据点");
            Console.WriteLine($"  失效时间: [{string.Join(", ", failureTimes)}]");

            // 所有参数必须转换为 (object)
            dynamic result = analyzer.WeibullAnalysisFromData(
                (object)failureTimes,
                (object)censoringTypes,
                (object)quantities,
                (object)lastInspTimes,
                (object)"TestModule",
                (object)"测试模组"
            );

            PrintResult(result);
        }

        /// <summary>
        /// 批量分析所有模组
        /// </summary>
        static void AnalyzeAll(WeibullAnalyzer analyzer)
        {
            Console.WriteLine("分析所有活跃模组...");
            dynamic results = analyzer.WeibullAnalysisAll();

            if (results == null)
            {
                Console.WriteLine("  没有数据");
                return;
            }

            int count = GetElementCount(results);
            Console.WriteLine($"  共 {count} 个模组");
            Console.WriteLine();

            Console.WriteLine($"{"编码",-12} {"名称",-15} {"Beta",8} {"Eta",10} {"R²",8} {"MTTF",10}");
            Console.WriteLine(new string('-', 70));

            for (int i = 1; i <= count; i++)
            {
                bool success = GetBool(results, "success", i);
                if (success)
                {
                    string code = GetString(results, "moduleCode", i);
                    string name = GetString(results, "moduleName", i);
                    double beta = GetDouble(results, "beta", i);
                    double eta = GetDouble(results, "eta", i);
                    double r2 = GetDouble(results, "r2", i);
                    double mttf = GetDouble(results, "mttf", i);
                    Console.WriteLine($"{code,-12} {name,-15} {beta,8:F3} {eta,10:F1} {r2,8:F3} {mttf,10:F1}");
                }
                else
                {
                    string msg = GetString(results, "message", i);
                    Console.WriteLine($"  模组{i} 失败: {msg}");
                }
            }
        }

        /// <summary>
        /// 打印分析结果
        /// </summary>
        static void PrintResult(dynamic result)
        {
            if (result == null)
            {
                Console.WriteLine("  结果为空");
                return;
            }

            bool success = GetBool(result, "success");
            string message = GetString(result, "message");

            if (!success)
            {
                Console.WriteLine($"  分析失败: {message}");
                return;
            }

            // 获取所有结果字段
            string moduleCode = GetString(result, "moduleCode");
            string moduleName = GetString(result, "moduleName");
            double beta = GetDouble(result, "beta");
            double eta = GetDouble(result, "eta");
            double mttf = GetDouble(result, "mttf");
            double median = GetDouble(result, "median");
            double b10 = GetDouble(result, "b10");
            double b50 = GetDouble(result, "b50");
            double b90 = GetDouble(result, "b90");
            double r2 = GetDouble(result, "r2");
            double lowerBeta = GetDouble(result, "lowerBeta");
            double upperBeta = GetDouble(result, "upperBeta");
            double lowerEta = GetDouble(result, "lowerEta");
            double upperEta = GetDouble(result, "upperEta");
            int totalN = (int)GetDouble(result, "totalN");
            int completeN = (int)GetDouble(result, "completeN");
            int rightCensN = (int)GetDouble(result, "rightCensN");
            int intervalCensN = (int)GetDouble(result, "intervalCensN");
            int leftCensN = (int)GetDouble(result, "leftCensN");

            // 输出结果
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

        #region 辅助方法 - 处理 MATLAB R2025b Native API 返回的数组类型

        /// <summary>
        /// 获取结构体数组元素数量
        /// </summary>
        static int GetElementCount(dynamic arr)
        {
            try { return (int)arr.NumberOfElements; }
            catch { return 1; }
        }

        /// <summary>
        /// 从结构体获取 double 值
        /// </summary>
        static double GetDouble(dynamic arr, string field, int index = 1)
        {
            try
            {
                dynamic val = arr[field, index];
                if (val == null) return double.NaN;

                Type t = val.GetType();

                // double[,] - 最常见
                if (t == typeof(double[,]))
                    return ((double[,])val)[0, 0];

                // double[]
                if (t == typeof(double[]))
                {
                    var a = (double[])val;
                    return a.Length > 0 ? a[0] : double.NaN;
                }

                // double
                if (t == typeof(double))
                    return (double)val;

                // 其他数值类型
                return Convert.ToDouble(val);
            }
            catch { return double.NaN; }
        }

        /// <summary>
        /// 从结构体获取 string 值
        /// </summary>
        static string GetString(dynamic arr, string field, int index = 1)
        {
            try
            {
                dynamic val = arr[field, index];
                if (val == null) return "";

                Type t = val.GetType();

                // string[] - C# 传入的字符串
                if (t == typeof(string[]))
                {
                    var a = (string[])val;
                    return a.Length > 0 ? (a[0]?.Trim() ?? "") : "";
                }

                // string
                if (t == typeof(string))
                    return ((string)val).Trim();

                // char[,] - MATLAB 原生字符串
                if (t == typeof(char[,]))
                {
                    var c = (char[,])val;
                    int cols = c.GetLength(1);
                    var chars = new char[cols];
                    for (int i = 0; i < cols; i++)
                        chars[i] = c[0, i];
                    return new string(chars).Trim();
                }

                // char[]
                if (t == typeof(char[]))
                    return new string((char[])val).Trim();

                // 其他 - 过滤类型名
                string s = val.ToString();
                if (!s.StartsWith("System.") && !s.StartsWith("MathWorks."))
                    return s.Trim();

                return "";
            }
            catch { return ""; }
        }

        /// <summary>
        /// 从结构体获取 bool 值
        /// </summary>
        static bool GetBool(dynamic arr, string field, int index = 1)
        {
            try
            {
                dynamic val = arr[field, index];
                if (val == null) return false;

                Type t = val.GetType();

                // bool[,] - MATLAB logical
                if (t == typeof(bool[,]))
                    return ((bool[,])val)[0, 0];

                // bool[]
                if (t == typeof(bool[]))
                {
                    var a = (bool[])val;
                    return a.Length > 0 && a[0];
                }

                // bool
                if (t == typeof(bool))
                    return (bool)val;

                // double[,] - MATLAB 用 1/0 表示
                if (t == typeof(double[,]))
                    return ((double[,])val)[0, 0] != 0;

                // double[]
                if (t == typeof(double[]))
                {
                    var a = (double[])val;
                    return a.Length > 0 && a[0] != 0;
                }

                return Convert.ToBoolean(val);
            }
            catch { return false; }
        }

        #endregion
    }
}
