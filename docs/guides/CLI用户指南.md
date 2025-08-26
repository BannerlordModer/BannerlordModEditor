# Bannerlord Mod Editor CLI - 用户指南

## 概述

Bannerlord Mod Editor CLI 是一个现代化的命令行工具，专门用于《骑马与砍杀2：霸主》（Mount & Blade II: Bannerlord）模组开发者的XML配置文件处理。该工具基于CliFx框架，提供了强大的Excel和XML格式转换功能，支持35+种Bannerlord数据模型。

## 主要功能

- **Excel转XML**：将Excel文件转换为Bannerlord XML格式
- **XML转Excel**：将XML文件转换为Excel格式便于编辑
- **格式识别**：自动识别XML文件类型和数据模型
- **模型验证**：验证Excel文件格式是否符合指定的数据模型
- **批量处理**：支持命令行批量操作
- **详细日志**：提供详细的操作日志和错误信息

## 系统要求

- **.NET 9.0** 或更高版本
- **支持的操作系统**：Windows、Linux、macOS
- **内存**：推荐4GB以上（用于处理大型XML文件）

## 安装和运行

### 从源码编译

```bash
# 克隆项目
git clone <repository-url>
cd BannerlordModEditor-CLI

# 还原依赖
dotnet restore

# 构建项目
dotnet build

# 运行CLI工具
dotnet run --project BannerlordModEditor.Cli -- --help
```

### 运行测试

```bash
# 运行所有测试
dotnet test

# 运行CLI特定测试
dotnet test BannerlordModEditor.Cli.Tests
```

## 命令参考

### 基本语法

```bash
dotnet run --project BannerlordModEditor.Cli -- [命令] [选项]
```

### 全局选项

| 选项 | 描述 |
|------|------|
| `--help` | 显示帮助信息 |
| `--version` | 显示版本信息 |
| `--verbose` | 启用详细输出 |

### 命令列表

#### 1. list-models - 列出支持的数据模型

列出所有支持的Bannerlord数据模型类型。

```bash
# 列出所有模型
dotnet run --project BannerlordModEditor.Cli -- list-models

# 搜索特定模型
dotnet run --project BannerlordModEditor.Cli -- list-models --search "action"

# 显示详细信息
dotnet run --project BannerlordModEditor.Cli -- list-models --detailed
```

**选项**:
- `--search, -s` : 搜索模型名称（可选）
- `--detailed, -d` : 显示详细信息（可选）

#### 2. recognize - 识别XML格式

识别XML文件的格式类型和对应的数据模型。

```bash
# 识别XML文件
dotnet run --project BannerlordModEditor.Cli -- recognize -i "path/to/file.xml"

# 识别多个文件
dotnet run --project BannerlordModEditor.Cli -- recognize -i "file1.xml" -i "file2.xml"

# 批量识别目录中的XML文件
dotnet run --project BannerlordModEditor.Cli -- recognize --directory "path/to/xml/files"
```

**选项**:
- `--input, -i` : 输入XML文件路径（必需，可多个）
- `--directory, -dir` : 输入目录路径（可选）
- `--recursive, -r` : 递归搜索子目录（可选）

#### 3. convert - 转换文件

执行Excel和XML文件之间的转换。

```bash
# Excel转XML
dotnet run --project BannerlordModEditor.Cli -- convert -i "data.xlsx" -o "data.xml" -m "ActionTypesDO"

# XML转Excel
dotnet run --project BannerlordModEditor.Cli -- convert -i "data.xml" -o "data.xlsx"

# 仅验证格式
dotnet run --project BannerlordModEditor.Cli -- convert -i "data.xlsx" -o "data.xml" -m "ActionTypesDO" --validate

# 指定工作表
dotnet run --project BannerlordModEditor.Cli -- convert -i "data.xlsx" -o "data.xml" -m "ActionTypesDO" --worksheet "Actions"
```

**选项**:
- `--input, -i` : 输入文件路径（必需）
- `--output, -o` : 输出文件路径（必需）
- `--model, -m` : 数据模型类型（Excel转XML时必需）
- `--worksheet, -w` : Excel工作表名称（可选）
- `--validate, -v` : 仅验证格式不转换（可选）
- `--overwrite` : 覆盖已存在的输出文件（可选）
- `--backup` : 创建备份文件（可选）

## 使用示例

### 示例1：基本转换

