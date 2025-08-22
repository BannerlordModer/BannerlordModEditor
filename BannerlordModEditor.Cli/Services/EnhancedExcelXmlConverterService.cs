using System.Reflection;
using System.Linq;
using BannerlordModEditor.Common.Loaders;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DO.Game;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;
using BannerlordModEditor.Common.Services;
using ClosedXML.Excel;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using ActionTypesDTO = BannerlordModEditor.Common.Models.DTO.ActionTypesDTO;

namespace BannerlordModEditor.Cli.Services
{
    /// <summary>
    /// 增强的Excel和XML转换服务实现
    /// 支持完整的DO/DTO架构转换流程
    /// </summary>
    public class EnhancedExcelXmlConverterService : IExcelXmlConverterService
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly Dictionary<string, Type> _doModelTypes;
        private readonly Dictionary<string, Type> _dtoModelTypes;

        public EnhancedExcelXmlConverterService(IFileDiscoveryService fileDiscoveryService)
        {
            _fileDiscoveryService = fileDiscoveryService;
            _doModelTypes = LoadModelTypes("BannerlordModEditor.Common.Models.DO");
            _dtoModelTypes = LoadModelTypes("BannerlordModEditor.Common.Models.DTO");
            
            // 调试信息
            Console.WriteLine($"调试: 加载了 {_doModelTypes.Count} 个DO模型类型");
            Console.WriteLine($"调试: 加载了 {_dtoModelTypes.Count} 个DTO模型类型");
        }

        /// <summary>
        /// 加载指定命名空间下的所有模型类型
        /// </summary>
        private Dictionary<string, Type> LoadModelTypes(string nameSpace)
        {
            var modelTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            
            var assembly = Assembly.GetAssembly(typeof(ActionTypesDO));
            if (assembly != null)
            {
                var types = assembly.GetTypes()
                    .Where(t => t.Namespace == nameSpace && 
                               t.IsClass && 
                               !t.IsAbstract && 
                               t.GetCustomAttribute<XmlRootAttribute>() != null)
                    .ToList();

                foreach (var type in types)
                {
                    var xmlRootAttr = type.GetCustomAttribute<XmlRootAttribute>();
                    if (xmlRootAttr != null)
                    {
                        var modelName = xmlRootAttr.ElementName ?? type.Name;
                        modelTypes[modelName] = type;
                    }
                }
            }

            return modelTypes;
        }

        public async Task<bool> ConvertExcelToXmlAsync(string excelFilePath, string xmlFilePath, string modelType, string? worksheetName = null)
        {
            try
            {
                Console.WriteLine($"开始转换 Excel 到 XML: {excelFilePath} -> {xmlFilePath}");
                
                // 验证输入文件
                if (!File.Exists(excelFilePath))
                {
                    throw new FileNotFoundException("Excel 文件不存在", excelFilePath);
                }

                // 获取DO模型类型
                Console.WriteLine($"调试: 查找模型类型: {modelType}");
                // 将模型类型转换为XML根元素名称
                var xmlRootName = ConvertModelTypeToXmlRootName(modelType);
                Console.WriteLine($"调试: 转换后的XML根元素名称: {xmlRootName}");
                
                if (!_doModelTypes.TryGetValue(xmlRootName, out var doType))
                {
                    Console.WriteLine($"调试: 可用的DO模型类型: {string.Join(", ", _doModelTypes.Keys)}");
                    throw new ArgumentException($"不支持的DO模型类型: {xmlRootName}");
                }

                // 读取 Excel 数据
                var excelData = await ReadExcelAsync(excelFilePath, worksheetName);
                Console.WriteLine($"读取到 {excelData.Rows.Count} 行数据，{excelData.Headers.Count} 个列");
                
                // 转换为 DO 对象
                var doObject = ConvertExcelToDo(excelData, doType, modelType);
                Console.WriteLine($"Excel 转 DO 对象成功: {doObject.GetType().Name}");
                
                // 转换为 DTO 对象
                var dtoObject = ConvertDoToDto(doObject, modelType);
                Console.WriteLine($"DO 转 DTO 对象成功: {dtoObject.GetType().Name}");
                
                // 保存 XML 文件
                await SaveXmlAsync(dtoObject, xmlFilePath);
                Console.WriteLine($"XML 文件保存成功: {xmlFilePath}");
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excel 转 XML 失败: {ex.Message}");
                throw new ConversionException($"Excel 转 XML 失败: {ex.Message}", ex);
            }
        }

