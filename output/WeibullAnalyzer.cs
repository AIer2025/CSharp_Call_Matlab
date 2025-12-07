/*
* MATLAB Compiler: 25.2 (R2025b)
* 日期: Sun Dec  7 10:08:34 2025
* 参量:
* "-B""macro_default""-W""dotnet:WeibullAnalysis,WeibullAnalyzer,4.0""-T""link:lib""-d""./
* output""WeibullAnalysisByModuleID.m""WeibullAnalysisByModuleCode.m""WeibullAnalysisFromD
* ata.m""WeibullAnalysisAll.m""-a""WeibullAnalysisLib.m""-v"
*/
using System;
using System.Reflection;
using System.IO;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;

#if SHARED
[assembly: System.Reflection.AssemblyKeyFile(@"")]
#endif

namespace WeibullAnalysis
{

  /// <summary>
  /// The WeibullAnalyzer class provides a CLS compliant, MWArray interface to the MATLAB
  /// functions contained in the files:
  /// <newpara></newpara>
  /// C:\Users\86138\Desktop\深圳测试服务器\WeibullAnalysis-DLL\WeibullAnalysisByMo
  /// duleID.m
  /// <newpara></newpara>
  /// C:\Users\86138\Desktop\深圳测试服务器\WeibullAnalysis-DLL\WeibullAnalysisByMo
  /// duleCode.m
  /// <newpara></newpara>
  /// C:\Users\86138\Desktop\深圳测试服务器\WeibullAnalysis-DLL\WeibullAnalysisFrom
  /// Data.m
  /// <newpara></newpara>
  /// C:\Users\86138\Desktop\深圳测试服务器\WeibullAnalysis-DLL\WeibullAnalysisAll.
  /// m
  /// </summary>
  /// <remarks>
  /// @Version 1.0
  /// </remarks>
  public class WeibullAnalyzer : IDisposable
  {
    #region Constructors

    /// <summary internal= "true">
    /// The static constructor instantiates and initializes the MATLAB Runtime instance.
    /// </summary>
    static WeibullAnalyzer()
    {
      if (MWMCR.MCRAppInitialized)
      {
        try
        {
          System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

          string ctfFilePath= assembly.Location;

		  int lastDelimiter = ctfFilePath.LastIndexOf(@"/");

	      if (lastDelimiter == -1)
		  {
		    lastDelimiter = ctfFilePath.LastIndexOf(@"\");
		  }

          ctfFilePath= ctfFilePath.Remove(lastDelimiter, (ctfFilePath.Length - lastDelimiter));

          string ctfFileName = "WeibullAnalysis.ctf";

          Stream embeddedCtfStream = null;

          String[] resourceStrings = assembly.GetManifestResourceNames();

          foreach (String name in resourceStrings)
          {
            if (name.Contains(ctfFileName))
            {
              embeddedCtfStream = assembly.GetManifestResourceStream(name);
              break;
            }
          }
          mcr= new MWMCR("",
                         ctfFilePath, embeddedCtfStream, true);
        }
        catch(Exception ex)
        {
          ex_ = new Exception("MWArray assembly failed to be initialized", ex);
        }
      }
      else
      {
        ex_ = new ApplicationException("MWArray assembly could not be initialized");
      }
    }


    /// <summary>
    /// Constructs a new instance of the WeibullAnalyzer class.
    /// </summary>
    public WeibullAnalyzer()
    {
      if(ex_ != null)
      {
        throw ex_;
      }
    }


    #endregion Constructors

    #region Finalize

    /// <summary internal= "true">
    /// Class destructor called by the CLR garbage collector.
    /// </summary>
    ~WeibullAnalyzer()
    {
      Dispose(false);
    }


    /// <summary>
    /// Frees the native resources associated with this object
    /// </summary>
    public void Dispose()
    {
      Dispose(true);

      GC.SuppressFinalize(this);
    }


    /// <summary internal= "true">
    /// Internal dispose function
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposed)
      {
        disposed= true;

        if (disposing)
        {
          // Free managed resources;
        }

        // Free native resources
      }
    }


    #endregion Finalize

    #region Methods

