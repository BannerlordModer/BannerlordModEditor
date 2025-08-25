using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Services;
using ClosedXML.Excel;

namespace BannerlordModEditor.Cli.Services
{
    /// <summary>
    /// Excel 和 XML 转换服务接口
    /// </summary>
    public interface IExcelXmlConverterService
    {
        /// <summary>
        /// 将 Excel 文件转换为 XML 文件
        /// </summary>
        /// <param name="excelFilePath">Excel 文件路径</param>
        /// <param name="xmlFilePath">XML 文件路径</param>
        /// <param name="modelType">目标模型类型</param>
        /// <param name="worksheetName">工作表名称（可选）</param>
        /// <returns>转换是否成功</returns>
        Task<bool> ConvertExcelToXmlAsync(string excelFilePath, string xmlFilePath, string modelType, string? worksheetName = null);

        /// <summary>
        /// 将 XML 文件转换为 Excel 文件
        /// </summary>
        /// <param name="xmlFilePath">XML 文件路径</param>
        /// <param name="excelFilePath">Excel 文件路径</param>
        /// <param name="worksheetName">工作表名称（可选）</param>
        /// <returns>转换是否成功</returns>
        Task<bool> ConvertXmlToExcelAsync(string xmlFilePath, string excelFilePath, string? worksheetName = null);

        /// <summary>
        /// 识别 XML 文件格式
        /// </summary>
        /// <param name="xmlFilePath">XML 文件路径</param>
        /// <returns>识别出的模型类型</returns>
        Task<string?> RecognizeXmlFormatAsync(string xmlFilePath);

        /// <summary>
        /// 验证 Excel 文件格式是否符合指定的 XML 模型
        /// </summary>
        /// <param name="excelFilePath">Excel 文件路径</param>
        /// <param name="modelType">目标模型类型</param>
        /// <param name="worksheetName">工作表名称（可选）</param>
        /// <returns>验证结果</returns>
        Task<bool> ValidateExcelFormatAsync(string excelFilePath, string modelType, string? worksheetName = null);
    }

    /// <summary>
    /// Excel 数据结构
    /// </summary>
    public class ExcelData
    {
        public string? WorksheetName { get; set; }
        public List<string> Headers { get; set; } = new List<string>();
        public List<Dictionary<string, object?>> Rows { get; set; } = new List<Dictionary<string, object?>>();
        public Dictionary<string, string>? ColumnMappings { get; set; }
    }

    /// <summary>
    /// 转换结果
    /// </summary>
    public class ConversionResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? OutputPath { get; set; }
        public int RecordsProcessed { get; set; }
        public List<string>? Warnings { get; set; }
        public Exception? Exception { get; set; }
    }

    /// <summary>
    /// 格式识别结果
    /// </summary>
    public class FormatRecognitionResult
    {
        public string? ModelType { get; set; }
        public bool IsRecognized { get; set; }
        public double Confidence { get; set; }
        public string? Message { get; set; }
        public List<string>? MatchedElements { get; set; }
    }

    /// <summary>
    /// 转换配置
    /// </summary>
    public class ConversionConfig
    {
        public string ModelType { get; set; } = string.Empty;
        public string? WorksheetName { get; set; }
        public bool IncludeHeaders { get; set; } = true;
        public bool AutoDetectFormat { get; set; } = true;
        public bool StrictValidation { get; set; } = true;
        public Dictionary<string, string>? ColumnMappings { get; set; }
        public string? DateFormat { get; set; }
        public string? NumberFormat { get; set; }
    }

    /// <summary>
    /// Excel 操作异常
    /// </summary>
    public class ExcelOperationException : Exception
    {
        public ExcelOperationException(string message) : base(message) { }
        public ExcelOperationException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// XML 格式识别异常
    /// </summary>
    public class XmlFormatException : Exception
    {
        public XmlFormatException(string message) : base(message) { }
        public XmlFormatException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// 转换异常
    /// </summary>
    public class ConversionException : Exception
    {
        public ConversionException(string message) : base(message) { }
        public ConversionException(string message, Exception innerException) : base(message, innerException) { }
    }
}