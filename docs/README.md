# 📚 Bannerlord Mod Editor 文档中心

欢迎来到BannerlordModEditor项目的文档中心。这里包含了项目的所有技术文档、开发指南和参考资料。

## 📁 文档结构

### 🛠️ 开发文档 (`development/`)
- **项目计划** (`project/`) - 历史项目需求和计划文档
  - **[需求规格](development/project/requirements.md)** - 项目需求规格说明
  - **[用户故事](development/project/user-stories.md)** - 用户需求故事和场景
  - **[验收标准](development/project/acceptance-criteria.md)** - 项目验收标准
  - **[实施策略](development/project/implementation-strategy.md)** - 项目实施策略和计划
  - **[多人游戏XML适配](project/multiplayer-xml-adaptation/)** - 多人游戏XML适配项目
- **技术规格** (`technical/`) - 架构设计和技术规格文档
  - **[GitHub Actions Tmux测试配置](development/technical/GitHub Actions Tmux测试配置.md)** - Tmux集成测试配置
  - **[TUI集成测试技术文档](development/technical/TUI集成测试技术文档.md)** - TUI集成测试技术细节
  - **[GUI增强计划](development/technical/GUI增强计划.md)** - GUI界面增强计划
  - **[XML转换框架技术规格](development/technical/XML转换框架技术规格.md)** - 通用XML转换框架技术规格
  - **[XML转换框架架构设计](development/technical/XML转换框架架构设计.md)** - XML转换框架架构设计
  - **[基于TUI的XML转换框架实现指南](development/technical/基于TUI的XML转换框架实现指南.md)** - TUI集成实现指南
  - **[XML转换框架实现示例](development/technical/XML转换框架实现示例.md)** - 框架实现示例
  - **[XML转换框架测试报告](development/technical/XML转换框架测试报告.md)** - 框架测试报告

### 📖 使用指南 (`guides/`)
- **[TUI开发指南](guides/TUI开发指南.md)** - TUI应用程序开发指南
- **[TUI用户指南](guides/TUI用户指南.md)** - TUI应用程序使用指南
- **[UAT用户验收测试指南](guides/UAT用户验收测试指南.md)** - UAT测试框架使用指南
- **[综合测试套件指南](guides/综合测试套件指南.md)** - 完整测试套件使用指南
- **[XML转换框架测试指南](guides/XML转换框架测试指南.md)** - XML转换框架测试指南

### 🔧 技术参考 (`reference/`)
- **[系统架构](reference/architecture.md)** - 系统整体架构设计
- **[数据流设计](reference/data-flow.md)** - 系统数据流程和交互
- **[技术栈](reference/tech-stack.md)** - 项目使用的技术和工具
- **[技术分析](reference/tech-analysis.md)** - 项目技术分析
- **[XML适配技术分析](reference/XML_Adaptation_Technical_Analysis.md)** - XML适配技术深度分析
- **[API参考文档](reference/API参考文档.md)** - 应用程序API接口参考
- **[字符串型XML策略](reference/STRING_BASED_XML_STRATEGY.md)** - 字符串型XML处理策略

### 📦 归档文档 (`archive/`)
- **旧分析** (`old-analysis/`) - 历史技术分析文档
- **旧报告** (`old-reports/`) - 过期的项目报告

## 🎯 快速导航

### 新用户入门
1. 阅读 **[TUI用户指南](guides/TUI用户指南.md)** 了解应用程序使用方法
2. 查看 **[项目最终总结](PROJECT_FINAL_SUMMARY.md)** 了解项目整体情况
3. 浏览 **[系统架构](reference/architecture.md)** 了解技术架构
4. 了解 **[XML转换框架技术规格](development/technical/XML转换框架技术规格.md)** 了解XML转换功能

### 开发者指南
1. 阅读 **[TUI开发指南](guides/TUI开发指南.md)** 了解开发流程
2. 查看 **[开发文档](development/)** 了解项目计划和技术规格
3. 参考 **[使用指南](guides/)** 了解测试和最佳实践
4. 学习 **[XML转换框架实现指南](development/technical/基于TUI的XML转换框架实现指南.md)** 了解XML转换实现

### 测试和验证
1. 使用 **[UAT用户验收测试指南](guides/UAT用户验收测试指南.md)** 进行用户验收测试
2. 参考 **[综合测试套件指南](guides/综合测试套件指南.md)** 运行完整测试套件
3. 查看 **[GitHub Actions Tmux测试配置](development/technical/GitHub Actions Tmux测试配置.md)** 了解CI/CD配置
4. 参考 **[XML转换框架测试指南](guides/XML转换框架测试指南.md)** 进行XML转换框架测试

### 🔄 CI/CD 文档 (`ci-cd/`)
- **[GitHub Actions 恢复完成报告](ci-cd/github_actions_restoration_report.md)** - GitHub Actions配置恢复报告
- **[GitHub Actions CI/CD 分析报告](ci-cd/github-actions-cicd-analysis.md)** - CI/CD系统分析
- **[GitHub Actions 修复总结](ci-cd/github-actions-fix-summary.md)** - GitHub Actions问题修复总结
- **[CI 修复报告](ci-cd/ci-fix-report.md)** - CI系统修复报告

### 💻 CLI 文档 (`cli/`)
- **[CLI 开发进度报告](cli/CLI_DEVELOPMENT_PROGRESS_REPORT.md)** - 命令行界面开发进度
- **[CLI 项目总结](cli/CLI_PROJECT_SUMMARY.md)** - CLI项目完整总结
- **[Excel XML 转换指南](cli/Excel_XML_Conversion_Guide.md)** - Excel与XML转换使用指南

