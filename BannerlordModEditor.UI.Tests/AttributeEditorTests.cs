using Xunit;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using BannerlordModEditor.UI.Views.Editors;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Tests.Helpers;
using System.Linq;

namespace BannerlordModEditor.UI.Tests;

public class AttributeEditorTests
{
    [Fact]
    public void AttributeEditor_ShouldInitializeWithSampleData()
    {
        // Arrange
        var viewModel = TestServiceProvider.GetService<AttributeEditorViewModel>();

        // Assert
        Assert.NotEmpty(viewModel.Attributes);
        Assert.Equal("NewAttribute", viewModel.Attributes[0].Id);
        Assert.False(viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void AttributeEditor_ShouldAddNewAttribute()
    {
        // Arrange
        var viewModel = TestServiceProvider.GetService<AttributeEditorViewModel>();
        var initialCount = viewModel.Attributes.Count;
        
        // Act
        viewModel.AddAttributeCommand.Execute(null);

        // Assert
        Assert.Equal(initialCount + 1, viewModel.Attributes.Count);
        Assert.True(viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void AttributeEditor_ShouldRemoveAttribute()
    {
        // Arrange
        var viewModel = TestServiceProvider.GetService<AttributeEditorViewModel>();
        var attributeToRemove = viewModel.Attributes.First();
        var initialCount = viewModel.Attributes.Count;
        
        // Act
        viewModel.RemoveAttributeCommand.Execute(attributeToRemove);

        // Assert
        Assert.Equal(initialCount - 1, viewModel.Attributes.Count);
        Assert.DoesNotContain(attributeToRemove, viewModel.Attributes);
        Assert.True(viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void AttributeDataViewModel_ShouldValidateCorrectly()
    {
        // Arrange
        var attributeViewModel = new AttributeDataViewModel();

        // Act & Assert
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

    [Fact]
    public void AttributeDataViewModel_ShouldHaveCorrectSourceOptions()
    {
        // Arrange
        var attributeViewModel = new AttributeDataViewModel();
        
        // Act
        var sourceOptions = attributeViewModel.SourceOptions.ToList();
        
        // Assert
        Assert.Contains("Character", sourceOptions);
        Assert.Contains("WieldedWeapon", sourceOptions);
        Assert.Contains("WieldedShield", sourceOptions);
        Assert.Contains("SumEquipment", sourceOptions);
    }

    [Fact]
    public void AttributeEditorViewModel_ShouldInitializeCorrectly()
    {
        // Arrange
        var viewModel = TestServiceProvider.GetService<AttributeEditorViewModel>();

        // Assert
        Assert.NotNull(viewModel.Attributes);
        Assert.NotEmpty(viewModel.Attributes);
        Assert.Equal("NewAttribute", viewModel.Attributes[0].Id);
        Assert.False(viewModel.HasUnsavedChanges);
    }
} 