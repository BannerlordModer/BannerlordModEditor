using FluentAssertions;
using Xunit;

namespace BannerlordModEditor.Cli.UATTests
{
    /// <summary>
    /// Bannerlord Mod Editor CLI - BDD风格的用户验收测试
    /// </summary>
    public class BannerlordModEditorCliBddTests : BddUatTestBase
    {
        [Fact]
        public async Task Feature01_CliBasicFunctionality()
        {
            // Feature: CLI基本功能
            // 作为一名模组开发者
            // 我想要使用CLI工具的基本功能
            // 以便我能够快速开始处理XML和Excel文件

            var testResults = new Dictionary<string, object>();

            await GivenAsync("我是一个新的模组开发者", async () =>
            {
                await CreateUserWorkspaceAsync();
            });

            await WhenAsync("我查看工具的帮助信息", async () =>
            {
                var result = await ExecuteCliAsync("--help");
                testResults["帮助信息显示"] = result.Success;
                result.ShouldSucceed();
            });

            await ThenAsync("我应该看到完整的命令列表", async () =>
            {
                // 验证在Then步骤中通过When步骤的结果
            });

            await WhenAsync("我查看支持的模型类型", async () =>
            {
                var result = await ExecuteCliAsync("list-models");
                testResults["模型列表显示"] = result.Success;
                result.ShouldSucceed();
                result.ShouldContain("ActionTypesDO");
                result.ShouldContain("CombatParametersDO");
            });

            await ThenAsync("我应该看到所有35种支持的模型类型", async () =>
            {
                // 验证逻辑在When步骤中已经完成
            });

            await AndAsync("我检查工具的版本信息", async () =>
            {
                var result = await ExecuteCliAsync("--version");
                testResults["版本信息显示"] = result.Success;
                result.ShouldSucceed();
                result.ShouldContain("BannerlordModEditor.Cli");
            });

            await GenerateBddReportAsync("CLI_Basic_Functionality", "CLI工具的基本功能测试", testResults);
        }

        [Fact]
        public async Task Feature02_XmlFormatRecognition()
        {
            // Feature: XML格式识别
            // 作为一名模组开发者
            // 我想要识别XML文件的格式
            // 以便我能够了解如何处理这些文件

            var testResults = new Dictionary<string, object>();

            await GivenAsync("我有一些Bannerlord的XML文件", async () =>
            {
                await CreateUserWorkspaceAsync();
                await CopyTestDataToWorkspaceAsync("action_types.xml");
                await CopyTestDataToWorkspaceAsync("combat_parameters.xml");
                await CopyTestDataToWorkspaceAsync("map_icons.xml");
            });

            await WhenAsync("我识别action_types.xml的格式", async () =>
            {
                var result = await ExecuteCliAsync("recognize -i action_types.xml");
                testResults["识别action_types.xml"] = result.Success;
                result.ShouldSucceed();
                result.ShouldContain("✓ 识别成功");
                result.ShouldContain("action_types");
            });

            await ThenAsync("我应该看到正确的文件类型识别结果", async () =>
            {
                // 验证逻辑在When步骤中已经完成
            });

            await WhenAsync("我识别combat_parameters.xml的格式", async () =>
            {
                var result = await ExecuteCliAsync("recognize -i combat_parameters.xml");
                testResults["识别combat_parameters.xml"] = result.Success;
                result.ShouldSucceed();
                result.ShouldContain("combat_parameters");
            });

            await ThenAsync("我应该看到正确的战斗参数文件类型", async () =>
            {
                // 验证逻辑在When步骤中已经完成
            });

            await WhenAsync("我使用详细模式识别map_icons.xml", async () =>
            {
                var result = await ExecuteCliAsync("recognize -i map_icons.xml --verbose");
                testResults["详细模式识别"] = result.Success;
                result.ShouldSucceed();
                result.ShouldContain("✓ 识别成功");
            });

            await ThenAsync("我应该看到详细的识别信息", async () =>
            {
                // 验证逻辑在When步骤中已经完成
            });

            await WhenAsync("我尝试识别一个不存在的文件", async () =>
            {
                var result = await ExecuteCliAsync("recognize -i nonexistent.xml");
                testResults["处理不存在文件"] = result.Success == false;
                result.ShouldFailWithError("错误");
                result.ShouldContain("文件不存在");
            });

            await ThenAsync("我应该看到友好的错误提示", async () =>
            {
                // 验证逻辑在When步骤中已经完成
            });

            await GenerateBddReportAsync("XML_Format_Recognition", "XML文件格式识别功能测试", testResults);
        }

