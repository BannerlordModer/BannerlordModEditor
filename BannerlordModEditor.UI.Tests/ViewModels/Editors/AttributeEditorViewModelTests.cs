using Xunit;
using Moq;
using CommunityToolkit.Mvvm.ComponentModel;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.Common.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BannerlordModEditor.UI.Tests.ViewModels.Editors;

public class AttributeEditorViewModelTests
{
    private readonly Mock<IValidationService> _mockValidationService;
    private readonly AttributeEditorViewModel _viewModel;

    public AttributeEditorViewModelTests()
    {
        _mockValidationService = new Mock<IValidationService>();
        _viewModel = new AttributeEditorViewModel(_mockValidationService.Object);
    }

    [Fact]
    public void Constructor_InitializesWithDefaultData()
    {
        // Assert
        Assert.NotNull(_viewModel.Attributes);
        Assert.Single(_viewModel.Attributes);
        Assert.Equal("NewAttribute", _viewModel.Attributes[0].Id);
        Assert.Equal("New Attribute", _viewModel.Attributes[0].Name);
        Assert.Equal("Character", _viewModel.Attributes[0].Source);
    }

    [Fact]
    public void AddAttribute_ShouldAddNewAttribute()
    {
        // Arrange
        var initialCount = _viewModel.Attributes.Count;

        // Act
        _viewModel.AddAttribute();

        // Assert
        Assert.Equal(initialCount + 1, _viewModel.Attributes.Count);
        Assert.Contains("NewAttribute", _viewModel.Attributes.Last().Id);
        Assert.True(_viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void RemoveAttribute_ShouldRemoveAttribute()
    {
        // Arrange
        var attributeToRemove = _viewModel.Attributes.First();
        var initialCount = _viewModel.Attributes.Count;

        // Act
        _viewModel.RemoveAttribute(attributeToRemove);

        // Assert
        Assert.Equal(initialCount - 1, _viewModel.Attributes.Count);
        Assert.DoesNotContain(attributeToRemove, _viewModel.Attributes);
        Assert.True(_viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void RemoveAttribute_WithNull_ShouldDoNothing()
    {
        // Arrange
        var initialCount = _viewModel.Attributes.Count;

        // Act
        _viewModel.RemoveAttribute(null);

        // Assert
        Assert.Equal(initialCount, _viewModel.Attributes.Count);
        Assert.False(_viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void DuplicateAttribute_ShouldCreateCopy()
    {
        // Arrange
        var originalAttribute = _viewModel.Attributes.First();
        var initialCount = _viewModel.Attributes.Count;

        // Act
        _viewModel.DuplicateAttribute(originalAttribute);

        // Assert
        Assert.Equal(initialCount + 1, _viewModel.Attributes.Count);
        var duplicatedAttribute = _viewModel.Attributes.Last();
        Assert.Equal($"{originalAttribute.Id}_Copy", duplicatedAttribute.Id);
        Assert.Equal($"{originalAttribute.Name} (Copy)", duplicatedAttribute.Name);
        Assert.Equal(originalAttribute.Source, duplicatedAttribute.Source);
        Assert.True(_viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void ItemMatchesFilter_ShouldMatchById()
    {
        // Arrange
        var item = new AttributeDataViewModel { Id = "test_id", Name = "Test Name" };
        var viewModel = new TestableAttributeEditorViewModel();

        // Act
        var result1 = viewModel.TestItemMatchesFilter(item, "test_id");
        var result2 = viewModel.TestItemMatchesFilter(item, "TEST_ID"); // Case insensitive
        var result3 = viewModel.TestItemMatchesFilter(item, "other_id");

        // Assert
        Assert.True(result1);
        Assert.True(result2);
        Assert.False(result3);
    }

    [Fact]
    public void ItemMatchesFilter_ShouldMatchByName()
    {
        // Arrange
        var item = new AttributeDataViewModel { Id = "test_id", Name = "Test Name" };
        var viewModel = new TestableAttributeEditorViewModel();

        // Act
        var result1 = viewModel.TestItemMatchesFilter(item, "Test Name");
        var result2 = viewModel.TestItemMatchesFilter(item, "test name"); // Case insensitive
        var result3 = viewModel.TestItemMatchesFilter(item, "Other Name");

        // Assert
        Assert.True(result1);
        Assert.True(result2);
        Assert.False(result3);
    }

    [Fact]
    public void ItemMatchesFilter_ShouldMatchByDocumentation()
    {
        // Arrange
        var item = new AttributeDataViewModel { Id = "test_id", Name = "Test Name", Documentation = "Test documentation text" };
        var viewModel = new TestableAttributeEditorViewModel();

        // Act
        var result1 = viewModel.TestItemMatchesFilter(item, "documentation");
        var result2 = viewModel.TestItemMatchesFilter(item, "DOCUMENTATION"); // Case insensitive
        var result3 = viewModel.TestItemMatchesFilter(item, "other text");

        // Assert
        Assert.True(result1);
        Assert.True(result2);
        Assert.False(result3);
    }

    [Fact]
    public void CreateNewItemViewModel_ShouldCreateValidItem()
    {
        // Arrange
        var viewModel = new TestableAttributeEditorViewModel();
        // Clear initial sample data to have predictable count
        viewModel.Attributes.Clear();
        viewModel.Attributes.Add(new AttributeDataViewModel { Id = "test1" });
        viewModel.Attributes.Add(new AttributeDataViewModel { Id = "test2" });

        // Act
        var result = viewModel.TestCreateNewItemViewModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("NewAttribute3", result.Id); // Should be NewAttribute{count + 1}
        Assert.Equal("New Attribute", result.Name);
        Assert.Equal("Character", result.Source);
    }

    [Fact]
    public void ConvertToItemModel_ShouldConvertCorrectly()
    {
        // Arrange
        var viewModel = new TestableAttributeEditorViewModel();
        var attributeViewModel = new AttributeDataViewModel
        {
            Id = "test_id",
            Name = "Test Name",
            Source = "Character",
            Documentation = "Test documentation"
        };

        // Act
        var result = viewModel.TestConvertToItemModel(attributeViewModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test_id", result.Id);
        Assert.Equal("Test Name", result.Name);
        Assert.Equal("Character", result.Source);
        Assert.Equal("Test documentation", result.Documentation);
    }

    [Fact]
    public void ConvertToItemModel_ShouldHandleEmptyDocumentation()
    {
        // Arrange
        var viewModel = new TestableAttributeEditorViewModel();
        var attributeViewModel = new AttributeDataViewModel
        {
            Id = "test_id",
            Name = "Test Name",
            Source = "Character",
            Documentation = ""
        };

        // Act
        var result = viewModel.TestConvertToItemModel(attributeViewModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test_id", result.Id);
        Assert.Equal("Test Name", result.Name);
        Assert.Equal("Character", result.Source);
        Assert.Null(result.Documentation); // Empty string should become null
    }

    [Fact]
    public void ConvertToItemViewModel_ShouldConvertCorrectly()
    {
        // Arrange
        var viewModel = new TestableAttributeEditorViewModel();
        var attributeModel = new AttributeData
        {
            Id = "test_id",
            Name = "Test Name",
            Source = "Character",
            Documentation = "Test documentation"
        };

        // Act
        var result = viewModel.TestConvertToItemViewModel(attributeModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test_id", result.Id);
        Assert.Equal("Test Name", result.Name);
        Assert.Equal("Character", result.Source);
        Assert.Equal("Test documentation", result.Documentation);
    }

    [Fact]
    public void ConvertToItemViewModel_ShouldHandleNullDocumentation()
    {
        // Arrange
        var viewModel = new TestableAttributeEditorViewModel();
        var attributeModel = new AttributeData
        {
            Id = "test_id",
            Name = "Test Name",
            Source = "Character",
            Documentation = ""
        };

        // Act
        var result = viewModel.TestConvertToItemViewModel(attributeModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test_id", result.Id);
        Assert.Equal("Test Name", result.Name);
        Assert.Equal("Character", result.Source);
        Assert.Equal("", result.Documentation); // Null should become empty string
    }

    [Fact]
    public void ConvertToRootModel_ShouldConvertCollection()
    {
        // Arrange
        var viewModel = new TestableAttributeEditorViewModel();
        var attributes = new ObservableCollection<AttributeDataViewModel>
        {
            new AttributeDataViewModel { Id = "attr1", Name = "Attribute 1", Source = "Character" },
            new AttributeDataViewModel { Id = "attr2", Name = "Attribute 2", Source = "Character" }
        };

        // Act
        var result = viewModel.TestConvertToRootModel(attributes);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.AttributeData.Count);
        Assert.Equal("attr1", result.AttributeData[0].Id);
        Assert.Equal("attr2", result.AttributeData[1].Id);
    }

    [Fact]
    public void ConvertFromRootModel_ShouldConvertCollection()
    {
        // Arrange
        var viewModel = new TestableAttributeEditorViewModel();
        var rootModel = new ArrayOfAttributeData
        {
            AttributeData =
            {
                new AttributeData { Id = "attr1", Name = "Attribute 1", Source = "Character" },
                new AttributeData { Id = "attr2", Name = "Attribute 2", Source = "Character" }
            }
        };

        // Act
        var result = viewModel.TestConvertFromRootModel(rootModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("attr1", result[0].Id);
        Assert.Equal("attr2", result[1].Id);
    }

    [Fact]
    public void ValidateAll_ShouldShowCorrectStatus()
    {
        // Arrange
        var viewModel = new TestableAttributeEditorViewModel();
        viewModel.Attributes.Add(new AttributeDataViewModel { Id = "valid1", Name = "Valid 1", Source = "Character" });
        viewModel.Attributes.Add(new AttributeDataViewModel { Id = "", Name = "Invalid", Source = "Character" }); // Invalid - empty ID

        // Act
        viewModel.ValidateAll();

        // Assert
        Assert.Contains("存在验证错误", viewModel.StatusMessage);
    }

    [Fact]
    public void ValidateAll_ShouldShowSuccessWhenAllValid()
    {
        // Arrange
        var viewModel = new TestableAttributeEditorViewModel();
        viewModel.Attributes.Clear();
        viewModel.Attributes.Add(new AttributeDataViewModel { Id = "valid1", Name = "Valid 1", Source = "Character" });
        viewModel.Attributes.Add(new AttributeDataViewModel { Id = "valid2", Name = "Valid 2", Source = "Character" });

        // Act
        viewModel.ValidateAll();

        // Assert
        Assert.Contains("所有属性验证通过", viewModel.StatusMessage);
    }

    // Testable class to access protected methods
    private class TestableAttributeEditorViewModel : AttributeEditorViewModel
    {
        public TestableAttributeEditorViewModel() : base()
        {
        }

        public TestableAttributeEditorViewModel(IValidationService validationService) : base(validationService)
        {
        }

        public bool TestItemMatchesFilter(AttributeDataViewModel item, string filterText)
        {
            return base.ItemMatchesFilter(item, filterText);
        }

        public AttributeDataViewModel TestCreateNewItemViewModel()
        {
            return base.CreateNewItemViewModel();
        }

        public AttributeData TestConvertToItemModel(AttributeDataViewModel viewModel)
        {
            return base.ConvertToItemModel(viewModel);
        }

        public AttributeDataViewModel TestConvertToItemViewModel(AttributeData itemModel)
        {
            return base.ConvertToItemViewModel(itemModel);
        }

        public ArrayOfAttributeData TestConvertToRootModel(ObservableCollection<AttributeDataViewModel> items)
        {
            return base.ConvertToRootModel(items);
        }

        public ObservableCollection<AttributeDataViewModel> TestConvertFromRootModel(ArrayOfAttributeData rootModel)
        {
            return base.ConvertFromRootModel(rootModel);
        }
    }
}