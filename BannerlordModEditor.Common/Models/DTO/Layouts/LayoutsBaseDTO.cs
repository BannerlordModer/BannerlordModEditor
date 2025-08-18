using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Layouts
{
    /// <summary>
    /// 基础布局配置的数据传输对象
    /// 用于所有Layouts XML文件的通用结构
    /// </summary>
    [XmlRoot("base")]
    public class LayoutsBaseDTO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "string";

        [XmlElement("layouts")]
        public LayoutsContainerDTO Layouts { get; set; } = new LayoutsContainerDTO();
        
        [XmlIgnore]
        public bool HasLayouts { get; set; } = false;
        
        public bool ShouldSerializeLayouts() => HasLayouts && Layouts != null && Layouts.LayoutList.Count > 0;
    }

    /// <summary>
    /// 布局容器
    /// </summary>
    public class LayoutsContainerDTO
    {
        [XmlElement("layout")]
        public List<LayoutDTO> LayoutList { get; set; } = new List<LayoutDTO>();
    }

    /// <summary>
    /// 单个布局配置
    /// </summary>
    public class LayoutDTO
    {
        [XmlAttribute("class")]
        public string Class { get; set; } = string.Empty;

        [XmlAttribute("version")]
        public string Version { get; set; } = "0.1";

        [XmlAttribute("xml_tag")]
        public string XmlTag { get; set; } = string.Empty;

        [XmlAttribute("name_attribute")]
        public string NameAttribute { get; set; } = string.Empty;

        [XmlIgnore]
        public bool HasNameAttribute { get; set; } = false;
        
        [XmlIgnore]
        public bool HasEmptyNameAttribute { get; set; } = false;

        [XmlAttribute("use_in_treeview")]
        public string UseInTreeview { get; set; } = "true";

        [XmlElement("columns")]
        public ColumnsDTO Columns { get; set; } = new ColumnsDTO();

        [XmlIgnore]
        public bool HasColumns { get; set; } = false;

        [XmlElement("insertion_definitions")]
        public InsertionDefinitionsDTO InsertionDefinitions { get; set; } = new InsertionDefinitionsDTO();

        [XmlIgnore]
        public bool HasInsertionDefinitions { get; set; } = false;

        [XmlElement("treeview_context_menu")]
        public TreeviewContextMenuDTO TreeviewContextMenu { get; set; } = new TreeviewContextMenuDTO();

        [XmlIgnore]
        public bool HasTreeviewContextMenu { get; set; } = false;

        [XmlElement("items")]
        public ItemsDTO Items { get; set; } = new ItemsDTO();

        [XmlIgnore]
        public bool HasItems { get; set; } = false;

        public bool ShouldSerializeNameAttribute() => HasNameAttribute;
        public bool ShouldSerializeColumns() => HasColumns && Columns != null && Columns.ColumnList.Count > 0;
        public bool ShouldSerializeInsertionDefinitions() => HasInsertionDefinitions && InsertionDefinitions != null && InsertionDefinitions.InsertionDefinitionList.Count > 0;
        public bool ShouldSerializeTreeviewContextMenu() => HasTreeviewContextMenu && TreeviewContextMenu != null && TreeviewContextMenu.ItemList.Count > 0;
        public bool ShouldSerializeItems() => HasItems && Items != null && Items.ItemList.Count > 0;
    }

    /// <summary>
    /// 列配置
    /// </summary>
    public class ColumnsDTO
    {
        [XmlElement("column")]
        public List<ColumnDTO> ColumnList { get; set; } = new List<ColumnDTO>();
    }

    /// <summary>
    /// 单个列配置
    /// </summary>
    public class ColumnDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("width")]
        public string Width { get; set; } = string.Empty;
    }

    /// <summary>
    /// 插入定义容器
    /// </summary>
    public class InsertionDefinitionsDTO
    {
        [XmlElement("insertion_definition")]
        public List<InsertionDefinitionDTO> InsertionDefinitionList { get; set; } = new List<InsertionDefinitionDTO>();
    }

    /// <summary>
    /// 单个插入定义
    /// </summary>
    public class InsertionDefinitionDTO
    {
        [XmlAttribute("label")]
        public string Label { get; set; } = string.Empty;

        [XmlAttribute("xml_path")]
        public string XmlPath { get; set; } = string.Empty;

        [XmlElement("default_node")]
        public DefaultNodeDTO DefaultNode { get; set; } = new DefaultNodeDTO();

        [XmlIgnore]
        public bool HasDefaultNode { get; set; } = false;

        public bool ShouldSerializeDefaultNode() => HasDefaultNode && DefaultNode != null;
    }

    /// <summary>
    /// 默认节点配置
    /// </summary>
    public class DefaultNodeDTO
    {
        [XmlAnyElement]
        public System.Xml.XmlElement[]? AnyElements { get; set; }
        
        [XmlIgnore]
        public bool HasAnyElements { get; set; } = false;
        
        public bool ShouldSerializeAnyElements() => HasAnyElements && AnyElements != null && AnyElements.Length > 0;
    }

    /// <summary>
    /// 树形视图上下文菜单
    /// </summary>
    public class TreeviewContextMenuDTO
    {
        [XmlElement("item")]
        public List<ContextMenuItemDTO> ItemList { get; set; } = new List<ContextMenuItemDTO>();
    }

    /// <summary>
    /// 上下文菜单项
    /// </summary>
    public class ContextMenuItemDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("action_code")]
        public string ActionCode { get; set; } = string.Empty;

        [XmlIgnore]
        public bool HasActionCode { get; set; } = false;

        [XmlElement("treeview_context_menu")]
        public TreeviewContextMenuDTO TreeviewContextMenu { get; set; } = new TreeviewContextMenuDTO();

        [XmlIgnore]
        public bool HasTreeviewContextMenu { get; set; } = false;

        public bool ShouldSerializeActionCode() => HasActionCode && !string.IsNullOrEmpty(ActionCode);
        
        public bool ShouldSerializeTreeviewContextMenu() => HasTreeviewContextMenu && TreeviewContextMenu != null && TreeviewContextMenu.ItemList.Count > 0;
    }

    /// <summary>
    /// 布局项容器
    /// </summary>
    public class ItemsDTO
    {
        [XmlElement("item")]
        public List<ItemDTO> ItemList { get; set; } = new List<ItemDTO>();
    }

    /// <summary>
    /// 单个布局项
    /// </summary>
    public class ItemDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("label")]
        public string Label { get; set; } = string.Empty;

        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlAttribute("column")]
        public string Column { get; set; } = string.Empty;

        [XmlAttribute("xml_path")]
        public string XmlPath { get; set; } = string.Empty;

        [XmlAttribute("optional")]
        public string Optional { get; set; } = string.Empty;

        [XmlIgnore]
        public bool HasOptional { get; set; } = false;

        [XmlElement("properties")]
        public PropertiesDTO Properties { get; set; } = new PropertiesDTO();

        [XmlElement("default_node")]
        public DefaultNodeDTO DefaultNode { get; set; } = new DefaultNodeDTO();

        [XmlIgnore]
        public bool HasProperties { get; set; } = false;
        
        [XmlIgnore]
        public bool HasDefaultNode { get; set; } = false;

        public bool ShouldSerializeProperties() => HasProperties && Properties != null && Properties.PropertyList.Count > 0;
        public bool ShouldSerializeOptional() => HasOptional && !string.IsNullOrEmpty(Optional);
        public bool ShouldSerializeDefaultNode() => HasDefaultNode && DefaultNode != null && DefaultNode.HasAnyElements;
    }

    /// <summary>
    /// 属性容器
    /// </summary>
    public class PropertiesDTO
    {
        [XmlElement("property")]
        public List<PropertyDTO> PropertyList { get; set; } = new List<PropertyDTO>();
    }

    /// <summary>
    /// 单个属性
    /// </summary>
    public class PropertyDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }
}