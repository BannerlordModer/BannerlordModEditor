using System;
using System.Collections.Generic;
using System.Linq;

namespace BannerlordModEditor.Common.Mappers
{
    public static class CraftingTemplatesMapper
    {
        public static CraftingTemplatesDTO ToDTO(CraftingTemplatesDO source)
        {
            if (source == null) return null;

            return new CraftingTemplatesDTO
            {
                CraftingTemplateList = source.CraftingTemplateList?
                    .Select(CraftingTemplateMapper.ToDTO)
                    .ToList() ?? new List<CraftingTemplateDTO>()
            };
        }

        public static CraftingTemplatesDO ToDO(CraftingTemplatesDTO source)
        {
            if (source == null) return null;

            return new CraftingTemplatesDO
            {
                CraftingTemplateList = source.CraftingTemplateList?
                    .Select(CraftingTemplateMapper.ToDO)
                    .ToList() ?? new List<CraftingTemplateDO>()
            };
        }

        public static CraftingTemplateDTO ToDTO(CraftingTemplateDO source)
        {
            if (source == null) return null;

            return new CraftingTemplateDTO
            {
                Id = source.Id,
                ItemType = source.ItemType,
                ModifierGroup = source.ModifierGroup,
                ItemHolsters = source.ItemHolsters,
                PieceTypeToScaleHolsterWith = source.PieceTypeToScaleHolsterWith,
                HiddenPieceTypesOnHolster = source.HiddenPieceTypesOnHolster,
                DefaultItemHolsterPositionOffset = source.DefaultItemHolsterPositionOffset,
                AlwaysShowHolsterWithWeapon = source.AlwaysShowHolsterWithWeapon,
                UseWeaponAsHolsterMesh = source.UseWeaponAsHolsterMesh,
                PieceDatas = PieceDatasMapper.ToDTO(source.PieceDatas),
                WeaponDescriptions = CraftingWeaponDescriptionsMapper.ToDTO(source.WeaponDescriptions),
                StatsDataList = source.StatsDataList?
                    .Select(StatsDataMapper.ToDTO)
                    .ToList() ?? new List<StatsDataDTO>(),
                UsablePieces = UsablePiecesMapper.ToDTO(source.UsablePieces)
            };
        }

        public static CraftingTemplateDO ToDO(CraftingTemplateDTO source)
        {
            if (source == null) return null;

            return new CraftingTemplateDO
            {
                Id = source.Id,
                ItemType = source.ItemType,
                ModifierGroup = source.ModifierGroup,
                ItemHolsters = source.ItemHolsters,
                PieceTypeToScaleHolsterWith = source.PieceTypeToScaleHolsterWith,
                HiddenPieceTypesOnHolster = source.HiddenPieceTypesOnHolster,
                DefaultItemHolsterPositionOffset = source.DefaultItemHolsterPositionOffset,
                AlwaysShowHolsterWithWeapon = source.AlwaysShowHolsterWithWeapon,
                UseWeaponAsHolsterMesh = source.UseWeaponAsHolsterMesh,
                PieceDatas = PieceDatasMapper.ToDO(source.PieceDatas),
                WeaponDescriptions = CraftingWeaponDescriptionsMapper.ToDO(source.WeaponDescriptions),
                StatsDataList = source.StatsDataList?
                    .Select(StatsDataMapper.ToDO)
                    .ToList() ?? new List<StatsDataDO>(),
                UsablePieces = UsablePiecesMapper.ToDO(source.UsablePieces)
            };
        }

        public static PieceDatasDTO? ToDTO(PieceDatasDO? source)
        {
            if (source == null) return null;

            return new PieceDatasDTO
            {
                PieceDataList = source.PieceDataList?
                    .Select(PieceDataMapper.ToDTO)
                    .ToList() ?? new List<PieceDataDTO>()
            };
        }

        public static PieceDatasDO? ToDO(PieceDatasDTO? source)
        {
            if (source == null) return null;

            return new PieceDatasDO
            {
                PieceDataList = source.PieceDataList?
                    .Select(PieceDataMapper.ToDO)
                    .ToList() ?? new List<PieceDataDO>()
            };
        }

