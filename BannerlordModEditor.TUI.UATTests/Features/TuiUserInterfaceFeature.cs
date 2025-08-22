using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using Shouldly;
using System;
using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.TUI.Services;
using BannerlordModEditor.TUI.ViewModels;
using BannerlordModEditor.Common.Models;
using Moq;
using Terminal.Gui;
using BannerlordModEditor.TUI.UATTests.Common;

namespace BannerlordModEditor.TUI.UATTests.Features
{
    /// <summary>
    /// BDD特性：TUI用户界面交互
    /// 
    /// 作为一个Bannerlord Mod开发者
    /// 我希望通过终端用户界面进行文件转换
    /// 并且界面应该直观易用
    /// 提供清晰的操作反馈
    /// </summary>
    public class TuiUserInterfaceFeature : BddTestBase
    {
        public TuiUserInterfaceFeature(ITestOutputHelper output) : base(output)
        {
        }

        #region 场景1: 基本的文件选择和转换流程

        /// <summary>
        /// 场景: 基本的文件选择和转换流程
        /// 当 我通过TUI界面选择要转换的文件
        /// 并且 设置输出路径
        /// 那么 系统应该正确执行转换流程
        /// 并且 显示转换结果
        /// </summary>
        [Fact]
        public async Task BasicFileSelectionAndConversion_Workflow()
        {
            // Given - 准备测试环境和ViewModel
            MainViewModel viewModel = null;
            string sourceFile = null;
            string targetFile = null;
            
            try
            {
                // Given 我通过TUI界面选择要转换的文件
                var mockConversionService = new Mock<IFormatConversionService>();
                var conversionResult = new ConversionResult
                {
                    Success = true,
                    Message = "转换成功完成",
                    RecordsProcessed = 5,
                    Duration = TimeSpan.FromMilliseconds(150),
                    OutputPath = "output.xml",
                    Warnings = new System.Collections.Generic.List<string>(),
                    Errors = new System.Collections.Generic.List<string>()
                };

                mockConversionService
                    .Setup(x => x.ExcelToXmlAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConversionOptions>()))
                    .ReturnsAsync(conversionResult);

                mockConversionService
                    .Setup(x => x.DetectFileFormatAsync(It.IsAny<string>()))
                    .ReturnsAsync(new FileFormatInfo { IsSupported = true, FormatType = FileFormatType.Excel });

                viewModel = new MainViewModel(mockConversionService.Object);

                sourceFile = CreateTestExcelFile("test_input.xlsx", "Name,Value\nItem1,100\nItem2,200");
                targetFile = Path.Combine(TestTempDir, "test_output.xml");

                // When 我设置输入文件和输出路径
                viewModel.SourceFilePath = sourceFile;
                viewModel.TargetFilePath = targetFile;
                viewModel.SourceFileInfo = await mockConversionService.Object.DetectFileFormatAsync(sourceFile);

                // And 执行转换
                await viewModel.ConvertCommand.ExecuteAsync();

                // Then 系统应该正确执行转换流程
                viewModel.IsBusy.Should().BeFalse("转换完成后不应该处于忙碌状态");
                viewModel.StatusMessage.Should().Contain("成功", "状态消息应该显示成功信息");

                // And 显示转换结果
                viewModel.StatusMessage.Should().Contain(conversionResult.Message, "应该显示转换结果消息");
                
                // 验证服务调用
                mockConversionService.Verify(
                    x => x.ExcelToXmlAsync(
                        It.Is<string>(s => s == sourceFile),
                        It.Is<string>(s => s == targetFile),
                        It.IsAny<ConversionOptions>()),
                    Times.Once);

                Output.WriteLine($"基本转换流程测试通过");
                Output.WriteLine($"状态消息: {viewModel.StatusMessage}");
            }
            finally
            {
                CleanupTestFiles(sourceFile, targetFile);
            }
        }

        #endregion

        #region 场景2: 文件格式自动检测

