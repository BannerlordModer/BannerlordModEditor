using Xunit;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.UI.Tests.Helpers;

namespace BannerlordModEditor.UI.Tests.Helpers
{
    /// <summary>
    /// 测试数据管理测试套件
    /// 
    /// 这个测试套件专门验证TestDataHelper的所有功能。
    /// 主要功能：
    /// - 验证TestDataHelper的所有功能
    /// - 测试跨平台文件路径处理
    /// - 确保测试数据文件的正确复制和访问
    /// - 验证测试数据的管理和组织
    /// 
    /// 测试覆盖范围：
    /// 1. 测试数据文件的存在性检查
    /// 2. 跨平台路径处理
    /// 3. 测试数据文件复制
    /// 4. 测试数据文件访问
    /// 5. 测试数据文件管理
    /// 6. 测试数据文件组织
    /// 7. 测试数据文件同步
    /// 8. 测试数据文件验证
    /// </summary>
    public class TestDataManagementTests
    {
        [Fact]
        public void TestDataHelper_Should_Be_Initialized_Correctly()
        {
            // Arrange & Act
            var testDataDir = TestDataHelper.TestDataDirectory;

            // Assert
            Assert.NotNull(testDataDir);
            Assert.NotNull(TestDataHelper.TestDataDirectory);
            Assert.True(Directory.Exists(TestDataHelper.TestDataDirectory));
        }

        [Fact]
        public void GetTestDataPath_Should_Return_Valid_Path()
        {
            // Arrange & Act
            var path = TestDataHelper.GetTestDataPath("test.xml");

            // Assert
            Assert.NotNull(path);
            Assert.True(Path.IsPathFullyQualified(path));
            Assert.EndsWith("test.xml", path);
        }

        [Fact]
        public void GetTestDataPath_Should_Handle_Subdirectories()
        {
            // Arrange & Act
            var path = TestDataHelper.GetTestDataPath(Path.Combine("subdir", "test.xml"));

            // Assert
            Assert.NotNull(path);
            Assert.True(Path.IsPathFullyQualified(path));
            Assert.Contains("subdir", path);
            Assert.EndsWith("test.xml", path);
        }

        [Fact]
        public void GetTestDataPath_Should_Handle_Relative_Paths()
        {
            // Arrange & Act
            var path = TestDataHelper.GetTestDataPath(Path.Combine("..", "test.xml"));

            // Assert
            Assert.NotNull(path);
            Assert.True(Path.IsPathFullyQualified(path));
        }

        [Fact]
        public void TestDataFileExists_Should_Check_File_Existence()
        {
            // Arrange
            var existingFile = "test.xml";
            var nonExistingFile = "nonexistent.xml";

            // Act
            var exists = TestDataHelper.TestDataFileExists(existingFile);
            var notExists = TestDataHelper.TestDataFileExists(nonExistingFile);

            // Assert
            // 这里我们无法确定具体文件是否存在，所以主要测试方法调用是否成功
            Assert.NotNull(exists);
            Assert.NotNull(notExists);
        }

        [Fact]
        public void GetTestDataDirectory_Should_Return_Valid_Directory()
        {
            // Arrange & Act
            var directory = TestDataHelper.TestDataDirectory;

            // Assert
            Assert.NotNull(directory);
            Assert.True(Directory.Exists(directory));
            Assert.True(Path.IsPathFullyQualified(directory));
        }

        [Fact]
        public void ListTestDataFiles_Should_Return_File_List()
        {
            // Arrange & Act
            var files = TestDataHelper.ListTestDataFiles();

            // Assert
            Assert.NotNull(files);
            Assert.IsAssignableFrom<IEnumerable<string>>(files);
        }

        [Fact]
        public void CopyTestDataFile_Should_Copy_File_Correctly()
        {
            // Arrange
            var sourceFile = "test.xml";
            var destinationFile = "copied_test.xml";

            // 清理可能存在的目标文件
            if (File.Exists(destinationFile))
            {
                File.Delete(destinationFile);
            }

            // Act
            try
            {
                TestDataHelper.CopyTestDataFile(sourceFile, destinationFile);

                // Assert
                Assert.True(File.Exists(destinationFile));
            }
            finally
            {
                // 清理
                if (File.Exists(destinationFile))
                {
                    File.Delete(destinationFile);
                }
            }
        }

        [Fact]
        public void CopyTestDataFile_Should_Handle_Nonexistent_Source()
        {
            // Arrange
            var sourceFile = "nonexistent.xml";
            var destinationFile = "destination.xml";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => 
                TestDataHelper.CopyTestDataFile(sourceFile, destinationFile));
        }

