using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Layouts;

/// <summary>
/// 基础布局配置的领域对象
/// 用于所有Layouts XML文件的通用结构
/// 包含完整的编辑器UI布局配置和业务逻辑
/// </summary>
[XmlRoot("base", Namespace = "")]
public class LayoutsBaseDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "string";

    [XmlElement("layouts")]
    public LayoutsContainerDO Layouts { get; set; } = new LayoutsContainerDO();

    // 运行时标记
    [XmlIgnore]
    public bool HasLayouts { get; set; } = false;

    // 业务逻辑：布局缓存和索引
    [XmlIgnore]
    public Dictionary<string, LayoutDO> LayoutsByClass { get; set; } = new Dictionary<string, LayoutDO>();

    [XmlIgnore]
    public Dictionary<string, LayoutDO> LayoutsByXmlTag { get; set; } = new Dictionary<string, LayoutDO>();

    // 业务逻辑：布局验证状态
    [XmlIgnore]
    public bool IsValid { get; set; } = true;

    [XmlIgnore]
    public List<string> ValidationErrors { get; set; } = new List<string>();

    // 业务逻辑方法
    public void InitializeIndexes()
    {
        LayoutsByClass.Clear();
        LayoutsByXmlTag.Clear();

        foreach (var layout in Layouts.LayoutList)
        {
            if (!string.IsNullOrEmpty(layout.Class))
            {
                LayoutsByClass[layout.Class] = layout;
            }

            if (!string.IsNullOrEmpty(layout.XmlTag))
            {
                LayoutsByXmlTag[layout.XmlTag] = layout;
            }
        }
    }

    public LayoutDO? GetLayoutByClass(string className)
    {
        return LayoutsByClass.GetValueOrDefault(className);
    }

    public LayoutDO? GetLayoutByXmlTag(string xmlTag)
    {
        return LayoutsByXmlTag.GetValueOrDefault(xmlTag);
    }

    public List<LayoutDO> GetTreeviewLayouts()
    {
        return Layouts.LayoutList.Where(l => l.UseInTreeview == "true").ToList();
    }

    public void Validate()
    {
        ValidationErrors.Clear();
        IsValid = true;

        foreach (var layout in Layouts.LayoutList)
        {
            if (string.IsNullOrEmpty(layout.Class))
            {
                ValidationErrors.Add($"Layout missing class attribute");
                IsValid = false;
            }

            if (string.IsNullOrEmpty(layout.XmlTag))
            {
                ValidationErrors.Add($"Layout '{layout.Class}' missing xml_tag attribute");
                IsValid = false;
            }

            // 验证列配置
            if (layout.HasColumns && layout.Columns.ColumnList.Count == 0)
            {
                ValidationErrors.Add($"Layout '{layout.Class}' has empty columns configuration");
                IsValid = false;
            }

            // 验证项配置
            if (layout.HasItems && layout.Items.ItemList.Count == 0)
            {
                ValidationErrors.Add($"Layout '{layout.Class}' has empty items configuration");
                IsValid = false;
            }
        }
    }

    // 生成UI配置
    public UIConfiguration GenerateUIConfiguration()
    {
        var config = new UIConfiguration
        {
            Type = Type,
            Layouts = new List<UILayout>()
        };

        foreach (var layout in Layouts.LayoutList)
        {
            var uiLayout = new UILayout
            {
                Class = layout.Class,
                XmlTag = layout.XmlTag,
                UseInTreeview = layout.UseInTreeview == "true",
                Columns = layout.Columns.ColumnList.Select(c => new UIColumn
                {
                    Id = c.Id,
                    Width = c.Width
                }).ToList(),
                Items = layout.Items.ItemList.Select(i => new UIItem
                {
                    Name = i.Name,
                    Label = i.Label,
                    Type = i.Type,
                    Column = i.Column,
                    XmlPath = i.XmlPath,
                    Optional = i.Optional == "true"
                }).ToList()
            };

            config.Layouts.Add(uiLayout);
        }

        return config;
    }

    public bool ShouldSerializeLayouts() => HasLayouts && Layouts != null && Layouts.LayoutList.Count > 0;
}

/// <summary>
/// 布局容器
/// </summary>
public class LayoutsContainerDO
{
    [XmlElement("layout")]
    public List<LayoutDO> LayoutList { get; set; } = new List<LayoutDO>();

    public bool ShouldSerializeLayoutList() => LayoutList != null && LayoutList.Count > 0;
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

    [XmlIgnore]
    public bool HasNameAttribute { get; set; } = false;

    [XmlIgnore]
    public bool HasEmptyNameAttribute { get; set; } = false;

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

    // 业务逻辑方法
    public bool IsValidVersion()
    {
        return System.Version.TryParse(Version, out _);
    }

