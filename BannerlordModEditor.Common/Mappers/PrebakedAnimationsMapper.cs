using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class PrebakedAnimationsMapper
    {
        public static PrebakedAnimationsDTO ToDTO(PrebakedAnimationsDO source)
        {
            if (source == null) return null;
            
            return new PrebakedAnimationsDTO
            {
                Type = source.Type,
                PrebakedAnimationsList = PrebakedAnimationsListMapper.ToDTO(source.PrebakedAnimationsList)
            };
        }

        public static PrebakedAnimationsDO ToDO(PrebakedAnimationsDTO source)
        {
            if (source == null) return null;
            
            return new PrebakedAnimationsDO
            {
                Type = source.Type,
                PrebakedAnimationsList = PrebakedAnimationsListMapper.ToDO(source.PrebakedAnimationsList)
            };
        }
    }

    public static class PrebakedAnimationsListMapper
    {
        public static PrebakedAnimationsListDTO ToDTO(PrebakedAnimationsListDO source)
        {
            if (source == null) return null;
            
            return new PrebakedAnimationsListDTO
            {
                Animations = source.Animations?
                    .Select(AnimationMapper.ToDTO)
                    .ToList() ?? new List<AnimationDTO>()
            };
        }

        public static PrebakedAnimationsListDO ToDO(PrebakedAnimationsListDTO source)
        {
            if (source == null) return null;
            
            return new PrebakedAnimationsListDO
            {
                Animations = source.Animations?
                    .Select(AnimationMapper.ToDO)
                    .ToList() ?? new List<AnimationDO>()
            };
        }
    }

    public static class AnimationMapper
    {
        public static AnimationDTO ToDTO(AnimationDO source)
        {
            if (source == null) return null;
            
            return new AnimationDTO
            {
                Id = source.Id,
                SkeletonName = source.SkeletonName,
                Bones = source.Bones?
                    .Select(BoneMapper.ToDTO)
                    .ToList() ?? new List<BoneDTO>()
            };
        }

        public static AnimationDO ToDO(AnimationDTO source)
        {
            if (source == null) return null;
            
            return new AnimationDO
            {
                Id = source.Id,
                SkeletonName = source.SkeletonName,
                Bones = source.Bones?
                    .Select(BoneMapper.ToDO)
                    .ToList() ?? new List<BoneDO>()
            };
        }
    }

    public static class BoneMapper
    {
        public static BoneDTO ToDTO(BoneDO source)
        {
            if (source == null) return null;
            
            return new BoneDTO
            {
                Index = source.Index,
                KeyList = source.KeyList
            };
        }

        public static BoneDO ToDO(BoneDTO source)
        {
            if (source == null) return null;
            
            return new BoneDO
            {
                Index = source.Index,
                KeyList = source.KeyList
            };
        }
    }
}