    /// <summary>
    /// Provides a single output, 0-input MWArrayinterface to the
    /// WeibullAnalysisByModuleID MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleID()
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleID", new MWArray[]{});
    }


    /// <summary>
    /// Provides a single output, 1-input MWArrayinterface to the
    /// WeibullAnalysisByModuleID MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="moduleID">Input argument #1</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleID(MWArray moduleID)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleID", moduleID);
    }


    /// <summary>
    /// Provides a single output, 2-input MWArrayinterface to the
    /// WeibullAnalysisByModuleID MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="moduleID">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleID(MWArray moduleID, MWArray dbHost)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleID", moduleID, dbHost);
    }


    /// <summary>
    /// Provides a single output, 3-input MWArrayinterface to the
    /// WeibullAnalysisByModuleID MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="moduleID">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleID(MWArray moduleID, MWArray dbHost, MWArray 
                                       dbPort)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleID", moduleID, dbHost, dbPort);
    }


    /// <summary>
    /// Provides a single output, 4-input MWArrayinterface to the
    /// WeibullAnalysisByModuleID MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="moduleID">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleID(MWArray moduleID, MWArray dbHost, MWArray 
                                       dbPort, MWArray dbName)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleID", moduleID, dbHost, dbPort, dbName);
    }


    /// <summary>
    /// Provides a single output, 5-input MWArrayinterface to the
    /// WeibullAnalysisByModuleID MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="moduleID">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <param name="dbUser">Input argument #5</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleID(MWArray moduleID, MWArray dbHost, MWArray 
                                       dbPort, MWArray dbName, MWArray dbUser)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleID", moduleID, dbHost, dbPort, dbName, dbUser);
    }


    /// <summary>
    /// Provides a single output, 6-input MWArrayinterface to the
    /// WeibullAnalysisByModuleID MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="moduleID">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <param name="dbUser">Input argument #5</param>
    /// <param name="dbPassword">Input argument #6</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleID(MWArray moduleID, MWArray dbHost, MWArray 
                                       dbPort, MWArray dbName, MWArray dbUser, MWArray 
                                       dbPassword)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleID", moduleID, dbHost, dbPort, dbName, dbUser, dbPassword);
    }


    /// <summary>
    /// Provides a single output, 7-input MWArrayinterface to the
    /// WeibullAnalysisByModuleID MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="moduleID">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <param name="dbUser">Input argument #5</param>
    /// <param name="dbPassword">Input argument #6</param>
    /// <param name="jdbcPath">Input argument #7</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleID(MWArray moduleID, MWArray dbHost, MWArray 
                                       dbPort, MWArray dbName, MWArray dbUser, MWArray 
                                       dbPassword, MWArray jdbcPath)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleID", moduleID, dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath);
    }


    /// <summary>
    /// Provides the standard 0-input MWArray interface to the WeibullAnalysisByModuleID
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleID(int numArgsOut)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleID", new MWArray[]{});
    }


    /// <summary>
    /// Provides the standard 1-input MWArray interface to the WeibullAnalysisByModuleID
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleID">Input argument #1</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleID(int numArgsOut, MWArray moduleID)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleID", moduleID);
    }


    /// <summary>
    /// Provides the standard 2-input MWArray interface to the WeibullAnalysisByModuleID
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleID">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleID(int numArgsOut, MWArray moduleID, MWArray 
                                         dbHost)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleID", moduleID, dbHost);
    }


    /// <summary>
    /// Provides the standard 3-input MWArray interface to the WeibullAnalysisByModuleID
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleID">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleID(int numArgsOut, MWArray moduleID, MWArray 
                                         dbHost, MWArray dbPort)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleID", moduleID, dbHost, dbPort);
    }


    /// <summary>
    /// Provides the standard 4-input MWArray interface to the WeibullAnalysisByModuleID
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleID">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleID(int numArgsOut, MWArray moduleID, MWArray 
                                         dbHost, MWArray dbPort, MWArray dbName)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleID", moduleID, dbHost, dbPort, dbName);
    }


    /// <summary>
    /// Provides the standard 5-input MWArray interface to the WeibullAnalysisByModuleID
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleID">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <param name="dbUser">Input argument #5</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleID(int numArgsOut, MWArray moduleID, MWArray 
                                         dbHost, MWArray dbPort, MWArray dbName, MWArray 
                                         dbUser)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleID", moduleID, dbHost, dbPort, dbName, dbUser);
    }


    /// <summary>
    /// Provides the standard 6-input MWArray interface to the WeibullAnalysisByModuleID
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleID">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <param name="dbUser">Input argument #5</param>
    /// <param name="dbPassword">Input argument #6</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleID(int numArgsOut, MWArray moduleID, MWArray 
                                         dbHost, MWArray dbPort, MWArray dbName, MWArray 
                                         dbUser, MWArray dbPassword)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleID", moduleID, dbHost, dbPort, dbName, dbUser, dbPassword);
    }


    /// <summary>
    /// Provides the standard 7-input MWArray interface to the WeibullAnalysisByModuleID
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleID">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <param name="dbUser">Input argument #5</param>
    /// <param name="dbPassword">Input argument #6</param>
    /// <param name="jdbcPath">Input argument #7</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleID(int numArgsOut, MWArray moduleID, MWArray 
                                         dbHost, MWArray dbPort, MWArray dbName, MWArray 
                                         dbUser, MWArray dbPassword, MWArray jdbcPath)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleID", moduleID, dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath);
    }


    /// <summary>
    /// Provides an interface for the WeibullAnalysisByModuleID function in which the
    /// input and output
    /// arguments are specified as an array of MWArrays.
    /// </summary>
    /// <remarks>
    /// This method will allocate and return by reference the output argument
    /// array.<newpara></newpara>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULEID 按模组ID进行Weibull分析
    /// result = WeibullAnalysisByModuleID(moduleID) 对指定模组ID进行分析
    /// result = WeibullAnalysisByModuleID(moduleID, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleID - 模组ID (数字或字符串)
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含以下字段:
    /// success - 是否成功 (logical)
    /// message - 状态消息 (string)
    /// moduleID - 模组ID (double)
    /// moduleCode - 模组代码 (string)
    /// moduleName - 模组名称 (string)
    /// beta - 形状参数 (double)
    /// eta - 尺度参数 (double)
    /// mttf - 平均失效时间 (double)
    /// median - 中位寿命 (double)
    /// b10, b50, b90 - B寿命 (double)
    /// r2 - R²拟合度 (double)
    /// lowerBeta, upperBeta - beta置信区间 (double)
    /// lowerEta, upperEta - eta置信区间 (double)
    /// totalN, completeN, rightCensN, intervalCensN, leftCensN - 数据统计 (double)
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return</param>
    /// <param name= "argsOut">Array of MWArray output arguments</param>
    /// <param name= "argsIn">Array of MWArray input arguments</param>
    ///
    public void WeibullAnalysisByModuleID(int numArgsOut, ref MWArray[] argsOut, 
                                MWArray[] argsIn)
    {
      mcr.EvaluateFunction("WeibullAnalysisByModuleID", numArgsOut, ref argsOut, argsIn);
    }


    /// <summary>
    /// Provides a single output, 0-input MWArrayinterface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleCode()
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleCode", new MWArray[]{});
    }


    /// <summary>
    /// Provides a single output, 1-input MWArrayinterface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="moduleCode">Input argument #1</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleCode(MWArray moduleCode)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleCode", moduleCode);
    }


    /// <summary>
    /// Provides a single output, 2-input MWArrayinterface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="moduleCode">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleCode(MWArray moduleCode, MWArray dbHost)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleCode", moduleCode, dbHost);
    }


    /// <summary>
    /// Provides a single output, 3-input MWArrayinterface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="moduleCode">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleCode(MWArray moduleCode, MWArray dbHost, 
                                         MWArray dbPort)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleCode", moduleCode, dbHost, dbPort);
    }


    /// <summary>
    /// Provides a single output, 4-input MWArrayinterface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="moduleCode">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleCode(MWArray moduleCode, MWArray dbHost, 
                                         MWArray dbPort, MWArray dbName)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleCode", moduleCode, dbHost, dbPort, dbName);
    }


    /// <summary>
    /// Provides a single output, 5-input MWArrayinterface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="moduleCode">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <param name="dbUser">Input argument #5</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleCode(MWArray moduleCode, MWArray dbHost, 
                                         MWArray dbPort, MWArray dbName, MWArray dbUser)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleCode", moduleCode, dbHost, dbPort, dbName, dbUser);
    }


    /// <summary>
    /// Provides a single output, 6-input MWArrayinterface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="moduleCode">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <param name="dbUser">Input argument #5</param>
    /// <param name="dbPassword">Input argument #6</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleCode(MWArray moduleCode, MWArray dbHost, 
                                         MWArray dbPort, MWArray dbName, MWArray dbUser, 
                                         MWArray dbPassword)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleCode", moduleCode, dbHost, dbPort, dbName, dbUser, dbPassword);
    }


    /// <summary>
    /// Provides a single output, 7-input MWArrayinterface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="moduleCode">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <param name="dbUser">Input argument #5</param>
    /// <param name="dbPassword">Input argument #6</param>
    /// <param name="jdbcPath">Input argument #7</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisByModuleCode(MWArray moduleCode, MWArray dbHost, 
                                         MWArray dbPort, MWArray dbName, MWArray dbUser, 
                                         MWArray dbPassword, MWArray jdbcPath)
    {
      return mcr.EvaluateFunction("WeibullAnalysisByModuleCode", moduleCode, dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath);
    }


    /// <summary>
    /// Provides the standard 0-input MWArray interface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleCode(int numArgsOut)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleCode", new MWArray[]{});
    }


    /// <summary>
    /// Provides the standard 1-input MWArray interface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleCode">Input argument #1</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleCode(int numArgsOut, MWArray moduleCode)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleCode", moduleCode);
    }


    /// <summary>
    /// Provides the standard 2-input MWArray interface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleCode">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleCode(int numArgsOut, MWArray moduleCode, 
                                           MWArray dbHost)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleCode", moduleCode, dbHost);
    }


    /// <summary>
    /// Provides the standard 3-input MWArray interface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleCode">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleCode(int numArgsOut, MWArray moduleCode, 
                                           MWArray dbHost, MWArray dbPort)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleCode", moduleCode, dbHost, dbPort);
    }


    /// <summary>
    /// Provides the standard 4-input MWArray interface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleCode">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleCode(int numArgsOut, MWArray moduleCode, 
                                           MWArray dbHost, MWArray dbPort, MWArray dbName)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleCode", moduleCode, dbHost, dbPort, dbName);
    }


    /// <summary>
    /// Provides the standard 5-input MWArray interface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleCode">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <param name="dbUser">Input argument #5</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleCode(int numArgsOut, MWArray moduleCode, 
                                           MWArray dbHost, MWArray dbPort, MWArray 
                                           dbName, MWArray dbUser)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleCode", moduleCode, dbHost, dbPort, dbName, dbUser);
    }


    /// <summary>
    /// Provides the standard 6-input MWArray interface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleCode">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <param name="dbUser">Input argument #5</param>
    /// <param name="dbPassword">Input argument #6</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleCode(int numArgsOut, MWArray moduleCode, 
                                           MWArray dbHost, MWArray dbPort, MWArray 
                                           dbName, MWArray dbUser, MWArray dbPassword)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleCode", moduleCode, dbHost, dbPort, dbName, dbUser, dbPassword);
    }


    /// <summary>
    /// Provides the standard 7-input MWArray interface to the
    /// WeibullAnalysisByModuleCode MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="moduleCode">Input argument #1</param>
    /// <param name="dbHost">Input argument #2</param>
    /// <param name="dbPort">Input argument #3</param>
    /// <param name="dbName">Input argument #4</param>
    /// <param name="dbUser">Input argument #5</param>
    /// <param name="dbPassword">Input argument #6</param>
    /// <param name="jdbcPath">Input argument #7</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisByModuleCode(int numArgsOut, MWArray moduleCode, 
                                           MWArray dbHost, MWArray dbPort, MWArray 
                                           dbName, MWArray dbUser, MWArray dbPassword, 
                                           MWArray jdbcPath)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisByModuleCode", moduleCode, dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath);
    }


    /// <summary>
    /// Provides an interface for the WeibullAnalysisByModuleCode function in which the
    /// input and output
    /// arguments are specified as an array of MWArrays.
    /// </summary>
    /// <remarks>
    /// This method will allocate and return by reference the output argument
    /// array.<newpara></newpara>
    /// M-Documentation:
    /// WEIBULLANALYSISBYMODULECODE 按模组代码进行Weibull分析
    /// result = WeibullAnalysisByModuleCode(moduleCode)
    /// 对指定模组代码进行分析
    /// result = WeibullAnalysisByModuleCode(moduleCode, dbHost, dbPort, dbName, dbUser,
    /// dbPassword, jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// moduleCode - 模组代码 (字符串)
    /// dbHost - 数据库服务器地址 (可选)
    /// dbPort - 端口号 (可选)
    /// dbName - 数据库名称 (可选)
    /// dbUser - 用户名 (可选)
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return</param>
    /// <param name= "argsOut">Array of MWArray output arguments</param>
    /// <param name= "argsIn">Array of MWArray input arguments</param>
    ///
    public void WeibullAnalysisByModuleCode(int numArgsOut, ref MWArray[] argsOut, 
                                  MWArray[] argsIn)
    {
      mcr.EvaluateFunction("WeibullAnalysisByModuleCode", numArgsOut, ref argsOut, 
                                  argsIn);
    }


    /// <summary>
    /// Provides a single output, 0-input MWArrayinterface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisFromData()
    {
      return mcr.EvaluateFunction("WeibullAnalysisFromData", new MWArray[]{});
    }


    /// <summary>
    /// Provides a single output, 1-input MWArrayinterface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="failureTimes">Input argument #1</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisFromData(MWArray failureTimes)
    {
      return mcr.EvaluateFunction("WeibullAnalysisFromData", failureTimes);
    }


    /// <summary>
    /// Provides a single output, 2-input MWArrayinterface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="failureTimes">Input argument #1</param>
    /// <param name="censoringTypes">Input argument #2</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisFromData(MWArray failureTimes, MWArray censoringTypes)
    {
      return mcr.EvaluateFunction("WeibullAnalysisFromData", failureTimes, censoringTypes);
    }


    /// <summary>
    /// Provides a single output, 3-input MWArrayinterface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="failureTimes">Input argument #1</param>
    /// <param name="censoringTypes">Input argument #2</param>
    /// <param name="quantities">Input argument #3</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisFromData(MWArray failureTimes, MWArray censoringTypes, 
                                     MWArray quantities)
    {
      return mcr.EvaluateFunction("WeibullAnalysisFromData", failureTimes, censoringTypes, quantities);
    }


    /// <summary>
    /// Provides a single output, 4-input MWArrayinterface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="failureTimes">Input argument #1</param>
    /// <param name="censoringTypes">Input argument #2</param>
    /// <param name="quantities">Input argument #3</param>
    /// <param name="lastInspectionTimes">Input argument #4</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisFromData(MWArray failureTimes, MWArray censoringTypes, 
                                     MWArray quantities, MWArray lastInspectionTimes)
    {
      return mcr.EvaluateFunction("WeibullAnalysisFromData", failureTimes, censoringTypes, quantities, lastInspectionTimes);
    }


    /// <summary>
    /// Provides a single output, 5-input MWArrayinterface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="failureTimes">Input argument #1</param>
    /// <param name="censoringTypes">Input argument #2</param>
    /// <param name="quantities">Input argument #3</param>
    /// <param name="lastInspectionTimes">Input argument #4</param>
    /// <param name="moduleCode">Input argument #5</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisFromData(MWArray failureTimes, MWArray censoringTypes, 
                                     MWArray quantities, MWArray lastInspectionTimes, 
                                     MWArray moduleCode)
    {
      return mcr.EvaluateFunction("WeibullAnalysisFromData", failureTimes, censoringTypes, quantities, lastInspectionTimes, moduleCode);
    }


    /// <summary>
    /// Provides a single output, 6-input MWArrayinterface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="failureTimes">Input argument #1</param>
    /// <param name="censoringTypes">Input argument #2</param>
    /// <param name="quantities">Input argument #3</param>
    /// <param name="lastInspectionTimes">Input argument #4</param>
    /// <param name="moduleCode">Input argument #5</param>
    /// <param name="moduleName">Input argument #6</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisFromData(MWArray failureTimes, MWArray censoringTypes, 
                                     MWArray quantities, MWArray lastInspectionTimes, 
                                     MWArray moduleCode, MWArray moduleName)
    {
      return mcr.EvaluateFunction("WeibullAnalysisFromData", failureTimes, censoringTypes, quantities, lastInspectionTimes, moduleCode, moduleName);
    }


    /// <summary>
    /// Provides the standard 0-input MWArray interface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisFromData(int numArgsOut)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisFromData", new MWArray[]{});
    }


    /// <summary>
    /// Provides the standard 1-input MWArray interface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="failureTimes">Input argument #1</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisFromData(int numArgsOut, MWArray failureTimes)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisFromData", failureTimes);
    }


    /// <summary>
    /// Provides the standard 2-input MWArray interface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="failureTimes">Input argument #1</param>
    /// <param name="censoringTypes">Input argument #2</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisFromData(int numArgsOut, MWArray failureTimes, 
                                       MWArray censoringTypes)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisFromData", failureTimes, censoringTypes);
    }


    /// <summary>
    /// Provides the standard 3-input MWArray interface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="failureTimes">Input argument #1</param>
    /// <param name="censoringTypes">Input argument #2</param>
    /// <param name="quantities">Input argument #3</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisFromData(int numArgsOut, MWArray failureTimes, 
                                       MWArray censoringTypes, MWArray quantities)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisFromData", failureTimes, censoringTypes, quantities);
    }


    /// <summary>
    /// Provides the standard 4-input MWArray interface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="failureTimes">Input argument #1</param>
    /// <param name="censoringTypes">Input argument #2</param>
    /// <param name="quantities">Input argument #3</param>
    /// <param name="lastInspectionTimes">Input argument #4</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisFromData(int numArgsOut, MWArray failureTimes, 
                                       MWArray censoringTypes, MWArray quantities, 
                                       MWArray lastInspectionTimes)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisFromData", failureTimes, censoringTypes, quantities, lastInspectionTimes);
    }


    /// <summary>
    /// Provides the standard 5-input MWArray interface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="failureTimes">Input argument #1</param>
    /// <param name="censoringTypes">Input argument #2</param>
    /// <param name="quantities">Input argument #3</param>
    /// <param name="lastInspectionTimes">Input argument #4</param>
    /// <param name="moduleCode">Input argument #5</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisFromData(int numArgsOut, MWArray failureTimes, 
                                       MWArray censoringTypes, MWArray quantities, 
                                       MWArray lastInspectionTimes, MWArray moduleCode)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisFromData", failureTimes, censoringTypes, quantities, lastInspectionTimes, moduleCode);
    }


    /// <summary>
    /// Provides the standard 6-input MWArray interface to the WeibullAnalysisFromData
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="failureTimes">Input argument #1</param>
    /// <param name="censoringTypes">Input argument #2</param>
    /// <param name="quantities">Input argument #3</param>
    /// <param name="lastInspectionTimes">Input argument #4</param>
    /// <param name="moduleCode">Input argument #5</param>
    /// <param name="moduleName">Input argument #6</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisFromData(int numArgsOut, MWArray failureTimes, 
                                       MWArray censoringTypes, MWArray quantities, 
                                       MWArray lastInspectionTimes, MWArray moduleCode, 
                                       MWArray moduleName)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisFromData", failureTimes, censoringTypes, quantities, lastInspectionTimes, moduleCode, moduleName);
    }


    /// <summary>
    /// Provides an interface for the WeibullAnalysisFromData function in which the input
    /// and output
    /// arguments are specified as an array of MWArrays.
    /// </summary>
    /// <remarks>
    /// This method will allocate and return by reference the output argument
    /// array.<newpara></newpara>
    /// M-Documentation:
    /// WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
    /// 不需要访问数据库，直接传入失效数据进行分析
    /// 输入参数:
    /// failureTimes - 失效时间数组 (double[])
    /// censoringTypes - 删失类型数组 (double[]) 
    /// 0=完全数据, 1=右删失, 2=区间删失, 3=左删失
    /// quantities - 数量数组 (double[])，可选，默认全1
    /// lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
    /// moduleCode - 模组代码 (string)，可选
    /// moduleName - 模组名称 (string)，可选
    /// 输出参数:
    /// result - 结构体，包含分析结果
    /// 示例:
    /// 简单完全数据分析
    /// times = [100, 200, 300, 400, 500];
    /// result = WeibullAnalysisFromData(times);
    /// 混合删失数据分析
    /// times = [100, 200, 300, 400, 500];
    /// censorTypes = [0, 0, 1, 0, 2];     0=完全, 1=右删失, 2=区间删失
    /// quantities = [1, 1, 1, 1, 1];
    /// lastInsp = [0, 0, 0, 0, 350];      区间删失的下界
    /// result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return</param>
    /// <param name= "argsOut">Array of MWArray output arguments</param>
    /// <param name= "argsIn">Array of MWArray input arguments</param>
    ///
    public void WeibullAnalysisFromData(int numArgsOut, ref MWArray[] argsOut, MWArray[] 
                              argsIn)
    {
      mcr.EvaluateFunction("WeibullAnalysisFromData", numArgsOut, ref argsOut, argsIn);
    }


    /// <summary>
    /// Provides a single output, 0-input MWArrayinterface to the WeibullAnalysisAll
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisAll()
    {
      return mcr.EvaluateFunction("WeibullAnalysisAll", new MWArray[]{});
    }


    /// <summary>
    /// Provides a single output, 1-input MWArrayinterface to the WeibullAnalysisAll
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="dbHost">Input argument #1</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisAll(MWArray dbHost)
    {
      return mcr.EvaluateFunction("WeibullAnalysisAll", dbHost);
    }


    /// <summary>
    /// Provides a single output, 2-input MWArrayinterface to the WeibullAnalysisAll
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="dbHost">Input argument #1</param>
    /// <param name="dbPort">Input argument #2</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisAll(MWArray dbHost, MWArray dbPort)
    {
      return mcr.EvaluateFunction("WeibullAnalysisAll", dbHost, dbPort);
    }


    /// <summary>
    /// Provides a single output, 3-input MWArrayinterface to the WeibullAnalysisAll
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="dbHost">Input argument #1</param>
    /// <param name="dbPort">Input argument #2</param>
    /// <param name="dbName">Input argument #3</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisAll(MWArray dbHost, MWArray dbPort, MWArray dbName)
    {
      return mcr.EvaluateFunction("WeibullAnalysisAll", dbHost, dbPort, dbName);
    }


    /// <summary>
    /// Provides a single output, 4-input MWArrayinterface to the WeibullAnalysisAll
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="dbHost">Input argument #1</param>
    /// <param name="dbPort">Input argument #2</param>
    /// <param name="dbName">Input argument #3</param>
    /// <param name="dbUser">Input argument #4</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisAll(MWArray dbHost, MWArray dbPort, MWArray dbName, 
                                MWArray dbUser)
    {
      return mcr.EvaluateFunction("WeibullAnalysisAll", dbHost, dbPort, dbName, dbUser);
    }


    /// <summary>
    /// Provides a single output, 5-input MWArrayinterface to the WeibullAnalysisAll
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="dbHost">Input argument #1</param>
    /// <param name="dbPort">Input argument #2</param>
    /// <param name="dbName">Input argument #3</param>
    /// <param name="dbUser">Input argument #4</param>
    /// <param name="dbPassword">Input argument #5</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisAll(MWArray dbHost, MWArray dbPort, MWArray dbName, 
                                MWArray dbUser, MWArray dbPassword)
    {
      return mcr.EvaluateFunction("WeibullAnalysisAll", dbHost, dbPort, dbName, dbUser, dbPassword);
    }


    /// <summary>
    /// Provides a single output, 6-input MWArrayinterface to the WeibullAnalysisAll
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="dbHost">Input argument #1</param>
    /// <param name="dbPort">Input argument #2</param>
    /// <param name="dbName">Input argument #3</param>
    /// <param name="dbUser">Input argument #4</param>
    /// <param name="dbPassword">Input argument #5</param>
    /// <param name="jdbcPath">Input argument #6</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray WeibullAnalysisAll(MWArray dbHost, MWArray dbPort, MWArray dbName, 
                                MWArray dbUser, MWArray dbPassword, MWArray jdbcPath)
    {
      return mcr.EvaluateFunction("WeibullAnalysisAll", dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath);
    }


    /// <summary>
    /// Provides the standard 0-input MWArray interface to the WeibullAnalysisAll MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisAll(int numArgsOut)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisAll", new MWArray[]{});
    }


    /// <summary>
    /// Provides the standard 1-input MWArray interface to the WeibullAnalysisAll MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="dbHost">Input argument #1</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisAll(int numArgsOut, MWArray dbHost)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisAll", dbHost);
    }


    /// <summary>
    /// Provides the standard 2-input MWArray interface to the WeibullAnalysisAll MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="dbHost">Input argument #1</param>
    /// <param name="dbPort">Input argument #2</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisAll(int numArgsOut, MWArray dbHost, MWArray dbPort)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisAll", dbHost, dbPort);
    }


    /// <summary>
    /// Provides the standard 3-input MWArray interface to the WeibullAnalysisAll MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="dbHost">Input argument #1</param>
    /// <param name="dbPort">Input argument #2</param>
    /// <param name="dbName">Input argument #3</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisAll(int numArgsOut, MWArray dbHost, MWArray dbPort, 
                                  MWArray dbName)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisAll", dbHost, dbPort, dbName);
    }


    /// <summary>
    /// Provides the standard 4-input MWArray interface to the WeibullAnalysisAll MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="dbHost">Input argument #1</param>
    /// <param name="dbPort">Input argument #2</param>
    /// <param name="dbName">Input argument #3</param>
    /// <param name="dbUser">Input argument #4</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisAll(int numArgsOut, MWArray dbHost, MWArray dbPort, 
                                  MWArray dbName, MWArray dbUser)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisAll", dbHost, dbPort, dbName, dbUser);
    }


    /// <summary>
    /// Provides the standard 5-input MWArray interface to the WeibullAnalysisAll MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="dbHost">Input argument #1</param>
    /// <param name="dbPort">Input argument #2</param>
    /// <param name="dbName">Input argument #3</param>
    /// <param name="dbUser">Input argument #4</param>
    /// <param name="dbPassword">Input argument #5</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisAll(int numArgsOut, MWArray dbHost, MWArray dbPort, 
                                  MWArray dbName, MWArray dbUser, MWArray dbPassword)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisAll", dbHost, dbPort, dbName, dbUser, dbPassword);
    }


    /// <summary>
    /// Provides the standard 6-input MWArray interface to the WeibullAnalysisAll MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="dbHost">Input argument #1</param>
    /// <param name="dbPort">Input argument #2</param>
    /// <param name="dbName">Input argument #3</param>
    /// <param name="dbUser">Input argument #4</param>
    /// <param name="dbPassword">Input argument #5</param>
    /// <param name="jdbcPath">Input argument #6</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] WeibullAnalysisAll(int numArgsOut, MWArray dbHost, MWArray dbPort, 
                                  MWArray dbName, MWArray dbUser, MWArray dbPassword, 
                                  MWArray jdbcPath)
    {
      return mcr.EvaluateFunction(numArgsOut, "WeibullAnalysisAll", dbHost, dbPort, dbName, dbUser, dbPassword, jdbcPath);
    }


    /// <summary>
    /// Provides an interface for the WeibullAnalysisAll function in which the input and
    /// output
    /// arguments are specified as an array of MWArrays.
    /// </summary>
    /// <remarks>
    /// This method will allocate and return by reference the output argument
    /// array.<newpara></newpara>
    /// M-Documentation:
    /// WEIBULLANALYSISALL 对所有活跃模组进行批量Weibull分析
    /// results = WeibullAnalysisAll() 使用默认数据库配置分析所有模组
    /// results = WeibullAnalysisAll(dbHost, dbPort, dbName, dbUser, dbPassword,
    /// jdbcPath)
    /// 使用指定的数据库连接参数
    /// 输入参数:
    /// dbHost - 数据库服务器地址 (可选，默认'localhost')
    /// dbPort - 端口号 (可选，默认3306)
    /// dbName - 数据库名称 (可选，默认'SysHardTestDB')
    /// dbUser - 用户名 (可选，默认'root')
    /// dbPassword - 密码 (可选)
    /// jdbcPath - JDBC驱动路径 (可选)
    /// 输出参数:
    /// results - 结构体数组，每个元素包含一个模组的分析结果
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return</param>
    /// <param name= "argsOut">Array of MWArray output arguments</param>
    /// <param name= "argsIn">Array of MWArray input arguments</param>
    ///
    public void WeibullAnalysisAll(int numArgsOut, ref MWArray[] argsOut, MWArray[] 
                         argsIn)
    {
      mcr.EvaluateFunction("WeibullAnalysisAll", numArgsOut, ref argsOut, argsIn);
    }



    /// <summary>
    /// This method will cause a MATLAB figure window to behave as a modal dialog box.
    /// The method will not return until all the figure windows associated with this
    /// component have been closed.
    /// </summary>
    /// <remarks>
    /// An application should only call this method when required to keep the
    /// MATLAB figure window from disappearing.  Other techniques, such as calling
    /// Console.ReadLine() from the application should be considered where
    /// possible.</remarks>
    ///
    public void WaitForFiguresToDie()
    {
      mcr.WaitForFiguresToDie();
    }



    #endregion Methods

    #region Class Members

    private static MWMCR mcr= null;

    private static Exception ex_= null;

    private bool disposed= false;

    #endregion Class Members
  }
}
