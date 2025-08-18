using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Models.DTO.Layouts;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 物理材质布局配置的映射器
    /// 用于PhysicsMaterialsLayoutDO和PhysicsMaterialsLayoutDTO之间的双向转换
    /// </summary>
    public static class PhysicsMaterialsLayoutMapper
    {
        public static PhysicsMaterialsLayoutDTO ToDTO(PhysicsMaterialsLayoutDO source)
        {
            if (source == null) return null;

            return new PhysicsMaterialsLayoutDTO
            {
                Type = source.Type,
                HasLayouts = source.HasLayouts,
                Layouts = LayoutsContainerMapper.ToDTO(source.Layouts)
            };
        }

        public static PhysicsMaterialsLayoutDO ToDO(PhysicsMaterialsLayoutDTO source)
        {
            if (source == null) return null;

            var layouts = LayoutsContainerMapper.ToDO(source.Layouts);
            var hasLayouts = source.HasLayouts || (layouts.LayoutList != null && layouts.LayoutList.Count > 0);
            
            return new PhysicsMaterialsLayoutDO
            {
                Type = source.Type,
                HasLayouts = hasLayouts,
                Layouts = layouts
            };
        }
    }
}