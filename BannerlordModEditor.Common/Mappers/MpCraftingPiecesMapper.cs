using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class MpCraftingPiecesMapper
    {
        public static MpCraftingPiecesDTO ToDTO(MpCraftingPiecesDO source)
        {
            if (source == null)
                return null;

            return new MpCraftingPiecesDTO
            {
                CraftingPieceList = source.CraftingPieceList?.Select(ToDTO).ToList() ?? new List<MpCraftingPieceDTO>()
            };
        }

        public static MpCraftingPieceDTO ToDTO(MpCraftingPieceDO source)
        {
            if (source == null)
                return null;

            return new MpCraftingPieceDTO
            {
                Id = source.Id,
                Name = source.Name,
                Tier = source.Tier,
                PieceType = source.PieceType,
                Mesh = source.Mesh,
                Scale = source.Scale,
                IsCraftable = source.IsCraftable
            };
        }

        public static MpCraftingPiecesDO ToDO(MpCraftingPiecesDTO source)
        {
            if (source == null)
                return null;

            return new MpCraftingPiecesDO
            {
                CraftingPieceList = source.CraftingPieceList?.Select(ToDo).ToList() ?? new List<MpCraftingPieceDO>()
            };
        }

        public static MpCraftingPieceDO ToDo(MpCraftingPieceDTO source)
        {
            if (source == null)
                return null;

            return new MpCraftingPieceDO
            {
                Id = source.Id,
                Name = source.Name,
                Tier = source.Tier,
                PieceType = source.PieceType,
                Mesh = source.Mesh,
                Scale = source.Scale,
                IsCraftable = source.IsCraftable
            };
        }
    }
}