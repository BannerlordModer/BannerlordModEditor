using BannerlordModEditor.Common.Services;
using BannerlordModEditor.XmlAdaptationChecker;
using BannerlordModEditor.XmlAdaptationChecker.Core;
using BannerlordModEditor.XmlAdaptationChecker.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using XmlChecker = BannerlordModEditor.XmlAdaptationChecker.Core.XmlAdaptationChecker;

namespace BannerlordModEditor.XmlAdaptationChecker.Tests.Mocks
{
    /// <summary>
    /// 测试数据和模拟对象的工厂类
    /// </summary>
    public static class TestDataFactory
    {
        /// <summary>
        /// 创建模拟的文件发现服务
        /// </summary>
        public static Mock<IFileDiscoveryService> CreateMockFileDiscoveryService()
        {
            var mock = new Mock<IFileDiscoveryService>();

            // 设置默认行为
            mock.Setup(s => s.IsFileAdapted(It.IsAny<string>()))
                .Returns<string>(fileName => fileName.Contains("adapted"));

            mock.Setup(s => s.ConvertToModelName(It.IsAny<string>()))
                .Returns<string>(fileName => 
                {
                    var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    return string.Join("", nameWithoutExtension.Split('_')
                        .Select(word => char.ToUpper(word[0]) + word.Substring(1)));
                });

            return mock;
        }

        /// <summary>
        /// 创建模拟的配置验证器
        /// </summary>
        public static Mock<IConfigurationValidator> CreateMockConfigurationValidator(bool isValid = true)
        {
            var mock = new Mock<IConfigurationValidator>();

            if (isValid)
            {
                mock.Setup(v => v.Validate(It.IsAny<AdaptationCheckerConfiguration>()))
                    .Returns(new ValidationResult { IsValid = true });
            }
            else
            {
                mock.Setup(v => v.Validate(It.IsAny<AdaptationCheckerConfiguration>()))
                    .Returns(new ValidationResult 
                    { 
                        IsValid = false, 
                        Errors = new List<string> { "配置验证失败" } 
                    });
            }

            return mock;
        }

        /// <summary>
        /// 创建模拟的日志记录器
        /// </summary>
        public static Mock<ILogger<T>> CreateMockLogger<T>()
        {
            return new Mock<ILogger<T>>();
        }

        /// <summary>
        /// 创建有效的配置
        /// </summary>
        public static AdaptationCheckerConfiguration CreateValidConfiguration()
        {
            return new AdaptationCheckerConfiguration
            {
                XmlDirectory = "/test/xml",
                ModelDirectories = new List<string> { "/test/models" },
                OutputDirectory = "/test/output",
                OutputFormats = new List<OutputFormat> { OutputFormat.Console, OutputFormat.Json },
                VerboseLogging = false,
                EnableParallelProcessing = true,
                MaxParallelism = 4,
                FileSizeThreshold = 1024 * 1024,
                AnalyzeComplexity = true,
                GenerateStatistics = true,
                ExcludePatterns = new List<string> { "temp_" }
            };
        }

        /// <summary>
        /// 创建无效的配置
        /// </summary>
        public static AdaptationCheckerConfiguration CreateInvalidConfiguration()
        {
            return new AdaptationCheckerConfiguration
            {
                XmlDirectory = "",
                ModelDirectories = new List<string>(),
                OutputFormats = new List<OutputFormat>(),
                MaxParallelism = 0,
                FileSizeThreshold = -1
            };
        }

        /// <summary>
        /// 创建测试用的检查结果
        /// </summary>
        public static XmlChecker.AdaptationCheckResult CreateTestResult(
            int totalFiles = 10,
            int adaptedFiles = 7,
            int unadaptedFiles = 3)
        {
            return new XmlChecker.AdaptationCheckResult
            {
                CheckTime = DateTime.Now,
                TotalFiles = totalFiles,
                AdaptedFiles = adaptedFiles,
                UnadaptedFiles = unadaptedFiles,
                AdaptedFileInfos = CreateAdaptedFileInfos(adaptedFiles),
                UnadaptedFileInfos = CreateUnadaptedFileInfos(unadaptedFiles),
                Errors = new List<string>()
            };
        }

        /// <summary>
        /// 创建带有错误的检查结果
        /// </summary>
        public static XmlChecker.AdaptationCheckResult CreateErrorResult(params string[] errors)
        {
            return new XmlChecker.AdaptationCheckResult
            {
                CheckTime = DateTime.Now,
                TotalFiles = 0,
                AdaptedFiles = 0,
                UnadaptedFiles = 0,
                AdaptedFileInfos = new List<XmlChecker.AdaptedFileInfo>(),
                UnadaptedFileInfos = new List<XmlChecker.UnadaptedFileInfo>(),
                Errors = errors.ToList()
            };
        }

