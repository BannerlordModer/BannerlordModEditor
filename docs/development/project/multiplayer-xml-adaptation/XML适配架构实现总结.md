# BannerlordModEditor项目XML适配架构实现总结

## 项目概述

本文档总结了在BannerlordModEditor项目中实现的XML适配架构，包括DO/DTO模式的应用、大型XML文件处理器的开发，以及Layouts文件的增强功能。

## 实现的功能模块

### 1. TerrainMaterials.xml的DO/DTO架构适配

**已实现文件：**
- `/BannerlordModEditor.Common/Models/DO/Engine/TerrainMaterialsDO.cs`
- `/BannerlordModEditor.Common/Models/DTO/Engine/TerrainMaterialsDTO.cs`
- `/BannerlordModEditor.Common/Mappers/TerrainMaterialsMapper.cs`

**核心特性：**
- 完整的地形材质系统数据模型
- 支持复杂嵌套结构（纹理、层标志、网格）
- 业务逻辑方法（IsFloraMaterial、HasDetailMaps等）
- 精确的XML序列化控制
- 大型文件处理支持

**关键业务逻辑：**
```csharp
// 业务逻辑方法
public bool IsFloraMaterial() => IsFloraLayer == "true";
public bool IsMeshBlendMaterial() => IsMeshBlendLayer == "true";
public bool HasDetailMaps() => BigDetailMapMode != "0";

// 序列化控制方法
public bool ShouldSerializeTextures() => HasTextures && Textures != null && Textures.TextureList.Count > 0;
public bool ShouldSerializeLayerFlags() => HasLayerFlags && LayerFlags != null && LayerFlags.FlagList.Count > 0;
```

### 2. MPClassDivisions.xml的DO/DTO架构适配

**已实现文件：**
- `/BannerlordModEditor.Common/Models/DO/Multiplayer/MPClassDivisionsDO.cs`
- `/BannerlordModEditor.Common/Models/DTO/Multiplayer/MPClassDivisionsDTO.cs`
- `/BannerlordModEditor.Common/Mappers/MPClassDivisionsMapper.cs`

**核心特性：**
- 多人游戏分类系统的完整数据模型
- 复杂的天赋系统（OnSpawnEffect、RandomOnSpawnEffect等）
- 游戏平衡性检查方法
- 按文化和游戏模式的索引
- 大型文件处理支持

**关键业务逻辑：**
```csharp
// 游戏平衡性检查
public bool IsBalanced()
{
    if (!HasValidCost()) return false;
    
    var costValue = decimal.Parse(Cost);
    var armorValue = GetArmorValue();
    var moveSpeed = GetMovementSpeedValue();
    
    // 简单的平衡性检查：高成本应该有相应的属性
    if (costValue > 150 && armorValue < 10) return false;
    if (costValue > 100 && moveSpeed < 0.8) return false;
    
    return true;
}

// 索引初始化
public void InitializeIndexes()
{
    ClassDivisionsByCulture.Clear();
    ClassDivisionsByGameMode.Clear();

    foreach (var division in MPClassDivisions)
    {
        var culture = ExtractCultureFromId(division.Id);
        // ... 索引逻辑
    }
}
```

### 3. 大型XML文件处理器

**已实现文件：**
- `/BannerlordModEditor.Common/Services/LargeXmlFileProcessor.cs`

**核心特性：**
- 分块处理大型XML文件
- 内存优化和垃圾回收
- 异步处理支持
- 文件大小检测和参数优化
- 错误处理和恢复机制

**关键功能：**
```csharp
// 处理大型地形材质文件
public async Task<TerrainMaterialsDO> ProcessLargeTerrainMaterialsFileAsync(string filePath)
{
    return await ProcessLargeFileAsync<TerrainMaterialsDO, TerrainMaterialDO>(
        filePath,
        "terrain_material",
        (materials, chunk) =>
        {
            materials.TerrainMaterialList.AddRange(chunk);
            materials.ProcessedChunks++;
        },
        DefaultChunkSize
    );
}

// 内存优化
private async Task CheckMemoryUsageAsync()
{
    var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
    var memoryUsedMB = currentProcess.WorkingSet64 / (1024 * 1024);
    
    if (memoryUsedMB > MaxMemoryUsageMB)
    {
        await Task.Run(() => GC.Collect());
        await Task.Delay(100);
    }
}
```

