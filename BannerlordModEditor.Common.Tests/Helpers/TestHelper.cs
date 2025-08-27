using System;
using System.IO;

namespace BannerlordModEditor.Tests.Helpers
{
    /// <summary>
    /// 测试助手类
    /// 提供测试过程中常用的辅助方法
    /// </summary>
    public class TestHelper
    {
        /// <summary>
        /// 创建临时目录
        /// </summary>
        public string CreateTempDirectory(string prefix = "Test_")
        {
            var tempPath = Path.Combine(Path.GetTempPath(), prefix + Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);
            return tempPath;
        }

        /// <summary>
        /// 清理临时目录
        /// </summary>
        public void CleanupTempDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"清理临时目录时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建测试XML文件
        /// </summary>
        public string CreateTestXmlFile(string directory, string fileName, string content)
        {
            var filePath = Path.Combine(directory, fileName);
            File.WriteAllText(filePath, content);
            return filePath;
        }

        /// <summary>
        /// 获取测试数据目录路径
        /// </summary>
        public string GetTestDataPath()
        {
            var currentDir = Directory.GetCurrentDirectory();
            return Path.Combine(currentDir, "TestData");
        }

        /// <summary>
        /// 创建标准的测试文件结构
        /// </summary>
        public void CreateStandardTestFiles(string directory)
        {
            // 创建基础的测试文件
            CreateTestXmlFile(directory, "managed_core_parameters.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<managed_core_parameters>
    <parameter name=""test_param"" value=""1.0""/>
</managed_core_parameters>");

            CreateTestXmlFile(directory, "crafting_pieces.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<CraftingPieces>
    <CraftingPiece id=""test_piece"" difficulty=""50"" damage=""10""/>
</CraftingPieces>");

            CreateTestXmlFile(directory, "items.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""test_item"" weight=""5"" value=""100""/>
    <Item id=""test_item2"" weight=""10"" value=""200""/>
</Items>");

            CreateTestXmlFile(directory, "characters.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<Characters>
    <Character id=""test_char"" level=""5"" occupation=""Soldier""/>
</Characters>");
        }

        /// <summary>
        /// 创建包含问题的测试文件
        /// </summary>
        public void CreateProblematicTestFiles(string directory)
        {
            // 创建重复ID的文件
            CreateTestXmlFile(directory, "items.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""duplicate_item"" weight=""10"" value=""100""/>
    <Item id=""duplicate_item"" weight=""15"" value=""200""/>
    <Item id=""invalid_weight"" weight=""-5"" value=""300""/>
    <Item id=""invalid_value"" weight=""10"" value=""-100""/>
</Items>");

            // 创建无效等级的文件
            CreateTestXmlFile(directory, "characters.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<Characters>
    <Character id=""test_char"" level=""0"" occupation=""Soldier""/>
    <Character id=""test_char2"" level=""100"" occupation=""InvalidOccupation""/>
</Characters>");

            // 创建无效制作难度的文件
            CreateTestXmlFile(directory, "crafting_pieces.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<CraftingPieces>
    <CraftingPiece id=""test_piece"" difficulty=""-10"" damage=""10""/>
    <CraftingPiece id=""test_piece2"" difficulty=""500"" damage=""15000""/>
</CraftingPieces>");
        }

        /// <summary>
        /// 创建循环依赖的测试文件
        /// </summary>
        public void CreateCircularDependencyFiles(string directory)
        {
            CreateTestXmlFile(directory, "file_a.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
    <item ref=""file_b.item1""/>
    <item ref=""file_c.item1""/>
</root>");

            CreateTestXmlFile(directory, "file_b.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
    <item ref=""file_c.item1""/>
</root>");

            CreateTestXmlFile(directory, "file_c.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
    <item ref=""file_a.item1""/>
</root>");
        }

        /// <summary>
        /// 创建引用完整性问题的测试文件
        /// </summary>
        public void CreateReferenceIntegrityFiles(string directory)
        {
            CreateTestXmlFile(directory, "items.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""test_item"" weight=""5"" value=""100""/>
</Items>");

            CreateTestXmlFile(directory, "test_data.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<TestData>
    <TestItem id=""test_item"" item=""nonexistent.item"" character=""missing.hero""/>
</TestData>");
        }

        /// <summary>
        /// 创建无效XML格式的测试文件
        /// </summary>
        public void CreateInvalidXmlFiles(string directory)
        {
            CreateTestXmlFile(directory, "invalid.xml", "This is not valid XML content");
            CreateTestXmlFile(directory, "malformed.xml", @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
    <item id=""test""
    <missing_closing_tag>
</root>");
        }

        /// <summary>
        /// 创建大型XML测试文件
        /// </summary>
        public void CreateLargeXmlFile(string directory, string fileName, int itemCount = 1000)
        {
            var writer = new StringWriter();
            writer.WriteLine(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>");

            for (int i = 0; i < itemCount; i++)
            {
                writer.WriteLine($"    <Item id=\"item_{i}\" weight=\"{i % 100}\" value=\"{i * 10}\"/>");
            }

            writer.WriteLine("</Items>");
            
            CreateTestXmlFile(directory, fileName, writer.ToString());
        }

        /// <summary>
        /// 创建复杂的嵌套XML测试文件
        /// </summary>
        public void CreateComplexNestedXmlFile(string directory, string fileName)
        {
            var content = @"<?xml version=""1.0"" encoding=""utf-8""?>
<ComplexData>
    <Section id=""section1"">
        <SubSection id=""subsection1"">
            <Item id=""item1"" weight=""10"" value=""100""/>
            <Item id=""item2"" weight=""15"" value=""200""/>
        </SubSection>
        <SubSection id=""subsection2"">
            <Item id=""item3"" weight=""20"" value=""300""/>
            <Reference ref=""section1.subsection1.item1""/>
        </SubSection>
    </Section>
    <Section id=""section2"">
        <Item id=""item4"" weight=""25"" value=""400""/>
        <Reference ref=""section1.subsection2.item3""/>
        <InvalidReference ref=""nonexistent.item""/>
    </Section>
</ComplexData>";

            CreateTestXmlFile(directory, fileName, content);
        }

        /// <summary>
        /// 创建Schema验证测试文件
        /// </summary>
        public void CreateSchemaValidationFiles(string directory)
        {
            // 创建XSD文件
            var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
    <xs:element name=""Items"">
        <xs:complexType>
            <xs:sequence>
                <xs:element name=""Item"" maxOccurs=""unbounded"">
                    <xs:complexType>
                        <xs:attribute name=""id"" type=""xs:string"" use=""required""/>
                        <xs:attribute name=""weight"" type=""xs:float"" use=""required""/>
                        <xs:attribute name=""value"" type=""xs:int"" use=""required""/>
                    </xs:complexType>
                </xs:element>
            </xs:sequence>
        </xs:complexType>
    </xs:element>
</xs:schema>";

            CreateTestXmlFile(directory, "items.xsd", xsdContent);

            // 创建符合Schema的XML文件
            var validXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
      xsi:noNamespaceSchemaLocation=""items.xsd"">
    <Item id=""test_item"" weight=""10"" value=""100""/>
    <Item id=""test_item2"" weight=""15"" value=""200""/>
</Items>";

            CreateTestXmlFile(directory, "valid_items.xml", validXml);

            // 创建不符合Schema的XML文件
            var invalidXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
      xsi:noNamespaceSchemaLocation=""items.xsd"">
    <Item id=""test_item"" weight=""10""/>
    <Item weight=""15"" value=""200""/>
    <InvalidElement/>
</Items>";

            CreateTestXmlFile(directory, "invalid_items.xml", invalidXml);
        }
    }
}