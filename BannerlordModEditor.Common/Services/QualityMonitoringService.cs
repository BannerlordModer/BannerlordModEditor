using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DO.Engine;

namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// 质量监控服务
    /// 用于监控XML序列化/反序列化的质量和性能
    /// </summary>
    public class QualityMonitoringService
    {
        private readonly Dictionary<string, QualityMetrics> _metrics = new();
        private readonly object _lock = new();

        /// <summary>
        /// 质量指标数据结构
        /// </summary>
        public class QualityMetrics
        {
            public int TotalOperations { get; set; }
            public int SuccessfulOperations { get; set; }
            public int FailedOperations { get; set; }
            public double AverageExecutionTime { get; set; }
            public double MaxExecutionTime { get; set; }
            public double MinExecutionTime { get; set; }
            public List<string> ErrorMessages { get; set; } = new();
            public Dictionary<string, int> ErrorFrequency { get; set; } = new();
            public DateTime LastOperationTime { get; set; }
            public DateTime FirstOperationTime { get; set; }
        }

        /// <summary>
        /// 记录操作指标
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <param name="executionTime">执行时间（毫秒）</param>
        /// <param name="success">是否成功</param>
        /// <param name="errorMessage">错误信息（如果失败）</param>
        public void RecordOperation(string operationType, double executionTime, bool success, string? errorMessage = null)
        {
            lock (_lock)
            {
                if (!_metrics.ContainsKey(operationType))
                {
                    _metrics[operationType] = new QualityMetrics
                    {
                        FirstOperationTime = DateTime.Now,
                        MinExecutionTime = executionTime,
                        MaxExecutionTime = executionTime
                    };
                }

                var metrics = _metrics[operationType];
                metrics.TotalOperations++;
                metrics.LastOperationTime = DateTime.Now;

                if (success)
                {
                    metrics.SuccessfulOperations++;
                }
                else
                {
                    metrics.FailedOperations++;
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        metrics.ErrorMessages.Add(errorMessage);
                        metrics.ErrorFrequency[errorMessage] = metrics.ErrorFrequency.GetValueOrDefault(errorMessage) + 1;
                    }
                }

                // 更新执行时间统计
                metrics.AverageExecutionTime = (metrics.AverageExecutionTime * (metrics.TotalOperations - 1) + executionTime) / metrics.TotalOperations;
                metrics.MaxExecutionTime = Math.Max(metrics.MaxExecutionTime, executionTime);
                metrics.MinExecutionTime = Math.Min(metrics.MinExecutionTime, executionTime);
            }
        }

        /// <summary>
        /// 获取质量报告
        /// </summary>
        /// <returns>质量报告字符串</returns>
        public string GetQualityReport()
        {
            lock (_lock)
            {
                var report = new System.Text.StringBuilder();
                report.AppendLine("=== 质量监控报告 ===");
                report.AppendLine($"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine();

                foreach (var kvp in _metrics)
                {
                    var operationType = kvp.Key;
                    var metrics = kvp.Value;

                    report.AppendLine($"操作类型: {operationType}");
                    report.AppendLine($"  总操作数: {metrics.TotalOperations}");
                    report.AppendLine($"  成功操作数: {metrics.SuccessfulOperations}");
                    report.AppendLine($"  失败操作数: {metrics.FailedOperations}");
                    report.AppendLine($"  成功率: {(metrics.TotalOperations > 0 ? (metrics.SuccessfulOperations * 100.0 / metrics.TotalOperations) : 0):F2}%");
                    report.AppendLine($"  平均执行时间: {metrics.AverageExecutionTime:F2} ms");
                    report.AppendLine($"  最小执行时间: {metrics.MinExecutionTime:F2} ms");
                    report.AppendLine($"  最大执行时间: {metrics.MaxExecutionTime:F2} ms");
                    report.AppendLine($"  首次操作时间: {metrics.FirstOperationTime:yyyy-MM-dd HH:mm:ss}");
                    report.AppendLine($"  最后操作时间: {metrics.LastOperationTime:yyyy-MM-dd HH:mm:ss}");

                    if (metrics.ErrorMessages.Count > 0)
                    {
                        report.AppendLine($"  常见错误:");
                        foreach (var error in metrics.ErrorFrequency.OrderByDescending(x => x.Value).Take(3))
                        {
                            report.AppendLine($"    - {error.Key} (出现次数: {error.Value})");
                        }
                    }

                    report.AppendLine();
                }

                return report.ToString();
            }
        }

        /// <summary>
        /// 获取操作成功率
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <returns>成功率（0-100）</returns>
        public double GetSuccessRate(string operationType)
        {
            lock (_lock)
            {
                if (!_metrics.ContainsKey(operationType))
                    return 0.0;

                var metrics = _metrics[operationType];
                return metrics.TotalOperations > 0 ? (metrics.SuccessfulOperations * 100.0 / metrics.TotalOperations) : 0.0;
            }
        }

        /// <summary>
        /// 获取性能等级
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <returns>性能等级（Excellent/Good/Average/Poor）</returns>
        public string GetPerformanceGrade(string operationType)
        {
            lock (_lock)
            {
                if (!_metrics.ContainsKey(operationType))
                    return "Unknown";

                var metrics = _metrics[operationType];
                var avgTime = metrics.AverageExecutionTime;

                if (avgTime < 10)
                    return "Excellent";
                else if (avgTime < 50)
                    return "Good";
                else if (avgTime < 200)
                    return "Average";
                else
                    return "Poor";
            }
        }

        /// <summary>
        /// 重置指标
        /// </summary>
        /// <param name="operationType">操作类型，如果为null则重置所有</param>
        public void ResetMetrics(string? operationType = null)
        {
            lock (_lock)
            {
                if (string.IsNullOrEmpty(operationType))
                {
                    _metrics.Clear();
                }
                else if (_metrics.ContainsKey(operationType))
                {
                    _metrics.Remove(operationType);
                }
            }
        }

        /// <summary>
        /// 监控XML序列化操作
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="operationName">操作名称</param>
        /// <returns>序列化的XML字符串</returns>
        public string MonitorSerialization<T>(T obj, string operationName)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var stringWriter = new StringWriter();
                using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings 
                { 
                    Indent = true, 
                    OmitXmlDeclaration = false
                });
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                serializer.Serialize(xmlWriter, obj, namespaces);
                
                var result = stringWriter.ToString();
                stopwatch.Stop();
                
                RecordOperation(operationName, stopwatch.ElapsedMilliseconds, true);
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                RecordOperation(operationName, stopwatch.ElapsedMilliseconds, false, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 监控XML反序列化操作
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xml">XML字符串</param>
        /// <param name="operationName">操作名称</param>
        /// <returns>反序列化的对象</returns>
        public T MonitorDeserialization<T>(string xml, string operationName)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var reader = new StringReader(xml);
                var result = (T)serializer.Deserialize(reader)!;
                
                stopwatch.Stop();
                RecordOperation(operationName, stopwatch.ElapsedMilliseconds, true);
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                RecordOperation(operationName, stopwatch.ElapsedMilliseconds, false, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 保存质量报告到文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public void SaveQualityReport(string filePath)
        {
            try
            {
                var report = GetQualityReport();
                File.WriteAllText(filePath, report);
            }
            catch (Exception ex)
            {
                // 记录保存失败的错误
                RecordOperation("SaveQualityReport", 0, false, ex.Message);
            }
        }
    }
}