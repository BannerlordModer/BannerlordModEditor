/// <summary>
/// XmlTestUtils.CompareXmlStructure 方法单元测试，覆盖属性null与不存在、节点差异、嵌套递归、差异报告断言等场景。
/// </summary>
using System;
using Xunit;
using BannerlordModEditor.Common.Tests;

namespace BannerlordModEditor.Common.Tests.__tests__
{
    public class XmlTestUtils_CompareXmlStructure_Tests
    {
        /// <summary>
        /// 属性为null与属性不存在的区分
        /// </summary>
        [Fact]
        public void AttributeNullVsMissing_ShouldReportCorrectly()
        {
            string xmlA = "<root attr=\"\"></root>";
            string xmlB = "<root></root>";
            var report = XmlTestUtils.CompareXmlStructure(xmlA, xmlB);
            Assert.Contains("/@attr (B缺失)", report.ExtraAttributes);
            Assert.True(!report.IsStructurallyEqual);
        }

        /// <summary>
        /// 节点缺失、多余、节点名/属性名/属性值/文本内容差异
        /// </summary>
        [Fact]
        public void NodeAndAttributeDifferences_ShouldBeReported()
        {
            string xmlA = "<root><a attr=\"value1\">text</a></root>";
            string xmlB = "<root><b attr=\"value2\">diff</b><extra/></root>";
            var report = XmlTestUtils.CompareXmlStructure(xmlA, xmlB);
            Assert.Contains("/a[0]: A=a, B=b", report.NodeNameDifferences);
            Assert.Contains("/a[0]@attr: A=value1, B=value2", report.AttributeValueDifferences);
            Assert.Contains("/a[0]: A文本='text', B文本='diff'", report.TextDifferences);
            Assert.Contains("/extra[1] (A缺失)", report.MissingNodes);
            Assert.True(!report.IsStructurallyEqual);
        }

        /// <summary>
        /// 复杂嵌套结构的递归比较
        /// </summary>
        [Fact]
        public void DeepNestedStructure_ShouldCompareRecursively()
        {
            string xmlA = "<root><level1><level2 attr=\"x\"><leaf>abc</leaf></level2></level1></root>";
            string xmlB = "<root><level1><level2 attr=\"y\"><leaf>def</leaf></level2></level1></root>";
            var report = XmlTestUtils.CompareXmlStructure(xmlA, xmlB);
            Assert.Contains("/level1[0]/level2[0]@attr: A=x, B=y", report.AttributeValueDifferences);
            Assert.Contains("/level1[0]/level2[0]/leaf[0]: A文本='abc', B文本='def'", report.TextDifferences);
            Assert.True(!report.IsStructurallyEqual);
        }

        /// <summary>
        /// 差异报告对象的正确性和断言兼容性
        /// </summary>
        [Fact]
        public void DiffReportObject_ShouldBeCompatibleWithAssertions()
        {
            string xmlA = "<root><a attr=\"1\">text</a></root>";
            string xmlB = "<root><a attr=\"1\">text</a></root>";
            var report = XmlTestUtils.CompareXmlStructure(xmlA, xmlB);
            Assert.True(report.IsStructurallyEqual);
            Assert.Empty(report.MissingNodes);
            Assert.Empty(report.ExtraNodes);
            Assert.Empty(report.NodeNameDifferences);
            Assert.Empty(report.MissingAttributes);
            Assert.Empty(report.ExtraAttributes);
            Assert.Empty(report.AttributeValueDifferences);
            Assert.Empty(report.TextDifferences);
        }
    }
}