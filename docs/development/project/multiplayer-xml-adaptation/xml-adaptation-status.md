# XML适配状态报告

## 检查时间
2025年 08月 22日 星期五 04:03:26 UTC

## 摘要信息

info: BannerlordModEditor.XmlAdaptationChecker.Core.XmlAdaptationChecker[0]
      开始XML适配状态检查
info: BannerlordModEditor.XmlAdaptationChecker.Core.XmlAdaptationChecker[0]
      XML适配状态检查完成
info: BannerlordModEditor.XmlAdaptationChecker.Core.XmlAdaptationChecker[0]
      总文件数: 102, 已适配: 0, 未适配: 102, 适配率: 0.0%
XML适配状态摘要 (2025-08-22 04:03:26)
==================================================
总文件数: 102
已适配: 0
未适配: 102
适配率: 0.0%

未适配文件按复杂度:
  Large: 3 个文件
  Complex: 33 个文件
  Medium: 21 个文件
  Simple: 45 个文件

## 详细分析
请运行以下命令查看详细的未适配文件列表：

```bash
cd scripts/XmlAdaptationChecker
dotnet run -- check
```

## 未适配文件统计

基于当前的TestData目录扫描结果：

- **总文件数**: 102个XML文件
- **已适配**: 0个文件
- **未适配**: 102个文件
- **适配率**: 0%

## 按复杂度分类

- **Large**: 3个文件（需要分块处理的大型文件）
- **Complex**: 33个文件（复杂结构的文件）
- **Medium**: 21个文件（中等复杂度的文件）
- **Simple**: 45个文件（简单结构的文件）

## 优先级建议

1. **高优先级**: Large文件（particle_systems_hardcoded_misc1.xml, flora_kinds.xml等）
2. **中优先级**: Complex文件（包含深度嵌套结构的文件）
3. **低优先级**: Medium和Simple文件

## 工具使用

### 运行检查
```bash
cd scripts/XmlAdaptationChecker
dotnet run -- check
```

### 查看摘要
```bash
cd scripts/XmlAdaptationChecker
dotnet run -- summary
```

### 初始化配置
```bash
cd scripts/XmlAdaptationChecker
dotnet run -- config init
```


