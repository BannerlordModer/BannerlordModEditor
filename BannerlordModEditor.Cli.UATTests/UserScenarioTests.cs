using FluentAssertions;
using Xunit;

namespace BannerlordModEditor.Cli.UATTests
{
    /// <summary>
    /// 真实用户场景的UAT测试
    /// </summary>
    public class UserScenarioTests : UatTestBase, IDisposable
    {
        public UserScenarioTests()
        {
            // 创建用户场景数据
            CreateUserScenarioDataAsync().Wait();
        }

        public void Dispose()
        {
            Cleanup();
        }

        [Fact]
        public async Task Scenario01_FirstTimeUser_ExploringAvailableModels()
        {
            // 场景：新用户第一次使用工具，想要了解支持的模型类型

            // Arrange - 用户想要了解工具支持哪些模型
            var testResults = new Dictionary<string, object>();

            // Act - 用户查看帮助信息
            var helpResult = await ExecuteCliCommandAsync("--help");
            testResults["帮助信息显示"] = helpResult.Success;

            // 用户查看支持的模型类型
            var modelsResult = await ExecuteCliCommandAsync("list-models");
            testResults["模型列表显示"] = modelsResult.Success;

            // 用户想要了解更多关于特定模型的信息
            var modelCount = modelsResult.StandardOutput.Split('\n')
                .Count(line => line.Trim().StartsWith("- "));
            testResults["支持的模型数量"] = modelCount;

            // Assert - 验证用户体验
            helpResult.ShouldSucceed("用户应该能够看到帮助信息");
            helpResult.ShouldContain("convert", "帮助信息应该包含convert命令");
            helpResult.ShouldContain("list-models", "帮助信息应该包含list-models命令");
            helpResult.ShouldContain("recognize", "帮助信息应该包含recognize命令");

            modelsResult.ShouldSucceed("用户应该能够看到模型列表");
            modelsResult.ShouldContain("ActionTypesDO", "模型列表应该包含ActionTypesDO");
            modelsResult.ShouldContain("CombatParametersDO", "模型列表应该包含CombatParametersDO");
            modelsResult.ShouldContain("总计", "模型列表应该显示总数");

            modelCount.Should().BeGreaterThan(30, "应该支持至少30种模型类型");

            // 创建测试报告
            await CreateTestReportAsync("FirstTimeUser_ExploringModels", testResults);
        }

