using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Cli.Helpers;
using BannerlordModEditor.Common.Models.DO.Testing;
using BannerlordModEditor.Common.Services.Testing;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace BannerlordModEditor.Cli.Commands
{
    /// <summary>
    /// 质量门禁管理命令
    /// </summary>
    [Command("quality-gates", Description = "管理质量门禁配置")]
    public class QualityGatesCommand : ICommand
    {
        private readonly QualityGateService _qualityGateService;
        private readonly CliOutputHelper _outputHelper;

        /// <summary>
        /// 操作类型
        /// </summary>
        [CommandOption("action", 'a', Description = "操作类型 (list/add/update/remove/enable/disable/validate/export)")]
        public string Action { get; set; } = "list";

        /// <summary>
        /// 门禁ID
        /// </summary>
        [CommandOption("gate-id", Description = "质量门禁ID")]
        public string? GateId { get; set; }

        /// <summary>
        /// 门禁名称
        /// </summary>
        [CommandOption("name", Description = "质量门禁名称")]
        public string? Name { get; set; }

        /// <summary>
        /// 门禁类型
        /// </summary>
        [CommandOption("type", Description = "质量门禁类型 (TestPassRate/CodeCoverage/ExecutionTime/ErrorRate/Performance/Custom)")]
        public string? Type { get; set; }

        /// <summary>
        /// 阈值
        /// </summary>
        [CommandOption("threshold", Description = "质量门禁阈值")]
        public double? Threshold { get; set; }

        /// <summary>
        /// 操作符
        /// </summary>
        [CommandOption("operator", Description = "比较操作符 (Equal/NotEqual/GreaterThan/GreaterThanOrEqual/LessThan/LessThanOrEqual)")]
        public string? Operator { get; set; }

        /// <summary>
        /// 严重程度
        /// </summary>
        [CommandOption("severity", Description = "严重程度 (Low/Medium/High/Critical)")]
        public string? Severity { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [CommandOption("description", Description = "质量门禁描述")]
        public string? Description { get; set; }

        /// <summary>
        /// 失败消息
        /// </summary>
        [CommandOption("failure-message", Description = "失败时的消息模板")]
        public string? FailureMessage { get; set; }

        /// <summary>
        /// 成功消息
        /// </summary>
        [CommandOption("success-message", Description = "成功时的消息模板")]
        public string? SuccessMessage { get; set; }

        /// <summary>
        /// 输出文件
        /// </summary>
        [CommandOption("output-file", Description = "输出文件路径")]
        public string? OutputFile { get; set; }

        /// <summary>
        /// 是否详细输出
        /// </summary>
        [CommandOption("verbose", 'v', Description = "详细输出")]
        public bool Verbose { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public QualityGatesCommand(
            QualityGateService qualityGateService,
            CliOutputHelper outputHelper)
        {
            _qualityGateService = qualityGateService ?? throw new ArgumentNullException(nameof(qualityGateService));
            _outputHelper = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public async ValueTask ExecuteAsync(IConsole console)
        {
            try
            {
                _outputHelper.SetConsole(console);
                
                switch (Action.ToLower())
                {
                    case "list":
                        await ListQualityGates();
                        break;
                    case "add":
                        await AddQualityGate();
                        break;
                    case "update":
                        await UpdateQualityGate();
                        break;
                    case "remove":
                        await RemoveQualityGate();
                        break;
                    case "enable":
                        await EnableQualityGate(true);
                        break;
                    case "disable":
                        await EnableQualityGate(false);
                        break;
                    case "validate":
                        await ValidateConfiguration();
                        break;
                    case "export":
                        await ExportConfiguration();
                        break;
                    default:
                        throw new InvalidOperationException($"不支持的操作类型: {Action}");
                }
            }
            catch (Exception ex)
            {
                _outputHelper.WriteLine($"❌ 执行失败: {ex.Message}", ConsoleColor.Red);
                if (Verbose)
                {
                    _outputHelper.WriteLine($"堆栈跟踪: {ex.StackTrace}", ConsoleColor.Gray);
                }
                throw;
            }
        }

        /// <summary>
        /// 列出质量门禁
        /// </summary>
        private async Task ListQualityGates()
        {
            _outputHelper.WriteLine("🚪 质量门禁列表:", ConsoleColor.Cyan);
            
            var gates = _qualityGateService.GetAllQualityGateDefinitions();
            
            if (gates.Count == 0)
            {
                _outputHelper.WriteLine("  没有配置质量门禁", ConsoleColor.Yellow);
                return;
            }
            
            _outputHelper.WriteLine("  ID                            名称                          类型         阈值     操作符    严重程度  状态", ConsoleColor.White);
            _outputHelper.WriteLine("  " + new string('-', 100), ConsoleColor.White);
            
            foreach (var gate in gates)
            {
                var statusColor = gate.Enabled ? ConsoleColor.Green : ConsoleColor.Red;
                var severityColor = GetSeverityColor(gate.Severity);
                
                _outputHelper.WriteLine($"  {gate.Id,-30} {gate.Name,-28} {gate.Type,-12} {gate.Threshold,8:F2} {gate.Operator,-10} {gate.Severity,-8} {(gate.Enabled ? "启用" : "禁用")}", 
                    ConsoleColor.White);
            }
            
            if (Verbose)
            {
                _outputHelper.WriteLine(string.Empty);
                _outputHelper.WriteLine("📋 详细信息:", ConsoleColor.Cyan);
                foreach (var gate in gates)
                {
                    _outputHelper.WriteLine($"  门禁: {gate.Name} ({gate.Id})", ConsoleColor.White);
                    _outputHelper.WriteLine($"    描述: {gate.Description}", ConsoleColor.Gray);
                    _outputHelper.WriteLine($"    类型: {gate.Type}", ConsoleColor.Gray);
                    _outputHelper.WriteLine($"    阈值: {gate.Threshold}", ConsoleColor.Gray);
                    _outputHelper.WriteLine($"    操作符: {gate.Operator}", ConsoleColor.Gray);
                    _outputHelper.WriteLine($"    严重程度: {gate.Severity}", GetSeverityColor(gate.Severity));
                    _outputHelper.WriteLine($"    状态: {(gate.Enabled ? "启用" : "禁用")}", gate.Enabled ? ConsoleColor.Green : ConsoleColor.Red);
                    _outputHelper.WriteLine($"    失败消息: {gate.MessageTemplate}", ConsoleColor.Gray);
                    _outputHelper.WriteLine($"    成功消息: {gate.SuccessMessage}", ConsoleColor.Gray);
                    _outputHelper.WriteLine(string.Empty);
                }
            }
        }

        /// <summary>
        /// 添加质量门禁
        /// </summary>
        private async Task AddQualityGate()
        {
            if (string.IsNullOrEmpty(GateId) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Type))
            {
                throw new InvalidOperationException("添加质量门禁需要指定ID、名称和类型");
            }
            
            if (!Threshold.HasValue)
            {
                throw new InvalidOperationException("添加质量门禁需要指定阈值");
            }
            
            if (!Enum.TryParse<QualityGateType>(Type, true, out var gateType))
            {
                throw new InvalidOperationException($"不支持的质量门禁类型: {Type}");
            }
            
            if (!Enum.TryParse<ComparisonOperator>(Operator ?? "GreaterThanOrEqual", true, out var comparisonOperator))
            {
                throw new InvalidOperationException($"不支持的比较操作符: {Operator}");
            }
            
            if (!Enum.TryParse<RiskLevel>(Severity ?? "Medium", true, out var riskLevel))
            {
                throw new InvalidOperationException($"不支持的严重程度: {Severity}");
            }
            
            var gate = new QualityGateDefinition
            {
                Id = GateId,
                Name = Name,
                Type = gateType,
                Threshold = Threshold.Value,
                Operator = comparisonOperator,
                Severity = riskLevel,
                Description = Description ?? string.Empty,
                MessageTemplate = FailureMessage ?? $"{{current_value:F2}} 不满足条件 {{threshold:F2}}",
                SuccessMessage = SuccessMessage ?? $"质量门禁检查通过: {{current_value:F2}}",
                Enabled = true
            };
            
            _qualityGateService.AddQualityGate(gate);
            
            _outputHelper.WriteLine($"✅ 质量门禁 '{Name}' 添加成功", ConsoleColor.Green);
            
            if (Verbose)
            {
                _outputHelper.WriteLine($"  ID: {gate.Id}", ConsoleColor.White);
                _outputHelper.WriteLine($"  类型: {gate.Type}", ConsoleColor.White);
                _outputHelper.WriteLine($"  阈值: {gate.Threshold}", ConsoleColor.White);
                _outputHelper.WriteLine($"  操作符: {gate.Operator}", ConsoleColor.White);
                _outputHelper.WriteLine($"  严重程度: {gate.Severity}", GetSeverityColor(gate.Severity));
            }
        }

        /// <summary>
        /// 更新质量门禁
        /// </summary>
        private async Task UpdateQualityGate()
        {
            if (string.IsNullOrEmpty(GateId))
            {
                throw new InvalidOperationException("更新质量门禁需要指定门禁ID");
            }
            
            var existingGate = _qualityGateService.GetQualityGateDefinition(GateId);
            if (existingGate == null)
            {
                throw new InvalidOperationException($"质量门禁不存在: {GateId}");
            }
            
            _qualityGateService.UpdateQualityGate(GateId, gate =>
            {
                if (!string.IsNullOrEmpty(Name))
                    gate.Name = Name;
                
                if (!string.IsNullOrEmpty(Type) && Enum.TryParse<QualityGateType>(Type, true, out var gateType))
                    gate.Type = gateType;
                
                if (Threshold.HasValue)
                    gate.Threshold = Threshold.Value;
                
                if (!string.IsNullOrEmpty(Operator) && Enum.TryParse<ComparisonOperator>(Operator, true, out var comparisonOperator))
                    gate.Operator = comparisonOperator;
                
                if (!string.IsNullOrEmpty(Severity) && Enum.TryParse<RiskLevel>(Severity, true, out var riskLevel))
                    gate.Severity = riskLevel;
                
                if (!string.IsNullOrEmpty(Description))
                    gate.Description = Description;
                
                if (!string.IsNullOrEmpty(FailureMessage))
                    gate.MessageTemplate = FailureMessage;
                
                if (!string.IsNullOrEmpty(SuccessMessage))
                    gate.SuccessMessage = SuccessMessage;
            });
            
            _outputHelper.WriteLine($"✅ 质量门禁 '{GateId}' 更新成功", ConsoleColor.Green);
        }

        /// <summary>
        /// 删除质量门禁
        /// </summary>
        private async Task RemoveQualityGate()
        {
            if (string.IsNullOrEmpty(GateId))
            {
                throw new InvalidOperationException("删除质量门禁需要指定门禁ID");
            }
            
            var existingGate = _qualityGateService.GetQualityGateDefinition(GateId);
            if (existingGate == null)
            {
                throw new InvalidOperationException($"质量门禁不存在: {GateId}");
            }
            
            _qualityGateService.RemoveQualityGate(GateId);
            
            _outputHelper.WriteLine($"✅ 质量门禁 '{GateId}' 删除成功", ConsoleColor.Green);
        }

        /// <summary>
        /// 启用/禁用质量门禁
        /// </summary>
        private async Task EnableQualityGate(bool enable)
        {
            if (string.IsNullOrEmpty(GateId))
            {
                throw new InvalidOperationException($"{(enable ? "启用" : "禁用")}质量门禁需要指定门禁ID");
            }
            
            var existingGate = _qualityGateService.GetQualityGateDefinition(GateId);
            if (existingGate == null)
            {
                throw new InvalidOperationException($"质量门禁不存在: {GateId}");
            }
            
            _qualityGateService.SetQualityGateEnabled(GateId, enable);
            
            _outputHelper.WriteLine($"✅ 质量门禁 '{GateId}' {(enable ? "启用" : "禁用")}成功", ConsoleColor.Green);
        }

        /// <summary>
        /// 验证配置
        /// </summary>
        private async Task ValidateConfiguration()
        {
            _outputHelper.WriteLine("🔍 验证质量门禁配置...", ConsoleColor.Cyan);
            
            var errors = _qualityGateService.ValidateQualityGateConfiguration();
            
            if (errors.Count == 0)
            {
                _outputHelper.WriteLine("✅ 配置验证通过", ConsoleColor.Green);
            }
            else
            {
                _outputHelper.WriteLine("❌ 配置验证失败:", ConsoleColor.Red);
                foreach (var error in errors)
                {
                    _outputHelper.WriteLine($"  - {error}", ConsoleColor.Red);
                }
            }
        }

        /// <summary>
        /// 导出配置
        /// </summary>
        private async Task ExportConfiguration()
        {
            _outputHelper.WriteLine("📄 导出质量门禁配置...", ConsoleColor.Cyan);
            
            var config = _qualityGateService.ExportQualityGateConfiguration();
            
            if (!string.IsNullOrEmpty(OutputFile))
            {
                await File.WriteAllTextAsync(OutputFile, config);
                _outputHelper.WriteLine($"✅ 配置已导出到: {OutputFile}", ConsoleColor.Green);
            }
            else
            {
                _outputHelper.WriteLine(config, ConsoleColor.White);
            }
        }

        /// <summary>
        /// 获取严重程度颜色
        /// </summary>
        private ConsoleColor GetSeverityColor(RiskLevel severity)
        {
            return severity switch
            {
                RiskLevel.Low => ConsoleColor.Green,
                RiskLevel.Medium => ConsoleColor.Yellow,
                RiskLevel.High => ConsoleColor.Red,
                RiskLevel.Critical => ConsoleColor.DarkRed,
                _ => ConsoleColor.White
            };
        }
    }
}