    public bool HasValidColumnConfiguration()
    {
        return HasColumns && Columns.ColumnList.Count > 0;
    }

    public bool HasValidItemsConfiguration()
    {
        return HasItems && Items.ItemList.Count > 0;
    }

    public List<ItemDO> GetRequiredItems()
    {
        return Items.ItemList.Where(i => i.Optional != "true").ToList();
    }

    public List<ItemDO> GetOptionalItems()
    {
        return Items.ItemList.Where(i => i.Optional == "true").ToList();
    }

    public bool SupportsTreeview()
    {
        return UseInTreeview == "true";
    }

    public bool SupportsInsertion()
    {
        return HasInsertionDefinitions && InsertionDefinitions.InsertionDefinitionList.Count > 0;
    }

    public bool SupportsContextMenu()
    {
        return HasTreeviewContextMenu && TreeviewContextMenu.ItemList.Count > 0;
    }

    // 序列化控制方法
    public bool ShouldSerializeNameAttribute() => HasNameAttribute;
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

    // 业务逻辑方法
    public ColumnDO? GetColumnById(string id)
    {
        return ColumnList.FirstOrDefault(c => c.Id == id);
    }

    public bool HasValidWidths()
    {
        return ColumnList.All(c => !string.IsNullOrEmpty(c.Width));
    }

    public bool ShouldSerializeColumnList() => ColumnList != null && ColumnList.Count > 0;
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

    // 业务逻辑方法
    public bool IsPercentageWidth()
    {
        return Width.EndsWith("%");
    }

    public bool IsPixelWidth()
    {
        return Width.EndsWith("px") || !Width.EndsWith("%");
    }

    public double GetNumericWidth()
    {
        if (double.TryParse(Width.Replace("%", "").Replace("px", ""), out var width))
            return width;
        return 0;
    }

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeWidth() => !string.IsNullOrEmpty(Width);
}

/// <summary>
/// 插入定义容器
/// </summary>
public class InsertionDefinitionsDO
{
    [XmlElement("insertion_definition")]
    public List<InsertionDefinitionDO> InsertionDefinitionList { get; set; } = new List<InsertionDefinitionDO>();

    // 业务逻辑方法
    public InsertionDefinitionDO? GetDefinitionByLabel(string label)
    {
        return InsertionDefinitionList.FirstOrDefault(d => d.Label == label);
    }

    public List<InsertionDefinitionDO> GetDefinitionsByXmlPath(string xmlPath)
    {
        return InsertionDefinitionList.Where(d => d.XmlPath == xmlPath).ToList();
    }

    public bool ShouldSerializeInsertionDefinitionList() => InsertionDefinitionList != null && InsertionDefinitionList.Count > 0;
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

    // 业务逻辑方法
    public bool HasValidDefaultNode()
    {
        return HasDefaultNode && DefaultNode.HasAnyElements;
    }

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

    // 业务逻辑方法
    public string GetDefaultXml()
    {
        if (!HasAnyElements || AnyElements == null) return string.Empty;

        return string.Join(Environment.NewLine, AnyElements.Select(e => e.OuterXml));
    }

    public bool ShouldSerializeAnyElements() => HasAnyElements && AnyElements != null && AnyElements.Length > 0;
}

/// <summary>
/// 树形视图上下文菜单
/// </summary>
public class TreeviewContextMenuDO
{
    [XmlElement("item")]
    public List<ContextMenuItemDO> ItemList { get; set; } = new List<ContextMenuItemDO>();

    // 业务逻辑方法
    public ContextMenuItemDO? GetItemByName(string name)
    {
        return ItemList.FirstOrDefault(i => i.Name == name);
    }

    public List<ContextMenuItemDO> GetActionableItems()
    {
        return ItemList.Where(i => i.HasActionCode).ToList();
    }

    public bool ShouldSerializeItemList() => ItemList != null && ItemList.Count > 0;
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

    [XmlIgnore]
    public bool HasActionCode { get; set; } = false;

    [XmlElement("treeview_context_menu")]
    public TreeviewContextMenuDO TreeviewContextMenu { get; set; } = new TreeviewContextMenuDO();

    [XmlIgnore]
    public bool HasTreeviewContextMenu { get; set; } = false;

    // 业务逻辑方法
    public bool IsSeparator()
    {
        return Name == "-";
    }

    public bool HasSubmenu()
    {
        return HasTreeviewContextMenu && TreeviewContextMenu.ItemList.Count > 0;
    }

    public bool IsActionable()
    {
        return HasActionCode && !string.IsNullOrEmpty(ActionCode);
    }

    public bool ShouldSerializeActionCode() => HasActionCode && !string.IsNullOrEmpty(ActionCode);
    public bool ShouldSerializeTreeviewContextMenu() => HasTreeviewContextMenu && TreeviewContextMenu != null && TreeviewContextMenu.ItemList.Count > 0;
}