        public async Task<bool> ConvertXmlToExcelAsync(string xmlFilePath, string excelFilePath, string? worksheetName = null)
        {
            try
            {
                Console.WriteLine($"开始转换 XML 到 Excel: {xmlFilePath} -> {excelFilePath}");
                
                // 验证输入文件
                if (!File.Exists(xmlFilePath))
                {
                    throw new FileNotFoundException("XML 文件不存在", xmlFilePath);
                }

                // 识别 XML 格式
                var modelType = await RecognizeXmlFormatAsync(xmlFilePath);
                if (string.IsNullOrEmpty(modelType))
                {
                    throw new XmlFormatException("无法识别 XML 格式");
                }

                Console.WriteLine($"识别到模型类型: {modelType}");

                // 获取DO模型类型
                if (!_doModelTypes.TryGetValue(modelType, out var doType))
                {
                    throw new ArgumentException($"不支持的DO模型类型: {modelType}");
                }

                // 读取 XML 数据为 DTO 对象
                var dtoObject = await ReadXmlAsync(xmlFilePath, doType);
                Console.WriteLine($"XML 读取成功: {dtoObject.GetType().Name}");
                
                // 转换为 DO 对象
                var doObject = ConvertDtoToDo(dtoObject, modelType);
                Console.WriteLine($"DTO 转 DO 对象成功: {doObject.GetType().Name}");
                
                // 转换为 Excel 数据
                var excelData = ConvertDoToExcel(doObject, modelType);
                Console.WriteLine($"DO 转 Excel 数据成功: {excelData.Rows.Count} 行");
                
                // 保存 Excel 文件
                await SaveExcelAsync(excelData, excelFilePath, worksheetName);
                Console.WriteLine($"Excel 文件保存成功: {excelFilePath}");
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XML 转 Excel 失败: {ex.Message}");
                throw new ConversionException($"XML 转 Excel 失败: {ex.Message}", ex);
            }
        }