### 🏗️ 架构文档 (`architecture/`)
- **[DO/DTO 架构文档](architecture/do-dto/)** - DO/DTO设计模式相关文档
  - **[DO/DTO Mapper适配完成总结](architecture/do-dto/DO_DTO_Mapper适配完成总结.md)** - DO/DTO模式实施总结
  - **[DO/DTO Mapper适配进度报告](architecture/do-dto/DO_DTO_Mapper适配进度报告_第二阶段.md)** - 第二阶段适配进度
  - **[SoundFiles适配完成报告](architecture/do-dto/SoundFiles适配完成报告.md)** - SoundFiles模块适配报告

### 🧪 测试文档 (`testing/`)
- **[GUI 测试修复总结](testing/GUI_Test_Fix_Summary.md)** - GUI测试问题修复总结
- **[集成和UAT测试总结](testing/INTEGRATION_AND_UAT_TESTING_SUMMARY.md)** - 集成测试和用户验收测试总结
- **[UI 测试重构策略](testing/UI_Test_Refactoring_Strategy.md)** - UI测试重构策略文档
- **[UI 测试重构成功报告](testing/UI_Test_Refactoring_Success_Report.md)** - UI测试重构成功报告
- **[UI 测试策略](testing/UI_Test_Strategy.md)** - UI测试整体策略
- **[项目测试状态总结](testing/Project_Test_Status_Summary.md)** - 项目测试状态总结

### 📊 项目报告 (`reports/`)
- **[Git 合并冲突解决报告](reports/GIT_MERGE_CONFLICT_RESOLUTION_REPORT.md)** - Git合并冲突解决过程报告
- **[项目最终状态报告](reports/PROJECT_FINAL_STATUS_REPORT.md)** - 项目最终状态报告
- **[项目最终总结](reports/PROJECT_FINAL_SUMMARY.md)** - 项目完整总结报告
- **[项目状态报告](reports/PROJECT_STATUS.md)** - 项目状态报告
- **[XML 适配状态报告](reports/xml_adapter_status.md)** - XML适配系统状态报告
- **[GUI 增强计划](reports/gui-enhancement-plan.md)** - GUI界面增强计划

## 🚀 快速开始

### 新开发者
1. 阅读 [项目最终总结](reports/PROJECT_FINAL_SUMMARY.md) 了解项目状态
2. 查看 `development/project/` 目录中的项目需求和计划
3. 参考 `development/technical/` 目录中的技术规格和架构设计
4. 使用 `guides/` 目录中的测试和开发指南
5. 学习 **[XML转换框架技术规格](development/technical/XML转换框架技术规格.md)** 了解XML转换功能

### 贡献者
1. 熟悉 `development/technical/` 目录中的技术规格
2. 参考 `guides/` 目录中的最佳实践
3. 查看 `reference/` 目录中的技术细节
4. 了解 `architecture/do-dto/` 目录中的架构设计模式
5. 了解 **[XML转换框架实现指南](development/technical/基于TUI的XML转换框架实现指南.md)**

### 维护者
1. 定期清理 `archive/` 目录中的过期文档
2. 更新 `development/project/` 目录中的项目计划
3. 维护 `development/technical/` 目录中的技术规格
4. 维护文档的整体结构和一致性<<<<<<< feature/cli-development
5. 更新 `reports/` 目录中的项目状态
6. 更新XML转换框架相关文档

## 📝 文档维护

### 添加新文档
- 项目计划文档放在 `development/project/` 目录
- 技术规格文档放在 `development/technical/` 目录
- 用户指南文档放在 `guides/` 目录
- 技术参考文档放在 `reference/` 目录
- CI/CD相关文档放在 `ci-cd/` 目录
- CLI相关文档放在 `cli/` 目录
- 架构设计文档放在 `architecture/` 目录
- 测试相关文档放在 `testing/` 目录
- 项目报告放在 `reports/` 目录
- 过期文档移到 `archive/` 目录

### 文档规范
- 使用中文编写文档
- 文档名称应清晰描述内容
- 每个目录应包含README说明
- 定期清理过期和重复文档

### 质量标准
- 文档内容应准确无误
- 代码示例应可运行
- 技术说明应详细清晰
- 保持文档结构的一致性

## 🔍 快速导航

| 需求 | 推荐文档 |
|------|----------|
| 了解项目概况 | [项目最终总结](reports/PROJECT_FINAL_SUMMARY.md) |
| 项目需求和计划 | `development/project/` 目录 |
| 技术架构设计 | `development/technical/` 目录 |
| DO/DTO架构模式 | `architecture/do-dto/` 目录 |
| CLI开发相关 | `cli/` 目录 |
| 测试相关 | `testing/` 目录 |
| CI/CD配置 | `ci-cd/` 目录 |
| 开发指南 | `guides/` 目录 |
| 技术细节参考 | `reference/` 目录 |
| 项目报告 | `reports/` 目录 |
| 历史文档 | `archive/` 目录 |

## 📞 联系方式

如有文档相关问题，请通过以下方式联系：
- **GitHub Issues**: 报告文档问题或建议
- **Pull Requests**: 贡献文档改进
- **邮件**: [项目维护者](mailto:project@example.com)

---

**最后更新**: 2025年8月25日  
**维护**: BannerlordModEditor开发团队