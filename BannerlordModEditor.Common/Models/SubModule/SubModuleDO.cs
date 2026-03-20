using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.SubModule
{
    [XmlRoot("Module")]
    public class SubModuleDO
    {
        [XmlElement("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("Id")]
        public string Id { get; set; } = string.Empty;

        [XmlElement("Version")]
        public string Version { get; set; } = string.Empty;

        [XmlElement("SingleplayerModule")]
        public bool SingleplayerModule { get; set; } = true;

        [XmlElement("MultiplayerModule")]
        public bool MultiplayerModule { get; set; }

        [XmlArray("DependedModules")]
        [XmlArrayItem("Module")]
        public List<DependedModuleDO> DependedModules { get; set; } = new();

        [XmlArray("SubModules")]
        [XmlArrayItem("SubModule")]
        public List<SubModuleItemDO> SubModules { get; set; } = new();

        [XmlArray("Xmls")]
        [XmlArrayItem("XmlNode")]
        public List<XmlNodeDO> Xmls { get; set; } = new();

        [XmlArray("OptionalDependedModules")]
        [XmlArrayItem("Module")]
        public List<OptionalDependedModuleDO> OptionalDependedModules { get; set; } = new();
    }

    public class DependedModuleDO
    {
        [XmlAttribute("Id")]
        public string Id { get; set; } = string.Empty;
    }

    public class OptionalDependedModuleDO
    {
        [XmlAttribute("Id")]
        public string Id { get; set; } = string.Empty;
    }

    public class SubModuleItemDO
    {
        [XmlElement("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("DLLName")]
        public string DLLName { get; set; } = string.Empty;

        [XmlElement("SubModuleClassType")]
        public string SubModuleClassType { get; set; } = string.Empty;

        [XmlElement("IsOptional")]
        public bool IsOptional { get; set; }

        [XmlElement("IsTicked")]
        public bool IsTicked { get; set; } = true;

        [XmlArray("Tags")]
        [XmlArrayItem("Tag")]
        public List<SubModuleTagDO> Tags { get; set; } = new();
    }

    public class SubModuleTagDO
    {
        [XmlAttribute("Key")]
        public string Key { get; set; } = string.Empty;

        [XmlAttribute("Value")]
        public string Value { get; set; } = string.Empty;
    }

    public class XmlNodeDO
    {
        [XmlAttribute("Id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("Type")]
        public string Type { get; set; } = string.Empty;

        [XmlText]
        public string Path { get; set; } = string.Empty;
    }
}
