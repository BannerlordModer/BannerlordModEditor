using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Models.DTO.Layouts;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 骨骼布局配置的映射器
    /// 用于SkeletonsLayoutDO和SkeletonsLayoutDTO之间的双向转换
    /// </summary>
    public static class SkeletonsLayoutMapper
    {
        /// <summary>
        /// 将SkeletonsLayoutDO转换为SkeletonsLayoutDTO
        /// </summary>
        public static SkeletonsLayoutDTO ToDTO(SkeletonsLayoutDO source)
        {
            if (source == null) return null;

            return new SkeletonsLayoutDTO
            {
                Type = source.Type,
                HasLayouts = source.HasLayouts,
                Layouts = LayoutsContainerMapper.ToDTO(source.Layouts)
            };
        }

        /// <summary>
        /// 将SkeletonsLayoutDTO转换为SkeletonsLayoutDO
        /// </summary>
        public static SkeletonsLayoutDO ToDO(SkeletonsLayoutDTO source)
        {
            if (source == null) return null;

            return new SkeletonsLayoutDO
            {
                Type = source.Type,
                HasLayouts = source.HasLayouts,
                Layouts = LayoutsContainerMapper.ToDO(source.Layouts)
            };
        }
    }
}