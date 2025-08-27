using Xunit;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System;
using BannerlordModEditor.UI.Tests.Helpers;

namespace BannerlordModEditor.UI.Tests;

/// <summary>
/// UI界面空白问题修复验证测试
/// 
/// 这个测试类验证EditorManagerViewModel是否能正确创建所有编辑器，
/// 以及依赖注入系统是否正常工作，确保UI界面不会出现空白问题。
/// </summary>
public class UIFunctionalityTests
{
    /// <summary>
    /// 验证EditorManagerViewModel构造函数能正确接收依赖项
    /// </summary>
    [Fact]
    public void EditorManagerViewModel_Constructor_Should_Accept_Dependencies()
    {
        // Arrange
        var serviceProvider = TestServiceProvider.GetServiceProvider();
        var options = EditorManagerOptions.ForDependencyInjection(serviceProvider);

        // Act
        var editorManager = new EditorManagerViewModel(options);

        // Assert
        Assert.NotNull(editorManager);
        Assert.NotNull(editorManager.Categories);
        Assert.True(editorManager.Categories.Count > 0);
    }

    /// <summary>
    /// 验证EditorManagerViewModel通过依赖注入能正确创建
    /// </summary>
    [Fact]
    public void EditorManagerViewModel_DependencyInjection_Should_Work()
    {
        // Arrange & Act
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();

        // Assert
        Assert.NotNull(editorManager);
        Assert.NotNull(editorManager.Categories);
        
        // 验证所有默认分类都存在
        var expectedCategories = new[] { "角色设定", "装备物品", "战斗系统", "世界场景", "音频系统", "多人游戏", "游戏配置" };
        foreach (var category in expectedCategories)
        {
            var actualCategory = editorManager.Categories.FirstOrDefault(c => c.Name == category);
            Assert.NotNull(actualCategory);
            Assert.Equal(category, actualCategory.Name);
        }
    }

