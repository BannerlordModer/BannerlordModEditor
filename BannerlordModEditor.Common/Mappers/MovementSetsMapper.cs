using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 动作集合配置的映射器
    /// 用于MovementSetsDO和MovementSetsDTO之间的双向转换
    /// </summary>
    public static class MovementSetsMapper
    {
        public static MovementSetsDTO ToDTO(MovementSetsDO source)
        {
            if (source == null) return null;

            return new MovementSetsDTO
            {
                MovementSetList = ConvertMovementSetList(source.MovementSetList),
                HasMovementSetList = source.HasMovementSetList
            };
        }

        public static MovementSetsDO ToDO(MovementSetsDTO source)
        {
            if (source == null) return null;

            var movementSetList = ConvertMovementSetList(source.MovementSetList);
            var hasMovementSetList = source.HasMovementSetList || 
                (movementSetList != null && movementSetList.Count > 0);

            return new MovementSetsDO
            {
                MovementSetList = movementSetList ?? new List<MovementSetDO>(),
                HasMovementSetList = hasMovementSetList
            };
        }

        private static List<MovementSetDTO> ConvertMovementSetList(List<MovementSetDO> sourceList)
        {
            if (sourceList == null || sourceList.Count == 0)
                return new List<MovementSetDTO>();

            var result = new List<MovementSetDTO>(sourceList.Count);
            foreach (var source in sourceList)
            {
                result.Add(ConvertMovementSet(source));
            }
            return result;
        }

        private static List<MovementSetDO> ConvertMovementSetList(List<MovementSetDTO> sourceList)
        {
            if (sourceList == null || sourceList.Count == 0)
                return new List<MovementSetDO>();

            var result = new List<MovementSetDO>(sourceList.Count);
            foreach (var source in sourceList)
            {
                result.Add(ConvertMovementSet(source));
            }
            return result;
        }

        private static MovementSetDTO ConvertMovementSet(MovementSetDO source)
        {
            if (source == null) return null;

            return new MovementSetDTO
            {
                Id = source.Id,
                Idle = source.Idle,
                Forward = source.Forward,
                Backward = source.Backward,
                Right = source.Right,
                RightBack = source.RightBack,
                Left = source.Left,
                LeftBack = source.LeftBack,
                LeftToRight = source.LeftToRight,
                RightToLeft = source.RightToLeft,
                Rotate = source.Rotate,
                ForwardAdder = source.ForwardAdder,
                BackwardAdder = source.BackwardAdder,
                RightAdder = source.RightAdder,
                RightBackAdder = source.RightBackAdder,
                LeftAdder = source.LeftAdder,
                LeftBackAdder = source.LeftBackAdder,
                LeftToRightAdder = source.LeftToRightAdder,
                RightToLeftAdder = source.RightToLeftAdder
            };
        }

        private static MovementSetDO ConvertMovementSet(MovementSetDTO source)
        {
            if (source == null) return null;

            return new MovementSetDO
            {
                Id = source.Id,
                Idle = source.Idle,
                Forward = source.Forward,
                Backward = source.Backward,
                Right = source.Right,
                RightBack = source.RightBack,
                Left = source.Left,
                LeftBack = source.LeftBack,
                LeftToRight = source.LeftToRight,
                RightToLeft = source.RightToLeft,
                Rotate = source.Rotate,
                ForwardAdder = source.ForwardAdder,
                BackwardAdder = source.BackwardAdder,
                RightAdder = source.RightAdder,
                RightBackAdder = source.RightBackAdder,
                LeftAdder = source.LeftAdder,
                LeftBackAdder = source.LeftBackAdder,
                LeftToRightAdder = source.LeftToRightAdder,
                RightToLeftAdder = source.RightToLeftAdder
            };
        }
    }
}