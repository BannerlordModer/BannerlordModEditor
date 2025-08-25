# 示例：CombatParameterEditor实现

## 概述

这是一个针对`CombatParametersDO`模型的编辑器实现示例，展示了如何为复杂的XML模型创建GUI编辑器。

## 模型分析

### CombatParametersDO结构
```csharp
[XmlRoot("base")]
public class CombatParametersDO
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlElement("definitions")]
    public DefinitionsDO Definitions { get; set; } = new DefinitionsDO();
    
    [XmlArray("combat_parameters")]
    [XmlArrayItem("combat_parameter")]
    public List<BaseCombatParameterDO> CombatParametersList { get; set; } = new List<BaseCombatParameterDO>();
}
```

### 复杂度分析
- **嵌套结构**：包含definitions和combat_parameters两个主要部分
- **大量属性**：BaseCombatParameterDO有30+个属性
- **条件序列化**：需要处理空元素和可选属性

## 编辑器实现

### 1. ViewModel实现

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BannerlordModEditor.Common.Models.DO;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;
using BannerlordModEditor.Common.Loaders;

namespace BannerlordModEditor.UI.ViewModels.Editors;

public partial class CombatParameterEditorViewModel : BaseEditorViewModel<CombatParametersDO, BaseCombatParameterDO>
{
    [ObservableProperty]
    private string type = string.Empty;

    [ObservableProperty]
    private ObservableCollection<DefDataViewModel> definitions = new();

    [ObservableProperty]
    private bool hasDefinitions = false;

    [ObservableProperty]
    private bool hasEmptyCombatParameters = false;

    public CombatParameterEditorViewModel() : base("combat_parameters.xml", "战斗参数编辑器")
    {
        // 添加默认定义
        Definitions.Add(new DefDataViewModel { Name = "default_def", Value = "1.0" });
    }

    protected override CombatParametersDO LoadDataFromFile(string path)
    {
        var loader = new GenericXmlLoader<CombatParametersDO>();
        return loader.Load(path) ?? new CombatParametersDO();
    }

    protected override void SaveDataToFile(CombatParametersDO data, string path)
    {
        var loader = new GenericXmlLoader<CombatParametersDO>();
        loader.Save(data, path);
    }

    protected override IEnumerable<BaseCombatParameterDO> GetItemsFromData(CombatParametersDO data)
    {
        Type = data.Type ?? string.Empty;
        HasDefinitions = data.HasDefinitions;
        HasEmptyCombatParameters = data.HasEmptyCombatParameters;
        
        // 加载definitions
        Definitions.Clear();
        foreach (var def in data.Definitions.Defs)
        {
            Definitions.Add(new DefDataViewModel
            {
                Name = def.Name ?? string.Empty,
                Value = def.Value ?? string.Empty
            });
        }
        
        return data.CombatParametersList;
    }

    protected override CombatParametersDO CreateDataFromItems(ObservableCollection<BaseCombatParameterDO> items)
    {
        var data = new CombatParametersDO
        {
            Type = string.IsNullOrWhiteSpace(Type) ? null : Type,
            HasDefinitions = HasDefinitions,
            HasEmptyCombatParameters = HasEmptyCombatParameters,
            Definitions = new DefinitionsDO
            {
                Defs = Definitions.Select(d => new DefDO
                {
                    Name = string.IsNullOrWhiteSpace(d.Name) ? null : d.Name,
                    Value = string.IsNullOrWhiteSpace(d.Value) ? null : d.Value
                }).ToList()
            },
            CombatParametersList = items.ToList()
        };
        
        return data;
    }

    protected override BaseCombatParameterDO CreateNewItem()
    {
        return new BaseCombatParameterDO
        {
            Id = $"new_parameter_{Items.Count + 1}",
            CollisionCheckStartingPercent = "0.1",
            CollisionDamageStartingPercent = "0.2",
            CollisionCheckEndingPercent = "0.8"
        };
    }

    protected override BaseCombatParameterDO CreateErrorItem(string errorMessage)
    {
        return new BaseCombatParameterDO
        {
            Id = "error",
            CollisionCheckStartingPercent = "0.0",
            CollisionDamageStartingPercent = "0.0",
            CollisionCheckEndingPercent = "0.0"
        };
    }

    protected override bool MatchesSearchFilter(BaseCombatParameterDO item, string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return true;

        return (item.Id?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false) ||
               (item.CollisionCheckStartingPercent?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false) ||
               (item.CollisionDamageStartingPercent?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false);
    }