        [Fact]
        public async Task Scenario02_ModDeveloper_ConvertingXmlFilesToExcel()
        {
            // 场景：模组开发者需要将XML文件转换为Excel以便于编辑

            // Arrange - 开发者有一组XML文件需要转换
            var testResults = new Dictionary<string, object>();

            // Act - 开发者识别XML文件类型
            var recognizeResult = await ExecuteCliCommandAsync("recognize -i action_types.xml");
            testResults["识别action_types.xml"] = recognizeResult.Success;

            // 开发者转换XML到Excel
            var convertResult1 = await ExecuteCliCommandAsync("convert -i action_types.xml -o actions.xlsx");
            testResults["转换action_types.xml"] = convertResult1.Success;

            // 开发者处理另一个文件
            var recognizeResult2 = await ExecuteCliCommandAsync("recognize -i combat_parameters.xml");
            testResults["识别combat_parameters.xml"] = recognizeResult2.Success;

            var convertResult2 = await ExecuteCliCommandAsync("convert -i combat_parameters.xml -o combat_params.xlsx");
            testResults["转换combat_parameters.xml"] = convertResult2.Success;

            // 开发者使用详细模式
            var verboseResult = await ExecuteCliCommandAsync("convert -i map_icons.xml -o map_icons.xlsx --verbose");
            testResults["详细模式转换"] = verboseResult.Success;

            // 验证输出文件
            var actionsExcelExists = File.Exists(GetUatFilePath("actions.xlsx"));
            var combatExcelExists = File.Exists(GetUatFilePath("combat_params.xlsx"));
            var mapIconsExcelExists = File.Exists(GetUatFilePath("map_icons.xlsx"));
            
            testResults["actions.xlsx创建成功"] = actionsExcelExists;
            testResults["combat_params.xlsx创建成功"] = combatExcelExists;
            testResults["map_icons.xlsx创建成功"] = mapIconsExcelExists;

            // Assert - 验证转换结果
            recognizeResult.ShouldSucceed("应该能够识别action_types.xml");
            recognizeResult.ShouldContain("action_types", "应该正确识别文件类型");

            convertResult1.ShouldSucceed("应该能够转换action_types.xml");
            convertResult1.ShouldContain("✓ XML 转 Excel 转换成功", "应该显示转换成功消息");

            recognizeResult2.ShouldSucceed("应该能够识别combat_parameters.xml");
            convertResult2.ShouldSucceed("应该能够转换combat_parameters.xml");

            verboseResult.ShouldSucceed("详细模式应该工作正常");
            verboseResult.ShouldContain("输入文件:", "详细模式应该显示输入文件信息");
            verboseResult.ShouldContain("输出文件:", "详细模式应该显示输出文件信息");

            actionsExcelExists.Should().BeTrue("应该创建actions.xlsx文件");
            combatExcelExists.Should().BeTrue("应该创建combat_params.xlsx文件");
            mapIconsExcelExists.Should().BeTrue("应该创建map_icons.xlsx文件");

            // 验证Excel文件格式
            VerifyExcelFile(GetUatFilePath("actions.xlsx"));
            VerifyExcelFile(GetUatFilePath("combat_params.xlsx"));
            VerifyExcelFile(GetUatFilePath("map_icons.xlsx"));

            // 创建测试报告
            await CreateTestReportAsync("ModDeveloper_ConvertingXmlToExcel", testResults);
        }

        [Fact]
        public async Task Scenario03_AdvancedUser_BatchProcessing()
        {
            // 场景：高级用户需要批量处理多个文件

            // Arrange - 用户有多个文件需要处理
            var testResults = new Dictionary<string, object>();

            // Act - 用户创建批处理脚本
            var batchScript = @"
#!/bin/bash
echo '开始批量处理...'

# 处理所有XML文件
for file in *.xml; do
    echo ""处理文件: $file""
    if [ -f ""$file"" ]; then
        dotnet run --project ../BannerlordModEditor.Cli recognize -i ""$file""
        if [ $? -eq 0 ]; then
            dotnet run --project ../BannerlordModEditor.Cli convert -i ""$file"" -o ""${file%.xml}.xlsx""
        fi
    fi
done

echo '批量处理完成'
";
            var batchScriptPath = GetUatFilePath("batch_process.sh");
            await File.WriteAllTextAsync(batchScriptPath, batchScript);
            testResults["批处理脚本创建"] = true;

            // 用户执行单个命令测试
            var testFiles = new[] { "action_types.xml", "combat_parameters.xml", "map_icons.xml" };
            var conversionResults = new List<bool>();

            foreach (var file in testFiles)
            {
                var result = await ExecuteCliCommandAsync($"convert -i {file} -o {file.Replace(".xml", "_batch.xlsx")}");
                conversionResults.Add(result.Success);
                testResults[$"{file}转换成功"] = result.Success;
            }

            // 用户验证所有输出文件
            var outputFiles = Directory.GetFiles(_uatWorkspace, "*_batch.xlsx");
            testResults["生成的Excel文件数量"] = outputFiles.Length;

            // Assert - 验证批处理结果
            conversionResults.Should().AllBeTrue("所有文件都应该成功转换");
            outputFiles.Length.Should().Be(3, "应该生成3个Excel文件");

            foreach (var file in outputFiles)
            {
                VerifyExcelFile(file);
            }

            // 创建测试报告
            await CreateTestReportAsync("AdvancedUser_BatchProcessing", testResults);
        }

        [Fact]
        public async Task Scenario04_ErrorHandling_UserMakesMistakes()
        // 场景：用户使用工具时犯各种错误