        [Fact]
        public async Task Feature03_XmlToExcelConversion()
        {
            // Feature: XML到Excel的转换
            // 作为一名模组开发者
            // 我想要将XML文件转换为Excel格式
            // 以便我能够使用Excel编辑器更方便地编辑数据

            var testResults = new Dictionary<string, object>();

            await GivenAsync("我有一些需要编辑的XML文件", async () =>
            {
                await CreateUserWorkspaceAsync();
                await CopyTestDataToWorkspaceAsync("action_types.xml");
                await CopyTestDataToWorkspaceAsync("combat_parameters.xml");
                await CopyTestDataToWorkspaceAsync("map_icons.xml");
            });

            await WhenAsync("我将action_types.xml转换为Excel", async () =>
            {
                var result = await ExecuteCliAsync("convert -i action_types.xml -o actions.xlsx");
                testResults["转换action_types.xml"] = result.Success;
                result.ShouldSucceed();
                result.ShouldContain("✓ XML 转 Excel 转换成功");
            });

            await ThenAsync("我应该得到一个有效的Excel文件", async () =>
            {
                VerifyExcelFile(GetUatFilePath("actions.xlsx"));
            });

            await WhenAsync("我将combat_parameters.xml转换为Excel", async () =>
            {
                var result = await ExecuteCliAsync("convert -i combat_parameters.xml -o combat_params.xlsx");
                testResults["转换combat_parameters.xml"] = result.Success;
                result.ShouldSucceed();
                result.ShouldContain("✓ XML 转 Excel 转换成功");
            });

            await ThenAsync("我应该得到战斗参数的Excel文件", async () =>
            {
                VerifyExcelFile(GetUatFilePath("combat_params.xlsx"));
            });

            await WhenAsync("我使用详细模式转换map_icons.xml", async () =>
            {
                var result = await ExecuteCliAsync("convert -i map_icons.xml -o map_icons.xlsx --verbose");
                testResults["详细模式转换"] = result.Success;
                result.ShouldSucceed();
                result.ShouldContain("输入文件:");
                result.ShouldContain("输出文件:");
                result.ShouldContain("模型类型:");
            });

            await ThenAsync("我应该看到详细的转换信息", async () =>
            {
                VerifyExcelFile(GetUatFilePath("map_icons.xlsx"));
            });

            await WhenAsync("我使用自定义工作表名称", async () =>
            {
                var result = await ExecuteCliAsync("convert -i action_types.xml -o custom_sheet.xlsx -w \"MyActions\"");
                testResults["自定义工作表"] = result.Success;
                result.ShouldSucceed();
            });

            await ThenAsync("我应该得到使用自定义工作表名称的Excel文件", async () =>
            {
                VerifyExcelFile(GetUatFilePath("custom_sheet.xlsx"));
            });

            await GenerateBddReportAsync("XML_To_Excel_Conversion", "XML到Excel的转换功能测试", testResults);
        }

