using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Layouts
{
    /// <summary>
    /// 基础布局配置的领域对象
    /// 用于所有Layouts XML文件的通用结构
    /// </summary>
    [XmlRoot("base")]
    public class LayoutsBaseDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "string";

        [XmlElement("layouts")]
        public LayoutsContainerDO Layouts { get; set; } = new LayoutsContainerDO();

        [XmlIgnore]
        public bool HasLayouts { get; set; } = false;

        public bool ShouldSerializeLayouts() => HasLayouts && Layouts != null && Layouts.LayoutList.Count > 0;
    }

    /// <summary>
    /// 布局容器
    /// </summary>
    public class LayoutsContainerDO
    {
        [XmlElement("layout")]
        public List<LayoutDO> LayoutList { get; set; } = new List<LayoutDO>();
    }

    /// <summary>
    /// 单个布局配置
    /// </summary>
    public class LayoutDO
    {
        [XmlAttribute("class")]
        public string Class { get; set; } = string.Empty;

        [XmlAttribute("version")]
        public string Version { get; set; } = "0.1";

        [XmlAttribute("xml_tag")]
        public string XmlTag { get; set; } = string.Empty;

        [XmlAttribute("name_attribute")]
        public string NameAttribute { get; set; } = string.Empty;

        [XmlAttribute("use_in_treeview")]
        public string UseInTreeview { get; set; } = "true";

        [XmlElement("columns")]
        public ColumnsDO Columns { get; set; } = new ColumnsDO();

        [XmlIgnore]
        public bool HasColumns { get; set; } = false;

        [XmlElement("insertion_definitions")]
        public InsertionDefinitionsDO InsertionDefinitions { get; set; } = new InsertionDefinitionsDO();

        [XmlIgnore]
        public bool HasInsertionDefinitions { get; set; } = false;

        [XmlElement("treeview_context_menu")]
        public TreeviewContextMenuDO TreeviewContextMenu { get; set; } = new TreeviewContextMenuDO();

        [XmlIgnore]
        public bool HasTreeviewContextMenu { get; set; } = false;

        [XmlElement("items")]
        public ItemsDO Items { get; set; } = new ItemsDO();

        [XmlIgnore]
        public bool HasItems { get; set; } = false;

        public bool ShouldSerializeColumns() => HasColumns && Columns != null && Columns.ColumnList.Count > 0;
        public bool ShouldSerializeInsertionDefinitions() => HasInsertionDefinitions && InsertionDefinitions != null && InsertionDefinitions.InsertionDefinitionList.Count > 0;
        public bool ShouldSerializeTreeviewContextMenu() => HasTreeviewContextMenu && TreeviewContextMenu != null && TreeviewContextMenu.ItemList.Count > 0;
        public bool ShouldSerializeItems() => HasItems && Items != null && Items.ItemList.Count > 0;
    }

    /// <summary>
    /// 列配置
    /// </summary>
    public class ColumnsDO
    {
        [XmlElement("column")]
        public List<ColumnDO> ColumnList { get; set; } = new List<ColumnDO>();
    }

    /// <summary>
    /// 单个列配置
    /// </summary>
    public class ColumnDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("width")]
        public string Width { get; set; } = string.Empty;
    }

    /// <summary>
    /// 插入定义容器
    /// </summary>
    public class InsertionDefinitionsDO
    {
        [XmlElement("insertion_definition")]
        public List<InsertionDefinitionDO> InsertionDefinitionList { get; set; } = new List<InsertionDefinitionDO>();
    }

    /// <summary>
    /// 单个插入定义
    /// </summary>
    public class InsertionDefinitionDO
    {
        [XmlAttribute("label")]
        public string Label { get; set; } = string.Empty;

        [XmlAttribute("xml_path")]
        public string XmlPath { get; set; } = string.Empty;

        [XmlElement("default_node")]
        public DefaultNodeDO DefaultNode { get; set; } = new DefaultNodeDO();

        [XmlIgnore]
        public bool HasDefaultNode { get; set; } = false;

        public bool ShouldSerializeDefaultNode() => HasDefaultNode && DefaultNode != null;
    }

    /// <summary>
    /// 默认节点配置
    /// </summary>
    public class DefaultNodeDO
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
    public class TreeviewContextMenuDO
    {
        [XmlElement("item")]
        public List<ContextMenuItemDO> ItemList { get; set; } = new List<ContextMenuItemDO>();
    }

    /// <summary>
    /// 上下文菜单项
    /// </summary>
    public class ContextMenuItemDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("action_code")]
        public string ActionCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// 布局项容器
    /// </summary>
    public class ItemsDO
    {
        [XmlElement("item")]
        public List<ItemDO> ItemList { get; set; } = new List<ItemDO>();
    }

    /// <summary>
    /// 单个布局项
    /// </summary>
    public class ItemDO
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

        [XmlElement("properties")]
        public PropertiesDO Properties { get; set; } = new PropertiesDO();

        [XmlIgnore]
        public bool HasProperties { get; set; } = false;

        [XmlIgnore]
        public bool HasOptional { get; set; } = false;

        public bool ShouldSerializeProperties() => HasProperties && Properties != null && Properties.PropertyList.Count > 0;
        public bool ShouldSerializeOptional() => HasOptional && !string.IsNullOrEmpty(Optional);
    }

    /// <summary>
    /// 属性容器
    /// </summary>
    public class PropertiesDO
    {
        [XmlElement("property")]
        public List<PropertyDO> PropertyList { get; set; } = new List<PropertyDO>();
    }

    /// <summary>
    /// 单个属性
    /// </summary>
    public class PropertyDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }
}