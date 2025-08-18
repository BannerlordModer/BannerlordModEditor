using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Models.DTO.Layouts;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 动画布局配置的映射器
    /// 用于AnimationsLayoutDO和AnimationsLayoutDTO之间的双向转换
    /// </summary>
    public static class AnimationsLayoutMapper
    {
        public static AnimationsLayoutDTO ToDTO(AnimationsLayoutDO source)
        {
            if (source == null) return null;

            return new AnimationsLayoutDTO
            {
                Type = source.Type,
                HasLayouts = source.HasLayouts,
                Layouts = LayoutsContainerMapper.ToDTO(source.Layouts)
            };
        }

        public static AnimationsLayoutDO ToDO(AnimationsLayoutDTO source)
        {
            if (source == null) return null;

            var layouts = LayoutsContainerMapper.ToDO(source.Layouts);
            var hasLayouts = source.HasLayouts || (layouts.LayoutList != null && layouts.LayoutList.Count > 0);
            
            return new AnimationsLayoutDO
            {
                Type = source.Type,
                HasLayouts = hasLayouts,
                Layouts = layouts
            };
        }
    }
}