/// <summary>
/// 布局项容器
/// </summary>
public class ItemsDO
{
    [XmlElement("item")]
    public List<ItemDO> ItemList { get; set; } = new List<ItemDO>();

    // 业务逻辑方法
    public ItemDO? GetItemByName(string name)
    {
        return ItemList.FirstOrDefault(i => i.Name == name);
    }

    public List<ItemDO> GetItemsByType(string type)
    {
        return ItemList.Where(i => i.Type == type).ToList();
    }

    public List<ItemDO> GetItemsByColumn(string column)
    {
        return ItemList.Where(i => i.Column == column).ToList();
    }

    public bool ShouldSerializeItemList() => ItemList != null && ItemList.Count > 0;
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

    [XmlElement("default_node")]
    public DefaultNodeDO DefaultNode { get; set; } = new DefaultNodeDO();

    [XmlIgnore]
    public bool HasProperties { get; set; } = false;

    [XmlIgnore]
    public bool HasOptional { get; set; } = false;

    [XmlIgnore]
    public bool HasDefaultNode { get; set; } = false;

    // 业务逻辑方法
    public bool IsRequired()
    {
        return Optional != "true";
    }

    public bool IsOptional()
    {
        return Optional == "true";
    }

    public bool HasValidProperties()
    {
        return HasProperties && Properties.PropertyList.Count > 0;
    }

    public bool HasValidDefaultNode()
    {
        return HasDefaultNode && DefaultNode.HasAnyElements;
    }

    public PropertyDO? GetProperty(string name)
    {
        return Properties.PropertyList.FirstOrDefault(p => p.Name == name);
    }

    public string GetPropertyValue(string name, string defaultValue = "")
    {
        var property = GetProperty(name);
        return property?.Value ?? defaultValue;
    }

    // 类型常量
    public const string StringType = "string";
    public const string IntType = "int";
    public const string FloatType = "float";
    public const string BoolType = "bool";
    public const string EnumType = "enum";
    public const string ColorType = "color";

    public bool IsStringType() => Type == StringType;
    public bool IsNumericType() => Type == IntType || Type == FloatType;
    public bool IsBooleanType() => Type == BoolType;
    public bool IsEnumType() => Type == EnumType;
    public bool IsColorType() => Type == ColorType;

    public bool ShouldSerializeProperties() => HasProperties && Properties != null && Properties.PropertyList.Count > 0;
    public bool ShouldSerializeOptional() => HasOptional && !string.IsNullOrEmpty(Optional);
    public bool ShouldSerializeDefaultNode() => HasDefaultNode && DefaultNode != null && DefaultNode.HasAnyElements;
}

/// <summary>
/// 属性容器
/// </summary>
public class PropertiesDO
{
    [XmlElement("property")]
    public List<PropertyDO> PropertyList { get; set; } = new List<PropertyDO>();

    // 业务逻辑方法
    public PropertyDO? GetProperty(string name)
    {
        return PropertyList.FirstOrDefault(p => p.Name == name);
    }

    public List<PropertyDO> GetPropertiesByPrefix(string prefix)
    {
        return PropertyList.Where(p => p.Name.StartsWith(prefix)).ToList();
    }

    public bool ShouldSerializePropertyList() => PropertyList != null && PropertyList.Count > 0;
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

    [XmlIgnore]
    public bool HasValue { get; set; } = false;

    // 业务逻辑方法
    public bool IsBoolean()
    {
        return bool.TryParse(Value, out _);
    }

    public bool IsNumeric()
    {
        return double.TryParse(Value, out _);
    }

    public bool GetBooleanValue(bool defaultValue = false)
    {
        if (bool.TryParse(Value, out var result))
            return result;
        return defaultValue;
    }

    public double GetNumericValue(double defaultValue = 0)
    {
        if (double.TryParse(Value, out var result))
            return result;
        return defaultValue;
    }

    public bool ShouldSerializeValue() => HasValue && !string.IsNullOrEmpty(Value);
}

/// <summary>
/// UI配置辅助类
/// </summary>
public class UIConfiguration
{
    public string Type { get; set; } = string.Empty;
    public List<UILayout> Layouts { get; set; } = new List<UILayout>();
}

public class UILayout
{
    public string Class { get; set; } = string.Empty;
    public string XmlTag { get; set; } = string.Empty;
    public bool UseInTreeview { get; set; }
    public List<UIColumn> Columns { get; set; } = new List<UIColumn>();
    public List<UIItem> Items { get; set; } = new List<UIItem>();
}

public class UIColumn
{
    public string Id { get; set; } = string.Empty;
    public string Width { get; set; } = string.Empty;
}

public class UIItem
{
    public string Name { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Column { get; set; } = string.Empty;
    public string XmlPath { get; set; } = string.Empty;
    public bool Optional { get; set; }
}