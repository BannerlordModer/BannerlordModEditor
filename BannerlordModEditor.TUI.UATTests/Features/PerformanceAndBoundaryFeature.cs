using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using Shouldly;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using BannerlordModEditor.TUI.Services;
using BannerlordModEditor.TUI.ViewModels;
using BannerlordModEditor.Common.Models;
using BannerlordModEditor.TUI.UATTests.Common;

namespace BannerlordModEditor.TUI.UATTests.Features
{
    /// <summary>
    /// BDD特性：性能和边界条件测试
    /// 
    /// 作为一个Bannerlord Mod开发者
    /// 我希望系统在处理大量数据时保持良好性能
    /// 并且 能够处理各种边界条件
    /// </summary>
    public class PerformanceAndBoundaryFeature : BddTestBase
    {
        public PerformanceAndBoundaryFeature(ITestOutputHelper output) : base(output)
        {
        }

        #region 场景1: 大文件性能基准测试

        /// <summary>
        /// 场景: 大文件性能基准测试
        /// 当 我处理包含大量记录的文件
        /// 那么 系统应该在可接受的时间内完成处理
        /// 并且 内存使用应该保持在合理范围内
        /// </summary>
        [Fact]
        public async Task LargeFilePerformance_Benchmark()
        {
            // Given - 准备大型测试文件
            string largeExcelFile = null;
            string outputXmlFile = null;
            
            try
            {
                // Given 我处理包含大量记录的文件
                var recordCount = 5000; // 5000条记录
                largeExcelFile = CreateTestExcelFile("large_performance.xlsx", 
                    GenerateLargeDataSet(recordCount));
                outputXmlFile = Path.Combine(TestTempDir, "large_output.xml");

                var stopwatch = new Stopwatch();
                var memoryBefore = GC.GetTotalMemory(true);

                // When 我执行转换操作
                stopwatch.Start();
                var result = await ConversionService.ExcelToXmlAsync(largeExcelFile, outputXmlFile);
                stopwatch.Stop();

                var memoryAfter = GC.GetTotalMemory(true);
                var memoryUsed = memoryAfter - memoryBefore;

                // Then 系统应该在可接受的时间内完成处理
                result.Success.Should().BeTrue("大文件转换应该成功");
                result.RecordsProcessed.ShouldBe(recordCount, $"应该处理{recordCount}条记录");

                // 性能断言 - 根据实际情况调整阈值
                var executionTime = stopwatch.ElapsedMilliseconds;
                var timePerRecord = executionTime / (double)recordCount;

                executionTime.Should().BeLessThan(30000, $"5000条记录应该在30秒内完成，实际用时: {executionTime}ms");
                timePerRecord.Should().BeLessThan(10, $"每条记录处理时间应该小于10ms，实际: {timePerRecord:F2}ms");

                // And 内存使用应该保持在合理范围内
                var memoryPerRecord = memoryUsed / (double)recordCount;
                memoryUsed.Should().BeLessThan(100 * 1024 * 1024, "内存使用应该小于100MB"); // 100MB阈值
                
                Output.WriteLine($"=== 大文件性能测试结果 ===");
                Output.WriteLine($"记录数量: {recordCount}");
                Output.WriteLine($"总执行时间: {executionTime}ms");
                Output.WriteLine($"平均每条记录: {timePerRecord:F2}ms");
                Output.WriteLine($"内存使用: {memoryUsed / 1024 / 1024:F2}MB");
                Output.WriteLine($"平均每条记录内存: {memoryPerRecord:F2}bytes");
                Output.WriteLine($"转换结果: {result.Message}");
            }
            finally
            {
                CleanupTestFiles(largeExcelFile, outputXmlFile);
            }
        }

        #endregion

        #region 场景2: 并发转换测试

