using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class ClothBodiesMapper
    {
        public static ClothBodiesDTO ToDTO(ClothBodiesDO source)
        {
            if (source == null) return null;
            
            return new ClothBodiesDTO
            {
                Bodies = source.Bodies?
                    .Select(ClothBodyMapper.ToDTO)
                    .ToList() ?? new List<ClothBodyDTO>()
            };
        }

        public static ClothBodiesDO ToDO(ClothBodiesDTO source)
        {
            if (source == null) return null;
            
            return new ClothBodiesDO
            {
                Bodies = source.Bodies?
                    .Select(ClothBodyMapper.ToDO)
                    .ToList() ?? new List<ClothBodyDO>()
            };
        }
    }

    public static class ClothBodyMapper
    {
        public static ClothBodyDTO ToDTO(ClothBodyDO source)
        {
            if (source == null) return null;
            
            return new ClothBodyDTO
            {
                Name = source.Name,
                OwnerSkeleton = source.OwnerSkeleton,
                Capsules = ClothCapsulesMapper.ToDTO(source.Capsules)
            };
        }

        public static ClothBodyDO ToDO(ClothBodyDTO source)
        {
            if (source == null) return null;
            
            return new ClothBodyDO
            {
                Name = source.Name,
                OwnerSkeleton = source.OwnerSkeleton,
                Capsules = ClothCapsulesMapper.ToDO(source.Capsules)
            };
        }
    }

    public static class ClothCapsulesMapper
    {
        public static ClothCapsulesDTO ToDTO(ClothCapsulesDO source)
        {
            if (source == null) return null;
            
            return new ClothCapsulesDTO
            {
                CapsuleList = source.CapsuleList?
                    .Select(ClothCapsuleMapper.ToDTO)
                    .ToList() ?? new List<ClothCapsuleDTO>()
            };
        }

        public static ClothCapsulesDO ToDO(ClothCapsulesDTO source)
        {
            if (source == null) return null;
            
            return new ClothCapsulesDO
            {
                CapsuleList = source.CapsuleList?
                    .Select(ClothCapsuleMapper.ToDO)
                    .ToList() ?? new List<ClothCapsuleDO>()
            };
        }
    }

    public static class ClothCapsuleMapper
    {
        public static ClothCapsuleDTO ToDTO(ClothCapsuleDO source)
        {
            if (source == null) return null;
            
            return new ClothCapsuleDTO
            {
                Name = source.Name,
                Length = source.Length,
                Origin = source.Origin,
                Frame = source.Frame,
                Points = ClothPointsMapper.ToDTO(source.Points)
            };
        }

        public static ClothCapsuleDO ToDO(ClothCapsuleDTO source)
        {
            if (source == null) return null;
            
            return new ClothCapsuleDO
            {
                Name = source.Name,
                Length = source.Length,
                Origin = source.Origin,
                Frame = source.Frame,
                Points = ClothPointsMapper.ToDO(source.Points)
            };
        }
    }

    public static class ClothPointsMapper
    {
        public static ClothPointsDTO ToDTO(ClothPointsDO source)
        {
            if (source == null) return null;
            
            return new ClothPointsDTO
            {
                PointList = source.PointList?
                    .Select(ClothPointMapper.ToDTO)
                    .ToList() ?? new List<ClothPointDTO>()
            };
        }

        public static ClothPointsDO ToDO(ClothPointsDTO source)
        {
            if (source == null) return null;
            
            return new ClothPointsDO
            {
                PointList = source.PointList?
                    .Select(ClothPointMapper.ToDO)
                    .ToList() ?? new List<ClothPointDO>()
            };
        }
    }

    public static class ClothPointMapper
    {
        public static ClothPointDTO ToDTO(ClothPointDO source)
        {
            if (source == null) return null;
            
            return new ClothPointDTO
            {
                Radius = source.Radius,
                Bones = ClothBonesMapper.ToDTO(source.Bones)
            };
        }

        public static ClothPointDO ToDO(ClothPointDTO source)
        {
            if (source == null) return null;
            
            return new ClothPointDO
            {
                Radius = source.Radius,
                Bones = ClothBonesMapper.ToDO(source.Bones)
            };
        }
    }

    public static class ClothBonesMapper
    {
        public static ClothBonesDTO ToDTO(ClothBonesDO source)
        {
            if (source == null) return null;
            
            return new ClothBonesDTO
            {
                BoneList = source.BoneList?
                    .Select(ClothBoneMapper.ToDTO)
                    .ToList() ?? new List<ClothBoneDTO>()
            };
        }

        public static ClothBonesDO ToDO(ClothBonesDTO source)
        {
            if (source == null) return null;
            
            return new ClothBonesDO
            {
                BoneList = source.BoneList?
                    .Select(ClothBoneMapper.ToDO)
                    .ToList() ?? new List<ClothBoneDO>()
            };
        }
    }

    public static class ClothBoneMapper
    {
        public static ClothBoneDTO ToDTO(ClothBoneDO source)
        {
            if (source == null) return null;
            
            return new ClothBoneDTO
            {
                Name = source.Name,
                Weight = source.Weight
            };
        }

        public static ClothBoneDO ToDO(ClothBoneDTO source)
        {
            if (source == null) return null;
            
            return new ClothBoneDO
            {
                Name = source.Name,
                Weight = source.Weight
            };
        }
    }
}