using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models {
    [XmlRoot(ElementName = "Name")]
    public class Name {

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "Id")]
    public class Id {

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "Version")]
    public class Version {

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "DefaultModule")]
    public class DefaultModule {

        [XmlAttribute(AttributeName = "value")]
        public bool Value { get; set; }
    }

    [XmlRoot(ElementName = "ModuleCategory")]
    public class ModuleCategory {

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "ModuleType")]
    public class ModuleType {

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "DependedModule")]
    public class DependedModule {

        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "DependentVersion")]
        public string DependentVersion { get; set; }

        [XmlAttribute(AttributeName = "Optional")]
        public bool Optional { get; set; }
    }

    [XmlRoot(ElementName = "DependedModules")]
    public class DependedModules {

        [XmlElement(ElementName = "DependedModule")]
        public List<DependedModule> DependedModule { get; set; }
    }

    [XmlRoot(ElementName = "DLLName")]
    public class DLLName {

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "SubModuleClassType")]
    public class SubModuleClassType {

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "Tag")]
    public class Tag {

        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "Tags")]
    public class Tags {

        [XmlElement(ElementName = "Tag")]
        public List<Tag> Tag { get; set; }
    }

    [XmlRoot(ElementName = "SubModule")]
    public class SubModule {

        [XmlElement(ElementName = "Name")]
        public Name Name { get; set; }

        [XmlElement(ElementName = "DLLName")]
        public DLLName DLLName { get; set; }

        [XmlElement(ElementName = "SubModuleClassType")]
        public SubModuleClassType SubModuleClassType { get; set; }

        [XmlElement(ElementName = "Tags")]
        public Tags Tags { get; set; }

        [XmlElement(ElementName = "Assemblies")]
        public Assemblies Assemblies { get; set; }
    }

    [XmlRoot(ElementName = "Assembly")]
    public class Assembly {

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "Assemblies")]
    public class Assemblies {

        [XmlElement(ElementName = "Assembly")]
        public Assembly Assembly { get; set; }
    }

    [XmlRoot(ElementName = "SubModules")]
    public class SubModules {

        [XmlElement(ElementName = "SubModule")]
        public List<SubModule> SubModule { get; set; }
    }

    [XmlRoot(ElementName = "XmlName")]
    public class XmlName {

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "path")]
        public string Path { get; set; }
    }

    [XmlRoot(ElementName = "GameType")]
    public class GameType {

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "IncludedGameTypes")]
    public class IncludedGameTypes {

        [XmlElement(ElementName = "GameType")]
        public GameType GameType { get; set; }
    }

    [XmlRoot(ElementName = "XmlNode")]
    public class XmlNode {

        [XmlElement(ElementName = "XmlName")]
        public XmlName XmlName { get; set; }

        [XmlElement(ElementName = "IncludedGameTypes")]
        public IncludedGameTypes IncludedGameTypes { get; set; }
    }

    [XmlRoot(ElementName = "Xmls")]
    public class Xmls {

        [XmlElement(ElementName = "XmlNode")]
        public List<XmlNode> XmlNode { get; set; }
    }

    [XmlRoot(ElementName = "Module")]
    public class Module {

        [XmlElement(ElementName = "Name")]
        public Name Name { get; set; }

        [XmlElement(ElementName = "Id")]
        public Id Id { get; set; }

        [XmlElement(ElementName = "Version")]
        public Version Version { get; set; }

        [XmlElement(ElementName = "DefaultModule")]
        public DefaultModule DefaultModule { get; set; }

        [XmlElement(ElementName = "ModuleCategory")]
        public ModuleCategory ModuleCategory { get; set; }

        [XmlElement(ElementName = "ModuleType")]
        public ModuleType ModuleType { get; set; }

        [XmlElement(ElementName = "DependedModules")]
        public DependedModules DependedModules { get; set; }

        [XmlElement(ElementName = "SubModules")]
        public SubModules SubModules { get; set; }

        [XmlElement(ElementName = "Xmls")]
        public Xmls Xmls { get; set; }
    }

}
