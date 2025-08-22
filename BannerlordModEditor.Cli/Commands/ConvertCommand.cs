using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using BannerlordModEditor.Cli.Services;

namespace BannerlordModEditor.Cli.Commands
{
    /// <summary>
    /// Excel 和 XML 转换命令
    /// </summary>
    [Command("convert", Description = "Excel 和 XML 文件之间的相互转换")]
    public class ConvertCommand : ICommand
    {
        private readonly IExcelXmlConverterService _converterService;
        private readonly ErrorMessageService _errorMessageService;

        public ConvertCommand(IExcelXmlConverterService converterService, ErrorMessageService errorMessageService)
        {
            _converterService = converterService;
            _errorMessageService = errorMessageService;
        }

        [CommandOption("input", 'i', Description = "输入文件路径（Excel 或 XML）")]
        public required string InputFile { get; set; }

        [CommandOption("output", 'o', Description = "输出文件路径")]
        public required string OutputFile { get; set; }

        [CommandOption("model", 'm', Description = "模型类型（可选，自动检测）")]
        public string? ModelType { get; set; }

        [CommandOption("worksheet", 'w', Description = "工作表名称（可选）")]
        public string? WorksheetName { get; set; }

        [CommandOption("validate", 'v', Description = "仅验证格式，不执行转换")]
        public bool ValidateOnly { get; set; }

        [CommandOption("verbose", Description = "显示详细信息")]
        public bool Verbose { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            try
            {
                if (!File.Exists(InputFile))
                {
                    console.Error.WriteLine($"错误：输入文件不存在 - {InputFile}");
                    return;
                }

                var inputExt = Path.GetExtension(InputFile).ToLowerInvariant();
                var outputExt = Path.GetExtension(OutputFile).ToLowerInvariant();

                // 验证文件扩展名
                if (inputExt != ".xlsx" && inputExt != ".xml")
                {
                    console.Error.WriteLine($"错误：不支持的输入文件格式 - {inputExt}");
                    return;
                }

                if (outputExt != ".xlsx" && outputExt != ".xml")
                {
                    console.Error.WriteLine($"错误：不支持的输出文件格式 - {outputExt}");
                    return;
                }

                // 验证转换方向
                if (inputExt == outputExt)
                {
                    console.Error.WriteLine("错误：输入和输出文件格式相同");
                    return;
                }

                if (Verbose)
                {
                    console.Output.WriteLine($"输入文件: {InputFile}");
                    console.Output.WriteLine($"输出文件: {OutputFile}");
                    console.Output.WriteLine($"模型类型: {ModelType ?? "自动检测"}");
                    console.Output.WriteLine($"工作表: {WorksheetName ?? "默认"}");
                    console.Output.WriteLine($"验证模式: {ValidateOnly}");
                }

                bool success;

                if (inputExt == ".xlsx")
                {
                    // Excel 转 XML
                    if (string.IsNullOrEmpty(ModelType))
                    {
                        console.Error.WriteLine("错误：Excel 转 XML 需要指定模型类型");
                        return;
                    }

                    if (ValidateOnly)
                    {
                        success = await _converterService.ValidateExcelFormatAsync(InputFile, ModelType, WorksheetName);
                        console.Output.WriteLine(success ? "✓ Excel 格式验证通过" : "✗ Excel 格式验证失败");
                    }
                    else
                    {
                        success = await _converterService.ConvertExcelToXmlAsync(InputFile, OutputFile, ModelType, WorksheetName);
                        console.Output.WriteLine(success ? "✓ Excel 转 XML 转换成功" : "✗ Excel 转 XML 转换失败");
                    }
                }
                else
                {
                    // XML 转 Excel
                    if (string.IsNullOrEmpty(ModelType))
                    {
                        // 自动识别模型类型
                        ModelType = await _converterService.RecognizeXmlFormatAsync(InputFile);
                        if (string.IsNullOrEmpty(ModelType))
                        {
                            console.Error.WriteLine("错误：无法识别 XML 格式，请手动指定模型类型");
                            return;
                        }

                        if (Verbose)
                        {
                            console.Output.WriteLine($"识别的模型类型: {ModelType}");
                        }
                    }

                    if (ValidateOnly)
                    {
                        console.Output.WriteLine("✓ XML 格式验证通过");
                        success = true;
                    }
                    else
                    {
                        success = await _converterService.ConvertXmlToExcelAsync(InputFile, OutputFile, WorksheetName);
                        console.Output.WriteLine(success ? "✓ XML 转 Excel 转换成功" : "✗ XML 转 Excel 转换失败");
                    }
                }

                if (success && !ValidateOnly)
                {
                    console.Output.WriteLine($"输出文件: {OutputFile}");
                }
            }
            catch (Exception ex)
            {
                var friendlyMessage = _errorMessageService.GetUserFriendlyMessage(ex);
                console.Error.WriteLine($"错误：{friendlyMessage}");
                
                if (Verbose)
                {
                    console.Error.WriteLine($"详细错误信息：");
                    console.Error.WriteLine(_errorMessageService.GetDetailedError(ex));
                }
                
                // 提供额外的建议
                if (ex is ArgumentException || ex is ConversionException)
                {
                    var suggestion = _errorMessageService.GetConversionSuggestion(
                        Path.GetExtension(InputFile).ToLowerInvariant(),
                        Path.GetExtension(OutputFile).ToLowerInvariant(),
                        ModelType);
                    
                    console.Error.WriteLine("\n建议：");
                    console.Error.WriteLine(suggestion);
                }
            }
        }
    }
}