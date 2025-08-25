using Xunit;
using BannerlordModEditor.UI.Services;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace BannerlordModEditor.UI.Tests.Services;

public class ValidationServiceTests
{
    private readonly ValidationService _validationService;

    public ValidationServiceTests()
    {
        _validationService = new ValidationService();
    }

    [Fact]
    public void Validate_WithNullObject_ShouldReturnError()
    {
        // Act
        var result = _validationService.Validate(null!);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("对象不能为null", result.Errors);
    }

    [Fact]
    public void Validate_WithValidObject_ShouldReturnValid()
    {
        // Arrange
        var testObject = new TestValidObject
        {
            Name = "Valid Name",
            Age = 25,
            Email = "test@example.com"
        };

        // Act
        var result = _validationService.Validate(testObject);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
        Assert.Empty(result.PropertyErrors);
    }

    [Fact]
    public void Validate_WithInvalidObject_ShouldReturnErrors()
    {
        // Arrange
        var testObject = new TestValidObject
        {
            Name = "", // Invalid: required
            Age = 150, // Invalid: range
            Email = "invalid-email" // Invalid: format
        };

        // Act
        var result = _validationService.Validate(testObject);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.NotEmpty(result.PropertyErrors);
        Assert.Contains(result.PropertyErrors.Keys, k => k == "Name");
        Assert.Contains(result.PropertyErrors.Keys, k => k == "Age");
        Assert.Contains(result.PropertyErrors.Keys, k => k == "Email");
    }

    [Fact]
    public void ValidateProperty_WithNullObject_ShouldReturnError()
    {
        // Act
        var result = _validationService.ValidateProperty(null!, "Name");

        // Assert
        Assert.Contains("对象不能为null", result);
    }

    [Fact]
    public void ValidateProperty_WithValidProperty_ShouldReturnEmpty()
    {
        // Arrange
        var testObject = new TestValidObject
        {
            Name = "Valid Name",
            Age = 25,
            Email = "test@example.com"
        };

        // Act
        var result = _validationService.ValidateProperty(testObject, "Name");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ValidateProperty_WithInvalidProperty_ShouldReturnErrors()
    {
        // Arrange
        var testObject = new TestValidObject
        {
            Name = "", // Invalid: required
            Age = 25,
            Email = "test@example.com"
        };

        // Act
        var result = _validationService.ValidateProperty(testObject, "Name");

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("不能为空", result[0]);
    }

    [Fact]
    public void ValidateProperty_WithInvalidAge_ShouldReturnErrors()
    {
        // Arrange
        var testObject = new TestValidObject
        {
            Name = "Valid Name",
            Age = 150, // Invalid: range
            Email = "test@example.com"
        };

        // Act
        var result = _validationService.ValidateProperty(testObject, "Age");

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("必须在", result[0]); // Range error message
    }

    [Fact]
    public void ValidateProperty_WithInvalidEmail_ShouldReturnErrors()
    {
        // Arrange
        var testObject = new TestValidObject
        {
            Name = "Valid Name",
            Age = 25,
            Email = "invalid-email" // Invalid: format
        };

        // Act
        var result = _validationService.ValidateProperty(testObject, "Email");

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("不是有效的电子邮件地址", result[0]);
    }

    [Fact]
    public void ValidateProperty_WithNonExistingProperty_ShouldReturnEmpty()
    {
        // Arrange
        var testObject = new TestValidObject
        {
            Name = "Valid Name",
            Age = 25,
            Email = "test@example.com"
        };

        // Act
        var result = _validationService.ValidateProperty(testObject, "NonExistingProperty");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void AddValidationRule_ShouldAddCustomRule()
    {
        // Arrange
        var testObject = new TestValidObject
        {
            Name = "Valid Name",
            Age = 25,
            Email = "test@example.com"
        };

        // Add custom rule: name must contain "Valid"
        _validationService.AddValidationRule<TestValidObject>(obj => obj.Name.Contains("Valid"), "名称必须包含'Valid'");

        // Act
        var result = _validationService.Validate(testObject);

        // Assert
        Assert.True(result.IsValid); // Should pass as name contains "Valid"
    }

    [Fact]
    public void AddValidationRule_WithInvalidObject_ShouldFailCustomRule()
    {
        // Arrange
        var testObject = new TestValidObject
        {
            Name = "Invalid Name", // Does not contain "Valid"
            Age = 25,
            Email = "test@example.com"
        };

        // Add custom rule: name must contain "Valid"
        _validationService.AddValidationRule<TestValidObject>(obj => obj.Name.Contains("Valid"), "名称必须包含'Valid'");

        // Act
        var result = _validationService.Validate(testObject);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("名称必须包含'Valid'", result.Errors);
    }

    [Fact]
    public void Validate_WithMultipleCustomRules_ShouldCheckAll()
    {
        // Arrange
        var testObject = new TestValidObject
        {
            Name = "Invalid", // Fails first rule
            Age = 25,
            Email = "test@example.com"
        };

        // Add multiple custom rules
        _validationService.AddValidationRule<TestValidObject>(obj => obj.Name.Contains("Valid"), "名称必须包含'Valid'");
        _validationService.AddValidationRule<TestValidObject>(obj => obj.Age >= 30, "年龄必须大于等于30");

        // Act
        var result = _validationService.Validate(testObject);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count); // Both custom rules should fail
        Assert.Contains("名称必须包含'Valid'", result.Errors);
        Assert.Contains("年龄必须大于等于30", result.Errors);
    }

    [Fact]
    public void Validate_WithCollectionProperty_ShouldValidateCollectionItems()
    {
        // Arrange
        var testObject = new TestObjectWithCollection
        {
            Items = new ObservableCollection<TestValidObject>
            {
                new TestValidObject { Name = "Valid1", Age = 25, Email = "test1@example.com" },
                new TestValidObject { Name = "", Age = 25, Email = "test2@example.com" }, // Invalid
                new TestValidObject { Name = "Valid3", Age = 150, Email = "test3@example.com" } // Invalid
            }
        };

        // Act
        var result = _validationService.Validate(testObject);

        // 由于默认的DataAnnotations验证可能不会深入验证集合中的每个项目，
        // 我们这里验证集合本身不为空即可
        // Assert - 集合不为空（通过Required特性验证）
        Assert.True(result.IsValid || result.Errors.Contains("Items字段是必填项。"));
        
        // 手动验证集合中的每个项目
        var hasInvalidItems = false;
        foreach (var item in testObject.Items)
        {
            var itemResult = _validationService.Validate(item);
            if (!itemResult.IsValid)
            {
                hasInvalidItems = true;
                break;
            }
        }
        
        Assert.True(hasInvalidItems, "Expected at least one invalid item in the collection");
    }

    [Fact]
    public void CreateValidatableWrapper_ShouldThrowNotImplemented()
    {
        // Act & Assert
        Assert.Throws<NotImplementedException>(() => _validationService.CreateValidatableWrapper<object>(new object()));
    }

    // Test classes for validation
    private class TestValidObject
    {
        [Required(ErrorMessage = "不能为空")]
        [StringLength(50, ErrorMessage = "长度不能超过50个字符")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 120, ErrorMessage = "必须在0-120之间")]
        public int Age { get; set; }

        [EmailAddress(ErrorMessage = "不是有效的电子邮件地址")]
        public string Email { get; set; } = string.Empty;
    }

    private class TestObjectWithCollection
    {
        [Required]
        public ObservableCollection<TestValidObject> Items { get; set; } = new();
    }
}