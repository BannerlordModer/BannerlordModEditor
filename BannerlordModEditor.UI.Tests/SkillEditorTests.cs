using Xunit;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.LogicalTree;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Views.Editors;
using BannerlordModEditor.UI.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.UI.Tests.Helpers;

namespace BannerlordModEditor.UI.Tests;

public class SkillEditorTests
{
    [Fact]
    public void SkillEditor_ShouldInitializeWithDefaultSkill()
    {
        // Arrange & Act
        var editor = new SkillEditorViewModel();
        
        // Assert
        Assert.NotNull(editor.Skills);
        Assert.Single(editor.Skills);
        Assert.Equal("NewSkill", editor.Skills.First().Id);
        Assert.Equal("New Skill", editor.Skills.First().Name);
        Assert.False(editor.HasUnsavedChanges);
    }

    [Fact]
    public void SkillEditorViewModel_ShouldInitializeCorrectly()
    {
        // Arrange
        var viewModel = TestServiceProvider.GetService<SkillEditorViewModel>();

        // Assert
        Assert.NotNull(viewModel.Skills);
        Assert.Single(viewModel.Skills);
        Assert.Equal("NewSkill", viewModel.Skills.First().Id);
        Assert.Equal("New Skill", viewModel.Skills.First().Name);
        Assert.False(viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void AddSkillCommand_ShouldAddNewSkill()
    {
        // Arrange
        var viewModel = TestServiceProvider.GetService<SkillEditorViewModel>();
        var initialCount = viewModel.Skills.Count;
        
        // Act
        viewModel.AddSkillCommand.Execute(null);

        // Assert
        Assert.Equal(initialCount + 1, viewModel.Skills.Count);
        Assert.True(viewModel.HasUnsavedChanges);
        Assert.Equal("NewSkill2", viewModel.Skills.Last().Id);
    }

    [Fact]
    public void RemoveSkillCommand_ShouldRemoveSkill()
    {
        // Arrange
        var viewModel = TestServiceProvider.GetService<SkillEditorViewModel>();
        var skillToRemove = viewModel.Skills.First();
        var initialCount = viewModel.Skills.Count;
        
        // Act
        viewModel.RemoveSkillCommand.Execute(skillToRemove);

        // Assert
        Assert.Equal(initialCount - 1, viewModel.Skills.Count);
        Assert.DoesNotContain(skillToRemove, viewModel.Skills);
        Assert.True(viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void LoadFileCommand_ShouldAttemptToLoadFile()
    {
        // Arrange
        var viewModel = TestServiceProvider.GetService<SkillEditorViewModel>();
        var originalFilePath = viewModel.FilePath;
        
        // Act
        viewModel.LoadFileCommand.Execute(null);

        // Assert
        // 文件路径应该更新，即使文件不存在
        Assert.NotEqual(originalFilePath, viewModel.FilePath);
        Assert.False(viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void SkillDataViewModel_PropertyChanges_ShouldUpdateCorrectly()
    {
        // Arrange
        var viewModel = TestServiceProvider.GetService<SkillEditorViewModel>();
        var skill = viewModel.Skills.First();
        var originalId = skill.Id;
        var originalName = skill.Name;
        
        // Act
        skill.Id = "ModifiedId";
        skill.Name = "Modified Name";
        skill.Documentation = "Modified Documentation";

        // Assert
        Assert.NotEqual(originalId, skill.Id);
        Assert.NotEqual(originalName, skill.Name);
        Assert.Equal("ModifiedId", skill.Id);
        Assert.Equal("Modified Name", skill.Name);
        Assert.Equal("Modified Documentation", skill.Documentation);
    }

    [Fact]
    public void SkillDataViewModel_ShouldValidateCorrectly()
    {
        // Arrange & Act
        var validSkill = new SkillDataViewModel
        {
            Id = "TestSkill",
            Name = "Test Skill"
        };
        
        var invalidSkill1 = new SkillDataViewModel
        {
            Id = "",
            Name = "Test Skill"
        };
        
        var invalidSkill2 = new SkillDataViewModel
        {
            Id = "TestSkill",
            Name = ""
        };
        
        // Assert
        Assert.True(validSkill.IsValid);
        Assert.False(invalidSkill1.IsValid);
        Assert.False(invalidSkill2.IsValid);
    }

    [Fact]
    public void SkillDataViewModel_ShouldNotifyPropertyChanges()
    {
        // Arrange
        var skill = new SkillDataViewModel();
        var propertyChangedFired = false;
        var validPropertyChangedFired = false;

        skill.PropertyChanged += (sender, e) => 
        {
            if (e.PropertyName == nameof(SkillDataViewModel.Id))
                propertyChangedFired = true;
            if (e.PropertyName == nameof(SkillDataViewModel.IsValid))
                validPropertyChangedFired = true;
        };

        // Act
        skill.Id = "TestId";

        // Assert
        Assert.True(propertyChangedFired);
        Assert.True(validPropertyChangedFired);
    }

    [Fact]
    public void LoadXmlFile_WithNonExistentFile_ShouldCreateNewSkill()
    {
        // Arrange
        var editor = new SkillEditorViewModel();
        
        // Act
        editor.LoadXmlFile("non_existent_file.xml");
        
        // Assert
        Assert.Single(editor.Skills);
        Assert.Equal("NewSkill", editor.Skills.First().Id);
        Assert.Contains("non_existent_file.xml", editor.Skills.First().Documentation);
        Assert.Equal("non_existent_file.xml", editor.FilePath);
        Assert.False(editor.HasUnsavedChanges);
    }

    [Fact]
    public void LoadXmlFile_WithValidTestData_ShouldLoadCorrectly()
    {
        // Arrange
        var editor = new SkillEditorViewModel();
        
        // Act
        editor.LoadXmlFile("skills.xml");
        
        // Assert
        Assert.NotEmpty(editor.Skills);
        Assert.Contains("skills.xml", editor.FilePath);
        Assert.False(editor.HasUnsavedChanges);
    }

    [Fact]
    public void SaveFile_WithValidData_ShouldNotThrow()
    {
        // Arrange
        var editor = new SkillEditorViewModel();
        editor.FilePath = "test_output.xml";
        
        // Act & Assert
        var exception = Record.Exception(() => editor.SaveFileCommand.Execute(null));
        Assert.Null(exception);
    }

    [Fact]
    public void MainWindow_Integration_ShouldSelectSkillEditor()
    {
        // Arrange
        var mainViewModel = TestServiceProvider.GetService<MainWindowViewModel>();
        var skillEditor = TestServiceProvider.GetService<SkillEditorViewModel>();

        // Act
        mainViewModel.EditorManager.SelectEditorCommand.Execute(skillEditor);

        // Assert
        Assert.Equal(skillEditor, mainViewModel.EditorManager.SelectedEditor);
        Assert.NotNull(mainViewModel.EditorManager.CurrentEditorViewModel);
        Assert.IsType<SkillEditorViewModel>(mainViewModel.EditorManager.CurrentEditorViewModel);
        Assert.Contains("角色设定", mainViewModel.EditorManager.CurrentBreadcrumb);
    }

    [Fact]
    public void SkillEditor_Search_ShouldFindSkillEditor()
    {
        // Arrange
        var mainViewModel = TestServiceProvider.GetService<MainWindowViewModel>();

        // Act
        mainViewModel.EditorManager.SearchText = "技能";

        // Assert
        var skillCategory = mainViewModel.EditorManager.Categories
            .FirstOrDefault(c => c.Name == "角色设定");
        Assert.NotNull(skillCategory);
        Assert.True(skillCategory.IsExpanded);

        var skillEditor = skillCategory.Editors
            .FirstOrDefault(e => e.EditorType == "SkillEditor");
        Assert.NotNull(skillEditor);
        Assert.True(skillEditor.IsAvailable);
    }

    [Fact]
    public void SkillEditor_MultipleOperations_ShouldMaintainDataIntegrity()
    {
        // Arrange
        var viewModel = TestServiceProvider.GetService<SkillEditorViewModel>();
        
        // Act - 执行多个操作
        var originalCount = viewModel.Skills.Count;
        
        // 添加技能
        viewModel.AddSkillCommand.Execute(null);
        viewModel.AddSkillCommand.Execute(null);
        
        // 修改第一个技能
        var firstSkill = viewModel.Skills.First();
        firstSkill.Id = "ModifiedSkill1";
        firstSkill.Name = "Modified Skill 1";
        
        // 删除最后一个技能
        var lastSkill = viewModel.Skills.Last();
        viewModel.RemoveSkillCommand.Execute(lastSkill);

        // Assert
        Assert.Equal(originalCount + 1, viewModel.Skills.Count); // +2-1
        Assert.Equal("ModifiedSkill1", viewModel.Skills.First().Id);
        Assert.Equal("Modified Skill 1", viewModel.Skills.First().Name);
        Assert.DoesNotContain(lastSkill, viewModel.Skills);
        Assert.True(viewModel.HasUnsavedChanges);
    }
} 