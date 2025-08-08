using System;
using System.IO;
using System.Xml.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace BannerlordModEditor.Common.Tests
{
    public static class TestHelpers
    {
        public static bool TestFileExistsOrSkip(string testDataPath)
        {
            if (!File.Exists(testDataPath))
            {
                return false; // Will be handled by caller
            }
            return true;
        }

        public static T LoadXmlOrSkip<T>(string testDataPath) where T : class
        {
            if (!TestFileExistsOrSkip(testDataPath))
            {
                return null!; // Handle in test - skip or fail appropriately
            }

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var reader = new StreamReader(testDataPath);
                return (T)serializer.Deserialize(reader)!;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to deserialize {testDataPath}: {ex.Message}", ex);
            }
        }

        public static string GetTestDataPath(string filename)
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            return Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", filename);
        }

        public static string SerializeForValidation<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var writer = new StringWriter();
            serializer.Serialize(writer, obj);
            return writer.ToString();
        }

        public static T RoundTripDeserialize<T>(T obj) where T : class
        {
            var xml = SerializeForValidation(obj);
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xml);
            return (T)serializer.Deserialize(reader)!;
        }

        public static void AssertRoundTrip<T>(string testDataPath) where T : class
        {
            if (!File.Exists(testDataPath))
            {
                throw new SkipException($"Test data file not found: {testDataPath}");
            }

            var original = File.ReadAllText(testDataPath);
            var deserialized = LoadXmlOrSkip<T>(testDataPath);
            
            Assert.NotNull(deserialized);
            
            var serialized = SerializeForValidation(deserialized);
            var options = new XmlTestUtils.XmlComparisonOptions 
            { 
                AllowCaseInsensitiveBooleans = true,
                AllowNumericTolerance = true
            };
            
            XmlTestUtils.AssertXmlRoundTrip(original, serialized, options);
        }
    }

    // Custom Skip Exception
    public class SkipException : XunitException
    {
        public SkipException(string message) : base(message) { }
    }
}