    [RelayCommand]
    private void AddDefinition()
    {
        Definitions.Add(new DefDataViewModel 
        { 
            Name = $"new_def_{Definitions.Count + 1}", 
            Value = "1.0" 
        });
        HasDefinitions = true;
        HasUnsavedChanges = true;
    }

    [RelayCommand]
    private void RemoveDefinition(DefDataViewModel definition)
    {
        if (definition != null)
        {
            Definitions.Remove(definition);
            HasUnsavedChanges = true;
        }
    }

    partial void OnTypeChanged(string value)
    {
        HasUnsavedChanges = true;
    }

    partial void OnHasDefinitionsChanged(bool value)
    {
        HasUnsavedChanges = true;
    }

    partial void OnHasEmptyCombatParametersChanged(bool value)
    {
        HasUnsavedChanges = true;
    }
}

public partial class DefDataViewModel : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string value = string.Empty;

    public bool IsValid => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Value);
}
```

### 2. View实现 (CombatParameterEditorView.axaml)

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:vm="clr-namespace:BannerlordModEditor.UI.ViewModels.Editors"
             xmlns:controls="clr-namespace:BannerlordModEditor.UI.Controls"
             mc:Ignorable="d" d:DesignWidth="800" d:DesignHeight="600"
             x:Class="BannerlordModEditor.UI.Views.Editors.CombatParameterEditorView">

    <Design.DataContext>
        <vm:CombatParameterEditorViewModel/>
    </Design.DataContext>

    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

        <!-- 工具栏 -->
        <StackPanel Grid.Row="0" Spacing="10" Margin="10">
            <TextBlock Text="战斗参数编辑器" FontSize="18" FontWeight="Bold"/>
            
            <!-- 基本设置 -->
            <Expander Header="基本设置" IsExpanded="True">
                <StackPanel Spacing="10" Margin="10">
                    <TextBox Text="{Binding Type}" 
                             Watermark="类型 (可选)"
                             Width="300"/>
                    
                    <CheckBox IsChecked="{Binding HasDefinitions}"
                              Content="包含定义 (definitions)"/>
                    
                    <CheckBox IsChecked="{Binding HasEmptyCombatParameters}"
                              Content="包含空的战斗参数列表"/>
                </StackPanel>
            </Expander>

            <!-- 定义编辑器 -->
            <Expander Header="定义 (Definitions)" IsExpanded="{Binding HasDefinitions}">
                <Grid Margin="10">
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="*"/>
                    </Grid.RowDefinitions>
                    
                    <Button Grid.Row="0" Content="添加定义" 
                            Command="{Binding AddDefinition}"
                            HorizontalAlignment="Left"
                            Margin="0,0,0,10"/>
                    
                    <DataGrid Grid.Row="1" ItemsSource="{Binding Definitions}"
                              AutoGenerateColumns="False"
                              CanUserAddRows="False"
                              CanUserDeleteRows="False">
                        <DataGrid.Columns>
                            <DataGridTextColumn Header="名称" 
                                                Binding="{Binding Name}"
                                                Width="200"/>
                            <DataGridTextColumn Header="值" 
                                                Binding="{Binding Value}"
                                                Width="150"/>
                            <DataGridTemplateColumn Header="操作" Width="100">
                                <DataGridTemplateColumn.CellTemplate>
                                    <DataTemplate>
                                        <Button Content="删除"
                                                Command="{Binding DataContext.RemoveDefinition, RelativeSource={RelativeSource AncestorType=DataGrid}}"
                                                CommandParameter="{Binding}"/>
                                    </DataTemplate>
                                </DataGridTemplateColumn.CellTemplate>
                            </DataGridTemplateColumn>
                        </DataGrid.Columns>
                    </DataGrid>
                </Grid>
            </Expander>
        </StackPanel>

        <!-- 主要编辑区域 -->
        <TabControl Grid.Row="1">
            <TabItem Header="战斗参数列表">
                <Grid>
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="*"/>
                    </Grid.RowDefinitions>
                    
                    <!-- 搜索和过滤 -->
                    <StackPanel Grid.Row="0" Orientation="Horizontal" Spacing="10" Margin="10">
                        <TextBox Text="{Binding SearchFilter}" 
                                 Watermark="搜索参数..."
                                 Width="200"/>
                        <TextBlock Text="{Binding FilteredCount, StringFormat='显示: {0}'}"
                                   VerticalAlignment="Center"/>
                        <TextBlock Text="{Binding TotalCount, StringFormat='总计: {0}'}"
                                   VerticalAlignment="Center"/>
                    </StackPanel>
                    
                    <!-- 参数列表 -->
                    <DataGrid Grid.Row="1" ItemsSource="{Binding FilteredItems}"
                              AutoGenerateColumns="False"
                              CanUserAddRows="False"
                              CanUserDeleteRows="False">
                        <DataGrid.Columns>
                            <DataGridTextColumn Header="ID" 
                                                Binding="{Binding Id}"
                                                Width="150"/>
                            <DataGridTextColumn Header="碰撞检测开始%" 
                                                Binding="{Binding CollisionCheckStartingPercent}"
                                                Width="150"/>
                            <DataGridTextColumn Header="碰撞伤害开始%" 
                                                Binding="{Binding CollisionDamageStartingPercent}"
                                                Width="150"/>
                            <DataGridTextColumn Header="碰撞检测结束%" 
                                                Binding="{Binding CollisionCheckEndingPercent}"
                                                Width="150"/>
                            <DataGridTextColumn Header="碰撞半径" 
                                                Binding="{Binding CollisionRadius}"
                                                Width="120"/>
                            <DataGridTextColumn Header="武器偏移" 
                                                Binding="{Binding WeaponOffset}"
                                                Width="120"/>
                            <DataGridTemplateColumn Header="操作" Width="100">
                                <DataGridTemplateColumn.CellTemplate>
                                    <DataTemplate>
                                        <Button Content="删除"
                                                Command="{Binding DataContext.RemoveItem, RelativeSource={RelativeSource AncestorType=DataGrid}}"
                                                CommandParameter="{Binding}"/>
                                    </DataTemplate>
                                </DataGridTemplateColumn.CellTemplate>
                            </DataGridTemplateColumn>
                        </DataGrid.Columns>
                    </DataGrid>
                </Grid>
            </TabItem>
            
            <TabItem Header="详细编辑">
                <ScrollViewer>
                    <StackPanel Spacing="15" Margin="20">
                        <TextBlock Text="选择一个参数进行详细编辑" 
                                   FontSize="16"
                                   Foreground="Gray"
                                   HorizontalAlignment="Center"
                                   VerticalAlignment="Center"/>
                    </StackPanel>
                </ScrollViewer>
            </TabItem>
        </TabControl>

        <!-- 状态栏 -->
        <StackPanel Grid.Row="2" Orientation="Horizontal" Spacing="10" Margin="10">
            <Button Content="加载文件" Command="{Binding LoadFileAsync}"/>
            <Button Content="保存文件" Command="{Binding SaveFileAsync}"/>
            <Button Content="添加参数" Command="{Binding AddItem}"/>
            <TextBlock Text="{Binding StatusMessage}" 
                       VerticalAlignment="Center"/>
            <TextBlock Text="{Binding FilePath}" 
                       VerticalAlignment="Center"
                       Margin="20,0,0,0"/>
        </StackPanel>
    </Grid>
</UserControl>
```