将Excel文件转换为Bannerlord动作类型XML：

```bash
dotnet run --project BannerlordModEditor.Cli -- convert -i "actions.xlsx" -o "actions.xml" -m "ActionTypesDO"
```

### 示例2：批量识别XML文件

识别目录中的所有XML文件类型：

```bash
dotnet run --project BannerlordModEditor.Cli -- recognize --directory "ModuleData" --recursive
```

### 示例3：格式验证

验证Excel文件是否符合指定模型：

```bash
dotnet run --project BannerlordModEditor.Cli -- convert -i "combat_parameters.xlsx" -o "temp.xml" -m "CombatParametersDO" --validate
```

### 示例4：XML转Excel

将XML文件转换为Excel便于编辑：

```bash
dotnet run --project BannerlordModEditor.Cli -- convert -i "items.xml" -o "items.xlsx"
```

## 支持的数据模型

CLI工具支持35+种Bannerlord数据模型，主要包括：

### 游戏核心机制
- **ActionTypesDO** - 动作类型配置
- **CombatParametersDO** - 战斗参数配置
- **ActionSetsDO** - 动作集配置
- **AttributesDO** - 角色属性配置
- **SkillsDO** - 技能配置

### 物品系统
- **ItemModifiersDO** - 物品修饰符
- **ItemHolstersDO** - 物品携带位置
- **CraftingPiecesDO** - 制作部件
- **CraftingTemplatesDO** - 制作模板

### 多人游戏
- **MpItemsDO** - 多人物品
- **MpCraftingPiecesDO** - 多人制作部件
- **MpCosmeticsDO** - 多人装饰品
- **MpCulturesDO** - 多人文化

### 音视频系统
- **SoundFilesDO** - 音效文件
- **ModuleSoundsDO** - 模组音效
- **ParticleSystemsDO** - 粒子系统

### 地图和环境
- **MapIconsDO** - 地图图标
- **FloraKindsDO** - 植被类型
- **TerrainMaterialsDO** - 地形材质

使用 `list-models` 命令查看完整的支持列表。

## 文件格式要求

### Excel文件格式

- **第一行**：必须包含列标题
- **数据行**：从第二行开始
- **列名**：使用与XML元素对应的名称
- **数据类型**：支持文本、数字、日期等基本类型

**示例Excel格式**：
```
name        | type        | usage_direction | action_stage
------------|-------------|-----------------|--------------
attack_1    | thrust      | forward         | ready
attack_2    | swing       | horizontal     | ready
```

### XML文件格式

- **编码**：UTF-8编码
- **结构**：有效的XML文档
- **命名空间**：支持标准XML命名空间
- **根元素**：根据数据模型类型确定

**示例XML格式**：
```xml
<?xml version="1.0" encoding="utf-8"?>
<action_types>
  <action>
    <name>attack_1</name>
    <type>thrust</type>
    <usage_direction>forward</usage_direction>
    <action_stage>ready</action_stage>
  </action>
</action_types>
```

## 错误处理

### 常见错误及解决方案

#### 1. 文件不存在错误
```
错误：文件不存在: path/to/file.xlsx
解决方案：检查文件路径是否正确
```

#### 2. 模型类型不支持
```
错误：不支持的模型类型: InvalidModelType
解决方案：使用 list-models 命令查看支持的模型类型
```

#### 3. Excel格式错误
```
错误：Excel文件格式不正确
解决方案：确保第一行包含列标题，数据格式正确
```

#### 4. XML格式错误
```
错误：XML文件格式不正确
解决方案：检查XML语法，确保文件编码为UTF-8
```

#### 5. 权限错误
```
错误：无法访问文件: 权限不足
解决方案：检查文件权限，确保有读写权限
```

### 详细日志

使用 `--verbose` 选项获取详细的操作日志：

```bash
dotnet run --project BannerlordModEditor.Cli -- convert -i "data.xlsx" -o "data.xml" -m "ActionTypesDO" --verbose
```

## 高级用法

### 批量处理

虽然CLI工具主要设计为单文件处理，但可以结合shell脚本进行批量处理：

```bash
# Linux/macOS批量处理
for file in *.xlsx; do
    dotnet run --project BannerlordModEditor.Cli -- convert -i "$file" -o "${file%.xlsx}.xml" -m "ActionTypesDO"
done

# Windows批量处理
for %f in (*.xlsx) do (
    dotnet run --project BannerlordModEditor.Cli -- convert -i "%f" -o "%~nf.xml" -m "ActionTypesDO"
)
```

