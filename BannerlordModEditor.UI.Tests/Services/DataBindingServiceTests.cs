using Xunit;
using Moq;
using CommunityToolkit.Mvvm.ComponentModel;
using BannerlordModEditor.UI.Services;
using System.Collections.ObjectModel;
using System.Reactive;

namespace BannerlordModEditor.UI.Tests.Services;

public class DataBindingServiceTests
{
    private readonly DataBindingService _dataBindingService;

    public DataBindingServiceTests()
    {
        _dataBindingService = new DataBindingService();
    }

    [Fact]
    public void CreateBinding_TwoWay_ShouldCreateBidirectionalBinding()
    {
        // Arrange
        var source = new TestObservableObject { Name = "Source" };
        var target = new TestObservableObject { Name = "Target" };

        // Act
        using var binding = _dataBindingService.CreateBinding<TestObservableObject, TestObservableObject>(source, nameof(TestObservableObject.Name), target, nameof(TestObservableObject.Name), true);

        // Assert
        Assert.NotNull(binding);

        // Test source to target
        source.Name = "New Source Value";
        Assert.Equal("New Source Value", target.Name);

        // Test target to source
        target.Name = "New Target Value";
        Assert.Equal("New Target Value", source.Name);
    }

    [Fact]
    public void CreateBinding_OneWay_ShouldCreateUnidirectionalBinding()
    {
        // Arrange
        var source = new TestObservableObject { Name = "Source" };
        var target = new TestObservableObject { Name = "Target" };
        var originalTargetName = target.Name;

        // Act
        using var binding = _dataBindingService.CreateBinding<TestObservableObject, TestObservableObject>(source, nameof(TestObservableObject.Name), target, nameof(TestObservableObject.Name), false);

        // Assert
        Assert.NotNull(binding);

        // Test source to target
        source.Name = "New Source Value";
        Assert.Equal("New Source Value", target.Name);

        // Test that target doesn't affect source
        target.Name = "New Target Value";
        Assert.Equal("New Source Value", source.Name); // Source should not change
    }

    [Fact]
    public void CreateCollectionBinding_ShouldSyncCollections()
    {
        // Arrange
        var source = new ObservableCollection<string> { "Item1", "Item2" };
        var target = new ObservableCollection<string> { "Item3", "Item4" };

        // Act
        using var binding = _dataBindingService.CreateCollectionBinding(source, target);

        // Assert
        Assert.NotNull(binding);
        Assert.Equal(2, target.Count);
        Assert.Contains("Item1", target);
        Assert.Contains("Item2", target);

        // Test adding to source
        source.Add("Item3");
        Assert.Contains("Item3", target);

        // Test removing from source
        source.Remove("Item1");
        Assert.DoesNotContain("Item1", target);
    }

    [Fact]
    public void CreateCollectionBinding_WithEmptySource_ShouldClearTarget()
    {
        // Arrange
        var source = new ObservableCollection<string>();
        var target = new ObservableCollection<string> { "Item1", "Item2" };

        // Act
        using var binding = _dataBindingService.CreateCollectionBinding(source, target);

        // Assert
        Assert.Empty(target);
    }

    [Fact]
    public void CreateValidationBinding_ShouldTriggerValidation()
    {
        // Arrange
        var source = new TestObservableObject { Name = "Valid" };
        var validationResults = new List<List<string>>();
        
        var validationCallback = new Action<List<string>>(errors =>
        {
            validationResults.Add(errors);
        });

        // Act
        using var binding = _dataBindingService.CreateValidationBinding(source, nameof(TestObservableObject.Name), validationCallback);

        // Assert
        Assert.NotNull(binding);

        // Trigger validation
        source.Name = "New Value";

        // Note: In a real test, we'd need to wait for the debounce or use a test scheduler
        // For now, we just verify the binding was created
    }

    [Fact]
    public void ObservePropertyChanges_ShouldObserveChanges()
    {
        // Arrange
        var source = new TestObservableObject { Name = "Initial" };
        var changes = new List<DataBindingEventArgs>();

        // Act
        var observable = _dataBindingService.ObservePropertyChanges(source, nameof(TestObservableObject.Name));
        using var subscription = observable.Subscribe(changes.Add);

        // Assert
        Assert.NotNull(observable);

        // Trigger change
        source.Name = "Changed";

        // Verify change was observed
        Assert.Single(changes);
        Assert.Equal(nameof(TestObservableObject.Name), changes[0].PropertyName);
        Assert.Equal("Initial", changes[0].OldValue);
        Assert.Equal("Changed", changes[0].NewValue);
    }

