using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Services;
using ClosedXML.Excel;

namespace BannerlordModEditor.TUI.Services
{
    /// <summary>
    /// 增强的类型化XML转换服务
    /// 支持所有DO模型的完整XML-Excel互转功能
    /// </summary>
    public class EnhancedTypedXmlConversionService : ITypedXmlConversionService
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly Dictionary<string, Type> _xmlTypeMappings;
        private readonly Dictionary<string, XmlTypeMetadata> _xmlTypeMetadata;

        public EnhancedTypedXmlConversionService(IFileDiscoveryService fileDiscoveryService)
        {
            _fileDiscoveryService = fileDiscoveryService;
            _xmlTypeMappings = InitializeXmlTypeMappings();
            _xmlTypeMetadata = InitializeXmlTypeMetadata();
        }

        public async Task<ConversionResult> TypedXmlToExcelAsync<T>(string xmlFilePath, string excelFilePath, ConversionOptions? options = null)
            where T : class
        {
            var startTime = DateTime.UtcNow;
            var result = new ConversionResult();
            options ??= new ConversionOptions();

            try
            {
                // 验证输入参数
                if (!ValidateInputParameters(xmlFilePath, excelFilePath, result, "XML", "Excel"))
                    return result;

                // 创建备份
                await CreateBackupAsync(excelFilePath, options, result);

                // 加载类型化XML
                var data = await LoadTypedXmlAsync<T>(xmlFilePath);

                // 转换为Excel数据
                var excelData = await ConvertToExcelDataAsync<T>(data);
                
                // 保存Excel文件
                await SaveExcelAsync(excelFilePath, excelData, typeof(T).Name);

                result.Success = true;
                result.OutputPath = excelFilePath;
                result.RecordsProcessed = excelData.Count;
                result.Message = $"成功将 {typeof(T).Name} 转换为Excel，共 {result.RecordsProcessed} 条记录";
                result.Duration = DateTime.UtcNow - startTime;
            }
            catch (Exception ex)
            {
                HandleConversionError(ex, result, "类型化XML到Excel转换失败");
            }

            return result;
        }

        public async Task<ConversionResult> ExcelToTypedXmlAsync<T>(string excelFilePath, string xmlFilePath, ConversionOptions? options = null)
            where T : class
        {
            var startTime = DateTime.UtcNow;
            var result = new ConversionResult();
            options ??= new ConversionOptions();

            try
            {
                // 验证输入参数
                if (!ValidateInputParameters(excelFilePath, xmlFilePath, result, "Excel", "XML"))
                    return result;

                // 创建备份
                await CreateBackupAsync(xmlFilePath, options, result);

                // 读取Excel数据
                var excelData = await ReadExcelAsync(excelFilePath);
                
                // 转换为类型化对象
                var data = await ConvertFromExcelDataAsync<T>(excelData);

                // 保存类型化XML
                await SaveTypedXmlAsync(data, xmlFilePath);

                result.Success = true;
                result.OutputPath = xmlFilePath;
                result.RecordsProcessed = CountRecords(data);
                result.Message = $"成功将Excel转换为 {typeof(T).Name}，共 {result.RecordsProcessed} 条记录";
                result.Duration = DateTime.UtcNow - startTime;
            }
            catch (Exception ex)
            {
                HandleConversionError(ex, result, "Excel到类型化XML转换失败");
            }

            return result;
        }

