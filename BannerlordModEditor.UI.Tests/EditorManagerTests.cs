using Xunit;
using BannerlordModEditor.UI.ViewModels;
using System.Linq;
using BannerlordModEditor.UI.Tests.Helpers;

namespace BannerlordModEditor.UI.Tests;

public class EditorManagerTests
{
    [Fact]
    public void EditorManager_Should_Initialize_Categories()
    {
        // Arrange & Act
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();

        // Assert
        Assert.NotEmpty(editorManager.Categories);
        Assert.True(editorManager.Categories.Count >= 7); // 我们定义了7个主要分类
    }

    [Fact]
    public void EditorManager_Should_Have_Character_Category_With_Editors()
    {
        // Arrange & Act
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();
        var characterCategory = editorManager.Categories.FirstOrDefault(c => c.Name == "角色设定");

        // Assert
        Assert.NotNull(characterCategory);
        Assert.NotEmpty(characterCategory.Editors);
        Assert.Contains(characterCategory.Editors, e => e.Name == "属性定义");
        Assert.Contains(characterCategory.Editors, e => e.Name == "骨骼体型");
    }

    [Fact]
    public void EditorManager_Should_Have_Equipment_Category_With_Editors()
    {
        // Arrange & Act
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();
        var equipmentCategory = editorManager.Categories.FirstOrDefault(c => c.Name == "装备物品");

        // Assert
        Assert.NotNull(equipmentCategory);
        Assert.NotEmpty(equipmentCategory.Editors);
        Assert.Contains(equipmentCategory.Editors, e => e.Name == "物品数据");
        Assert.Contains(equipmentCategory.Editors, e => e.Name == "制作部件");
    }

    [Fact]
    public void SelectEditor_Should_Update_Current_Editor()
    {
        // Arrange
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();
        var characterCategory = editorManager.Categories.FirstOrDefault(c => c.Name == "角色设定");
        var attributeEditor = characterCategory?.Editors.FirstOrDefault(e => e.Name == "属性定义");

        // Act
        editorManager.SelectEditorCommand.Execute(attributeEditor);

        // Assert
        Assert.Equal(attributeEditor, editorManager.SelectedEditor);
        Assert.NotNull(editorManager.CurrentEditorViewModel);
        Assert.Contains("角色设定", editorManager.CurrentBreadcrumb);
        Assert.Contains("属性定义", editorManager.CurrentBreadcrumb);
    }

    [Fact]
    public void SearchEditors_Should_Filter_Results()
    {
        // Arrange
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();

        // Act - 搜索"属性"
        editorManager.SearchText = "属性";

        // Assert
        var visibleEditors = editorManager.Categories
            .SelectMany(c => c.Editors)
            .Where(e => e.IsAvailable);
        
        Assert.Contains(visibleEditors, e => e.Name == "属性定义");
        
        // 确保不相关的编辑器被隐藏
        var hiddenEditors = editorManager.Categories
            .SelectMany(c => c.Editors)
            .Where(e => !e.IsAvailable);
        
        Assert.Contains(hiddenEditors, e => e.Name == "模组声音");
    }

    [Fact]
    public void SearchEditors_Empty_Should_Show_All()
    {
        // Arrange
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();
        
        // 先搜索一些东西，然后清空
        editorManager.SearchText = "属性";
        editorManager.SearchText = "";

        // Assert
        var allEditors = editorManager.Categories
            .SelectMany(c => c.Editors);
        
        Assert.True(allEditors.All(e => e.IsAvailable));
    }

    [Fact]
    public void CreateEditorViewModel_Should_Return_Correct_Types()
    {
        // Arrange
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();
        var attributeEditor = editorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "AttributeEditor");
        
        var boneBodyTypeEditor = editorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "BoneBodyTypeEditor");

        // Act
        editorManager.SelectEditorCommand.Execute(attributeEditor);
        var attributeVM = editorManager.CurrentEditorViewModel;
        
        editorManager.SelectEditorCommand.Execute(boneBodyTypeEditor);
        var boneBodyTypeVM = editorManager.CurrentEditorViewModel;

        // Assert
        Assert.IsType<ViewModels.Editors.AttributeEditorViewModel>(attributeVM);
        Assert.IsType<ViewModels.Editors.BoneBodyTypeEditorViewModel>(boneBodyTypeVM);
    }

    [Fact]
    public void EditorCategories_Should_Have_Appropriate_Icons()
    {
        // Arrange & Act
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();

        // Assert
        foreach (var category in editorManager.Categories)
        {
            Assert.NotEmpty(category.Icon);
            Assert.NotEmpty(category.Description);
            
            foreach (var editor in category.Editors)
            {
                Assert.NotEmpty(editor.Icon);
                Assert.NotEmpty(editor.XmlFileName);
                Assert.NotEmpty(editor.Description);
            }
        }
    }
} 