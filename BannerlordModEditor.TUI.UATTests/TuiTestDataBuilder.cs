using System.IO;
using System.Linq;

namespace BannerlordModEditor.TUI.UATTests
{
    /// <summary>
    /// TUI UAT测试数据构建器
    /// </summary>
    public static class TuiTestDataBuilder
    {
        /// <summary>
        /// 创建测试数据目录并复制必要的测试文件
        /// </summary>
        public static void CreateTestData()
        {
            var testDataPath = Path.Combine("BannerlordModEditor.TUI.UATTests", "TestData");
            Directory.CreateDirectory(testDataPath);
            
            // 复制Common.Tests中的TestData文件
            var sourcePath = Path.Combine("BannerlordModEditor.Common.Tests", "TestData");
            if (Directory.Exists(sourcePath))
            {
                CopyDirectory(sourcePath, testDataPath);
                Console.WriteLine($"已复制测试数据从 {sourcePath} 到 {testDataPath}");
            }
            else
            {
                Console.WriteLine($"警告：源测试数据目录不存在: {sourcePath}");
                CreateMinimalTestData(testDataPath);
            }
            
            // 创建额外的测试数据
            CreateAdditionalTestData(testDataPath);
        }
        
        /// <summary>
        /// 创建最小测试数据集
        /// </summary>
        private static void CreateMinimalTestData(string testDataPath)
        {
            // 创建基本的XML测试文件
            var basicXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_types>
    <action id=""test_action"" name=""Test Action"" type=""swing"" usage_direction=""one_handed"" />
</action_types>";
            
            File.WriteAllText(Path.Combine(testDataPath, "action_types.xml"), basicXml);
            
            var combatParamsXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<combat_parameters>
    <combat_parameter name=""test_param"" value=""1.0"" />
</combat_parameters>";
            
            File.WriteAllText(Path.Combine(testDataPath, "combat_parameters.xml"), combatParamsXml);
            
            Console.WriteLine($"已创建最小测试数据集在 {testDataPath}");
        }
        
        /// <summary>
        /// 创建额外的测试数据
        /// </summary>
        private static void CreateAdditionalTestData(string testDataPath)
        {
            // 创建大文件测试数据
            CreateLargeXmlFile(testDataPath);
            
            // 创建无效XML文件
            CreateInvalidXmlFile(testDataPath);
            
            // 创建空文件
            CreateEmptyFiles(testDataPath);
            
            Console.WriteLine($"已创建额外的测试数据在 {testDataPath}");
        }
        
        /// <summary>
        /// 创建大XML文件用于性能测试
        /// </summary>
        private static void CreateLargeXmlFile(string testDataPath)
        {
            var largeXmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_types>";
            
            // 添加大量action元素
            for (int i = 0; i < 1000; i++)
            {
                largeXmlContent += $@"
    <action id=""action_{i}"" name=""Action {i}"" type=""swing"" usage_direction=""one_handed"" action_stage=""attack"" />";
            }
            
            largeXmlContent += @"
</action_types>";
            
            File.WriteAllText(Path.Combine(testDataPath, "large_action_types.xml"), largeXmlContent);
        }
        
        /// <summary>
        /// 创建无效XML文件
        /// </summary>
        private static void CreateInvalidXmlFile(string testDataPath)
        {
            var invalidXmlContent = @"This is not a valid XML file
It contains plain text instead of XML markup
No root element or proper structure";
            
            File.WriteAllText(Path.Combine(testDataPath, "invalid.xml"), invalidXmlContent);
        }
        
        /// <summary>
        /// 创建空文件
        /// </summary>
        private static void CreateEmptyFiles(string testDataPath)
        {
            File.WriteAllText(Path.Combine(testDataPath, "empty.xml"), string.Empty);
            File.WriteAllText(Path.Combine(testDataPath, "whitespace.xml"), "   \n\t\n   ");
        }
        
        /// <summary>
        /// 复制目录
        /// </summary>
        private static void CopyDirectory(string source, string destination)
        {
            Directory.CreateDirectory(destination);
            
            // 复制文件
            foreach (var file in Directory.GetFiles(source))
            {
                var destFile = Path.Combine(destination, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }
            
            // 复制子目录
            foreach (var dir in Directory.GetDirectories(source))
            {
                var destDir = Path.Combine(destination, Path.GetFileName(dir));
                CopyDirectory(dir, destDir);
            }
        }
        
        /// <summary>
        /// 验证测试数据是否已创建
        /// </summary>
        public static bool ValidateTestData(string testDataPath)
        {
            if (!Directory.Exists(testDataPath))
            {
                return false;
            }
            
            var requiredFiles = new[] { "action_types.xml", "combat_parameters.xml" };
            
            return requiredFiles.All(file => File.Exists(Path.Combine(testDataPath, file)));
        }
        
        /// <summary>
        /// 获取测试数据路径
        /// </summary>
        public static string GetTestDataPath()
        {
            return Path.Combine("BannerlordModEditor.TUI.UATTests", "TestData");
        }
    }
}