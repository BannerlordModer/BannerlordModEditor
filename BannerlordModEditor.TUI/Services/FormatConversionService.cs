using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ClosedXML.Excel;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.TUI.Models;

namespace BannerlordModEditor.TUI.Services
{
    public class FormatConversionService : IFormatConversionService
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly IXmlTypeDetectionService _xmlTypeDetectionService;
        private readonly ITypedXmlConversionService _typedXmlConversionService;
        private readonly Dictionary<string, Type> _xmlTypeMappings;

        public FormatConversionService(
            IFileDiscoveryService fileDiscoveryService,
            IXmlTypeDetectionService xmlTypeDetectionService,
            ITypedXmlConversionService typedXmlConversionService)
        {
            _fileDiscoveryService = fileDiscoveryService;
            _xmlTypeDetectionService = xmlTypeDetectionService;
            _typedXmlConversionService = typedXmlConversionService;
            _xmlTypeMappings = InitializeXmlTypeMappings();
        }

        private Dictionary<string, Type> InitializeXmlTypeMappings()
        {
            var mappings = new Dictionary<string, Type>();

            // 获取所有Common层的DO类型
            var doTypes = typeof(ActionTypesDO).Assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("DO") && !t.IsInterface && !t.IsAbstract)
                .ToList();

            foreach (var type in doTypes)
            {
                var xmlTypeName = type.Name.Replace("DO", "");
                mappings[xmlTypeName] = type;
            }