        /// <summary>
        /// 创建适配文件信息列表
        /// </summary>
        public static List<XmlChecker.AdaptedFileInfo> CreateAdaptedFileInfos(int count)
        {
            var list = new List<XmlChecker.AdaptedFileInfo>();
            
            for (int i = 0; i < count; i++)
            {
                list.Add(new XmlChecker.AdaptedFileInfo
                {
                    FileName = $"adapted_file_{i}.xml",
                    FullPath = $"/test/adapted_file_{i}.xml",
                    FileSize = 1024 * (i + 1),
                    ModelName = $"AdaptedFile{i}"
                });
            }

            return list;
        }

        /// <summary>
        /// 创建未适配文件信息列表
        /// </summary>
        public static List<XmlChecker.UnadaptedFileInfo> CreateUnadaptedFileInfos(int count)
        {
            var list = new List<XmlChecker.UnadaptedFileInfo>();
            var complexities = new[] { AdaptationComplexity.Simple, AdaptationComplexity.Medium, AdaptationComplexity.Complex, AdaptationComplexity.Large };
            
            for (int i = 0; i < count; i++)
            {
                list.Add(new XmlChecker.UnadaptedFileInfo
                {
                    FileName = $"unadapted_file_{i}.xml",
                    FullPath = $"/test/unadapted_file_{i}.xml",
                    FileSize = 1024 * (i + 1),
                    ExpectedModelName = $"UnadaptedFile{i}",
                    Complexity = complexities[i % complexities.Length]
                });
            }

            return list;
        }

        /// <summary>
        /// 创建测试XML文件内容
        /// </summary>
        public static string CreateTestXmlContent(string rootName = "root", int elementCount = 5)
        {
            var elements = new List<string>();
            
            for (int i = 0; i < elementCount; i++)
            {
                elements.Add($"    <element id=\"{i}\">Value {i}</element>");
            }

            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<{rootName}>
{string.Join("\n", elements)}
</{rootName}>";
        }

        /// <summary>
        /// 创建大型测试XML文件内容
        /// </summary>
        public static string CreateLargeTestXmlContent(int lineCount = 1000)
        {
            var lines = new List<string>
            {
                @"<?xml version=""1.0"" encoding=""utf-8""?>",
                @"<root>",
                @"    <items>"
            };

            for (int i = 0; i < lineCount - 6; i++)
            {
                lines.Add($@"        <item id=""{i}""><name>Item {i}</name><value>{i}</value></item>");
            }

            lines.AddRange(new[]
            {
                @"    </items>",
                @"    <metadata>",
                @"        <count>" + (lineCount - 6) + @"</count>",
                @"    </metadata>",
                @"</root>"
            });

            return string.Join("\n", lines);
        }

        /// <summary>
        /// 创建测试文件系统
        /// </summary>
        public static TestFileSystem CreateTestFileSystem()
        {
            return new TestFileSystem();
        }

        /// <summary>
        /// 创建测试目录结构
        /// </summary>
        public static string CreateTestDirectoryStructure()
        {
            var basePath = Path.Combine(Path.GetTempPath(), $"XmlAdaptationChecker_Test_{Guid.NewGuid()}");
            
            Directory.CreateDirectory(basePath);
            Directory.CreateDirectory(Path.Combine(basePath, "xml"));
            Directory.CreateDirectory(Path.Combine(basePath, "models"));
            Directory.CreateDirectory(Path.Combine(basePath, "output"));

            return basePath;
        }

        /// <summary>
        /// 清理测试目录结构
        /// </summary>
        public static void CleanupTestDirectoryStructure(string basePath)
        {
            if (Directory.Exists(basePath))
            {
                Directory.Delete(basePath, true);
            }
        }

        /// <summary>
        /// 创建测试XML文件
        /// </summary>
        public static void CreateTestXmlFiles(string directory, int fileCount = 10)
        {
            for (int i = 0; i < fileCount; i++)
            {
                var fileName = i % 3 == 0 ? $"adapted_file_{i}.xml" : $"unadapted_file_{i}.xml";
                var filePath = Path.Combine(directory, fileName);
                var content = CreateTestXmlContent("root", 5);
                File.WriteAllText(filePath, content);
            }
        }

        /// <summary>
        /// 创建验证结果
        /// </summary>
        public static ValidationResult CreateValidationResult(bool isValid = true, params string[] errors)
        {
            return new ValidationResult
            {
                IsValid = isValid,
                Errors = errors.ToList()
            };
        }

