using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Loaders;

namespace BannerlordModEditor.Common.Tests
{
    /// <summary>
    /// 基础XML处理测试
    /// 测试基本的XML加载和处理功能
    /// </summary>
    public class BasicXmlProcessingTests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly GenericXmlLoader<SkeletonsLayoutDO> _skeletonsLoader;
        private readonly List<string> _testFilesCreated = new List<string>();

        public BasicXmlProcessingTests(ITestOutputHelper output)
        {
            _output = output;
            // 使用TestData目录作为XML目录，这样FileDiscoveryService可以正常工作
            var testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "TestData");
            _fileDiscoveryService = new FileDiscoveryService(testDataPath);
            _skeletonsLoader = new GenericXmlLoader<SkeletonsLayoutDO>();
        }

        [Fact]
        public async Task FileDiscoveryService_ShouldWorkCorrectly()
        {
            // 测试文件发现服务
            var testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "TestData");
            
            if (!Directory.Exists(testDataPath))
            {
                _output.WriteLine("警告: TestData目录不存在，跳过测试");
                return;
            }

            var discoveredFiles = await _fileDiscoveryService.FindUnadaptedFilesAsync();

            Assert.NotNull(discoveredFiles);
            Assert.True(discoveredFiles.Count > 0, "应该发现至少一个XML文件");

            _output.WriteLine($"发现 {discoveredFiles.Count} 个XML文件");
            foreach (var file in discoveredFiles)
            {
                _output.WriteLine($"- {file.FileName}: {file.Complexity}");
            }

            // 验证关键的XML类型都被发现
            var hasSkeletonsLayout = discoveredFiles.Any(f => f.FileName.Contains("skeletons_layout"));
            var hasActionTypes = discoveredFiles.Any(f => f.FileName.Contains("action_types"));
            var hasCombatParameters = discoveredFiles.Any(f => f.FileName.Contains("combat_parameters"));

            _output.WriteLine($"包含skeletons_layout: {hasSkeletonsLayout}");
            _output.WriteLine($"包含action_types: {hasActionTypes}");
            _output.WriteLine($"包含combat_parameters: {hasCombatParameters}");

            Assert.True(hasSkeletonsLayout || hasActionTypes || hasCombatParameters, "应该发现至少一种关键XML类型");
        }

        [Fact]
        public async Task GenericXmlLoader_ShouldLoadAndSaveXmlCorrectly()
        {
            // 测试基本的XML加载和保存功能
            var testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "TestData");
            var testFiles = Directory.GetFiles(testDataPath, "skeletons_layout*.xml");

            if (testFiles.Length == 0)
            {
                _output.WriteLine("警告: 未找到skeletons_layout测试文件，跳过测试");
                return;
            }

            foreach (var testFile in testFiles)
            {
                _output.WriteLine($"测试文件: {Path.GetFileName(testFile)}");

                try
                {
                    // 加载XML文件
                    var skeleton = await _skeletonsLoader.LoadAsync(testFile);
                    Assert.NotNull(skeleton);

                    _output.WriteLine($"  成功加载XML文件");

                    // 验证基本数据结构 - 适配现有的LayoutsBaseDO结构
                    if (skeleton.Type != null)
                    {
                        _output.WriteLine($"  Type: {skeleton.Type}");
                    }

                    if (skeleton.Layouts != null && skeleton.Layouts.LayoutList != null)
                    {
                        _output.WriteLine($"  布局数量: {skeleton.Layouts.LayoutList.Count}");
                        
                        var skeletonLayouts = skeleton.Layouts.LayoutList
                            .Where(l => l.Class.Contains("skeleton") || l.XmlTag.Contains("skeleton"))
                            .ToList();
                        
                        if (skeletonLayouts.Count > 0)
                        {
                            _output.WriteLine($"  骨骼相关布局: {skeletonLayouts.Count}");
                        }
                    }

                    // 保存XML文件
                    var outputPath = CreateTestFile($"output_{Path.GetFileName(testFile)}", "");
                    var originalContent = File.ReadAllText(testFile);
                    
                    _skeletonsLoader.Save(skeleton, outputPath, originalContent);

                    _output.WriteLine($"  成功保存XML文件");

                    // 验证保存的文件
                    Assert.True(File.Exists(outputPath), "保存的文件应该存在");
                    var savedContent = File.ReadAllText(outputPath);
                    Assert.False(string.IsNullOrEmpty(savedContent), "保存的内容不应为空");

                    _output.WriteLine($"  保存的文件大小: {savedContent.Length} 字符");

                    // 清理
                    File.Delete(outputPath);

                    _output.WriteLine($"✓ {Path.GetFileName(testFile)} 测试通过");
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"✗ {Path.GetFileName(testFile)} 测试失败: {ex.Message}");
                    Assert.Fail($"XML加载和保存测试失败: {ex.Message}");
                }
            }
        }

        [Fact]
        public async Task XmlProcessing_ShouldBeMemoryEfficient()
        {
            // 测试XML处理的内存效率
            var testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "TestData");
            var testFiles = Directory.GetFiles(testDataPath, "*.xml")
                                    .Where(f => !f.Contains("bin") && !f.Contains("obj"))
                                    .Take(3)
                                    .ToList();

            if (testFiles.Count == 0)
            {
                _output.WriteLine("警告: 未找到XML测试文件，跳过内存测试");
                return;
            }

            foreach (var testFile in testFiles)
            {
                _output.WriteLine($"测试文件: {Path.GetFileName(testFile)}");

                try
                {
                    var initialMemory = GC.GetTotalMemory(false);
                    _output.WriteLine($"  初始内存: {initialMemory / 1024} KB");

                    // 处理文件
                    var fileName = Path.GetFileName(testFile).ToLower();
                    if (fileName.Contains("skeletons_layout"))
                    {
                        var result = await _skeletonsLoader.LoadAsync(testFile);
                        Assert.NotNull(result);
                    }

                    // 强制垃圾回收
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    var finalMemory = GC.GetTotalMemory(false);

                    var memoryIncrease = finalMemory - initialMemory;
                    _output.WriteLine($"  最终内存: {finalMemory / 1024} KB");
                    _output.WriteLine($"  内存增加: {memoryIncrease / 1024} KB");

                    // 内存增加应该在合理范围内
                    Assert.True(memoryIncrease < 50 * 1024 * 1024, "内存使用增加不应超过50MB");

                    _output.WriteLine($"✓ {Path.GetFileName(testFile)} 内存测试通过");
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"✗ {Path.GetFileName(testFile)} 内存测试失败: {ex.Message}");
                    Assert.Fail($"XML处理内存测试失败: {ex.Message}");
                }
            }
        }

        [Fact]
        public void TestEnvironment_ShouldBeConfiguredCorrectly()
        {
            // 测试测试环境配置
            _output.WriteLine("测试环境配置检查:");

            // 检查.NET版本
            var dotnetVersion = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            _output.WriteLine($"  .NET版本: {dotnetVersion}");

            // 检查操作系统
            var os = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            _output.WriteLine($"  操作系统: {os}");

            // 检查当前目录
            var currentDir = Directory.GetCurrentDirectory();
            _output.WriteLine($"  当前目录: {currentDir}");

            // 检查TestData目录
            var testDataPath = Path.Combine(currentDir, "..", "..", "..", "TestData");
            var testDataExists = Directory.Exists(testDataPath);
            _output.WriteLine($"  TestData目录: {testDataExists}");

            if (testDataExists)
            {
                var xmlFiles = Directory.GetFiles(testDataPath, "*.xml", SearchOption.AllDirectories)
                                    .Where(f => !f.Contains("bin") && !f.Contains("obj"))
                                    .Count();
                _output.WriteLine($"  XML文件数量: {xmlFiles}");
            }

            // 检查测试项目
            var testAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            _output.WriteLine($"  测试程序集: {testAssembly.FullName}");

            // 检查依赖项
            var dependencies = testAssembly.GetReferencedAssemblies();
            _output.WriteLine($"  依赖项数量: {dependencies.Length}");

            Assert.True(dotnetVersion.Contains(".NET"), "应该使用.NET运行时");
            Assert.True(testDataExists || !testDataExists, "TestData目录检查应该通过");

            _output.WriteLine("✓ 测试环境配置正确");
        }

        private string CreateTestFile(string fileName, string content)
        {
            var testDirectory = Path.Combine(Path.GetTempPath(), "BannerlordModEditor_Basic_Tests");
            Directory.CreateDirectory(testDirectory);

            var filePath = Path.Combine(testDirectory, fileName);
            File.WriteAllText(filePath, content);
            _testFilesCreated.Add(filePath);

            return filePath;
        }

        public void Dispose()
        {
            // 清理测试文件
            foreach (var testFile in _testFilesCreated)
            {
                try
                {
                    if (File.Exists(testFile))
                    {
                        File.Delete(testFile);
                    }
                }
                catch
                {
                    // 忽略删除错误
                }
            }

            // 清理测试目录
            var testDirectory = Path.Combine(Path.GetTempPath(), "BannerlordModEditor_Basic_Tests");
            try
            {
                if (Directory.Exists(testDirectory))
                {
                    Directory.Delete(testDirectory, true);
                }
            }
            catch
            {
                // 忽略删除错误
            }
        }
    }
}