        // Arrange - 用户可能会犯的错误
        var testResults = new Dictionary<string, object>();

        // Act - 用户尝试转换不存在的文件
        var missingFileResult = await ExecuteCliCommandAsync("convert -i nonexistent.xml -o output.xlsx");
        testResults["不存在的文件处理"] = missingFileResult.Success == false;

        // 用户使用错误的文件扩展名
        var wrongExtResult = await ExecuteCliCommandAsync("convert -i action_types.txt -o output.xlsx");
        testResults["错误扩展名处理"] = wrongExtResult.Success == false;

        // 用户忘记必需的参数
        var missingParamsResult = await ExecuteCliCommandAsync("convert");
        testResults["缺少参数处理"] = missingParamsResult.Success == false;

        // 用户使用无效的模型类型
        var invalidModelResult = await ExecuteCliCommandAsync("convert -i action_types.xml -o output.xlsx -m InvalidModel");
        testResults["无效模型类型处理"] = invalidModelResult.Success == false;

        // 用户尝试转换损坏的XML文件
        var corruptedXmlPath = GetUatFilePath("corrupted.xml");
        await File.WriteAllTextAsync(corruptedXmlPath, "this is not valid xml");
        var corruptedResult = await ExecuteCliCommandAsync($"convert -i corrupted.xml -o output.xlsx");
        testResults["损坏XML处理"] = corruptedResult.Success == false;

        // Assert - 验证错误处理
        missingFileResult.ShouldFailWithError("错误", "应该处理不存在的文件");
        missingFileResult.ShouldContain("文件不存在", "应该显示文件不存在的错误");

        wrongExtResult.ShouldFailWithError("错误", "应该处理错误的文件扩展名");
        wrongExtResult.ShouldContain("不支持的输入文件格式", "应该显示格式错误");

        missingParamsResult.ShouldFailWithError("错误", "应该处理缺少参数的情况");
        missingParamsResult.ShouldContain("缺少必需的参数", "应该显示参数错误");

        invalidModelResult.ShouldFailWithError("错误", "应该处理无效的模型类型");
        invalidModelResult.ShouldContain("不支持的模型类型", "应该显示模型类型错误");

        corruptedResult.ShouldFailWithError("错误", "应该处理损坏的XML文件");
        corruptedResult.ShouldContain("XML 格式识别失败", "应该显示XML格式错误");

        // 验证错误消息的友好性
        var errorMessages = new[]
        {
            missingFileResult.AllOutput,
            wrongExtResult.AllOutput,
            missingParamsResult.AllOutput,
            invalidModelResult.AllOutput,
            corruptedResult.AllOutput
        };

        foreach (var message in errorMessages)
        {
            message.Should().NotContain("Exception", "错误消息不应该包含技术性异常信息");
            message.Should().NotContain("Stack", "错误消息不应该包含堆栈跟踪");
        }

        // 创建测试报告
        await CreateTestReportAsync("ErrorHandling_UserMistakes", testResults);
    }

