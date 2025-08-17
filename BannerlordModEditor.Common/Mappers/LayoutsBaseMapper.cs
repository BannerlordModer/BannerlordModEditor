using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Models.DTO.Layouts;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// Layouts基础配置的映射器
    /// 用于DO和DTO之间的双向转换
    /// </summary>
    public static class LayoutsBaseMapper
    {
        /// <summary>
        /// 将DO转换为DTO
        /// </summary>
        public static LayoutsBaseDTO ToDTO(LayoutsBaseDO source)
        {
            if (source == null) return null;

            return new LayoutsBaseDTO
            {
                Type = source.Type,
                Layouts = LayoutsContainerMapper.ToDTO(source.Layouts)
            };
        }

        /// <summary>
        /// 将DTO转换为DO
        /// </summary>
        public static LayoutsBaseDO ToDO(LayoutsBaseDTO source)
        {
            if (source == null) return null;

            return new LayoutsBaseDO
            {
                Type = source.Type,
                Layouts = LayoutsContainerMapper.ToDO(source.Layouts)
            };
        }
    }

    /// <summary>
    /// 布局容器映射器
    /// </summary>
    internal static class LayoutsContainerMapper
    {
        public static LayoutsContainerDTO ToDTO(LayoutsContainerDO source)
        {
            if (source == null) return null;

            return new LayoutsContainerDTO
            {
                LayoutList = source.LayoutList?
                    .Select(LayoutMapper.ToDTO)
                    .ToList() ?? new List<LayoutDTO>()
            };
        }

        public static LayoutsContainerDO ToDO(LayoutsContainerDTO source)
        {
            if (source == null) return null;

            return new LayoutsContainerDO
            {
                LayoutList = source.LayoutList?
                    .Select(LayoutMapper.ToDO)
                    .ToList() ?? new List<LayoutDO>()
            };
        }
    }

    /// <summary>
    /// 布局映射器
    /// </summary>
    internal static class LayoutMapper
    {
        public static LayoutDTO ToDTO(LayoutDO source)
        {
            if (source == null) return null;

            return new LayoutDTO
            {
                Class = source.Class,
                Version = source.Version,
                XmlTag = source.XmlTag,
                NameAttribute = source.NameAttribute,
                UseInTreeview = source.UseInTreeview,
                Columns = ColumnsMapper.ToDTO(source.Columns),
                InsertionDefinitions = InsertionDefinitionsMapper.ToDTO(source.InsertionDefinitions),
                TreeviewContextMenu = TreeviewContextMenuMapper.ToDTO(source.TreeviewContextMenu),
                Items = ItemsMapper.ToDTO(source.Items)
            };
        }

        public static LayoutDO ToDO(LayoutDTO source)
        {
            if (source == null) return null;

            return new LayoutDO
            {
                Class = source.Class,
                Version = source.Version,
                XmlTag = source.XmlTag,
                NameAttribute = source.NameAttribute,
                UseInTreeview = source.UseInTreeview,
                Columns = ColumnsMapper.ToDO(source.Columns),
                InsertionDefinitions = InsertionDefinitionsMapper.ToDO(source.InsertionDefinitions),
                TreeviewContextMenu = TreeviewContextMenuMapper.ToDO(source.TreeviewContextMenu),
                Items = ItemsMapper.ToDO(source.Items)
            };
        }
    }

    /// <summary>
    /// 列配置映射器
    /// </summary>
    internal static class ColumnsMapper
    {
        public static ColumnsDTO ToDTO(ColumnsDO source)
        {
            if (source == null) return null;

            return new ColumnsDTO
            {
                ColumnList = source.ColumnList?
                    .Select(ColumnMapper.ToDTO)
                    .ToList() ?? new List<ColumnDTO>()
            };
        }

        public static ColumnsDO ToDO(ColumnsDTO source)
        {
            if (source == null) return null;

            return new ColumnsDO
            {
                ColumnList = source.ColumnList?
                    .Select(ColumnMapper.ToDO)
                    .ToList() ?? new List<ColumnDO>()
            };
        }
    }

    /// <summary>
    /// 单个列映射器
    /// </summary>
    internal static class ColumnMapper
    {
        public static ColumnDTO ToDTO(ColumnDO source)
        {
            if (source == null) return null;

            return new ColumnDTO
            {
                Id = source.Id,
                Width = source.Width
            };
        }

        public static ColumnDO ToDO(ColumnDTO source)
        {
            if (source == null) return null;

            return new ColumnDO
            {
                Id = source.Id,
                Width = source.Width
            };
        }
    }

    /// <summary>
    /// 插入定义映射器
    /// </summary>
    internal static class InsertionDefinitionsMapper
    {
        public static InsertionDefinitionsDTO ToDTO(InsertionDefinitionsDO source)
        {
            if (source == null) return null;

            return new InsertionDefinitionsDTO
            {
                InsertionDefinitionList = source.InsertionDefinitionList?
                    .Select(InsertionDefinitionMapper.ToDTO)
                    .ToList() ?? new List<InsertionDefinitionDTO>()
            };
        }

        public static InsertionDefinitionsDO ToDO(InsertionDefinitionsDTO source)
        {
            if (source == null) return null;

            return new InsertionDefinitionsDO
            {
                InsertionDefinitionList = source.InsertionDefinitionList?
                    .Select(InsertionDefinitionMapper.ToDO)
                    .ToList() ?? new List<InsertionDefinitionDO>()
            };
        }
    }

    /// <summary>
    /// 单个插入定义映射器
    /// </summary>
    internal static class InsertionDefinitionMapper
    {
        public static InsertionDefinitionDTO ToDTO(InsertionDefinitionDO source)
        {
            if (source == null) return null;

            return new InsertionDefinitionDTO
            {
                Label = source.Label,
                XmlPath = source.XmlPath,
                DefaultNode = DefaultNodeMapper.ToDTO(source.DefaultNode)
            };
        }

        public static InsertionDefinitionDO ToDO(InsertionDefinitionDTO source)
        {
            if (source == null) return null;

            return new InsertionDefinitionDO
            {
                Label = source.Label,
                XmlPath = source.XmlPath,
                DefaultNode = DefaultNodeMapper.ToDO(source.DefaultNode)
            };
        }
    }

    /// <summary>
    /// 默认节点映射器
    /// </summary>
    internal static class DefaultNodeMapper
    {
        public static DefaultNodeDTO ToDTO(DefaultNodeDO source)
        {
            if (source == null) return null;

            return new DefaultNodeDTO
            {
                AnyElements = source.AnyElements
            };
        }

        public static DefaultNodeDO ToDO(DefaultNodeDTO source)
        {
            if (source == null) return null;

            return new DefaultNodeDO
            {
                AnyElements = source.AnyElements
            };
        }
    }

    /// <summary>
    /// 树形视图上下文菜单映射器
    /// </summary>
    internal static class TreeviewContextMenuMapper
    {
        public static TreeviewContextMenuDTO ToDTO(TreeviewContextMenuDO source)
        {
            if (source == null) return null;

            return new TreeviewContextMenuDTO
            {
                ItemList = source.ItemList?
                    .Select(ContextMenuItemMapper.ToDTO)
                    .ToList() ?? new List<ContextMenuItemDTO>()
            };
        }

        public static TreeviewContextMenuDO ToDO(TreeviewContextMenuDTO source)
        {
            if (source == null) return null;

            return new TreeviewContextMenuDO
            {
                ItemList = source.ItemList?
                    .Select(ContextMenuItemMapper.ToDO)
                    .ToList() ?? new List<ContextMenuItemDO>()
            };
        }
    }

    /// <summary>
    /// 上下文菜单项映射器
    /// </summary>
    internal static class ContextMenuItemMapper
    {
        public static ContextMenuItemDTO ToDTO(ContextMenuItemDO source)
        {
            if (source == null) return null;

            return new ContextMenuItemDTO
            {
                Name = source.Name,
                ActionCode = source.ActionCode
            };
        }

        public static ContextMenuItemDO ToDO(ContextMenuItemDTO source)
        {
            if (source == null) return null;

            return new ContextMenuItemDO
            {
                Name = source.Name,
                ActionCode = source.ActionCode
            };
        }
    }

    /// <summary>
    /// 布局项映射器
    /// </summary>
    internal static class ItemsMapper
    {
        public static ItemsDTO ToDTO(ItemsDO source)
        {
            if (source == null) return null;

            return new ItemsDTO
            {
                ItemList = source.ItemList?
                    .Select(LayoutsItemMapper.ToDTO)
                    .ToList() ?? new List<ItemDTO>()
            };
        }

        public static ItemsDO ToDO(ItemsDTO source)
        {
            if (source == null) return null;

            return new ItemsDO
            {
                ItemList = source.ItemList?
                    .Select(LayoutsItemMapper.ToDO)
                    .ToList() ?? new List<ItemDO>()
            };
        }
    }

    /// <summary>
    /// 单个布局项映射器
    /// </summary>
    internal static class LayoutsItemMapper
    {
        public static ItemDTO ToDTO(ItemDO source)
        {
            if (source == null) return null;

            return new ItemDTO
            {
                Name = source.Name,
                Label = source.Label,
                Type = source.Type,
                Column = source.Column,
                XmlPath = source.XmlPath,
                Optional = source.Optional,
                Properties = PropertiesMapper.ToDTO(source.Properties)
            };
        }

        public static ItemDO ToDO(ItemDTO source)
        {
            if (source == null) return null;

            return new ItemDO
            {
                Name = source.Name,
                Label = source.Label,
                Type = source.Type,
                Column = source.Column,
                XmlPath = source.XmlPath,
                Optional = source.Optional,
                Properties = PropertiesMapper.ToDO(source.Properties)
            };
        }
    }

    /// <summary>
    /// 属性映射器
    /// </summary>
    internal static class PropertiesMapper
    {
        public static PropertiesDTO ToDTO(PropertiesDO source)
        {
            if (source == null) return null;

            return new PropertiesDTO
            {
                PropertyList = source.PropertyList?
                    .Select(PropertyMapper.ToDTO)
                    .ToList() ?? new List<PropertyDTO>()
            };
        }

        public static PropertiesDO ToDO(PropertiesDTO source)
        {
            if (source == null) return null;

            return new PropertiesDO
            {
                PropertyList = source.PropertyList?
                    .Select(PropertyMapper.ToDO)
                    .ToList() ?? new List<PropertyDO>()
            };
        }
    }

    /// <summary>
    /// 单个属性映射器
    /// </summary>
    internal static class PropertyMapper
    {
        public static PropertyDTO ToDTO(PropertyDO source)
        {
            if (source == null) return null;

            return new PropertyDTO
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        public static PropertyDO ToDO(PropertyDTO source)
        {
            if (source == null) return null;

            return new PropertyDO
            {
                Name = source.Name,
                Value = source.Value
            };
        }
    }
}