# Bannerlord Mod Editor CLI - 性能测试报告

**测试时间**: 2025年 08月 22日 星期五 07:36:38 UTC
**测试环境**: Linux VSCodeRemote 6.8.12-2-pve #1 SMP PREEMPT_DYNAMIC PMX 6.8.12-2 (2024-09-05T10:03Z) x86_64 GNU/Linux

## 性能测试结果

| 测试名称 | 执行时间(秒) | 内存使用(MB) | 文件大小 |
|----------|-------------|-------------|----------|
| list-models | [0;34m[INFO][0m 开始测试: list-models命令 |  |  |
| help | [0;34m[INFO][0m 开始测试: help命令 |  |  |
| version | [0;34m[INFO][0m 开始测试: version命令 |  |  |
| recognize-action_types.xml | [0;34m[INFO][0m 开始测试: 识别 action_types.xml |  |  |
| recognize-combat_parameters.xml | [0;34m[INFO][0m 开始测试: 识别 combat_parameters.xml |  |  |
| recognize-map_icons.xml | [0;34m[INFO][0m 开始测试: 识别 map_icons.xml |  |  |
| recognize-flora_kinds.xml | [0;34m[INFO][0m 开始测试: 识别 flora_kinds.xml |  |  |
| convert-action_types.xml | [0;34m[INFO][0m 开始测试: 转换 action_types.xml |  |  |
| convert-combat_parameters.xml | [0;34m[INFO][0m 开始测试: 转换 combat_parameters.xml |  |  |
| convert-map_icons.xml | [0;34m[INFO][0m 开始测试: 转换 map_icons.xml |  |  |
| large-file-conversion | [0;34m[INFO][0m 开始测试: 转换大型文件 |  |  |
| concurrent-3-tasks | 2.073661962 | 0 | 0 |

## 性能分析

### 基本命令性能
- list-models命令应该快速响应
- help命令应该快速显示
- version命令应该快速显示

### XML处理性能
- XML识别应该在合理时间内完成
- XML到Excel转换应该在可接受时间内完成
- 大型文件处理应该有良好的性能

### 并发性能
- 并发处理多个文件应该有良好的性能表现

## 性能建议

1. **优化建议**:
   - 考虑使用并行处理提高大型文件处理速度
   - 优化内存使用以处理更大的文件
   - 改进错误处理以减少性能影响

2. **监控建议**:
   - 监控内存使用情况
   - 监控CPU使用率
   - 监控磁盘I/O性能

## 环境信息

- **操作系统**: Linux
- **处理器**: x86_64
- **内存**: 
- **磁盘空间**: 40G

---
*此报告由性能测试脚本自动生成*
