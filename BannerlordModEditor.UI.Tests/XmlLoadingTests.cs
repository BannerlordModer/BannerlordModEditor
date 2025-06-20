using Xunit;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using System.Linq;

namespace BannerlordModEditor.UI.Tests;

public class XmlLoadingTests
{
    [Fact]
    public void AttributeEditor_Should_Load_Xml_File()
    {
        // Arrange
        var editorManager = new EditorManagerViewModel();
        var attributeEditor = editorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "AttributeEditor");

        // Act
        editorManager.SelectEditorCommand.Execute(attributeEditor);
        var currentEditor = editorManager.CurrentEditorViewModel as AttributeEditorViewModel;

        // Assert
        Assert.NotNull(currentEditor);
        Assert.NotEmpty(currentEditor.Attributes);
        Assert.Contains("attributes.xml", currentEditor.FilePath);
    }

    [Fact]
    public void BoneBodyTypeEditor_Should_Load_Xml_File()
    {
        // Arrange
        var editorManager = new EditorManagerViewModel();
        var boneBodyTypeEditor = editorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "BoneBodyTypeEditor");

        // Act
        editorManager.SelectEditorCommand.Execute(boneBodyTypeEditor);
        var currentEditor = editorManager.CurrentEditorViewModel as BoneBodyTypeEditorViewModel;

        // Assert
        Assert.NotNull(currentEditor);
        Assert.NotEmpty(currentEditor.BoneBodyTypes);
        Assert.Contains("bone_body_types.xml", currentEditor.FilePath);
    }

    [Fact]
    public void AttributeEditor_Should_Handle_Missing_File_Gracefully()
    {
        // Arrange
        var attributeEditor = new AttributeEditorViewModel();

        // Act
        attributeEditor.LoadXmlFile("nonexistent.xml");

        // Assert - 应该创建一个默认的编辑器，不崩溃
        Assert.NotEmpty(attributeEditor.Attributes);
        Assert.Equal("nonexistent.xml", attributeEditor.FilePath);
        Assert.False(attributeEditor.HasUnsavedChanges);
    }

    [Fact]
    public void BoneBodyTypeEditor_Should_Handle_Missing_File_Gracefully()
    {
        // Arrange
        var boneBodyTypeEditor = new BoneBodyTypeEditorViewModel();

        // Act
        boneBodyTypeEditor.LoadXmlFile("nonexistent.xml");

        // Assert - 应该创建一个默认的编辑器，不崩溃
        Assert.NotEmpty(boneBodyTypeEditor.BoneBodyTypes);
        Assert.Equal("nonexistent.xml", boneBodyTypeEditor.FilePath);
        Assert.False(boneBodyTypeEditor.HasUnsavedChanges);
    }

    [Fact]
    public void EditorManager_Should_Update_Breadcrumb_When_Selecting_Editor()
    {
        // Arrange
        var editorManager = new EditorManagerViewModel();
        var attributeEditor = editorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.Name == "属性定义");

        // Act
        editorManager.SelectEditorCommand.Execute(attributeEditor);

        // Assert
        Assert.Contains("角色设定", editorManager.CurrentBreadcrumb);
        Assert.Contains("属性定义", editorManager.CurrentBreadcrumb);
    }

    [Fact]
    public void Multiple_Editor_Selections_Should_Work_Correctly()
    {
        // Arrange
        var editorManager = new EditorManagerViewModel();
        var attributeEditor = editorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "AttributeEditor");
        var boneBodyTypeEditor = editorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "BoneBodyTypeEditor");

        // Act & Assert - 选择属性编辑器
        editorManager.SelectEditorCommand.Execute(attributeEditor);
        Assert.IsType<AttributeEditorViewModel>(editorManager.CurrentEditorViewModel);

        // Act & Assert - 切换到骨骼体型编辑器
        editorManager.SelectEditorCommand.Execute(boneBodyTypeEditor);
        Assert.IsType<BoneBodyTypeEditorViewModel>(editorManager.CurrentEditorViewModel);
        
        // 确保面包屑更新了
        Assert.Contains("骨骼体型", editorManager.CurrentBreadcrumb);
    }
} 