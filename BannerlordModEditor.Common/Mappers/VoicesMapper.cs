using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class VoicesMapper
    {
        public static VoicesBaseDTO ToDTO(VoicesBaseDO source)
        {
            if (source == null) return null;

            return new VoicesBaseDTO
            {
                Type = source.Type,
                FaceAnimationRecords = FaceAnimationRecordsToDTO(source.FaceAnimationRecords)
            };
        }

        public static VoicesBaseDO ToDO(VoicesBaseDTO source)
        {
            if (source == null) return null;

            return new VoicesBaseDO
            {
                Type = source.Type,
                FaceAnimationRecords = FaceAnimationRecordsToDo(source.FaceAnimationRecords)
            };
        }

        public static FaceAnimationRecordsDTO FaceAnimationRecordsToDTO(FaceAnimationRecordsDO source)
        {
            if (source == null) return null;

            return new FaceAnimationRecordsDTO
            {
                FaceAnimationRecordList = source.FaceAnimationRecordList?.Select(FaceAnimationRecordToDTO).ToList() ?? new List<FaceAnimationRecordDTO>()
            };
        }

        public static FaceAnimationRecordsDO FaceAnimationRecordsToDo(FaceAnimationRecordsDTO source)
        {
            if (source == null) return null;

            return new FaceAnimationRecordsDO
            {
                FaceAnimationRecordList = source.FaceAnimationRecordList?.Select(FaceAnimationRecordToDo).ToList() ?? new List<FaceAnimationRecordDO>()
            };
        }

        public static FaceAnimationRecordDTO FaceAnimationRecordToDTO(FaceAnimationRecordDO source)
        {
            if (source == null) return null;

            return new FaceAnimationRecordDTO
            {
                Id = source.Id,
                AnimationName = source.AnimationName,
                Flags = AnimationFlagsToDTO(source.Flags)
            };
        }

        public static FaceAnimationRecordDO FaceAnimationRecordToDo(FaceAnimationRecordDTO source)
        {
            if (source == null) return null;

            return new FaceAnimationRecordDO
            {
                Id = source.Id,
                AnimationName = source.AnimationName,
                Flags = AnimationFlagsToDo(source.Flags)
            };
        }

        public static AnimationFlagsDTO? AnimationFlagsToDTO(AnimationFlagsDO? source)
        {
            if (source == null) return null;

            return new AnimationFlagsDTO
            {
                FlagList = source.FlagList?.Select(AnimationFlagToDTO).ToList() ?? new List<AnimationFlagDTO>()
            };
        }

        public static AnimationFlagsDO? AnimationFlagsToDo(AnimationFlagsDTO? source)
        {
            if (source == null) return null;

            return new AnimationFlagsDO
            {
                FlagList = source.FlagList?.Select(AnimationFlagToDo).ToList() ?? new List<AnimationFlagDO>()
            };
        }

        public static AnimationFlagDTO AnimationFlagToDTO(AnimationFlagDO source)
        {
            if (source == null) return null;

            return new AnimationFlagDTO
            {
                Name = source.Name
            };
        }

        public static AnimationFlagDO AnimationFlagToDo(AnimationFlagDTO source)
        {
            if (source == null) return null;

            return new AnimationFlagDO
            {
                Name = source.Name
            };
        }
    }
}