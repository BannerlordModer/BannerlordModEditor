# 📋 文档管理规范

本文档规定了BannerlordModEditor项目的文档管理规则，确保文档结构清晰、格式统一、易于维护。

## 🌐 语言规范

- **文档语言**: 所有文档必须使用中文编写
- **文档名称**: 使用中文命名，清晰描述文档内容
- **技术术语**: 保持技术术语的英文原名，必要时提供中文注释
- **代码注释**: 代码示例中的注释使用中文

## 📁 文档路径规则

### 根目录结构
```
docs/
├── README.md                    # 文档中心导航
├── CLAUDE.md                    # 本文档管理规范
├── PROJECT_FINAL_SUMMARY.md     # 项目最终总结
├── development/                 # 开发文档
├── guides/                      # 使用指南
├── reference/                   # 技术参考
└── archive/                     # 归档文档
```

### development/ 目录规则
```
development/
├── README.md                    # 开发文档总览
├── project/                     # 项目计划文档
│   ├── README.md
│   ├── requirements.md         # 需求规格
│   ├── user-stories.md         # 用户故事
│   ├── acceptance-criteria.md  # 验收标准
│   └── implementation-strategy.md # 实施策略
└── technical/                  # 技术规格文档
    ├── README.md
    ├── 架构设计文档.md
    ├── 技术规格文档.md
    ├── 实现指南文档.md
    └── 修复报告文档.md
```

### guides/ 目录规则
```
guides/
├── README.md                    # 使用指南总览
├── comprehensive-test-suite.md  # 综合测试套件
├── test-execution-plan-and-quality-assessment.md # 测试执行计划
└── 具体使用指南文档.md
```

### reference/ 目录规则
```
reference/
├── README.md                    # 技术参考总览
├── architecture.md             # 架构文档
├── tech-stack.md               # 技术栈
├── tech-analysis.md            # 技术分析
└── 其他技术参考文档.md
```

### archive/ 目录规则
```
archive/
├── README.md                    # 归档文档说明
├── old-analysis/               # 历史分析文档
│   ├── README.md
│   └── 各种分析报告.md
└── old-reports/                # 过期报告
    ├── README.md
    └── 各种历史报告.md
```

## 📝 文档分类规则

### 项目计划文档 (development/project/)
- **requirements.md**: 项目需求规格说明
- **user-stories.md**: 用户需求故事和场景
- **acceptance-criteria.md**: 项目验收标准
- **implementation-strategy.md**: 项目实施策略和计划

### 技术规格文档 (development/technical/)
- **架构设计文档**: 系统架构、组件设计等
- **API规格文档**: 接口规范、数据模型等
- **实现指南文档**: 具体实现步骤和示例
- **修复报告文档**: 问题修复的完整报告

### 使用指南文档 (guides/)
- **测试指南**: 测试策略、测试方法
- **最佳实践**: 开发建议和规范
- **故障排除**: 常见问题解决方案

### 技术参考文档 (reference/)
- **架构文档**: 深度技术架构分析
- **技术分析**: 技术方案分析
- **技术栈**: 使用的技术和工具

### 归档文档 (archive/)
- **历史分析**: 过期的技术分析文档
- **过期报告**: 完成的项目报告

## 📄 文档格式规范

### Markdown格式要求
- **标题层级**: 使用 `#`、`##`、`###` 等标记标题层级
- **代码块**: 使用 ```语言名称 包裹代码
- **表格**: 使用标准Markdown表格格式
- **链接**: 使用 `[描述](URL)` 格式
- **列表**: 使用 `-` 或 `1.` 格式

### 文档结构模板
```markdown
# 📋 文档标题

## 📋 目录内容

### 主要分类
- **[文档名称](文件名.md)** - 简短描述

## 🎯 使用指南

### 目标用户
1. 具体使用步骤
2. 注意事项

## 📝 维护说明

- 维护规则
- 更新要求

## 🔗 相关文档

- **[相关文档](路径)** - 关联说明

---

**维护**: 维护者  
**更新**: YYYY年MM月DD日
```

### README文件规范
每个目录必须包含README.md文件，内容要求：
- 描述本目录的用途和范围
- 列出目录中的所有文档及其功能
- 提供使用指南和维护说明
- 包含相关文档的链接

## 🔄 文档生命周期管理

### 新建文档
1. 根据文档类型选择正确的目录
2. 按照格式规范创建文档
3. 更新目录的README.md
4. 更新相关文档的链接

### 更新文档
1. 修改内容后更新文档末尾的日期
2. 检查并更新相关文档的引用
3. 确保文档格式符合规范

### 归档文档
1. 将过期的分析文档移至 `archive/old-analysis/`
2. 将完成的报告移至 `archive/old-reports/`
3. 更新原目录的README.md
4. 确保归档文档不会影响当前文档结构

## 📋 文档质量标准

### 内容质量
- **准确性**: 信息必须准确无误
- **完整性**: 内容应该完整覆盖主题
- **时效性**: 信息应该是最新的
- **可读性**: 语言应该清晰易懂

### 格式质量
- **一致性**: 格式应该保持一致
- **规范性**: 遵循Markdown规范
- **结构性**: 逻辑结构清晰
- **链接性**: 内部链接正确有效

### 维护质量
- **定期更新**: 定期检查和更新文档
- **版本控制**: 使用Git管理文档版本
- **备份**: 重要文档应该有备份
- **审查**: 定期审查文档质量

## 🚀 快速参考

| 文档类型 | 推荐路径 | 文件名示例 |
|----------|----------|------------|
| 项目需求 | `development/project/` | `requirements.md` |
| 用户故事 | `development/project/` | `user-stories.md` |
| 验收标准 | `development/project/` | `acceptance-criteria.md` |
| 架构设计 | `development/technical/` | `架构设计文档.md` |
| 技术规格 | `development/technical/` | `API规格文档.md` |
| 实现指南 | `development/technical/` | `XML适配实现指南.md` |
| 测试指南 | `guides/` | `comprehensive-test-suite.md` |
| 技术分析 | `reference/` | `tech-analysis.md` |
| 历史分析 | `archive/old-analysis/` | `历史分析报告.md` |
| 过期报告 | `archive/old-reports/` | `项目完成报告.md` |

## 📝 维护责任人

- **文档管理员**: 负责文档整体结构和规范
- **技术文档**: 由技术负责人维护
- **项目文档**: 由项目经理维护
- **用户指南**: 由开发团队共同维护

---

**最后更新**: 2025年8月21日  
**维护**: BannerlordModEditor开发团队
