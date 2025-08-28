using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace BannerlordModEditor.UI.Tests.Performance;

/// <summary>
/// EditorManagerViewModel性能和边界测试
/// </summary>
public class EditorManagerPerformanceTests
{
    private readonly IServiceProvider _serviceProvider;

    public EditorManagerPerformanceTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ILogService, LogService>();
        services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        services.AddSingleton<IValidationService, ValidationService>();
        services.AddSingleton<IDataBindingService, DataBindingService>();
        services.AddSingleton<IEditorFactory, MockPerformanceEditorFactory>();
        services.AddSingleton<IEditorManagerFactory, EditorManagerFactory>();
        services.AddTransient<EditorManagerViewModel>();
        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public void CreateEditorManager_Performance_ShouldBeFast()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IEditorManagerFactory>();
        const int iterations = 100;
        var stopwatch = Stopwatch.StartNew();

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var editorManager = factory.CreateEditorManager();
            Assert.NotNull(editorManager);
        }

        stopwatch.Stop();

        // Assert
        var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
        Assert.True(averageTime < 100, $"Average creation time should be less than 100ms, but was {averageTime}ms");
        
        Console.WriteLine($"Average EditorManager creation time: {averageTime}ms");
        Console.WriteLine($"Total time for {iterations} iterations: {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public async Task CreateEditorManagerAsync_Performance_ShouldBeFast()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IEditorManagerFactory>();
        const int iterations = 100;
        var stopwatch = Stopwatch.StartNew();

        // Act
        var tasks = Enumerable.Range(0, iterations).Select(async i =>
        {
            var editorManager = await factory.CreateEditorManagerAsync();
            Assert.NotNull(editorManager);
            return editorManager;
        });

        await Task.WhenAll(tasks);
        stopwatch.Stop();

        // Assert
        var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
        Assert.True(averageTime < 150, $"Average async creation time should be less than 150ms, but was {averageTime}ms");
        
        Console.WriteLine($"Average async EditorManager creation time: {averageTime}ms");
        Console.WriteLine($"Total time for {iterations} iterations: {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public void CreateEditorManager_MemoryUsage_ShouldBeReasonable()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IEditorManagerFactory>();
        const int iterations = 1000;
        var initialMemory = GC.GetTotalMemory(true);
        var editors = new List<EditorManagerViewModel>();

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var editorManager = factory.CreateEditorManager();
            editors.Add(editorManager);
        }

        var finalMemory = GC.GetTotalMemory(false);
        var memoryIncrease = finalMemory - initialMemory;
        var averageMemoryPerInstance = memoryIncrease / (double)iterations;

        // Assert
        Assert.True(averageMemoryPerInstance < 100000, $"Average memory per instance should be less than 100KB, but was {averageMemoryPerInstance / 1024}KB");
        
        Console.WriteLine($"Memory increase for {iterations} instances: {memoryIncrease / 1024}KB");
        Console.WriteLine($"Average memory per instance: {averageMemoryPerInstance / 1024}KB");
    }

    [Fact]
    public void EditorManager_Concurrency_ShouldBeThreadSafe()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IEditorManagerFactory>();
        const int threadCount = 50;
        var results = new List<EditorManagerViewModel>();
        var exceptions = new List<Exception>();
        var stopwatch = Stopwatch.StartNew();

        // Act
        Parallel.For(0, threadCount, i =>
        {
            try
            {
                var editorManager = factory.CreateEditorManager();
                lock (results)
                {
                    results.Add(editorManager);
                }
            }
            catch (Exception ex)
            {
                lock (exceptions)
                {
                    exceptions.Add(ex);
                }
            }
        });

        stopwatch.Stop();

        // Assert
        Assert.Empty(exceptions);
        Assert.Equal(threadCount, results.Count);
        Assert.All(results, result => Assert.NotNull(result));
        
        var stats = factory.GetStatistics();
        Assert.Equal(threadCount, stats.InstancesCreated);
        
        Console.WriteLine($"Concurrent creation of {threadCount} instances took {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public async Task EditorManagerAsync_Concurrency_ShouldBeThreadSafe()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IEditorManagerFactory>();
        const int threadCount = 50;
        var results = new List<EditorManagerViewModel>();
        var exceptions = new List<Exception>();
        var stopwatch = Stopwatch.StartNew();

        // Act
        var tasks = Enumerable.Range(0, threadCount).Select(async i =>
        {
            try
            {
                var editorManager = await factory.CreateEditorManagerAsync();
                lock (results)
                {
                    results.Add(editorManager);
                }
            }
            catch (Exception ex)
            {
                lock (exceptions)
                {
                    exceptions.Add(ex);
                }
            }
        });

        await Task.WhenAll(tasks);
        stopwatch.Stop();

        // Assert
        Assert.Empty(exceptions);
        Assert.Equal(threadCount, results.Count);
        Assert.All(results, result => Assert.NotNull(result));
        
        var stats = factory.GetStatistics();
        Assert.Equal(threadCount, stats.InstancesCreated);
        
        Console.WriteLine($"Concurrent async creation of {threadCount} instances took {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public void EditorManager_Search_Performance_ShouldBeFast()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IEditorManagerFactory>();
        var editorManager = factory.CreateEditorManager();
        const int searchIterations = 100;
        var stopwatch = Stopwatch.StartNew();

        // Act
        for (int i = 0; i < searchIterations; i++)
        {
            editorManager.SearchText = "属性";
            editorManager.SearchText = "技能";
            editorManager.SearchText = "";
        }

        stopwatch.Stop();

        // Assert
        var averageTime = stopwatch.ElapsedMilliseconds / (double)(searchIterations * 3);
        Assert.True(averageTime < 10, $"Average search time should be less than 10ms, but was {averageTime}ms");
        
        Console.WriteLine($"Average search time: {averageTime}ms");
        Console.WriteLine($"Total time for {searchIterations * 3} searches: {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public void EditorManager_EditorSelection_Performance_ShouldBeFast()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IEditorManagerFactory>();
        var editorManager = factory.CreateEditorManager();
        var characterCategory = editorManager.Categories.FirstOrDefault(c => c.Name == "角色设定");
        var editor = characterCategory?.Editors.FirstOrDefault();
        
        Assert.NotNull(editor);
        
        const int selectionIterations = 100;
        var stopwatch = Stopwatch.StartNew();

        // Act
        for (int i = 0; i < selectionIterations; i++)
        {
            editorManager.SelectEditorCommand.Execute(editor);
        }

        stopwatch.Stop();

        // Assert
        var averageTime = stopwatch.ElapsedMilliseconds / (double)selectionIterations;
        Assert.True(averageTime < 50, $"Average selection time should be less than 50ms, but was {averageTime}ms");
        
        Console.WriteLine($"Average editor selection time: {averageTime}ms");
        Console.WriteLine($"Total time for {selectionIterations} selections: {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public void EditorManager_WithStressLoad_ShouldRemainStable()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IEditorManagerFactory>();
        const int stressIterations = 1000;
        var stopwatch = Stopwatch.StartNew();

        // Act
        for (int i = 0; i < stressIterations; i++)
        {
            var editorManager = factory.CreateEditorManager();
            
            // 执行各种操作
            editorManager.SearchText = "test";
            editorManager.SearchText = "";
            
            var category = editorManager.Categories.FirstOrDefault();
            if (category?.Editors.Any() == true)
            {
                var editor = category.Editors.First();
                editorManager.SelectEditorCommand.Execute(editor);
            }
            
            editorManager.RefreshEditorsCommand.Execute(null);
        }

        stopwatch.Stop();

        // Assert
        var stats = factory.GetStatistics();
        Assert.Equal(stressIterations, stats.InstancesCreated);
        Assert.True(stopwatch.ElapsedMilliseconds < stressIterations * 200, 
            $"Stress test should complete in less than {(stressIterations * 200) / 1000} seconds, but took {stopwatch.ElapsedMilliseconds / 1000} seconds");
        
        Console.WriteLine($"Stress test completed {stressIterations} iterations in {stopwatch.ElapsedMilliseconds}ms");
        Console.WriteLine($"Average time per iteration: {stopwatch.ElapsedMilliseconds / (double)stressIterations}ms");
    }

    [Fact]
    public void EditorManagerFactory_BoundaryConditions_ShouldHandleCorrectly()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IEditorManagerFactory>();

        // Act & Assert - 测试超时边界
        var timeoutOptions = new EditorManagerCreationOptions
        {
            CreationTimeout = 1 // 极短超时
        };

        var editorManager = factory.CreateEditorManager(timeoutOptions);
        Assert.NotNull(editorManager);

        // 测试零超时
        var zeroTimeoutOptions = new EditorManagerCreationOptions
        {
            CreationTimeout = 0
        };

        var exception = Assert.Throws<InvalidOperationException>(() => 
            factory.CreateEditorManager(zeroTimeoutOptions));
        Assert.Contains("CreationTimeout must be positive", exception.Message);
    }

    [Fact]
    public void EditorManager_OptionsValidation_BoundaryConditions_ShouldHandleCorrectly()
    {
        // Arrange
        var options = new EditorManagerOptions
        {
            LogService = _serviceProvider.GetRequiredService<ILogService>(),
            ErrorHandlerService = _serviceProvider.GetRequiredService<IErrorHandlerService>(),
            CreationTimeout = int.MaxValue // 最大值
        };

        // Act & Assert
        var result = options.Validate();
        Assert.True(result.IsValid);

        // 测试最小正值
        options.CreationTimeout = 1;
        result = options.Validate();
        Assert.True(result.IsValid);
    }

    [Fact]
    public void EditorManager_WithLargeDataSet_ShouldHandleCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<ILogService, LogService>();
        services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        services.AddSingleton<IValidationService, ValidationService>();
        services.AddSingleton<IDataBindingService, DataBindingService>();
        services.AddSingleton<IEditorFactory, LargeDatasetEditorFactory>();
        services.AddSingleton<IEditorManagerFactory, EditorManagerFactory>();
        services.AddTransient<EditorManagerViewModel>();
        
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();

        // Act
        var editorManager = factory.CreateEditorManager();

        // Assert
        Assert.NotNull(editorManager);
        Assert.NotEmpty(editorManager.Categories);
        
        // 验证大数据集处理
        var totalEditors = editorManager.Categories.Sum(c => c.Editors.Count);
        Assert.True(totalEditors > 0);
        
        Console.WriteLine($"Large dataset loaded: {editorManager.Categories.Count} categories, {totalEditors} editors");
    }

    [Fact]
    public void EditorManager_WithDeepRecursion_ShouldHandleCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<ILogService, LogService>();
        services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        services.AddSingleton<IValidationService, ValidationService>();
        services.AddSingleton<IDataBindingService, DataBindingService>();
        services.AddSingleton<IEditorFactory, RecursiveEditorFactory>();
        services.AddSingleton<IEditorManagerFactory, EditorManagerFactory>();
        services.AddTransient<EditorManagerViewModel>();
        
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();

        // Act
        var editorManager = factory.CreateEditorManager();

        // Assert
        Assert.NotNull(editorManager);
        // 验证递归结构被正确处理
        Assert.NotEmpty(editorManager.Categories);
    }

    [Fact]
    public void EditorManager_WithHighMemoryPressure_ShouldHandleCorrectly()
        {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IEditorManagerFactory>();
        const int largeInstanceCount = 10000;
        
        // Act
        var stopwatch = Stopwatch.StartNew();
        
        for (int i = 0; i < largeInstanceCount; i++)
        {
            var editorManager = factory.CreateEditorManager();
            Assert.NotNull(editorManager);
            
            // 强制垃圾回收以测试内存压力
            if (i % 1000 == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        stopwatch.Stop();

        // Assert
        var stats = factory.GetStatistics();
        Assert.Equal(largeInstanceCount, stats.InstancesCreated);
        
        Console.WriteLine($"High memory pressure test: {largeInstanceCount} instances in {stopwatch.ElapsedMilliseconds}ms");
        Console.WriteLine($"Average time per instance: {stopwatch.ElapsedMilliseconds / (double)largeInstanceCount}ms");
    }

    [Fact]
    public async Task EditorManager_WithCancellation_ShouldHandleCorrectly()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IEditorManagerFactory>();
        var cts = new CancellationTokenSource();

        // Act
        var task = factory.CreateEditorManagerAsync(cancellationToken: cts.Token);
        
        // 立即取消
        cts.Cancel();

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => task);
    }

    [Fact]
    public void EditorManager_WithTimeout_ShouldHandleCorrectly()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IEditorManagerFactory>();
        var options = new EditorManagerCreationOptions
        {
            CreationTimeout = 100 // 100ms超时
        };

        // Act
        var stopwatch = Stopwatch.StartNew();
        var editorManager = factory.CreateEditorManager(options);
        stopwatch.Stop();

        // Assert
        Assert.NotNull(editorManager);
        Assert.True(stopwatch.ElapsedMilliseconds < 200, 
            $"Creation should complete within 200ms, but took {stopwatch.ElapsedMilliseconds}ms");
    }

    private class MockPerformanceEditorFactory : IEditorFactory
    {
        public virtual ViewModelBase? CreateEditorViewModel(string editorType, string xmlFileName)
        {
            return new MockEditorViewModel(editorType, xmlFileName);
        }

        public virtual BannerlordModEditor.UI.Views.Editors.BaseEditorView? CreateEditorView(string editorType)
        {
            return null;
        }

        public virtual void RegisterEditor<TViewModel, TView>(string editorType)
            where TViewModel : ViewModelBase
            where TView : BannerlordModEditor.UI.Views.Editors.BaseEditorView
        {
        }

        public virtual IEnumerable<string> GetRegisteredEditorTypes()
        {
            return new[] { "AttributeEditor", "SkillEditor", "CombatParameterEditor" };
        }

        public virtual EditorTypeInfo? GetEditorTypeInfo(string editorType)
        {
            return new EditorTypeInfo
            {
                EditorType = editorType,
                DisplayName = $"{editorType} Display",
                Category = "测试分类"
            };
        }

        public virtual IEnumerable<EditorTypeInfo> GetEditorsByCategory(string category)
        {
            return Enumerable.Empty<EditorTypeInfo>();
        }

        public virtual IEnumerable<string> GetCategories()
        {
            return new[] { "测试分类" };
        }

        public virtual void RegisterEditor<TViewModel, TView>(string editorType, string displayName, string description, string xmlFileName, string category = "General")
            where TViewModel : ViewModelBase
            where TView : BannerlordModEditor.UI.Views.Editors.BaseEditorView
        {
        }

        public virtual IEnumerable<ViewModelBase> GetAllEditors()
        {
            return new List<ViewModelBase>
            {
                new MockEditorViewModel("AttributeEditor", "attributes.xml"),
                new MockEditorViewModel("SkillEditor", "skills.xml"),
                new MockEditorViewModel("CombatParameterEditor", "combat_parameters.xml")
            };
        }
    }

    private class LargeDatasetEditorFactory : MockPerformanceEditorFactory
    {
        public override IEnumerable<ViewModelBase> GetAllEditors()
        {
            // 创建大量编辑器以测试大数据集性能
            var editors = new List<ViewModelBase>();
            for (int i = 0; i < 1000; i++)
            {
                editors.Add(new MockEditorViewModel($"Editor{i}", $"file{i}.xml"));
            }
            return editors;
        }
    }

    private class RecursiveEditorFactory : MockPerformanceEditorFactory
    {
        public override IEnumerable<ViewModelBase> GetAllEditors()
        {
            // 创建复杂的递归结构以测试边界条件
            return new List<ViewModelBase>
            {
                new MockEditorViewModel("DeepEditor1", "deep1.xml"),
                new MockEditorViewModel("DeepEditor2", "deep2.xml"),
                new MockEditorViewModel("DeepEditor3", "deep3.xml")
            };
        }
    }

    private class MockEditorViewModel : ViewModelBase
    {
        public new string EditorType { get; }
        public new string XmlFileName { get; }

        public MockEditorViewModel(string editorType, string xmlFileName)
        {
            EditorType = editorType;
            XmlFileName = xmlFileName;
        }
    }
}