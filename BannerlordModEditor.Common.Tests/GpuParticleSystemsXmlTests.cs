using System.IO;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Data;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class GpuParticleSystemsXmlTests
    {
        private const string TestDataPath = "TestData/gpu_particle_systems.xml";

        [Fact]
        public void Can_Deserialize_GpuParticleSystems()
        {
            var serializer = new XmlSerializer(typeof(GpuParticleSystems));
            using var stream = File.OpenRead(TestDataPath);
            var model = serializer.Deserialize(stream) as GpuParticleSystems;
            Assert.NotNull(model);
            Assert.NotNull(model.ParticleSystems);
            Assert.NotNull(model.ParticleSystems.Items);
            Assert.NotEmpty(model.ParticleSystems.Items);
            Assert.Equal("test", model.ParticleSystems.Items[0].Name);
        }

        [Fact]
        public void Can_Serialize_GpuParticleSystems()
        {
            var serializer = new XmlSerializer(typeof(GpuParticleSystems));
            using var stream = File.OpenRead(TestDataPath);
            var model = serializer.Deserialize(stream) as GpuParticleSystems;
            using var ms = new MemoryStream();
            serializer.Serialize(ms, model);
            ms.Position = 0;
            var deserialized = serializer.Deserialize(ms) as GpuParticleSystems;
            Assert.NotNull(deserialized);
            Assert.Equal(model.ParticleSystems.Items[0].Name, deserialized.ParticleSystems.Items[0].Name);
        }
    }
}