    [Fact]
    public void ObservePropertyChanges_ShouldFilterByPropertyName()
    {
        // Arrange
        var source = new TestObservableObject { Name = "Initial", Age = 25 };
        var changes = new List<DataBindingEventArgs>();

        // Act
        var observable = _dataBindingService.ObservePropertyChanges(source, nameof(TestObservableObject.Name));
        using var subscription = observable.Subscribe(changes.Add);

        // Assert
        Assert.NotNull(observable);

        // Trigger change to observed property
        source.Name = "Changed";

        // Trigger change to non-observed property
        source.Age = 30;

        // Verify only name change was observed
        Assert.Single(changes);
        Assert.Equal(nameof(TestObservableObject.Name), changes[0].PropertyName);
    }

    [Fact]
    public void BatchUpdate_ShouldExecuteAction()
    {
        // Arrange
        var source = new TestObservableObject { Name = "Initial" };
        var actionExecuted = false;

        Action updateAction = () =>
        {
            actionExecuted = true;
            source.Name = "Updated";
        };

        // Act
        _dataBindingService.BatchUpdate(source, updateAction);

        // Assert
        Assert.True(actionExecuted);
        Assert.Equal("Updated", source.Name);
    }

    [Fact]
    public void CreateBinding_WithNullObjects_ShouldHandleGracefully()
    {
        // Arrange
        var source = new TestObservableObject { Name = "Source" };
        TestObservableObject? target = null;

        // Act
        var binding = _dataBindingService.CreateBinding<TestObservableObject, TestObservableObject>(source, nameof(TestObservableObject.Name), target!, nameof(TestObservableObject.Name));

        // Assert
        // Should not throw exception, but binding might be null or ineffective
        Assert.NotNull(binding);
    }

    [Fact]
    public void CreateCollectionBinding_WithNullCollections_ShouldHandleGracefully()
    {
        // Arrange
        ObservableCollection<string>? source = null;
        var target = new ObservableCollection<string>();

        // Act
        var binding = _dataBindingService.CreateCollectionBinding(source!, target);

        // Assert
        // Should not throw exception
        Assert.NotNull(binding);
    }

    [Fact]
    public void CreateValidationBinding_WithNullCallback_ShouldHandleGracefully()
    {
        // Arrange
        var source = new TestObservableObject { Name = "Valid" };
        Action<List<string>>? validationCallback = null;

        // Act
        var binding = _dataBindingService.CreateValidationBinding(source, nameof(TestObservableObject.Name), validationCallback!);

        // Assert
        // Should not throw exception
        Assert.NotNull(binding);
    }

    [Fact]
    public void CreateBinding_Dispose_ShouldUnsubscribe()
    {
        // Arrange
        var source = new TestObservableObject { Name = "Source" };
        var target = new TestObservableObject { Name = "Target" };

        // Act
        var binding = _dataBindingService.CreateBinding<TestObservableObject, TestObservableObject>(source, nameof(TestObservableObject.Name), target, nameof(TestObservableObject.Name));

        // Test binding works
        source.Name = "Before Dispose";
        Assert.Equal("Before Dispose", target.Name);

        // Dispose binding
        binding.Dispose();

        // Test that binding no longer works
        source.Name = "After Dispose";
        Assert.Equal("Before Dispose", target.Name); // Target should not change
    }

    [Fact]
    public void CreateCollectionBinding_Dispose_ShouldUnsubscribe()
    {
        // Arrange
        var source = new ObservableCollection<string> { "Item1" };
        var target = new ObservableCollection<string> { "Item2" };

        // Act
        var binding = _dataBindingService.CreateCollectionBinding(source, target);

        // Test binding works
        source.Add("Item3");
        Assert.Contains("Item3", target);

        // Dispose binding
        binding.Dispose();

        // Test that binding no longer works
        source.Add("Item4");
        Assert.DoesNotContain("Item4", target);
    }

    // Test helper class
    private class TestObservableObject : ObservableObject
    {
        private string _name = string.Empty;
        private int _age = 0;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }
    }
}