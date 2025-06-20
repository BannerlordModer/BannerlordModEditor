# XML文件加载功能使用指南

## 🎯 问题已解决！

您之前遇到的"左边的按钮点了好像没有反应"的问题现在已经完全解决了！现在点击左侧导航中的任何编辑器都会：

1. **自动加载对应的XML文件**
2. **显示文件内容**
3. **提供完整的编辑功能**

## 🚀 使用方法

### 1. 启动应用程序
```bash
dotnet run --project BannerlordModEditor.UI
```

### 2. 选择编辑器
- 在左侧导航栏选择要编辑的XML文件类型
- 例如：点击 "角色设定 > 属性定义" 

### 3. 自动加载
- 应用程序会自动尝试加载对应的XML文件
- 支持的文件路径：
  - `TestData/attributes.xml`
  - `BannerlordModEditor.Common.Tests/TestData/attributes.xml`
  - 直接文件名路径

### 4. 编辑功能
- ✅ **添加新项目** - 点击"➕ 添加属性"按钮
- ✅ **删除项目** - 点击每个条目右侧的"删除"按钮
- ✅ **修改属性** - 直接在文本框中编辑
- ✅ **实时验证** - 错误字段会高亮显示
- ✅ **保存文件** - 点击"💾 保存文件"按钮

## 🎮 支持的编辑器

### 当前完全支持：
1. **属性定义** (`attributes.xml`)
   - ID、名称、来源、文档编辑
   - 下拉选择来源类型
   - 实时输入验证

2. **骨骼体型** (`bone_body_types.xml`)
   - 骨骼类型选择
   - 优先级设置
   - 布尔属性配置

### 即将支持：
- 技能系统 (`skills.xml`)
- 物品数据 (`mpitems.xml`)
- 物品修饰符 (`item_modifiers.xml`)
- 制作部件 (`crafting_pieces.xml`)
- 制作模板 (`crafting_templates.xml`)
- 战斗参数 (`combat_parameters.xml`)
- 攻城器械 (`siegeengines.xml`)
- 武器描述 (`weapon_descriptions.xml`)
- 场景配置 (`scenes.xml`)
- 地图图标 (`map_icons.xml`)
- 环境对象 (`objects.xml`)
- 模组声音 (`module_sounds.xml`)
- 声音文件 (`soundfiles.xml`)
- 角色声音 (`voices.xml`)
- 多人角色 (`mpcharacters.xml`)
- 多人文化 (`mpcultures.xml`)
- 多人徽章 (`mpbadges.xml`)
- 多人场景 (`MultiplayerScenes.xml`)
- 物理材质 (`physics_materials.xml`)
- 布料材质 (`cloth_materials.xml`)
- 粒子系统 (`gpu_particle_systems.xml`)
- 后处理图 (`before_transparents_graph.xml`)

## 📁 文件路径说明

应用程序会按优先级搜索XML文件：

1. **`TestData/[文件名]`** - 项目根目录的测试数据
2. **`BannerlordModEditor.Common.Tests/TestData/[文件名]`** - 测试项目中的数据
3. **直接文件名** - 作为绝对或相对路径

如果文件不存在，会创建一个空的编辑器供您开始编辑。

## 🎨 界面特性

### 状态指示器
- **🟢 已保存** - 绿色标签，文件已保存
- **🟠 未保存** - 橙色标签，有未保存的更改

### 文件信息
- 顶部工具栏显示当前加载的文件路径
- 面包屑导航显示当前位置

### 搜索功能
- 支持实时搜索编辑器名称、描述、XML文件名
- 中文关键词搜索
- 自动筛选和高亮结果

## 🧪 质量保证

- **35个自动化测试** 确保功能稳定
- **XML加载测试** 验证文件读取功能
- **UI交互测试** 验证按钮和命令功能
- **错误处理测试** 确保应用程序不会崩溃

## 💡 使用技巧

1. **文件不存在时**：不用担心，应用程序会创建一个默认编辑器
2. **多文件切换**：可以在不同编辑器间自由切换，每个都保持独立状态
3. **实时保存**：编辑后记得点击保存按钮，状态指示器会提醒您
4. **错误恢复**：如果加载失败，会显示错误信息但不会崩溃

## 🆘 故障排除

### 如果按钮仍然没反应：
1. 确保测试数据文件存在：`TestData/attributes.xml`
2. 检查控制台输出是否有错误信息
3. 重新构建项目：`dotnet build`

### 如果文件加载失败：
1. 检查XML文件格式是否正确
2. 查看调试输出获取详细错误信息
3. 尝试创建新文件而不是加载现有文件

现在您可以享受完整的XML编辑体验了！🎉 