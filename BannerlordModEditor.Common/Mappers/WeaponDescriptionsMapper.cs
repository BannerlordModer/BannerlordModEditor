using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// WeaponDescriptions的DO/DTO映射器
    /// 处理武器描述配置的领域对象和数据传输对象之间的双向转换
    /// </summary>
    public static class WeaponDescriptionsMapper
    {
        /// <summary>
        /// 将DO转换为DTO
        /// </summary>
        public static WeaponDescriptionsDTO ToDTO(WeaponDescriptionsDO source)
        {
            if (source == null) return null;

            return new WeaponDescriptionsDTO
            {
                Descriptions = source.Descriptions?
                    .Select(WeaponDescriptionMapper.ToDTO)
                    .Where(dto => dto != null)
                    .ToList() ?? new List<WeaponDescriptionDTO>()
            };
        }

        /// <summary>
        /// 将DTO转换为DO
        /// </summary>
        public static WeaponDescriptionsDO ToDO(WeaponDescriptionsDTO source)
        {
            if (source == null) return null;

            var result = new WeaponDescriptionsDO
            {
                Descriptions = source.Descriptions?
                    .Select(WeaponDescriptionMapper.ToDO)
                    .Where(dobj => dobj != null)
                    .ToList() ?? new List<WeaponDescriptionDO>(),
                HasEmptyDescriptions = source.Descriptions == null || source.Descriptions.Count == 0
            };

            // 初始化索引
            result.InitializeIndexes();

            return result;
        }

        /// <summary>
        /// 批量转换DO列表为DTO列表
        /// </summary>
        public static List<WeaponDescriptionsDTO> ToDTO(List<WeaponDescriptionsDO> source)
        {
            return source?.Select(ToDTO).Where(dto => dto != null).ToList() ?? new List<WeaponDescriptionsDTO>();
        }

        /// <summary>
        /// 批量转换DTO列表为DO列表
        /// </summary>
        public static List<WeaponDescriptionsDO> ToDO(List<WeaponDescriptionsDTO> source)
        {
            return source?.Select(WeaponDescriptionsMapper.ToDO).Where(dobj => dobj != null).ToList() ?? new List<WeaponDescriptionsDO>();
        }

        /// <summary>
        /// 深度复制DO
        /// </summary>
        public static WeaponDescriptionsDO DeepCopy(WeaponDescriptionsDO source)
        {
            if (source == null) return null;

            var dto = ToDTO(source);
            return ToDO(dto);
        }

        /// <summary>
        /// 验证DO对象数据完整性
        /// </summary>
        public static bool Validate(WeaponDescriptionsDO source)
        {
            if (source == null) return false;
            return source.IsValid();
        }

        /// <summary>
        /// 获取验证错误信息
        /// </summary>
        public static List<string> GetValidationErrors(WeaponDescriptionsDO source)
        {
            var errors = new List<string>();

            if (source == null)
            {
                errors.Add("WeaponDescriptionsDO object is null");
                return errors;
            }

            if (source.Descriptions == null || source.Descriptions.Count == 0)
                errors.Add("At least one weapon description is required");

            if (source.Descriptions != null)
            {
                foreach (var desc in source.Descriptions)
                {
                    var descErrors = WeaponDescriptionMapper.GetValidationErrors(desc);
                    errors.AddRange(descErrors.Select(e => $"Weapon '{desc.Id}': {e}"));
                }
            }

            return errors;
        }

        /// <summary>
        /// 合并两个DO对象（source的值覆盖target）
        /// </summary>
        public static WeaponDescriptionsDO Merge(WeaponDescriptionsDO target, WeaponDescriptionsDO source)
        {
            if (target == null) return source;
            if (source == null) return target;

            // 合并描述列表（按ID合并）
            if (source.Descriptions != null)
            {
                if (target.Descriptions == null)
                {
                    target.Descriptions = new List<WeaponDescriptionDO>();
                }

                foreach (var sourceDesc in source.Descriptions)
                {
                    var targetDesc = target.Descriptions
                        .FirstOrDefault(d => d.Id == sourceDesc.Id);

                    if (targetDesc == null)
                    {
                        // 添加新描述
                        targetDesc = WeaponDescriptionMapper.DeepCopy(sourceDesc);
                        target.Descriptions.Add(targetDesc);
                    }
                    else
                    {
                        // 合并现有描述
                        WeaponDescriptionMapper.Merge(targetDesc, sourceDesc);
                    }
                }
            }

            // 更新运行时标记
            target.HasEmptyDescriptions = target.Descriptions.Count == 0;

            // 重新初始化索引
            target.InitializeIndexes();

            return target;
        }
    }

    /// <summary>
    /// WeaponDescription的映射器
    /// </summary>
    public static class WeaponDescriptionMapper
    {
        public static WeaponDescriptionDTO ToDTO(WeaponDescriptionDO source)
        {
            if (source == null) return null;

            return new WeaponDescriptionDTO
            {
                Id = source.Id,
                WeaponClass = source.WeaponClass,
                ItemUsageFeatures = source.ItemUsageFeatures,
                WeaponFlags = WeaponFlagsMapper.ToDTO(source.WeaponFlags),
                AvailablePieces = AvailablePiecesMapper.ToDTO(source.AvailablePieces)
            };
        }

        public static WeaponDescriptionDO ToDO(WeaponDescriptionDTO source)
        {
            if (source == null) return null;

            return new WeaponDescriptionDO
            {
                Id = source.Id,
                WeaponClass = source.WeaponClass,
                ItemUsageFeatures = source.ItemUsageFeatures,
                WeaponFlags = WeaponFlagsMapper.ToDO(source.WeaponFlags),
                AvailablePieces = AvailablePiecesMapper.ToDO(source.AvailablePieces),
                HasWeaponFlags = source.WeaponFlags != null && source.WeaponFlags.Flags.Count > 0,
                HasAvailablePieces = source.AvailablePieces != null && source.AvailablePieces.Pieces.Count > 0
            };
        }

        public static WeaponDescriptionDO DeepCopy(WeaponDescriptionDO source)
        {
            if (source == null) return null;

            var dto = ToDTO(source);
            return ToDO(dto);
        }

        public static List<string> GetValidationErrors(WeaponDescriptionDO source)
        {
            return source?.GetValidationErrors() ?? new List<string> { "WeaponDescriptionDO object is null" };
        }

        public static WeaponDescriptionDO Merge(WeaponDescriptionDO target, WeaponDescriptionDO source)
        {
            if (target == null) return source;
            if (source == null) return target;

            // 合并基本属性（source覆盖target）
            if (!string.IsNullOrEmpty(source.WeaponClass)) target.WeaponClass = source.WeaponClass;
            if (!string.IsNullOrEmpty(source.ItemUsageFeatures)) target.ItemUsageFeatures = source.ItemUsageFeatures;

            // 合并标志
            if (source.WeaponFlags != null)
            {
                target.WeaponFlags = WeaponFlagsMapper.DeepCopy(source.WeaponFlags);
            }

            // 合并可用部件
            if (source.AvailablePieces != null)
            {
                target.AvailablePieces = AvailablePiecesMapper.DeepCopy(source.AvailablePieces);
            }

            // 更新运行时标记
            target.HasWeaponFlags = target.WeaponFlags?.Flags?.Any() ?? false;
            target.HasAvailablePieces = target.AvailablePieces?.Pieces?.Any() ?? false;

            return target;
        }
    }

    /// <summary>
    /// WeaponFlags的映射器
    /// </summary>
    public static class WeaponFlagsMapper
    {
        public static WeaponFlagsDTO ToDTO(WeaponFlagsDO source)
        {
            if (source == null) return null;

            return new WeaponFlagsDTO
            {
                Flags = source.Flags?
                    .Select(WeaponFlagMapper.ToDTO)
                    .Where(dto => dto != null)
                    .ToList() ?? new List<WeaponFlagDTO>()
            };
        }

        public static WeaponFlagsDO ToDO(WeaponFlagsDTO source)
        {
            if (source == null) return null;

            return new WeaponFlagsDO
            {
                Flags = source.Flags?
                    .Select(WeaponFlagMapper.ToDO)
                    .Where(dobj => dobj != null)
                    .ToList() ?? new List<WeaponFlagDO>()
            };
        }

        public static WeaponFlagsDO DeepCopy(WeaponFlagsDO source)
        {
            if (source == null) return null;

            var dto = ToDTO(source);
            return ToDO(dto);
        }
    }

    /// <summary>
    /// WeaponFlag的映射器
    /// </summary>
    public static class WeaponFlagMapper
    {
        public static WeaponFlagDTO ToDTO(WeaponFlagDO source)
        {
            if (source == null) return null;

            return new WeaponFlagDTO
            {
                Value = source.Value
            };
        }

        public static WeaponFlagDO ToDO(WeaponFlagDTO source)
        {
            if (source == null) return null;

            return new WeaponFlagDO
            {
                Value = source.Value
            };
        }

        public static WeaponFlagDO DeepCopy(WeaponFlagDO source)
        {
            if (source == null) return null;

            return new WeaponFlagDO
            {
                Value = source.Value
            };
        }
    }

    /// <summary>
    /// AvailablePieces的映射器
    /// </summary>
    public static class AvailablePiecesMapper
    {
        public static AvailablePiecesDTO ToDTO(AvailablePiecesDO source)
        {
            if (source == null) return null;

            return new AvailablePiecesDTO
            {
                Pieces = source.Pieces?
                    .Select(AvailablePieceMapper.ToDTO)
                    .Where(dto => dto != null)
                    .ToList() ?? new List<AvailablePieceDTO>()
            };
        }

        public static AvailablePiecesDO ToDO(AvailablePiecesDTO source)
        {
            if (source == null) return null;

            return new AvailablePiecesDO
            {
                Pieces = source.Pieces?
                    .Select(AvailablePieceMapper.ToDO)
                    .Where(dobj => dobj != null)
                    .ToList() ?? new List<AvailablePieceDO>()
            };
        }

        public static AvailablePiecesDO DeepCopy(AvailablePiecesDO source)
        {
            if (source == null) return null;

            var dto = ToDTO(source);
            return ToDO(dto);
        }
    }

    /// <summary>
    /// AvailablePiece的映射器
    /// </summary>
    public static class AvailablePieceMapper
    {
        public static AvailablePieceDTO ToDTO(AvailablePieceDO source)
        {
            if (source == null) return null;

            return new AvailablePieceDTO
            {
                Id = source.Id
            };
        }

        public static AvailablePieceDO ToDO(AvailablePieceDTO source)
        {
            if (source == null) return null;

            return new AvailablePieceDO
            {
                Id = source.Id
            };
        }

        public static AvailablePieceDO DeepCopy(AvailablePieceDO source)
        {
            if (source == null) return null;

            return new AvailablePieceDO
            {
                Id = source.Id
            };
        }
    }
}