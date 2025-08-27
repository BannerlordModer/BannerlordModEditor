# UI.Tests 测试数据同步指南

## 概述

UI.Tests 项目需要特定的 XML 测试数据文件来运行测试。这些文件原本位于 `BannerlordModEditor.Common.Tests/TestData/` 目录中。为了确保 GitHub Actions CI 环境中的测试能够正常运行，我们需要将这些测试数据文件同步到 `BannerlordModEditor.UI.Tests/TestData/` 目录中。

## 必需的测试数据文件

以下文件是 UI 测试运行所必需的：

- `attributes.xml` - 属性定义数据
- `bone_body_types.xml` - 骨骼体型数据  
- `skills.xml` - 技能数据
- `module_sounds.xml` - 模组声音数据
- `crafting_pieces.xml` - 制作部件数据
- `item_modifiers.xml` - 物品修饰符数据

## 同步脚本

我们提供了三个同步脚本，分别适用于不同的操作系统：

### 1. PowerShell 脚本 (推荐)

**文件**: `Sync-TestData.ps1`

**用法**:
```powershell
# 基本同步
.\Sync-TestData.ps1

# 强制覆盖已存在的文件
.\Sync-TestData.ps1 -Force
```

**特性**:
- 支持详细的输出和错误处理
- 自动验证同步结果
- 显示文件处理摘要

### 2. Windows 批处理脚本

**文件**: `Sync-TestData.bat`

**用法**:
```batch
# 基本同步
Sync-TestData.bat

# 强制覆盖已存在的文件
Sync-TestData.bat -force
```

### 3. Shell 脚本 (Linux/macOS)

**文件**: `Sync-TestData.sh`

**用法**:
```bash
# 基本同步
./Sync-TestData.sh

# 强制覆盖已存在的文件
./Sync-TestData.sh -f

# 显示帮助信息
./Sync-TestData.sh -h
```

## 项目配置

项目文件 `BannerlordModEditor.UI.Tests.csproj` 已经配置了以下内容：

```xml
<ItemGroup>
  <None Update="TestData\**\*">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
</ItemGroup>
```

这个配置确保：
- 所有 `TestData/` 目录下的文件都会在构建时复制到输出目录
- 使用 `PreserveNewest` 模式，只有当源文件更新时才会覆盖目标文件
- 支持嵌套目录结构

## 使用场景

### 1. 开发环境

在开发环境中，当 Common.Tests 的测试数据更新后，运行同步脚本：

```powershell
.\Sync-TestData.ps1 -Force
```

### 2. CI/CD 环境

在 GitHub Actions 中，测试数据应该：
1. 作为构建过程的一部分自动同步
2. 包含在测试运行器的文件系统中
3. 确保测试可以访问这些文件

### 3. 新增测试文件

当需要新增测试数据文件时：
1. 将文件添加到 `Common.Tests/TestData/` 目录
2. 更新同步脚本中的文件列表
3. 运行同步脚本将文件复制到 UI.Tests

## 故障排除

### 问题1: 文件未找到错误

**现象**: 测试运行时报告文件不存在

**解决方案**:
```powershell
# 检查文件是否存在
ls TestData\

# 重新同步测试数据
.\Sync-TestData.ps1 -Force
```

### 问题2: 权限问题

**现象**: 无法访问或复制文件

**解决方案**:
```bash
# Linux/macOS: 检查文件权限
ls -la TestData/

# Windows: 检查文件权限
icacls TestData\
```

### 问题3: 文件内容不一致

**现象**: 测试失败因为文件内容不匹配

**解决方案**:
1. 确保源文件是最新的
2. 运行强制同步
3. 验证文件内容是否正确

## 验证同步结果

同步完成后，可以通过以下方式验证：

```powershell
# 检查目标目录
ls TestData\

# 验证特定文件
Test-Path TestData\attributes.xml

# 检查文件大小
ls -la TestData\
```

## 最佳实践

1. **定期同步**: 当 Common.Tests 的测试数据更新后，及时同步到 UI.Tests
2. **版本控制**: 测试数据文件应该包含在版本控制中
3. **CI/CD 集成**: 在构建脚本中包含同步步骤
4. **文档更新**: 当新增测试数据文件时，更新相关文档

## 相关文件

- `BannerlordModEditor.UI.Tests.csproj` - 项目配置文件
- `Sync-TestData.ps1` - PowerShell 同步脚本
- `Sync-TestData.bat` - Windows 批处理脚本
- `Sync-TestData.sh` - Shell 同步脚本
- `TestData/` - 测试数据目录