        public async Task<ConversionResult> DynamicTypedXmlToExcelAsync(string xmlFilePath, string excelFilePath, string xmlType, ConversionOptions? options = null)
        {
            try
            {
                if (_xmlTypeMappings.TryGetValue(xmlType, out var modelType))
                {
                    var method = typeof(EnhancedTypedXmlConversionService)
                        .GetMethod(nameof(TypedXmlToExcelAsync))
                        ?.MakeGenericMethod(modelType);

                    if (method != null)
                    {
                        var task = (Task<ConversionResult>)method.Invoke(this, new object[] { xmlFilePath, excelFilePath, options })!;
                        return await task;
                    }
                }

                return new ConversionResult
                {
                    Success = false,
                    Message = $"不支持的XML类型: {xmlType}",
                    Errors = { $"不支持的XML类型: {xmlType}" }
                };
            }
            catch (Exception ex)
            {
                return new ConversionResult
                {
                    Success = false,
                    Message = $"动态类型化XML转换失败: {ex.Message}",
                    Errors = { $"动态类型化XML转换失败: {ex.Message}" }
                };
            }
        }

        public async Task<ConversionResult> DynamicExcelToTypedXmlAsync(string excelFilePath, string xmlFilePath, string xmlType, ConversionOptions? options = null)
        {
            try
            {
                if (_xmlTypeMappings.TryGetValue(xmlType, out var modelType))
                {
                    var method = typeof(EnhancedTypedXmlConversionService)
                        .GetMethod(nameof(ExcelToTypedXmlAsync))
                        ?.MakeGenericMethod(modelType);

                    if (method != null)
                    {
                        var task = (Task<ConversionResult>)method.Invoke(this, new object[] { excelFilePath, xmlFilePath, options })!;
                        return await task;
                    }
                }

                return new ConversionResult
                {
                    Success = false,
                    Message = $"不支持的XML类型: {xmlType}",
                    Errors = { $"不支持的XML类型: {xmlType}" }
                };
            }
            catch (Exception ex)
            {
                return new ConversionResult
                {
                    Success = false,
                    Message = $"动态Excel到类型化XML转换失败: {ex.Message}",
                    Errors = { $"动态Excel到类型化XML转换失败: {ex.Message}" }
                };
            }
        }

