using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BannerlordModEditor.TUI.Services
{
    public interface IFormatConversionService
    {
        Task<ConversionResult> ExcelToXmlAsync(string excelFilePath, string xmlFilePath, ConversionOptions? options = null);
        Task<ConversionResult> XmlToExcelAsync(string xmlFilePath, string excelFilePath, ConversionOptions? options = null);
        Task<FileFormatInfo> DetectFileFormatAsync(string filePath);
        Task<ValidationResult> ValidateConversionAsync(string sourceFilePath, string targetFilePath, ConversionDirection direction);
    }

    public enum ConversionDirection
    {
        ExcelToXml,
        XmlToExcel
    }

    public class ConversionResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? OutputPath { get; set; }
        public int RecordsProcessed { get; set; }
        public TimeSpan Duration { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class ConversionOptions
    {
        public bool IncludeSchemaValidation { get; set; } = true;
        public bool PreserveFormatting { get; set; } = true;
        public bool CreateBackup { get; set; } = true;
        public string? WorksheetName { get; set; }
        public string? RootElementName { get; set; }
        public string? RowElementName { get; set; }
        public bool FlattenNestedElements { get; set; } = false;
        public string? NestedElementSeparator { get; set; } = "_";
    }

    public class FileFormatInfo
    {
        public FileFormatType FormatType { get; set; }
        public string? RootElement { get; set; }
        public List<string> ColumnNames { get; set; } = new List<string>();
        public int RowCount { get; set; }
        public bool IsSupported { get; set; }
        public string? FormatDescription { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }

    public enum FileFormatType
    {
        Unknown,
        Excel,
        Xml,
        Csv
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string? Message { get; set; }
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
        public List<ValidationWarning> Warnings { get; set; } = new List<ValidationWarning>();
    }

    public class ValidationError
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
        public ValidationErrorType ErrorType { get; set; }
    }

    public class ValidationWarning
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
        public ValidationWarningType WarningType { get; set; }
    }

    public enum ValidationErrorType
    {
        SchemaMismatch,
        DataTypeMismatch,
        MissingRequiredField,
        InvalidFormat,
        StructureMismatch
    }

    public enum ValidationWarningType
    {
        DataTruncation,
        FormatConversion,
        EmptyField,
        OptionalFieldMissing
    }
}