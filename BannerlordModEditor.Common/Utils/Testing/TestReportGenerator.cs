using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using BannerlordModEditor.Common.Models.DO.Testing;

namespace BannerlordModEditor.Common.Utils.Testing
{
    /// <summary>
    /// 测试报告生成工具类
    /// 提供多种格式的测试报告生成功能
    /// </summary>
    public class TestReportGenerator
    {
        /// <summary>
        /// 生成HTML格式的测试报告
        /// </summary>
        public async Task<string> GenerateHtmlReportAsync(TestSessionDO session, string templatePath = "")
        {
            try
            {
                var html = new System.Text.StringBuilder();
                
                // HTML头部
                html.AppendLine("<!DOCTYPE html>");
                html.AppendLine("<html lang=\"zh-CN\">");
                html.AppendLine("<head>");
                html.AppendLine("    <meta charset=\"UTF-8\">");
                html.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
                html.AppendLine("    <title>测试报告 - " + session.SessionName + "</title>");
                html.AppendLine("    <style>");
                html.AppendLine(await GetDefaultCssAsync());
                html.AppendLine("    </style>");
                html.AppendLine("</head>");
                html.AppendLine("<body>");
                html.AppendLine("    <div class=\"container\">");
                html.AppendLine("        <h1>测试报告</h1>");
                
                // 会话信息
                html.AppendLine("        <div class=\"section\">");
                html.AppendLine("            <h2>会话信息</h2>");
                html.AppendLine("            <table class=\"info-table\">");
                html.AppendLine("                <tr><th>会话ID</th><td>" + session.SessionId + "</td></tr>");
                html.AppendLine("                <tr><th>会话名称</th><td>" + session.SessionName + "</td></tr>");
                html.AppendLine("                <tr><th>项目路径</th><td>" + session.ProjectPath + "</td></tr>");
                html.AppendLine("                <tr><th>解决方案路径</th><td>" + session.SolutionPath + "</td></tr>");
                html.AppendLine("                <tr><th>开始时间</th><td>" + session.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "</td></tr>");
                html.AppendLine("                <tr><th>结束时间</th><td>" + session.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "</td></tr>");
                html.AppendLine("                <tr><th>会话状态</th><td><span class=\"status-" + session.SessionStatus.ToString().ToLower() + "\">" + session.SessionStatus + "</span></td></tr>");
                html.AppendLine("                <tr><th>构建配置</th><td>" + session.BuildConfiguration + "</td></tr>");
                html.AppendLine("                <tr><th>目标框架</th><td>" + session.TargetFramework + "</td></tr>");
                html.AppendLine("            </table>");
                html.AppendLine("        </div>");
                
                // 测试统计
                html.AppendLine("        <div class=\"section\">");
                html.AppendLine("            <h2>测试统计</h2>");
                html.AppendLine("            <div class=\"stats-grid\">");
                html.AppendLine("                <div class=\"stat-card passed\">");
                html.AppendLine("                    <h3>通过测试</h3>");
                html.AppendLine("                    <div class=\"stat-number\">" + session.PassedTests + "</div>");
                html.AppendLine("                </div>");
                html.AppendLine("                <div class=\"stat-card failed\">");
                html.AppendLine("                    <h3>失败测试</h3>");
                html.AppendLine("                    <div class=\"stat-number\">" + session.FailedTests + "</div>");
                html.AppendLine("                </div>");
                html.AppendLine("                <div class=\"stat-card skipped\">");
                html.AppendLine("                    <h3>跳过测试</h3>");
                html.AppendLine("                    <div class=\"stat-number\">" + session.SkippedTests + "</div>");
                html.AppendLine("                </div>");
                html.AppendLine("                <div class=\"stat-card total\">");
                html.AppendLine("                    <h3>总测试数</h3>");
                html.AppendLine("                    <div class=\"stat-number\">" + session.TotalTests + "</div>");
                html.AppendLine("                </div>");
                html.AppendLine("            </div>");
                html.AppendLine("            <div class=\"pass-rate\">");
                html.AppendLine("                <div class=\"pass-rate-bar\" style=\"width: " + session.PassRate + "%\"></div>");
                html.AppendLine("                <span>通过率: " + session.PassRate.ToString("F2") + "%</span>");
                html.AppendLine("            </div>");
                html.AppendLine("        </div>");
                
                // 覆盖率信息
                if (session.CoverageMetrics != null)
                {
                    html.AppendLine("        <div class=\"section\">");
                    html.AppendLine("            <h2>覆盖率指标</h2>");
                    html.AppendLine("            <div class=\"coverage-grid\">");
                    html.AppendLine("                <div class=\"coverage-item\">");
                    html.AppendLine("                    <div class=\"coverage-label\">行覆盖率</div>");
                    html.AppendLine("                    <div class=\"coverage-bar\" style=\"width: " + session.CoverageMetrics.LineCoverage + "%\"></div>");
                    html.AppendLine("                    <div class=\"coverage-value\">" + session.CoverageMetrics.LineCoverage.ToString("F2") + "%</div>");
                    html.AppendLine("                </div>");
                    html.AppendLine("                <div class=\"coverage-item\">");
                    html.AppendLine("                    <div class=\"coverage-label\">分支覆盖率</div>");
                    html.AppendLine("                    <div class=\"coverage-bar\" style=\"width: " + session.CoverageMetrics.BranchCoverage + "%\"></div>");
                    html.AppendLine("                    <div class=\"coverage-value\">" + session.CoverageMetrics.BranchCoverage.ToString("F2") + "%</div>");
                    html.AppendLine("                </div>");
                    html.AppendLine("                <div class=\"coverage-item\">");
                    html.AppendLine("                    <div class=\"coverage-label\">方法覆盖率</div>");
                    html.AppendLine("                    <div class=\"coverage-bar\" style=\"width: " + session.CoverageMetrics.MethodCoverage + "%\"></div>");
                    html.AppendLine("                    <div class=\"coverage-value\">" + session.CoverageMetrics.MethodCoverage.ToString("F2") + "%</div>");
                    html.AppendLine("                </div>");
                    html.AppendLine("                <div class=\"coverage-item\">");
                    html.AppendLine("                    <div class=\"coverage-label\">类覆盖率</div>");
                    html.AppendLine("                    <div class=\"coverage-bar\" style=\"width: " + session.CoverageMetrics.ClassCoverage + "%\"></div>");
                    html.AppendLine("                    <div class=\"coverage-value\">" + session.CoverageMetrics.ClassCoverage.ToString("F2") + "%</div>");
                    html.AppendLine("                </div>");
                    html.AppendLine("            </div>");
                    html.AppendLine("        </div>");
                }
                
                // 质量门禁
                if (session.QualityGates.Count > 0)
                {
                    html.AppendLine("        <div class=\"section\">");
                    html.AppendLine("            <h2>质量门禁</h2>");
                    html.AppendLine("            <table class=\"gates-table\">");
                    html.AppendLine("                <thead>");
                    html.AppendLine("                    <tr><th>门禁名称</th><th>类型</th><th>状态</th><th>当前值</th><th>阈值</th><th>消息</th></tr>");
                    html.AppendLine("                </thead>");
                    html.AppendLine("                <tbody>");
                    
                    foreach (var gate in session.QualityGates)
                    {
                        html.AppendLine("                    <tr>");
                        html.AppendLine("                        <td>" + gate.GateName + "</td>");
                        html.AppendLine("                        <td>" + gate.GateType + "</td>");
                        html.AppendLine("                        <td><span class=\"status-" + gate.Status.ToString().ToLower() + "\">" + gate.Status + "</span></td>");
                        html.AppendLine("                        <td>" + gate.CurrentValue.ToString("F2") + "</td>");
                        html.AppendLine("                        <td>" + gate.Threshold.ToString("F2") + "</td>");
                        html.AppendLine("                        <td>" + gate.Message + "</td>");
                        html.AppendLine("                    </tr>");
                    }
                    
                    html.AppendLine("                </tbody>");
                    html.AppendLine("            </table>");
                    html.AppendLine("        </div>");
                }
                
                // 测试结果详情
                html.AppendLine("        <div class=\"section\">");
                html.AppendLine("            <h2>测试结果详情</h2>");
                html.AppendLine("            <div class=\"test-results\">");
                
                var groupedResults = session.TestResults.GroupBy(r => r.Status);
                foreach (var group in groupedResults)
                {
                    html.AppendLine("                <div class=\"result-group " + group.Key.ToString().ToLower() + "\">");
                    html.AppendLine("                    <h3>" + group.Key + " (" + group.Count() + ")</h3>");
                    html.AppendLine("                    <table class=\"results-table\">");
                    html.AppendLine("                        <thead>");
                    html.AppendLine("                            <tr><th>测试名称</th><th>类别</th><th>执行时间</th><th>错误信息</th></tr>");
                    html.AppendLine("                        </thead>");
                    html.AppendLine("                        <tbody>");
                    
                    foreach (var result in group)
                    {
                        html.AppendLine("                            <tr>");
                        html.AppendLine("                                <td>" + result.Name + "</td>");
                        html.AppendLine("                                <td>" + result.Category + "</td>");
                        html.AppendLine("                                <td>" + result.DurationMs + " ms</td>");
                        html.AppendLine("                                <td>" + (result.ErrorMessage ?? "") + "</td>");
                        html.AppendLine("                            </tr>");
                    }
                    
                    html.AppendLine("                        </tbody>");
                    html.AppendLine("                    </table>");
                    html.AppendLine("                </div>");
                }
                
                html.AppendLine("            </div>");
                html.AppendLine("        </div>");
                
                // 页脚
                html.AppendLine("        <div class=\"footer\">");
                html.AppendLine("            <p>报告生成时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
                html.AppendLine("            <p>总执行时间: " + session.TotalDurationMs + " ms</p>");
                html.AppendLine("        </div>");
                html.AppendLine("    </div>");
                html.AppendLine("</body>");
                html.AppendLine("</html>");
                
                return html.ToString();
            }
            catch (Exception ex)
            {
                throw new ReportGenerationException($"生成HTML报告失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 生成XML格式的测试报告
        /// </summary>
        public async Task<string> GenerateXmlReportAsync(TestSessionDO session)
        {
            try
            {
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "    ",
                    NewLineChars = "\n",
                    Encoding = System.Text.Encoding.UTF8
                };

                using var stringWriter = new System.IO.StringWriter();
                using var xmlWriter = XmlWriter.Create(stringWriter, settings);

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("TestSessionReport");
                
                // 会话信息
                xmlWriter.WriteStartElement("SessionInfo");
                xmlWriter.WriteElementString("SessionId", session.SessionId);
                xmlWriter.WriteElementString("SessionName", session.SessionName);
                xmlWriter.WriteElementString("ProjectPath", session.ProjectPath);
                xmlWriter.WriteElementString("SolutionPath", session.SolutionPath);
                xmlWriter.WriteElementString("StartTime", session.StartTime.ToString("O"));
                xmlWriter.WriteElementString("EndTime", session.EndTime.ToString("O"));
                xmlWriter.WriteElementString("Status", session.SessionStatus.ToString());
                xmlWriter.WriteElementString("BuildConfiguration", session.BuildConfiguration);
                xmlWriter.WriteElementString("TargetFramework", session.TargetFramework);
                xmlWriter.WriteEndElement();

                // 测试统计
                xmlWriter.WriteStartElement("TestStatistics");
                xmlWriter.WriteElementString("TotalTests", session.TotalTests.ToString());
                xmlWriter.WriteElementString("PassedTests", session.PassedTests.ToString());
                xmlWriter.WriteElementString("FailedTests", session.FailedTests.ToString());
                xmlWriter.WriteElementString("SkippedTests", session.SkippedTests.ToString());
                xmlWriter.WriteElementString("PassRate", session.PassRate.ToString("F2"));
                xmlWriter.WriteElementString("TotalDurationMs", session.TotalDurationMs.ToString());
                xmlWriter.WriteEndElement();

                // 覆盖率指标
                if (session.CoverageMetrics != null)
                {
                    xmlWriter.WriteStartElement("CoverageMetrics");
                    xmlWriter.WriteElementString("ProjectName", session.CoverageMetrics.ProjectName);
                    xmlWriter.WriteElementString("LineCoverage", session.CoverageMetrics.LineCoverage.ToString("F2"));
                    xmlWriter.WriteElementString("BranchCoverage", session.CoverageMetrics.BranchCoverage.ToString("F2"));
                    xmlWriter.WriteElementString("MethodCoverage", session.CoverageMetrics.MethodCoverage.ToString("F2"));
                    xmlWriter.WriteElementString("ClassCoverage", session.CoverageMetrics.ClassCoverage.ToString("F2"));
                    xmlWriter.WriteElementString("CoverageGrade", session.CoverageMetrics.CoverageGrade.ToString());
                    xmlWriter.WriteElementString("GeneratedAt", session.CoverageMetrics.GeneratedAt.ToString("O"));
                    xmlWriter.WriteEndElement();
                }

                // 质量门禁
                if (session.QualityGates.Count > 0)
                {
                    xmlWriter.WriteStartElement("QualityGates");
                    foreach (var gate in session.QualityGates)
                    {
                        xmlWriter.WriteStartElement("QualityGate");
                        xmlWriter.WriteElementString("GateId", gate.GateId);
                        xmlWriter.WriteElementString("GateName", gate.GateName);
                        xmlWriter.WriteElementString("GateType", gate.GateType.ToString());
                        xmlWriter.WriteElementString("Status", gate.Status.ToString());
                        xmlWriter.WriteElementString("Threshold", gate.Threshold.ToString("F2"));
                        xmlWriter.WriteElementString("CurrentValue", gate.CurrentValue.ToString("F2"));
                        xmlWriter.WriteElementString("Message", gate.Message);
                        xmlWriter.WriteElementString("CheckedAt", gate.CheckedAt.ToString("O"));
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }

                // 测试结果
                xmlWriter.WriteStartElement("TestResults");
                foreach (var result in session.TestResults)
                {
                    xmlWriter.WriteStartElement("TestResult");
                    xmlWriter.WriteElementString("Id", result.Id);
                    xmlWriter.WriteElementString("Name", result.Name);
                    xmlWriter.WriteElementString("Type", result.Type);
                    xmlWriter.WriteElementString("Category", result.Category);
                    xmlWriter.WriteElementString("Status", result.Status.ToString());
                    xmlWriter.WriteElementString("StartTime", result.StartTime.ToString("O"));
                    xmlWriter.WriteElementString("EndTime", result.EndTime.ToString("O"));
                    xmlWriter.WriteElementString("DurationMs", result.DurationMs.ToString());
                    xmlWriter.WriteElementString("ProjectPath", result.ProjectPath);
                    xmlWriter.WriteElementString("MethodFullName", result.MethodFullName);
                    
                    if (!string.IsNullOrEmpty(result.ErrorMessage))
                    {
                        xmlWriter.WriteElementString("ErrorMessage", result.ErrorMessage);
                    }
                    
                    if (!string.IsNullOrEmpty(result.ErrorStackTrace))
                    {
                        xmlWriter.WriteElementString("ErrorStackTrace", result.ErrorStackTrace);
                    }
                    
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();

                return stringWriter.ToString();
            }
            catch (Exception ex)
            {
                throw new ReportGenerationException($"生成XML报告失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 生成JSON格式的测试报告
        /// </summary>
        public async Task<string> GenerateJsonReportAsync(TestSessionDO session)
        {
            try
            {
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                };

                return System.Text.Json.JsonSerializer.Serialize(session, options);
            }
            catch (Exception ex)
            {
                throw new ReportGenerationException($"生成JSON报告失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 生成Markdown格式的测试报告
        /// </summary>
        public async Task<string> GenerateMarkdownReportAsync(TestSessionDO session)
        {
            try
            {
                var markdown = new System.Text.StringBuilder();
                
                markdown.AppendLine("# 测试报告");
                markdown.AppendLine();
                
                markdown.AppendLine("## 会话信息");
                markdown.AppendLine("- **会话ID**: " + session.SessionId);
                markdown.AppendLine("- **会话名称**: " + session.SessionName);
                markdown.AppendLine("- **项目路径**: " + session.ProjectPath);
                markdown.AppendLine("- **解决方案路径**: " + session.SolutionPath);
                markdown.AppendLine("- **开始时间**: " + session.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                markdown.AppendLine("- **结束时间**: " + session.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                markdown.AppendLine("- **会话状态**: " + session.SessionStatus);
                markdown.AppendLine("- **构建配置**: " + session.BuildConfiguration);
                markdown.AppendLine("- **目标框架**: " + session.TargetFramework);
                markdown.AppendLine();
                
                markdown.AppendLine("## 测试统计");
                markdown.AppendLine("- **总测试数**: " + session.TotalTests);
                markdown.AppendLine("- **通过测试**: " + session.PassedTests);
                markdown.AppendLine("- **失败测试**: " + session.FailedTests);
                markdown.AppendLine("- **跳过测试**: " + session.SkippedTests);
                markdown.AppendLine("- **通过率**: " + session.PassRate.ToString("F2") + "%");
                markdown.AppendLine("- **总执行时间**: " + session.TotalDurationMs + " ms");
                markdown.AppendLine();
                
                if (session.CoverageMetrics != null)
                {
                    markdown.AppendLine("## 覆盖率指标");
                    markdown.AppendLine("- **项目名称**: " + session.CoverageMetrics.ProjectName);
                    markdown.AppendLine("- **行覆盖率**: " + session.CoverageMetrics.LineCoverage.ToString("F2") + "%");
                    markdown.AppendLine("- **分支覆盖率**: " + session.CoverageMetrics.BranchCoverage.ToString("F2") + "%");
                    markdown.AppendLine("- **方法覆盖率**: " + session.CoverageMetrics.MethodCoverage.ToString("F2") + "%");
                    markdown.AppendLine("- **类覆盖率**: " + session.CoverageMetrics.ClassCoverage.ToString("F2") + "%");
                    markdown.AppendLine("- **覆盖率等级**: " + session.CoverageMetrics.CoverageGrade);
                    markdown.AppendLine();
                }
                
                if (session.QualityGates.Count > 0)
                {
                    markdown.AppendLine("## 质量门禁");
                    markdown.AppendLine("| 门禁名称 | 类型 | 状态 | 当前值 | 阈值 | 消息 |");
                    markdown.AppendLine("|----------|------|------|--------|------|------|");
                    
                    foreach (var gate in session.QualityGates)
                    {
                        markdown.AppendLine($"| {gate.GateName} | {gate.GateType} | {gate.Status} | {gate.CurrentValue.ToString("F2")} | {gate.Threshold.ToString("F2")} | {gate.Message} |");
                    }
                    markdown.AppendLine();
                }
                
                markdown.AppendLine("## 测试结果详情");
                
                var groupedResults = session.TestResults.GroupBy(r => r.Status);
                foreach (var group in groupedResults)
                {
                    markdown.AppendLine("### " + group.Key + " (" + group.Count() + ")");
                    markdown.AppendLine("| 测试名称 | 类别 | 执行时间 | 错误信息 |");
                    markdown.AppendLine("|----------|------|----------|----------|");
                    
                    foreach (var result in group)
                    {
                        markdown.AppendLine($"| {result.Name} | {result.Category} | {result.DurationMs} ms | {result.ErrorMessage ?? ""} |");
                    }
                    markdown.AppendLine();
                }
                
                markdown.AppendLine("---");
                markdown.AppendLine("*报告生成时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*");
                markdown.AppendLine("*总执行时间: " + session.TotalDurationMs + " ms*");
                
                return markdown.ToString();
            }
            catch (Exception ex)
            {
                throw new ReportGenerationException($"生成Markdown报告失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 生成CSV格式的测试报告
        /// </summary>
        public async Task<string> GenerateCsvReportAsync(TestSessionDO session)
        {
            try
            {
                var csv = new System.Text.StringBuilder();
                
                // CSV头部
                csv.AppendLine("SessionId,SessionName,TestName,TestCategory,TestType,Status,DurationMs,StartTime,EndTime,ErrorMessage,ProjectPath");
                
                // 测试结果数据
                foreach (var result in session.TestResults)
                {
                    csv.AppendLine($"{session.SessionId},{session.SessionName},{result.Name},{result.Category},{result.Type},{result.Status},{result.DurationMs},{result.StartTime:O},{result.EndTime:O},\"{result.ErrorMessage ?? ""}\",{result.ProjectPath}");
                }
                
                return csv.ToString();
            }
            catch (Exception ex)
            {
                throw new ReportGenerationException($"生成CSV报告失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取默认CSS样式
        /// </summary>
        private async Task<string> GetDefaultCssAsync()
        {
            return await Task.FromResult(@"
                body {
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 20px;
                    background-color: #f5f5f5;
                }
                .container {
                    max-width: 1200px;
                    margin: 0 auto;
                    background-color: white;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                }
                h1 {
                    color: #333;
                    border-bottom: 2px solid #007acc;
                    padding-bottom: 10px;
                }
                h2 {
                    color: #555;
                    margin-top: 30px;
                }
                .section {
                    margin-bottom: 30px;
                }
                .info-table {
                    width: 100%;
                    border-collapse: collapse;
                    margin-bottom: 20px;
                }
                .info-table th, .info-table td {
                    border: 1px solid #ddd;
                    padding: 8px;
                    text-align: left;
                }
                .info-table th {
                    background-color: #f2f2f2;
                    font-weight: bold;
                }
                .stats-grid {
                    display: grid;
                    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
                    gap: 20px;
                    margin-bottom: 20px;
                }
                .stat-card {
                    background-color: #f8f9fa;
                    padding: 20px;
                    border-radius: 8px;
                    text-align: center;
                    border-left: 4px solid #007acc;
                }
                .stat-card.passed {
                    border-left-color: #28a745;
                }
                .stat-card.failed {
                    border-left-color: #dc3545;
                }
                .stat-card.skipped {
                    border-left-color: #ffc107;
                }
                .stat-card.total {
                    border-left-color: #007acc;
                }
                .stat-number {
                    font-size: 2em;
                    font-weight: bold;
                    color: #333;
                }
                .pass-rate {
                    background-color: #e9ecef;
                    height: 30px;
                    border-radius: 15px;
                    overflow: hidden;
                    position: relative;
                }
                .pass-rate-bar {
                    height: 100%;
                    background-color: #28a745;
                    transition: width 0.3s ease;
                }
                .pass-rate span {
                    position: absolute;
                    top: 50%;
                    left: 50%;
                    transform: translate(-50%, -50%);
                    font-weight: bold;
                    color: #333;
                }
                .coverage-grid {
                    display: grid;
                    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
                    gap: 20px;
                }
                .coverage-item {
                    background-color: #f8f9fa;
                    padding: 15px;
                    border-radius: 8px;
                }
                .coverage-label {
                    font-weight: bold;
                    margin-bottom: 5px;
                }
                .coverage-bar {
                    height: 20px;
                    background-color: #007acc;
                    border-radius: 10px;
                    margin-bottom: 5px;
                }
                .coverage-value {
                    font-weight: bold;
                    color: #333;
                }
                .gates-table, .results-table {
                    width: 100%;
                    border-collapse: collapse;
                    margin-top: 10px;
                }
                .gates-table th, .gates-table td,
                .results-table th, .results-table td {
                    border: 1px solid #ddd;
                    padding: 8px;
                    text-align: left;
                }
                .gates-table th, .results-table th {
                    background-color: #f2f2f2;
                }
                .status-passed {
                    color: #28a745;
                    font-weight: bold;
                }
                .status-failed {
                    color: #dc3545;
                    font-weight: bold;
                }
                .status-skipped {
                    color: #ffc107;
                    font-weight: bold;
                }
                .status-completed {
                    color: #28a745;
                    font-weight: bold;
                }
                .status-failed {
                    color: #dc3545;
                    font-weight: bold;
                }
                .status-cancelled {
                    color: #6c757d;
                    font-weight: bold;
                }
                .result-group {
                    margin-bottom: 20px;
                }
                .result-group h3 {
                    color: #333;
                    border-bottom: 1px solid #ddd;
                    padding-bottom: 5px;
                }
                .footer {
                    margin-top: 40px;
                    padding-top: 20px;
                    border-top: 1px solid #ddd;
                    color: #666;
                    text-align: center;
                }
            ");
        }

        /// <summary>
        /// 保存报告到文件
        /// </summary>
        public async Task SaveReportAsync(string reportContent, string filePath, ReportFormat format)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                await File.WriteAllTextAsync(filePath, reportContent);
            }
            catch (Exception ex)
            {
                throw new ReportGenerationException($"保存报告失败: {ex.Message}", ex);
            }
        }
    }

    /// <summary>
    /// 报告格式枚举
    /// </summary>
    public enum ReportFormat
    {
        Html,
        Xml,
        Json,
        Markdown,
        Csv
    }

    /// <summary>
    /// 报告生成异常
    /// </summary>
    public class ReportGenerationException : Exception
    {
        public ReportGenerationException(string message) : base(message) { }

        public ReportGenerationException(string message, Exception innerException) : base(message, innerException) { }
    }
}