        public static PieceDataDTO ToDTO(PieceDataDO source)
        {
            if (source == null) return null;

            return new PieceDataDTO
            {
                PieceType = source.PieceType,
                BuildOrder = source.BuildOrder
            };
        }

        public static PieceDataDO ToDO(PieceDataDTO source)
        {
            if (source == null) return null;

            return new PieceDataDO
            {
                PieceType = source.PieceType,
                BuildOrder = source.BuildOrder
            };
        }

        public static CraftingWeaponDescriptionsDTO? ToDTO(CraftingWeaponDescriptionsDO? source)
        {
            if (source == null) return null;

            return new CraftingWeaponDescriptionsDTO
            {
                WeaponDescriptionList = source.WeaponDescriptionList?
                    .Select(CraftingWeaponDescriptionMapper.ToDTO)
                    .ToList() ?? new List<CraftingWeaponDescriptionDTO>()
            };
        }

        public static CraftingWeaponDescriptionsDO? ToDO(CraftingWeaponDescriptionsDTO? source)
        {
            if (source == null) return null;

            return new CraftingWeaponDescriptionsDO
            {
                WeaponDescriptionList = source.WeaponDescriptionList?
                    .Select(CraftingWeaponDescriptionMapper.ToDO)
                    .ToList() ?? new List<CraftingWeaponDescriptionDO>()
            };
        }

        public static CraftingWeaponDescriptionDTO ToDTO(CraftingWeaponDescriptionDO source)
        {
            if (source == null) return null;

            return new CraftingWeaponDescriptionDTO
            {
                Id = source.Id
            };
        }

        public static CraftingWeaponDescriptionDO ToDO(CraftingWeaponDescriptionDTO source)
        {
            if (source == null) return null;

            return new CraftingWeaponDescriptionDO
            {
                Id = source.Id
            };
        }

        public static StatsDataDTO ToDTO(StatsDataDO source)
        {
            if (source == null) return null;

            return new StatsDataDTO
            {
                WeaponDescription = source.WeaponDescription,
                StatDataList = source.StatDataList?
                    .Select(StatDataMapper.ToDTO)
                    .ToList() ?? new List<StatDataDTO>()
            };
        }

        public static StatsDataDO ToDO(StatsDataDTO source)
        {
            if (source == null) return null;

            return new StatsDataDO
            {
                WeaponDescription = source.WeaponDescription,
                StatDataList = source.StatDataList?
                    .Select(StatDataMapper.ToDO)
                    .ToList() ?? new List<StatDataDO>()
            };
        }

        public static StatDataDTO ToDTO(StatDataDO source)
        {
            if (source == null) return null;

            return new StatDataDTO
            {
                StatType = source.StatType,
                MaxValue = source.MaxValue
            };
        }

        public static StatDataDO ToDO(StatDataDTO source)
        {
            if (source == null) return null;

            return new StatDataDO
            {
                StatType = source.StatType,
                MaxValue = source.MaxValue
            };
        }

        public static UsablePiecesDTO? ToDTO(UsablePiecesDO? source)
        {
            if (source == null) return null;

            return new UsablePiecesDTO
            {
                UsablePieceList = source.UsablePieceList?
                    .Select(UsablePieceMapper.ToDTO)
                    .ToList() ?? new List<UsablePieceDTO>()
            };
        }

        public static UsablePiecesDO? ToDO(UsablePiecesDTO? source)
        {
            if (source == null) return null;

            return new UsablePiecesDO
            {
                UsablePieceList = source.UsablePieceList?
                    .Select(UsablePieceMapper.ToDO)
                    .ToList() ?? new List<UsablePieceDO>()
            };
        }

        public static UsablePieceDTO ToDTO(UsablePieceDO source)
        {
            if (source == null) return null;

            return new UsablePieceDTO
            {
                PieceId = source.PieceId,
                MpPiece = source.MpPiece
            };
        }

        public static UsablePieceDO ToDO(UsablePieceDTO source)
        {
            if (source == null) return null;

            return new UsablePieceDO
            {
                PieceId = source.PieceId,
                MpPiece = source.MpPiece
            };
        }
    }
}