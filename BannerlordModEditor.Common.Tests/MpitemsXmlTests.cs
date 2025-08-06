using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class MpitemsXmlTests
    {
        [Fact]
        public void Mpitems_RoundTrip_StructuralEquality()
        {
            var xmlPath = Path.Combine("BannerlordModEditor.Common.Tests", "TestData", "mpitems.xml");
            var xml = File.ReadAllText(xmlPath);

            var model = XmlTestUtils.Deserialize<Mpitems>(xml);
            var serialized = XmlTestUtils.Serialize(model);

            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}