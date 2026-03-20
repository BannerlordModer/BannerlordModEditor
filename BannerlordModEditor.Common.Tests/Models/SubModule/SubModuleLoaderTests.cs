using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using BannerlordModEditor.Common.Models.SubModule;

namespace BannerlordModEditor.Common.Tests.Models.SubModule
{
    public class SubModuleLoaderTests
    {
        private readonly SubModuleLoader _loader;

        public SubModuleLoaderTests()
        {
            _loader = new SubModuleLoader();
        }

        [Fact]
        public void Load_ReturnsNullForNonExistentFile()
        {
            var result = _loader.Load("/nonexistent/path/SubModule.xml");
            Assert.Null(result);
        }

        [Fact]
        public async Task LoadAsync_ReturnsNullForNonExistentFile()
        {
            var result = await _loader.LoadAsync("/nonexistent/path/SubModule.xml");
            Assert.Null(result);
        }

        [Fact]
        public void Load_ReadsValidSubModuleXml()
        {
            var tempFile = Path.GetTempFileName();
            var xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Module>
  <Name>Test Mod</Name>
  <Id>TestMod</Id>
  <Version>v1.0.0</Version>
  <SingleplayerModule>true</SingleplayerModule>
  <MultiplayerModule>false</MultiplayerModule>
  <DependedModules>
    <Module Id=""Native""/>
  </DependedModules>
  <SubModules>
    <SubModule>
      <Name>TestSubModule</Name>
      <DLLName>TestMod.dll</DLLName>
      <SubModuleClassType>TestMod.SubModule</SubModuleClassType>
      <IsOptional>false</IsOptional>
      <IsTicked>true</IsTicked>
      <Tags>
        <Tag Key=""DedicatedServerType"" Value=""none""/>
      </Tags>
    </SubModule>
  </SubModules>
</Module>";

            try
            {
                File.WriteAllText(tempFile, xml);

                var result = _loader.Load(tempFile);

                Assert.NotNull(result);
                Assert.Equal("Test Mod", result.Name);
                Assert.Equal("TestMod", result.Id);
                Assert.Equal("v1.0.0", result.Version);
                Assert.True(result.SingleplayerModule);
                Assert.False(result.MultiplayerModule);
                Assert.Single(result.DependedModules);
                Assert.Equal("Native", result.DependedModules[0].Id);
                Assert.Single(result.SubModules);
                Assert.Equal("TestSubModule", result.SubModules[0].Name);
                Assert.Equal("TestMod.dll", result.SubModules[0].DLLName);
                Assert.Single(result.SubModules[0].Tags);
                Assert.Equal("DedicatedServerType", result.SubModules[0].Tags[0].Key);
                Assert.Equal("none", result.SubModules[0].Tags[0].Value);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void Save_CreatesValidXml()
        {
            var tempFile = Path.GetTempFileName();
            var data = new SubModuleDO
            {
                Name = "Save Test Mod",
                Id = "SaveTestMod",
                Version = "v2.0.0",
                SingleplayerModule = true,
                MultiplayerModule = false,
                DependedModules = new()
                {
                    new DependedModuleDO { Id = "Native" },
                    new DependedModuleDO { Id = "Sandbox" }
                },
                SubModules = new()
                {
                    new SubModuleItemDO
                    {
                        Name = "SaveTestSubModule",
                        DLLName = "SaveTestMod.dll",
                        SubModuleClassType = "SaveTestMod.SubModule",
                        IsOptional = false,
                        IsTicked = true,
                        Tags = new()
                        {
                            new SubModuleTagDO { Key = "DedicatedServerType", Value = "none" }
                        }
                    }
                }
            };

            try
            {
                _loader.Save(data, tempFile);

                Assert.True(File.Exists(tempFile));
                var loadedData = _loader.Load(tempFile);
                Assert.NotNull(loadedData);
                Assert.Equal("Save Test Mod", loadedData.Name);
                Assert.Equal("SaveTestMod", loadedData.Id);
                Assert.Equal(2, loadedData.DependedModules.Count);
                Assert.Single(loadedData.SubModules);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public async Task SaveAsync_CreatesValidXml()
        {
            var tempFile = Path.GetTempFileName();
            var data = _loader.CreateDefault("Async Test Mod", "AsyncTestMod");

            try
            {
                await _loader.SaveAsync(data, tempFile);

                Assert.True(File.Exists(tempFile));
                var loadedData = await _loader.LoadAsync(tempFile);
                Assert.NotNull(loadedData);
                Assert.Equal("Async Test Mod", loadedData.Name);
                Assert.Equal("AsyncTestMod", loadedData.Id);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void CreateDefault_CreatesValidDefault()
        {
            var result = _loader.CreateDefault("Default Test", "DefaultTest");

            Assert.Equal("Default Test", result.Name);
            Assert.Equal("DefaultTest", result.Id);
            Assert.Equal("v1.0.0", result.Version);
            Assert.True(result.SingleplayerModule);
            Assert.False(result.MultiplayerModule);
            Assert.Empty(result.DependedModules);
            Assert.Empty(result.SubModules);
            Assert.Empty(result.Xmls);
            Assert.Empty(result.OptionalDependedModules);
        }
    }
}