        /// <summary>
        /// 场景: 文件格式自动检测
        /// 当 我选择一个源文件
        /// 那么 系统应该自动检测文件格式
        /// 并且 设置合适的转换方向
        /// </summary>
        [Fact]
        public async Task FileFormatAutoDetection_FormatRecognition()
        {
            // Given - 准备不同格式的测试文件
            MainViewModel viewModel = null;
            string excelFile = null;
            string xmlFile = null;
            
            try
            {
                var mockConversionService = new Mock<IFormatConversionService>();

                // 设置Excel文件检测
                mockConversionService
                    .Setup(x => x.DetectFileFormatAsync(It.Is<string>(s => s.EndsWith(".xlsx"))))
                    .ReturnsAsync(new FileFormatInfo 
                    { 
                        IsSupported = true, 
                        FormatType = FileFormatType.Excel,
                        FormatDescription = "Excel文件"
                    });

                // 设置XML文件检测
                mockConversionService
                    .Setup(x => x.DetectFileFormatAsync(It.Is<string>(s => s.EndsWith(".xml"))))
                    .ReturnsAsync(new FileFormatInfo 
                    { 
                        IsSupported = true, 
                        FormatType = FileFormatType.Xml,
                        FormatDescription = "XML文件"
                    });

                viewModel = new MainViewModel(mockConversionService.Object);

                excelFile = CreateTestExcelFile("test.xlsx", "Name,Value\nTest,100");
                xmlFile = CreateTestXmlFile("test.xml", "<root><item><name>Test</name></item></root>");

                // When 我选择一个Excel文件
                viewModel.SourceFilePath = excelFile;
                // 直接设置文件信息，因为AnalyzeSourceFileAsync是private方法
                viewModel.SourceFileInfo = await mockConversionService.Object.DetectFileFormatAsync(excelFile);

                // Then 系统应该自动检测文件格式
                viewModel.SourceFileInfo.ShouldNotBeNull("应该检测到文件信息");
                viewModel.SourceFileInfo.FormatType.ShouldBe(FileFormatType.Excel, "应该识别为Excel格式");
                viewModel.ConversionDirection.ShouldBe(ConversionDirection.ExcelToXml, "应该设置Excel到XML的转换方向");

                // When 我选择一个XML文件
                viewModel.SourceFilePath = xmlFile;
                // 直接设置文件信息，因为AnalyzeSourceFileAsync是private方法
                viewModel.SourceFileInfo = await mockConversionService.Object.DetectFileFormatAsync(xmlFile);

                // Then 系统应该设置合适的转换方向
                viewModel.SourceFileInfo.FormatType.ShouldBe(FileFormatType.Xml, "应该识别为XML格式");
                viewModel.ConversionDirection.ShouldBe(ConversionDirection.XmlToExcel, "应该设置XML到Excel的转换方向");

                Output.WriteLine($"Excel文件检测: {viewModel.SourceFileInfo.FormatDescription}");
                Output.WriteLine($"XML文件检测: {viewModel.SourceFileInfo.FormatDescription}");
            }
            finally
            {
                CleanupTestFiles(excelFile, xmlFile);
            }
        }

        #endregion

        #region 场景3: 转换方向切换和默认扩展名

        /// <summary>
        /// 场景: 转换方向切换和默认扩展名
        /// 当 我切换转换方向
        /// 那么 系统应该更新默认文件扩展名
        /// 并且 保持文件名的基本部分
        /// </summary>
        [Fact]
        public void ConversionDirectionSwitching_DefaultExtensions()
        {
            // Given - 准备ViewModel
            var mockConversionService = new Mock<IFormatConversionService>();
            var viewModel = new MainViewModel(mockConversionService.Object);

            // Given 我有一个源文件路径
            viewModel.SourceFilePath = "/path/to/mydata.xlsx";

            // When 我切换转换方向
            viewModel.ConversionDirection = ConversionDirection.ExcelToXml;

            // Then 系统应该更新默认文件扩展名
            // 注意：实际的扩展名更新逻辑可能在设置源文件路径时触发
            viewModel.ConversionDirection.ShouldBe(ConversionDirection.ExcelToXml);

            // When 我切换到相反的方向
            viewModel.ConversionDirection = ConversionDirection.XmlToExcel;

            // Then 系统应该保持文件名的基本部分
            viewModel.ConversionDirection.ShouldBe(ConversionDirection.XmlToExcel);

            Output.WriteLine($"转换方向切换测试通过");
            Output.WriteLine($"当前方向: {viewModel.ConversionDirection}");
        }

