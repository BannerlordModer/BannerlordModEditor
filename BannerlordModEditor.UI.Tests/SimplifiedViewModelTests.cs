using Xunit;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.Tests.Helpers;
using BannerlordModEditor.Common.Services;
using System.IO;

namespace BannerlordModEditor.UI.Tests;

/// <summary>
/// 简化的ViewModel测试，专注于业务逻辑而不是UI组件
/// </summary>
public class SimplifiedViewModelTests
{
    [Fact]
    public void AttributeEditorViewModel_ShouldInitializeCorrectly()
    {
        // 使用依赖注入创建ViewModel
        var viewModel = TestServiceProvider.GetService<AttributeEditorViewModel>();
        
        // 验证基本属性
        Assert.NotNull(viewModel);
        Assert.NotNull(viewModel.Attributes);
        Assert.Equal("attributes.xml", viewModel.XmlFileName);
        Assert.False(viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void AttributeEditorViewModel_AddCommand_ShouldWork()
    {
        var viewModel = TestServiceProvider.GetService<AttributeEditorViewModel>();
        var initialCount = viewModel.Attributes.Count;
        
        // 测试添加命令
        Assert.NotNull(viewModel.AddAttributeCommand);
        Assert.True(viewModel.AddAttributeCommand.CanExecute(null));
        
        viewModel.AddAttributeCommand.Execute(null);
        
        Assert.Equal(initialCount + 1, viewModel.Attributes.Count);
        Assert.True(viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void AttributeEditorViewModel_RemoveCommand_ShouldWork()
    {
        var viewModel = TestServiceProvider.GetService<AttributeEditorViewModel>();
        
        // 先添加一个属性
        viewModel.AddAttributeCommand.Execute(null);
        var attributeToRemove = viewModel.Attributes.Last();
        var initialCount = viewModel.Attributes.Count;
        
        // 测试删除命令
        Assert.NotNull(viewModel.RemoveAttributeCommand);
        Assert.True(viewModel.RemoveAttributeCommand.CanExecute(attributeToRemove));
        
        viewModel.RemoveAttributeCommand.Execute(attributeToRemove);
        
        Assert.Equal(initialCount - 1, viewModel.Attributes.Count);
        Assert.True(viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void SkillEditorViewModel_ShouldInitializeCorrectly()
    {
        var viewModel = TestServiceProvider.GetService<SkillEditorViewModel>();
        
        Assert.NotNull(viewModel);
        Assert.NotNull(viewModel.Skills);
        Assert.False(viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void EditorManagerViewModel_ShouldHaveCategories()
    {
        var manager = TestServiceProvider.GetService<EditorManagerViewModel>();
        
        Assert.NotNull(manager);
        Assert.NotNull(manager.Categories);
        Assert.True(manager.Categories.Count > 0);
        
        // 验证角色分类存在
        var characterCategory = manager.Categories.FirstOrDefault(c => c.Name == "角色设定");
        Assert.NotNull(characterCategory);
        Assert.True(characterCategory.Editors.Count > 0);
    }

    [Fact]
    public void EditorFactory_ShouldCreateCorrectViewModels()
    {
        var factory = TestServiceProvider.GetService<IEditorFactory>();
        
        // 测试创建属性编辑器
        var attributeEditor = factory.CreateEditorViewModel("AttributeEditor", "attributes.xml");
        Assert.NotNull(attributeEditor);
        Assert.IsType<AttributeEditorViewModel>(attributeEditor);
        
        // 测试创建技能编辑器
        var skillEditor = factory.CreateEditorViewModel("SkillEditor", "skills.xml");
        Assert.NotNull(skillEditor);
        Assert.IsType<SkillEditorViewModel>(skillEditor);
    }

    [Fact]
    public void XmlFileDiscovery_ShouldWork()
    {
        var discoveryService = TestServiceProvider.GetService<IFileDiscoveryService>();
        
        Assert.NotNull(discoveryService);
        
        // 测试命名转换功能
        var modelName = discoveryService.ConvertToModelName("attributes.xml");
        Assert.Equal("Attributes", modelName);
        
        var modelName2 = discoveryService.ConvertToModelName("bone_body_types.xml");
        Assert.Equal("BoneBodyTypes", modelName2);
    }
}