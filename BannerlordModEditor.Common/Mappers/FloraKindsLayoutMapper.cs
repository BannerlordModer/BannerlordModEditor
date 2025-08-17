using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Models.DTO.Layouts;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 植被种类布局配置的映射器
    /// 用于FloraKindsLayoutDO和FloraKindsLayoutDTO之间的双向转换
    /// </summary>
    public static class FloraKindsLayoutMapper
    {
        public static FloraKindsLayoutDTO ToDTO(FloraKindsLayoutDO source)
        {
            if (source == null) return null;

            return new FloraKindsLayoutDTO
            {
                Type = source.Type,
                Layouts = LayoutsContainerMapper.ToDTO(source.Layouts)
            };
        }

        public static FloraKindsLayoutDO ToDO(FloraKindsLayoutDTO source)
        {
            if (source == null) return null;

            return new FloraKindsLayoutDO
            {
                Type = source.Type,
                Layouts = LayoutsContainerMapper.ToDO(source.Layouts)
            };
        }
    }
}