        public async Task<string?> RecognizeXmlFormatAsync(string xmlFilePath)
        {
            try
            {
                var xmlContent = await File.ReadAllTextAsync(xmlFilePath);
                var doc = XDocument.Parse(xmlContent);
                
                if (doc.Root == null)
                {
                    throw new XmlFormatException("XML 文件没有根元素");
                }

                var rootName = doc.Root.Name.LocalName;
                
                // 查找匹配的DO模型类型
                foreach (var kvp in _doModelTypes)
                {
                    var xmlRootAttr = kvp.Value.GetCustomAttribute<XmlRootAttribute>();
                    if (xmlRootAttr != null && 
                        (xmlRootAttr.ElementName?.Equals(rootName, StringComparison.OrdinalIgnoreCase) ?? false))
                    {
                        return kvp.Key;
                    }
                }

                // 如果没有直接匹配，尝试基于命名约定匹配
                var convertedName = _fileDiscoveryService.ConvertToModelName(rootName);
                if (_doModelTypes.TryGetValue(convertedName, out var matchedType))
                {
                    return convertedName;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new XmlFormatException($"XML 格式识别失败: {ex.Message}", ex);
            }
        }

        public async Task<bool> ValidateExcelFormatAsync(string excelFilePath, string modelType, string? worksheetName = null)
        {
            try
            {
                if (!_doModelTypes.TryGetValue(modelType, out var targetType))
                {
                    throw new ArgumentException($"不支持的模型类型: {modelType}");
                }

                var excelData = await ReadExcelAsync(excelFilePath, worksheetName);
                
                Console.WriteLine($"验证Excel格式: {excelData.Headers.Count} 个列, {excelData.Rows.Count} 行");
                
                // 验证表头是否符合模型结构
                return ValidateExcelHeaders(excelData.Headers, targetType);
            }
            catch (Exception ex)
            {
                throw new ExcelOperationException($"Excel 格式验证失败: {ex.Message}", ex);
            }
        }

        private async Task<ExcelData> ReadExcelAsync(string filePath, string? worksheetName = null)
        {
            try
            {
                Console.WriteLine($"正在读取Excel文件: {filePath}");
                using var workbook = new XLWorkbook(filePath);
                IXLWorksheet worksheet;
                
                if (string.IsNullOrEmpty(worksheetName))
                {
                    worksheet = workbook.Worksheets.First();
                }
                else
                {
                    worksheet = workbook.Worksheets.Worksheet(worksheetName);
                    if (worksheet == null)
                    {
                        throw new ExcelOperationException($"工作表 '{worksheetName}' 不存在");
                    }
                }

                Console.WriteLine($"使用工作表: {worksheet.Name}");

                var excelData = new ExcelData
                {
                    WorksheetName = worksheet.Name,
                    Headers = new List<string>(),
                    Rows = new List<Dictionary<string, object?>>()
                };

                // 读取表头
                var headerRow = worksheet.Row(1);
                foreach (var cell in headerRow.Cells())
                {
                    var headerValue = cell.Value.ToString()?.Trim() ?? $"Column_{cell.Address.ColumnNumber}";
                    excelData.Headers.Add(headerValue);
                }

                // 读取数据行
                var dataRows = worksheet.RowsUsed().Skip(1); // 跳过表头
                foreach (var row in dataRows)
                {
                    var rowData = new Dictionary<string, object?>();
                    for (int i = 0; i < excelData.Headers.Count; i++)
                    {
                        var cell = row.Cell(i + 1);
                        rowData[excelData.Headers[i]] = GetCellValue(cell);
                    }
                    excelData.Rows.Add(rowData);
                }

                Console.WriteLine($"成功读取Excel文件: {excelData.Headers.Count} 列, {excelData.Rows.Count} 行");
                return excelData;
            }
            catch (Exception ex)
            {
                throw new ExcelOperationException($"读取 Excel 文件失败: {ex.Message}", ex);
            }
        }

        private object? GetCellValue(IXLCell cell)
        {
            if (cell.IsEmpty())
            {
                return null;
            }

            try
            {
                return cell.Value;
            }
            catch
            {
                return cell.GetString();
            }
        }

        private object ConvertExcelToDo(ExcelData excelData, Type doType, string modelType)
        {
            try
            {
                return ModelTypeConverter.ConvertExcelToModel(excelData, doType);
            }
            catch (Exception ex)
            {
                throw new ConversionException($"Excel 转 DO 对象失败: {ex.Message}", ex);
            }
        }

        private object ConvertDoToDto(object doObject, string modelType)
        {
            try
            {
                // 使用反射调用对应的Mapper
                var mapperClassName = ConvertModelTypeToMapperClassName(modelType);
                var mapperTypeName = $"BannerlordModEditor.Common.Mappers.{mapperClassName}";
                Console.WriteLine($"调试: DO转DTO查找Mapper类型: {mapperTypeName}");
                
                // 获取Common程序集
                var commonAssembly = Assembly.GetAssembly(typeof(ActionTypesDO));
                if (commonAssembly == null)
                {
                    throw new InvalidOperationException("无法获取Common程序集");
                }
                
                var mapperType = commonAssembly.GetType(mapperTypeName);
                
                if (mapperType != null)
                {
                    Console.WriteLine($"调试: DO转DTO找到Mapper类型: {mapperType.Name}");
                    var toDtoMethod = mapperType.GetMethod("ToDTO", new[] { doObject.GetType() });
                    if (toDtoMethod != null)
                    {
                        Console.WriteLine($"调试: DO转DTO找到ToDTO方法");
                        return toDtoMethod.Invoke(null, new[] { doObject })!;
                    }
                    else
                    {
                        Console.WriteLine($"调试: DO转DTO未找到ToDTO方法");
                    }
                }
                else
                {
                    Console.WriteLine($"调试: DO转DTO未找到Mapper类型");
                }

                throw new InvalidOperationException($"找不到 {modelType} 的 Mapper");
            }
            catch (Exception ex)
            {
                throw new ConversionException($"DO 转 DTO 对象失败: {ex.Message}", ex);
            }
        }

        private object ConvertDtoToDo(object dtoObject, string modelType)
        {
            try
            {
                // 使用反射调用对应的Mapper
                var mapperClassName = ConvertModelTypeToMapperClassName(modelType);
                var mapperTypeName = $"BannerlordModEditor.Common.Mappers.{mapperClassName}";
                Console.WriteLine($"调试: 查找Mapper类型: {mapperTypeName}");
                
                // 获取Common程序集
                var commonAssembly = Assembly.GetAssembly(typeof(ActionTypesDO));
                if (commonAssembly == null)
                {
                    throw new InvalidOperationException("无法获取Common程序集");
                }
                
                var mapperType = commonAssembly.GetType(mapperTypeName);
                
                if (mapperType != null)
                {
                    Console.WriteLine($"调试: 找到Mapper类型: {mapperType.Name}");
                    Console.WriteLine($"调试: dtoObject类型: {dtoObject.GetType().FullName}");
                    
                    // 直接调用ToDo方法 - 简化实现
                    var methods = mapperType.GetMethods(BindingFlags.Public | BindingFlags.Static);
                    Console.WriteLine($"调试: Mapper类型 {mapperType.Name} 有 {methods.Length} 个方法");
                    foreach (var method in methods)
                    {
                        Console.WriteLine($"调试: 方法 {method.Name}, 参数数量: {method.GetParameters().Length}");
                        if (method.GetParameters().Length > 0)
                        {
                            Console.WriteLine($"调试: 参数类型: {method.GetParameters()[0].ParameterType.FullName}");
                        }
                    }
                    MethodInfo toDoMethod = null;
                    foreach (var method in methods)
                    {
                        Console.WriteLine($"调试: 检查方法 '{method.Name}' (长度: {method.Name.Length})");
                        if (method.Name == "ToDo" && method.GetParameters().Length == 1)
                        {
                            Console.WriteLine($"调试: 找到匹配的方法!");
                            toDoMethod = method;
                            break;
                        }
                        else if (method.Name.ToLower() == "todo" && method.GetParameters().Length == 1)
                        {
                            Console.WriteLine($"调试: 找到小写匹配的方法!");
                            toDoMethod = method;
                            break;
                        }
                    }
                    
                    Console.WriteLine($"调试: 查找ToDo方法结果: {toDoMethod != null}");
                    Console.WriteLine($"调试: 方法列表:");
                    foreach (var method in methods)
                    {
                        Console.WriteLine($"调试:  - {method.Name} (参数数量: {method.GetParameters().Length})");
                        if (method.Name == "ToDo" && method.GetParameters().Length == 1)
                        {
                            Console.WriteLine($"调试:    找到匹配的ToDo方法!");
                        }
                    }
                    if (toDoMethod != null)
                    {
                        Console.WriteLine($"调试: 找到的方法参数类型: {toDoMethod.GetParameters()[0].ParameterType.FullName}");
                        Console.WriteLine($"调试: dtoObject类型: {dtoObject.GetType().FullName}");
                        Console.WriteLine($"调试: 类型比较: {toDoMethod.GetParameters()[0].ParameterType.FullName == dtoObject.GetType().FullName}");
                    }
                    
                    if (toDoMethod != null)
                    {
                        Console.WriteLine($"调试: 找到ToDo方法，调用转换");
                        return toDoMethod.Invoke(null, new[] { dtoObject })!;
                    }
                    else
                    {
                        Console.WriteLine($"调试: 未找到ToDo方法，尝试直接调用");
                        // 直接尝试调用 - 动态调用对应的Mapper
                        try
                        {
                            // 使用动态调用，根据mapperType调用对应的ToDo方法
                            var toDoMethodGeneric = mapperType.GetMethod("ToDo", new[] { dtoObject.GetType() });
                            if (toDoMethodGeneric != null)
                            {
                                var result = toDoMethodGeneric.Invoke(null, new[] { dtoObject });
                                Console.WriteLine($"调试: 动态调用成功");
                                return result!;
                            }
                            else
                            {
                                throw new InvalidOperationException($"找不到适合 {dtoObject.GetType().Name} 的 ToDo 方法");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"调试: 动态调用失败: {ex.Message}");
                            throw new InvalidOperationException($"找不到 {modelType} 的 ToDo 方法");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"调试: 未找到Mapper类型");
                }

                throw new InvalidOperationException($"找不到 {modelType} 的 Mapper");
            }
            catch (Exception ex)
            {
                throw new ConversionException($"DTO 转 DO 对象失败: {ex.Message}", ex);
            }
        }

        private ExcelData ConvertDoToExcel(object doObject, string modelType)
        {
            try
            {
                return ModelTypeConverter.ConvertModelToExcel(doObject, modelType);
            }
            catch (Exception ex)
            {
                throw new ConversionException($"DO 对象转 Excel 失败: {ex.Message}", ex);
            }
        }

        private async Task<object> ReadXmlAsync(string filePath, Type targetType)
        {
            try
            {
                // 获取DO类型的XML根元素名称
                var xmlRootAttr = targetType.GetCustomAttribute<XmlRootAttribute>();
                var xmlRootName = xmlRootAttr?.ElementName;
                
                Console.WriteLine($"调试: DO类型 {targetType.Name} -> XML根元素 {xmlRootName}");
                
                // 在DTO类型中查找对应的类型
                var dtoType = _dtoModelTypes.Values.FirstOrDefault(t => 
                {
                    var dtoXmlRootAttr = t.GetCustomAttribute<XmlRootAttribute>();
                    var dtoXmlRootName = dtoXmlRootAttr?.ElementName;
                    return dtoXmlRootName == xmlRootName;
                });
                
                Console.WriteLine($"调试: 找到的DTO类型: {dtoType?.Name ?? "null"}");
                
                if (dtoType == null)
                {
                    Console.WriteLine($"调试: 可用的DTO类型: {string.Join(", ", _dtoModelTypes.Keys)}");
                    throw new InvalidOperationException($"找不到对应的DTO类型，XML根元素: {xmlRootName}");
                }

                // 使用GenericXmlLoader读取DTO对象
                var loaderType = typeof(GenericXmlLoader<>).MakeGenericType(dtoType);
                var loader = Activator.CreateInstance(loaderType);
                
                if (loader == null)
                {
                    throw new InvalidOperationException($"无法创建 GenericXmlLoader<{dtoType.Name}> 的实例");
                }
                
                var loadMethod = loaderType.GetMethod("LoadAsync", new[] { typeof(string) });
                if (loadMethod == null)
                {
                    throw new InvalidOperationException($"GenericXmlLoader<{dtoType.Name}> 没有 LoadAsync 方法");
                }
                
                var task = (Task)loadMethod.Invoke(loader, new object[] { filePath })!;
                await task.ConfigureAwait(false);
                
                var resultProperty = task.GetType().GetProperty("Result");
                return resultProperty?.GetValue(task) ?? throw new XmlFormatException("无法读取 XML 文件");
            }
            catch (Exception ex)
            {
                throw new XmlFormatException($"读取 XML 文件失败: {ex.Message}", ex);
            }
        }

        private async Task SaveXmlAsync(object xmlObject, string filePath)
        {
            try
            {
                var objectType = xmlObject.GetType();
                var loaderType = typeof(GenericXmlLoader<>).MakeGenericType(objectType);
                var loader = Activator.CreateInstance(loaderType);
                
                if (loader == null)
                {
                    throw new InvalidOperationException($"无法创建 GenericXmlLoader<{objectType.Name}> 的实例");
                }
                
                var saveMethod = loaderType.GetMethod("Save", new[] { objectType, typeof(string), typeof(string) });
                if (saveMethod == null)
                {
                    throw new InvalidOperationException($"GenericXmlLoader<{objectType.Name}> 没有 Save 方法");
                }
                
                saveMethod.Invoke(loader, new object[] { xmlObject, filePath, (string?)null });
            }
            catch (Exception ex)
            {
                throw new XmlFormatException($"保存 XML 文件失败: {ex.Message}", ex);
            }
        }

        private async Task SaveExcelAsync(ExcelData excelData, string filePath, string? worksheetName = null)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var wsName = worksheetName ?? excelData.WorksheetName ?? "Sheet1";
                var worksheet = workbook.Worksheets.Add(wsName);

                // 写入表头
                for (int i = 0; i < excelData.Headers.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = excelData.Headers[i];
                }

                // 写入数据
                for (int row = 0; row < excelData.Rows.Count; row++)
                {
                    for (int col = 0; col < excelData.Headers.Count; col++)
                    {
                        var header = excelData.Headers[col];
                        var value = excelData.Rows[row][header];
                        worksheet.Cell(row + 2, col + 1).Value = value?.ToString();
                    }
                }

                workbook.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                throw new ExcelOperationException($"保存 Excel 文件失败: {ex.Message}", ex);
            }
        }

