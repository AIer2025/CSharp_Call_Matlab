/*
 * WeibullAnalysisWrapper.cs
 * 
 * 对 MATLAB 生成的 DLL 进行封装，提供更友好的 C# API
 * 处理 MWArray 与 C# 原生类型之间的转换
 */

using System;
using System.Collections.Generic;
using MathWorks.MATLAB.NET.Arrays;
using WeibullAnalysisNative; // MATLAB 生成的命名空间

namespace WeibullAnalysisDemo
{
    /// <summary>
    /// Weibull 分析结果数据类
    /// </summary>
    public class WeibullResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public int ModuleID { get; set; }
        public string ModuleCode { get; set; } = "";
        public string ModuleName { get; set; } = "";
        
        // Weibull 参数
        public double Beta { get; set; }        // 形状参数
        public double Eta { get; set; }         // 尺度参数
        public double R2 { get; set; }          // R² 拟合度
        
        // 置信区间
        public double LowerBeta { get; set; }
        public double UpperBeta { get; set; }
        public double LowerEta { get; set; }
        public double UpperEta { get; set; }
        
        // 寿命指标
        public double MTTF { get; set; }        // 平均失效时间
        public double Median { get; set; }      // 中位寿命
        public double B10 { get; set; }         // 10%失效寿命
        public double B50 { get; set; }         // 50%失效寿命
        public double B90 { get; set; }         // 90%失效寿命
        
        // 数据统计
        public int TotalN { get; set; }         // 总样本数
        public int CompleteN { get; set; }      // 完全数据数
        public int RightCensN { get; set; }     // 右删失数
        public int IntervalCensN { get; set; }  // 区间删失数
        public int LeftCensN { get; set; }      // 左删失数
        
        /// <summary>
        /// 失效模式判断 (根据 Beta 值)
        /// </summary>
        public string FailureMode
        {
            get
            {
                if (double.IsNaN(Beta)) return "未知";
                if (Beta < 1) return "早期失效 (递减失效率)";
                if (Math.Abs(Beta - 1) < 0.1) return "随机失效 (恒定失效率)";
                return "磨损失效 (递增失效率)";
            }
        }

