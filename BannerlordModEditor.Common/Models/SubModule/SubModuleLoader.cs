using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.SubModule
{
    public interface ISubModuleLoader
    {
        Task<SubModuleDO?> LoadAsync(string filePath);
        SubModuleDO? Load(string filePath);
        Task SaveAsync(SubModuleDO data, string filePath);
        void Save(SubModuleDO data, string filePath);
        SubModuleDO CreateDefault(string modName, string modId);
    }

    public class SubModuleLoader : ISubModuleLoader
    {
        private readonly XmlSerializer _serializer;

        public SubModuleLoader()
        {
            _serializer = new XmlSerializer(typeof(SubModuleDTO));
        }

        public async Task<SubModuleDO?> LoadAsync(string filePath)
        {
            return await Task.Run(() => Load(filePath));
        }

        public SubModuleDO? Load(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            try
            {
                using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var dto = (SubModuleDTO?)_serializer.Deserialize(stream);
                return dto != null ? SubModuleMapper.ToDO(dto) : null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load SubModule.xml from {filePath}: {ex.Message}", ex);
            }
        }

        public async Task SaveAsync(SubModuleDO data, string filePath)
        {
            await Task.Run(() => Save(data, filePath));
        }

        public void Save(SubModuleDO data, string filePath)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                var dto = SubModuleMapper.ToDTO(data);
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\n",
                    OmitXmlDeclaration = false,
                    Encoding = new System.Text.UTF8Encoding(false)
                };

                using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                using var writer = XmlWriter.Create(stream, settings);
                
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                
                _serializer.Serialize(writer, dto, namespaces);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save SubModule.xml to {filePath}: {ex.Message}", ex);
            }
        }

        public SubModuleDO CreateDefault(string modName, string modId)
        {
            return new SubModuleDO
            {
                Name = modName,
                Id = modId,
                Version = "v1.0.0",
                SingleplayerModule = true,
                MultiplayerModule = false,
                DependedModules = new(),
                SubModules = new(),
                Xmls = new(),
                OptionalDependedModules = new()
            };
        }
    }
}
