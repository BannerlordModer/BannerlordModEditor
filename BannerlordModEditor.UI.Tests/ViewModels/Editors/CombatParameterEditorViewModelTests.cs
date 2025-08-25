using Xunit;
using Moq;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.Common.Models.DO;
using System.Collections.ObjectModel;

namespace BannerlordModEditor.UI.Tests.ViewModels.Editors;

public class CombatParameterEditorViewModelTests
{
    private readonly Mock<IValidationService> _mockValidationService;
    private readonly CombatParameterEditorViewModel _viewModel;

    public CombatParameterEditorViewModelTests()
    {
        _mockValidationService = new Mock<IValidationService>();
        _viewModel = new CombatParameterEditorViewModel(_mockValidationService.Object);
    }

    [Fact]
    public void Constructor_InitializesWithDefaultData()
    {
        // Assert
        Assert.NotNull(_viewModel.CombatParameters);
        Assert.Single(_viewModel.CombatParameters);
        Assert.Equal("example_parameter", _viewModel.CombatParameters[0].Id);
        
        Assert.NotNull(_viewModel.Definitions);
        Assert.Single(_viewModel.Definitions);
        Assert.Equal("example_def", _viewModel.Definitions[0].Name);
        
        Assert.False(_viewModel.HasEmptyCombatParameters);
    }

