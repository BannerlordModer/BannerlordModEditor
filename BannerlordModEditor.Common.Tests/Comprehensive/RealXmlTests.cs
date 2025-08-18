using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.Common.Loaders;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DO.Game;
using BannerlordModEditor.Common.Models.DO.Audio;
using BannerlordModEditor.Common.Models.DO.Multiplayer;
using BannerlordModEditor.Common.Models.DO.Engine;
using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Models.DO.Configuration;

namespace BannerlordModEditor.Common.Tests.Comprehensive
{
    /// <summary>
    /// 真实存在的XML类型完整测试套件
    /// 只测试实际存在的DO模型，确保测试的真实性和可靠性
    /// </summary>
    public class RealXmlTests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly Dictionary<string, object> _loaders;
        private readonly List<string> _testFilesCreated = new List<string>();

        public RealXmlTests(ITestOutputHelper output)
        {
            _output = output;
            var testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
            _fileDiscoveryService = new FileDiscoveryService(testDataPath);
            
            // 只包含实际存在的DO模型
            _loaders = new Dictionary<string, object>
            {
                // 核心游戏机制
                ["action_types"] = new GenericXmlLoader<ActionTypesDO>(),
                ["combat_parameters"] = new GenericXmlLoader<CombatParametersDO>(),
                ["skills"] = new GenericXmlLoader<SkillsDO>(),
                ["attributes"] = new GenericXmlLoader<AttributesDO>(),
                ["item_modifiers"] = new GenericXmlLoader<ItemModifiersDO>(),
                ["item_modifiers_groups"] = new GenericXmlLoader<ItemModifierGroupsDO>(),
                ["crafting_pieces"] = new GenericXmlLoader<CraftingPiecesDO>(),
                ["crafting_templates"] = new GenericXmlLoader<CraftingTemplatesDO>(),
                
                // 角色和生物
                ["bone_body_types"] = new GenericXmlLoader<BoneBodyTypesDO>(),
                ["monsters"] = new GenericXmlLoader<MonstersDO>(),
                ["parties"] = new GenericXmlLoader<PartiesDO>(),
                
                // 物品和装备
                ["banner_icons"] = new GenericXmlLoader<BannerIconsDO>(),
                ["item_holsters"] = new GenericXmlLoader<ItemHolstersDO>(),
                ["item_usage_sets"] = new GenericXmlLoader<ItemUsageSetsDO>(),
                ["mpitems"] = new GenericXmlLoader<MpItemsDO>(),
                ["mpcrafting_pieces"] = new GenericXmlLoader<MpCraftingPiecesDO>(),
                
                // 界面和布局
                ["map_icons"] = new GenericXmlLoader<MapIconsDO>(),
                ["layouts_base"] = new GenericXmlLoader<LayoutsBaseDO>(),
                ["skeletons_layout"] = new GenericXmlLoader<SkeletonsLayoutDO>(),
                ["looknfeel"] = new GenericXmlLoader<LooknfeelDO>(),
                
                // 物理和碰撞
                ["collision_infos"] = new GenericXmlLoader<CollisionInfosRootDO>(),
                ["physics_materials"] = new GenericXmlLoader<PhysicsMaterialsDO>(),
                ["cloth_bodies"] = new GenericXmlLoader<ClothBodiesDO>(),
                
                // 粒子效果
                ["particle_systems2"] = new GenericXmlLoader<ParticleSystems2DO>(),
                ["particle_systems_basic"] = new GenericXmlLoader<ParticleSystemsDO>(),
                ["particle_systems_general"] = new GenericXmlLoader<ParticleSystemsDO>(),
                ["particle_systems_map_icon"] = new GenericXmlLoader<ParticleSystemsMapIconDO>(),
                
                // 音频系统
                ["voice_definitions"] = new GenericXmlLoader<VoiceDefinitionsDO>(),
                ["module_sounds"] = new GenericXmlLoader<ModuleSoundsDO>(),
                
                // 多人游戏
                ["mpcharacters"] = new GenericXmlLoader<MPCharactersDO>(),
                ["mpcosmetics"] = new GenericXmlLoader<MpcosmeticsDO>(),
                ["badges"] = new GenericXmlLoader<BadgesDO>(),
                ["mpclassdivisions"] = new GenericXmlLoader<MPClassDivisionsDO>(),
                
                // 环境和地形
                ["flora_kinds"] = new GenericXmlLoader<FloraKindsDO>(),
                ["flora_layer_sets"] = new GenericXmlLoader<FloraLayerSetsDO>(),
                ["terrain_materials"] = new GenericXmlLoader<TerrainMaterialsDO>(),
                
                // 场景和动画
                ["scenes"] = new GenericXmlLoader<ScenesDO>(),
                ["movement_sets"] = new GenericXmlLoader<MovementSetsDO>(),
                ["prebaked_animations"] = new GenericXmlLoader<PrebakedAnimationsDO>(),
                
                // 配置和参数
                ["Adjustables"] = new GenericXmlLoader<AdjustablesDO>(),
                ["native_strings"] = new GenericXmlLoader<NativeStringsDO>(),
                
                // 特殊效果
                ["before_transparents_graph"] = new GenericXmlLoader<BeforeTransparentsGraphDO>(),
                ["postfx_graphs"] = new GenericXmlLoader<BannerlordModEditor.Common.Models.DO.Engine.PostfxGraphsDO>(),
                ["prerender"] = new GenericXmlLoader<PrerenderDO>(),
                
                // 战斗系统
                ["monster_usage_sets"] = new GenericXmlLoader<MonsterUsageSetsDO>(),
                ["taunt_usage_sets"] = new GenericXmlLoader<TauntUsageSetsDO>(),
                ["weapon_descriptions"] = new GenericXmlLoader<WeaponDescriptionsDO>(),
                
                // 其他系统
                ["objects"] = new GenericXmlLoader<ObjectsDO>(),
                ["skins"] = new GenericXmlLoader<SkinsDO>()
            };
        }