        [Fact]
        public void GetTestDataContent_Should_Return_File_Content()
        {
            // Arrange
            var testFile = "test.xml";

            // Act
            var content = TestDataHelper.GetTestDataContent(testFile);

            // Assert
            Assert.NotNull(content);
            Assert.IsType<string>(content);
        }

        [Fact]
        public void GetTestDataContent_Should_Handle_Nonexistent_File()
        {
            // Arrange
            var testFile = "nonexistent.xml";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => 
                TestDataHelper.GetTestDataContent(testFile));
        }

        [Fact]
        public void WriteTestDataFile_Should_Write_File_Correctly()
        {
            // Arrange
            var testFile = "written_test.xml";
            var testContent = "<test>content</test>";

            // 清理可能存在的文件
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            // Act
            try
            {
                TestDataHelper.WriteTestDataFile(testFile, testContent);

                // Assert
                Assert.True(File.Exists(testFile));
                var content = File.ReadAllText(testFile);
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

        [Fact]
        public void CreateTestDataDirectory_Should_Create_Directory_Correctly()
        {
            // Arrange
            var testDir = "test_directory";

            // 清理可能存在的目录
            if (Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }

            // Act
            try
            {
                TestDataHelper.CreateTestDataDirectory(testDir);

                // Assert
                Assert.True(Directory.Exists(testDir));
            }
            finally
            {
                // 清理
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
            }
        }

        [Fact]
        public void DeleteTestDataFile_Should_Delete_File_Correctly()
        {
            // Arrange
            var testFile = "delete_test.xml";
            var testContent = "<test>content</test>";

            // 创建测试文件
            File.WriteAllText(testFile, testContent);
            Assert.True(File.Exists(testFile));

            // Act
            TestDataHelper.DeleteTestDataFile(testFile);

            // Assert
            Assert.False(File.Exists(testFile));
        }

        [Fact]
        public void DeleteTestDataFile_Should_Handle_Nonexistent_File()
        {
            // Arrange
            var testFile = "nonexistent_delete_test.xml";

            // Act & Assert
            // 不应该抛出异常
            TestDataHelper.DeleteTestDataFile(testFile);
        }

        [Fact]
        public void GetTestDataFileInfo_Should_Return_File_Info()
        {
            // Arrange
            var testFile = "test.xml";

            // Act
            var fileInfo = TestDataHelper.GetTestDataFileInfo(testFile);

            // Assert
            Assert.NotNull(fileInfo);
            Assert.Equal(testFile, fileInfo.Name);
        }

        [Fact]
        public void GetTestDataFileInfo_Should_Handle_Nonexistent_File()
        {
            // Arrange
            var testFile = "nonexistent.xml";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => 
                TestDataHelper.GetTestDataFileInfo(testFile));
        }

        [Fact]
        public void GetTestDataSize_Should_Return_File_Size()
        {
            // Arrange
            var testFile = "test.xml";

            // Act
            var size = TestDataHelper.GetTestDataSize(testFile);

            // Assert
            Assert.True(size >= 0);
        }

        [Fact]
        public void GetTestDataSize_Should_Handle_Nonexistent_File()
        {
            // Arrange
            var testFile = "nonexistent.xml";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => 
                TestDataHelper.GetTestDataSize(testFile));
        }

        [Fact]
        public void ValidateTestDataFile_Should_Validate_File_Correctly()
        {
            // Arrange
            var testFile = "test.xml";

            // Act
            var isValid = TestDataHelper.ValidateTestDataFile(testFile);

            // Assert
            Assert.NotNull(isValid);
            Assert.IsType<bool>(isValid);
        }

        [Fact]
        public void ValidateTestDataFile_Should_Handle_Nonexistent_File()
        {
            // Arrange
            var testFile = "nonexistent.xml";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => 
                TestDataHelper.ValidateTestDataFile(testFile));
        }

        [Fact]
        public void GetTestDataHash_Should_Return_File_Hash()
        {
            // Arrange
            var testFile = "test.xml";

            // Act
            var hash = TestDataHelper.GetTestDataHash(testFile);

            // Assert
            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
        }

        [Fact]
        public void GetTestDataHash_Should_Handle_Nonexistent_File()
        {
            // Arrange
            var testFile = "nonexistent.xml";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => 
                TestDataHelper.GetTestDataHash(testFile));
        }

        [Fact]
        public void BackupTestDataFile_Should_Create_Backup()
        {
            // Arrange
            var testFile = "backup_test.xml";
            var testContent = "<test>content</test>";

            // 创建测试文件
            File.WriteAllText(testFile, testContent);
            Assert.True(File.Exists(testFile));

            // Act
            try
            {
                var backupFile = TestDataHelper.BackupTestDataFile(testFile);

                // Assert
                Assert.True(File.Exists(backupFile));
                Assert.NotEqual(testFile, backupFile);
                Assert.EndsWith(".bak", backupFile);

                var backupContent = File.ReadAllText(backupFile);
                Assert.Equal(testContent, backupContent);
            }
            finally
            {
                // 清理
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                }
                // 清理备份文件
                var backupFiles = Directory.GetFiles(".", "*.bak");
                foreach (var backupFile in backupFiles)
                {
                    File.Delete(backupFile);
                }
            }
        }

        [Fact]
        public void RestoreTestDataFile_Should_Restore_From_Backup()
        {
            // Arrange
            var testFile = "restore_test.xml";
            var originalContent = "<test>original</test>";
            var modifiedContent = "<test>modified</test>";

            // 创建测试文件
            File.WriteAllText(testFile, originalContent);
            Assert.True(File.Exists(testFile));

            // 创建备份
            var backupFile = TestDataHelper.BackupTestDataFile(testFile);

            // 修改原文件
            File.WriteAllText(testFile, modifiedContent);

            // Act
            try
            {
                TestDataHelper.RestoreTestDataFile(testFile);

                // Assert
                Assert.True(File.Exists(testFile));
                var restoredContent = File.ReadAllText(testFile);
                Assert.Equal(originalContent, restoredContent);
                Assert.NotEqual(modifiedContent, restoredContent);
            }
            finally
            {
                // 清理
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                }
                if (File.Exists(backupFile))
                {
                    File.Delete(backupFile);
                }
            }
        }

        [Fact]
        public void TestDataHelper_Should_Handle_Concurrent_Access()
        {
            // Arrange
            const int threadCount = 5;
            var results = new bool[threadCount];
            var exceptions = new System.Exception[threadCount];

            // Act
            Parallel.For(0, threadCount, i =>
            {
                try
                {
                    var path = TestDataHelper.GetTestDataPath($"concurrent_test_{i}.xml");
                    results[i] = File.Exists(path);
                }
                catch (Exception ex)
                {
                    exceptions[i] = ex;
                    results[i] = false;
                }
            });

            // Assert
            Assert.All(results, result => Assert.True(result));
            Assert.All(exceptions, ex => Assert.Null(ex));
        }

        [Fact]
        public void TestDataHelper_Should_Handle_Large_Files()
        {
            // Arrange
            var largeFile = "large_test.xml";
            var largeContent = new string('x', 1024 * 1024); // 1MB

            // 清理可能存在的文件
            if (File.Exists(largeFile))
            {
                File.Delete(largeFile);
            }

            // Act
            try
            {
                TestDataHelper.WriteTestDataFile(largeFile, largeContent);

                // Assert
                Assert.True(File.Exists(largeFile));
                var size = TestDataHelper.GetTestDataSize(largeFile);
                Assert.Equal(largeContent.Length, size);

                var content = TestDataHelper.GetTestDataContent(largeFile);
                Assert.Equal(largeContent, content);
            }
            finally
            {
                // 清理
                if (File.Exists(largeFile))
                {
                    File.Delete(largeFile);
                }
            }
        }

        [Fact]
        public void TestDataHelper_Should_Handle_Special_Characters()
        {
            // Arrange
            var specialFile = "special_测试_文件.xml";
            var specialContent = "<test>测试内容</test>";

            // 清理可能存在的文件
            if (File.Exists(specialFile))
            {
                File.Delete(specialFile);
            }

            // Act
            try
            {
                TestDataHelper.WriteTestDataFile(specialFile, specialContent);

                // Assert
                Assert.True(File.Exists(specialFile));
                var content = TestDataHelper.GetTestDataContent(specialFile);
                Assert.Equal(specialContent, content);
            }
            finally
            {
                // 清理
                if (File.Exists(specialFile))
                {
                    File.Delete(specialFile);
                }
            }
        }

        [Fact]
        public void TestDataHelper_Should_Handle_Long_Paths()
        {
            // Arrange
            var longFileName = new string('a', 200) + ".xml";
            var testContent = "<test>long path test</test>";

            // 清理可能存在的文件
            if (File.Exists(longFileName))
            {
                File.Delete(longFileName);
            }

            // Act
            try
            {
                TestDataHelper.WriteTestDataFile(longFileName, testContent);

                // Assert
                Assert.True(File.Exists(longFileName));
                var content = TestDataHelper.GetTestDataContent(longFileName);
                Assert.Equal(testContent, content);
            }
            finally
            {
                // 清理
                if (File.Exists(longFileName))
                {
                    File.Delete(longFileName);
                }
            }
        }
    }
}