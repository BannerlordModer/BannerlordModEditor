using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Models.DTO.Layouts;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 粒子系统布局配置的映射器
    /// 用于ParticleSystemLayoutDO和ParticleSystemLayoutDTO之间的双向转换
    /// </summary>
    public static class ParticleSystemLayoutMapper
    {
        public static ParticleSystemLayoutDTO ToDTO(ParticleSystemLayoutDO source)
        {
            if (source == null) return null;

            return new ParticleSystemLayoutDTO
            {
                Type = source.Type,
                Layouts = LayoutsContainerMapper.ToDTO(source.Layouts)
            };
        }

        public static ParticleSystemLayoutDO ToDO(ParticleSystemLayoutDTO source)
        {
            if (source == null) return null;

            return new ParticleSystemLayoutDO
            {
                Type = source.Type,
                Layouts = LayoutsContainerMapper.ToDO(source.Layouts)
            };
        }
    }
}