        /// <summary>
        /// 场景: 并发转换测试
        /// 当 我同时启动多个转换任务
        /// 那么 系统应该能够正确处理并发请求
        /// 并且 各个任务之间不应该相互干扰
        /// </summary>
        [Fact]
        public async Task ConcurrentConversion_ThreadSafety()
        {
            // Given - 准备多个并发转换任务
            var tasks = new List<Task<ConversionResult>>();
            var files = new List<string>();

            try
            {
                // Given 我同时启动多个转换任务
                var taskCount = 5;
                for (int i = 0; i < taskCount; i++)
                {
                    var sourceFile = CreateTestExcelFile($"concurrent_{i}.xlsx", 
                        $"Name,Value,Description\nItem{i},{i * 10},描述{i}");
                    var targetFile = Path.Combine(TestTempDir, $"concurrent_output_{i}.xml");
                    files.Add(sourceFile);
                    files.Add(targetFile);

                    tasks.Add(ConversionService.ExcelToXmlAsync(sourceFile, targetFile));
                }

                // When 我同时执行所有任务
                var stopwatch = Stopwatch.StartNew();
                var results = await Task.WhenAll(tasks);
                stopwatch.Stop();

                // Then 系统应该能够正确处理并发请求
                results.Length.ShouldBe(taskCount, $"应该完成{taskCount}个任务");

                for (int i = 0; i < results.Length; i++)
                {
                    results[i].Success.Should().BeTrue($"任务{i}应该成功");
                    results[i].Errors.ShouldBeEmpty($"任务{i}不应该有错误");
                }

                // And 各个任务之间不应该相互干扰
                for (int i = 0; i < taskCount; i++)
                {
                    var outputFile = Path.Combine(TestTempDir, $"concurrent_output_{i}.xml");
                    VerifyFileExistsAndNotEmpty(outputFile);
                    
                    var content = await File.ReadAllTextAsync(outputFile);
                    content.Should().Contain($"Item{i}", $"输出文件{i}应该包含正确的数据");
                }

                var totalTime = stopwatch.ElapsedMilliseconds;
                Output.WriteLine($"=== 并发转换测试结果 ===");
                Output.WriteLine($"并发任务数: {taskCount}");
                Output.WriteLine($"总执行时间: {totalTime}ms");
                Output.WriteLine($"平均每个任务: {totalTime / (double)taskCount:F2}ms");
            }
            finally
            {
                CleanupTestFiles(files.ToArray());
            }
        }

        #endregion

        #region 场景3: 内存使用监控

        /// <summary>
        /// 场景: 内存使用监控
        /// 当 我处理极大量的数据
        /// 那么 系统应该及时释放内存
        /// 并且 避免内存泄漏
        /// </summary>
        [Fact]
        public async Task MemoryUsageMonitoring_MemoryManagement()
        {
            // Given - 准备内存监控测试
            string hugeFile = null;
            string outputFile = null;
            
            try
            {
                // Given 我处理极大量的数据
                var hugeRecordCount = 10000; // 10000条记录
                hugeFile = CreateTestExcelFile("huge_dataset.xlsx", 
                    GenerateLargeDataSet(hugeRecordCount));
                outputFile = Path.Combine(TestTempDir, "huge_output.xml");

                // 强制垃圾回收以获得准确的基线内存使用
                GC.Collect();
                GC.WaitForPendingFinalizers();
                var baselineMemory = GC.GetTotalMemory(true);

                // When 我执行转换操作
                var result = await ConversionService.ExcelToXmlAsync(hugeFile, outputFile);

                // 再次强制垃圾回收
                GC.Collect();
                GC.WaitForPendingFinalizers();
                var afterConversionMemory = GC.GetTotalMemory(true);

                // Then 系统应该及时释放内存
                result.Success.Should().BeTrue("大文件转换应该成功");
                result.RecordsProcessed.ShouldBe(hugeRecordCount, $"应该处理{hugeRecordCount}条记录");

                // 验证文件确实被创建
                VerifyFileExistsAndNotEmpty(outputFile);

                // 内存增长应该在合理范围内
                var memoryGrowth = afterConversionMemory - baselineMemory;
                var memoryPerRecord = memoryGrowth / (double)hugeRecordCount;

                memoryGrowth.Should().BeLessThan(50 * 1024 * 1024, "内存增长应该小于50MB");
                memoryPerRecord.Should().BeLessThan(1024, "每条记录内存增长应该小于1KB");

                Output.WriteLine($"=== 内存使用监控结果 ===");
                Output.WriteLine($"记录数量: {hugeRecordCount}");
                Output.WriteLine($"基线内存: {baselineMemory / 1024 / 1024:F2}MB");
                Output.WriteLine($"转换后内存: {afterConversionMemory / 1024 / 1024:F2}MB");
                Output.WriteLine($"内存增长: {memoryGrowth / 1024 / 1024:F2}MB");
                Output.WriteLine($"每条记录内存: {memoryPerRecord:F2}bytes");
            }
            finally
            {
                CleanupTestFiles(hugeFile, outputFile);
            }
        }

        #endregion

        #region 场景4: 边界条件测试

