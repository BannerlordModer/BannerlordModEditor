using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using BannerlordModEditor.Cli.Services;

namespace BannerlordModEditor.Cli.Commands
{
    /// <summary>
    /// 格式识别命令
    /// </summary>
    [Command("recognize", Description = "识别 XML 文件格式")]
    public class RecognizeCommand : ICommand
    {
        private readonly IExcelXmlConverterService _converterService;
        private readonly ErrorMessageService _errorMessageService;

        public RecognizeCommand(IExcelXmlConverterService converterService, ErrorMessageService errorMessageService)
        {
            _converterService = converterService;
            _errorMessageService = errorMessageService;
        }

        [CommandOption("input", 'i', Description = "XML 文件路径")]
        public required string InputFile { get; set; }

        [CommandOption("verbose", Description = "显示详细信息")]
        public bool Verbose { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            try
            {
                if (!File.Exists(InputFile))
                {
                    console.Error.WriteLine($"错误：文件不存在 - {InputFile}");
                    return;
                }

                var fileExt = Path.GetExtension(InputFile).ToLowerInvariant();
                if (fileExt != ".xml")
                {
                    console.Error.WriteLine($"错误：不支持的文件格式 - {fileExt}，仅支持 .xml 文件");
                    return;
                }

                if (Verbose)
                {
                    console.Output.WriteLine($"识别文件: {InputFile}");
                }

                var modelType = await _converterService.RecognizeXmlFormatAsync(InputFile);

                if (!string.IsNullOrEmpty(modelType))
                {
                    console.Output.WriteLine($"✓ 识别成功: {modelType}");
                    
                    if (Verbose)
                    {
                        console.Output.WriteLine($"可以使用以下命令进行转换:");
                        console.Output.WriteLine($"  BannerlordModEditor.Cli convert -i \"{InputFile}\" -o \"output.xlsx\" -m {modelType}");
                    }
                }
                else
                {
                    console.Output.WriteLine("✗ 识别失败：无法识别 XML 格式");
                    
                    if (Verbose)
                    {
                        console.Output.WriteLine("请检查 XML 文件格式或手动指定模型类型");
                    }
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
                if (ex is XmlFormatException)
                {
                    console.Error.WriteLine("\n建议：");
                    console.Error.WriteLine("1. 检查 XML 文件格式是否正确");
                    console.Error.WriteLine("2. 确保 XML 文件有正确的根元素");
                    console.Error.WriteLine("3. 使用文本编辑器验证 XML 语法");
                }
            }
        }
    }
}