        private bool ValidateExcelHeaders(List<string> headers, Type targetType)
        {
            // 这里需要根据具体的模型类型验证表头
            // 这是一个简化的实现，实际使用时需要根据不同的模型类型进行具体的验证
            
            return true; // 简化实现，总是返回 true
        }

        /// <summary>
        /// 将模型类型（如 attributes）转换为XML根元素名称（如 ArrayOfAttributeData）
        /// </summary>
        private string ConvertModelTypeToXmlRootName(string modelType)
        {
            if (string.IsNullOrEmpty(modelType))
                return string.Empty;
            
            // 已知的映射关系
            var mappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["action_types"] = "action_types",
                ["combat_parameters"] = "combat_parameters",
                ["map_icons"] = "map_icons",
                ["attributes"] = "ArrayOfAttributeData",
                ["ArrayOfAttributeData"] = "ArrayOfAttributeData",
                ["crafting_pieces"] = "base",
                ["item_modifiers"] = "item_modifiers",
                ["skills"] = "skills"
            };
            
            if (mappings.TryGetValue(modelType, out var xmlRootName))
            {
                return xmlRootName;
            }
            
            // 如果没有找到映射，直接返回原名称
            return modelType;
        }

        /// <summary>
        /// 将模型类型（如 attributes）转换为DO类型名称（如 AttributesDO）
        /// </summary>
        private string ConvertModelTypeToDoTypeName(string modelType)
        {
            if (string.IsNullOrEmpty(modelType))
                return string.Empty;
            
            // 已知的映射关系
            var mappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["action_types"] = nameof(ActionTypesDO),
                ["combat_parameters"] = nameof(CombatParametersDO),
                ["map_icons"] = nameof(MapIconsDO),
                ["attributes"] = nameof(AttributesDO),
                ["ArrayOfAttributeData"] = nameof(AttributesDO),
                ["crafting_pieces"] = nameof(CraftingPiecesDO),
                ["item_modifiers"] = nameof(ItemModifiersDO),
                ["skills"] = nameof(SkillsDO)
            };
            
            if (mappings.TryGetValue(modelType, out var doTypeName))
            {
                return doTypeName;
            }
            
            // 如果没有找到映射，尝试使用命名约定转换
            // 例如：action_types -> ActionTypesDO
            var pascalCase = System.Text.RegularExpressions.Regex.Replace(modelType, @"_(\w)", m => m.Groups[1].Value.ToUpper());
            pascalCase = char.ToUpper(pascalCase[0]) + pascalCase.Substring(1);
            return pascalCase + "DO";
        }

        /// <summary>
        /// 将模型类型转换为Mapper类名
        /// 例如：action_types -> ActionTypesMapper
        /// </summary>
        private string ConvertModelTypeToMapperClassName(string modelType)
        {
            if (string.IsNullOrEmpty(modelType))
                return string.Empty;

            // 使用FileDiscoveryService的ConvertToModelName方法将下划线命名转换为PascalCase
            var pascalCaseName = _fileDiscoveryService.ConvertToModelName(modelType);
            var mapperName = $"{pascalCaseName}Mapper";
            Console.WriteLine($"调试: 模型类型 {modelType} -> Mapper类名 {mapperName}");
            return mapperName;
        }
    }
}