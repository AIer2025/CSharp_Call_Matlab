function result = WeibullAnalysisFromData(failureTimes, censoringTypes, quantities, lastInspectionTimes, moduleCode, moduleName)
%WEIBULLANALYSISFROMDATA 直接使用数据数组进行Weibull分析
%   不需要访问数据库，直接传入失效数据进行分析
%
%   输入参数:
%       failureTimes - 失效时间数组 (double[])
%       censoringTypes - 删失类型数组 (double[]) 
%                        0=完全数据, 1=右删失, 2=区间删失, 3=左删失
%       quantities - 数量数组 (double[])，可选，默认全1
%       lastInspectionTimes - 上次检查时间数组 (double[])，区间删失时需要
%       moduleCode - 模组代码 (string)，可选
%       moduleName - 模组名称 (string)，可选
%
%   输出参数:
%       result - 结构体，包含分析结果
%
%   示例:
%       % 简单完全数据分析
%       times = [100, 200, 300, 400, 500];
%       result = WeibullAnalysisFromData(times);
%
%       % 混合删失数据分析
%       times = [100, 200, 300, 400, 500];
%       censorTypes = [0, 0, 1, 0, 2];  % 0=完全, 1=右删失, 2=区间删失
%       quantities = [1, 1, 1, 1, 1];
%       lastInsp = [0, 0, 0, 0, 350];   % 区间删失的下界
%       result = WeibullAnalysisFromData(times, censorTypes, quantities, lastInsp);

    % 处理可选参数
    if nargin < 2 || isempty(censoringTypes)
        censoringTypes = zeros(size(failureTimes));
    end
    
    if nargin < 3 || isempty(quantities)
        quantities = ones(size(failureTimes));
    end
    
    if nargin < 4 || isempty(lastInspectionTimes)
        lastInspectionTimes = zeros(size(failureTimes));
    end
    
    if nargin < 5 || isempty(moduleCode)
        moduleCode = 'DirectInput';
    end
    
    if nargin < 6 || isempty(moduleName)
        moduleName = '直接输入数据';
    end
    
    lib = WeibullAnalysisLib();
    result = lib.WeibullAnalysisFromData(failureTimes, censoringTypes, quantities, ...
        lastInspectionTimes, moduleCode, moduleName);
end