### 自动化脚本

可以将CLI工具集成到自动化构建脚本中：

```bash
#!/bin/bash
# 自动化XML处理脚本

set -e

# 定义模型映射
declare -A MODEL_MAP=(
    ["actions"]="ActionTypesDO"
    ["combat_parameters"]="CombatParametersDO"
    ["items"]="ItemsDO"
)

# 处理所有Excel文件
for file in ModuleData/*.xlsx; do
    basename=$(basename "$file" .xlsx)
    model=${MODEL_MAP[$basename]}
    
    if [ -n "$model" ]; then
        echo "处理 $file -> $basename.xml (模型: $model)"
        dotnet run --project BannerlordModEditor.Cli -- convert \
            -i "$file" \
            -o "ModuleData/$basename.xml" \
            -m "$model" \
            --verbose
    else
        echo "警告: 未找到 $basename 对应的模型类型"
    fi
done

echo "批量处理完成"
```

## 性能优化

### 大文件处理

- **内存使用**：工具支持大型XML文件的流式处理
- **分片处理**：对于特别大的文件，考虑分片处理
- **异步操作**：内部使用异步操作提高性能

### 批量操作优化

- **并行处理**：可以同时运行多个CLI实例
- **缓存利用**：重复操作时会利用缓存机制
- **进度显示**：使用 `--verbose` 查看处理进度

## 故障排除

### 调试模式

启用详细输出以获取调试信息：

```bash
dotnet run --project BannerlordModEditor.Cli -- convert -i "data.xlsx" -o "data.xml" -m "ActionTypesDO" --verbose
```

### 常见问题

#### 1. .NET运行时错误
**解决方案**：确保安装了.NET 9.0 SDK

#### 2. 文件编码问题
**解决方案**：确保文件使用UTF-8编码

#### 3. 内存不足
**解决方案**：增加系统内存或分批处理大文件

#### 4. 依赖包问题
**解决方案**：运行 `dotnet restore` 重新安装依赖

### 日志文件

工具会在当前目录生成日志文件，包含：
- 操作日志
- 错误信息
- 性能统计

## 开发信息

### 项目结构

```
BannerlordModEditor.Cli/
├── Commands/                     # CLI命令实现
│   ├── ListModelsCommand.cs      # 列出模型命令
│   ├── RecognizeCommand.cs       # 识别格式命令
│   └── ConvertCommand.cs         # 转换文件命令
├── Services/                     # CLI服务
│   ├── ExcelXmlConverterService.cs # Excel/XML转换服务
│   └── ModelValidationService.cs # 模型验证服务
└── BannerlordModEditor.Cli.csproj # 项目文件
```

### 技术栈

- **.NET 9.0**：最新的.NET平台
- **CliFx**：现代化的命令行框架
- **ClosedXML**：Excel文件处理
- **System.Xml**：XML文件处理
- **xUnit**：单元测试框架

### 扩展开发

项目采用模块化设计，可以轻松扩展：

1. **添加新的命令**：在Commands目录中创建新的命令类
2. **支持新的数据模型**：在Common层添加对应的模型类
3. **自定义转换逻辑**：扩展ExcelXmlConverterService
4. **添加验证规则**：扩展ModelValidationService

## 版本历史

### v1.0.0 (当前版本)

- 初始版本发布
- 支持35+种Bannerlord数据模型
- Excel/XML双向转换
- 格式识别和验证
- 完整的命令行界面

## 贡献指南

欢迎贡献代码和建议！请遵循以下步骤：

1. Fork项目
2. 创建功能分支
3. 提交更改并测试
4. 推送到分支
5. 创建Pull Request

## 许可证

本项目采用MIT许可证。详情请参阅LICENSE文件。

## 联系方式

如有问题或建议，请通过以下方式联系：

- **GitHub Issues**：报告bug和功能请求
- **Pull Requests**：代码贡献和改进
- **讨论区**：技术讨论和经验分享

---

**注意**：本工具专为《骑马与砍杀2：霸主》模组开发者设计，请确保在使用前备份重要的配置文件。

**最后更新**: 2025年8月26日  
**项目状态**: 生产就绪  
**维护团队**: BannerlordModEditor开发团队