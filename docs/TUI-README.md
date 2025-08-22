# BannerlordModEditor TUI 项目

## 项目概述

这是一个针对《骑马与砍杀2》（Bannerlord）的模组编辑器TUI（终端用户界面）工具，使用C#和.NET 9开发。项目采用现代化的架构，主要功能是实现Excel表格与XML配置文件之间的双向转换。

## 核心功能

### 1. Excel/XML 双向转换
- **Excel → XML**: 将Excel表格数据转换为XML格式，支持骑砍2的XML配置结构
- **XML → Excel**: 将XML配置文件转换为Excel表格，方便用户编辑
- **格式自动识别**: 自动识别源文件格式并设置合适的转换方向
- **格式验证**: 内置格式验证机制，确保转换结果的正确性

### 2. 数据库式映射支持
- **一对多关系**: 支持单个父记录对应多个子记录的映射
- **多对一关系**: 支持多个子记录对应单个父记录的映射  
- **多对多关系**: 支持复杂的多对多关系映射
- **嵌套元素处理**: 支持XML嵌套元素的扁平化处理

### 3. 格式识别与错误处理
- **自动格式检测**: 自动识别Excel和XML文件格式
- **错误报告**: 详细的错误信息和警告提示
- **格式不匹配处理**: 当源文件格式与期望不符时报错退出
- **备份机制**: 转换前自动创建备份文件

### 4. 用户界面特性
- **终端界面**: 基于Terminal.Gui的现代化终端用户界面
- **实时反馈**: 转换进度和状态的实时显示
- **文件浏览**: 内置文件选择对话框
- **选项配置**: 丰富的转换选项配置

## 技术架构

### 解决方案结构
```
BannerlordModEditor.sln
├── BannerlordModEditor.Common/           # 核心业务逻辑层
├── BannerlordModEditor.TUI/               # TUI表现层
├── BannerlordModEditor.Common.Tests/     # Common层测试
├── BannerlordModEditor.TUI.Tests/         # TUI层测试
├── BannerlordModEditor.UI/               # 原有UI层（Avalonia）
└── BannerlordModEditor.UI.Tests/         # UI层测试
```

### 核心技术栈
- **.NET 9.0**: 最新.NET平台
- **Terminal.Gui 1.14.0**: 跨平台终端UI框架
- **ClosedXML 0.102.3**: Excel文件处理库
- **xUnit 2.5.3**: 单元测试框架
- **Moq 4.20.70**: 模拟框架

### 关键设计模式

#### 1. 服务层模式
- `IFormatConversionService`: 定义转换服务接口
- `FormatConversionService`: 提供具体转换实现
- 依赖注入就绪的设计

#### 2. MVVM模式  
- `MainViewModel`: 主界面视图模型
- `ViewModelBase`: 通用视图模型基类
- `Command`: 自定义命令实现

#### 3. 选项模式
- `ConversionOptions`: 转换选项配置
- `FileFormatInfo`: 文件格式信息
- `ValidationResult`: 验证结果

## 使用说明

### 基本操作流程
1. **选择源文件**: 使用"浏览"按钮选择Excel或XML文件
2. **分析文件**: 点击"分析"按钮自动识别文件格式
3. **设置目标**: 系统自动设置目标文件路径，可手动修改
4. **配置选项**: 点击"选项"按钮配置转换参数
5. **执行转换**: 点击"转换"按钮开始转换过程
6. **查看结果**: 转换完成后查看结果和日志

### 转换选项说明
- **包含架构验证**: 启用XML格式验证
- **保留格式**: 保留原始格式信息
- **创建备份**: 转换前创建备份文件
- **扁平化嵌套元素**: 将嵌套XML元素转换为扁平结构
- **自定义元素名称**: 设置根元素和行元素名称

### 支持的文件格式
- **Excel文件**: .xlsx, .xls
- **XML文件**: .xml
- **编码**: UTF-8

## 开发指南

### 构建项目
```bash
# 构建整个解决方案
dotnet build BannerlordModEditor.sln

# 构建TUI项目
dotnet build BannerlordModEditor.TUI/BannerlordModEditor.TUI.csproj

# 运行测试
dotnet test BannerlordModEditor.TUI.Tests/BannerlordModEditor.TUI.Tests.csproj
```

### 运行应用程序
```bash
# 运行TUI应用程序
dotnet run --project BannerlordModEditor.TUI
```

### 添加新的转换功能
1. 在`FormatConversionService`中添加新的转换逻辑
2. 更新`FileFormatInfo`以支持新格式
3. 添加相应的单元测试
4. 更新用户界面（如需要）

### 扩展验证规则
1. 在`FormatConversionService`的验证方法中添加新规则
2. 更新`ValidationErrorType`枚举
3. 添加测试用例验证新规则

## 测试策略

### 单元测试覆盖
- **FormatConversionServiceTests**: 转换服务核心功能测试
- **MainViewModelTests**: 视图模型逻辑测试
- **集成测试**: 端到端转换流程测试

### 测试数据管理
- 使用临时目录进行测试
- 自动清理测试文件
- 支持大型文件测试

## 性能考虑

### 大文件处理
- 异步处理机制
- 流式处理支持
- 内存使用优化

### 转换性能
- 批量处理优化
- 并行处理支持
- 缓存机制

## 已知限制

1. **CSV支持**: 当前版本不支持CSV文件格式
2. **文件大小**: 超大文件可能需要特殊处理
3. **复杂XML**: 某些高度复杂的XML结构可能需要手动调整
4. **终端依赖**: 需要支持终端操作系统的环境

## 未来计划

### 短期目标
- [ ] 添加CSV文件支持
- [ ] 改进错误处理机制
- [ ] 增加更多验证规则
- [ ] 优化大文件处理性能

### 长期目标
- [ ] 支持更多文件格式
- [ ] 添加批量转换功能
- [ ] 实现命令行界面
- [ ] 添加配置文件支持

## 贡献指南

1. Fork项目仓库
2. 创建功能分支
3. 提交代码更改
4. 创建Pull Request
5. 等待代码审查

## 许可证

本项目采用MIT许可证。详见LICENSE文件。

## 联系方式

如有问题或建议，请通过GitHub Issues联系。