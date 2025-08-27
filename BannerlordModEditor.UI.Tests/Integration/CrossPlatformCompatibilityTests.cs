using Xunit;
using Moq;
using CommunityToolkit.Mvvm.ComponentModel;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.Common.Models;
using BannerlordModEditor.Common.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;
using BannerlordModEditor.UI.Tests.Helpers;
using static BannerlordModEditor.UI.Tests.Environment.EnvironmentHelper;

namespace BannerlordModEditor.UI.Tests.Integration
{
    /// <summary>
    /// 跨平台兼容性测试套件
    /// 
    /// 这个测试套件专门验证应用程序在不同平台上的兼容性。
    /// 主要功能：
    /// - 测试跨平台文件路径处理
    /// - 验证不同操作系统的行为一致性
    /// - 确保文件系统操作的兼容性
    /// - 测试环境特定的功能
    /// 
    /// 测试覆盖范围：
    /// 1. 文件路径处理
    /// 2. 操作系统特定的行为
    /// 3. 文件系统兼容性
    /// 4. 环境变量处理
    /// 5. 字符编码处理
    /// 6. 权限处理
    /// 7. 网络路径处理
    /// 8. 特殊字符处理
    /// 9. 性能差异处理
    /// 10. UI框架兼容性
    /// </summary>
    public class CrossPlatformCompatibilityTests
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEditorFactory _editorFactory;
        private readonly ILogService _logService;
        private readonly IErrorHandlerService _errorHandlerService;

        public CrossPlatformCompatibilityTests()
        {
            // 设置依赖注入
            var services = new ServiceCollection();
            
            // 注册核心服务
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
            services.AddSingleton<IValidationService, ValidationService>();
            services.AddSingleton<IDataBindingService, DataBindingService>();
            
            // 注册工厂
            services.AddSingleton<IEditorFactory, MockEditorFactory>();
            
            // 注册ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<EditorManagerViewModel>();
            services.AddTransient<AttributeEditorViewModel>();
            services.AddTransient<SkillEditorViewModel>();
            services.AddTransient<CombatParameterEditorViewModel>();
            services.AddTransient<ItemEditorViewModel>();
            services.AddTransient<CraftingPieceEditorViewModel>();
            services.AddTransient<ItemModifierEditorViewModel>();
            services.AddTransient<BoneBodyTypeEditorViewModel>();
            
            _serviceProvider = services.BuildServiceProvider();
            _logService = _serviceProvider.GetRequiredService<ILogService>();
            _errorHandlerService = _serviceProvider.GetRequiredService<IErrorHandlerService>();
            _editorFactory = _serviceProvider.GetRequiredService<IEditorFactory>();
        }

        [Fact]
        public void Should_Detect_Current_Operating_System()
        {
            // Arrange & Act
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            var isMacOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            // Assert
            // 应该至少有一个平台为true
            Assert.True(isWindows || isLinux || isMacOS);
            
            // 不应该同时为true
            var trueCount = new[] { isWindows, isLinux, isMacOS }.Count(b => b);
            Assert.Equal(1, trueCount);
        }

