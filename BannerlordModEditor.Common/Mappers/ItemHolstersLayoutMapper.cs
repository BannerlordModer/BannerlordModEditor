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
                Layouts = LayoutsContainerMapper.ToDTO(source.Layouts)
            };
        }

        public static ItemHolstersLayoutDO ToDO(ItemHolstersLayoutDTO source)
        {
            if (source == null) return null;

            return new ItemHolstersLayoutDO
            {
                Type = source.Type,
                Layouts = LayoutsContainerMapper.ToDO(source.Layouts)
            };
        }
    }
}