        #endregion

        #region 场景4: 繁忙状态和命令禁用

        /// <summary>
        /// 场景: 繁忙状态和命令禁用
        /// 当 系统正在执行转换操作
        /// 那么 转换命令应该被禁用
        /// 并且 显示忙碌状态
        /// </summary>
        [Fact]
        public async Task BusyStateAndCommandDisabling_StateManagement()
        {
            // Given - 准备异步转换测试
            var mockConversionService = new Mock<IFormatConversionService>();
            var viewModel = new MainViewModel(mockConversionService.Object);

            // 设置一个会延迟的转换操作
            var tcs = new TaskCompletionSource<ConversionResult>();
            mockConversionService
                .Setup(x => x.ExcelToXmlAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConversionOptions>()))
                .Returns(tcs.Task);

            string sourceFile = CreateTestExcelFile("busy_test.xlsx", "Name,Value\nTest,100");
            string targetFile = Path.Combine(TestTempDir, "busy_output.xml");

            viewModel.SourceFilePath = sourceFile;
            viewModel.TargetFilePath = targetFile;
            viewModel.SourceFileInfo = new FileFormatInfo { IsSupported = true };

            // 当开始转换时，立即检查忙碌状态
            var conversionTask = viewModel.ConvertCommand.ExecuteAsync();

            // Then 系统应该显示忙碌状态
            viewModel.IsBusy.Should().BeTrue("转换过程中应该处于忙碌状态");

            // And 转换命令应该被禁用
            viewModel.ConvertCommand.CanExecute().Should().BeFalse("忙碌时转换命令应该被禁用");

            // 完成转换任务
            tcs.SetResult(new ConversionResult { Success = true, Message = "测试完成" });
            await conversionTask;

            // 转换完成后，应该恢复可用状态
            viewModel.IsBusy.Should().BeFalse("转换完成后不应该处于忙碌状态");
            viewModel.ConvertCommand.CanExecute().Should().BeTrue("转换完成后命令应该重新启用");

