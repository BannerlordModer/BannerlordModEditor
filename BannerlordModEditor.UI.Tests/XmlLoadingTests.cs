using Xunit;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.UI.Tests.Helpers;

namespace BannerlordModEditor.UI.Tests;

public class XmlLoadingTests
{
    [Fact]
    public async Task AttributeEditor_Should_Load_Xml_File()
    {
        // Arrange
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();
        var attributeEditor = editorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "AttributeEditor");

        // Act
        editorManager.SelectEditorCommand.Execute(attributeEditor);
        var currentEditor = editorManager.CurrentEditorViewModel as AttributeEditorViewModel;
        
        // 等待异步XML加载完成
        await Task.Delay(100); // 等待异步操作

        // Assert
        Assert.NotNull(currentEditor);
        Assert.NotEmpty(currentEditor.Attributes);
        // 检查是否有数据被加载（表示XML文件被正确处理）
        Assert.True(currentEditor.Attributes.Count > 0, "应该有属性数据被加载");
    }

    [Fact]
    public async Task BoneBodyTypeEditor_Should_Load_Xml_File()
    {
        // Arrange
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();
        var boneBodyTypeEditor = editorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "BoneBodyTypeEditor");

        // Act
        editorManager.SelectEditorCommand.Execute(boneBodyTypeEditor);
        var currentEditor = editorManager.CurrentEditorViewModel as BoneBodyTypeEditorViewModel;
        
        // 等待异步XML加载完成
        await Task.Delay(100); // 等待异步操作

        // Assert
        Assert.NotNull(currentEditor);
        Assert.NotEmpty(currentEditor.BoneBodyTypes);
        Assert.Contains("bone_body_types.xml", currentEditor.FilePath);
    }

    [Fact]
    public async Task AttributeEditor_Should_Handle_Missing_File_Gracefully()
    {
        // Arrange
        var attributeEditor = TestServiceProvider.GetService<AttributeEditorViewModel>();

        // Act
        await attributeEditor.LoadXmlFileAsync("nonexistent.xml");

        // Assert - 应该创建一个默认的编辑器，不崩溃
        Assert.NotEmpty(attributeEditor.Attributes);
        Assert.Equal("nonexistent.xml", attributeEditor.FilePath);
        Assert.False(attributeEditor.HasUnsavedChanges);
    }

    [Fact]
    public void BoneBodyTypeEditor_Should_Handle_Missing_File_Gracefully()
    {
        // Arrange
        var boneBodyTypeEditor = TestServiceProvider.GetService<BoneBodyTypeEditorViewModel>();

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
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();
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
        var editorManager = TestServiceProvider.GetService<EditorManagerViewModel>();
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