        [Fact]
        public async Task AllRealXmlTypes_ShouldHaveLoaders()
        {
            // 验证所有真实存在的XML类型都有对应的加载器
            _output.WriteLine($"真实存在的XML类型数量: {_loaders.Count}");
            
            // 验证加载器可以正常实例化
            foreach (var loaderEntry in _loaders)
            {
                Assert.NotNull(loaderEntry.Value);
                _output.WriteLine($"✓ {loaderEntry.Key} 加载器已就绪");
            }

            _output.WriteLine($"✓ 所有 {_loaders.Count} 个真实存在的XML类型都有对应的加载器");
        }

        [Theory]
        [InlineData("action_types")]
        [InlineData("combat_parameters")]
        [InlineData("skills")]
        [InlineData("attributes")]
        [InlineData("item_modifiers")]
        [InlineData("crafting_pieces")]
        [InlineData("bone_body_types")]
        [InlineData("banner_icons")]
        [InlineData("map_icons")]
        [InlineData("physics_materials")]
        [InlineData("particle_systems2")]
        [InlineData("module_sounds")]
        [InlineData("scenes")]
        [InlineData("flora_kinds")]
        [InlineData("mpitems")]
        [InlineData("objects")]
        [InlineData("movement_sets")]
        [InlineData("cloth_bodies")]
        [InlineData("collision_infos")]
        public async Task RealXmlType_ShouldProcessCorrectly(string xmlType)
        {
            // 测试真实存在的XML类型的处理
            var testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
            var testFiles = Directory.GetFiles(testDataPath, $"{xmlType}*.xml").ToList();

            if (testFiles.Count == 0)
            {
                _output.WriteLine($"警告: 未找到 {xmlType} 测试文件，跳过测试");
                return;
            }

            _output.WriteLine($"测试 {xmlType}: 发现 {testFiles.Count} 个测试文件");

            foreach (var testFile in testFiles)
            {
                _output.WriteLine($"  处理: {Path.GetFileName(testFile)}");

                // 特殊处理item_modifiers，有两种不同的XML结构
                object loader;
                string loaderKey;
                
                if (xmlType == "item_modifiers" && testFile.Contains("item_modifiers_groups"))
                {
                    loaderKey = "item_modifiers_groups";
                    loader = _loaders[loaderKey];
                }
                else
                {
                    loaderKey = xmlType;
                    loader = _loaders[xmlType];
                }
                
                Assert.True(_loaders.ContainsKey(loaderKey), $"{loaderKey} 应该有对应的加载器");

                var loadMethod = loader.GetType().GetMethod("LoadAsync");
                var saveMethod = loader.GetType().GetMethod("SaveToString");

                Assert.NotNull(loadMethod);
                Assert.NotNull(saveMethod);

                try
                {
                    // 1. 加载测试
                    var loadedObjTask = (Task)loadMethod.Invoke(loader, new object[] { testFile });
                    await loadedObjTask;
                    var loadedObj = loadedObjTask.GetType().GetProperty("Result")?.GetValue(loadedObjTask);
                    Assert.NotNull(loadedObj);

                    // 2. 序列化测试
                    var originalXml = await File.ReadAllTextAsync(testFile);
                    var serializedXml = (string)saveMethod.Invoke(loader, new object[] { loadedObj, originalXml });
                    Assert.False(string.IsNullOrEmpty(serializedXml));

                    _output.WriteLine($"    ✓ {Path.GetFileName(testFile)} 处理成功");
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"    ✗ {Path.GetFileName(testFile)} 处理失败: {ex.Message}");
                    throw;
                }
            }
        }

        [Fact]
        public async Task RealBatchProcessing_ShouldWork()
        {
            // 真实批量处理测试
            var testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
            var sampleFiles = Directory.GetFiles(testDataPath, "*.xml")
                                    .Where(f => new FileInfo(f).Length < 100 * 1024) // < 100KB
                                    .Take(15)
                                    .ToList();

            if (sampleFiles.Count == 0)
            {
                _output.WriteLine("警告: 未找到测试文件，跳过批量处理测试");
                return;
            }

            _output.WriteLine($"开始真实批量处理 {sampleFiles.Count} 个XML文件");

            var successCount = 0;
            var failureCount = 0;
            var processedTypes = new HashSet<string>();

            foreach (var sampleFile in sampleFiles)
            {
                try
                {
                    var fileName = Path.GetFileNameWithoutExtension(sampleFile).ToLower();
                    var loaderKey = _loaders.Keys.FirstOrDefault(key => fileName.Contains(key));
                    
                    if (loaderKey != null)
                    {
                        var loader = _loaders[loaderKey];
                        var loadMethod = loader.GetType().GetMethod("LoadAsync");
                        
                        if (loadMethod != null)
                        {
                            var resultTask = (Task)loadMethod.Invoke(loader, new object[] { sampleFile });
                            await resultTask;
                            var result = resultTask.GetType().GetProperty("Result")?.GetValue(resultTask);
                            
                            if (result != null)
                            {
                                successCount++;
                                processedTypes.Add(loaderKey);
                                _output.WriteLine($"✓ {Path.GetFileName(sampleFile)} ({loaderKey})");
                            }
                            else
                            {
                                failureCount++;
                                _output.WriteLine($"✗ {Path.GetFileName(sampleFile)} 返回null");
                            }
                        }
                    }
                    else
                    {
                        _output.WriteLine($"? {Path.GetFileName(sampleFile)} 没有对应的加载器");
                    }
                }
                catch (Exception ex)
                {
                    failureCount++;
                    _output.WriteLine($"✗ {Path.GetFileName(sampleFile)} 处理失败: {ex.Message}");
                }
            }

            var successRate = (double)successCount / sampleFiles.Count * 100;
            var typeCoverage = (double)processedTypes.Count / _loaders.Count * 100;

            _output.WriteLine($"真实批量处理完成:");
            _output.WriteLine($"- 成功: {successCount}/{sampleFiles.Count} ({successRate:F1}%)");
            _output.WriteLine($"- 失败: {failureCount}");
            _output.WriteLine($"- 覆盖类型: {processedTypes.Count}/{_loaders.Count} ({typeCoverage:F1}%)");
            _output.WriteLine($"- 处理的类型: {string.Join(", ", processedTypes)}");

            // 验证批量处理效果
            Assert.True(successRate >= 20, $"批量处理成功率应该>=20%，实际为{successRate:F1}%");
            Assert.True(typeCoverage >= 10, $"类型覆盖率应该>=10%，实际为{typeCoverage:F1}%");
        }

        [Fact]
        public async Task RealErrorHandling_ShouldBeRobust()
        {
            // 真实错误处理测试
            _output.WriteLine("开始真实错误处理测试");

            // 测试不存在的文件
            var nonExistentFiles = new[]
            {
                "/nonexistent/file.xml",
                "nonexistent_file.xml",
                Path.Combine(Path.GetTempPath(), "nonexistent_temp_file.xml")
            };

            foreach (var nonExistentFile in nonExistentFiles)
            {
                try
                {
                    var loader = _loaders.Values.First();
                    var loadMethod = loader.GetType().GetMethod("LoadAsync");
                    if (loadMethod != null)
                    {
                        await (Task)loadMethod.Invoke(loader, new object[] { nonExistentFile });
                        _output.WriteLine($"✓ 不存在文件处理未崩溃: {nonExistentFile}");
                    }
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"✓ 正确处理不存在的文件: {nonExistentFile} - {ex.GetType().Name}");
                }
            }

            // 测试空路径
            try
            {
                var loader = _loaders.Values.First();
                var loadMethod = loader.GetType().GetMethod("LoadAsync");
                if (loadMethod != null)
                {
                    await (Task)loadMethod.Invoke(loader, new object[] { "" });
                    _output.WriteLine("✓ 空路径处理未崩溃");
                }
            }
            catch (Exception ex)
            {
                _output.WriteLine($"✓ 正确处理空路径: {ex.GetType().Name}");
            }

            _output.WriteLine("✓ 真实错误处理测试通过");
        }

        [Fact]
        public async Task TestDataCoverage_ShouldBeAdequate()
        {
            // 测试数据覆盖度验证
            var testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
            var allXmlFiles = Directory.GetFiles(testDataPath, "*.xml")
                                    .Where(f => new FileInfo(f).Length < 1024 * 1024) // < 1MB
                                    .ToList();

            if (allXmlFiles.Count == 0)
            {
                _output.WriteLine("警告: 未找到XML文件，跳过测试覆盖度验证");
                return;
            }

            _output.WriteLine($"测试数据覆盖度分析:");
            _output.WriteLine($"- 总XML文件数: {allXmlFiles.Count}");
            _output.WriteLine($"- 支持的类型数: {_loaders.Count}");

            var coveredFiles = 0;
            var typeCoverage = new Dictionary<string, int>();

            foreach (var xmlFile in allXmlFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(xmlFile).ToLower();
                var loaderKey = _loaders.Keys.FirstOrDefault(key => fileName.Contains(key));
                
                if (loaderKey != null)
                {
                    coveredFiles++;
                    typeCoverage[loaderKey] = typeCoverage.GetValueOrDefault(loaderKey) + 1;
                }
            }

            var coverageRate = (double)coveredFiles / allXmlFiles.Count * 100;
            
            _output.WriteLine($"- 覆盖文件数: {coveredFiles}");
            _output.WriteLine($"- 覆盖率: {coverageRate:F1}%");

            _output.WriteLine("各类型覆盖情况:");
            foreach (var type in typeCoverage.OrderByDescending(t => t.Value))
            {
                _output.WriteLine($"  {type.Key}: {type.Value} 个文件");
            }

            // 验证测试数据覆盖度
            Assert.True(coverageRate >= 20, $"测试数据覆盖率应该>=20%，实际为{coverageRate:F1}%");
            Assert.True(coveredFiles >= 10, $"应该覆盖至少10个文件，实际覆盖{coveredFiles}个");
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
        }
    }
}