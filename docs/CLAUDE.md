- 文档以及文档名称应当使用中文编写
- 单次计划中需要的specs、requirements、plan、tech等应当归类放在同一个文件夹内
- 计划相关应当放在project文件夹内
- 每一层README中应该描述清楚本层每个文档的功能以及每个文件夹的功能

## 📁 文档路径规则

### 文档存放路径
- **项目计划文档**: `docs/development/project/` (requirements.md, user-stories.md, acceptance-criteria.md, implementation-strategy.md)
- **技术规格文档**: `docs/development/technical/` (架构设计、API规格、实现指南、修复报告)
- **使用指南文档**: `docs/guides/` (测试指南、最佳实践、故障排除)
- **技术参考文档**: `docs/reference/` (架构分析、技术栈、技术分析)
- **归档文档**: `docs/archive/` (old-analysis/ 存放过期分析，old-reports/ 存放历史报告)

### 文档分类原则
- 按功能分类：项目计划、技术规格、使用指南、技术参考、归档文档
- 按生命周期分类：活跃文档在对应目录，过期文档移至archive
- 按受众分类：开发者文档、用户文档、维护者文档

## 📝 文档格式要求

### Markdown格式
- 使用标准Markdown语法
- 标题层级清晰：`#`、`##`、`###`
- 代码块使用语言标识：```csharp、```xml
- 表格格式规范，对齐整齐
- 链接使用相对路径

### 文档结构
每个文档应包含：
1. 清晰的标题和描述
2. 目录内容列表
3. 使用指南（针对不同用户）
4. 维护说明
5. 相关文档链接
6. 更新日期和维护者信息

### README文件规范
- 每个目录必须有README.md
- 描述目录用途和范围
- 列出所有文件及其功能
- 提供使用指南

### 命名规范
- 文件名使用中文，清晰描述内容
- 避免特殊字符，使用连字符或下划线
- 保持命名一致性