        public async Task<ValidationResult> ValidateTypedXmlAsync<T>(string xmlFilePath)
            where T : class
        {
            var result = new ValidationResult();

            try
            {
                if (!File.Exists(xmlFilePath))
                {
                    result.IsValid = false;
                    result.Errors.Add(new ValidationError
                    {
                        Message = "XML文件不存在",
                        ErrorType = ValidationErrorType.InvalidFormat
                    });
                    return result;
                }

                // 验证XML格式
                var data = await LoadTypedXmlAsync<T>(xmlFilePath);
                
                // 验证数据完整性
                await ValidateDataIntegrityAsync<T>(data, result);

                result.IsValid = result.Errors.Count == 0;
                result.Message = result.IsValid ? 
                    $"类型化XML ({typeof(T).Name}) 验证通过" : 
                    $"类型化XML ({typeof(T).Name}) 验证失败";
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    Message = $"类型化XML验证失败: {ex.Message}",
                    ErrorType = ValidationErrorType.InvalidFormat
                });
            }

            return result;
        }

        public async Task<CreationResult> CreateTypedXmlTemplateAsync<T>(string outputPath)
            where T : class, new()
        {
            var result = new CreationResult();

            try
            {
                if (string.IsNullOrWhiteSpace(outputPath))
                {
                    result.Success = false;
                    result.Message = "输出路径不能为空";
                    result.Errors.Add("输出路径不能为空");
                    return result;
                }

                // 创建新的类型化对象
                var data = new T();

                // 初始化默认值
                await InitializeDefaultValuesAsync(data);

                // 保存为XML
                await SaveTypedXmlAsync(data, outputPath);

                result.Success = true;
                result.Message = $"成功创建类型化XML模板: {typeof(T).Name}";
                result.OutputPath = outputPath;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"创建类型化XML模板失败: {ex.Message}";
                result.Errors.Add($"创建类型化XML模板失败: {ex.Message}");
            }

            return result;
        }

        #region 私有方法

        private async Task<T> LoadTypedXmlAsync<T>(string xmlFilePath)
            where T : class
        {
            if (!File.Exists(xmlFilePath))
                throw new FileNotFoundException($"XML文件不存在: {xmlFilePath}");

            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StreamReader(xmlFilePath);
            var data = (T?)serializer.Deserialize(reader);
            
            if (data == null)
                throw new Exception($"无法反序列化XML文件: {xmlFilePath}");

            return data;
        }

        private async Task SaveTypedXmlAsync<T>(T data, string xmlFilePath)
            where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            var settings = new System.Xml.XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                Encoding = System.Text.Encoding.UTF8,
                CheckCharacters = true,
                CloseOutput = true
            };

            using var writer = System.Xml.XmlWriter.Create(xmlFilePath, settings);
            serializer.Serialize(writer, data);
            
            await Task.CompletedTask;
        }

        private async Task<List<Dictionary<string, object>>> ConvertToExcelDataAsync<T>(T data)
        {
            var result = new List<Dictionary<string, object>>();

            if (data == null)
                return result;

            // 获取类型元数据
            var metadata = GetXmlTypeMetadata<T>();
            
            // 使用反射将对象转换为字典列表
            var properties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var prop in properties)
            {
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType) &&
                    prop.PropertyType != typeof(string))
                {
                    var collection = prop.GetValue(data) as System.Collections.IEnumerable;
                    if (collection != null)
                    {
                        foreach (var item in collection)
                        {
                            var dict = await ConvertObjectToDictionaryAsync(item, metadata);
                            result.Add(dict);
                        }
                    }
                }
            }

            return result;
        }

        private async Task<T> ConvertFromExcelDataAsync<T>(List<Dictionary<string, object>> excelData)
            where T : class
        {
            var result = System.Activator.CreateInstance<T>();
            var metadata = GetXmlTypeMetadata<T>();

            // 查找集合属性
            var collectionProperties = result.GetType().GetProperties()
                .Where(p => typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) &&
                           p.PropertyType != typeof(string))
                .ToList();

            if (collectionProperties.Any())
            {
                var collectionProperty = collectionProperties.First();
                var collectionType = collectionProperty.PropertyType.GetGenericArguments().FirstOrDefault();
                
                if (collectionType != null)
                {
                    var listType = typeof(List<>).MakeGenericType(collectionType);
                    var list = (System.Collections.IList?)System.Activator.CreateInstance(listType);
                    
                    if (list != null)
                    {
                        foreach (var rowData in excelData)
                        {
                            var item = await ConvertDictionaryToObjectAsync(rowData, collectionType, metadata);
                            if (item != null)
                            {
                                list.Add(item);
                            }
                        }
                        
                        collectionProperty.SetValue(result, list);
                    }
                }
            }

            return result;
        }

        private async Task<Dictionary<string, object>> ConvertObjectToDictionaryAsync(object obj, XmlTypeMetadata metadata)
        {
            var dict = new Dictionary<string, object>();
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                var displayName = metadata.GetDisplayName(prop.Name);
                
                // 处理复杂对象
                if (value != null && !IsSimpleType(value.GetType()))
                {
                    dict[displayName] = await ConvertComplexValueToStringAsync(value);
                }
                else
                {
                    dict[displayName] = value?.ToString() ?? "";
                }
            }

            return dict;
        }

        private async Task<object?> ConvertDictionaryToObjectAsync(Dictionary<string, object> dict, Type objectType, XmlTypeMetadata metadata)
        {
            var obj = System.Activator.CreateInstance(objectType);
            if (obj == null)
                return null;

            var properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var displayName = metadata.GetDisplayName(prop.Name);
                if (dict.TryGetValue(displayName, out var value))
                {
                    var convertedValue = await ConvertStringToPropertyTypeAsync(value?.ToString(), prop.PropertyType);
                    prop.SetValue(obj, convertedValue);
                }
            }

            return obj;
        }

        private async Task<string> ConvertComplexValueToStringAsync(object value)
        {
            // 简化处理：将复杂对象转换为JSON字符串
            try
            {
                return System.Text.Json.JsonSerializer.Serialize(value);
            }
            catch
            {
                return value.ToString() ?? "";
            }
        }

        private async Task<object?> ConvertStringToPropertyTypeAsync(string? value, Type targetType)
        {
            if (string.IsNullOrEmpty(value))
                return GetDefaultValue(targetType);

            try
            {
                if (targetType == typeof(string))
                    return value;
                else if (targetType == typeof(int) || targetType == typeof(int?))
                    return int.Parse(value);
                else if (targetType == typeof(float) || targetType == typeof(float?))
                    return float.Parse(value);
                else if (targetType == typeof(double) || targetType == typeof(double?))
                    return double.Parse(value);
                else if (targetType == typeof(decimal) || targetType == typeof(decimal?))
                    return decimal.Parse(value);
                else if (targetType == typeof(bool) || targetType == typeof(bool?))
                    return bool.Parse(value);
                else if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
                    return DateTime.Parse(value);
                else if (targetType.IsEnum)
                    return Enum.Parse(targetType, value);
                else
                    return value; // 其他类型保持字符串
            }
            catch
            {
                return GetDefaultValue(targetType);
            }
        }

        private object? GetDefaultValue(Type type)
        {
            if (type == typeof(string))
                return "";
            else if (type == typeof(int) || type == typeof(int?))
                return 0;
            else if (type == typeof(float) || type == typeof(float?))
                return 0f;
            else if (type == typeof(double) || type == typeof(double?))
                return 0.0;
            else if (type == typeof(decimal) || type == typeof(decimal?))
                return 0m;
            else if (type == typeof(bool) || type == typeof(bool?))
                return false;
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
                return DateTime.MinValue;
            else if (type.IsEnum)
                return Enum.GetValues(type).GetValue(0);
            else
                return null;
        }

        private bool IsSimpleType(Type type)
        {
            return type.IsPrimitive || 
                   type == typeof(string) || 
                   type == typeof(decimal) || 
                   type == typeof(DateTime) || 
                   type.IsEnum;
        }

        private async Task<List<Dictionary<string, object>>> ReadExcelAsync(string excelFilePath)
        {
            var result = new List<Dictionary<string, object>>();

            using var workbook = new XLWorkbook(excelFilePath);
            var worksheet = workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
                return result;

            // 获取表头
            var headers = new List<string>();
            var firstRow = worksheet.FirstRowUsed();
            if (firstRow != null)
            {
                foreach (var cell in firstRow.Cells())
                {
                    headers.Add(cell.Value.ToString() ?? $"Column{cell.Address.ColumnNumber}");
                }
            }

            // 读取数据行
            var dataRows = worksheet.RowsUsed().Skip(1);
            foreach (var row in dataRows)
            {
                var rowData = new Dictionary<string, object>();
                for (int i = 0; i < headers.Count; i++)
                {
                    try
                    {
                        var cell = row.Cell(i + 1);
                        rowData[headers[i]] = cell.Value;
                    }
                    catch
                    {
                        rowData[headers[i]] = "";
                    }
                }
                result.Add(rowData);
            }

            return await Task.FromResult(result);
        }

        private async Task SaveExcelAsync(string excelFilePath, List<Dictionary<string, object>> excelData, string typeName)
        {
            if (!excelData.Any())
                return;

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(typeName);

            // 获取所有列名
            var allColumns = new HashSet<string>();
            foreach (var row in excelData)
            {
                foreach (var key in row.Keys)
                {
                    allColumns.Add(key);
                }
            }

            var sortedColumns = allColumns.OrderBy(c => c).ToList();

            // 添加表头
            for (int i = 0; i < sortedColumns.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = sortedColumns[i];
            }

            // 添加数据行
            for (int rowIndex = 0; rowIndex < excelData.Count; rowIndex++)
            {
                var rowData = excelData[rowIndex];
                for (int colIndex = 0; colIndex < sortedColumns.Count; colIndex++)
                {
                    var columnName = sortedColumns[colIndex];
                    if (rowData.TryGetValue(columnName, out var value))
                    {
                        worksheet.Cell(rowIndex + 2, colIndex + 1).Value = value?.ToString() ?? "";
                    }
                }
            }

            // 自动调整列宽
            worksheet.Columns().AdjustToContents();

            // 保存Excel文件
            workbook.SaveAs(excelFilePath);

            await Task.CompletedTask;
        }

        private async Task InitializeDefaultValuesAsync<T>(T data)
            where T : class
        {
            // 初始化集合属性
            var properties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var prop in properties)
            {
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType) &&
                    prop.PropertyType != typeof(string))
                {
                    var collectionType = prop.PropertyType.GetGenericArguments().FirstOrDefault();
                    if (collectionType != null)
                    {
                        var listType = typeof(List<>).MakeGenericType(collectionType);
                        var list = System.Activator.CreateInstance(listType);
                        prop.SetValue(data, list);
                    }
                }
            }
        }

        private async Task ValidateDataIntegrityAsync<T>(T data, ValidationResult result)
            where T : class
        {
            var properties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var prop in properties)
            {
                var value = prop.GetValue(data);
                
                // 验证必需属性
                if (value == null && IsRequiredProperty(prop))
                {
                    result.Errors.Add(new ValidationError
                    {
                        Message = $"必需属性 {prop.Name} 为空",
                        ErrorType = ValidationErrorType.MissingRequiredField
                    });
                }
                
                // 验证集合属性
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType) &&
                    prop.PropertyType != typeof(string))
                {
                    var collection = value as System.Collections.IEnumerable;
                    if (collection != null)
                    {
                        var count = 0;
                        var enumerator = collection.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            count++;
                        }
                        
                        if (count == 0)
                        {
                            result.Warnings.Add(new ValidationWarning
                            {
                                Message = $"集合属性 {prop.Name} 为空",
                                WarningType = ValidationWarningType.EmptyField
                            });
                        }
                    }
                }
            }
        }

        private bool IsRequiredProperty(PropertyInfo prop)
        {
            // 检查是否有Required特性或其他标记
            return prop.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true).Any() ||
                   prop.PropertyType.IsValueType &&
                   Nullable.GetUnderlyingType(prop.PropertyType) == null;
        }

        private int CountRecords<T>(T data)
        {
            if (data == null)
                return 0;

            var properties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var prop in properties)
            {
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType) &&
                    prop.PropertyType != typeof(string))
                {
                    var collection = prop.GetValue(data) as System.Collections.IEnumerable;
                    if (collection != null)
                    {
                        var count = 0;
                        var enumerator = collection.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            count++;
                        }
                        return count;
                    }
                }
            }

            return 0;
        }

        private bool ValidateInputParameters(string sourcePath, string targetPath, ConversionResult result, string sourceType, string targetType)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                result.Success = false;
                result.Message = $"{sourceType}文件路径不能为空";
                result.Errors.Add($"{sourceType}文件路径不能为空");
                return false;
            }

            if (string.IsNullOrWhiteSpace(targetPath))
            {
                result.Success = false;
                result.Message = $"{targetType}文件路径不能为空";
                result.Errors.Add($"{targetType}文件路径不能为空");
                return false;
            }

            if (!File.Exists(sourcePath))
            {
                result.Success = false;
                result.Message = $"{sourceType}文件不存在: {sourcePath}";
                result.Errors.Add($"{sourceType}文件不存在: {sourcePath}");
                return false;
            }

            return true;
        }

        private async Task CreateBackupAsync(string filePath, ConversionOptions options, ConversionResult result)
        {
            if (options.CreateBackup && File.Exists(filePath))
            {
                try
                {
                    var backupPath = $"{filePath}.backup_{DateTime.UtcNow:yyyyMMddHHmmss}";
                    File.Copy(filePath, backupPath);
                    result.Warnings.Add($"已创建备份文件: {backupPath}");
                }
                catch (Exception ex)
                {
                    result.Warnings.Add($"创建备份文件失败: {ex.Message}");
                }
            }
        }

        private void HandleConversionError(Exception ex, ConversionResult result, string errorMessage)
        {
            result.Success = false;
            result.Message = $"{errorMessage}: {ex.Message}";
            result.Errors.Add($"{errorMessage}: {ex.Message}");
            result.Duration = TimeSpan.Zero;
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

        private Dictionary<string, XmlTypeMetadata> InitializeXmlTypeMetadata()
        {
            var metadata = new Dictionary<string, XmlTypeMetadata>();

            // 为每个XML类型创建元数据
            foreach (var kvp in _xmlTypeMappings)
            {
                metadata[kvp.Key] = new XmlTypeMetadata(kvp.Value);
            }

            return metadata;
        }

        private XmlTypeMetadata GetXmlTypeMetadata<T>()
        {
            var typeName = typeof(T).Name.Replace("DO", "");
            return _xmlTypeMetadata.TryGetValue(typeName, out var metadata) ? metadata : new XmlTypeMetadata(typeof(T));
        }

        #endregion
    }

    /// <summary>
    /// XML类型元数据
    /// </summary>
    public class XmlTypeMetadata
    {
        private readonly Dictionary<string, string> _propertyDisplayNames;
        private readonly Dictionary<string, string> _propertyDescriptions;

        public XmlTypeMetadata(Type modelType)
        {
            ModelType = modelType;
            _propertyDisplayNames = new Dictionary<string, string>();
            _propertyDescriptions = new Dictionary<string, string>();

            InitializePropertyMetadata();
        }

        public Type ModelType { get; }
        public string TypeName => ModelType.Name.Replace("DO", "");

        public string GetDisplayName(string propertyName)
        {
            return _propertyDisplayNames.TryGetValue(propertyName, out var displayName) 
                ? displayName 
                : propertyName;
        }

        public string GetDescription(string propertyName)
        {
            return _propertyDescriptions.TryGetValue(propertyName, out var description) 
                ? description 
                : "";
        }

        private void InitializePropertyMetadata()
        {
            var properties = ModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var prop in properties)
            {
                // 生成显示名称（将PascalCase转换为更友好的格式）
                _propertyDisplayNames[prop.Name] = ConvertToDisplayName(prop.Name);
                
                // 生成描述（基于属性名和类型）
                _propertyDescriptions[prop.Name] = GeneratePropertyDescription(prop);
            }
        }

        private string ConvertToDisplayName(string propertyName)
        {
            // 将PascalCase转换为空格分隔的格式
            return System.Text.RegularExpressions.Regex.Replace(propertyName, "([a-z])([A-Z])", "$1 $2");
        }

        private string GeneratePropertyDescription(PropertyInfo prop)
        {
            var type = prop.PropertyType;
            var nullableType = Nullable.GetUnderlyingType(type);
            var actualType = nullableType ?? type;

            string typeDescription = actualType.Name;
            if (actualType == typeof(string))
                typeDescription = "字符串";
            else if (actualType == typeof(int) || actualType == typeof(int?))
                typeDescription = "整数";
            else if (actualType == typeof(float) || actualType == typeof(float?))
                typeDescription = "浮点数";
            else if (actualType == typeof(bool) || actualType == typeof(bool?))
                typeDescription = "布尔值";
            else if (actualType == typeof(DateTime) || actualType == typeof(DateTime?))
                typeDescription = "日期时间";
            else if (actualType.IsEnum)
                typeDescription = "枚举";

            return $"{ConvertToDisplayName(prop.Name)} ({typeDescription})";
        }
    }
}