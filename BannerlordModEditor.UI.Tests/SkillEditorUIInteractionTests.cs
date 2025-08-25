using Xunit;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.UI.Tests.Helpers;

namespace BannerlordModEditor.UI.Tests;

public class SkillEditorCommandTests
{
    [Fact]
    public void SkillEditor_AddCommand_ShouldAddNewSkill()
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
    public void SkillEditor_RemoveCommand_ShouldRemoveSkill()
    {
        // Arrange
        var viewModel = TestServiceProvider.GetService<SkillEditorViewModel>();
        viewModel.AddSkillCommand.Execute(null); // 添加一个技能以便删除
        
        var skillToRemove = viewModel.Skills.Last();
        var initialCount = viewModel.Skills.Count;

        // Act
        viewModel.RemoveSkillCommand.Execute(skillToRemove);

        // Assert
        Assert.Equal(initialCount - 1, viewModel.Skills.Count);
        Assert.DoesNotContain(skillToRemove, viewModel.Skills);
        Assert.True(viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void SkillEditor_LoadCommand_ShouldAttemptLoad()
    {
        // Arrange
        var viewModel = TestServiceProvider.GetService<SkillEditorViewModel>();
        var originalFilePath = viewModel.FilePath;

        // Act
        viewModel.LoadFileCommand.Execute(null);

        // Assert - 文件路径应该更新
        Assert.NotEqual(originalFilePath, viewModel.FilePath);
        Assert.Contains("skills.xml", viewModel.FilePath);
    }

    [Fact]
    public void SkillEditor_SaveCommand_ShouldNotThrow()
    {
        // Arrange
        var viewModel = new SkillEditorViewModel();
        viewModel.FilePath = "test_output.xml";
        viewModel.AddSkillCommand.Execute(null); // 产生一些变更

        // Act & Assert
        var exception = Record.Exception(() => viewModel.SaveFileCommand.Execute(null));
        Assert.Null(exception);
    }

    [Fact]
    public void SkillEditor_ValidationStates_ShouldUpdateCorrectly()
    {
        // Arrange
        var viewModel = new SkillEditorViewModel();
        var skill = viewModel.Skills.First();
        
        // Act & Assert - 测试验证状态变化
        Assert.True(skill.IsValid); // 初始状态应该有效

        skill.Id = ""; // 清空ID
        Assert.False(skill.IsValid); // 应该无效

        skill.Id = "ValidId"; // 恢复有效ID
        Assert.True(skill.IsValid); // 应该又变有效

        skill.Name = ""; // 清空名称
        Assert.False(skill.IsValid); // 应该无效

        skill.Name = "Valid Name"; // 恢复有效名称
        Assert.True(skill.IsValid); // 应该又变有效
    }

    [Fact]
    public void SkillEditor_PropertyChanges_ShouldUpdateViewModel()
    {
        // Arrange
        var viewModel = new SkillEditorViewModel();
        var skill = viewModel.Skills.First();
        var originalId = skill.Id;
        var originalName = skill.Name;

        // Act
        skill.Id = "ModifiedSkillId";
        skill.Name = "Modified Skill Name";
        skill.Documentation = "Modified Documentation Content";

        // Assert
        Assert.NotEqual(originalId, skill.Id);
        Assert.NotEqual(originalName, skill.Name);
        Assert.Equal("ModifiedSkillId", skill.Id);
        Assert.Equal("Modified Skill Name", skill.Name);
        Assert.Equal("Modified Documentation Content", skill.Documentation);
        Assert.True(skill.IsValid);
    }

    [Fact]
    public void SkillEditor_UnsavedChangesIndicator_ShouldUpdateCorrectly()
    {
        // Arrange
        var viewModel = new SkillEditorViewModel();

        // 初始状态
        Assert.False(viewModel.HasUnsavedChanges);

        // Act & Assert - 各种操作应该触发未保存状态
        viewModel.AddSkillCommand.Execute(null);
        Assert.True(viewModel.HasUnsavedChanges);

        // 加载文件应该重置未保存状态
        viewModel.LoadXmlFile("test.xml");
        Assert.False(viewModel.HasUnsavedChanges);

        // 删除技能应该触发未保存状态
        if (viewModel.Skills.Any())
        {
            viewModel.RemoveSkillCommand.Execute(viewModel.Skills.First());
            Assert.True(viewModel.HasUnsavedChanges);
        }
    }

    [Fact]
    public void SkillEditor_MultipleOperations_ShouldMaintainDataIntegrity()
    {
        // Arrange
        var viewModel = new SkillEditorViewModel();
        var originalCount = viewModel.Skills.Count;
        
        // Act - 执行多个操作
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
    public void SkillDataViewModel_PropertyChangedNotification_ShouldWork()
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
} 