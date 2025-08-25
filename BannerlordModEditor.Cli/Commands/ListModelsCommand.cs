using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using BannerlordModEditor.Cli.Services;
using System.Reflection;

namespace BannerlordModEditor.Cli.Commands
{
    /// <summary>
    /// 列出支持的模型类型
    /// </summary>
    [Command("list-models", Description = "列出所有支持的模型类型")]
    public class ListModelsCommand : ICommand
    {
        private readonly IExcelXmlConverterService _converterService;

        public ListModelsCommand(IExcelXmlConverterService converterService)
        {
            _converterService = converterService;
        }

        [CommandOption("search", 's', Description = "搜索关键词")]
        public string? SearchTerm { get; set; }

        [CommandOption("verbose", Description = "显示详细信息")]
        public bool Verbose { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
        {
            try
            {
                // 使用反射获取所有支持的模型类型
                var assembly = Assembly.GetAssembly(typeof(BannerlordModEditor.Common.Models.DO.ActionTypesDO));
                if (assembly != null)
                {
                    var modelTypes = assembly.GetTypes()
                        .Where(t => t.Namespace == "BannerlordModEditor.Common.Models.DO" && 
                                   t.IsClass && 
                                   !t.IsAbstract && 
                                   t.GetCustomAttribute<System.Xml.Serialization.XmlRootAttribute>() != null)
                        .ToList();

                    console.Output.WriteLine("支持的模型类型:");
                    console.Output.WriteLine(new string('-', 60));

                    var filteredTypes = string.IsNullOrEmpty(SearchTerm) 
                        ? modelTypes 
                        : modelTypes.Where(t => t.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

                    if (!filteredTypes.Any())
                    {
                        console.Output.WriteLine($"没有找到匹配的模型类型: {SearchTerm}");
                        return ValueTask.CompletedTask;
                    }

                    foreach (var type in filteredTypes.OrderBy(t => t.Name))
                    {
                        var xmlRootAttr = type.GetCustomAttribute<System.Xml.Serialization.XmlRootAttribute>();
                        var elementName = xmlRootAttr?.ElementName ?? type.Name;
                        
                        console.Output.WriteLine($"- {type.Name}");
                        
                        if (Verbose)
                        {
                            console.Output.WriteLine($"  XML 根元素: {elementName}");
                            console.Output.WriteLine($"  命名空间: {type.Namespace}");
                            
                            // 显示主要属性
                            var properties = type.GetProperties()
                                .Where(p => p.DeclaringType == type)
                                .OrderBy(p => p.Name)
                                .ToList();
                            
                            if (properties.Any())
                            {
                                console.Output.WriteLine("  主要属性:");
                                foreach (var prop in properties.Take(5)) // 只显示前5个属性
                                {
                                    console.Output.WriteLine($"    - {prop.Name}: {prop.PropertyType.Name}");
                                }
                                
                                if (properties.Count > 5)
                                {
                                    console.Output.WriteLine($"    ... 还有 {properties.Count - 5} 个属性");
                                }
                            }
                            
                            console.Output.WriteLine();
                        }
                    }

                    console.Output.WriteLine(new string('-', 60));
                    console.Output.WriteLine($"总计: {filteredTypes.Count} 个模型类型");
                    
                    if (!string.IsNullOrEmpty(SearchTerm))
                    {
                        console.Output.WriteLine($"搜索结果: {SearchTerm}");
                    }
                }
                else
                {
                    console.Error.WriteLine("错误：无法加载模型类型");
                }
            }
            catch (Exception ex)
            {
                console.Error.WriteLine($"错误：{ex.Message}");
            }

            return ValueTask.CompletedTask;
        }
    }
}