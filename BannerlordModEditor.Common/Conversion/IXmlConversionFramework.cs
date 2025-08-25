using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Common.Conversion
{
    /// <summary>
    /// 通用XML转换框架接口
    /// </summary>
    public interface IXmlConversionFramework
    {
        /// <summary>
        /// 将XML转换为二维表格格式
        /// </summary>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<TableConversionResult> XmlToTableAsync(string xmlFilePath, TableConversionOptions? options = null);

        /// <summary>
        /// 将二维表格格式转换为XML
        /// </summary>
        /// <param name="tableData">表格数据</param>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> TableToXmlAsync(TableData tableData, string xmlFilePath, TableConversionOptions? options = null);

        /// <summary>
        /// 将XML转换为CSV格式
        /// </summary>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <param name="csvFilePath">CSV文件路径</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> XmlToCsvAsync(string xmlFilePath, string csvFilePath, CsvConversionOptions? options = null);

        /// <summary>
        /// 将CSV格式转换为XML
        /// </summary>
        /// <param name="csvFilePath">CSV文件路径</param>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> CsvToXmlAsync(string csvFilePath, string xmlFilePath, CsvConversionOptions? options = null);

        /// <summary>
        /// 将XML转换为JSON格式
        /// </summary>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <param name="jsonFilePath">JSON文件路径</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> XmlToJsonAsync(string xmlFilePath, string jsonFilePath, JsonConversionOptions? options = null);

        /// <summary>
        /// 将JSON格式转换为XML
        /// </summary>
        /// <param name="jsonFilePath">JSON文件路径</param>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <param name="options">转换选项</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> JsonToXmlAsync(string jsonFilePath, string xmlFilePath, JsonConversionOptions? options = null);

        /// <summary>
        /// 分析XML结构并返回结构信息
        /// </summary>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <returns>XML结构信息</returns>
        Task<XmlStructureInfo> AnalyzeXmlStructureAsync(string xmlFilePath);

        /// <summary>
        /// 获取支持的转换格式
        /// </summary>
        /// <returns>支持的格式列表</returns>
        Task<List<SupportedFormat>> GetSupportedFormatsAsync();

        /// <summary>
        /// 注册自定义转换器
        /// </summary>
        /// <param name="converter">转换器</param>
        void RegisterConverter(IFormatConverter converter);

        /// <summary>
        /// 批量转换XML文件
        /// </summary>
        /// <param name="request">批量转换请求</param>
        /// <returns>批量转换结果</returns>
        Task<BatchConversionResult> BatchConvertAsync(BatchConversionRequest request);
    }

    /// <summary>
    /// 格式转换器接口
    /// </summary>
    public interface IFormatConverter
    {
        /// <summary>
        /// 转换器名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 支持的源格式
        /// </summary>
        FileFormatType SourceFormat { get; }

        /// <summary>
        /// 支持的目标格式
        /// </summary>
        FileFormatType TargetFormat { get; }

        /// <summary>
        /// 是否支持指定的XML类型
        /// </summary>
        /// <param name="xmlType">XML类型</param>
        /// <returns>是否支持</returns>
        bool SupportsXmlType(string xmlType);

        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="request">转换请求</param>
        /// <returns>转换结果</returns>
        Task<ConversionResult> ConvertAsync(ConversionRequest request);
    }

    /// <summary>
    /// 转换策略接口
    /// </summary>
    public interface IConversionStrategy
    {
        /// <summary>
        /// 策略名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 适用的XML类型
        /// </summary>
        HashSet<string> ApplicableXmlTypes { get; }

        /// <summary>
        /// 将XML转换为表格数据
        /// </summary>
        /// <param name="xmlDoc">XML文档</param>
        /// <param name="options">转换选项</param>
        /// <returns>表格数据</returns>
        Task<TableData> ConvertToTableAsync(System.Xml.Linq.XDocument xmlDoc, TableConversionOptions? options = null);

        /// <summary>
        /// 将表格数据转换为XML
        /// </summary>
        /// <param name="tableData">表格数据</param>
        /// <param name="options">转换选项</param>
        /// <returns>XML文档</returns>
        Task<System.Xml.Linq.XDocument> ConvertFromTableAsync(TableData tableData, TableConversionOptions? options = null);
    }

    /// <summary>
    /// 转换管道步骤接口
    /// </summary>
    public interface IConversionPipelineStep
    {
        /// <summary>
        /// 步骤名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 执行步骤
        /// </summary>
        /// <param name="context">转换上下文</param>
        /// <returns>执行结果</returns>
        Task<PipelineStepResult> ExecuteAsync(ConversionContext context);

        /// <summary>
        /// 是否支持指定的转换方向
        /// </summary>
        /// <param name="direction">转换方向</param>
        /// <returns>是否支持</returns>
        bool SupportsDirection(ConversionDirection direction);
    }

    /// <summary>
    /// 转换结果基类
    /// </summary>
    public class ConversionResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? OutputPath { get; set; }
        public int RecordsProcessed { get; set; }
        public TimeSpan Duration { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 表格转换结果
    /// </summary>
    public class TableConversionResult : ConversionResult
    {
        public TableData? TableData { get; set; }
        public XmlStructureInfo? StructureInfo { get; set; }
    }

    /// <summary>
    /// 表格数据
    /// </summary>
    public class TableData
    {
        public List<string> Columns { get; set; } = new List<string>();
        public List<TableRow> Rows { get; set; } = new List<TableRow>();
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="columnName">列名</param>
        public void AddColumn(string columnName)
        {
            if (!Columns.Contains(columnName))
            {
                Columns.Add(columnName);
            }
        }

        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="row">行数据</param>
        public void AddRow(TableRow row)
        {
            Rows.Add(row);
        }

        /// <summary>
        /// 获取数据透视表
        /// </summary>
        /// <returns>数据透视表</returns>
        public Dictionary<string, List<object>> ToDataTable()
        {
            var dataTable = new Dictionary<string, List<object>>();
            
            // 初始化列
            foreach (var column in Columns)
            {
                dataTable[column] = new List<object>();
            }

            // 填充数据
            foreach (var row in Rows)
            {
                foreach (var column in Columns)
                {
                    dataTable[column].Add(row.TryGetValue(column, out var value) ? value : "");
                }
            }

            return dataTable;
        }
    }

    /// <summary>
    /// 表格行
    /// </summary>
    public class TableRow : Dictionary<string, object>
    {
        public TableRow() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public TableRow(IDictionary<string, object> dictionary) : base(dictionary, StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// 获取值，如果不存在则返回默认值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>值</returns>
        public T GetValue<T>(string key, T defaultValue = default!)
        {
            if (TryGetValue(key, out var value))
            {
                if (value is T typedValue)
                {
                    return typedValue;
                }

                // 尝试类型转换
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T))!;
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void SetValue(string key, object value)
        {
            this[key] = value;
        }
    }

    /// <summary>
    /// XML结构信息
    /// </summary>
    public class XmlStructureInfo
    {
        public string? RootElement { get; set; }
        public List<string> Elements { get; set; } = new List<string>();
        public List<string> Attributes { get; set; } = new List<string>();
        public List<string> Namespaces { get; set; } = new List<string>();
        public int EstimatedDepth { get; set; }
        public int EstimatedRecordCount { get; set; }
        public XmlComplexity Complexity { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// XML复杂度级别
    /// </summary>
    public enum XmlComplexity
    {
        Simple,     // 简单列表结构
        Medium,     // 中等复杂度，包含嵌套
        Complex,    // 复杂结构，多层嵌套
        VeryComplex // 非常复杂，混合结构
    }

    /// <summary>
    /// 支持的格式信息
    /// </summary>
    public class SupportedFormat
    {
        public FileFormatType FormatType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<FileFormatType> CanConvertTo { get; set; } = new List<FileFormatType>();
        public List<FileFormatType> CanConvertFrom { get; set; } = new List<FileFormatType>();
    }

    /// <summary>
    /// 转换请求
    /// </summary>
    public class ConversionRequest
    {
        public string SourcePath { get; set; } = string.Empty;
        public string TargetPath { get; set; } = string.Empty;
        public FileFormatType SourceFormat { get; set; }
        public FileFormatType TargetFormat { get; set; }
        public string? XmlType { get; set; }
        public Dictionary<string, object> Options { get; set; } = new Dictionary<string, object>();
        public ConversionDirection Direction { get; set; }
    }

    /// <summary>
    /// 转换上下文
    /// </summary>
    public class ConversionContext
    {
        public ConversionRequest Request { get; set; } = null!;
        public object? SourceData { get; set; }
        public object? TargetData { get; set; }
        public Dictionary<string, object> State { get; set; } = new Dictionary<string, object>();
        public List<string> Warnings { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// 管道步骤结果
    /// </summary>
    public class PipelineStepResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// 批量转换请求
    /// </summary>
    public class BatchConversionRequest
    {
        public List<ConversionRequest> Conversions { get; set; } = new List<ConversionRequest>();
        public bool ParallelProcessing { get; set; } = true;
        public int MaxDegreeOfParallelism { get; set; } = 4;
        public Dictionary<string, object> GlobalOptions { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 批量转换结果
    /// </summary>
    public class BatchConversionResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int TotalCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public TimeSpan Duration { get; set; }
        public List<ConversionResult> Results { get; set; } = new List<ConversionResult>();
        public Dictionary<string, object> Statistics { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 表格转换选项
    /// </summary>
    public class TableConversionOptions
    {
        public bool FlattenNestedElements { get; set; } = true;
        public string NestedElementSeparator { get; set; } = "_";
        public bool IncludeAttributes { get; set; } = true;
        public bool IncludeEmptyValues { get; set; } = false;
        public string? RootElementName { get; set; }
        public string? RowElementName { get; set; }
        public bool PreserveXmlStructure { get; set; } = false;
        public int MaxDepth { get; set; } = 10;
        public List<string> ExcludedElements { get; set; } = new List<string>();
        public List<string> IncludedAttributes { get; set; } = new List<string>();
    }

    /// <summary>
    /// CSV转换选项
    /// </summary>
    public class CsvConversionOptions
    {
        public string Delimiter { get; set; } = ",";
        public bool IncludeHeaders { get; set; } = true;
        public System.Text.Encoding Encoding { get; set; } = System.Text.Encoding.UTF8;
        public string QuoteCharacter { get; set; } = "\"";
        public bool EscapeQuotes { get; set; } = true;
        public bool IncludeBom { get; set; } = false;
        public TableConversionOptions? TableOptions { get; set; }
    }

    /// <summary>
    /// JSON转换选项
    /// </summary>
    public class JsonConversionOptions
    {
        public bool PrettyPrint { get; set; } = true;
        public bool IncludeXmlMetadata { get; set; } = false;
        public bool PreserveArrays { get; set; } = true;
        public string DateFormat { get; set; } = "yyyy-MM-ddTHH:mm:ss.fffZ";
        public bool ConvertNullToEmpty { get; set; } = false;
        public TableConversionOptions? TableOptions { get; set; }
    }

    /// <summary>
    /// 文件格式类型
    /// </summary>
    public enum FileFormatType
    {
        Unknown,
        Xml,
        Excel,
        Csv,
        Json,
        Table
    }

    /// <summary>
    /// 转换方向
    /// </summary>
    public enum ConversionDirection
    {
        XmlToTable,
        TableToXml,
        XmlToCsv,
        CsvToXml,
        XmlToJson,
        JsonToXml,
        XmlToExcel,
        ExcelToXml,
        CsvToExcel,
        ExcelToCsv,
        CsvToJson,
        JsonToCsv
    }
}