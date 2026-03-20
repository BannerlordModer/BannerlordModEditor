using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Common.Tests.Services
{
    public class TpacParserServiceTests
    {
        private readonly TpacParserService _service;

        public TpacParserServiceTests()
        {
            _service = new TpacParserService();
        }

        [Fact]
        public void IsTpacFile_ReturnsTrueForTpacExtension()
        {
            Assert.True(_service.IsTpacFile("test.tpac"));
            Assert.True(_service.IsTpacFile("path/to/file.TPAC"));
        }

        [Fact]
        public void IsTpacFile_ReturnsFalseForNonTpacExtension()
        {
            Assert.False(_service.IsTpacFile("test.xml"));
            Assert.False(_service.IsTpacFile("test.txt"));
        }

        [Fact]
        public void ParseTpacFile_ReturnsInvalidForNonExistentFile()
        {
            var result = _service.ParseTpacFile("/nonexistent/file.tpac");
            Assert.False(result.IsValid);
            Assert.Equal("File not found", result.ErrorMessage);
        }

        [Fact]
        public async Task ParseTpacFileAsync_ReturnsInvalidForNonExistentFile()
        {
            var result = await _service.ParseTpacFileAsync("/nonexistent/file.tpac");
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ParseTpacFile_ExtractsModelsFromValidFile()
        {
            var tempFile = Path.GetTempFileName();
            var content = @"model_id: 12345
model_name: TestModel
model_type: Character
property1: value1
property2: value2
model_id: 67890
model_name: AnotherModel
model_type: Item";

            try
            {
                File.WriteAllText(tempFile, content);

                var result = _service.ParseTpacFile(tempFile);

                Assert.True(result.IsValid);
                Assert.Equal(2, result.Models.Count);
                Assert.Equal("12345", result.Models[0].ModelId);
                Assert.Equal("TestModel", result.Models[0].ModelName);
                Assert.Equal("Character", result.Models[0].ModelType);
                Assert.Equal("67890", result.Models[1].ModelId);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public async Task ParseTpacFileAsync_ExtractsModelsFromValidFile()
        {
            var tempFile = Path.GetTempFileName();
            var content = @"model_id: 99999
model_name: AsyncModel
model_type: Weapon";

            try
            {
                File.WriteAllText(tempFile, content);

                var result = await _service.ParseTpacFileAsync(tempFile);

                Assert.True(result.IsValid);
                Assert.Single(result.Models);
                Assert.Equal("99999", result.Models[0].ModelId);
                Assert.Equal("AsyncModel", result.Models[0].ModelName);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void ExtractModels_ReturnsEmptyForNonExistentFile()
        {
            var result = _service.ExtractModels("/nonexistent/file.tpac");
            Assert.Empty(result);
        }
    }
}
