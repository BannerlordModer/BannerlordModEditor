using Xunit;
using Moq;
using CommunityToolkit.Mvvm.ComponentModel;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.Common.Models;
using BannerlordModEditor.Common.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.IO;

namespace BannerlordModEditor.UI.Tests.Integration;

public class EditorIntegrationTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEditorFactory _editorFactory;
    private readonly IValidationService _validationService;
    private readonly IDataBindingService _dataBindingService;

    public EditorIntegrationTests()
    {
        // 设置依赖注入
        var services = new ServiceCollection();
        
        // 注册服务
        services.AddSingleton<IValidationService, ValidationService>();
        services.AddSingleton<IDataBindingService, DataBindingService>();
        services.AddSingleton<IEditorFactory, EditorFactory>();
        
        // 注册ViewModels
        services.AddTransient<AttributeEditorViewModel>();
        services.AddTransient<SkillEditorViewModel>();
        services.AddTransient<BoneBodyTypeEditorViewModel>();
        
        _serviceProvider = services.BuildServiceProvider();
        _validationService = _serviceProvider.GetRequiredService<IValidationService>();
        _dataBindingService = _serviceProvider.GetRequiredService<IDataBindingService>();
        _editorFactory = _serviceProvider.GetRequiredService<IEditorFactory>();
    }

    [Fact]
    public void EditorFactory_Should_Create_AttributeEditor()
    {
        // Arrange & Act
        var viewModel = _editorFactory.CreateEditorViewModel("AttributeEditor", "attributes.xml");

        // Assert
        Assert.NotNull(viewModel);
        Assert.IsType<AttributeEditorViewModel>(viewModel);
    }

    [Fact]
    public void EditorFactory_Should_Create_SkillEditor()
    {
        // Arrange & Act
        var viewModel = _editorFactory.CreateEditorViewModel("SkillEditor", "skills.xml");

        // Assert
        Assert.NotNull(viewModel);
        Assert.IsType<SkillEditorViewModel>(viewModel);
    }

    [Fact]
    public void AttributeEditor_Should_Initialize_With_Default_Attributes()
    {
        // Arrange & Act
        var viewModel = _serviceProvider.GetRequiredService<AttributeEditorViewModel>();

        // Assert
        Assert.NotNull(viewModel);
        Assert.NotNull(viewModel.Attributes);
        Assert.True(viewModel.Attributes.Count > 0);
        Assert.Equal("NewAttribute", viewModel.Attributes[0].Id);
        Assert.Equal("New Attribute", viewModel.Attributes[0].Name);
    }

    [Fact]
    public void SkillEditor_Should_Initialize_With_Default_Skills()
    {
        // Arrange & Act
        var viewModel = _serviceProvider.GetRequiredService<SkillEditorViewModel>();

        // Assert
        Assert.NotNull(viewModel);
        Assert.NotNull(viewModel.Skills);
        Assert.True(viewModel.Skills.Count > 0);
        Assert.Equal("NewSkill", viewModel.Skills[0].Id);
        Assert.Equal("New Skill", viewModel.Skills[0].Name);
    }

    [Fact]
    public void ValidationService_Should_Validate_AttributeData()
    {
        // Arrange
        var validAttribute = new AttributeDataViewModel
        {
            Id = "TestAttribute",
            Name = "Test Attribute",
            DefaultValue = "10"
        };

        var invalidAttribute = new AttributeDataViewModel
        {
            Id = "",
            Name = "Test Attribute",
            DefaultValue = "10"
        };

        // Act
        var validResult = _validationService.Validate(validAttribute);
        var invalidResult = _validationService.Validate(invalidAttribute);

        // Assert
        Assert.True(validResult.IsValid);
        Assert.False(invalidResult.IsValid);
        Assert.Contains("ID不能为空", invalidResult.Errors);
    }

    [Fact]
    public void DataBindingService_Should_Create_TwoWay_Binding()
    {
        // Arrange
        var source = new TestObservableObject();
        var target = new TestObservableObject();

        // Act
        using var binding = _dataBindingService.CreateBinding<TestObservableObject, TestObservableObject>(
            source, "Name",
            target, "Name",
            true);

        source.Name = "Test Name";

        // Assert
        Assert.Equal("Test Name", target.Name);
    }

    [Fact]
    public void DataBindingService_Should_Create_Collection_Binding()
    {
        // Arrange
        var source = new ObservableCollection<string> { "Item1", "Item2" };
        var target = new ObservableCollection<string>();

        // Act
        using var binding = _dataBindingService.CreateCollectionBinding(source, target);

        // Assert
        Assert.Equal(2, target.Count);
        Assert.Contains("Item1", target);
        Assert.Contains("Item2", target);

        // Test updates
        source.Add("Item3");
        Assert.Equal(3, target.Count);
        Assert.Contains("Item3", target);
    }

    [Fact]
    public void EditorIntegration_Should_Work_With_Dependency_Injection()
    {
        // Arrange
        var editorManager = new EditorManagerViewModel(_editorFactory);

        // Act
        var attributeEditor = editorManager.Categories
            .SelectMany(c => c.Editors)
            .FirstOrDefault(e => e.EditorType == "AttributeEditor");

        editorManager.SelectEditorCommand.Execute(attributeEditor);

        // Assert
        Assert.NotNull(editorManager.CurrentEditorViewModel);
        Assert.IsType<AttributeEditorViewModel>(editorManager.CurrentEditorViewModel);
        Assert.Contains("属性定义", editorManager.CurrentBreadcrumb);
    }

    [Fact]
    public void CrossEditorCommunication_Should_Work()
    {
        // Arrange
        var attributeEditor = _serviceProvider.GetRequiredService<AttributeEditorViewModel>();
        var skillEditor = _serviceProvider.GetRequiredService<SkillEditorViewModel>();

        // Act - 测试编辑器之间的通信
        attributeEditor.LoadXmlFile("attributes.xml");
        skillEditor.LoadXmlFile("skills.xml");

        // Assert
        Assert.NotNull(attributeEditor.FilePath);
        Assert.NotNull(skillEditor.FilePath);
        Assert.False(attributeEditor.HasUnsavedChanges);
        Assert.False(skillEditor.HasUnsavedChanges);
    }

    [Fact]
    public void ServiceComposition_Should_Be_Consistent()
    {
        // Arrange & Act
        var service1 = _serviceProvider.GetRequiredService<IValidationService>();
        var service2 = _serviceProvider.GetRequiredService<IValidationService>();
        var factory1 = _serviceProvider.GetRequiredService<IEditorFactory>();
        var factory2 = _serviceProvider.GetRequiredService<IEditorFactory>();

        // Assert - 确保服务是单例
        Assert.Same(service1, service2);
        Assert.Same(factory1, factory2);
    }

    [Fact]
    public void ErrorHandling_Should_Be_Graceful()
    {
        // Arrange
        var viewModel = _serviceProvider.GetRequiredService<AttributeEditorViewModel>();

        // Act - 尝试加载不存在的文件
        viewModel.LoadXmlFile("non_existent_file.xml");

        // Assert
        Assert.NotNull(viewModel);
        Assert.NotEmpty(viewModel.Attributes);
        Assert.Contains("未找到", viewModel.StatusMessage);
        Assert.False(viewModel.HasUnsavedChanges);
    }

    // Test helper class
    private class TestObservableObject : ObservableObject
    {
        private string _name = string.Empty;
        private int _value = 0;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}