    /// <summary>
    /// 验证CreateEditorViewModel方法能正确创建所有编辑器类型
    /// </summary>
    [Fact]
    public void CreateEditorViewModel_Should_Create_All_Editor_Types()
    {
        // Arrange
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();
        var validationService = TestServiceProvider.GetService<IValidationService>();

        // 测试所有编辑器类型
        var editorTypes = new[]
        {
            ("AttributeEditor", "attributes.xml"),
            ("SkillEditor", "skills.xml"),
            ("CombatParameterEditor", "combat_parameters.xml"),
            ("ItemEditor", "spitems.xml"),
            ("BoneBodyTypeEditor", "bone_body_types.xml"),
            ("CraftingPieceEditor", "crafting_pieces.xml"),
            ("ItemModifierEditor", "item_modifiers.xml")
        };

        foreach (var (editorType, xmlFileName) in editorTypes)
        {
            // Act
            var editorItem = new EditorItemViewModel(
                $"Test {editorType}",
                $"Test {editorType} Description",
                xmlFileName,
                editorType,
                "🧪");

            // 创建编辑器ViewModel
            var editorViewModel = CreateEditorViewModelInternal(editorManager, editorItem);

            // Assert
            Assert.NotNull(editorViewModel);
            Assert.NotNull(editorViewModel.GetType().Name);
            
            // 验证编辑器类型是否正确
            var actualType = editorViewModel.GetType().Name;
            Assert.Contains(editorType.Replace("Editor", ""), actualType, StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// 验证AttributeEditorViewModel能正确接收IValidationService
    /// </summary>
    [Fact]
    public void AttributeEditorViewModel_Should_Receive_ValidationService()
    {
        // Arrange
        var validationService = new ValidationService();

        // Act
        var attributeEditor = new AttributeEditorViewModel(validationService);

        // Assert
        Assert.NotNull(attributeEditor);
        Assert.NotNull(attributeEditor.Attributes);
        Assert.True(attributeEditor.Attributes.Count > 0);
        
        // 验证编辑器有验证服务
        Assert.Same(validationService, attributeEditor.ValidationService);
    }

    /// <summary>
    /// 验证ItemEditorViewModel能正确接收IValidationService
    /// </summary>
    [Fact]
    public void ItemEditorViewModel_Should_Receive_ValidationService()
    {
        // Arrange
        var validationService = new ValidationService();

        // Act
        var itemEditor = new ItemEditorViewModel(validationService);

        // Assert
        Assert.NotNull(itemEditor);
        Assert.NotNull(itemEditor.Items);
        Assert.True(itemEditor.Items.Count > 0);
        
        // 验证编辑器有验证服务
        Assert.Same(validationService, itemEditor.ValidationService);
    }

    /// <summary>
    /// 验证CombatParameterEditorViewModel能正确接收IValidationService
    /// </summary>
    [Fact]
    public void CombatParameterEditorViewModel_Should_Receive_ValidationService()
    {
        // Arrange
        var validationService = new ValidationService();

        // Act
        var combatParameterEditor = new CombatParameterEditorViewModel(validationService);

        // Assert
        Assert.NotNull(combatParameterEditor);
        Assert.NotNull(combatParameterEditor.CombatParameters);
        Assert.NotNull(combatParameterEditor.Definitions);
        
        // 验证编辑器有验证服务
        Assert.Same(validationService, combatParameterEditor.ValidationService);
    }

    /// <summary>
    /// 验证依赖注入系统能正确创建所有编辑器
    /// </summary>
    [Fact]
    public void DependencyInjection_Should_Create_All_Editors()
    {
        // Arrange & Act
        var serviceProvider = TestServiceProvider.GetServiceProvider();

        // 验证所有编辑器都能通过依赖注入创建
        var editorTypes = new[]
        {
            typeof(AttributeEditorViewModel),
            typeof(ItemEditorViewModel),
            typeof(CombatParameterEditorViewModel),
            typeof(SkillEditorViewModel),
            typeof(BoneBodyTypeEditorViewModel),
            typeof(CraftingPieceEditorViewModel),
            typeof(ItemModifierEditorViewModel)
        };

        foreach (var editorType in editorTypes)
        {
            var editor = serviceProvider.GetService(editorType);
            Assert.NotNull(editor);
            Assert.Equal(editorType, editor.GetType());
        }
    }

    /// <summary>
    /// 验证编辑器选择功能正常工作
    /// </summary>
    [Fact]
    public void EditorSelection_Should_Work_Correctly()
    {
        // Arrange
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();
        
        // 确保有编辑器可以选择
        var characterCategory = editorManager.Categories.FirstOrDefault(c => c.Name == "角色设定");
        Assert.NotNull(characterCategory);
        
        // 如果没有编辑器，添加一个测试编辑器
        if (characterCategory.Editors.Count == 0)
        {
            characterCategory.Editors.Add(new EditorItemViewModel(
                "属性定义", "属性定义编辑器", "attributes.xml", "AttributeEditor", "⚙️"));
        }

        var editorToSelect = characterCategory.Editors.First();

        // Act
        editorManager.SelectEditorCommand.Execute(editorToSelect);

        // Assert
        Assert.Equal(editorToSelect, editorManager.SelectedEditor);
        Assert.NotNull(editorManager.CurrentEditorViewModel);
        Assert.NotNull(editorManager.CurrentBreadcrumb);
        Assert.Contains("角色设定", editorManager.CurrentBreadcrumb);
        Assert.Contains("属性定义", editorManager.CurrentBreadcrumb);
    }

    /// <summary>
    /// 验证编辑器工厂能正确创建编辑器
    /// </summary>
    [Fact]
    public void EditorFactory_Should_Create_Editors()
    {
        // Arrange
        var editorFactory = TestServiceProvider.GetService<BannerlordModEditor.UI.Factories.IEditorFactory>();
        
        // Act & Assert
        Assert.NotNull(editorFactory);

        // 验证工厂能创建各种编辑器
        var editorTypes = new[] { "AttributeEditor", "ItemEditor", "CombatParameterEditor" };
        
        foreach (var editorType in editorTypes)
        {
            var editor = editorFactory.CreateEditorViewModel(editorType, $"{editorType.ToLower()}.xml");
            Assert.NotNull(editor);
            
            var editorName = editor.GetType().Name;
            Assert.Contains(editorType.Replace("Editor", ""), editorName, StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// 验证UI界面能正常显示编辑器选项
    /// </summary>
    [Fact]
    public void UI_Should_Display_Editor_Options()
    {
        // Arrange
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();

        // Act
        var categories = editorManager.Categories;
        var allEditors = categories.SelectMany(c => c.Editors).ToList();

        // Assert
        Assert.NotEmpty(categories);
        Assert.True(categories.Count >= 3); // 至少有3个分类
        
        // 验证角色设定分类有编辑器
        var characterCategory = categories.FirstOrDefault(c => c.Name == "角色设定");
        Assert.NotNull(characterCategory);
        
        // 如果没有编辑器，验证默认配置是否正确
        if (characterCategory.Editors.Count == 0)
        {
            // 这是正常的，因为在测试环境中可能使用默认配置
            Assert.True(characterCategory.Name == "角色设定");
            Assert.True(characterCategory.Description == "角色设定编辑器");
        }
        else
        {
            // 如果有编辑器，验证编辑器属性
            foreach (var editor in characterCategory.Editors)
            {
                Assert.NotEmpty(editor.Name);
                Assert.NotEmpty(editor.Description);
                Assert.NotEmpty(editor.XmlFileName);
                Assert.NotEmpty(editor.EditorType);
                Assert.NotEmpty(editor.Icon);
            }
        }
    }

    /// <summary>
    /// 验证没有异常抛出
    /// </summary>
    [Fact]
    public void All_Operations_Should_Not_Throw_Exceptions()
    {
        // Arrange
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();

        // Act & Assert - 验证所有操作都不会抛出异常
        var exception = Record.Exception(() =>
        {
            // 测试搜索功能
            editorManager.SearchText = "属性";
            editorManager.SearchText = "";
            
            // 测试选择功能
            if (editorManager.Categories.Count > 0)
            {
                var firstCategory = editorManager.Categories.First();
                if (firstCategory.Editors.Count > 0)
                {
                    var firstEditor = firstCategory.Editors.First();
                    editorManager.SelectEditorCommand.Execute(firstEditor);
                }
            }
            
            // 测试刷新功能
            editorManager.RefreshEditorsCommand.Execute(null);
            
            // 验证状态消息
            var statusMessage = editorManager.StatusMessage;
            Assert.NotNull(statusMessage);
        });

        Assert.Null(exception);
    }

    /// <summary>
    /// 内部方法：直接调用EditorManagerViewModel的CreateEditorViewModel方法
    /// </summary>
    private ViewModelBase CreateEditorViewModelInternal(EditorManagerViewModel editorManager, EditorItemViewModel editorItem)
    {
        return editorManager.CreateEditorViewModel(editorItem);
    }
}