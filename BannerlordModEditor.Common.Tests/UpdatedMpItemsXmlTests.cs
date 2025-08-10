using BannerlordModEditor.Common.Models;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class UpdatedMpItemsXmlTests : XmlModelTestBase<MpItems>
    {
        protected override string TestDataFileName => "mpitems.xml";
        protected override string ModelTypeName => "MpItems";

        // This class now inherits all round-trip tests from XmlModelTestBase
        // which provides comprehensive testing including skip handling, graceful missing file handling,
        // and enhanced comparison with boolean value tolerance
    }
}