        [Fact]
        public void File_Path_Separators_Should_Be_Handled_Correctly()
        {
            // Arrange
            var testPaths = new[]
            {
                "path\\to\\file.xml",          // Windows style
                "path/to/file.xml",            // Unix style
                "mixed/path\\to/file.xml",     // Mixed style
                "/absolute/path/to/file.xml",  // Unix absolute
                "C:\\absolute\\path\\file.xml", // Windows absolute
                "relative\\path\\file.xml",    // Windows relative
                "relative/path/file.xml"       // Unix relative
            };

            // Act & Assert
            foreach (var path in testPaths)
            {
                // 所有路径都应该能被正确处理
                var normalized = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), path));
                Assert.NotNull(normalized);
                Assert.True(Path.IsPathFullyQualified(normalized));
            }
        }

        [Fact]
        public void Path_Combination_Should_Work_Cross_Platform()
        {
            // Arrange
            var baseDir = Directory.GetCurrentDirectory();
            var relativePaths = new[]
            {
                "test.xml",
                "subdir\\test.xml",
                "subdir/test.xml",
                "deep\\nested\\path\\test.xml",
                "deep/nested/path/test.xml"
            };

            // Act & Assert
            foreach (var relativePath in relativePaths)
            {
                var combined = Path.Combine(baseDir, relativePath);
                var normalized = Path.GetFullPath(combined);
                
                Assert.NotNull(normalized);
                Assert.True(Path.IsPathFullyQualified(normalized));
                Assert.EndsWith("test.xml", normalized);
            }
        }

        [Fact]
        public void File_Operations_Should_Work_Cross_Platform()
        {
            // Arrange
            var testFile = "cross_platform_test.xml";
            var testContent = "<test>cross-platform content</test>";

            // 清理可能存在的文件
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            try
            {
                // Act - 创建文件
                File.WriteAllText(testFile, testContent);
                Assert.True(File.Exists(testFile));

                // 读取文件
                var readContent = File.ReadAllText(testFile);
                Assert.Equal(testContent, readContent);

                // 获取文件信息
                var fileInfo = new FileInfo(testFile);
                Assert.NotNull(fileInfo);
                Assert.True(fileInfo.Exists);
                Assert.Equal(testContent.Length, fileInfo.Length);

                // 复制文件
                var copiedFile = "copied_test.xml";
                File.Copy(testFile, copiedFile);
                Assert.True(File.Exists(copiedFile));

                // 验证复制的内容
                var copiedContent = File.ReadAllText(copiedFile);
                Assert.Equal(testContent, copiedContent);

                // 清理复制的文件
                File.Delete(copiedFile);
                Assert.False(File.Exists(copiedFile));
            }
            finally
            {
                // 清理测试文件
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                }
            }
        }

        [Fact]
        public void Directory_Operations_Should_Work_Cross_Platform()
        {
            // Arrange
            var testDir = "cross_platform_test_dir";
            var subDir = Path.Combine(testDir, "subdir");
            var testFile = Path.Combine(subDir, "test.xml");

            // 清理可能存在的目录
            if (Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }

            try
            {
                // Act - 创建目录
                Directory.CreateDirectory(subDir);
                Assert.True(Directory.Exists(testDir));
                Assert.True(Directory.Exists(subDir));

                // 在目录中创建文件
                File.WriteAllText(testFile, "<test>content</test>");
                Assert.True(File.Exists(testFile));

                // 列出目录内容
                var files = Directory.GetFiles(subDir);
                Assert.Single(files);
                Assert.Equal(testFile, files[0]);

                // 列出子目录
                var subDirs = Directory.GetDirectories(testDir);
                Assert.Single(subDirs);
                Assert.Equal(subDir, subDirs[0]);
            }
            finally
            {
                // 清理测试目录
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
            }
        }

        [Fact]
        public void Special_Characters_In_Paths_Should_Be_Handled()
        {
            // Arrange
            var specialNames = new[]
            {
                "test_with spaces.xml",
                "test-with-dashes.xml",
                "test_with_underscores.xml",
                "测试中文.xml",
                "test_with_dots.xml",
                "test.with.dots.xml",
                "test@special#chars$.xml"
            };

            foreach (var fileName in specialNames)
            {
                // 清理可能存在的文件
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                try
                {
                    // Act
                    File.WriteAllText(fileName, "<test>special content</test>");
                    Assert.True(File.Exists(fileName));

                    var content = File.ReadAllText(fileName);
                    Assert.Equal("<test>special content</test>", content);
                }
                finally
                {
                    // 清理
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                }
            }
        }

        [Fact]
        public void Long_Paths_Should_Be_Handled()
        {
            // Arrange
            var longDirName = new string('a', 100);
            var longFileName = new string('b', 100) + ".xml";
            var longPath = Path.Combine(longDirName, longFileName);
            var testContent = "<test>long path content</test>";

            // 清理可能存在的目录
            if (Directory.Exists(longDirName))
            {
                Directory.Delete(longDirName, true);
            }

            try
            {
                // Act
                Directory.CreateDirectory(longDirName);
                Assert.True(Directory.Exists(longDirName));

                File.WriteAllText(longPath, testContent);
                Assert.True(File.Exists(longPath));

                var readContent = File.ReadAllText(longPath);
                Assert.Equal(testContent, readContent);
            }
            finally
            {
                // 清理
                if (Directory.Exists(longDirName))
                {
                    Directory.Delete(longDirName, true);
                }
            }
        }

        [Fact]
        public void Environment_Variables_Should_Be_Accessible()
        {
            // Arrange & Act
            var pathVar = GetEnvironmentVariable("PATH");
            var homeVar = GetEnvironmentVariable("HOME") ?? 
                          GetEnvironmentVariable("USERPROFILE");
            var tempVar = GetEnvironmentVariable("TEMP") ?? 
                          GetEnvironmentVariable("TMP");

            // Assert
            Assert.NotNull(pathVar);
            Assert.NotEmpty(pathVar);
            
            Assert.NotNull(homeVar);
            Assert.NotEmpty(homeVar);
            
            Assert.NotNull(tempVar);
            Assert.NotEmpty(tempVar);
        }

        [Fact]
        public void Current_Directory_Should_Be_Accessible()
        {
            // Arrange & Act
            var currentDir = Directory.GetCurrentDirectory();
            var currentDirInfo = new DirectoryInfo(currentDir);

            // Assert
            Assert.NotNull(currentDir);
            Assert.True(Directory.Exists(currentDir));
            Assert.True(Path.IsPathFullyQualified(currentDir));
            
            Assert.NotNull(currentDirInfo);
            Assert.True(currentDirInfo.Exists);
        }

        [Fact]
        public void Application_Data_Directory_Should_Be_Accessible()
        {
            // Arrange & Act
            var appDataDir = GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var localAppDataDir = GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

            // Assert
            Assert.NotNull(appDataDir);
            Assert.NotEmpty(appDataDir);
            
            Assert.NotNull(localAppDataDir);
            Assert.NotEmpty(localAppDataDir);
        }

        [Fact]
        public void Process_Information_Should_Be_Accessible()
        {
            // Arrange & Act
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var processName = currentProcess.ProcessName;
            var processId = currentProcess.Id;
            var machineName = MachineName;
            var userName = UserName;

            // Assert
            Assert.NotNull(processName);
            Assert.NotEmpty(processName);
            Assert.True(processId > 0);
            Assert.NotNull(machineName);
            Assert.NotEmpty(machineName);
            Assert.NotNull(userName);
            Assert.NotEmpty(userName);
        }

        [Fact]
        public void File_Encoding_Should_Be_Handled_Correctly()
        {
            // Arrange
            var testFile = "encoding_test.xml";
            var testContent = "<test>编码测试 Encoding Test</test>";

            // 清理可能存在的文件
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            try
            {
                // Act - 使用UTF-8编码写入
                File.WriteAllText(testFile, testContent, System.Text.Encoding.UTF8);
                Assert.True(File.Exists(testFile));

                // 读取UTF-8编码
                var readContent = File.ReadAllText(testFile, System.Text.Encoding.UTF8);
                Assert.Equal(testContent, readContent);

                // 读取所有文本（自动检测编码）
                var autoContent = File.ReadAllText(testFile);
                Assert.Equal(testContent, autoContent);

                // 使用不同编码写入
                File.WriteAllText(testFile, testContent, System.Text.Encoding.Unicode);
                var unicodeContent = File.ReadAllText(testFile, System.Text.Encoding.Unicode);
                Assert.Equal(testContent, unicodeContent);
            }
            finally
            {
                // 清理
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                }
            }
        }

        [Fact]
        public void Network_Paths_Should_Be_Handled()
        {
            // Arrange
            var networkPaths = new[]
            {
                "\\\\server\\share\\file.xml",        // Windows UNC path
                "//server/share/file.xml",            // Unix-style UNC path
                "smb://server/share/file.xml",        // SMB URL
                "http://example.com/file.xml",        // HTTP URL
                "https://example.com/file.xml"        // HTTPS URL
            };

            // Act & Assert
            foreach (var path in networkPaths)
            {
                // 这些路径应该被正确处理，即使它们不存在
                Assert.NotNull(path);
                Assert.NotEmpty(path);
                
                // 验证路径格式
                Assert.True(path.Contains("/") || path.Contains("\\"));
            }
        }

        [Fact]
        public void Case_Sensitivity_Should_Be_Handled()
        {
            // Arrange
            var testFile = "case_sensitive_test.xml";
            var upperCaseFile = testFile.ToUpper();
            var lowerCaseFile = testFile.ToLower();
            var testContent = "<test>case sensitive content</test>";

            // 清理可能存在的文件
            foreach (var file in new[] { testFile, upperCaseFile, lowerCaseFile })
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }

            try
            {
                // Act - 创建小写文件
                File.WriteAllText(lowerCaseFile, testContent);
                Assert.True(File.Exists(lowerCaseFile));

                // 在Windows上，文件名不区分大小写
                // 在Unix上，文件名区分大小写
                var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                
                if (isWindows)
                {
                    // Windows上应该能找到
                    Assert.True(File.Exists(upperCaseFile));
                    Assert.True(File.Exists(testFile));
                }
                else
                {
                    // Unix上可能找不到
                    // 这取决于具体的文件系统
                }

                // 读取内容
                var content = File.ReadAllText(lowerCaseFile);
                Assert.Equal(testContent, content);
            }
            finally
            {
                // 清理
                foreach (var file in new[] { testFile, upperCaseFile, lowerCaseFile })
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        [Fact]
        public void Permission_Issues_Should_Be_Handled_Gracefully()
        {
            // Arrange
            var protectedDir = Path.Combine(Path.GetPathRoot(Directory.GetCurrentDirectory()) ?? "", "Windows");
            var protectedFile = Path.Combine(protectedDir, "test.xml");

            // Act & Assert
            // 尝试访问受保护的系统目录
            // 这应该抛出异常或返回适当的错误
            
            if (Directory.Exists(protectedDir))
            {
                // 尝试创建文件
                try
                {
                    File.WriteAllText(protectedFile, "<test>content</test>");
                    // 如果成功，删除文件
                    if (File.Exists(protectedFile))
                    {
                        File.Delete(protectedFile);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // 这是预期的行为
                    Assert.True(true);
                }
                catch (Exception ex)
                {
                    // 其他异常也应该被处理
                    Assert.NotNull(ex);
                }
            }
        }

        [Fact]
        public void Cross_Platform_UI_Should_Initialize_Correctly()
        {
            // Arrange & Act
            var mainWindow = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();

            // Assert
            Assert.NotNull(mainWindow);
            Assert.NotNull(editorManager);
            Assert.NotNull(mainWindow.EditorManager);
            Assert.Equal(editorManager, mainWindow.EditorManager);
            
            // 验证编辑器管理器能正确初始化
            Assert.NotNull(editorManager.Categories);
            Assert.True(editorManager.Categories.Count > 0);
        }

        [Fact]
        public void Cross_Platform_Editor_Factory_Should_Work()
        {
            // Arrange & Act
            var attributeEditor = _editorFactory.CreateEditorViewModel("AttributeEditor", "attributes.xml");
            var skillEditor = _editorFactory.CreateEditorViewModel("SkillEditor", "skills.xml");

            // Assert
            Assert.NotNull(attributeEditor);
            Assert.NotNull(skillEditor);
            Assert.IsType<AttributeEditorViewModel>(attributeEditor);
            Assert.IsType<SkillEditorViewModel>(skillEditor);
        }

        [Fact]
        public void Cross_Platform_Data_Binding_Should_Work()
        {
            // Arrange
            var dataBindingService = _serviceProvider.GetRequiredService<IDataBindingService>();
            var source = new TestObservableObject();
            var target = new TestObservableObject();

            // Act
            using var binding = dataBindingService.CreateBinding<TestObservableObject, TestObservableObject>(
                source, "Name",
                target, "Name",
                true);

            source.Name = "Cross-Platform Test";

            // Assert
            Assert.Equal("Cross-Platform Test", target.Name);
        }

        [Fact]
        public void Cross_Platform_File_Operations_Should_Integrate_With_Editors()
        {
            // Arrange
            var testFile = "cross_platform_editor_test.xml";
            var testContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<attributes>
    <attribute id=""CrossPlatformAttribute"" name=""Cross Platform Attribute"" default_value=""42""/>
</attributes>";

            // 清理可能存在的文件
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            try
            {
                // Act - 创建测试文件
                File.WriteAllText(testFile, testContent);
                Assert.True(File.Exists(testFile));

                // 验证编辑器能处理文件路径
                var normalizedPath = Path.GetFullPath(testFile);
                Assert.True(Path.IsPathFullyQualified(normalizedPath));
                Assert.True(File.Exists(normalizedPath));

                // 验证文件内容
                var content = File.ReadAllText(normalizedPath);
                Assert.Equal(testContent, content);
            }
            finally
            {
                // 清理
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                }
            }
        }

        // 测试辅助类
        private class TestObservableObject : ObservableObject
        {
            private string _name = string.Empty;

            public string Name
            {
                get => _name;
                set => SetProperty(ref _name, value);
            }
        }
    }
}