using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Models.DTO.Layouts;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 物品挂件布局配置的映射器
    /// 用于ItemHolstersLayoutDO和ItemHolstersLayoutDTO之间的双向转换
    /// </summary>
    public static class ItemHolstersLayoutMapper
    {
        public static ItemHolstersLayoutDTO ToDTO(ItemHolstersLayoutDO source)
        {
            if (source == null) return null;

            return new ItemHolstersLayoutDTO
            {
                Type = source.Type,
                HasLayouts = source.HasLayouts,
                Layouts = LayoutsContainerMapper.ToDTO(source.Layouts)
            };
        }

        public static ItemHolstersLayoutDO ToDO(ItemHolstersLayoutDTO source)
        {
            if (source == null) return null;

            var layouts = LayoutsContainerMapper.ToDO(source.Layouts);
            var hasLayouts = source.HasLayouts || (layouts.LayoutList != null && layouts.LayoutList.Count > 0);
            
            return new ItemHolstersLayoutDO
            {
                Type = source.Type,
                HasLayouts = hasLayouts,
                Layouts = layouts
            };
        }
    }
}