        public override string ToString()
        {
            if (!Success)
                return $"分析失败: {Message}";
                
            return $"模组: {ModuleCode} ({ModuleName})\n" +
                   $"β={Beta:F4}, η={Eta:F2}, R²={R2:F4}\n" +
                   $"MTTF={MTTF:F2}, 失效模式={FailureMode}";
        }
    }

    /// <summary>
    /// 删失类型枚举
    /// </summary>
    public enum CensoringType
    {
        Complete = 0,       // 完全数据（精确失效时间）
        RightCensored = 1,  // 右删失（在观测时间还未失效）
        IntervalCensored = 2, // 区间删失（失效发生在两次检查之间）
        LeftCensored = 3    // 左删失（失效发生在首次观测之前）
    }

    /// <summary>
    /// Weibull 分析 API 封装类
    /// </summary>
    public class WeibullAnalysisWrapper : IDisposable
    {
        private WeibullAnalyzer? _analyzer;
        private bool _disposed;

        public WeibullAnalysisWrapper()
        {
            _analyzer = new WeibullAnalyzer();
        }

        /// <summary>
        /// 按模组ID进行 Weibull 分析
        /// </summary>
        /// <param name="moduleID">模组ID</param>
        /// <param name="dbHost">数据库主机 (可选)</param>
        /// <param name="dbPort">数据库端口 (可选)</param>
        /// <param name="dbName">数据库名称 (可选)</param>
        /// <param name="dbUser">数据库用户名 (可选)</param>
        /// <param name="dbPassword">数据库密码 (可选)</param>
        /// <returns>分析结果</returns>
        public WeibullResult AnalyzeByModuleID(int moduleID, 
            string? dbHost = null, int? dbPort = null, string? dbName = null,
            string? dbUser = null, string? dbPassword = null)
        {
            ThrowIfDisposed();

            try
            {
                MWArray result;
                
                if (dbHost != null)
                {
                    result = (MWArray)_analyzer!.WeibullAnalysisByModuleID(
                        new MWNumericArray(moduleID),
                        dbHost: new MWCharArray(dbHost),
                        dbPort: new MWNumericArray(dbPort ?? 3306),
                        dbName: new MWCharArray(dbName ?? "SysHardTestDB"),
                        dbUser: new MWCharArray(dbUser ?? "root"),
                        dbPassword: new MWCharArray(dbPassword ?? "")
                    );
                }
                else
                {
                    result = (MWArray)_analyzer!.WeibullAnalysisByModuleID(
                        new MWNumericArray(moduleID)
                    );
                }

                return ParseResult((MWStructArray)result);
            }
            catch (Exception ex)
            {
                return new WeibullResult
                {
                    Success = false,
                    Message = $"调用错误: {ex.Message}",
                    ModuleID = moduleID
                };
            }
        }

        /// <summary>
        /// 按模组代码进行 Weibull 分析
        /// </summary>
        public WeibullResult AnalyzeByModuleCode(string moduleCode,
            string? dbHost = null, int? dbPort = null, string? dbName = null,
            string? dbUser = null, string? dbPassword = null)
        {
            ThrowIfDisposed();

            try
            {
                MWArray result;
                
                if (dbHost != null)
                {
                    result = (MWArray)_analyzer!.WeibullAnalysisByModuleCode(
                        new MWCharArray(moduleCode),
                        dbHost: new MWCharArray(dbHost),
                        dbPort: new MWNumericArray(dbPort ?? 3306),
                        dbName: new MWCharArray(dbName ?? "SysHardTestDB"),
                        dbUser: new MWCharArray(dbUser ?? "root"),
                        dbPassword: new MWCharArray(dbPassword ?? "")
                    );
                }
                else
                {
                    result = (MWArray)_analyzer!.WeibullAnalysisByModuleCode(
                        new MWCharArray(moduleCode)
                    );
                }

                return ParseResult((MWStructArray)result);
            }
            catch (Exception ex)
            {
                return new WeibullResult
                {
                    Success = false,
                    Message = $"调用错误: {ex.Message}",
                    ModuleCode = moduleCode
                };
            }
        }

        /// <summary>
        /// 直接使用数据进行 Weibull 分析 (不访问数据库)
        /// </summary>
        /// <param name="failureTimes">失效时间数组</param>
        /// <param name="censoringTypes">删失类型数组 (可选，默认全为完全数据)</param>
        /// <param name="quantities">数量数组 (可选，默认全为1)</param>
        /// <param name="lastInspectionTimes">上次检查时间 (区间删失时使用)</param>
        /// <param name="moduleCode">模组代码 (可选)</param>
        /// <param name="moduleName">模组名称 (可选)</param>
        public WeibullResult AnalyzeFromData(
            double[] failureTimes,
            CensoringType[]? censoringTypes = null,
            double[]? quantities = null,
            double[]? lastInspectionTimes = null,
            string moduleCode = "DirectInput",
            string moduleName = "直接输入")
        {
            ThrowIfDisposed();

            if (failureTimes == null || failureTimes.Length < 2)
            {
                return new WeibullResult
                {
                    Success = false,
                    Message = "至少需要2个数据点"
                };
            }

            try
            {
                int n = failureTimes.Length;
                
                // 转换删失类型
                double[] censoring = new double[n];
                if (censoringTypes != null)
                {
                    for (int i = 0; i < Math.Min(n, censoringTypes.Length); i++)
                    {
                        censoring[i] = (double)censoringTypes[i];
                    }
                }

                // 默认数量
                double[] qty = quantities ?? new double[n];
                if (quantities == null)
                {
                    for (int i = 0; i < n; i++) qty[i] = 1;
                }

                // 默认上次检查时间
                double[] lastInsp = lastInspectionTimes ?? new double[n];

                MWArray result = (MWArray)_analyzer!.WeibullAnalysisFromData(
                    new MWNumericArray(failureTimes),
                    new MWNumericArray(censoring),
                    new MWNumericArray(qty),
                    new MWNumericArray(lastInsp),
                    new MWCharArray(moduleCode),
                    new MWCharArray(moduleName)
                );

                return ParseResult((MWStructArray)result);
            }
            catch (Exception ex)
            {
                return new WeibullResult
                {
                    Success = false,
                    Message = $"调用错误: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 分析所有活跃模组
        /// </summary>
        public List<WeibullResult> AnalyzeAll(
            string? dbHost = null, int? dbPort = null, string? dbName = null,
            string? dbUser = null, string? dbPassword = null)
        {
            ThrowIfDisposed();
            var results = new List<WeibullResult>();

            try
            {
                MWArray mwResults;
                
                if (dbHost != null)
                {
                    mwResults = (MWArray)_analyzer!.WeibullAnalysisAll(
                        new MWCharArray(dbHost),
                        new MWNumericArray(dbPort ?? 3306),
                        new MWCharArray(dbName ?? "SysHardTestDB"),
                        new MWCharArray(dbUser ?? "root"),
                        new MWCharArray(dbPassword ?? "")
                    );
                }
                else
                {
                    mwResults = (MWArray)_analyzer!.WeibullAnalysisAll();
                }

                if (mwResults is MWStructArray structArray)
                {
                    int count = structArray.NumberOfElements;
                    for (int i = 1; i <= count; i++)
                    {
                        results.Add(ParseResult(structArray, i));
                    }
                }
            }
            catch (Exception ex)
            {
                results.Add(new WeibullResult
                {
                    Success = false,
                    Message = $"批量分析错误: {ex.Message}"
                });
            }

            return results;
        }

        #region 私有方法

        private WeibullResult ParseResult(MWStructArray arr, int index = 1)
        {
            var result = new WeibullResult
            {
                Success = GetBool(arr, "success", index),
                Message = GetString(arr, "message", index),
                ModuleID = (int)GetDouble(arr, "moduleID", index),
                ModuleCode = GetString(arr, "moduleCode", index),
                ModuleName = GetString(arr, "moduleName", index),
                Beta = GetDouble(arr, "beta", index),
                Eta = GetDouble(arr, "eta", index),
                R2 = GetDouble(arr, "r2", index),
                LowerBeta = GetDouble(arr, "lowerBeta", index),
                UpperBeta = GetDouble(arr, "upperBeta", index),
                LowerEta = GetDouble(arr, "lowerEta", index),
                UpperEta = GetDouble(arr, "upperEta", index),
                MTTF = GetDouble(arr, "mttf", index),
                Median = GetDouble(arr, "median", index),
                B10 = GetDouble(arr, "b10", index),
                B50 = GetDouble(arr, "b50", index),
                B90 = GetDouble(arr, "b90", index),
                TotalN = (int)GetDouble(arr, "totalN", index),
                CompleteN = (int)GetDouble(arr, "completeN", index),
                RightCensN = (int)GetDouble(arr, "rightCensN", index),
                IntervalCensN = (int)GetDouble(arr, "intervalCensN", index),
                LeftCensN = (int)GetDouble(arr, "leftCensN", index)
            };

            return result;
        }

        private static double GetDouble(MWStructArray arr, string field, int index = 1)
        {
            try
            {
                var f = arr[field, index];
                if (f is MWNumericArray num) return (double)num;
                return double.NaN;
            }
            catch { return double.NaN; }
        }

        private static string GetString(MWStructArray arr, string field, int index = 1)
        {
            try
            {
                var f = arr[field, index];
                if (f is MWCharArray c) return c.ToString();
                if (f is MWCellArray cell) return cell[1]?.ToString() ?? "";
                return f?.ToString() ?? "";
            }
            catch { return ""; }
        }

        private static bool GetBool(MWStructArray arr, string field, int index = 1)
        {
            try
            {
                var f = arr[field, index];
                if (f is MWLogicalArray log) return (bool)log;
                if (f is MWNumericArray num) return (double)num != 0;
                return false;
            }
            catch { return false; }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(WeibullAnalysisWrapper));
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _analyzer?.Dispose();
                    _analyzer = null;
                }
                _disposed = true;
            }
        }

        ~WeibullAnalysisWrapper()
        {
            Dispose(false);
        }

        #endregion
    }
}