    [Fact]
    public async Task Scenario05_RealWorldWorkflow_CompleteDevelopmentCycle()
        {
            // 场景：完整的开发工作流程 - 从XML到Excel再回到XML

            // Arrange - 开发者需要修改现有的模组数据
            var testResults = new Dictionary<string, object>();

            // Act - 1. 开发者识别和备份原始文件
            var originalFile = "action_types.xml";
            var backupResult = await ExecuteCliCommandAsync($"convert -i {originalFile} -o {originalFile.Replace(".xml", "_backup.xlsx")}");
            testResults["原始文件备份"] = backupResult.Success;

            // 2. 开发者将XML转换为Excel进行编辑
            var convertToExcelResult = await ExecuteCliCommandAsync($"convert -i {originalFile} -o {originalFile.Replace(".xml", "_editing.xlsx")}");
            testResults["转换为Excel用于编辑"] = convertToExcelResult.Success;

            // 3. 开发者验证Excel文件（模拟编辑过程）
            var excelFilePath = GetUatFilePath(originalFile.Replace(".xml", "_editing.xlsx"));
            var excelExists = File.Exists(excelFilePath);
            testResults["Excel文件存在"] = excelExists;

            // 4. 开发者尝试将Excel转回XML（注意：这个功能可能还在开发中）
            var convertToXmlResult = await ExecuteCliCommandAsync($"convert -i {originalFile.Replace(".xml", "_editing.xlsx")} -o {originalFile.Replace(".xml", "_modified.xml")} -m ActionTypesDO");
            testResults["Excel转XML尝试"] = convertToXmlResult.Success;

            // 5. 开发者比较原始文件和生成文件的差异
            var originalXmlContent = await File.ReadAllTextAsync(GetUatFilePath(originalFile));
            var modifiedXmlExists = File.Exists(GetUatFilePath(originalFile.Replace(".xml", "_modified.xml")));
            testResults["修改后的XML文件存在"] = modifiedXmlExists;

            // 6. 开发者使用验证功能
            var validateResult = await ExecuteCliCommandAsync($"convert -i {originalFile} -o temp.xlsx --validate");
            testResults["验证功能使用"] = validateResult.Success;

            // Assert - 验证完整工作流程
            backupResult.ShouldSucceed("应该能够备份原始文件");
            convertToExcelResult.ShouldSucceed("应该能够转换为Excel");
            excelExists.Should().BeTrue("应该生成Excel文件");

            // 注意：Excel到XML的转换可能失败，这是预期的
            if (convertToXmlResult.Success)
            {
                testResults["Excel到XML转换成功"] = true;
                modifiedXmlExists.Should().BeTrue("应该生成修改后的XML文件");
            }
            else
            {
                testResults["Excel到XML转换成功"] = false;
                convertToXmlResult.ShouldNotContain("严重错误", "即使失败也不应该有严重错误");
            }

            validateResult.ShouldSucceed("应该能够使用验证功能");
            validateResult.ShouldContain("✓ XML 格式验证通过", "验证功能应该正常工作");

            // 创建测试报告
            await CreateTestReportAsync("RealWorldWorkflow_CompleteCycle", testResults);
        }

        [Fact]
        public async Task Scenario06_Performance_UserWithLargeFiles()
        {
            // 场景：用户处理大型XML文件

            // Arrange - 用户有大型XML文件需要处理
            var testResults = new Dictionary<string, object>();

            // Act - 用户处理大型文件
            var largeFiles = new[] { "flora_kinds.xml" };
            var processingTimes = new List<long>();

            foreach (var file in largeFiles)
            {
                if (File.Exists(GetUatFilePath(file)))
                {
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                    var result = await ExecuteCliCommandAsync($"convert -i {file} -o {file.Replace(".xml", "_large.xlsx")}");
                    stopwatch.Stop();

                    processingTimes.Add(stopwatch.ElapsedMilliseconds);
                    testResults[$"{file}处理成功"] = result.Success;
                    testResults[$"{file}处理时间(ms)"] = stopwatch.ElapsedMilliseconds;
                }
            }

            // 用户使用详细模式处理大型文件
            var verboseResult = await ExecuteCliCommandAsync("convert -i flora_kinds.xml -o flora_kinds_verbose.xlsx --verbose");
            testResults["详细模式处理大型文件"] = verboseResult.Success;

            // Assert - 验证性能和处理结果
            processingTimes.Should().AllBeLessThan(60000, "所有大型文件都应该在60秒内处理完成");

            if (processingTimes.Any())
            {
                var averageTime = processingTimes.Average();
                testResults["平均处理时间(ms)"] = averageTime;
                averageTime.Should().BeLessThan(30000, "平均处理时间应该小于30秒");
            }

            verboseResult.ShouldSucceed("详细模式应该能够处理大型文件");

            // 创建测试报告
            await CreateTestReportAsync("Performance_LargeFiles", testResults);
        }
    }
}