### 4. Layouts文件管理器

**已实现文件：**
- `/BannerlordModEditor.Common/Services/LayoutsManager.cs`

**核心特性：**
- Layouts文件的缓存管理
- 文件修改时间检测
- 批量操作支持
- 布局合并和克隆功能
- 验证和错误处理

**关键功能：**
```csharp
// 缓存管理
public LayoutsBaseDO LoadLayoutsFile(string filePath)
{
    var fileInfo = new FileInfo(filePath);
    
    // 检查缓存
    if (_layoutsCache.ContainsKey(filePath) && 
        _lastModifiedCache.ContainsKey(filePath) && 
        _lastModifiedCache[filePath] == fileInfo.LastWriteTime)
    {
        return _layoutsCache[filePath];
    }
    
    // 加载并缓存文件
    // ... 加载逻辑
}

// 布局合并
public LayoutsBaseDO MergeLayouts(LayoutsBaseDO primary, LayoutsBaseDO secondary)
{
    var merged = CloneLayouts(primary);
    
    // 合并布局逻辑
    foreach (var secondaryLayout in secondary.Layouts.LayoutList)
    {
        var existingLayout = merged.Layouts.LayoutList.FirstOrDefault(l => l.Class == secondaryLayout.Class);
        // ... 合并逻辑
    }
    
    return merged;
}
```

### 5. 测试更新

**已更新文件：**
- `/BannerlordModEditor.Common.Tests/TerrainMaterialsXmlTests.cs`
- `/BannerlordModEditor.Common.Tests/LargeXmlFileProcessorTests.cs`

**测试覆盖：**
- DO/DTO模型的基本功能
- XML序列化和反序列化
- 大型文件处理功能
- Layouts管理器的各种功能
- 错误处理和边界情况

## 架构优势

### 1. 性能优化
- 分块处理大型文件，减少内存占用
- 缓存机制提高重复加载性能
- 异步处理支持，提高响应性

### 2. 可维护性
- 清晰的DO/DTO分层架构
- 完善的业务逻辑封装
- 全面的错误处理机制

### 3. 可扩展性
- 通用的大型文件处理框架
- 易于添加新的XML类型支持
- 模块化的设计便于功能扩展

### 4. 数据完整性
- 精确的XML序列化控制
- 完整的验证机制
- 业务逻辑约束检查

## 使用示例

### 1. 加载和处理TerrainMaterials文件
```csharp
var processor = new LargeXmlFileProcessor();
var terrainMaterials = await processor.ProcessLargeTerrainMaterialsFileAsync("terrain_materials.xml");

// 使用业务逻辑
var floraMaterials = terrainMaterials.TerrainMaterialList.Where(m => m.IsFloraMaterial()).ToList();
```

### 2. 管理Layouts文件
```csharp
var manager = new LayoutsManager();

// 加载文件
var layouts = manager.LoadLayoutsFile("layouts.xml");

// 创建新的布局配置
var newLayouts = manager.CreateNewLayouts("CustomType");

// 合并布局
var mergedLayouts = manager.MergeLayouts(layouts1, layouts2);
```

### 3. 处理大型MPClassDivisions文件
```csharp
var processor = new LargeXmlFileProcessor();
var mpDivisions = await processor.ProcessLargeMPClassDivisionsFileAsync("mp_class_divisions.xml");

// 使用索引
var vlandiaClasses = mpDivisions.GetClassDivisionsByCulture("vlandia");
var skirmishClasses = mpDivisions.GetClassDivisionsByGameMode("skirmish");
```

## 未来改进方向

1. **性能优化**：实现并行处理和更智能的缓存策略
2. **功能扩展**：支持更多XML类型的自动化处理
3. **用户体验**：添加更详细的错误信息和处理进度
4. **监控功能**：实现处理性能的监控和分析

## 结论

通过实现DO/DTO架构模式、大型XML文件处理器和Layouts管理器，BannerlordModEditor项目现在具备了处理复杂XML文件的强大能力。这些实现不仅提高了代码的可维护性和可扩展性，还为未来的功能扩展奠定了坚实的基础。