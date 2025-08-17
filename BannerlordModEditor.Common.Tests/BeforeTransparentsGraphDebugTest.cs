using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class BeforeTransparentsGraphDebugTest
    {
        [Fact]
        public void BeforeTransparentsGraph_Debug_CompareXml()
        {
            var xmlPath = "TestData/before_transparents_graph.xml";
            var xml = File.ReadAllText(xmlPath);

            var obj = XmlTestUtils.Deserialize<BeforeTransparentsGraph>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);

            Console.WriteLine("=== Original XML ===");
            Console.WriteLine(xml);
            Console.WriteLine();
            Console.WriteLine("=== Serialized XML ===");
            Console.WriteLine(xml2);
            Console.WriteLine();
            Console.WriteLine("=== Are they equal? ===");
            Console.WriteLine(XmlTestUtils.AreStructurallyEqual(xml, xml2));

            // Save both files for comparison
            File.WriteAllText("debug_original.xml", xml);
            File.WriteAllText("debug_serialized.xml", xml2);
        }
    }
}