        /// <summary>
        /// 场景: 边界条件测试
        /// 当 我使用各种边界条件输入
        /// 那么 系统应该正确处理极端情况
        /// </summary>
        [Fact]
        public async Task BoundaryConditions_EdgeCases()
        {
            // Given - 准备各种边界条件测试
            var boundaryTests = new List<Func<Task<string>>>
            {
                // 空文件测试
                async () => {
                    var file = CreateTestExcelFile("empty.xlsx", "");
                    var result = await ConversionService.ExcelToXmlAsync(file, Path.Combine(TestTempDir, "empty_output.xml"));
                    return $"空文件: {(result.Success ? "成功" : "失败")} - {result.Message}";
                },
                
                // 单行数据测试
                async () => {
                    var file = CreateTestExcelFile("single_row.xlsx", "Name,Value\nSingle,1");
                    var result = await ConversionService.ExcelToXmlAsync(file, Path.Combine(TestTempDir, "single_output.xml"));
                    return $"单行数据: {(result.Success ? "成功" : "失败")} - 记录数: {result.RecordsProcessed}";
                },
                
                // 超长字段测试
                async () => {
                    var longText = new string('A', 10000); // 10000个字符
                    var file = CreateTestExcelFile("long_field.xlsx", $"Name,Description\nLongField,{longText}");
                    var result = await ConversionService.ExcelToXmlAsync(file, Path.Combine(TestTempDir, "long_output.xml"));
                    return $"超长字段: {(result.Success ? "成功" : "失败")} - 字段长度: {longText.Length}";
                },
                
                // 特殊字符测试
                async () => {
                    var specialChars = "!@#$%^&*()_+-=[]{}|;':\",./<>?`~";
                    var file = CreateTestExcelFile("special_chars.xlsx", $"Name,Value\nSpecial,{specialChars}");
                    var result = await ConversionService.ExcelToXmlAsync(file, Path.Combine(TestTempDir, "special_output.xml"));
                    return $"特殊字符: {(result.Success ? "成功" : "失败")} - {result.Message}";
                },
                
                // Unicode字符测试
                async () => {
                    var unicodeText = "测试中文🚀emoji和ñáéíóú";
                    var file = CreateTestExcelFile("unicode.xlsx", $"Name,Value\nUnicode,{unicodeText}");
                    var result = await ConversionService.ExcelToXmlAsync(file, Path.Combine(TestTempDir, "unicode_output.xml"));
                    return $"Unicode字符: {(result.Success ? "成功" : "失败")} - {result.Message}";
                }
            };

            // When 我执行各种边界条件测试
            var results = new List<string>();
            foreach (var test in boundaryTests)
            {
                try
                {
                    var result = await test();
                    results.Add(result);
                }
                catch (Exception ex)
                {
                    results.Add($"异常: {ex.Message}");
                }
            }

            // Then 系统应该正确处理极端情况
            var successCount = results.Count(r => r.Contains("成功"));
            var totalCount = results.Count;

            Output.WriteLine($"=== 边界条件测试结果 ===");
            foreach (var result in results)
            {
                Output.WriteLine(result);
            }

            Output.WriteLine($"成功测试: {successCount}/{totalCount}");
            
            // 至少应该有一些测试成功
            successCount.ShouldBeGreaterThan(0, "至少应该有一些边界条件测试成功");
        }

        #endregion

        #region 场景5: 文件大小限制测试

        /// <summary>
        /// 场景: 文件大小限制测试
        /// 当 我处理接近大小限制的文件
        /// 那么 系统应该能够处理大文件
        /// 或者 提供明确的错误信息
        /// </summary>
        [Fact]
        public async Task FileSizeLimits_LargeFiles()
        {
            // Given - 准备不同大小的文件进行测试
            var fileSizes = new List<int> { 1024, 10240, 102400, 1048576 }; // 1KB, 10KB, 100KB, 1MB
            var results = new List<string>();

            foreach (var sizeInBytes in fileSizes)
            {
                try
                {
                    // Given 我处理接近大小限制的文件
                    var fileName = $"size_test_{sizeInBytes}.xlsx";
                    var largeContent = GenerateContentBySize(sizeInBytes);
                    var sourceFile = CreateTestExcelFile(fileName, largeContent);
                    var targetFile = Path.Combine(TestTempDir, $"size_output_{sizeInBytes}.xml");

                    var fileInfo = new FileInfo(sourceFile);
                    var actualSize = fileInfo.Length;

                    // When 我执行转换操作
                    var stopwatch = Stopwatch.StartNew();
                    var result = await ConversionService.ExcelToXmlAsync(sourceFile, targetFile);
                    stopwatch.Stop();

                    var testResult = $"大小: {actualSize / 1024:F2}KB - " +
                                     $"状态: {(result.Success ? "成功" : "失败")} - " +
                                     $"时间: {stopwatch.ElapsedMilliseconds}ms - " +
                                     $"记录: {result.RecordsProcessed}";

                    results.Add(testResult);

                    CleanupTestFiles(sourceFile, targetFile);
                }
                catch (Exception ex)
                {
                    results.Add($"大小: {sizeInBytes / 1024:F2}KB - 异常: {ex.Message}");
                }
            }

            // Then 系统应该能够处理大文件
            var successCount = results.Count(r => r.Contains("成功"));
            var totalCount = results.Count;

            Output.WriteLine($"=== 文件大小限制测试结果 ===");
            foreach (var result in results)
            {
                Output.WriteLine(result);
            }

            Output.WriteLine($"成功处理: {successCount}/{totalCount}");
            
            // 应该能够处理至少较小到中等大小的文件
            successCount.ShouldBeGreaterThan(0, "应该能够处理至少一些大小的文件");
        }

