using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class SimpleLooknfeelStructureTest
    {
        [Fact]
        public void Simple_Looknfeel_Element_Count()
        {
            var xmlPath = "TestData/looknfeel.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化 - XmlTestUtils会自动处理LooknfeelDO的空元素标记
            var obj = XmlTestUtils.Deserialize<LooknfeelDO>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 简单比较：检查节点数量和属性数量
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);
            
            int nodeCount1 = doc1.Descendants().Count();
            int nodeCount2 = doc2.Descendants().Count();
            
            int attrCount1 = doc1.Descendants().Sum(e => e.Attributes().Count());
            int attrCount2 = doc2.Descendants().Sum(e => e.Attributes().Count());
            
            Console.WriteLine($"Original nodes: {nodeCount1}, Serialized nodes: {nodeCount2}");
            Console.WriteLine($"Original attrs: {attrCount1}, Serialized attrs: {attrCount2}");
            
            // 只检查节点和属性数量，不检查具体值
            Assert.Equal(nodeCount1, nodeCount2);
            Assert.Equal(attrCount1, attrCount2);
        }
    }
}