        /// <summary>
        /// 创建复杂的测试结果
        /// </summary>
        public static XmlChecker.AdaptationCheckResult CreateComplexTestResult()
        {
            return new XmlChecker.AdaptationCheckResult
            {
                CheckTime = DateTime.Now,
                TotalFiles = 100,
                AdaptedFiles = 70,
                UnadaptedFiles = 30,
                AdaptedFileInfos = CreateAdaptedFileInfos(70),
                UnadaptedFileInfos = CreateUnadaptedFileInfos(30),
                Errors = new List<string>()
            };
        }

        /// <summary>
        /// 创建空的测试结果
        /// </summary>
        public static XmlChecker.AdaptationCheckResult CreateEmptyTestResult()
        {
            return new XmlChecker.AdaptationCheckResult
            {
                CheckTime = DateTime.Now,
                TotalFiles = 0,
                AdaptedFiles = 0,
                UnadaptedFiles = 0,
                AdaptedFileInfos = new List<XmlChecker.AdaptedFileInfo>(),
                UnadaptedFileInfos = new List<XmlChecker.UnadaptedFileInfo>(),
                Errors = new List<string>()
            };
        }
    }

    /// <summary>
    /// 测试文件系统模拟类
    /// </summary>
    public class TestFileSystem
    {
        private readonly Dictionary<string, string> _files = new Dictionary<string, string>();
        private readonly HashSet<string> _directories = new HashSet<string>();

        public TestFileSystem()
        {
            // 初始化根目录
            _directories.Add("/");
        }

        public void CreateDirectory(string path)
        {
            _directories.Add(path);
        }

        public bool DirectoryExists(string path)
        {
            return _directories.Contains(path);
        }

        public void CreateFile(string path, string content)
        {
            _files[path] = content;
        }

        public bool FileExists(string path)
        {
            return _files.ContainsKey(path);
        }

        public string ReadFile(string path)
        {
            return _files.TryGetValue(path, out var content) ? content : string.Empty;
        }

        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            var directory = path.EndsWith("/") ? path : path + "/";
            return _files.Keys
                .Where(f => f.StartsWith(directory) && f.EndsWith(".xml"))
                .ToArray();
        }

        public void WriteFile(string path, string content)
        {
            _files[path] = content;
        }

        public void DeleteFile(string path)
        {
            _files.Remove(path);
        }

        public long GetFileSize(string path)
        {
            return _files.TryGetValue(path, out var content) ? content.Length : 0;
        }

        public void Clear()
        {
            _files.Clear();
            _directories.Clear();
            _directories.Add("/");
        }
    }

    /// <summary>
    /// 测试用的验证结果扩展方法
    /// </summary>
    public static class ValidationResultExtensions
    {
        public static void ShouldBeValid(this ValidationResult result)
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        public static void ShouldBeInvalid(this ValidationResult result, params string[] expectedErrors)
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            
            if (expectedErrors.Length > 0)
            {
                result.Errors.Should().Contain(expectedErrors);
            }
        }

        public static void ShouldContainError(this ValidationResult result, string expectedError)
        {
            result.Errors.Should().Contain(expectedError);
        }

        public static void ShouldContainErrorContaining(this ValidationResult result, string expectedPartialError)
        {
            result.Errors.Should().Contain(e => e.Contains(expectedPartialError));
        }
    }

    /// <summary>
    /// 测试用的检查结果扩展方法
    /// </summary>
    public static class AdaptationCheckResultExtensions
    {
        public static void ShouldHaveNoErrors(this XmlChecker.AdaptationCheckResult result)
        {
            result.Errors.Should().BeEmpty();
        }

        public static void ShouldHaveErrors(this XmlChecker.AdaptationCheckResult result, int expectedErrorCount)
        {
            result.Errors.Should().HaveCount(expectedErrorCount);
        }

        public static void ShouldHaveAdaptationRate(this XmlChecker.AdaptationCheckResult result, double expectedRate)
        {
            result.AdaptationRate.Should().BeApproximately(expectedRate, 0.1);
        }

        public static void ShouldHaveFileCount(this XmlChecker.AdaptationCheckResult result, int expectedTotal, int expectedAdapted, int expectedUnadapted)
        {
            result.TotalFiles.Should().Be(expectedTotal);
            result.AdaptedFiles.Should().Be(expectedAdapted);
            result.UnadaptedFiles.Should().Be(expectedUnadapted);
        }

        public static void ShouldContainUnadaptedFile(this XmlChecker.AdaptationCheckResult result, string fileName)
        {
            result.UnadaptedFileInfos.Should().Contain(f => f.FileName == fileName);
        }

        public static void ShouldContainAdaptedFile(this XmlChecker.AdaptationCheckResult result, string fileName)
        {
            result.AdaptedFileInfos.Should().Contain(f => f.FileName == fileName);
        }
    }
}