using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO.Engine;
using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Models.DO.Multiplayer;
using BannerlordModEditor.Common.Services;
using Xunit;
using LayoutsItemDO = BannerlordModEditor.Common.Models.DO.Layouts.ItemDO;

namespace BannerlordModEditor.Common.Tests
{
    public class LargeXmlFileProcessorTests
    {
        [Fact]
        public async Task LargeXmlFileProcessor_GetFileSizeInfo_ShouldReturnCorrectInfo()
        {
            // Arrange
            var processor = new LargeXmlFileProcessor();
            var solutionRoot = TestUtils.GetSolutionRoot();
            var testFilePath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "terrain_materials.xml");
            
            // Act
            var sizeInfo = processor.GetFileSizeInfo(testFilePath);
            
            // Assert
            Assert.NotNull(sizeInfo);
            Assert.Equal(testFilePath, sizeInfo.FilePath);
            Assert.True(sizeInfo.SizeBytes > 0);
            Assert.True(sizeInfo.SizeMB > 0);
        }
        
        [Fact]
        public void LargeXmlFileProcessor_GetOptimalProcessingParameters_ShouldReturnAppropriateParameters()
        {
            // Arrange
            var processor = new LargeXmlFileProcessor();
            var solutionRoot = TestUtils.GetSolutionRoot();
            var testFilePath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "terrain_materials.xml");
            
            // Act
            var parameters = processor.GetOptimalProcessingParameters(testFilePath);
            
