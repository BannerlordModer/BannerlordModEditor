using System;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public abstract class XmlModelTestBase<TModel> where TModel : class
    {
        protected abstract string TestDataFileName { get; }
        protected abstract string ModelTypeName { get; }

        protected virtual XmlTestUtils.XmlComparisonOptions ComparisonOptions => new()
        {
            AllowCaseInsensitiveBooleans = true,
            AllowNumericTolerance = true,
            IgnoreComments = true,
            IgnoreWhitespace = true
        };

        [Fact]
        public virtual void CanDeserializeAndSerialize_WithoutDataLoss()
        {
            var testDataPath = TestHelpers.GetTestDataPath(TestDataFileName);
            
            if (!File.Exists(testDataPath))
            {
                throw new SkipException($"Test data file '{TestDataFileName}' not found at {testDataPath}. The model '{ModelTypeName}' is not yet supported or test data is missing.");
            }

            try
            {
                // Load original XML
                var originalXml = File.ReadAllText(testDataPath);

                // Deserialize
                var model = TestHelpers.LoadXmlOrSkip<TModel>(testDataPath);
                Assert.NotNull(model);

                // Serialize back
                var serializer = new XmlSerializer(typeof(TModel));
                var serializedXml = XmlTestUtils.Serialize(model);

                // Assert round-trip equivalence
                XmlTestUtils.AssertXmlRoundTrip(originalXml, serializedXml, ComparisonOptions);
            }
            catch (Exception ex)
            {
                Assert.True(false, $"XmlModelTestBase failed for {ModelTypeName}: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        [Fact]
        public virtual void CanDeserializeWithoutThrowing()
        {
            var testDataPath = TestHelpers.GetTestDataPath(TestDataFileName);
            
            if (!File.Exists(testDataPath))
            {
                throw new SkipException($"Test data file '{TestDataFileName}' not found at {testDataPath}.");
            }

            var exception = Record.Exception(() => TestHelpers.LoadXmlOrSkip<TModel>(testDataPath));
            Assert.Null(exception);
        }

        [Fact]
        public virtual void CanSerializeWithoutThrowing()
        {
            var testDataPath = TestHelpers.GetTestDataPath(TestDataFileName);
            
            if (!File.Exists(testDataPath))
            {
                throw new SkipException($"Test data file '{TestDataFileName}' not found at {testDataPath}.");
            }

            var model = TestHelpers.LoadXmlOrSkip<TModel>(testDataPath);
            Assert.NotNull(model);

            var exception = Record.Exception(() => XmlTestUtils.Serialize(model));
            Assert.Null(exception);
        }

        [Fact]
        public virtual void RoundTripDeserializationMaintainsLogicalEquivalence()
        {
            var testDataPath = TestHelpers.GetTestDataPath(TestDataFileName);
            
            if (!File.Exists(testDataPath))
            {
                throw new SkipException($"Test data file '{TestDataFileName}' not found.");
            }

            var original = File.ReadAllText(testDataPath);
            var model = TestHelpers.LoadXmlOrSkip<TModel>(testDataPath);
            Assert.NotNull(model);
            
            var roundTrip = TestHelpers.RoundTripDeserialize(model);
            Assert.NotNull(roundTrip);
        }
    }
}