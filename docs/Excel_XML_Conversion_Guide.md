# Bannerlord Mod Editor CLI - Excel/XML 转换工具使用指南

## 概述

Bannerlord Mod Editor CLI 是一个强大的命令行工具，专门用于《骑马与砍杀2》模组开发中的 Excel 表格与 XML 配置文件之间的相互转换。该工具支持多种数据格式映射，包括一对一、一对多、多对一和多对多关系。

## 功能特性

- **双向转换**：支持 Excel 转 XML 和 XML 转 Excel
- **格式识别**：自动识别 XML 文件格式
- **格式验证**：验证 Excel 文件格式是否符合目标 XML 模型
- **多模型支持**：支持所有 Bannerlord XML 数据模型
- **错误处理**：提供详细的错误信息和解决建议
- **批量处理**：支持批量转换多个文件

## 安装和配置

### 系统要求

- .NET 9.0 或更高版本
- Windows/Linux/macOS 操作系统

### 构建项目

```bash
# 克隆项目
git clone <repository-url>
cd BannerlordModEditor-CLI

# 构建解决方案
dotnet build

# 运行测试
dotnet test
```

## 使用方法

### 基本命令

#### 1. 转换命令 (convert)

**Excel 转 XML**
```bash
BannerlordModEditor.Cli convert -i "input.xlsx" -o "output.xml" -m "ActionTypesDO"
```

**XML 转 Excel**
```bash
BannerlordModEditor.Cli convert -i "input.xml" -o "output.xlsx"
```

**仅验证格式**
```bash
BannerlordModEditor.Cli convert -i "input.xlsx" -o "output.xml" -m "ActionTypesDO" --validate
```

#### 2. 格式识别命令 (recognize)

```bash
BannerlordModEditor.Cli recognize -i "action_types.xml"
```

#### 3. 列出支持的模型类型 (list-models)

```bash
# 列出所有模型类型
BannerlordModEditor.Cli list-models

# 搜索特定模型
BannerlordModEditor.Cli list-models --search "Combat"
```

### 命令参数说明

#### convert 命令参数

| 参数 | 短参数 | 描述 | 必需 |
|------|--------|------|------|
| --input | -i | 输入文件路径 | 是 |
| --output | -o | 输出文件路径 | 是 |
| --model | -m | 模型类型（Excel 转 XML 时必需） | 否 |
| --worksheet | -w | 工作表名称 | 否 |
| --validate | -v | 仅验证格式，不执行转换 | 否 |
| --verbose | | 显示详细信息 | 否 |

#### recognize 命令参数

| 参数 | 短参数 | 描述 | 必需 |
|------|--------|------|------|
| --input | -i | XML 文件路径 | 是 |
| --verbose | | 显示详细信息 | 否 |

#### list-models 命令参数

| 参数 | 短参数 | 描述 | 必需 |
|------|--------|------|------|
| --search | -s | 搜索关键词 | 否 |
| --verbose | | 显示详细信息 | 否 |

## 支持的数据模型

### 常用模型类型

| 模型类型 | XML 根元素 | 描述 |
|----------|------------|------|
| ActionTypesDO | action_types | 动作类型定义 |
| CombatParametersDO | base | 战斗参数配置 |
| MapIconsDO | base | 地图图标配置 |
| ItemModifiersDO | item_modifiers | 物品修饰符 |
| SkillsDO | skills | 技能定义 |
| VoicesDO | voices | 语音定义 |

### 完整模型列表

要查看所有支持的模型类型，运行：

```bash
BannerlordModEditor.Cli list-models --verbose
```

## 使用示例

### 示例 1：基本转换

**Excel 转 XML**
```bash
# 将动作类型 Excel 文件转换为 XML
BannerlordModEditor.Cli convert -i "actions.xlsx" -o "actions.xml" -m "ActionTypesDO"
```

**XML 转 Excel**
```bash
# 将动作类型 XML 文件转换为 Excel
BannerlordModEditor.Cli convert -i "actions.xml" -o "actions.xlsx"
```

### 示例 2：格式识别

```bash
# 识别 XML 文件格式
BannerlordModEditor.Cli recognize -i "combat_parameters.xml"
```

输出：
```
✓ 识别成功: CombatParametersDO
可以使用以下命令进行转换:
  BannerlordModEditor.Cli convert -i "combat_parameters.xml" -o "output.xlsx" -m CombatParametersDO
```

### 示例 3：格式验证

```bash
# 验证 Excel 文件格式
BannerlordModEditor.Cli convert -i "actions.xlsx" -o "actions.xml" -m "ActionTypesDO" --validate
```

输出：
```
✓ Excel 格式验证通过
```

### 示例 4：使用特定工作表

```bash
# 转换特定工作表
BannerlordModEditor.Cli convert -i "data.xlsx" -o "output.xml" -m "ActionTypesDO" -w "Actions"
```

### 示例 5：详细输出