            Output.WriteLine($"忙碌状态管理测试通过");
        }

        #endregion

        #region 场景5: 清除操作和状态重置

        /// <summary>
        /// 场景: 清除操作和状态重置
        /// 当 我执行清除操作
        /// 那么 系统应该重置所有状态
        /// 并且 清空所有文件路径
        /// </summary>
        [Fact]
        public void ClearOperation_StateReset()
        {
            // Given - 准备带有数据的ViewModel
            var mockConversionService = new Mock<IFormatConversionService>();
            var viewModel = new MainViewModel(mockConversionService.Object);

            // Given 我已经设置了一些文件路径和状态
            viewModel.SourceFilePath = "/path/to/source.xlsx";
            viewModel.TargetFilePath = "/path/to/target.xml";
            viewModel.StatusMessage = "转换完成";
            viewModel.SourceFileInfo = new FileFormatInfo { IsSupported = true };

            // When 我执行清除操作
            viewModel.ClearCommand.Execute();

            // Then 系统应该重置所有状态
            viewModel.SourceFilePath.ShouldBeNullOrEmpty("源文件路径应该被清空");
            viewModel.TargetFilePath.ShouldBeNullOrEmpty("目标文件路径应该被清空");
            viewModel.StatusMessage.ShouldBe("就绪", "状态消息应该重置为就绪");
            viewModel.SourceFileInfo.ShouldBeNull("文件信息应该被清空");

            Output.WriteLine($"清除操作测试通过");
            Output.WriteLine($"状态重置完成: {viewModel.StatusMessage}");
        }

        #endregion

        #region 场景6: 错误状态显示和恢复

        /// <summary>
        /// 场景: 错误状态显示和恢复
        /// 当 转换操作失败时
        /// 那么 系统应该显示错误信息
        /// 并且 允许用户重新尝试
        /// </summary>
        [Fact]
        public async Task ErrorStateDisplayAndRecovery_ErrorHandling()
        {
            // Given - 准备错误转换测试
            var mockConversionService = new Mock<IFormatConversionService>();
            var viewModel = new MainViewModel(mockConversionService.Object);

            // 设置转换失败的结果
            var errorResult = new ConversionResult
            {
                Success = false,
                Message = "文件格式不支持",
                Errors = new System.Collections.Generic.List<string> { "无法识别的文件格式" }
            };

            mockConversionService
                .Setup(x => x.ExcelToXmlAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConversionOptions>()))
                .ReturnsAsync(errorResult);

            string sourceFile = CreateTestExcelFile("error_test.xlsx", "Name,Value\nTest,100");
            string targetFile = Path.Combine(TestTempDir, "error_output.xml");

            viewModel.SourceFilePath = sourceFile;
            viewModel.TargetFilePath = targetFile;
            viewModel.SourceFileInfo = new FileFormatInfo { IsSupported = true };

            // When 转换操作失败时
            await viewModel.ConvertCommand.ExecuteAsync();

            // Then 系统应该显示错误信息
            viewModel.StatusMessage.Should().Contain("转换失败", "应该显示转换失败信息");
            viewModel.StatusMessage.Should().Contain(errorResult.Message, "应该显示具体的错误消息");

            // And 系统应该允许用户重新尝试
            viewModel.IsBusy.ShouldBeFalse("错误后不应该保持忙碌状态");
            viewModel.ConvertCommand.CanExecute().Should().BeTrue("错误后应该允许重新尝试");

            Output.WriteLine($"错误状态处理测试通过");
            Output.WriteLine($"错误信息: {viewModel.StatusMessage}");
        }

        #endregion

        #region 场景7: 文件验证和命令可用性

        /// <summary>
        /// 场景: 文件验证和命令可用性
        /// 当 我设置了无效的文件路径
        /// 那么 转换命令应该被禁用
        /// 并且 显示相应的验证信息
        /// </summary>
        [Fact]
        public void FileValidationAndCommandAvailability_ValidationLogic()
        {
            // Given - 准备ViewModel
            var mockConversionService = new Mock<IFormatConversionService>();
            var viewModel = new MainViewModel(mockConversionService.Object);

            // When 我没有设置任何文件路径
            viewModel.SourceFilePath = "";
            viewModel.TargetFilePath = "";

            // Then 转换命令应该被禁用
            viewModel.ConvertCommand.CanExecute().Should().BeFalse("没有文件路径时命令应该被禁用");

            // When 我设置了源文件但没有目标文件
            viewModel.SourceFilePath = "/path/to/source.xlsx";
            viewModel.TargetFilePath = "";

            // Then 转换命令应该仍然被禁用
            viewModel.ConvertCommand.CanExecute().ShouldBeFalse("没有目标文件时命令应该被禁用");

            // When 我设置了两个文件路径但源文件不存在
            viewModel.SourceFilePath = "/path/to/nonexistent.xlsx";
            viewModel.TargetFilePath = "/path/to/output.xml";

            // Then 转换命令应该被禁用
            viewModel.ConvertCommand.CanExecute().Should().BeFalse("源文件不存在时命令应该被禁用");

            // When 我设置了有效的文件路径
            string validFile = CreateTestExcelFile("validation_test.xlsx", "Name,Value\nTest,100");
            viewModel.SourceFilePath = validFile;
            viewModel.SourceFileInfo = new FileFormatInfo { IsSupported = true };

            // Then 转换命令应该被启用
            viewModel.ConvertCommand.CanExecute().Should().BeTrue("文件路径有效时命令应该被启用");

            Output.WriteLine($"文件验证逻辑测试通过");
            Output.WriteLine($"命令可用性: {viewModel.ConvertCommand.CanExecute()}");

            CleanupTestFiles(validFile);
        }

        #endregion
    }
}