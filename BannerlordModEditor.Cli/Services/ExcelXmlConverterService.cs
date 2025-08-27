using System.Reflection;
using BannerlordModEditor.Common.Loaders;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Services;
using ClosedXML.Excel;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace BannerlordModEditor.Cli.Services
{
    /// <summary>
    /// Excel 和 XML 转换服务的实现
    /// </summary>
    public class ExcelXmlConverterService : IExcelXmlConverterService
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly Dictionary<string, Type> _modelTypes;

        public ExcelXmlConverterService(IFileDiscoveryService fileDiscoveryService)
        {
            _fileDiscoveryService = fileDiscoveryService;
            _modelTypes = LoadModelTypes();
        }

        /// <summary>
        /// 加载所有可用的模型类型
        /// </summary>
        private Dictionary<string, Type> LoadModelTypes()
        {
            var modelTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            
            // 获取所有 DO 模型类型
            var assembly = Assembly.GetAssembly(typeof(ActionTypesDO));
            if (assembly != null)
            {
                var doTypes = assembly.GetTypes()
                    .Where(t => t.Namespace == "BannerlordModEditor.Common.Models.DO" && 
                               t.IsClass && 
                               !t.IsAbstract && 
                               t.GetCustomAttribute<XmlRootAttribute>() != null)
                    .ToList();

                foreach (var type in doTypes)
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
                // 验证输入文件
                if (!File.Exists(excelFilePath))
                {
                    throw new FileNotFoundException("Excel 文件不存在", excelFilePath);
                }

                // 获取模型类型
                if (!_modelTypes.TryGetValue(modelType, out var targetType))
                {
                    throw new ArgumentException($"不支持的模型类型: {modelType}");
                }

                // 读取 Excel 数据
                var excelData = await ReadExcelAsync(excelFilePath, worksheetName);
                
                // 转换为 XML 对象
                var xmlObject = ConvertExcelToXmlObject(excelData, targetType);
                
                // 保存 XML 文件
                await SaveXmlAsync(xmlObject, xmlFilePath);
                
                return true;
            }
            catch (Exception ex)
            {
                throw new ConversionException($"Excel 转 XML 失败: {ex.Message}", ex);
            }
        }

        public async Task<bool> ConvertXmlToExcelAsync(string xmlFilePath, string excelFilePath, string? worksheetName = null)
        {
            try
            {
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

                // 获取模型类型
                if (!_modelTypes.TryGetValue(modelType, out var targetType))
                {
                    throw new ArgumentException($"不支持的模型类型: {modelType}");
                }

                // 读取 XML 数据
                var xmlObject = await ReadXmlAsync(xmlFilePath, targetType);
                
                // 转换为 Excel 数据
                var excelData = ConvertXmlObjectToExcel(xmlObject, modelType);
                
                // 保存 Excel 文件
                await SaveExcelAsync(excelData, excelFilePath, worksheetName);
                
                return true;
            }
            catch (Exception ex)
            {
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
                
                // 查找匹配的模型类型
                foreach (var kvp in _modelTypes)
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
                if (_modelTypes.TryGetValue(convertedName, out var matchedType))
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
                if (!_modelTypes.TryGetValue(modelType, out var targetType))
                {
                    throw new ArgumentException($"不支持的模型类型: {modelType}");
                }

                var excelData = await ReadExcelAsync(excelFilePath, worksheetName);
                
                // 调试信息
                Console.WriteLine($"调试: Excel文件包含 {excelData.Headers.Count} 个列: {string.Join(", ", excelData.Headers)}");
                Console.WriteLine($"调试: Excel文件包含 {excelData.Rows.Count} 行数据");
                
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
                Console.WriteLine($"调试: 正在读取Excel文件: {filePath}");
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

                Console.WriteLine($"调试: 使用工作表: {worksheet.Name}");

                var excelData = new ExcelData
                {
                    WorksheetName = worksheet.Name,
                    Headers = new List<string>(),
                    Rows = new List<Dictionary<string, object?>>()
                };

                // 读取表头
                var headerRow = worksheet.Row(1);
                Console.WriteLine($"调试: 读取表头行，单元格数量: {headerRow.Cells().Count()}");
                foreach (var cell in headerRow.Cells())
                {
                    var headerValue = cell.Value.ToString()?.Trim() ?? $"Column_{cell.Address.ColumnNumber}";
                    excelData.Headers.Add(headerValue);
                    Console.WriteLine($"调试: 添加表头: {headerValue}");
                }

                // 读取数据行
                var dataRows = worksheet.RowsUsed().Skip(1); // 跳过表头
                Console.WriteLine($"调试: 数据行数量: {dataRows.Count()}");
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

                Console.WriteLine($"调试: 成功读取Excel文件，表头数量: {excelData.Headers.Count}，数据行数: {excelData.Rows.Count}");
                return excelData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"调试: Excel读取异常: {ex.Message}");
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

        private object ConvertExcelToXmlObject(ExcelData excelData, Type targetType)
        {
            try
            {
                return ModelTypeConverter.ConvertExcelToModel(excelData, targetType);
            }
            catch (Exception ex)
            {
                throw new ConversionException($"Excel 转 XML 对象失败: {ex.Message}", ex);
            }
        }

        private async Task<object> ReadXmlAsync(string filePath, Type targetType)
        {
            try
            {
                // 使用反射创建 GenericXmlLoader 实例
                var loaderType = typeof(GenericXmlLoader<>).MakeGenericType(targetType);
                var loader = Activator.CreateInstance(loaderType);
                
                if (loader == null)
                {
                    throw new InvalidOperationException($"无法创建 GenericXmlLoader<{targetType.Name}> 的实例");
                }
                
                var loadMethod = loaderType.GetMethod("LoadAsync", new[] { typeof(string) });
                if (loadMethod == null)
                {
                    throw new InvalidOperationException($"GenericXmlLoader<{targetType.Name}> 没有 LoadAsync 方法");
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

        private ExcelData ConvertXmlObjectToExcel(object xmlObject, string modelType)
        {
            try
            {
                return ModelTypeConverter.ConvertModelToExcel(xmlObject, modelType);
            }
            catch (Exception ex)
            {
                throw new ConversionException($"XML 对象到 Excel 失败: {ex.Message}", ex);
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
    }
}