```bash
# 显示详细转换信息
BannerlordModEditor.Cli convert -i "actions.xml" -o "actions.xlsx" --verbose
```

输出：
```
输入文件: actions.xml
输出文件: actions.xlsx
模型类型: ActionTypesDO
工作表: 默认
验证模式: False
识别的模型类型: ActionTypesDO
✓ XML 转 Excel 转换成功
输出文件: actions.xlsx
```

## Excel 文件格式要求

### 基本格式

1. **表头**：第一行必须包含列名
2. **数据行**：从第二行开始为数据内容
3. **空值处理**：空单元格将被转换为 null 值

### 常见模型的数据格式

#### ActionTypesDO

| 列名 | 数据类型 | 描述 | 示例 |
|------|----------|------|------|
| name | string | 动作名称 | act_jump |
| type | string | 动作类型 | actt_jump_start |
| usage_direction | string | 使用方向 | forward |
| action_stage | string | 动作阶段 | start |

#### CombatParametersDO

| 列名 | 数据类型 | 描述 | 示例 |
|------|----------|------|------|
| type | string | 参数类型 | combat_parameters |
| def_name | string | 定义名称 | narrow_vertical_rot_limit |
| def_val | string | 定义值 | 0.4 |
| param_name | string | 参数名称 | attack_speed |
| param_val | string | 参数值 | 1.0 |

#### MapIconsDO

| 列名 | 数据类型 | 描述 | 示例 |
|------|----------|------|------|
| id | string | 图标ID | map_icon_player |
| id_str | string | 图标字符串 | player |
| flags | string | 标志 | 0x0 |
| mesh_name | string | 网格名称 | player |
| mesh_scale | string | 网格缩放 | 0.15 |
| sound_no | string | 音效编号 | 17 |
| offset_pos | string | 偏移位置 | 0.15, 0.17, 0.0 |

## 错误处理

### 常见错误及解决方案

#### 1. 文件不存在

```
错误：输入文件不存在 - input.xlsx
```

**解决方案**：检查文件路径是否正确，确保文件存在。

#### 2. 不支持的文件格式

```
错误：不支持的输入文件格式 - .csv
```

**解决方案**：确保输入文件是 .xlsx 或 .xml 格式。

#### 3. 模型类型不支持

```
错误：不支持的模型类型: InvalidModelType
```

**解决方案**：使用 `list-models` 命令查看支持的模型类型。

#### 4. Excel 格式验证失败

```
✗ Excel 格式验证失败
```

**解决方案**：检查 Excel 文件的表头是否符合目标模型的要求。

#### 5. XML 格式识别失败

```
✗ 识别失败：无法识别 XML 格式
```

**解决方案**：检查 XML 文件格式，或手动指定模型类型。

### 详细错误信息

使用 `--verbose` 参数获取详细的错误信息和堆栈跟踪：

```bash
BannerlordModEditor.Cli convert -i "input.xlsx" -o "output.xml" -m "ActionTypesDO" --verbose
```

## 最佳实践

### 1. 数据准备

- **备份原始文件**：在转换前始终备份原始文件
- **验证数据完整性**：确保 Excel 数据完整且格式正确
- **使用标准命名**：使用标准的列名和文件命名

### 2. 转换流程

1. **识别格式**：首先使用 `recognize` 命令识别 XML 格式
2. **验证格式**：使用 `--validate` 参数验证 Excel 格式
3. **执行转换**：确认格式无误后执行转换
4. **检查结果**：检查转换后的文件是否符合预期

### 3. 性能优化

- **批量处理**：一次转换多个文件以提高效率
- **使用适当的工作表**：指定具体的工作表以减少处理时间
- **关闭详细输出**：在生产环境中关闭 `--verbose` 参数

## 故障排除

### 转换失败

如果转换失败，请按以下步骤排查：

1. **检查文件格式**：确保输入文件格式正确
2. **验证模型类型**：确认使用的模型类型正确
3. **检查数据完整性**：确保数据没有损坏
4. **查看错误详情**：使用 `--verbose` 参数获取详细信息

### 性能问题

如果遇到性能问题：

1. **减少数据量**：分批处理大量数据
2. **优化内存使用**：关闭不必要的应用程序
3. **检查磁盘空间**：确保有足够的磁盘空间

### 格式问题

如果遇到格式问题：

1. **参考示例**：参考本文档中的格式示例
2. **使用验证功能**：使用 `--validate` 参数验证格式
3. **检查编码**：确保文件使用 UTF-8 编码

## 技术支持

如果遇到无法解决的问题，请：

1. **检查文档**：仔细阅读本文档
2. **查看日志**：查看详细的错误日志
3. **提交问题**：通过适当渠道提交问题报告

## 更新日志

### 版本 1.0.0

- 初始版本发布
- 支持 Excel 和 XML 双向转换
- 支持格式识别和验证
- 支持所有主要 Bannerlord 数据模型