            // Assert
            Assert.NotNull(parameters);
            Assert.True(parameters.ChunkSize > 0);
            Assert.True(parameters.MaxConcurrentOperations > 0);
        }
        
        [Fact]
        public async Task LargeXmlFileProcessor_ProcessLargeTerrainMaterialsFileAsync_ShouldProcessFile()
        {
            // Arrange
            var processor = new LargeXmlFileProcessor();
            var solutionRoot = TestUtils.GetSolutionRoot();
            var testFilePath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "terrain_materials.xml");
            
            // Act
            var result = await processor.ProcessLargeTerrainMaterialsFileAsync(testFilePath);
            
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.TerrainMaterialList);
            Assert.True(result.TerrainMaterialList.Count > 0);
            
            // 验证一些基本属性
            var defaultMaterial = result.TerrainMaterialList.FirstOrDefault(t => t.Name == "default");
            Assert.NotNull(defaultMaterial);
            Assert.Equal("true", defaultMaterial.IsEnabled);
            Assert.Equal("soil", defaultMaterial.PhysicsMaterial);
        }
        
        [Fact]
        public async Task LargeXmlFileProcessor_ProcessLargeMPClassDivisionsFileAsync_ShouldProcessFile()
        {
            // Arrange
            var processor = new LargeXmlFileProcessor();
            var solutionRoot = TestUtils.GetSolutionRoot();
            var testFilePath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "mp_class_divisions.xml");
            
            // Skip if test file doesn't exist
            if (!File.Exists(testFilePath))
            {
                return;
            }
            
            // Act
            var result = await processor.ProcessLargeMPClassDivisionsFileAsync(testFilePath);
            
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.MPClassDivisions);
            
            // 验证索引已初始化
            Assert.NotNull(result.ClassDivisionsByCulture);
            Assert.NotNull(result.ClassDivisionsByGameMode);
        }
    }
    
    public class LayoutsManagerTests
    {
        [Fact]
        public void LayoutsManager_CreateNewLayouts_ShouldCreateValidLayouts()
        {
            // Arrange
            var manager = new LayoutsManager();
            
            // Act
            var layouts = manager.CreateNewLayouts("TestType", "Test Description");
            
            // Assert
            Assert.NotNull(layouts);
            Assert.Equal("TestType", layouts.Type);
            Assert.True(layouts.HasLayouts);
            Assert.NotNull(layouts.Layouts);
            Assert.NotEmpty(layouts.Layouts.LayoutList);
            
            var layout = layouts.Layouts.LayoutList[0];
            Assert.Equal("TestType_class", layout.Class);
            Assert.Equal("TestType_tag", layout.XmlTag);
            Assert.True(layout.HasColumns);
            Assert.True(layout.HasItems);
            
            // 验证索引已初始化
            Assert.NotNull(layouts.LayoutsByClass);
            Assert.NotNull(layouts.LayoutsByXmlTag);
            
            // 验证通过验证
            Assert.True(layouts.IsValid);
            Assert.Empty(layouts.ValidationErrors);
        }
        
        [Fact]
        public void LayoutsManager_CloneLayouts_ShouldCreateDeepCopy()
        {
            // Arrange
            var manager = new LayoutsManager();
            var original = manager.CreateNewLayouts("OriginalType", "Original Description");
            
            // Act
            var cloned = manager.CloneLayouts(original);
            
            // Assert
            Assert.NotNull(cloned);
            Assert.Equal(original.Type, cloned.Type);
            Assert.Equal(original.Layouts.LayoutList.Count, cloned.Layouts.LayoutList.Count);
            
            // 验证是深拷贝，不是引用
            cloned.Type = "ModifiedType";
            Assert.NotEqual(original.Type, cloned.Type);
        }
        
        [Fact]
        public void LayoutsManager_MergeLayouts_ShouldMergeCorrectly()
        {
            // Arrange
            var manager = new LayoutsManager();
            
            var primary = manager.CreateNewLayouts("PrimaryType", "Primary");
            var secondary = manager.CreateNewLayouts("SecondaryType", "Secondary");
            
            // Add a different layout to secondary
            var secondaryLayout = new LayoutDO
            {
                Class = "SecondaryClass",
                XmlTag = "SecondaryTag",
                Version = "1.0",
                UseInTreeview = "true",
                HasColumns = true,
                Columns = new ColumnsDO
                {
                    ColumnList = new List<ColumnDO>
                    {
                        new ColumnDO { Id = "col1", Width = "150px" }
                    }
                },
                HasItems = true,
                Items = new ItemsDO
                {
                    ItemList = new List<LayoutsItemDO>
                    {
                        new LayoutsItemDO
                        {
                            Name = "secondary_name",
                            Label = "Secondary Name",
                            Type = "string",
                            Column = "col1",
                            XmlPath = "@name"
                        }
                    }
                }
            };
            
            secondary.Layouts.LayoutList.Add(secondaryLayout);
            
            // Act
            var merged = manager.MergeLayouts(primary, secondary);
            
            // Assert
            Assert.NotNull(merged);
            Assert.True(merged.Layouts.LayoutList.Count > primary.Layouts.LayoutList.Count);
            
            // 验证包含来自两个源的布局
            Assert.Contains(merged.Layouts.LayoutList, l => l.Class == "PrimaryType_class");
            Assert.Contains(merged.Layouts.LayoutList, l => l.Class == "SecondaryClass");
        }
        
        [Fact]
        public void LayoutsManager_GetSupportedLayoutTypes_ShouldReturnAllTypes()
        {
            // Arrange
            var manager = new LayoutsManager();
            
            // Act
            var types = manager.GetSupportedLayoutTypes();
            
            // Assert
            Assert.NotNull(types);
            Assert.NotEmpty(types);
            Assert.Contains("SkeletonsLayout", types);
            Assert.Contains("ParticleSystemLayout", types);
            Assert.Contains("ItemHolstersLayout", types);
        }
        
        [Fact]
        public void LayoutsManager_GetCacheStatistics_ShouldReturnValidStatistics()
        {
            // Arrange
            var manager = new LayoutsManager();
            
            // Act
            var stats = manager.GetCacheStatistics();
            
            // Assert
            Assert.NotNull(stats);
            Assert.Equal(0, stats.CachedFilesCount); // 初始为空
            Assert.Equal(0, stats.TotalMemoryUsage);
        }
        
        [Fact]
        public void LayoutsManager_ClearCache_ShouldClearAllCache()
        {
            // Arrange
            var manager = new LayoutsManager();
            
            // 先创建一个缓存条目
            var layouts = manager.CreateNewLayouts("TestType");
            // 注意：由于我们无法直接访问私有字段，这个测试主要验证方法不抛出异常
            
            // Act & Assert
            manager.ClearCache(); // 不应该抛出异常
            
            var stats = manager.GetCacheStatistics();
            Assert.Equal(0, stats.CachedFilesCount);
        }
    }
}