### 3. 注册到EditorFactory

在`EditorFactory.cs`中添加：

```csharp
private void RegisterDefaultEditors()
{
    // 现有编辑器...
    
    // 添加战斗参数编辑器
    RegisterEditor<CombatParameterEditorViewModel, CombatParameterEditorView>("CombatParameterEditor");
}
```

### 4. 更新EditorManagerViewModel

在`EditorManagerViewModel.cs`的`InitializeCategories`方法中添加：

```csharp
var combatCategory = new EditorCategoryViewModel("战斗系统", "战斗参数、武器、攻城器械等", "⚔️");
combatCategory.Editors.Add(new EditorItemViewModel("战斗参数", "战斗系统基础参数", "combat_parameters.xml", "CombatParameterEditor", "🛡️"));
// 其他战斗系统编辑器...
```

## 关键实现要点

### 1. 复杂结构处理
- **嵌套对象**：使用单独的集合处理definitions
- **条件序列化**：通过标记属性控制空元素的序列化
- **大量属性**：提供分页或分类显示

### 2. 用户体验优化
- **分组显示**：使用Expander组织相关设置
- **搜索过滤**：支持按ID和属性值搜索
- **实时验证**：提供输入验证和错误提示

### 3. 性能考虑
- **异步加载**：大型XML文件的异步处理
- **虚拟化**：DataGrid的UI虚拟化
- **分批处理**：大量数据的分批加载

## 扩展建议

### 1. 详细编辑面板
- 为选中的参数提供详细的属性编辑面板
- 支持参数组的批量编辑
- 提供参数值的预设和模板

### 2. 可视化功能
- 碰撞参数的可视化预览
- 参数关系的图表显示
- 实时参数调整的效果预览

### 3. 高级功能
- 参数导入/导出
- 参数模板管理
- 参数版本控制

---

*示例代码展示了复杂XML模型编辑器的实现模式*
*可根据实际需求调整和扩展*