using Xunit;
using BannerlordModEditor.UI.ViewModels;
using System.Linq;
using BannerlordModEditor.UI.Tests.Helpers;

namespace BannerlordModEditor.UI.Tests;

public class UIVisibilityTests
{
    [Fact]
    public void MainWindow_Should_Show_Default_Content_Initially()
    {
        // Arrange & Act
        var mainViewModel = TestServiceProvider.GetService<MainWindowViewModel>();

        // Assert
        Assert.True(mainViewModel.ShowDefaultContent);
        Assert.False(mainViewModel.ShowAttributeEditor);
        Assert.False(mainViewModel.ShowBoneBodyTypeEditor);
    }

    [Fact]
    public void Selecting_AttributeEditor_Should_Show_AttributeEditor()
    {
        // Arrange
        var mainViewModel = TestServiceProvider.GetService<MainWindowViewModel>();
        var attributeEditor = mainViewModel.EditorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "AttributeEditor");

        // Act
        mainViewModel.EditorManager.SelectEditorCommand.Execute(attributeEditor);

        // Assert
        Assert.False(mainViewModel.ShowDefaultContent);
        Assert.True(mainViewModel.ShowAttributeEditor);
        Assert.False(mainViewModel.ShowBoneBodyTypeEditor);
        Assert.Equal(attributeEditor, mainViewModel.EditorManager.SelectedEditor);
    }

    [Fact]
    public void Selecting_BoneBodyTypeEditor_Should_Show_BoneBodyTypeEditor()
    {
        // Arrange
        var mainViewModel = TestServiceProvider.GetService<MainWindowViewModel>();
        var boneBodyTypeEditor = mainViewModel.EditorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "BoneBodyTypeEditor");

        // Act
        mainViewModel.EditorManager.SelectEditorCommand.Execute(boneBodyTypeEditor);

        // Assert
        Assert.False(mainViewModel.ShowDefaultContent);
        Assert.False(mainViewModel.ShowAttributeEditor);
        Assert.True(mainViewModel.ShowBoneBodyTypeEditor);
        Assert.Equal(boneBodyTypeEditor, mainViewModel.EditorManager.SelectedEditor);
    }

    [Fact]
    public void Selecting_SkillEditor_Should_Show_SkillEditor()
    {
        // Arrange
        var mainViewModel = TestServiceProvider.GetService<MainWindowViewModel>();
        var skillEditor = mainViewModel.EditorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "SkillEditor");

        // Act
        mainViewModel.EditorManager.SelectEditorCommand.Execute(skillEditor);

        // Assert
        Assert.NotNull(mainViewModel.EditorManager.CurrentEditorViewModel);
        Assert.IsType<BannerlordModEditor.UI.ViewModels.Editors.SkillEditorViewModel>(mainViewModel.EditorManager.CurrentEditorViewModel);
        Assert.Equal(skillEditor, mainViewModel.EditorManager.SelectedEditor);
        Assert.Contains("技能系统", mainViewModel.EditorManager.CurrentBreadcrumb);
    }

    [Fact]
    public void Switching_Between_Editors_Should_Update_Visibility()
    {
        // Arrange
        var mainViewModel = TestServiceProvider.GetService<MainWindowViewModel>();
        var attributeEditor = mainViewModel.EditorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "AttributeEditor");
        var boneBodyTypeEditor = mainViewModel.EditorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "BoneBodyTypeEditor");
        var skillEditor = mainViewModel.EditorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "SkillEditor");

        // Act & Assert - 选择属性编辑器
        mainViewModel.EditorManager.SelectEditorCommand.Execute(attributeEditor);
        Assert.True(mainViewModel.ShowAttributeEditor);
        Assert.False(mainViewModel.ShowBoneBodyTypeEditor);
        Assert.False(mainViewModel.ShowDefaultContent);

        // Act & Assert - 切换到骨骼体型编辑器
        mainViewModel.EditorManager.SelectEditorCommand.Execute(boneBodyTypeEditor);
        Assert.False(mainViewModel.ShowAttributeEditor);
        Assert.True(mainViewModel.ShowBoneBodyTypeEditor);
        Assert.False(mainViewModel.ShowDefaultContent);

        // Act & Assert - 切换到技能编辑器
        mainViewModel.EditorManager.SelectEditorCommand.Execute(skillEditor);
        Assert.NotNull(mainViewModel.EditorManager.CurrentEditorViewModel);
        Assert.IsType<BannerlordModEditor.UI.ViewModels.Editors.SkillEditorViewModel>(mainViewModel.EditorManager.CurrentEditorViewModel);
        Assert.False(mainViewModel.ShowDefaultContent);
    }

    [Fact]
    public void XML_Loading_Should_Work_When_Selecting_Editor()
    {
        // Arrange
        var mainViewModel = TestServiceProvider.GetService<MainWindowViewModel>();
        var attributeEditor = mainViewModel.EditorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "AttributeEditor");

        // Act
        mainViewModel.EditorManager.SelectEditorCommand.Execute(attributeEditor);

        // Assert
        Assert.NotEmpty(mainViewModel.AttributeEditor.Attributes);
        Assert.Contains("attributes.xml", mainViewModel.AttributeEditor.FilePath);
    }
} 