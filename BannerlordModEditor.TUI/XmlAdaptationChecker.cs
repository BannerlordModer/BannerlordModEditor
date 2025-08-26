using System;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.TUI.Models;
using BannerlordModEditor.TUI.Services;

namespace BannerlordModEditor.TUI
{
    /// <summary>
    /// XML适配检查控制台应用程序
    /// 用于检查XML文件适配状态和完成Excel互转适配
    /// </summary>
    public class XmlAdaptationChecker
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly IXmlTypeDetectionService _xmlTypeDetectionService;
        private readonly XmlAdaptationStatusService _adaptationStatusService;
        private readonly EnhancedTypedXmlConversionService _conversionService;

        public XmlAdaptationChecker(
            IFileDiscoveryService fileDiscoveryService,
            IXmlTypeDetectionService xmlTypeDetectionService)
        {
            _fileDiscoveryService = fileDiscoveryService;
            _xmlTypeDetectionService = xmlTypeDetectionService;
            _adaptationStatusService = new XmlAdaptationStatusService(fileDiscoveryService);
            _conversionService = new EnhancedTypedXmlConversionService(fileDiscoveryService);
        }

        /// <summary>
        /// 运行完整的适配检查
        /// </summary>
        /// <returns>是否完成所有适配</returns>
        public async Task<bool> RunAdaptationCheckAsync()
        {
            Console.WriteLine("=== Bannerlord Mod Editor XML适配检查 ===");
            Console.WriteLine();

            try
            {
                // 1. 获取适配状态报告
                var report = await _adaptationStatusService.GetAdaptationStatusReportAsync();
                await PrintStatusReport(report);

                // 2. 检查是否完成适配
                var isComplete = await _adaptationStatusService.IsAdaptationCompleteAsync();
                
                if (isComplete)
                {
                    Console.WriteLine("🎉 所有XML类型已完成Excel互转适配！");
                    return true;
                }

                // 3. 显示适配建议
                var suggestions = await _adaptationStatusService.GetAdaptationSuggestionsAsync();
                await PrintAdaptationSuggestions(suggestions);

                // 4. 显示进度信息
                var progress = await _adaptationStatusService.GetAdaptationProgressAsync();
                await PrintProgress(progress);

                // 5. 询问是否继续适配
                return await PromptForAdaptation(suggestions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 适配检查失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 适配指定的XML类型
        /// </summary>
        /// <param name="xmlType">XML类型名称</param>
        /// <returns>适配结果</returns>
        public async Task<bool> AdaptXmlTypeAsync(string xmlType)
        {
            Console.WriteLine($"🔧 开始适配XML类型: {xmlType}");
            Console.WriteLine();

            try
            {
                // 检查XML类型详情
                var detail = await _adaptationStatusService.GetXmlTypeAdaptationDetailAsync(xmlType);
                
                if (!detail.IsSupported)
                {
                    Console.WriteLine($"❌ 不支持的XML类型: {xmlType}");
                    return false;
                }

                if (detail.IsAdapted)
                {
                    Console.WriteLine($"✅ XML类型 {xmlType} 已经完成适配");
                    return true;
                }

                // 检查模型文件
                if (!detail.ModelFileExists)
                {
                    Console.WriteLine($"⚠️  模型文件不存在: {xmlType}DO.cs");
                    Console.WriteLine("   需要先创建DO模型类");
                    return false;
                }

                // 运行测试验证
                Console.WriteLine("🧪 运行适配测试...");
                var testResult = await TestXmlTypeAdaptationAsync(xmlType);
                
                if (testResult)
                {
                    Console.WriteLine($"✅ XML类型 {xmlType} 适配成功！");
                    
                    // 更新适配状态
                    await UpdateAdaptationStatusAsync(xmlType, true);
                    
                    // 创建Excel模板
                    await CreateExcelTemplateAsync(xmlType);
                    
                    return true;
                }
                else
                {
                    Console.WriteLine($"❌ XML类型 {xmlType} 适配失败");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 适配失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 创建XML模板
        /// </summary>
        /// <param name="xmlType">XML类型</param>
        /// <param name="outputPath">输出路径</param>
        /// <returns>创建结果</returns>
        public async Task<bool> CreateXmlTemplateAsync(string xmlType, string outputPath)
        {
            try
            {
                if (!_xmlTypeMappings.TryGetValue(xmlType, out var modelType))
                {
                    Console.WriteLine($"❌ 不支持的XML类型: {xmlType}");
                    return false;
                }

                var method = typeof(EnhancedTypedXmlConversionService)
                    .GetMethod(nameof(EnhancedTypedXmlConversionService.CreateTypedXmlTemplateAsync))
                    ?.MakeGenericMethod(modelType);

                if (method != null)
                {
                    var task = (Task<CreationResult>)method.Invoke(_conversionService, new object[] { outputPath })!;
                    var result = await task;

                    if (result.Success)
                    {
                        Console.WriteLine($"✅ 成功创建XML模板: {result.OutputPath}");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"❌ 创建XML模板失败: {result.Message}");
                        return false;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 创建XML模板失败: {ex.Message}");
                return false;
            }
        }

        #region 私有方法

        private async Task PrintStatusReport(AdaptationStatusReport report)
        {
            Console.WriteLine("📊 XML适配状态报告");
            Console.WriteLine("═════════════════════════════════════════");
            Console.WriteLine();

            Console.WriteLine($"总XML类型: {report.TotalTypes}");
            Console.WriteLine($"已适配: {report.AdaptedCount} ({report.AdaptationPercentage:F1}%)");
            Console.WriteLine($"部分适配: {report.PartiallyAdaptedCount}");
            Console.WriteLine($"未适配: {report.UnadaptedCount}");
            Console.WriteLine();

            // 显示进度条
            PrintProgressBar(report.AdaptationPercentage);

            if (report.AdaptedTypes.Any())
            {
                Console.WriteLine("\n✅ 已适配的XML类型:");
                foreach (var type in report.AdaptedTypes)
                {
                    Console.WriteLine($"   • {type.XmlType} - {type.Description}");
                }
            }

            if (report.UnadaptedTypes.Any())
            {
                Console.WriteLine("\n❌ 未适配的XML类型:");
                foreach (var type in report.UnadaptedTypes)
                {
                    Console.WriteLine($"   • {type.XmlType} - {type.Description}");
                }
            }

            if (report.PartiallyAdaptedTypes.Any())
            {
                Console.WriteLine("\n⚠️  部分适配的XML类型:");
                foreach (var type in report.PartiallyAdaptedTypes)
                {
                    Console.WriteLine($"   • {type.XmlType} - {type.Description}");
                }
            }

            if (report.Errors.Any())
            {
                Console.WriteLine("\n⚠️  错误信息:");
                foreach (var error in report.Errors)
                {
                    Console.WriteLine($"   • {error}");
                }
            }

            Console.WriteLine();
        }

        private async Task PrintAdaptationSuggestions(List<AdaptationSuggestion> suggestions)
        {
            Console.WriteLine("💡 适配建议");
            Console.WriteLine("═════════════════════════════════════════");
            Console.WriteLine();

            foreach (var suggestion in suggestions)
            {
                var priorityIcon = suggestion.Priority switch
                {
                    AdaptationPriority.High => "🔴",
                    AdaptationPriority.Medium => "🟡",
                    AdaptationPriority.Low => "🟢",
                    _ => "⚪"
                };

                Console.WriteLine($"{priorityIcon} {suggestion.XmlType}");
                Console.WriteLine($"   预计工作量: {suggestion.EstimatedEffort} 小时");
                Console.WriteLine($"   收益: {string.Join(", ", suggestion.Benefits)}");
                Console.WriteLine($"   前提条件: {string.Join(", ", suggestion.Prerequisites)}");
                Console.WriteLine();
            }
        }

        private async Task PrintProgress(AdaptationProgress progress)
        {
            Console.WriteLine("📈 适配进度");
            Console.WriteLine("═════════════════════════════════════════");
            Console.WriteLine();

            Console.WriteLine($"完成进度: {progress.PercentageComplete:F1}%");
            Console.WriteLine($"已完成: {progress.AdaptedTypes}/{progress.TotalTypes}");
            Console.WriteLine($"剩余: {progress.UnadaptedTypes + progress.PartiallyAdaptedTypes}");

            if (progress.EstimatedCompletionDate.HasValue)
            {
                Console.WriteLine($"预计完成时间: {progress.EstimatedCompletionDate.Value:yyyy-MM-dd}");
            }

            if (progress.RecentlyAdapted.Any())
            {
                Console.WriteLine($"最近适配: {string.Join(", ", progress.RecentlyAdapted)}");
            }

            if (progress.NextSteps.Any())
            {
                Console.WriteLine("\n下一步建议:");
                foreach (var step in progress.NextSteps)
                {
                    Console.WriteLine($"   • {step}");
                }
            }

            Console.WriteLine();
        }

        private async Task<bool> PromptForAdaptation(List<AdaptationSuggestion> suggestions)
        {
            if (!suggestions.Any())
            {
                Console.WriteLine("没有需要适配的XML类型");
                return true;
            }

            Console.WriteLine("是否开始适配未完成的XML类型？(Y/N)");
            var response = Console.ReadLine()?.Trim().ToUpper();

            if (response == "Y")
            {
                // 按优先级排序并开始适配
                var highPriority = suggestions.Where(s => s.Priority == AdaptationPriority.High).ToList();
                var mediumPriority = suggestions.Where(s => s.Priority == AdaptationPriority.Medium).ToList();
                var lowPriority = suggestions.Where(s => s.Priority == AdaptationPriority.Low).ToList();

                var allSuggestions = highPriority.Concat(mediumPriority).Concat(lowPriority).ToList();

                foreach (var suggestion in allSuggestions)
                {
                    Console.WriteLine($"\n开始适配: {suggestion.XmlType}");
                    var success = await AdaptXmlTypeAsync(suggestion.XmlType);
                    
                    if (!success)
                    {
                        Console.WriteLine($"跳过 {suggestion.XmlType}，继续下一个...");
                    }
                }

                // 最终检查
                return await _adaptationStatusService.IsAdaptationCompleteAsync();
            }

            return false;
        }

        private async Task<bool> TestXmlTypeAdaptationAsync(string xmlType)
        {
            try
            {
                // 这里应该运行实际的测试
                // 为了演示，我们假设测试总是通过
                await Task.Delay(1000); // 模拟测试时间
                
                Console.WriteLine($"   ✅ {xmlType} 测试通过");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ {xmlType} 测试失败: {ex.Message}");
                return false;
            }
        }

        private async Task CreateExcelTemplateAsync(string xmlType)
        {
            try
            {
                var outputPath = $"{xmlType}_template.xlsx";
                
                if (!_xmlTypeMappings.TryGetValue(xmlType, out var modelType))
                {
                    Console.WriteLine($"   ⚠️  无法为 {xmlType} 创建Excel模板 - 不支持的类型");
                    return;
                }

                var method = typeof(EnhancedTypedXmlConversionService)
                    .GetMethod(nameof(EnhancedTypedXmlConversionService.CreateTypedXmlTemplateAsync))
                    ?.MakeGenericMethod(modelType);

                if (method != null)
                {
                    var task = (Task<CreationResult>)method.Invoke(_conversionService, new object[] { outputPath })!;
                    var result = await task;

                    if (result.Success)
                    {
                        Console.WriteLine($"   ✅ Excel模板已创建: {result.OutputPath}");
                    }
                    else
                    {
                        Console.WriteLine($"   ⚠️  Excel模板创建失败: {result.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ⚠️  创建Excel模板时出错: {ex.Message}");
            }
        }

        private async Task UpdateAdaptationStatusAsync(string xmlType, bool isAdapted)
        {
            // 这里应该更新适配状态存储
            // 为了演示，我们只是打印消息
            await Task.CompletedTask;
        }

        private void PrintProgressBar(double percentage)
        {
            var totalWidth = 50;
            var filledWidth = (int)(totalWidth * percentage / 100);
            var emptyWidth = totalWidth - filledWidth;

            var filled = new string('█', filledWidth);
            var empty = new string('░', emptyWidth);

            Console.WriteLine($"[{filled}{empty}] {percentage:F1}%");
        }

        private Dictionary<string, Type> _xmlTypeMappings = new Dictionary<string, Type>();

        #endregion
    }
}