    [Fact]
    public void AddCombatParameter_ShouldAddNewParameter()
    {
        // Arrange
        var initialCount = _viewModel.CombatParameters.Count;

        // Act
        _viewModel.AddCombatParameter();

        // Assert
        Assert.Equal(initialCount + 1, _viewModel.CombatParameters.Count);
        Assert.Contains("parameter_", _viewModel.CombatParameters.Last().Id);
        Assert.True(_viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void RemoveCombatParameter_ShouldRemoveParameter()
    {
        // Arrange
        var parameterToRemove = _viewModel.CombatParameters.First();
        var initialCount = _viewModel.CombatParameters.Count;

        // Act
        _viewModel.RemoveCombatParameter(parameterToRemove);

        // Assert
        Assert.Equal(initialCount - 1, _viewModel.CombatParameters.Count);
        Assert.DoesNotContain(parameterToRemove, _viewModel.CombatParameters);
        Assert.True(_viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void AddDefinition_ShouldAddNewDefinition()
    {
        // Arrange
        var initialCount = _viewModel.Definitions.Count;

        // Act
        _viewModel.AddDefinition();

        // Assert
        Assert.Equal(initialCount + 1, _viewModel.Definitions.Count);
        Assert.Contains("def_", _viewModel.Definitions.Last().Name);
        Assert.Equal("1.0", _viewModel.Definitions.Last().Value);
        Assert.True(_viewModel.HasDefinitions);
        Assert.True(_viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void RemoveDefinition_ShouldRemoveDefinition()
    {
        // Arrange
        var definitionToRemove = _viewModel.Definitions.First();
        var initialCount = _viewModel.Definitions.Count;

        // Act
        _viewModel.RemoveDefinition(definitionToRemove);

        // Assert
        Assert.Equal(initialCount - 1, _viewModel.Definitions.Count);
        Assert.DoesNotContain(definitionToRemove, _viewModel.Definitions);
        Assert.True(_viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void RemoveDefinition_WhenEmpty_ShouldSetHasDefinitionsToFalse()
    {
        // Arrange
        _viewModel.Definitions.Clear();
        _viewModel.HasDefinitions = true;
        var definition = new DefinitionViewModel { Name = "test", Value = "1.0" };
        _viewModel.Definitions.Add(definition);

        // Act
        _viewModel.RemoveDefinition(definition);

        // Assert
        Assert.Empty(_viewModel.Definitions);
        Assert.False(_viewModel.HasDefinitions);
    }

    [Fact]
    public void ToggleEmptyCombatParameters_ShouldToggleValue()
    {
        // Arrange
        var initialValue = _viewModel.HasEmptyCombatParameters;

        // Act
        _viewModel.ToggleEmptyCombatParameters();

        // Assert
        Assert.NotEqual(initialValue, _viewModel.HasEmptyCombatParameters);
        Assert.True(_viewModel.HasUnsavedChanges);
    }

    [Fact]
    public void ItemMatchesFilter_ShouldMatchById()
    {
        // Arrange
        var item = new CombatParameterViewModel { Id = "test_parameter" };
        var viewModel = new TestableCombatParameterEditorViewModel();

        // Act
        var result1 = viewModel.TestItemMatchesFilter(item, "test_parameter");
        var result2 = viewModel.TestItemMatchesFilter(item, "TEST_PARAMETER"); // Case insensitive
        var result3 = viewModel.TestItemMatchesFilter(item, "other_parameter");

        // Assert
        Assert.True(result1);
        Assert.True(result2);
        Assert.False(result3);
    }

    [Fact]
    public void CreateNewItemViewModel_ShouldCreateValidItem()
    {
        // Arrange
        var viewModel = new TestableCombatParameterEditorViewModel();
        // Clear initial sample data to have predictable count
        viewModel.CombatParameters.Clear();
        viewModel.CombatParameters.Add(new CombatParameterViewModel { Id = "param1" });
        viewModel.CombatParameters.Add(new CombatParameterViewModel { Id = "param2" });

        // Act
        var result = viewModel.TestCreateNewItemViewModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("parameter_3", result.Id); // Should be parameter_{count + 1}
    }

    [Fact]
    public void DuplicateItemViewModel_ShouldCreateCopy()
    {
        // Arrange
        var viewModel = new TestableCombatParameterEditorViewModel();
        var source = new CombatParameterViewModel
        {
            Id = "original",
            CollisionRadius = "0.5",
            CollisionCheckStartingPercent = "0.1"
        };

        // Act
        var result = viewModel.TestDuplicateItemViewModel(source);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("original_Copy", result.Id);
        Assert.Equal("0.5", result.CollisionRadius);
        Assert.Equal("0.1", result.CollisionCheckStartingPercent);
    }

    [Fact]
    public void ConvertToItemDO_ShouldConvertCorrectly()
    {
        // Arrange
        var viewModel = new TestableCombatParameterEditorViewModel();
        var itemViewModel = new CombatParameterViewModel
        {
            Id = "test_param",
            CollisionRadius = "0.5",
            CollisionCheckStartingPercent = "0.1",
            HasCustomCollisionCapsule = true,
            CustomCollisionCapsuleP1 = "0,0,0",
            CustomCollisionCapsuleP2 = "1,1,1",
            CustomCollisionCapsuleR = "0.5"
        };

        // Act
        var result = viewModel.TestConvertToItemDO(itemViewModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test_param", result.Id);
        Assert.Equal("0.5", result.CollisionRadius);
        Assert.Equal("0.1", result.CollisionCheckStartingPercent);
        Assert.NotNull(result.CustomCollisionCapsule);
        Assert.Equal("0,0,0", result.CustomCollisionCapsule.P1);
        Assert.Equal("1,1,1", result.CustomCollisionCapsule.P2);
        Assert.Equal("0.5", result.CustomCollisionCapsule.R);
    }

    [Fact]
    public void ConvertToItemDO_ShouldHandleNullCustomCollisionCapsule()
    {
        // Arrange
        var viewModel = new TestableCombatParameterEditorViewModel();
        var itemViewModel = new CombatParameterViewModel
        {
            Id = "test_param",
            CollisionRadius = "0.5",
            HasCustomCollisionCapsule = false
        };

        // Act
        var result = viewModel.TestConvertToItemDO(itemViewModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test_param", result.Id);
        Assert.Null(result.CustomCollisionCapsule);
    }

    [Fact]
    public void ConvertToItemViewModel_ShouldConvertCorrectly()
    {
        // Arrange
        var viewModel = new TestableCombatParameterEditorViewModel();
        var itemDO = new BaseCombatParameterDO
        {
            Id = "test_param",
            CollisionRadius = "0.5",
            CollisionCheckStartingPercent = "0.1",
            CustomCollisionCapsule = new CustomCollisionCapsuleDO
            {
                P1 = "0,0,0",
                P2 = "1,1,1",
                R = "0.5"
            }
        };

        // Act
        var result = viewModel.TestConvertToItemViewModel(itemDO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test_param", result.Id);
        Assert.Equal("0.5", result.CollisionRadius);
        Assert.Equal("0.1", result.CollisionCheckStartingPercent);
        Assert.True(result.HasCustomCollisionCapsule);
        Assert.Equal("0,0,0", result.CustomCollisionCapsuleP1);
        Assert.Equal("1,1,1", result.CustomCollisionCapsuleP2);
        Assert.Equal("0.5", result.CustomCollisionCapsuleR);
    }

    [Fact]
    public void ConvertToItemViewModel_ShouldHandleNullCustomCollisionCapsule()
    {
        // Arrange
        var viewModel = new TestableCombatParameterEditorViewModel();
        var itemDO = new BaseCombatParameterDO
        {
            Id = "test_param",
            CollisionRadius = "0.5",
            CustomCollisionCapsule = null
        };

        // Act
        var result = viewModel.TestConvertToItemViewModel(itemDO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test_param", result.Id);
        Assert.Equal("0.5", result.CollisionRadius);
        Assert.False(result.HasCustomCollisionCapsule);
    }

    [Fact]
    public void ConvertToRootDO_ShouldConvertCollection()
    {
        // Arrange
        var viewModel = new TestableCombatParameterEditorViewModel();
        var items = new ObservableCollection<CombatParameterViewModel>
        {
            new CombatParameterViewModel { Id = "param1", CollisionRadius = "0.5" },
            new CombatParameterViewModel { Id = "param2", CollisionRadius = "0.6" }
        };

        // Act
        var result = viewModel.TestConvertToRootDO(items);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.CombatParametersList.Count);
        Assert.Equal("param1", result.CombatParametersList[0].Id);
        Assert.Equal("param2", result.CombatParametersList[1].Id);
    }

    [Fact]
    public void ConvertFromRootDO_ShouldConvertCollection()
    {
        // Arrange
        var viewModel = new TestableCombatParameterEditorViewModel();
        var rootDO = new CombatParametersDO
        {
            Type = "test_type",
            HasEmptyCombatParameters = false,
            CombatParametersList = new List<BaseCombatParameterDO>
            {
                new BaseCombatParameterDO { Id = "param1", CollisionRadius = "0.5" },
                new BaseCombatParameterDO { Id = "param2", CollisionRadius = "0.6" }
            }
        };

        // Act
        var result = viewModel.TestConvertFromRootDO(rootDO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("param1", result[0].Id);
        Assert.Equal("param2", result[1].Id);
        Assert.Equal("test_type", viewModel.Type);
        Assert.False(viewModel.HasEmptyCombatParameters);
    }

    [Fact]
    public void ConvertFromRootDO_ShouldHandleDefinitions()
    {
        // Arrange
        var viewModel = new TestableCombatParameterEditorViewModel();
        var rootDO = new CombatParametersDO
        {
            HasDefinitions = true,
            Definitions = new DefinitionsDO
            {
                Defs = new List<DefDO>
                {
                    new DefDO { Name = "def1", Value = "1.0" },
                    new DefDO { Name = "def2", Value = "2.0" }
                }
            }
        };

        // Act
        var result = viewModel.TestConvertFromRootDO(rootDO);

        // Assert
        Assert.NotNull(result);
        Assert.True(viewModel.HasDefinitions);
        Assert.Equal(2, viewModel.Definitions.Count);
        Assert.Equal("def1", viewModel.Definitions[0].Name);
        Assert.Equal("def2", viewModel.Definitions[1].Name);
    }

    [Fact]
    public void ValidateAll_ShouldShowCorrectStatus()
    {
        // Arrange
        var viewModel = new TestableCombatParameterEditorViewModel();
        viewModel.CombatParameters.Add(new CombatParameterViewModel { Id = "valid1" });
        viewModel.CombatParameters.Add(new CombatParameterViewModel { Id = "" }); // Invalid - empty ID

        // Act
        viewModel.ValidateAll();

        // Assert
        Assert.Contains("存在验证错误", viewModel.StatusMessage);
    }

    [Fact]
    public void ValidateAll_ShouldShowSuccessWhenAllValid()
    {
        // Arrange
        var viewModel = new TestableCombatParameterEditorViewModel();
        viewModel.CombatParameters.Clear();
        viewModel.CombatParameters.Add(new CombatParameterViewModel { Id = "valid1" });
        viewModel.CombatParameters.Add(new CombatParameterViewModel { Id = "valid2" });
        viewModel.Definitions.Clear();
        viewModel.Definitions.Add(new DefinitionViewModel { Name = "valid_def", Value = "1.0" });

        // Act
        viewModel.ValidateAll();

        // Assert
        Assert.Contains("所有数据验证通过", viewModel.StatusMessage);
    }

    // Testable class to access protected methods
    private class TestableCombatParameterEditorViewModel : CombatParameterEditorViewModel
    {
        public TestableCombatParameterEditorViewModel() : base()
        {
        }

        public TestableCombatParameterEditorViewModel(IValidationService validationService) : base(validationService)
        {
        }

        public bool TestItemMatchesFilter(CombatParameterViewModel item, string filterText)
        {
            return base.ItemMatchesFilter(item, filterText);
        }

        public CombatParameterViewModel TestCreateNewItemViewModel()
        {
            return base.CreateNewItemViewModel();
        }

        public CombatParameterViewModel TestDuplicateItemViewModel(CombatParameterViewModel source)
        {
            return base.DuplicateItemViewModel(source);
        }

        public BaseCombatParameterDO TestConvertToItemDO(CombatParameterViewModel viewModel)
        {
            return base.ConvertToItemDO(viewModel);
        }

        public CombatParameterViewModel TestConvertToItemViewModel(BaseCombatParameterDO itemDO)
        {
            return base.ConvertToItemViewModel(itemDO);
        }

        public CombatParametersDO TestConvertToRootDO(ObservableCollection<CombatParameterViewModel> items)
        {
            return base.ConvertToRootDO(items);
        }

        public ObservableCollection<CombatParameterViewModel> TestConvertFromRootDO(CombatParametersDO rootDO)
        {
            return base.ConvertFromRootDO(rootDO);
        }
    }
}