        #endregion

        #region 场景6: 重复转换测试

        /// <summary>
        /// 场景: 重复转换测试
        /// 当 我对同一文件进行多次转换
        /// 那么 每次转换都应该产生一致的结果
        /// 并且 不应该有累积的内存问题
        /// </summary>
        [Fact]
        public async Task RepeatedConversion_Consistency()
        {
            // Given - 准备重复转换测试
            string sourceFile = null;
            var outputFiles = new List<string>();
            
            try
            {
                sourceFile = CreateTestExcelFile("repeat_test.xlsx", 
                    "Name,Value,Category\nItem1,100,A\nItem2,200,B\nItem3,300,C");

                var conversionCount = 10;
                var results = new List<ConversionResult>();
                var executionTimes = new List<long>();

                // When 我对同一文件进行多次转换
                for (int i = 0; i < conversionCount; i++)
                {
                    var outputFile = Path.Combine(TestTempDir, $"repeat_output_{i}.xml");
                    outputFiles.Add(outputFile);

                    var stopwatch = Stopwatch.StartNew();
                    var result = await ConversionService.ExcelToXmlAsync(sourceFile, outputFile);
                    stopwatch.Stop();

                    results.Add(result);
                    executionTimes.Add(stopwatch.ElapsedMilliseconds);
                }

                // Then 每次转换都应该产生一致的结果
                results.Count.ShouldBe(conversionCount, $"应该完成{conversionCount}次转换");

                // 验证所有转换都成功
                foreach (var result in results)
                {
                    result.Success.Should().BeTrue("所有转换都应该成功");
                    result.Errors.ShouldBeEmpty("不应该有错误");
                    result.RecordsProcessed.ShouldBe(3, "每次都应该处理3条记录");
                }

                // 验证输出文件的一致性
                for (int i = 1; i < outputFiles.Count; i++)
                {
                    var content1 = await File.ReadAllTextAsync(outputFiles[0]);
                    var content2 = await File.ReadAllTextAsync(outputFiles[i]);
                    content2.ShouldBe(content1, $"第{i}次转换结果应该与第一次一致");
                }

                // 验证性能的一致性
                var avgTime = executionTimes.Average();
                var maxTime = executionTimes.Max();
                var minTime = executionTimes.Min();

                ((long)maxTime).Should().BeLessThan((long)(avgTime * 3), "最慢的转换时间不应该超过平均时间的3倍");

                Output.WriteLine($"=== 重复转换测试结果 ===");
                Output.WriteLine($"转换次数: {conversionCount}");
                Output.WriteLine($"平均执行时间: {avgTime:F2}ms");
                Output.WriteLine($"最快: {minTime}ms, 最慢: {maxTime}ms");
                Output.WriteLine($"时间一致性: {(maxTime - minTime) / avgTime * 100:F1}% 变异");
            }
            finally
            {
                CleanupTestFiles(sourceFile);
                CleanupTestFiles(outputFiles.ToArray());
            }
        }

        #endregion

        #region 辅助方法

        private string GenerateLargeDataSet(int recordCount)
        {
            var content = new System.Text.StringBuilder();
            content.AppendLine("ID,Name,Value,Category,Description,Formula,Notes,Priority,Enabled");

            for (int i = 0; i < recordCount; i++)
            {
                content.AppendLine($"{i:D6},Item_{i},{i * 1.5:F2},Category_{i % 20},这是第{i}个测试项目," +
                    $"base_value * {i * 0.1:F2},备注信息包含一些特殊字符：!@#$%^&*(),{i % 10},{(i % 2 == 0 ? "TRUE" : "FALSE")}");
            }

            return content.ToString();
        }

        private string GenerateContentBySize(int targetSizeInBytes)
        {
            var content = new System.Text.StringBuilder();
            content.AppendLine("ID,Name,Value,Description");

            var currentSize = content.ToString().Length;
            var id = 0;

            while (currentSize < targetSizeInBytes)
            {
                var line = $"{id:D6},Item_{id},{id * 1.5:F2},这是一个用于测试文件大小的描述项\n";
                content.Append(line);
                currentSize += line.Length;
                id++;
            }

            return content.ToString();
        }

        #endregion
    }
}