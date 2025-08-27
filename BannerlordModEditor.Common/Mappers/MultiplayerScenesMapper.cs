using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class MultiplayerScenesMapper
    {
        public static MultiplayerScenesDTO ToDTO(MultiplayerScenesDO source)
        {
            if (source == null) return null;
            
            return new MultiplayerScenesDTO
            {
                Scenes = source.Scenes?
                    .Select(SceneMapper.ToDTO)
                    .ToList() ?? new List<SceneDTO>()
            };
        }

        public static MultiplayerScenesDO ToDO(MultiplayerScenesDTO source)
        {
            if (source == null) return null;
            
            return new MultiplayerScenesDO
            {
                Scenes = source.Scenes?
                    .Select(SceneMapper.ToDO)
                    .ToList() ?? new List<SceneDO>()
            };
        }
    }

    public static class SceneMapper
    {
        public static SceneDTO ToDTO(SceneDO source)
        {
            if (source == null) return null;
            
            return new SceneDTO
            {
                Name = source.Name,
                GameTypes = source.GameTypes?
                    .Select(GameTypeMapper.ToDTO)
                    .ToList() ?? new List<GameTypeDTO>()
            };
        }

        public static SceneDO ToDO(SceneDTO source)
        {
            if (source == null) return null;
            
            return new SceneDO
            {
                Name = source.Name,
                GameTypes = source.GameTypes?
                    .Select(GameTypeMapper.ToDO)
                    .ToList() ?? new List<GameTypeDO>()
            };
        }
    }

    public static class GameTypeMapper
    {
        public static GameTypeDTO ToDTO(GameTypeDO source)
        {
            if (source == null) return null;
            
            return new GameTypeDTO
            {
                Name = source.Name
            };
        }

        public static GameTypeDO ToDO(GameTypeDTO source)
        {
            if (source == null) return null;
            
            return new GameTypeDO
            {
                Name = source.Name
            };
        }
    }
}