            return mappings;
        }

        public async Task<ConversionResult> ExcelToXmlAsync(string excelFilePath, string xmlFilePath, ConversionOptions? options = null)
        {
            var startTime = DateTime.UtcNow;
            var result = new ConversionResult();
            options ??= new ConversionOptions();

            try
            {
                // 验证输入参数
                if (string.IsNullOrWhiteSpace(excelFilePath))
                {
                    result.Success = false;
                    result.Message = "Excel文件路径不能为空";
                    result.Errors.Add("Excel文件路径不能为空");
                    return result;
                }

                if (string.IsNullOrWhiteSpace(xmlFilePath))
                {
                    result.Success = false;
                    result.Message = "XML文件路径不能为空";
                    result.Errors.Add("XML文件路径不能为空");
                    return result;
                }

                // 验证输入文件
                if (!File.Exists(excelFilePath))
                {
                    result.Success = false;
                    result.Message = $"Excel文件不存在: {excelFilePath}";
                    result.Errors.Add($"Excel文件不存在: {excelFilePath}");
                    return result;
                }

                // 验证文件扩展名
                var excelExtension = Path.GetExtension(excelFilePath).ToLowerInvariant();
                if (excelExtension != ".xlsx" && excelExtension != ".xls")
                {
                    result.Success = false;
                    result.Message = $"不支持的Excel文件格式: {excelExtension}";
                    result.Errors.Add($"不支持的Excel文件格式: {excelExtension}");
                    return result;
                }

                var xmlExtension = Path.GetExtension(xmlFilePath).ToLowerInvariant();
                if (xmlExtension != ".xml")
                {
                    result.Success = false;
                    result.Message = $"目标文件必须是XML格式: {xmlExtension}";
                    result.Errors.Add($"目标文件必须是XML格式: {xmlExtension}");
                    return result;
                }

                // 检查文件访问权限
                try
                {
                    var fileStream = File.OpenRead(excelFilePath);
                    fileStream.Dispose();
                }
                catch (UnauthorizedAccessException)
                {
                    result.Success = false;
                    result.Message = $"无法访问Excel文件，请检查权限: {excelFilePath}";
                    result.Errors.Add($"无法访问Excel文件，请检查权限: {excelFilePath}");
                    return result;
                }
                catch (IOException ex)
                {
                    result.Success = false;
                    result.Message = $"Excel文件被占用或损坏: {ex.Message}";
                    result.Errors.Add($"Excel文件被占用或损坏: {ex.Message}");
                    return result;
                }

                // 检查目标目录权限
                var targetDir = Path.GetDirectoryName(xmlFilePath);
                if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
                {
                    try
                    {
                        Directory.CreateDirectory(targetDir);
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.Message = $"无法创建目标目录: {ex.Message}";
                        result.Errors.Add($"无法创建目标目录: {ex.Message}");
                        return result;
                    }
                }

                // 创建备份
                if (options.CreateBackup && File.Exists(xmlFilePath))
                {
                    try
                    {
                        var backupPath = $"{xmlFilePath}.backup_{DateTime.UtcNow:yyyyMMddHHmmss}";
                        File.Copy(xmlFilePath, backupPath);
                        result.Warnings.Add($"已创建备份文件: {backupPath}");
                    }
                    catch (Exception ex)
                    {
                        result.Warnings.Add($"创建备份文件失败: {ex.Message}");
                    }
                }

                // 读取Excel文件
                using var workbook = new XLWorkbook(excelFilePath);
                var worksheet = workbook.Worksheets.FirstOrDefault();
                
                if (worksheet == null)
                {
                    result.Success = false;
                    result.Message = "Excel文件没有工作表";
                    result.Errors.Add("Excel文件没有工作表");
                    return result;
                }

                // 检查工作表是否为空
                var usedRange = worksheet.RangeUsed();
                if (usedRange == null)
                {
                    result.Success = false;
                    result.Message = "Excel工作表为空";
                    result.Errors.Add("Excel工作表为空");
                    return result;
                }

                // 获取列名
                var headers = new List<string>();
                var firstRowUsed = worksheet.FirstRowUsed();
                if (firstRowUsed != null)
                {
                    foreach (var cell in firstRowUsed.Cells())
                    {
                        try
                        {
                            var cellValue = cell.Value.ToString();
                            headers.Add(string.IsNullOrEmpty(cellValue?.Trim()) ? $"Column{cell.Address.ColumnNumber}" : cellValue.Trim());
                        }
                        catch (Exception ex)
                        {
                            result.Warnings.Add($"读取单元格 {cell.Address} 失败: {ex.Message}");
                            headers.Add($"Column{cell.Address.ColumnNumber}");
                        }
                    }
                }

                if (headers.Count == 0)
                {
                    result.Success = false;
                    result.Message = "Excel文件中没有找到列标题";
                    result.Errors.Add("Excel文件中没有找到列标题");
                    return result;
                }

                // 创建XML文档
                var xmlDoc = new XDocument();
                var rootName = options.RootElementName ?? "Root";
                var rootElement = new XElement(rootName);
                xmlDoc.Add(rootElement);

                var rowElementName = options.RowElementName ?? "Item";
                var recordsProcessed = 0;

                // 处理数据行
                var dataRows = worksheet.RowsUsed().Skip(1); // 跳过标题行
                var rowNumber = 1; // 用于错误报告
                
                foreach (var row in dataRows)
                {
                    rowNumber++;
                    
                    try
                    {
                        var rowElement = new XElement(rowElementName);
                        
                        for (int i = 0; i < headers.Count; i++)
                        {
                            try
                            {
                                var cell = row.Cell(i + 1);
                                var cellValue = cell.Value.ToString() ?? "";
                                var headerName = headers[i];
                                
                                // 处理嵌套元素（如果需要）
                                if (options.FlattenNestedElements && headerName.Contains(options.NestedElementSeparator ?? "_"))
                                {
                                    CreateNestedElements(rowElement, headerName, cellValue, options.NestedElementSeparator ?? "_");
                                }
                                else
                                {
                                    var element = new XElement(SanitizeXmlName(headerName), cellValue);
                                    rowElement.Add(element);
                                }
                            }
                            catch (Exception ex)
                            {
                                result.Warnings.Add($"处理第 {rowNumber} 行第 {i + 1} 列失败: {ex.Message}");
                                // 添加空元素以保持结构
                                var headerName = headers[i];
                                var element = new XElement(SanitizeXmlName(headerName), "");
                                rowElement.Add(element);
                            }
                        }
                        
                        rootElement.Add(rowElement);
                        recordsProcessed++;
                    }
                    catch (Exception ex)
                    {
                        result.Warnings.Add($"处理第 {rowNumber} 行失败: {ex.Message}");
                        // 跳过此行，继续处理下一行
                        continue;
                    }
                }

                // 保存XML文件
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = System.Text.Encoding.UTF8,
                    CheckCharacters = true,
                    CloseOutput = true
                };

                try
                {
                    using var writer = XmlWriter.Create(xmlFilePath, settings);
                    xmlDoc.Save(writer);
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Message = $"保存XML文件失败: {ex.Message}";
                    result.Errors.Add($"保存XML文件失败: {ex.Message}");
                    return result;
                }

                result.Success = true;
                result.OutputPath = xmlFilePath;
                result.RecordsProcessed = recordsProcessed;
                result.Message = $"成功将 {recordsProcessed} 条记录从Excel转换为XML";
                result.Duration = DateTime.UtcNow - startTime;

                // 验证转换结果
                if (options.IncludeSchemaValidation)
                {
                    var validationResult = await ValidateConversionAsync(excelFilePath, xmlFilePath, ConversionDirection.ExcelToXml);
                    result.Warnings.AddRange(validationResult.Warnings.Select(w => w.Message));
                    result.Errors.AddRange(validationResult.Errors.Select(e => e.Message));
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                result.Success = false;
                result.Message = $"文件访问权限不足: {ex.Message}";
                result.Errors.Add($"文件访问权限不足: {ex.Message}");
                result.Duration = DateTime.UtcNow - startTime;
            }
            catch (IOException ex)
            {
                result.Success = false;
                result.Message = $"文件I/O错误: {ex.Message}";
                result.Errors.Add($"文件I/O错误: {ex.Message}");
                result.Duration = DateTime.UtcNow - startTime;
            }
            catch (System.Xml.XmlException ex)
            {
                result.Success = false;
                result.Message = $"XML处理错误: {ex.Message}";
                result.Errors.Add($"XML处理错误: {ex.Message}");
                result.Duration = DateTime.UtcNow - startTime;
            }
            catch (OutOfMemoryException ex)
            {
                result.Success = false;
                result.Message = $"内存不足，文件可能过大: {ex.Message}";
                result.Errors.Add($"内存不足，文件可能过大: {ex.Message}");
                result.Duration = DateTime.UtcNow - startTime;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"转换失败: {ex.Message}";
                result.Errors.Add($"转换失败: {ex.Message}");
                result.Duration = DateTime.UtcNow - startTime;
            }

            return result;
        }

        public async Task<ConversionResult> XmlToExcelAsync(string xmlFilePath, string excelFilePath, ConversionOptions? options = null)
        {
            var startTime = DateTime.UtcNow;
            var result = new ConversionResult();
            options ??= new ConversionOptions();

            try
            {
                // 验证输入文件
                if (!File.Exists(xmlFilePath))
                {
                    result.Success = false;
                    result.Message = $"XML文件不存在: {xmlFilePath}";
                    result.Errors.Add($"XML文件不存在: {xmlFilePath}");
                    return result;
                }

                // 检测XML类型
                var xmlTypeInfo = await _xmlTypeDetectionService.DetectXmlTypeAsync(xmlFilePath);
                
                // 创建备份
                if (options.CreateBackup && File.Exists(excelFilePath))
                {
                    var backupPath = $"{excelFilePath}.backup_{DateTime.UtcNow:yyyyMMddHHmmss}";
                    File.Copy(excelFilePath, backupPath);
                    result.Warnings.Add($"已创建备份文件: {backupPath}");
                }

                // 如果是支持的类型化XML，使用类型化转换
                if (xmlTypeInfo.IsSupported && !string.IsNullOrEmpty(xmlTypeInfo.XmlType))
                {
                    try
                    {
                        result = await _typedXmlConversionService.DynamicTypedXmlToExcelAsync(
                            xmlFilePath, excelFilePath, xmlTypeInfo.XmlType, options);
                        
                        if (result.Success)
                        {
                            result.Message = $"成功转换类型化XML ({xmlTypeInfo.DisplayName}) 到Excel，共 {result.RecordsProcessed} 条记录";
                        }
                        
                        return result;
                    }
                    catch (Exception typedEx)
                    {
                        result.Warnings.Add($"类型化转换失败，回退到通用转换: {typedEx.Message}");
                        // 继续使用通用转换
                    }
                }

                // 通用XML转换（原有逻辑）
                var xmlDoc = XDocument.Load(xmlFilePath);
                var rootElement = xmlDoc.Root;

                if (rootElement == null)
                {
                    result.Success = false;
                    result.Message = "XML文件没有根元素";
                    result.Errors.Add("XML文件没有根元素");
                    return result;
                }

                // 收集所有可能的列名
                var columnNames = new HashSet<string>();
                var rowData = new List<Dictionary<string, string>>();

                // 分析XML结构并提取数据
                var rowElements = rootElement.Elements();
                foreach (var rowElement in rowElements)
                {
                    var rowDict = new Dictionary<string, string>();
                    ExtractElementData(rowElement, rowDict, "", options);
                    
                    foreach (var key in rowDict.Keys)
                    {
                        columnNames.Add(key);
                    }
                    
                    rowData.Add(rowDict);
                }

                // 创建Excel工作簿
                using var workbook = new XLWorkbook();
                var worksheetName = options.WorksheetName ?? "Sheet1";
                var worksheet = workbook.Worksheets.Add(worksheetName);

                // 添加表头
                var sortedColumns = columnNames.OrderBy(c => c).ToList();
                for (int i = 0; i < sortedColumns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = sortedColumns[i];
                }

                // 添加数据行
                for (int rowIndex = 0; rowIndex < rowData.Count; rowIndex++)
                {
                    var rowDict = rowData[rowIndex];
                    for (int colIndex = 0; colIndex < sortedColumns.Count; colIndex++)
                    {
                        var columnName = sortedColumns[colIndex];
                        if (rowDict.TryGetValue(columnName, out var value))
                        {
                            worksheet.Cell(rowIndex + 2, colIndex + 1).Value = value;
                        }
                    }
                }

                // 自动调整列宽
                worksheet.Columns().AdjustToContents();

                // 保存Excel文件
                workbook.SaveAs(excelFilePath);

                result.Success = true;
                result.OutputPath = excelFilePath;
                result.RecordsProcessed = rowData.Count;
                result.Message = $"成功将 {rowData.Count} 条记录从XML转换为Excel";
                result.Duration = DateTime.UtcNow - startTime;

                // 验证转换结果
                if (options.IncludeSchemaValidation)
                {
                    var validationResult = await ValidateConversionAsync(xmlFilePath, excelFilePath, ConversionDirection.XmlToExcel);
                    result.Warnings.AddRange(validationResult.Warnings.Select(w => w.Message));
                    result.Errors.AddRange(validationResult.Errors.Select(e => e.Message));
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                result.Success = false;
                result.Message = $"文件访问权限不足: {ex.Message}";
                result.Errors.Add($"文件访问权限不足: {ex.Message}");
                result.Duration = DateTime.UtcNow - startTime;
            }
            catch (IOException ex)
            {
                result.Success = false;
                result.Message = $"文件I/O错误: {ex.Message}";
                result.Errors.Add($"文件I/O错误: {ex.Message}");
                result.Duration = DateTime.UtcNow - startTime;
            }
            catch (System.Xml.XmlException ex)
            {
                result.Success = false;
                result.Message = $"XML处理错误: {ex.Message}";
                result.Errors.Add($"XML处理错误: {ex.Message}");
                result.Duration = DateTime.UtcNow - startTime;
            }
            catch (OutOfMemoryException ex)
            {
                result.Success = false;
                result.Message = $"内存不足，文件可能过大: {ex.Message}";
                result.Errors.Add($"内存不足，文件可能过大: {ex.Message}");
                result.Duration = DateTime.UtcNow - startTime;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"转换失败: {ex.Message}";
                result.Errors.Add($"转换失败: {ex.Message}");
                result.Duration = DateTime.UtcNow - startTime;
            }

            return result;
        }

        /// <summary>
        /// 检测XML文件类型
        /// </summary>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <returns>XML类型信息</returns>
        public async Task<XmlTypeInfo> DetectXmlTypeAsync(string xmlFilePath)
        {
            return await _xmlTypeDetectionService.DetectXmlTypeAsync(xmlFilePath);
        }

        /// <summary>
        /// 获取所有支持的XML类型
        /// </summary>
        /// <returns>支持的XML类型列表</returns>
        public async Task<List<XmlTypeInfo>> GetSupportedXmlTypesAsync()
        {
            return await _xmlTypeDetectionService.GetSupportedXmlTypesAsync();
        }

        /// <summary>
        /// 创建类型化XML模板
        /// </summary>
        /// <param name="xmlType">XML类型名称</param>
        /// <param name="outputPath">输出路径</param>
        /// <returns>创建结果</returns>
        public async Task<CreationResult> CreateTypedXmlTemplateAsync(string xmlType, string outputPath)
        {
            try
            {
                if (_xmlTypeMappings.TryGetValue(xmlType, out var modelType))
                {
                    var method = typeof(TypedXmlConversionService)
                        .GetMethod(nameof(ITypedXmlConversionService.CreateTypedXmlTemplateAsync))
                        ?.MakeGenericMethod(modelType);

                    if (method != null)
                    {
                        var task = (Task<CreationResult>)method.Invoke(_typedXmlConversionService, new object[] { outputPath })!;
                        return await task;
                    }
                }

                return new CreationResult
                {
                    Success = false,
                    Message = $"不支持的XML类型: {xmlType}",
                    Errors = { $"不支持的XML类型: {xmlType}" }
                };
            }
            catch (Exception ex)
            {
                return new CreationResult
                {
                    Success = false,
                    Message = $"创建XML模板失败: {ex.Message}",
                    Errors = { $"创建XML模板失败: {ex.Message}" }
                };
            }
        }

        public async Task<FileFormatInfo> DetectFileFormatAsync(string filePath)
        {
            var result = new FileFormatInfo();

            try
            {
                // 验证输入参数
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    result.IsSupported = false;
                    result.FormatDescription = "文件路径不能为空";
                    return result;
                }

                if (!File.Exists(filePath))
                {
                    result.IsSupported = false;
                    result.FormatDescription = "文件不存在";
                    return result;
                }

                // 检查文件访问权限
                try
                {
                    var fileStream = File.OpenRead(filePath);
                    fileStream.Dispose();
                }
                catch (UnauthorizedAccessException)
                {
                    result.IsSupported = false;
                    result.FormatDescription = "无法访问文件，请检查权限";
                    return result;
                }
                catch (IOException ex)
                {
                    result.IsSupported = false;
                    result.FormatDescription = $"文件被占用或损坏: {ex.Message}";
                    return result;
                }

                var extension = Path.GetExtension(filePath).ToLowerInvariant();
                
                switch (extension)
                {
                    case ".xlsx":
                    case ".xls":
                        result.FormatType = FileFormatType.Excel;
                        await AnalyzeExcelFormat(filePath, result);
                        break;
                        
                    case ".xml":
                        result.FormatType = FileFormatType.Xml;
                        await AnalyzeXmlFormat(filePath, result);
                        break;
                        
                    case ".csv":
                        result.FormatType = FileFormatType.Csv;
                        result.IsSupported = false;
                        result.FormatDescription = "CSV格式暂不支持";
                        break;
                        
                    default:
                        result.FormatType = FileFormatType.Unknown;
                        result.IsSupported = false;
                        result.FormatDescription = "不支持的文件格式";
                        break;
                }
            }
            catch (Exception ex)
            {
                result.IsSupported = false;
                result.FormatDescription = $"格式检测失败: {ex.Message}";
            }

            return result;
        }

        public async Task<ValidationResult> ValidateConversionAsync(string sourceFilePath, string targetFilePath, ConversionDirection direction)
        {
            var result = new ValidationResult();

            try
            {
                // 验证输入参数
                if (string.IsNullOrWhiteSpace(sourceFilePath))
                {
                    result.IsValid = false;
                    result.Errors.Add(new ValidationError
                    {
                        Message = "源文件路径不能为空",
                        ErrorType = ValidationErrorType.InvalidFormat
                    });
                    return result;
                }

                if (string.IsNullOrWhiteSpace(targetFilePath))
                {
                    result.IsValid = false;
                    result.Errors.Add(new ValidationError
                    {
                        Message = "目标文件路径不能为空",
                        ErrorType = ValidationErrorType.InvalidFormat
                    });
                    return result;
                }

                // 检查源文件是否存在
                if (!File.Exists(sourceFilePath))
                {
                    result.IsValid = false;
                    result.Errors.Add(new ValidationError
                    {
                        Message = $"源文件不存在: {sourceFilePath}",
                        ErrorType = ValidationErrorType.InvalidFormat
                    });
                    return result;
                }

                // 检查目标文件是否存在
                if (!File.Exists(targetFilePath))
                {
                    result.IsValid = false;
                    result.Errors.Add(new ValidationError
                    {
                        Message = $"目标文件不存在: {targetFilePath}",
                        ErrorType = ValidationErrorType.InvalidFormat
                    });
                    return result;
                }

                // 进行逻辑等价性验证
                if (direction == ConversionDirection.ExcelToXml)
                {
                    // 对于Excel到XML的转换，验证XML的结构和完整性
                    try
                    {
                        var xmlContent = File.ReadAllText(targetFilePath);
                        if (IsValidXml(xmlContent))
                        {
                            var normalizedXml = NormalizeXml(xmlContent);
                            result.IsValid = true;
                            result.Message = "XML格式验证通过";
                        }
                        else
                        {
                            result.IsValid = false;
                            result.Errors.Add(new ValidationError
                            {
                                Message = "生成的XML格式无效",
                                ErrorType = ValidationErrorType.InvalidFormat
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        result.IsValid = false;
                        result.Errors.Add(new ValidationError
                        {
                            Message = $"读取XML文件失败: {ex.Message}",
                            ErrorType = ValidationErrorType.InvalidFormat
                        });
                    }
                }
                else
                {
                    // 对于XML到Excel的转换，验证Excel的结构
                    try
                    {
                        using var workbook = new XLWorkbook(targetFilePath);
                        var worksheet = workbook.Worksheets.FirstOrDefault();
                        
                        if (worksheet != null)
                        {
                            var rowCount = worksheet.RowsUsed().Count();
                            var columnCount = worksheet.ColumnsUsed().Count();
                            
                            if (rowCount > 1 && columnCount > 0)
                            {
                                result.IsValid = true;
                                result.Message = $"Excel格式验证通过，包含 {rowCount - 1} 行数据，{columnCount} 列";
                            }
                            else
                            {
                                result.IsValid = false;
                                result.Errors.Add(new ValidationError
                                {
                                    Message = "生成的Excel文件没有有效数据",
                                    ErrorType = ValidationErrorType.StructureMismatch
                                });
                            }
                        }
                        else
                        {
                            result.IsValid = false;
                            result.Errors.Add(new ValidationError
                            {
                                Message = "Excel文件没有工作表",
                                ErrorType = ValidationErrorType.StructureMismatch
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        result.IsValid = false;
                        result.Errors.Add(new ValidationError
                        {
                            Message = $"Excel文件验证失败: {ex.Message}",
                            ErrorType = ValidationErrorType.InvalidFormat
                        });
                    }
                }

                // 检查数据完整性
                await ValidateDataIntegrity(sourceFilePath, targetFilePath, direction, result);
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    Message = $"验证失败: {ex.Message}",
                    ErrorType = ValidationErrorType.InvalidFormat
                });
            }

            return result;
        }

        private async Task AnalyzeExcelFormat(string filePath, FileFormatInfo result)
        {
            try
            {
                using var workbook = new XLWorkbook(filePath);
                var worksheet = workbook.Worksheets.FirstOrDefault();
                
                if (worksheet != null)
                {
                    // 获取列名
                    var firstRow = worksheet.FirstRowUsed();
                    if (firstRow != null)
                    {
                        foreach (var cell in firstRow.Cells())
                        {
                            var cellValue = cell.Value.ToString();
                            result.ColumnNames.Add(string.IsNullOrEmpty(cellValue) ? $"Column{cell.Address.ColumnNumber}" : cellValue);
                        }
                    }

                    result.RowCount = worksheet.RowsUsed().Count() - 1; // 减去标题行
                    result.IsSupported = true;
                    result.FormatDescription = $"Excel文件，包含 {result.RowCount} 行数据，{result.ColumnNames.Count} 列";
                }
                else
                {
                    result.IsSupported = false;
                    result.FormatDescription = "Excel文件没有工作表";
                }
            }
            catch (Exception ex)
            {
                result.IsSupported = false;
                result.FormatDescription = $"Excel文件分析失败: {ex.Message}";
            }
        }

        private async Task AnalyzeXmlFormat(string filePath, FileFormatInfo result)
        {
            try
            {
                var xmlDoc = XDocument.Load(filePath);
                var rootElement = xmlDoc.Root;
                
                if (rootElement != null)
                {
                    result.RootElement = rootElement.Name.LocalName;
                    
                    // 分析结构
                    var firstLevelElements = rootElement.Elements().ToList();
                    if (firstLevelElements.Any())
                    {
                        result.RowCount = firstLevelElements.Count;
                        
                        // 收集所有可能的字段名
                        var allFields = new HashSet<string>();
                        foreach (var element in firstLevelElements)
                        {
                            CollectElementNames(element, allFields, "");
                        }
                        
                        result.ColumnNames = allFields.OrderBy(n => n).ToList();
                        result.IsSupported = true;
                        result.FormatDescription = $"XML文件，根元素: {result.RootElement}，包含 {result.RowCount} 条记录";
                    }
                    else
                    {
                        result.IsSupported = false;
                        result.FormatDescription = "XML文件没有数据元素";
                    }
                }
                else
                {
                    result.IsSupported = false;
                    result.FormatDescription = "XML文件没有根元素";
                }
            }
            catch (Exception ex)
            {
                result.IsSupported = false;
                result.FormatDescription = $"XML文件分析失败: {ex.Message}";
            }
        }

        private void CreateNestedElements(XElement parent, string nestedPath, string value, string separator)
        {
            var parts = nestedPath.Split(separator);
            XElement currentElement = parent;

            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var elementName = SanitizeXmlName(part);

                if (i == parts.Length - 1)
                {
                    // 最后一个部分，设置值
                    currentElement.Add(new XElement(elementName, value));
                }
                else
                {
                    // 中间部分，查找或创建元素
                    var existingElement = currentElement.Element(elementName);
                    if (existingElement == null)
                    {
                        existingElement = new XElement(elementName);
                        currentElement.Add(existingElement);
                    }
                    currentElement = existingElement;
                }
            }
        }

        private void ExtractElementData(XElement element, Dictionary<string, string> data, string parentPath, ConversionOptions options)
        {
            if (element.HasElements)
            {
                // 处理嵌套元素
                foreach (var childElement in element.Elements())
                {
                    var childPath = string.IsNullOrEmpty(parentPath) ? childElement.Name.LocalName : $"{parentPath}{options.NestedElementSeparator}{childElement.Name.LocalName}";
                    
                    if (childElement.HasElements)
                    {
                        ExtractElementData(childElement, data, childPath, options);
                    }
                    else
                    {
                        var key = options.FlattenNestedElements ? childPath : childElement.Name.LocalName;
                        data[key] = childElement.Value;
                    }
                }
            }
            else
            {
                // 叶子节点
                var key = string.IsNullOrEmpty(parentPath) ? element.Name.LocalName : parentPath;
                data[key] = element.Value;
            }
        }

        private void CollectElementNames(XElement element, HashSet<string> names, string parentPath)
        {
            if (element.HasElements)
            {
                foreach (var childElement in element.Elements())
                {
                    var childPath = string.IsNullOrEmpty(parentPath) ? childElement.Name.LocalName : $"{parentPath}_{childElement.Name.LocalName}";
                    names.Add(childPath);
                    
                    if (childElement.HasElements)
                    {
                        CollectElementNames(childElement, names, childPath);
                    }
                }
            }
        }

        private async Task ValidateDataIntegrity(string sourceFilePath, string targetFilePath, ConversionDirection direction, ValidationResult result)
        {
            // 检查文件大小和数据量
            var sourceSize = new FileInfo(sourceFilePath).Length;
            var targetSize = new FileInfo(targetFilePath).Length;

            if (targetSize == 0)
            {
                result.Errors.Add(new ValidationError
                {
                    Message = "目标文件为空",
                    ErrorType = ValidationErrorType.StructureMismatch
                });
                result.IsValid = false;
                return;
            }

            // 根据转换方向进行特定验证
            if (direction == ConversionDirection.ExcelToXml)
            {
                // 验证XML结构
                try
                {
                    var xmlDoc = XDocument.Load(targetFilePath);
                    var rootElement = xmlDoc.Root;
                    
                    if (rootElement != null)
                    {
                        var recordCount = rootElement.Elements().Count();
                        if (recordCount == 0)
                        {
                            result.Warnings.Add(new ValidationWarning
                            {
                                Message = "XML文件没有数据记录",
                                WarningType = ValidationWarningType.EmptyField
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Errors.Add(new ValidationError
                    {
                        Message = $"XML结构验证失败: {ex.Message}",
                        ErrorType = ValidationErrorType.InvalidFormat
                    });
                    result.IsValid = false;
                }
            }
            else
            {
                // 验证Excel结构
                try
                {
                    using var workbook = new XLWorkbook(targetFilePath);
                    var worksheet = workbook.Worksheets.FirstOrDefault();
                    
                    if (worksheet == null)
                    {
                        result.Errors.Add(new ValidationError
                        {
                            Message = "Excel文件没有工作表",
                            ErrorType = ValidationErrorType.StructureMismatch
                        });
                        result.IsValid = false;
                    }
                }
                catch (Exception ex)
                {
                    result.Errors.Add(new ValidationError
                    {
                        Message = $"Excel结构验证失败: {ex.Message}",
                        ErrorType = ValidationErrorType.InvalidFormat
                    });
                    result.IsValid = false;
                }
            }
        }

        private string SanitizeXmlName(string name)
        {
            // 移除无效的XML字符
            var invalidChars = new[] { ' ', '<', '>', ':', '"', '/', '\\', '|', '?', '*' };
            var sanitized = name;
            
            foreach (var invalidChar in invalidChars)
            {
                sanitized = sanitized.Replace(invalidChar, '_');
            }

            // 确保以字母或下划线开头
            if (sanitized.Length > 0 && !char.IsLetter(sanitized[0]) && sanitized[0] != '_')
            {
                sanitized = "_" + sanitized;
            }

            return string.IsNullOrEmpty(sanitized) ? "element" : sanitized;
        }

        private bool IsValidXml(string xml)
        {
            try
            {
                XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string NormalizeXml(string xml)
        {
            var doc = XDocument.Parse(xml);
            
            // 移除注释
            doc.Descendants().OfType<XComment>().Remove();
            
            // 处理空白
            foreach (var element in doc.Descendants())
            {
                if (element.IsEmpty) continue;
                if (string.IsNullOrWhiteSpace(element.Value))
                {
                    element.Value = "";
                }
            }
            
            // 对属性进行排序
            foreach (var element in doc.Descendants())
            {
                var sortedAttributes = element.Attributes()
                    .OrderBy(a => a.IsNamespaceDeclaration ? 0 : 1)
                    .ThenBy(a => a.Name.NamespaceName)
                    .ThenBy(a => a.Name.LocalName)
                    .ToList();
                
                element.RemoveAttributes();
                foreach (var attr in sortedAttributes)
                {
                    element.Add(attr);
                }
            }
            
            return doc.ToString();
        }
    }
}