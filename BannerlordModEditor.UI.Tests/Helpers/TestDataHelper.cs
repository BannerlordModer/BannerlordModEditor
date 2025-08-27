using System.IO;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace BannerlordModEditor.UI.Tests.Helpers
{
    /// <summary>
    /// 测试数据路径帮助类
    /// 提供跨平台的测试数据路径处理功能
    /// </summary>
    public static class TestDataHelper
    {
        /// <summary>
        /// 获取测试数据目录路径
        /// </summary>
        public static string TestDataDirectory => GetTestDataDirectoryPath();
        /// <summary>
        /// 获取测试数据文件的完整路径
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>跨平台兼容的完整路径</returns>
        /// <exception cref="ArgumentNullException">当文件名为null或空时抛出</exception>
        /// <exception cref="ArgumentException">当文件名包含非法字符时抛出</exception>
        public static string GetTestDataPath(string fileName)
        {
            // 输入验证
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName), "文件名不能为null或空字符串");
            }

            if (fileName.Contains("..") || fileName.Contains("/") || fileName.Contains("\\"))
            {
                throw new ArgumentException("文件名不能包含路径分隔符或相对路径引用", nameof(fileName));
            }

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new ArgumentException("文件名包含非法字符", nameof(fileName));
            }

            return Path.Combine("TestData", fileName);
        }

        /// <summary>
        /// 检查测试数据文件是否存在
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>如果文件存在返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当文件名为null或空时抛出</exception>
        /// <exception cref="ArgumentException">当文件名包含非法字符时抛出</exception>
        public static bool TestDataFileExists(string fileName)
        {
            try
            {
                var fullPath = GetTestDataPath(fileName);
                return File.Exists(fullPath);
            }
            catch (Exception ex) when (ex is ArgumentNullException or ArgumentException)
            {
                // 重新抛出参数验证异常
                throw;
            }
            catch (Exception ex)
            {
                // 对于其他异常，记录日志并返回false
                Console.WriteLine($"检查测试数据文件存在性时发生错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取测试数据文件的完整路径（支持子目录）
        /// </summary>
        /// <param name="relativePath">相对于TestData目录的相对路径</param>
        /// <returns>跨平台兼容的完整路径</returns>
        /// <exception cref="ArgumentNullException">当相对路径为null或空时抛出</exception>
        /// <exception cref="ArgumentException">当相对路径包含非法字符时抛出</exception>
        public static string GetTestDataSubPath(string relativePath)
        {
            // 输入验证
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new ArgumentNullException(nameof(relativePath), "相对路径不能为null或空字符串");
            }

            if (relativePath.Contains(".."))
            {
                throw new ArgumentException("相对路径不能包含上级目录引用", nameof(relativePath));
            }

            // 检查路径中的每个部分是否包含非法字符
            var pathParts = relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            foreach (var part in pathParts)
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    continue; // 允许连续的分隔符
                }
                
                if (part.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    throw new ArgumentException($"路径部分 '{part}' 包含非法字符", nameof(relativePath));
                }
            }

            return Path.Combine("TestData", relativePath);
        }

        /// <summary>
        /// 检查测试数据子目录中的文件是否存在
        /// </summary>
        /// <param name="relativePath">相对于TestData目录的相对路径</param>
        /// <returns>如果文件存在返回true，否则返回false</returns>
        /// <exception cref="ArgumentNullException">当相对路径为null或空时抛出</exception>
        /// <exception cref="ArgumentException">当相对路径包含非法字符时抛出</exception>
        public static bool TestDataSubFileExists(string relativePath)
        {
            try
            {
                var fullPath = GetTestDataSubPath(relativePath);
                return File.Exists(fullPath);
            }
            catch (Exception ex) when (ex is ArgumentNullException or ArgumentException)
            {
                // 重新抛出参数验证异常
                throw;
            }
            catch (Exception ex)
            {
                // 对于其他异常，记录日志并返回false
                Console.WriteLine($"检查测试数据子文件存在性时发生错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取所有必需的测试数据文件列表
        /// </summary>
        /// <returns>必需的测试数据文件名列表</returns>
        public static string[] GetRequiredTestDataFiles()
        {
            return new[]
            {
                "attributes.xml",
                "bone_body_types.xml",
                "skills.xml",
                "module_sounds.xml",
                "crafting_pieces.xml",
                "item_modifiers.xml"
            };
        }

        /// <summary>
        /// 验证所有必需的测试数据文件是否存在
        /// </summary>
        /// <returns>如果所有文件都存在返回true，否则返回false</returns>
        /// <exception cref="InvalidOperationException">当验证过程中发生严重错误时抛出</exception>
        public static bool ValidateRequiredTestDataFiles()
        {
            try
            {
                var requiredFiles = GetRequiredTestDataFiles();
                var missingFiles = new List<string>();
                
                foreach (var file in requiredFiles)
                {
                    try
                    {
                        if (!TestDataFileExists(file))
                        {
                            missingFiles.Add(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"验证文件 '{file}' 时发生错误: {ex.Message}");
                        missingFiles.Add(file);
                    }
                }
                
                if (missingFiles.Count > 0)
                {
                    Console.WriteLine($"缺失的测试数据文件: {string.Join(", ", missingFiles)}");
                }
                
                return missingFiles.Count == 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("验证必需测试数据文件时发生严重错误", ex);
            }
        }

        /// <summary>
        /// 获取缺失的测试数据文件列表
        /// </summary>
        /// <returns>缺失的文件名列表</returns>
        /// <exception cref="InvalidOperationException">当获取缺失文件列表时发生严重错误时抛出</exception>
        public static string[] GetMissingTestDataFiles()
        {
            try
            {
                var requiredFiles = GetRequiredTestDataFiles();
                var missingFiles = new List<string>();
                
                foreach (var file in requiredFiles)
                {
                    try
                    {
                        if (!TestDataFileExists(file))
                        {
                            missingFiles.Add(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"检查文件 '{file}' 时发生错误: {ex.Message}");
                        missingFiles.Add(file);
                    }
                }
                
                return missingFiles.ToArray();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("获取缺失测试数据文件列表时发生严重错误", ex);
            }
        }

        /// <summary>
        /// 获取测试数据文件的详细信息
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件信息对象，如果文件不存在返回null</returns>
        /// <exception cref="ArgumentNullException">当文件名为null或空时抛出</exception>
        /// <exception cref="ArgumentException">当文件名包含非法字符时抛出</exception>
        public static FileInfo? GetTestDataFileInfo(string fileName)
        {
            try
            {
                var fullPath = GetTestDataPath(fileName);
                var fileInfo = new FileInfo(fullPath);
                return fileInfo.Exists ? fileInfo : null;
            }
            catch (Exception ex) when (ex is ArgumentNullException or ArgumentException)
            {
                // 重新抛出参数验证异常
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取测试数据文件信息时发生错误: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 确保TestData目录存在
        /// </summary>
        /// <returns>如果目录创建成功或已存在返回true，否则返回false</returns>
        public static bool EnsureTestDataDirectoryExists()
        {
            try
            {
                var testDataPath = Path.Combine("TestData");
                if (!Directory.Exists(testDataPath))
                {
                    Directory.CreateDirectory(testDataPath);
                    Console.WriteLine("创建TestData目录成功");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建TestData目录失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取测试数据目录的完整路径
        /// </summary>
        /// <returns>测试数据目录的完整路径</returns>
        public static string GetTestDataDirectoryPath()
        {
            return Path.Combine("TestData");
        }

        /// <summary>
        /// 列出TestData目录中的所有文件
        /// </summary>
        /// <returns>文件信息数组</returns>
        /// <exception cref="DirectoryNotFoundException">当TestData目录不存在时抛出</exception>
        public static FileInfo[] ListTestDataFiles()
        {
            try
            {
                var testDataPath = GetTestDataDirectoryPath();
                if (!Directory.Exists(testDataPath))
                {
                    throw new DirectoryNotFoundException($"TestData目录不存在: {testDataPath}");
                }

                var directoryInfo = new DirectoryInfo(testDataPath);
                return directoryInfo.GetFiles();
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"列出TestData目录文件时发生错误: {ex.Message}");
                return Array.Empty<FileInfo>();
            }
        }

        /// <summary>
        /// 获取测试数据文件的完整内容
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件内容字符串</returns>
        /// <exception cref="ArgumentNullException">当文件名为null或空时抛出</exception>
        /// <exception cref="ArgumentException">当文件名包含非法字符时抛出</exception>
        /// <exception cref="FileNotFoundException">当文件不存在时抛出</exception>
        public static string GetTestDataFileContent(string fileName)
        {
            try
            {
                var fullPath = GetTestDataPath(fileName);
                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"测试数据文件不存在: {fullPath}");
                }

                return File.ReadAllText(fullPath, Encoding.UTF8);
            }
            catch (Exception ex) when (ex is ArgumentNullException or ArgumentException or FileNotFoundException)
            {
                // 重新抛出已知异常
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"读取测试数据文件 '{fileName}' 时发生错误", ex);
            }
        }

        /// <summary>
        /// 获取测试数据内容（兼容旧API）
        /// </summary>
        public static string GetTestDataContent(string fileName)
        {
            return GetTestDataFileContent(fileName);
        }

        /// <summary>
        /// 写入测试数据文件
        /// </summary>
        public static void WriteTestDataFile(string fileName, string content)
        {
            try
            {
                var fullPath = GetTestDataPath(fileName);
                File.WriteAllText(fullPath, content, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"写入测试数据文件 '{fileName}' 时发生错误", ex);
            }
        }

        /// <summary>
        /// 验证测试数据文件
        /// </summary>
        public static bool ValidateTestDataFile(string fileName)
        {
            try
            {
                var fullPath = GetTestDataPath(fileName);
                return File.Exists(fullPath) && new FileInfo(fullPath).Length > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取测试数据文件的哈希值
        /// </summary>
        public static string GetTestDataHash(string fileName)
        {
            try
            {
                var fullPath = GetTestDataPath(fileName);
                using var md5 = System.Security.Cryptography.MD5.Create();
                using var stream = File.OpenRead(fullPath);
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// 获取测试数据文件大小
        /// </summary>
        public static long GetTestDataSize(string fileName)
        {
            try
            {
                var fullPath = GetTestDataPath(fileName);
                var fileInfo = new FileInfo(fullPath);
                return fileInfo.Length;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 备份测试数据文件
        /// </summary>
        public static string BackupTestDataFile(string fileName)
        {
            try
            {
                var fullPath = GetTestDataPath(fileName);
                var backupPath = fullPath + ".backup";
                File.Copy(fullPath, backupPath, true);
                return backupPath;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 恢复测试数据文件
        /// </summary>
        public static bool RestoreTestDataFile(string fileName)
        {
            try
            {
                var fullPath = GetTestDataPath(fileName);
                var backupPath = fullPath + ".backup";
                if (File.Exists(backupPath))
                {
                    File.Copy(backupPath, fullPath, true);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 复制测试数据文件
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destinationFileName">目标文件名</param>
        /// <returns>是否成功</returns>
        public static bool CopyTestDataFile(string sourceFileName, string destinationFileName)
        {
            try
            {
                var sourcePath = GetTestDataPath(sourceFileName);
                var destinationPath = GetTestDataPath(destinationFileName);
                
                if (!File.Exists(sourcePath))
                {
                    return false;
                }
                
                File.Copy(sourcePath, destinationPath, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建测试数据目录
        /// </summary>
        /// <param name="directoryName">目录名</param>
        /// <returns>是否成功</returns>
        public static bool CreateTestDataDirectory(string directoryName)
        {
            try
            {
                var directoryPath = Path.Combine(GetTestDataDirectoryPath(), directoryName);
                Directory.CreateDirectory(directoryPath);
                return Directory.Exists(directoryPath);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除测试数据文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>是否成功</returns>
        public static bool DeleteTestDataFile(string fileName)
        {
            try
            {
                var filePath = GetTestDataPath(fileName);
                if (!File.Exists(filePath))
                {
                    return false;
                }
                
                File.Delete(filePath);
                return !File.Exists(filePath);
            }
            catch
            {
                return false;
            }
        }
    }
}