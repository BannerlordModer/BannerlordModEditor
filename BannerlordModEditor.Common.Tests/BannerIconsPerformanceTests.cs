using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    /// <summary>
    /// BannerIcons性能测试
    /// 测试大型XML文件处理性能、内存使用情况、并发处理能力
    /// </summary>
    public class BannerIconsPerformanceTests
    {
        #region Serialization Performance Tests

        [Fact]
        public void Serialization_SmallObject_FastPerformance()
        {
            // Arrange
            var smallObject = CreateSmallBannerIconsDO();
            var iterations = 1000;

            // Act
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                var dto = BannerIconsMapper.ToDTO(smallObject);
                var result = BannerIconsMapper.ToDO(dto);
            }
            stopwatch.Stop();

            // Assert
            Assert.True(stopwatch.ElapsedMilliseconds < 1000, 
                $"Small object serialization took too long: {stopwatch.ElapsedMilliseconds}ms for {iterations} iterations");
            
            var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
            Assert.True(averageTime < 1.0, 
                $"Average serialization time too slow: {averageTime:F2}ms per operation");
        }

        [Fact]
        public void Serialization_MediumObject_AcceptablePerformance()
        {
            // Arrange
            var mediumObject = CreateMediumBannerIconsDO();
            var iterations = 100;

            // Act
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                var dto = BannerIconsMapper.ToDTO(mediumObject);
                var result = BannerIconsMapper.ToDO(dto);
            }
            stopwatch.Stop();

            // Assert
            Assert.True(stopwatch.ElapsedMilliseconds < 5000, 
                $"Medium object serialization took too long: {stopwatch.ElapsedMilliseconds}ms for {iterations} iterations");
            
            var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
            Assert.True(averageTime < 50.0, 
                $"Average serialization time too slow: {averageTime:F2}ms per operation");
        }

        [Fact]
        public void Serialization_LargeObject_HandlesWithinReasonableTime()
        {
            // Arrange
            var largeObject = CreateLargeBannerIconsDO();
            var iterations = 10;

            // Act
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                var dto = BannerIconsMapper.ToDTO(largeObject);
                var result = BannerIconsMapper.ToDO(dto);
            }
            stopwatch.Stop();

            // Assert
            Assert.True(stopwatch.ElapsedMilliseconds < 30000, 
                $"Large object serialization took too long: {stopwatch.ElapsedMilliseconds}ms for {iterations} iterations");
            
            var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
            Assert.True(averageTime < 3000.0, 
                $"Average serialization time too slow: {averageTime:F2}ms per operation");
        }

        #endregion

        #region Memory Usage Tests

        [Fact]
        public void MemoryUsage_SingleOperation_ReasonableFootprint()
        {
            // Arrange
            var largeObject = CreateLargeBannerIconsDO();

            // Act
            long memoryBefore = GC.GetTotalMemory(true);
            var dto = BannerIconsMapper.ToDTO(largeObject);
            var result = BannerIconsMapper.ToDO(dto);
            long memoryAfter = GC.GetTotalMemory(false);

            // Assert
            var memoryIncrease = memoryAfter - memoryBefore;
            Assert.True(memoryIncrease < 10 * 1024 * 1024, // Less than 10MB
                $"Memory usage too high: {memoryIncrease / 1024 / 1024:F2}MB for single operation");
        }

        [Fact]
        public void MemoryUsage_MultipleOperations_NoMemoryLeaks()
        {
            // Arrange
            var mediumObject = CreateMediumBannerIconsDO();
            var iterations = 100;

            // Act
            long memoryBefore = GC.GetTotalMemory(true);
            for (int i = 0; i < iterations; i++)
            {
                var dto = BannerIconsMapper.ToDTO(mediumObject);
                var result = BannerIconsMapper.ToDO(dto);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            long memoryAfter = GC.GetTotalMemory(true);

            // Assert
            var memoryIncrease = memoryAfter - memoryBefore;
            Assert.True(memoryIncrease < 5 * 1024 * 1024, // Less than 5MB increase after GC
                $"Potential memory leak: {memoryIncrease / 1024 / 1024:F2}MB increase after {iterations} operations");
        }

        [Fact]
        public void MemoryUsage_ExtremeLargeObject_HandlesGracefully()
        {
            // Arrange
            var extremeObject = CreateExtremeBannerIconsDO();

            // Act
            long memoryBefore = GC.GetTotalMemory(true);
            var dto = BannerIconsMapper.ToDTO(extremeObject);
            var result = BannerIconsMapper.ToDO(dto);
            long memoryAfter = GC.GetTotalMemory(false);

            // Assert
            var memoryIncrease = memoryAfter - memoryBefore;
            
            // This is a very large object, so we allow more memory but it should still be reasonable
            Assert.True(memoryIncrease < 100 * 1024 * 1024, // Less than 100MB
                $"Extreme object memory usage too high: {memoryIncrease / 1024 / 1024:F2}MB");
            
            // Verify object was created correctly
            Assert.Equal(1000, result.BannerIconData.BannerIconGroups.Count);
            Assert.Equal(5000, result.BannerIconData.BannerColors.Colors.Count);
        }

        #endregion

        #region Concurrency Tests

        [Fact]
        public void Concurrency_ParallelOperations_ThreadSafe()
        {
            // Arrange
            var mediumObject = CreateMediumBannerIconsDO();
            var taskCount = 10;
            var iterationsPerTask = 50;

            // Act
            var stopwatch = Stopwatch.StartNew();
            var tasks = new List<Task>();
            
            for (int i = 0; i < taskCount; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < iterationsPerTask; j++)
                    {
                        var dto = BannerIconsMapper.ToDTO(mediumObject);
                        var result = BannerIconsMapper.ToDO(dto);
                        
                        // Verify result integrity
                        Assert.NotNull(result);
                        Assert.NotNull(result.BannerIconData);
                    }
                }));
            }
            
            Task.WhenAll(tasks).Wait();
            stopwatch.Stop();

            // Assert
            Assert.True(stopwatch.ElapsedMilliseconds < 10000, 
                $"Concurrent operations took too long: {stopwatch.ElapsedMilliseconds}ms");
            
            // Verify no exceptions were thrown
            Assert.All(tasks, task => Assert.Equal(TaskStatus.RanToCompletion, task.Status));
        }

        [Fact]
        public void Concurrency_MixedObjectSizes_HandlesCorrectly()
        {
            // Arrange
            var objects = new List<BannerIconsDO>
            {
                CreateSmallBannerIconsDO(),
                CreateMediumBannerIconsDO(),
                CreateLargeBannerIconsDO()
            };
            
            var taskCount = 6; // 2 tasks per object size

            // Act
            var stopwatch = Stopwatch.StartNew();
            var tasks = new List<Task>();
            
            for (int i = 0; i < taskCount; i++)
            {
                var obj = objects[i % objects.Count];
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 20; j++)
                    {
                        var dto = BannerIconsMapper.ToDTO(obj);
                        var result = BannerIconsMapper.ToDO(dto);
                        
                        // Verify result integrity
                        Assert.NotNull(result);
                        Assert.Equal(obj.BannerIconData.BannerIconGroups.Count, 
                                    result.BannerIconData.BannerIconGroups.Count);
                    }
                }));
            }
            
            Task.WhenAll(tasks).Wait();
            stopwatch.Stop();

            // Assert
            Assert.True(stopwatch.ElapsedMilliseconds < 15000, 
                $"Mixed size concurrent operations took too long: {stopwatch.ElapsedMilliseconds}ms");
            
            Assert.All(tasks, task => Assert.Equal(TaskStatus.RanToCompletion, task.Status));
        }

        #endregion

        #region Scalability Tests

        [Fact]
        public void Scalability_LinearPerformance_AsSizeIncreases()
        {
            // Arrange
            var sizes = new[] { 10, 50, 100, 500, 1000 };
            var performanceResults = new List<double>();

            // Act
            foreach (var size in sizes)
            {
                var testObject = CreateScalableBannerIconsDO(size);
                var iterations = 10;

                var stopwatch = Stopwatch.StartNew();
                for (int i = 0; i < iterations; i++)
                {
                    var dto = BannerIconsMapper.ToDTO(testObject);
                    var result = BannerIconsMapper.ToDO(dto);
                }
                stopwatch.Stop();

                var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
                performanceResults.Add(averageTime);
            }

            // Assert
            // Performance should scale roughly linearly with size
            // (allowing for some overhead and non-linear factors)
            for (int i = 1; i < performanceResults.Count; i++)
            {
                var sizeRatio = (double)sizes[i] / sizes[i - 1];
                var timeRatio = performanceResults[i] / performanceResults[i - 1];
                
                // Time increase should be reasonable (not more than 2x the size increase)
                Assert.True(timeRatio <= sizeRatio * 2, 
                    $"Performance doesn't scale well: size {sizes[i-1]}->{sizes[i]} " +
                    $"took {timeRatio:F2}x longer (size ratio: {sizeRatio:F2}x)");
            }
        }

        [Fact]
        public void Scalability_MemoryUsage_LinearAsSizeIncreases()
        {
            // Arrange
            var sizes = new[] { 100, 500, 1000, 2000 };
            var memoryResults = new List<long>();

            // Act
            foreach (var size in sizes)
            {
                var testObject = CreateScalableBannerIconsDO(size);
                
                long memoryBefore = GC.GetTotalMemory(true);
                var dto = BannerIconsMapper.ToDTO(testObject);
                var result = BannerIconsMapper.ToDO(dto);
                long memoryAfter = GC.GetTotalMemory(false);
                
                memoryResults.Add(memoryAfter - memoryBefore);
            }

            // Assert
            // Memory usage should scale roughly linearly with size
            for (int i = 1; i < memoryResults.Count; i++)
            {
                var sizeRatio = (double)sizes[i] / sizes[i - 1];
                var memoryRatio = (double)memoryResults[i] / memoryResults[i - 1];
                
                // Memory increase should be reasonable (not more than 3x the size increase)
                Assert.True(memoryRatio <= sizeRatio * 3, 
                    $"Memory usage doesn't scale well: size {sizes[i-1]}->{sizes[i]} " +
                    $"used {memoryRatio:F2}x more memory (size ratio: {sizeRatio:F2}x)");
            }
        }

        #endregion

        #region Real XML File Performance Tests

        [Fact]
        public void RealXmlPerformance_DeserializeAndSerialize_AcceptablePerformance()
        {
            // Arrange
            var xmlPath = "TestData/banner_icons.xml";
            if (!File.Exists(xmlPath))
            {
                return; // Skip if test data not available
            }

            var xml = File.ReadAllText(xmlPath);
            var iterations = 10;

            // Act
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                var model = XmlTestUtils.Deserialize<BannerIconsDO>(xml);
                var serializedXml = XmlTestUtils.Serialize(model, xml);
            }
            stopwatch.Stop();

            // Assert
            Assert.True(stopwatch.ElapsedMilliseconds < 10000, 
                $"Real XML processing took too long: {stopwatch.ElapsedMilliseconds}ms for {iterations} iterations");
            
            var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
            Assert.True(averageTime < 1000.0, 
                $"Average real XML processing time too slow: {averageTime:F2}ms per operation");
        }

        [Fact]
        public void RealXmlPerformance_LargeFile_MemoryEfficient()
        {
            // Arrange
            var xmlPath = "TestData/banner_icons.xml";
            if (!File.Exists(xmlPath))
            {
                return; // Skip if test data not available
            }

            var xml = File.ReadAllText(xmlPath);

            // Act
            long memoryBefore = GC.GetTotalMemory(true);
            var model = XmlTestUtils.Deserialize<BannerIconsDO>(xml);
            var serializedXml = XmlTestUtils.Serialize(model, xml);
            long memoryAfter = GC.GetTotalMemory(false);

            // Assert
            var memoryIncrease = memoryAfter - memoryBefore;
            var fileSize = new FileInfo(xmlPath).Length;
            
            // Memory usage should be reasonable compared to file size
            var memoryToFileSizeRatio = (double)memoryIncrease / fileSize;
            Assert.True(memoryToFileSizeRatio < 10, 
                $"Memory usage too high compared to file size: {memoryToFileSizeRatio:F2}x " +
                $"(memory: {memoryIncrease / 1024}KB, file: {fileSize / 1024}KB)");
        }

        #endregion

        #region Type Conversion Performance Tests

        [Fact]
        public void TypeConversion_Performance_BulkOperationsEfficient()
        {
            // Arrange
            var groups = new List<BannerIconGroupDO>();
            for (int i = 0; i < 1000; i++)
            {
                groups.Add(new BannerIconGroupDO
                {
                    Id = i.ToString(),
                    Name = $"Group {i}",
                    IsPattern = (i % 2 == 0).ToString(),
                    Backgrounds = new List<BackgroundDO>
                    {
                        new BackgroundDO { Id = $"{i}_1", MeshName = $"mesh_{i}_1" }
                    }
                });
            }

            // Act
            var stopwatch = Stopwatch.StartNew();
            var dtos = groups.Select(g => BannerIconGroupMapper.ToDTO(g)).ToList();
            var results = dtos.Select(d => BannerIconGroupMapper.ToDO(d)).ToList();
            stopwatch.Stop();

            // Assert
            Assert.True(stopwatch.ElapsedMilliseconds < 1000, 
                $"Bulk type conversion took too long: {stopwatch.ElapsedMilliseconds}ms for 1000 items");
            
            // Verify type conversions worked correctly
            for (int i = 0; i < results.Count; i++)
            {
                Assert.Equal(i, results[i].IdInt);
                Assert.Equal(i % 2 == 0, results[i].IsPatternBool);
            }
        }

        [Fact]
        public void TypeConversion_Performance_InvalidValues_HandlesGracefully()
        {
            // Arrange
            var invalidGroups = new List<BannerIconGroupDO>();
            for (int i = 0; i < 1000; i++)
            {
                invalidGroups.Add(new BannerIconGroupDO
                {
                    Id = i % 5 == 0 ? "invalid" : i.ToString(),
                    IsPattern = i % 3 == 0 ? "maybe" : (i % 2 == 0).ToString()
                });
            }

            // Act
            var stopwatch = Stopwatch.StartNew();
            var dtos = invalidGroups.Select(g => BannerIconGroupMapper.ToDTO(g)).ToList();
            var results = dtos.Select(d => BannerIconGroupMapper.ToDO(d)).ToList();
            stopwatch.Stop();

            // Assert
            Assert.True(stopwatch.ElapsedMilliseconds < 1000, 
                $"Invalid value type conversion took too long: {stopwatch.ElapsedMilliseconds}ms for 1000 items");
            
            // Verify invalid values were handled correctly
            for (int i = 0; i < results.Count; i++)
            {
                if (i % 5 == 0)
                {
                    Assert.Null(results[i].IdInt);
                }
                else
                {
                    Assert.Equal(i, results[i].IdInt);
                }
                
                if (i % 3 == 0)
                {
                    Assert.Null(results[i].IsPatternBool);
                }
                else
                {
                    Assert.Equal(i % 2 == 0, results[i].IsPatternBool);
                }
            }
        }

        #endregion

        #region Helper Methods

        private BannerIconsDO CreateSmallBannerIconsDO()
        {
            return new BannerIconsDO
            {
                Type = "small_test",
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = new List<BannerIconGroupDO>
                    {
                        new BannerIconGroupDO
                        {
                            Id = "1",
                            Name = "Test",
                            IsPattern = "true",
                            Backgrounds = new List<BackgroundDO>
                            {
                                new BackgroundDO { Id = "1", MeshName = "test_mesh" }
                            }
                        }
                    },
                    BannerColors = new BannerColorsDO
                    {
                        Colors = new List<ColorEntryDO>
                        {
                            new ColorEntryDO { Id = "1", Hex = "#FFFFFF" }
                        }
                    }
                }
            };
        }

        private BannerIconsDO CreateMediumBannerIconsDO()
        {
            var groups = new List<BannerIconGroupDO>();
            var colors = new List<ColorEntryDO>();

            for (int i = 0; i < 50; i++)
            {
                groups.Add(new BannerIconGroupDO
                {
                    Id = i.ToString(),
                    Name = $"Group {i}",
                    IsPattern = (i % 2 == 0).ToString(),
                    Backgrounds = new List<BackgroundDO>
                    {
                        new BackgroundDO { Id = $"{i}_1", MeshName = $"mesh_{i}_1" },
                        new BackgroundDO { Id = $"{i}_2", MeshName = $"mesh_{i}_2" }
                    },
                    Icons = new List<IconDO>
                    {
                        new IconDO { Id = (100 + i).ToString(), MaterialName = $"material_{i}" }
                    }
                });

                colors.Add(new ColorEntryDO
                {
                    Id = i.ToString(),
                    Hex = $"#{i:X6}",
                    PlayerCanChooseForBackground = "true",
                    PlayerCanChooseForSigil = (i % 2 == 0).ToString()
                });
            }

            return new BannerIconsDO
            {
                Type = "medium_test",
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = groups,
                    BannerColors = new BannerColorsDO { Colors = colors }
                }
            };
        }

        private BannerIconsDO CreateLargeBannerIconsDO()
        {
            var groups = new List<BannerIconGroupDO>();
            var colors = new List<ColorEntryDO>();

            for (int i = 0; i < 200; i++)
            {
                var backgrounds = new List<BackgroundDO>();
                var icons = new List<IconDO>();

                for (int j = 0; j < 10; j++)
                {
                    backgrounds.Add(new BackgroundDO 
                    { 
                        Id = $"{i}_{j}", 
                        MeshName = $"mesh_{i}_{j}",
                        IsBaseBackground = (j % 5 == 0).ToString()
                    });
                    icons.Add(new IconDO 
                    { 
                        Id = (1000 + i * 10 + j).ToString(), 
                        MaterialName = $"material_{i}_{j}",
                        TextureIndex = j.ToString(),
                        IsReserved = (j % 3 == 0).ToString()
                    });
                }

                groups.Add(new BannerIconGroupDO
                {
                    Id = i.ToString(),
                    Name = $"Group {i}",
                    IsPattern = (i % 2 == 0).ToString(),
                    Backgrounds = backgrounds,
                    Icons = icons
                });
            }

            for (int i = 0; i < 500; i++)
            {
                colors.Add(new ColorEntryDO
                {
                    Id = i.ToString(),
                    Hex = $"#{i:X6}",
                    PlayerCanChooseForBackground = (i % 3 != 0).ToString(),
                    PlayerCanChooseForSigil = (i % 2 == 0).ToString()
                });
            }

            return new BannerIconsDO
            {
                Type = "large_test",
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = groups,
                    BannerColors = new BannerColorsDO { Colors = colors }
                }
            };
        }

        private BannerIconsDO CreateExtremeBannerIconsDO()
        {
            var groups = new List<BannerIconGroupDO>();
            var colors = new List<ColorEntryDO>();

            for (int i = 0; i < 1000; i++)
            {
                var backgrounds = new List<BackgroundDO>();
                var icons = new List<IconDO>();

                for (int j = 0; j < 5; j++)
                {
                    backgrounds.Add(new BackgroundDO 
                    { 
                        Id = $"{i}_{j}", 
                        MeshName = $"mesh_{i}_{j}" 
                    });
                    icons.Add(new IconDO 
                    { 
                        Id = (5000 + i * 5 + j).ToString(), 
                        MaterialName = $"material_{i}_{j}" 
                    });
                }

                groups.Add(new BannerIconGroupDO
                {
                    Id = i.ToString(),
                    Name = $"Group {i}",
                    IsPattern = (i % 2 == 0).ToString(),
                    Backgrounds = backgrounds,
                    Icons = icons
                });
            }

            for (int i = 0; i < 5000; i++)
            {
                colors.Add(new ColorEntryDO
                {
                    Id = i.ToString(),
                    Hex = $"#{i:X6}"
                });
            }

            return new BannerIconsDO
            {
                Type = "extreme_test",
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = groups,
                    BannerColors = new BannerColorsDO { Colors = colors }
                }
            };
        }

        private BannerIconsDO CreateScalableBannerIconsDO(int size)
        {
            var groups = new List<BannerIconGroupDO>();
            var colors = new List<ColorEntryDO>();

            for (int i = 0; i < size; i++)
            {
                groups.Add(new BannerIconGroupDO
                {
                    Id = i.ToString(),
                    Name = $"Group {i}",
                    IsPattern = (i % 2 == 0).ToString(),
                    Backgrounds = new List<BackgroundDO>
                    {
                        new BackgroundDO { Id = $"{i}_1", MeshName = $"mesh_{i}_1" }
                    },
                    Icons = new List<IconDO>
                    {
                        new IconDO { Id = (100 + i).ToString(), MaterialName = $"material_{i}" }
                    }
                });

                colors.Add(new ColorEntryDO
                {
                    Id = i.ToString(),
                    Hex = $"#{i:X6}"
                });
            }

            return new BannerIconsDO
            {
                Type = "scalable_test",
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = groups,
                    BannerColors = new BannerColorsDO { Colors = colors }
                }
            };
        }

        #endregion
    }
}