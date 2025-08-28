using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO.Testing;

namespace BannerlordModEditor.Common.Utils.Testing
{
    /// <summary>
    /// 测试通知服务
    /// 提供多种通知渠道的测试结果通知功能
    /// </summary>
    public class TestNotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, NotificationChannel> _channels;

        /// <summary>
        /// 初始化测试通知服务
        /// </summary>
        public TestNotificationService()
        {
            _httpClient = new HttpClient();
            _channels = new Dictionary<string, NotificationChannel>();
            
            // 初始化默认通知渠道
            InitializeDefaultChannels();
        }

        /// <summary>
        /// 初始化默认通知渠道
        /// </summary>
        private void InitializeDefaultChannels()
        {
            // 文件日志渠道
            _channels["file"] = new NotificationChannel
            {
                Name = "File Log",
                Type = NotificationType.File,
                Enabled = true,
                Config = new Dictionary<string, string>
                {
                    { "log_path", "./logs/test_notifications.log" }
                }
            };

            // 控制台渠道
            _channels["console"] = new NotificationChannel
            {
                Name = "Console",
                Type = NotificationType.Console,
                Enabled = true,
                Config = new Dictionary<string, string>()
            };
        }

        /// <summary>
        /// 发送测试会话完成通知
        /// </summary>
        public async Task SendTestSessionCompletedNotificationAsync(TestSessionDO session, 
            IEnumerable<string> channelNames = null)
        {
            try
            {
                var channels = GetEnabledChannels(channelNames);
                
                foreach (var channel in channels)
                {
                    await SendNotificationAsync(channel, CreateTestSessionCompletedMessage(session));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送测试会话完成通知失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送质量门禁失败通知
        /// </summary>
        public async Task SendQualityGateFailedNotificationAsync(TestSessionDO session, 
            IEnumerable<string> failedGates, IEnumerable<string> channelNames = null)
        {
            try
            {
                var channels = GetEnabledChannels(channelNames);
                
                foreach (var channel in channels)
                {
                    await SendNotificationAsync(channel, CreateQualityGateFailedMessage(session, failedGates));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送质量门禁失败通知失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送测试失败通知
        /// </summary>
        public async Task SendTestFailedNotificationAsync(TestSessionDO session, 
            IEnumerable<TestResultDO> failedTests, IEnumerable<string> channelNames = null)
        {
            try
            {
                var channels = GetEnabledChannels(channelNames);
                
                foreach (var channel in channels)
                {
                    await SendNotificationAsync(channel, CreateTestFailedMessage(session, failedTests));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送测试失败通知失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送性能警告通知
        /// </summary>
        public async Task SendPerformanceWarningNotificationAsync(TestSessionDO session, 
            IEnumerable<string> warnings, IEnumerable<string> channelNames = null)
        {
            try
            {
                var channels = GetEnabledChannels(channelNames);
                
                foreach (var channel in channels)
                {
                    await SendNotificationAsync(channel, CreatePerformanceWarningMessage(session, warnings));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送性能警告通知失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送自定义通知
        /// </summary>
        public async Task SendCustomNotificationAsync(string title, string message, 
            NotificationLevel level = NotificationLevel.Info, IEnumerable<string> channelNames = null)
        {
            try
            {
                var channels = GetEnabledChannels(channelNames);
                var notification = new NotificationMessage
                {
                    Title = title,
                    Message = message,
                    Level = level,
                    Timestamp = DateTime.Now
                };
                
                foreach (var channel in channels)
                {
                    await SendNotificationAsync(channel, notification);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送自定义通知失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 添加通知渠道
        /// </summary>
        public void AddNotificationChannel(string name, NotificationChannel channel)
        {
            _channels[name] = channel;
        }

        /// <summary>
        /// 移除通知渠道
        /// </summary>
        public void RemoveNotificationChannel(string name)
        {
            _channels.Remove(name);
        }

        /// <summary>
        /// 启用/禁用通知渠道
        /// </summary>
        public void SetChannelEnabled(string name, bool enabled)
        {
            if (_channels.TryGetValue(name, out var channel))
            {
                channel.Enabled = enabled;
            }
        }

        /// <summary>
        /// 获取通知渠道
        /// </summary>
        public NotificationChannel? GetNotificationChannel(string name)
        {
            _channels.TryGetValue(name, out var channel);
            return channel;
        }

        /// <summary>
        /// 获取所有通知渠道
        /// </summary>
        public Dictionary<string, NotificationChannel> GetAllNotificationChannels()
        {
            return new Dictionary<string, NotificationChannel>(_channels);
        }

        /// <summary>
        /// 获取启用的通知渠道
        /// </summary>
        public List<NotificationChannel> GetEnabledChannels(IEnumerable<string> channelNames = null)
        {
            var channels = channelNames != null 
                ? _channels.Where(kvp => channelNames.Contains(kvp.Key)).Select(kvp => kvp.Value)
                : _channels.Values;
            
            return channels.Where(c => c.Enabled).ToList();
        }

        /// <summary>
        /// 发送通知到指定渠道
        /// </summary>
        private async Task SendNotificationAsync(NotificationChannel channel, NotificationMessage message)
        {
            switch (channel.Type)
            {
                case NotificationType.Console:
                    await SendConsoleNotificationAsync(channel, message);
                    break;
                    
                case NotificationType.File:
                    await SendFileNotificationAsync(channel, message);
                    break;
                    
                case NotificationType.Email:
                    await SendEmailNotificationAsync(channel, message);
                    break;
                    
                case NotificationType.Webhook:
                    await SendWebhookNotificationAsync(channel, message);
                    break;
                    
                case NotificationType.Slack:
                    await SendSlackNotificationAsync(channel, message);
                    break;
                    
                case NotificationType.Discord:
                    await SendDiscordNotificationAsync(channel, message);
                    break;
                    
                default:
                    Console.WriteLine($"不支持的通知类型: {channel.Type}");
                    break;
            }
        }

        /// <summary>
        /// 发送控制台通知
        /// </summary>
        private async Task SendConsoleNotificationAsync(NotificationChannel channel, NotificationMessage message)
        {
            await Task.Run(() =>
            {
                var color = GetConsoleColor(message.Level);
                var originalColor = Console.ForegroundColor;
                
                Console.ForegroundColor = color;
                Console.WriteLine($"[{message.Timestamp:yyyy-MM-dd HH:mm:ss}] {message.Level}: {message.Title}");
                Console.WriteLine(message.Message);
                Console.WriteLine(new string('-', 50));
                Console.ForegroundColor = originalColor;
            });
        }

        /// <summary>
        /// 发送文件通知
        /// </summary>
        private async Task SendFileNotificationAsync(NotificationChannel channel, NotificationMessage message)
        {
            var logPath = channel.Config.GetValueOrDefault("log_path", "./logs/test_notifications.log");
            var directory = Path.GetDirectoryName(logPath);
            
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            var logEntry = $"[{message.Timestamp:yyyy-MM-dd HH:mm:ss}] {message.Level}: {message.Title}\n{message.Message}\n{new string('-', 50)}\n";
            
            await File.AppendAllTextAsync(logPath, logEntry);
        }

        /// <summary>
        /// 发送邮件通知
        /// </summary>
        private async Task SendEmailNotificationAsync(NotificationChannel channel, NotificationMessage message)
        {
            // 简化实现，实际应用中需要集成邮件服务
            var smtpServer = channel.Config.GetValueOrDefault("smtp_server");
            var smtpPort = channel.Config.GetValueOrDefault("smtp_port", "587");
            var username = channel.Config.GetValueOrDefault("username");
            var password = channel.Config.GetValueOrDefault("password");
            var fromAddress = channel.Config.GetValueOrDefault("from_address");
            var toAddresses = channel.Config.GetValueOrDefault("to_addresses");
            
            if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(toAddresses))
            {
                Console.WriteLine("邮件通知配置不完整，跳过发送");
                return;
            }
            
            // 这里应该实现实际的邮件发送逻辑
            Console.WriteLine($"发送邮件通知到: {toAddresses}");
            Console.WriteLine($"标题: {message.Title}");
            Console.WriteLine($"内容: {message.Message}");
        }

        /// <summary>
        /// 发送Webhook通知
        /// </summary>
        private async Task SendWebhookNotificationAsync(NotificationChannel channel, NotificationMessage message)
        {
            var webhookUrl = channel.Config.GetValueOrDefault("webhook_url");
            if (string.IsNullOrEmpty(webhookUrl))
            {
                Console.WriteLine("Webhook URL未配置，跳过发送");
                return;
            }
            
            try
            {
                var payload = new
                {
                    title = message.Title,
                    message = message.Message,
                    level = message.Level.ToString(),
                    timestamp = message.Timestamp.ToString("O")
                };
                
                var response = await _httpClient.PostAsJsonAsync(webhookUrl, payload);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送Webhook通知失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送Slack通知
        /// </summary>
        private async Task SendSlackNotificationAsync(NotificationChannel channel, NotificationMessage message)
        {
            var webhookUrl = channel.Config.GetValueOrDefault("slack_webhook_url");
            if (string.IsNullOrEmpty(webhookUrl))
            {
                Console.WriteLine("Slack Webhook URL未配置，跳过发送");
                return;
            }
            
            try
            {
                var color = GetSlackColor(message.Level);
                var payload = new
                {
                    attachments = new[]
                    {
                        new
                        {
                            color = color,
                            title = message.Title,
                            text = message.Message,
                            ts = ((DateTimeOffset)message.Timestamp).ToUnixTimeSeconds()
                        }
                    }
                };
                
                var response = await _httpClient.PostAsJsonAsync(webhookUrl, payload);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送Slack通知失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送Discord通知
        /// </summary>
        private async Task SendDiscordNotificationAsync(NotificationChannel channel, NotificationMessage message)
        {
            var webhookUrl = channel.Config.GetValueOrDefault("discord_webhook_url");
            if (string.IsNullOrEmpty(webhookUrl))
            {
                Console.WriteLine("Discord Webhook URL未配置，跳过发送");
                return;
            }
            
            try
            {
                var color = GetDiscordColor(message.Level);
                var payload = new
                {
                    embeds = new[]
                    {
                        new
                        {
                            title = message.Title,
                            description = message.Message,
                            color = color,
                            timestamp = message.Timestamp.ToString("O")
                        }
                    }
                };
                
                var response = await _httpClient.PostAsJsonAsync(webhookUrl, payload);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送Discord通知失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建测试会话完成消息
        /// </summary>
        private NotificationMessage CreateTestSessionCompletedMessage(TestSessionDO session)
        {
            var status = session.SessionStatus == TestSessionStatus.Completed ? "✅ 完成" : "❌ 失败";
            var message = new StringBuilder();
            
            message.AppendLine($"测试会话 {status}");
            message.AppendLine($"会话名称: {session.SessionName}");
            message.AppendLine($"总测试数: {session.TotalTests}");
            message.AppendLine($"通过测试: {session.PassedTests}");
            message.AppendLine($"失败测试: {session.FailedTests}");
            message.AppendLine($"跳过测试: {session.SkippedTests}");
            message.AppendLine($"通过率: {session.PassRate:F2}%");
            message.AppendLine($"执行时间: {session.TotalDurationMs} ms");
            
            if (session.CoverageMetrics != null)
            {
                message.AppendLine($"代码覆盖率: {session.CoverageMetrics.LineCoverage:F2}%");
            }
            
            return new NotificationMessage
            {
                Title = $"测试会话{status} - {session.SessionName}",
                Message = message.ToString(),
                Level = session.SessionStatus == TestSessionStatus.Completed ? NotificationLevel.Success : NotificationLevel.Error,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 创建质量门禁失败消息
        /// </summary>
        private NotificationMessage CreateQualityGateFailedMessage(TestSessionDO session, IEnumerable<string> failedGates)
        {
            var message = new StringBuilder();
            
            message.AppendLine($"⚠️ 质量门禁检查失败");
            message.AppendLine($"会话名称: {session.SessionName}");
            message.AppendLine($"失败的门禁:");
            
            foreach (var gate in failedGates)
            {
                message.AppendLine($"  - {gate}");
            }
            
            message.AppendLine($"请检查并修复相关问题后重新运行测试。");
            
            return new NotificationMessage
            {
                Title = $"质量门禁失败 - {session.SessionName}",
                Message = message.ToString(),
                Level = NotificationLevel.Warning,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 创建测试失败消息
        /// </summary>
        private NotificationMessage CreateTestFailedMessage(TestSessionDO session, IEnumerable<TestResultDO> failedTests)
        {
            var message = new StringBuilder();
            
            message.AppendLine($"❌ 测试失败");
            message.AppendLine($"会话名称: {session.SessionName}");
            message.AppendLine($"失败测试数: {failedTests.Count()}");
            message.AppendLine("失败的测试:");
            
            foreach (var test in failedTests.Take(5)) // 只显示前5个失败的测试
            {
                message.AppendLine($"  - {test.Name}: {test.ErrorMessage ?? "未知错误"}");
            }
            
            if (failedTests.Count() > 5)
            {
                message.AppendLine($"  ... 还有 {failedTests.Count() - 5} 个失败的测试");
            }
            
            return new NotificationMessage
            {
                Title = $"测试失败 - {session.SessionName}",
                Message = message.ToString(),
                Level = NotificationLevel.Error,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 创建性能警告消息
        /// </summary>
        private NotificationMessage CreatePerformanceWarningMessage(TestSessionDO session, IEnumerable<string> warnings)
        {
            var message = new StringBuilder();
            
            message.AppendLine($"⚡ 性能警告");
            message.AppendLine($"会话名称: {session.SessionName}");
            message.AppendLine($"总执行时间: {session.TotalDurationMs} ms");
            message.AppendLine("性能问题:");
            
            foreach (var warning in warnings)
            {
                message.AppendLine($"  - {warning}");
            }
            
            return new NotificationMessage
            {
                Title = $"性能警告 - {session.SessionName}",
                Message = message.ToString(),
                Level = NotificationLevel.Warning,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 获取控制台颜色
        /// </summary>
        private ConsoleColor GetConsoleColor(NotificationLevel level)
        {
            return level switch
            {
                NotificationLevel.Debug => ConsoleColor.Gray,
                NotificationLevel.Info => ConsoleColor.White,
                NotificationLevel.Success => ConsoleColor.Green,
                NotificationLevel.Warning => ConsoleColor.Yellow,
                NotificationLevel.Error => ConsoleColor.Red,
                NotificationLevel.Critical => ConsoleColor.DarkRed,
                _ => ConsoleColor.White
            };
        }

        /// <summary>
        /// 获取Slack颜色
        /// </summary>
        private string GetSlackColor(NotificationLevel level)
        {
            return level switch
            {
                NotificationLevel.Debug => "#808080",
                NotificationLevel.Info => "#36a64f",
                NotificationLevel.Success => "#36a64f",
                NotificationLevel.Warning => "#ff9500",
                NotificationLevel.Error => "#ff0000",
                NotificationLevel.Critical => "#990000",
                _ => "#36a64f"
            };
        }

        /// <summary>
        /// 获取Discord颜色
        /// </summary>
        private int GetDiscordColor(NotificationLevel level)
        {
            return level switch
            {
                NotificationLevel.Debug => 0x808080,
                NotificationLevel.Info => 0x36a64f,
                NotificationLevel.Success => 0x36a64f,
                NotificationLevel.Warning => 0xff9500,
                NotificationLevel.Error => 0xff0000,
                NotificationLevel.Critical => 0x990000,
                _ => 0x36a64f
            };
        }
    }

    /// <summary>
    /// 通知消息
    /// </summary>
    public class NotificationMessage
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 通知级别
        /// </summary>
        public NotificationLevel Level { get; set; } = NotificationLevel.Info;

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 通知渠道
    /// </summary>
    public class NotificationChannel
    {
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 通知类型
        /// </summary>
        public NotificationType Type { get; set; } = NotificationType.Console;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 配置参数
        /// </summary>
        public Dictionary<string, string> Config { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// 通知类型枚举
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// 控制台
        /// </summary>
        Console,

        /// <summary>
        /// 文件
        /// </summary>
        File,

        /// <summary>
        /// 邮件
        /// </summary>
        Email,

        /// <summary>
        /// Webhook
        /// </summary>
        Webhook,

        /// <summary>
        /// Slack
        /// </summary>
        Slack,

        /// <summary>
        /// Discord
        /// </summary>
        Discord
    }

    /// <summary>
    /// 通知级别枚举
    /// </summary>
    public enum NotificationLevel
    {
        /// <summary>
        /// 调试
        /// </summary>
        Debug,

        /// <summary>
        /// 信息
        /// </summary>
        Info,

        /// <summary>
        /// 成功
        /// </summary>
        Success,

        /// <summary>
        /// 警告
        /// </summary>
        Warning,

        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 严重
        /// </summary>
        Critical
    }
}