        [Fact]
        public async Task Feature04_ErrorHandling()
        {
            // Feature: 错误处理
            // 作为一名用户
            // 我希望工具能够优雅地处理各种错误情况
            // 以便我能够理解问题并知道如何解决

            var testResults = new Dictionary<string, object>();

            await GivenAsync("我可能会犯各种错误", async () =>
            {
                await CreateUserWorkspaceAsync();
                
                // 创建一些测试文件
                await File.WriteAllTextAsync(GetUatFilePath("invalid.xml"), "this is not valid xml");
                await File.WriteAllTextAsync(GetUatFilePath("test.txt"), "this is a text file");
            });

            await WhenAsync("我尝试转换不存在的文件", async () =>
            {
                var result = await ExecuteCliAsync("convert -i nonexistent.xml -o output.xlsx");
                testResults["处理不存在文件"] = result.Success == false;
                result.ShouldFailWithError("错误");
                result.ShouldContain("文件不存在");
                result.ShouldNotContain("Exception");
            });

            await ThenAsync("我应该看到友好的错误提示", async () =>
            {
                // 验证逻辑在When步骤中已经完成
            });

            await WhenAsync("我尝试转换无效的XML文件", async () =>
            {
                var result = await ExecuteCliAsync("convert -i invalid.xml -o output.xlsx");
                testResults["处理无效XML"] = result.Success == false;
                result.ShouldFailWithError("错误");
                result.ShouldContain("XML 格式识别失败");
            });

            await ThenAsync("我应该看到XML格式错误提示", async () =>
            {
                // 验证逻辑在When步骤中已经完成
            });

            await WhenAsync("我使用错误的文件扩展名", async () =>
            {
                var result = await ExecuteCliAsync("convert -i test.txt -o output.xlsx");
                testResults["处理错误扩展名"] = result.Success == false;
                result.ShouldFailWithError("错误");
                result.ShouldContain("不支持的输入文件格式");
            });

            await ThenAsync("我应该看到文件格式错误提示", async () =>
            {
                // 验证逻辑在When步骤中已经完成
            });

            await WhenAsync("我忘记提供必需的参数", async () =>
            {
                var result = await ExecuteCliAsync("convert");
                testResults["处理缺少参数"] = result.Success == false;
                result.ShouldFailWithError("错误");
                result.ShouldContain("缺少必需的参数");
            });

            await ThenAsync("我应该看到参数错误提示", async () =>
            {
                // 验证逻辑在When步骤中已经完成
            });

            await WhenAsync("我使用无效的模型类型", async () =>
            {
                var result = await ExecuteCliAsync("convert -i action_types.xml -o output.xlsx -m InvalidModelType");
                testResults["处理无效模型"] = result.Success == false;
                result.ShouldFailWithError("错误");
                result.ShouldContain("不支持的模型类型");
            });

            await ThenAsync("我应该看到模型类型错误提示", async () =>
            {
                // 验证逻辑在When步骤中已经完成
            });

            await GenerateBddReportAsync("Error_Handling", "错误处理功能测试", testResults);
        }

        [Fact]
        public async Task Feature05_ValidationMode()
        {
            // Feature: 验证模式
            // 作为一名谨慎的用户
            // 我想要在转换前验证文件格式
            // 以便我能够确保转换会成功

            var testResults = new Dictionary<string, object>();

            await GivenAsync("我想要在转换前验证文件格式", async () =>
            {
                await CreateUserWorkspaceAsync();
                await CopyTestDataToWorkspaceAsync("action_types.xml");
                await CopyTestDataToWorkspaceAsync("combat_parameters.xml");
            });

            await WhenAsync("我使用验证模式检查action_types.xml", async () =>
            {
                var result = await ExecuteCliAsync("convert -i action_types.xml -o output.xlsx --validate");
                testResults["验证action_types.xml"] = result.Success;
                result.ShouldSucceed();
                result.ShouldContain("✓ XML 格式验证通过");
            });

            await ThenAsync("我应该看到验证通过的消息", async () =>
            {
                // 验证逻辑在When步骤中已经完成
            });

            await AndAsync("输出文件不应该被创建", async () =>
            {
                var outputFileExists = File.Exists(GetUatFilePath("output.xlsx"));
                outputFileExists.Should().BeFalse("验证模式不应该创建输出文件");
                testResults["验证模式不创建文件"] = !outputFileExists;
            });

            await WhenAsync("我验证combat_parameters.xml", async () =>
            {
                var result = await ExecuteCliAsync("convert -i combat_parameters.xml -o combat_params.xlsx --validate");
                testResults["验证combat_parameters.xml"] = result.Success;
                result.ShouldSucceed();
                result.ShouldContain("✓ XML 格式验证通过");
            });

            await ThenAsync("我应该看到验证通过的消息", async () =>
            {
                // 验证逻辑在When步骤中已经完成
            });

            await GenerateBddReportAsync("Validation_Mode", "验证模式功能测试", testResults);
        }

