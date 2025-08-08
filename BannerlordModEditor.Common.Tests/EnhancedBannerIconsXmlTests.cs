using BannerlordModEditor.Common.Models.Data;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class EnhancedBannerIconsXmlTests : XmlModelTestBase<BannerIconsModel>
    {
        protected override string TestDataFileName => "banner_icons.xml";
        protected override string ModelTypeName => "BannerIcons";

        // Here I can add any specific tests for banner icons if needed
        // The base class automatically handles round-trip testing
    }
}