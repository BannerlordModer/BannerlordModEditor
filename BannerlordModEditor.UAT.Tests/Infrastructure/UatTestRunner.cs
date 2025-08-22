using Xunit;
using Xunit.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using BannerlordModEditor.UAT.Tests.Common;

namespace BannerlordModEditor.UAT.Tests.Infrastructure
{
    /// <summary>
    /// UAT测试运行器和报告生成器
    /// 提供测试执行统计和详细报告
    /// </summary>
    public class UatTestRunner
    {
        private readonly ITestOutputHelper _output;
        private readonly List<TestExecutionResult> _executionResults;

        public UatTestRunner(ITestOutputHelper output)
        {
            _output = output;
            _executionResults = new List<TestExecutionResult>();
        }

        /// <summary>
        /// 记录测试执行结果
        /// </summary>
        public void RecordTestResult(string testName, bool passed, string message, TimeSpan? duration = null)
        {
            var result = new TestExecutionResult
            {
                TestName = testName,
                Passed = passed,
                Message = message,
                Duration = duration,
                Timestamp = DateTime.UtcNow
            };

            _executionResults.Add(result);
        }

        /// <summary>
        /// 生成测试执行报告
        /// </summary>
        public void GenerateReport()
        {
            var totalTests = _executionResults.Count;
            var passedTests = _executionResults.Count(r => r.Passed);
            var failedTests = totalTests - passedTests;
            var passRate = totalTests > 0 ? (double)passedTests / totalTests * 100 : 0;

            var totalDuration = _executionResults
                .Where(r => r.Duration.HasValue)
                .Sum(r => r.Duration.Value.TotalMilliseconds);

            _output.WriteLine("==========================================");
            _output.WriteLine("         UAT 测试执行报告");
            _output.WriteLine("==========================================");
            _output.WriteLine($"总测试数: {totalTests}");
            _output.WriteLine($"通过测试: {passedTests}");
            _output.WriteLine($"失败测试: {failedTests}");
            _output.WriteLine($"通过率: {passRate:F1}%");
            _output.WriteLine($"总执行时间: {totalDuration:F0}ms");
            _output.WriteLine($"平均执行时间: {totalDuration / Math.Max(totalTests, 1):F2}ms");
            _output.WriteLine("==========================================");

            // 按特性分组显示结果
            var featureGroups = _executionResults
                .GroupBy(r => ExtractFeatureName(r.TestName))
                .OrderBy(g => g.Key);

            foreach (var group in featureGroups)
            {
                var featurePassed = group.Count(r => r.Passed);
                var featureTotal = group.Count();
                var featureRate = (double)featurePassed / featureTotal * 100;

                _output.WriteLine($"\n【{group.Key}】");
                _output.WriteLine($"  测试数: {featureTotal}, 通过: {featurePassed}, 通过率: {featureRate:F1}%");

                foreach (var result in group)
                {
                    var status = result.Passed ? "✅ 通过" : "❌ 失败";
                    var duration = result.Duration.HasValue ? $"{result.Duration.Value.TotalMilliseconds:F0}ms" : "N/A";
                    _output.WriteLine($"    {status} {result.TestName} ({duration})");
                    
                    if (!result.Passed)
                    {
                        _output.WriteLine($"       错误: {result.Message}");
                    }
                }
            }

            // 失败测试汇总
            var failedResults = _executionResults.Where(r => !r.Passed).ToList();
            if (failedResults.Any())
            {
                _output.WriteLine("\n❌ 失败测试汇总:");
                foreach (var result in failedResults)
                {
                    _output.WriteLine($"  - {result.TestName}: {result.Message}");
                }
            }

            _output.WriteLine("\n==========================================");
            _output.WriteLine("测试完成时间: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC"));
            _output.WriteLine("==========================================");

            // 生成JSON报告
            GenerateJsonReport();
        }

        /// <summary>
        /// 生成JSON格式的详细报告
        /// </summary>
        private void GenerateJsonReport()
        {
            var report = new
            {
                TestRunId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow,
                Summary = new
                {
                    TotalTests = _executionResults.Count,
                    PassedTests = _executionResults.Count(r => r.Passed),
                    FailedTests = _executionResults.Count(r => !r.Passed),
                    PassRate = _executionResults.Count > 0 ? 
                        (double)_executionResults.Count(r => r.Passed) / _executionResults.Count * 100 : 0,
                    TotalDurationMs = _executionResults
                        .Where(r => r.Duration.HasValue)
                        .Sum(r => r.Duration.Value.TotalMilliseconds)
                },
                Features = _executionResults
                    .GroupBy(r => ExtractFeatureName(r.TestName))
                    .Select(g => new
                    {
                        Name = g.Key,
                        TotalTests = g.Count(),
                        PassedTests = g.Count(r => r.Passed),
                        FailedTests = g.Count(r => !r.Passed),
                        PassRate = (double)g.Count(r => r.Passed) / g.Count() * 100,
                        Tests = g.Select(t => new
                        {
                            t.TestName,
                            t.Passed,
                            t.Message,
                            DurationMs = t.Duration?.TotalMilliseconds
                        }).ToList()
                    }).ToList(),
                FailedTests = _executionResults
                    .Where(r => !r.Passed)
                    .Select(t => new
                    {
                        t.TestName,
                        t.Message,
                        DurationMs = t.Duration?.TotalMilliseconds
                    }).ToList()
            };

            var jsonReport = JsonSerializer.Serialize(report, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var reportPath = Path.Combine(Path.GetTempPath(), $"uat_report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json");
            File.WriteAllText(reportPath, jsonReport);

            _output.WriteLine($"\n详细JSON报告已保存到: {reportPath}");
        }

        /// <summary>
        /// 从测试名称中提取特性名称
        /// </summary>
        private string ExtractFeatureName(string testName)
        {
            // 测试名称格式通常是 "FeatureName_ScenarioName"
            var underscoreIndex = testName.IndexOf('_');
            if (underscoreIndex > 0)
            {
                return testName.Substring(0, underscoreIndex);
            }
            return "Unknown";
        }
    }

    /// <summary>
    /// 测试执行结果记录
    /// </summary>
    public class TestExecutionResult
    {
        public string TestName { get; set; }
        public bool Passed { get; set; }
        public string Message { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// UAT测试基类，集成测试运行器
    /// </summary>
    public abstract class UatTestBase : BddTestBase
    {
        protected readonly UatTestRunner TestRunner;

        protected UatTestBase(ITestOutputHelper output) : base(output)
        {
            TestRunner = new UatTestRunner(output);
        }

        /// <summary>
        /// 执行UAT测试并自动记录结果
        /// </summary>
        protected async Task ExecuteUatTest(string testName, Func<Task> testAction)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            string errorMessage = null;
            bool passed = false;

            try
            {
                await testAction();
                passed = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                passed = false;
            }
            finally
            {
                stopwatch.Stop();
                TestRunner.RecordTestResult(testName, passed, errorMessage, stopwatch.Elapsed);
            }
        }

        /// <summary>
        /// 在测试完成后生成报告
        /// </summary>
        protected void GenerateUatReport()
        {
            TestRunner.GenerateReport();
        }
    }
}