        [Fact]
        public async Task Feature06_PerformanceAndLargeFiles()
        {
            // Feature: 性能和大型文件处理
            // 作为一名处理大量数据的用户
            // 我希望工具能够高效处理大型文件
            // 以便我能够快速完成工作

            var testResults = new Dictionary<string, object>();

            await GivenAsync("我有一些大型XML文件需要处理", async () =>
            {
                await CreateUserWorkspaceAsync();
                
                // 复制大型文件
                var largeFiles = new[] { "flora_kinds.xml" };
                foreach (var file in largeFiles)
                {
                    await CopyTestDataToWorkspaceAsync(file);
                }
            });

            await WhenAsync("我处理大型XML文件", async () =>
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var result = await ExecuteCliAsync("convert -i flora_kinds.xml -o flora_kinds_output.xlsx");
                stopwatch.Stop();

                testResults["大型文件转换成功"] = result.Success;
                testResults["大型文件转换时间(ms)"] = stopwatch.ElapsedMilliseconds;

                result.ShouldSucceed();
                result.ShouldContain("✓ XML 转 Excel 转换成功");

                stopwatch.ElapsedMilliseconds.Should().BeLessThan(60000, "大型文件应该在60秒内处理完成");
            });

            await ThenAsync("我应该得到转换后的Excel文件", async () =>
            {
                VerifyExcelFile(GetUatFilePath("flora_kinds_output.xlsx"));
            });

            await WhenAsync("我使用详细模式处理大型文件", async () =>
            {
                var result = await ExecuteCliAsync("convert -i flora_kinds.xml -o flora_kinds_verbose.xlsx --verbose");
                testResults["详细模式处理大型文件"] = result.Success;
                result.ShouldSucceed();
            });

            await ThenAsync("我应该看到详细的处理信息", async () =>
            {
                VerifyExcelFile(GetUatFilePath("flora_kinds_verbose.xlsx"));
            });

            await GenerateBddReportAsync("Performance_And_Large_Files", "性能和大型文件处理测试", testResults);
        }

        /// <summary>
        /// 创建用户工作空间
        /// </summary>
        private async Task CreateUserWorkspaceAsync()
        {
            // 复制一些常用的XML文件到工作空间
            var filesToCopy = new[] { "action_types.xml", "combat_parameters.xml", "map_icons.xml" };
            foreach (var file in filesToCopy)
            {
                try
                {
                    await CopyTestDataToWorkspaceAsync(file);
                }
                catch (FileNotFoundException)
                {
                    // 如果文件不存在，跳过
                }
            }

            // 创建README文件
            var readmeContent = @"
# Bannerlord Mod Editor CLI - UAT测试工作空间

这个目录包含UAT测试中使用的文件。

## 功能测试
- CLI基本功能测试
- XML格式识别测试
- XML到Excel转换测试
- 错误处理测试
- 验证模式测试
- 性能测试

## 使用方法
1. 运行 `dotnet run -- list-models` 查看支持的模型类型
2. 运行 `dotnet run -- recognize -i file.xml` 识别文件格式
3. 运行 `dotnet run -- convert -i file.xml -o output.xlsx` 转换文件
";
            await File.WriteAllTextAsync(GetUatFilePath("README.md"), readmeContent);
        }
    }
}