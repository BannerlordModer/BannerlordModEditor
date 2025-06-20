using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using BannerlordModEditor.UI.Views.Editors;
using BannerlordModEditor.UI.ViewModels.Editors;

namespace BannerlordModEditor.UI.Tests;

public class AttributeEditorTests
{
    [AvaloniaFact]
    public void AttributeEditor_ShouldInitializeWithSampleData()
    {
        var viewModel = new AttributeEditorViewModel();
        var view = new AttributeEditorView { DataContext = viewModel };
        
        var window = new Window { Content = view };
        window.Show();

        // 验证初始化状态
        Assert.NotEmpty(viewModel.Attributes);
        Assert.Equal("NewAttribute", viewModel.Attributes[0].Id);
        Assert.False(viewModel.HasUnsavedChanges);
    }

    [AvaloniaFact]
    public void AttributeEditor_ShouldAddNewAttribute()
    {
        var viewModel = new AttributeEditorViewModel();
        var view = new AttributeEditorView { DataContext = viewModel };
        
        var window = new Window { Content = view };
        window.Show();

        var initialCount = viewModel.Attributes.Count;
        
        // 添加新属性
        viewModel.AddAttributeCommand.Execute(null);

        Assert.Equal(initialCount + 1, viewModel.Attributes.Count);
        Assert.True(viewModel.HasUnsavedChanges);
    }

    [AvaloniaFact]
    public void AttributeEditor_ShouldRemoveAttribute()
    {
        var viewModel = new AttributeEditorViewModel();
        var view = new AttributeEditorView { DataContext = viewModel };
        
        var window = new Window { Content = view };
        window.Show();

        var attributeToRemove = viewModel.Attributes.First();
        var initialCount = viewModel.Attributes.Count;
        
        // 删除属性
        viewModel.RemoveAttributeCommand.Execute(attributeToRemove);

        Assert.Equal(initialCount - 1, viewModel.Attributes.Count);
        Assert.DoesNotContain(attributeToRemove, viewModel.Attributes);
        Assert.True(viewModel.HasUnsavedChanges);
    }

    [AvaloniaFact]
    public void AttributeDataViewModel_ShouldValidateCorrectly()
    {
        var attributeViewModel = new AttributeDataViewModel();

        // 初始状态应该无效（空ID和名称）
        Assert.False(attributeViewModel.IsValid);

        // 设置ID但没有名称
        attributeViewModel.Id = "TestId";
        Assert.False(attributeViewModel.IsValid);

        // 设置名称
        attributeViewModel.Name = "Test Name";
        Assert.True(attributeViewModel.IsValid);

        // 清空ID
        attributeViewModel.Id = "";
        Assert.False(attributeViewModel.IsValid);
    }

    [AvaloniaFact]
    public void AttributeDataViewModel_ShouldHaveCorrectSourceOptions()
    {
        var attributeViewModel = new AttributeDataViewModel();
        
        var sourceOptions = attributeViewModel.SourceOptions.ToList();
        
        Assert.Contains("Character", sourceOptions);
        Assert.Contains("WieldedWeapon", sourceOptions);
        Assert.Contains("WieldedShield", sourceOptions);
        Assert.Contains("SumEquipment", sourceOptions);
    }

    [AvaloniaFact]
    public void AttributeEditor_ShouldHandleDataContextChange()
    {
        var view = new AttributeEditorView();
        var window = new Window { Content = view };
        window.Show();

        // 初始状态没有DataContext
        Assert.Null(view.DataContext);

        // 设置ViewModel
        var viewModel = new AttributeEditorViewModel();
        view.DataContext = viewModel;

        Assert.Equal(viewModel, view.DataContext);
    }
} 