using System.Reflection;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DO.Game;
using BannerlordModEditor.Common.Models.DO.Audio;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Cli.Services
{
    /// <summary>
    /// 增强的模型类型转换器，支持多种XML模型与Excel之间的转换
    /// </summary>
    public class ModelTypeConverter
    {
        private static readonly Dictionary<string, Func<ExcelData, object>> _excelToModelConverters;
        private static readonly Dictionary<string, Func<object, ExcelData>> _modelToExcelConverters;

        static ModelTypeConverter()
        {
            _excelToModelConverters = new Dictionary<string, Func<ExcelData, object>>(StringComparer.OrdinalIgnoreCase)
            {
                [nameof(ActionTypesDO)] = ConvertExcelToActionTypes,
                [nameof(CombatParametersDO)] = ConvertExcelToCombatParameters,
                [nameof(MapIconsDO)] = ConvertExcelToMapIcons,
                [nameof(AttributesDO)] = ConvertExcelToAttributes,
                [nameof(CraftingPiecesDO)] = ConvertExcelToCraftingPieces,
                [nameof(ItemModifiersDO)] = ConvertExcelToItemModifiers,
                [nameof(SkillsDO)] = ConvertExcelToSkills,
                [nameof(GogAchievementDataDO)] = ConvertExcelToGogAchievementData,
                [nameof(ClothMaterialsDO)] = ConvertExcelToClothMaterials,
                [nameof(DecalSetsDO)] = ConvertExcelToDecalSets,
                [nameof(GlobalStringsDO)] = ConvertExcelToGlobalStrings,
                [nameof(MusicDO)] = ConvertExcelToMusic,
                [nameof(SoundFilesDO)] = ConvertExcelToSoundFiles
            };

            _modelToExcelConverters = new Dictionary<string, Func<object, ExcelData>>(StringComparer.OrdinalIgnoreCase)
            {
                [nameof(ActionTypesDO)] = obj => ConvertActionTypesToExcel((ActionTypesDO)obj),
                [nameof(CombatParametersDO)] = obj => ConvertCombatParametersToExcel((CombatParametersDO)obj),
                [nameof(MapIconsDO)] = obj => ConvertMapIconsToExcel((MapIconsDO)obj),
                [nameof(AttributesDO)] = obj => ConvertAttributesToExcel((AttributesDO)obj),
                [nameof(CraftingPiecesDO)] = obj => ConvertCraftingPiecesToExcel((CraftingPiecesDO)obj),
                [nameof(ItemModifiersDO)] = obj => ConvertItemModifiersToExcel((ItemModifiersDO)obj),
                [nameof(SkillsDO)] = obj => ConvertSkillsToExcel((SkillsDO)obj),
                [nameof(GogAchievementDataDO)] = obj => ConvertGogAchievementDataToExcel((GogAchievementDataDO)obj),
                [nameof(ClothMaterialsDO)] = obj => ConvertClothMaterialsToExcel((ClothMaterialsDO)obj),
                [nameof(DecalSetsDO)] = obj => ConvertDecalSetsToExcel((DecalSetsDO)obj),
                [nameof(GlobalStringsDO)] = obj => ConvertGlobalStringsToExcel((GlobalStringsDO)obj),
                [nameof(MusicDO)] = obj => ConvertMusicToExcel((MusicDO)obj),
                [nameof(SoundFilesDO)] = obj => ConvertSoundFilesToExcel((SoundFilesDO)obj)
            };
        }

        /// <summary>
        /// 将 Excel 数据转换为指定的 XML 模型对象
        /// </summary>
        public static object ConvertExcelToModel(ExcelData excelData, Type modelType)
        {
            var modelTypeName = modelType.Name;
            
            if (_excelToModelConverters.TryGetValue(modelTypeName, out var converter))
            {
                return converter(excelData);
            }
            
            // 使用反射进行通用转换
            return ConvertExcelToModelGeneric(excelData, modelType);
        }

        /// <summary>
        /// 将 XML 模型对象转换为 Excel 数据
        /// </summary>
        public static ExcelData ConvertModelToExcel(object modelObject, string modelType)
        {
            Console.WriteLine($"调试: ConvertModelToExcel 被调用，modelType = {modelType}");
            
            // 将模型类型（如 action_types）转换为DO类型名称（如 ActionTypesDO）
            var doTypeName = ConvertModelTypeToDoTypeName(modelType);
            Console.WriteLine($"调试: doTypeName = {doTypeName}");
            
            if (_modelToExcelConverters.TryGetValue(doTypeName, out var converter))
            {
                Console.WriteLine($"调试: 找到转换器，调用专用转换方法");
                return converter(modelObject);
            }
            
            Console.WriteLine($"调试: 未找到转换器，使用通用转换");
            // 使用反射进行通用转换
            return ConvertModelToExcelGeneric(modelObject);
        }

        /// <summary>
        /// Excel 转 ActionTypes
        /// </summary>
        private static ActionTypesDO ConvertExcelToActionTypes(ExcelData excelData)
        {
            var actionTypes = new ActionTypesDO();
            
            foreach (var row in excelData.Rows)
            {
                var action = new ActionTypeDO();
                
                if (row.TryGetValue("name", out var name))
                    action.Name = name?.ToString();
                
                if (row.TryGetValue("type", out var type))
                    action.Type = type?.ToString();
                
                if (row.TryGetValue("usage_direction", out var usageDirection))
                    action.UsageDirection = usageDirection?.ToString();
                
                if (row.TryGetValue("action_stage", out var actionStage))
                    action.ActionStage = actionStage?.ToString();
                
                actionTypes.Actions.Add(action);
            }
            
            return actionTypes;
        }

        /// <summary>
        /// ActionTypes 转 Excel
        /// </summary>
        private static ExcelData ConvertActionTypesToExcel(ActionTypesDO actionTypes)
        {
            Console.WriteLine($"调试: ConvertActionTypesToExcel 被调用，Actions.Count = {actionTypes.Actions.Count}");
            
            var excelData = new ExcelData
            {
                Headers = new List<string> { "name", "type", "usage_direction", "action_stage" },
                Rows = new List<Dictionary<string, object?>>()
            };
            
            Console.WriteLine($"调试: 开始遍历Actions列表");
            foreach (var action in actionTypes.Actions)
            {
                var row = new Dictionary<string, object?>
                {
                    ["name"] = action.Name,
                    ["type"] = action.Type,
                    ["usage_direction"] = action.UsageDirection,
                    ["action_stage"] = action.ActionStage
                };
                excelData.Rows.Add(row);
            }
            
            Console.WriteLine($"调试: Excel数据生成完成，Rows.Count = {excelData.Rows.Count}");
            return excelData;
        }

        /// <summary>
        /// Excel 转 CombatParameters
        /// </summary>
        private static CombatParametersDO ConvertExcelToCombatParameters(ExcelData excelData)
        {
            var combatParams = new CombatParametersDO();
            
            // 检查是否有 type 列
            if (excelData.Rows.Any() && excelData.Rows[0].TryGetValue("type", out var type))
            {
                combatParams.Type = type?.ToString();
            }
            
            // 处理 definitions
            var definitionRows = excelData.Rows.Where(r => r.ContainsKey("def_name") || r.ContainsKey("def_val")).ToList();
            if (definitionRows.Any())
            {
                combatParams.HasDefinitions = true;
                foreach (var row in definitionRows)
                {
                    var def = new DefDO();
                    if (row.TryGetValue("def_name", out var defName))
                        def.Name = defName?.ToString();
                    if (row.TryGetValue("def_val", out var defVal))
                        def.Value = defVal?.ToString();
                    combatParams.Definitions.Defs.Add(def);
                }
            }
            
            // 处理 combat_parameters
            var combatParamRows = excelData.Rows.Where(r => r.ContainsKey("param_name") || r.ContainsKey("param_val")).ToList();
            if (combatParamRows.Any())
            {
                foreach (var row in combatParamRows)
                {
                    var param = new BaseCombatParameterDO();
                    // 这里需要根据具体的 CombatParameterDO 结构进行设置
                    combatParams.CombatParametersList.Add(param);
                }
            }
            
            return combatParams;
        }

        /// <summary>
        /// CombatParameters 转 Excel
        /// </summary>
        private static ExcelData ConvertCombatParametersToExcel(CombatParametersDO combatParams)
        {
            var excelData = new ExcelData
            {
                Headers = new List<string>(),
                Rows = new List<Dictionary<string, object?>>()
            };
            
            // 添加 type 列
            if (!string.IsNullOrEmpty(combatParams.Type))
            {
                excelData.Headers.Add("type");
                var typeRow = new Dictionary<string, object?>
                {
                    ["type"] = combatParams.Type
                };
                excelData.Rows.Add(typeRow);
            }
            
            // 添加 definitions
            if (combatParams.HasDefinitions && combatParams.Definitions.Defs.Any())
            {
                if (!excelData.Headers.Contains("def_name")) excelData.Headers.Add("def_name");
                if (!excelData.Headers.Contains("def_val")) excelData.Headers.Add("def_val");
                
                foreach (var def in combatParams.Definitions.Defs)
                {
                    var row = new Dictionary<string, object?>
                    {
                        ["def_name"] = def.Name,
                        ["def_val"] = def.Value
                    };
                    excelData.Rows.Add(row);
                }
            }
            
            // 添加 combat_parameters
            if (combatParams.CombatParametersList.Any())
            {
                // 这里需要根据具体的 CombatParameterDO 结构进行设置
            }
            
            return excelData;
        }

        /// <summary>
        /// Excel 转 MapIcons
        /// </summary>
        private static MapIconsDO ConvertExcelToMapIcons(ExcelData excelData)
        {
            var mapIcons = new MapIconsDO();
            
            foreach (var row in excelData.Rows)
            {
                var mapIcon = new MapIconDO();
                
                if (row.TryGetValue("id", out var id))
                    mapIcon.Id = id?.ToString();
                
                if (row.TryGetValue("id_str", out var idStr))
                    mapIcon.IdStr = idStr?.ToString();
                
                if (row.TryGetValue("flags", out var flags))
                    mapIcon.Flags = flags?.ToString();
                
                if (row.TryGetValue("mesh_name", out var meshName))
                    mapIcon.MeshName = meshName?.ToString();
                
                if (row.TryGetValue("mesh_scale", out var meshScale))
                    mapIcon.MeshScale = meshScale?.ToString();
                
                if (row.TryGetValue("sound_no", out var soundNo))
                    mapIcon.SoundNo = soundNo?.ToString();
                
                if (row.TryGetValue("offset_pos", out var offsetPos))
                    mapIcon.OffsetPos = offsetPos?.ToString();
                
                mapIcons.MapIconsContainer.MapIconList.Add(mapIcon);
            }
            
            return mapIcons;
        }

        /// <summary>
        /// MapIcons 转 Excel
        /// </summary>
        private static ExcelData ConvertMapIconsToExcel(MapIconsDO mapIcons)
        {
            var excelData = new ExcelData
            {
                Headers = new List<string> { "id", "id_str", "flags", "mesh_name", "mesh_scale", "sound_no", "offset_pos" },
                Rows = new List<Dictionary<string, object?>>()
            };
            
            foreach (var mapIcon in mapIcons.MapIconsContainer.MapIconList)
            {
                var row = new Dictionary<string, object?>
                {
                    ["id"] = mapIcon.Id,
                    ["id_str"] = mapIcon.IdStr,
                    ["flags"] = mapIcon.Flags,
                    ["mesh_name"] = mapIcon.MeshName,
                    ["mesh_scale"] = mapIcon.MeshScale,
                    ["sound_no"] = mapIcon.SoundNo,
                    ["offset_pos"] = mapIcon.OffsetPos
                };
                excelData.Rows.Add(row);
            }
            
            return excelData;
        }

        #region 新增的转换器方法

        /// <summary>
        /// Excel 转 Attributes
        /// </summary>
        private static AttributesDO ConvertExcelToAttributes(ExcelData excelData)
        {
            var attributes = new AttributesDO();
            
            foreach (var row in excelData.Rows)
            {
                var attribute = new AttributeDataDO();
                
                if (row.TryGetValue("id", out var id))
                    attribute.Id = id?.ToString();
                
                if (row.TryGetValue("name", out var name))
                    attribute.Name = name?.ToString();
                
                if (row.TryGetValue("source", out var source))
                    attribute.Source = source?.ToString();
                
                if (row.TryGetValue("documentation", out var documentation))
                    attribute.Documentation = documentation?.ToString();
                
                attributes.Attributes.Add(attribute);
            }
            
            return attributes;
        }

        /// <summary>
        /// Attributes 转 Excel
        /// </summary>
        private static ExcelData ConvertAttributesToExcel(AttributesDO attributes)
        {
            var excelData = new ExcelData
            {
                Headers = new List<string> { "id", "name", "source", "documentation" },
                Rows = new List<Dictionary<string, object?>>()
            };
            
            foreach (var attribute in attributes.Attributes)
            {
                var row = new Dictionary<string, object?>
                {
                    ["id"] = attribute.Id,
                    ["name"] = attribute.Name,
                    ["source"] = attribute.Source,
                    ["documentation"] = attribute.Documentation
                };
                excelData.Rows.Add(row);
            }
            
            return excelData;
        }

        /// <summary>
        /// Excel 转 CraftingPieces
        /// </summary>
        private static CraftingPiecesDO ConvertExcelToCraftingPieces(ExcelData excelData)
        {
            var craftingPieces = new CraftingPiecesDO();
            
            // 处理 type 字段（如果有）
            if (excelData.Rows.Any() && excelData.Rows[0].TryGetValue("type", out var type))
            {
                craftingPieces.Type = type?.ToString() ?? "crafting_piece";
            }
            
            // 处理 crafting_pieces
            var pieceRows = excelData.Rows.Where(r => r.ContainsKey("id") || r.ContainsKey("name")).ToList();
            if (pieceRows.Any())
            {
                craftingPieces.HasEmptyCraftingPiecesContainer = true;
                foreach (var row in pieceRows)
                {
                    var piece = new CraftingPieceDO();
                    
                    if (row.TryGetValue("id", out var id))
                        piece.Id = id?.ToString();
                    
                    if (row.TryGetValue("name", out var name))
                        piece.Name = name?.ToString();
                    
                    if (row.TryGetValue("piece_type", out var pieceType))
                        piece.PieceType = pieceType?.ToString();
                    
                    if (row.TryGetValue("tier", out var tier))
                        piece.Tier = tier?.ToString();
                    
                    craftingPieces.CraftingPiecesContainer.Pieces.Add(piece);
                }
            }
            
            return craftingPieces;
        }

        /// <summary>
        /// CraftingPieces 转 Excel
        /// </summary>
        private static ExcelData ConvertCraftingPiecesToExcel(CraftingPiecesDO craftingPieces)
        {
            var excelData = new ExcelData
            {
                Headers = new List<string>(),
                Rows = new List<Dictionary<string, object?>>()
            };
            
            // 添加 type 列
            if (!string.IsNullOrEmpty(craftingPieces.Type))
            {
                excelData.Headers.Add("type");
                var typeRow = new Dictionary<string, object?>
                {
                    ["type"] = craftingPieces.Type
                };
                excelData.Rows.Add(typeRow);
            }
            
            // 添加 crafting_pieces
            if (craftingPieces.CraftingPiecesContainer.Pieces.Any())
            {
                if (!excelData.Headers.Contains("id")) excelData.Headers.Add("id");
                if (!excelData.Headers.Contains("name")) excelData.Headers.Add("name");
                if (!excelData.Headers.Contains("piece_type")) excelData.Headers.Add("piece_type");
                if (!excelData.Headers.Contains("tier")) excelData.Headers.Add("tier");
                
                foreach (var piece in craftingPieces.CraftingPiecesContainer.Pieces)
                {
                    var row = new Dictionary<string, object?>
                    {
                        ["id"] = piece.Id,
                        ["name"] = piece.Name,
                        ["piece_type"] = piece.PieceType,
                        ["tier"] = piece.Tier
                    };
                    excelData.Rows.Add(row);
                }
            }
            
            return excelData;
        }

        /// <summary>
        /// Excel 转 ItemModifiers
        /// </summary>
        private static ItemModifiersDO ConvertExcelToItemModifiers(ExcelData excelData)
        {
            var itemModifiers = new ItemModifiersDO();
            
            foreach (var row in excelData.Rows)
            {
                var modifier = new ItemModifierDO();
                
                if (row.TryGetValue("modifier_group", out var modifierGroup))
                    modifier.ModifierGroup = modifierGroup?.ToString();
                
                if (row.TryGetValue("id", out var id))
                    modifier.Id = id?.ToString();
                
                if (row.TryGetValue("name", out var name))
                    modifier.Name = name?.ToString();
                
                if (row.TryGetValue("loot_drop_score", out var lootDropScore))
                    modifier.LootDropScoreString = lootDropScore?.ToString();
                
                if (row.TryGetValue("production_drop_score", out var productionDropScore))
                    modifier.ProductionDropScoreString = productionDropScore?.ToString();
                
                itemModifiers.ItemModifierList.Add(modifier);
            }
            
            return itemModifiers;
        }

        /// <summary>
        /// ItemModifiers 转 Excel
        /// </summary>
        private static ExcelData ConvertItemModifiersToExcel(ItemModifiersDO itemModifiers)
        {
            var excelData = new ExcelData
            {
                Headers = new List<string> { "modifier_group", "id", "name", "loot_drop_score", "production_drop_score" },
                Rows = new List<Dictionary<string, object?>>()
            };
            
            foreach (var modifier in itemModifiers.ItemModifierList)
            {
                var row = new Dictionary<string, object?>
                {
                    ["modifier_group"] = modifier.ModifierGroup,
                    ["id"] = modifier.Id,
                    ["name"] = modifier.Name,
                    ["loot_drop_score"] = modifier.LootDropScoreString,
                    ["production_drop_score"] = modifier.ProductionDropScoreString
                };
                excelData.Rows.Add(row);
            }
            
            return excelData;
        }

        
        /// <summary>
        /// Excel 转 Skills
        /// </summary>
        private static SkillsDO ConvertExcelToSkills(ExcelData excelData)
        {
            var skills = new SkillsDO();
            
            foreach (var row in excelData.Rows)
            {
                var skill = new SkillDataDO();
                
                if (row.TryGetValue("id", out var id))
                    skill.Id = id?.ToString() ?? string.Empty;
                
                if (row.TryGetValue("name", out var name))
                    skill.Name = name?.ToString() ?? string.Empty;
                
                if (row.TryGetValue("documentation", out var documentation))
                    skill.Documentation = documentation?.ToString();
                
                skills.SkillDataList.Add(skill);
            }
            
            return skills;
        }

        /// <summary>
        /// Skills 转 Excel
        /// </summary>
        private static ExcelData ConvertSkillsToExcel(SkillsDO skills)
        {
            var excelData = new ExcelData
            {
                Headers = new List<string> { "id", "name", "documentation" },
                Rows = new List<Dictionary<string, object?>>()
            };
            
            foreach (var skill in skills.SkillDataList)
            {
                var row = new Dictionary<string, object?>
                {
                    ["id"] = skill.Id,
                    ["name"] = skill.Name,
                    ["documentation"] = skill.Documentation
                };
                excelData.Rows.Add(row);
            }
            
            return excelData;
        }

        /// <summary>
        /// Excel 转 SoundFiles
        /// </summary>
        private static SoundFilesDO ConvertExcelToSoundFiles(ExcelData excelData)
        {
            var soundFiles = new SoundFilesDO();
            
            // 处理 type 字段
            if (excelData.Rows.Any() && excelData.Rows[0].TryGetValue("type", out var type))
            {
                soundFiles.Type = type?.ToString() ?? "sound";
            }
            
            // 处理 bank_files
            var bankFileRows = excelData.Rows.Where(r => r.ContainsKey("name") && r.ContainsKey("decompress_samples")).ToList();
            if (bankFileRows.Any())
            {
                soundFiles.HasBankFiles = true;
                foreach (var row in bankFileRows)
                {
                    var soundFile = new SoundFileDO();
                    if (row.TryGetValue("name", out var name))
                        soundFile.Name = name?.ToString();
                    if (row.TryGetValue("decompress_samples", out var decompressSamples))
                        soundFile.DecompressSamples = decompressSamples?.ToString();
                    soundFiles.BankFiles.File.Add(soundFile);
                }
            }
            
            return soundFiles;
        }

        /// <summary>
        /// SoundFiles 转 Excel
        /// </summary>
        private static ExcelData ConvertSoundFilesToExcel(SoundFilesDO soundFiles)
        {
            var excelData = new ExcelData
            {
                Headers = new List<string>(),
                Rows = new List<Dictionary<string, object?>>()
            };
            
            // 添加 type 列
            if (!string.IsNullOrEmpty(soundFiles.Type))
            {
                excelData.Headers.Add("type");
                var typeRow = new Dictionary<string, object?>
                {
                    ["type"] = soundFiles.Type
                };
                excelData.Rows.Add(typeRow);
            }
            
            // 添加 bank_files
            if (soundFiles.HasBankFiles && soundFiles.BankFiles.File.Any())
            {
                if (!excelData.Headers.Contains("name")) excelData.Headers.Add("name");
                if (!excelData.Headers.Contains("decompress_samples")) excelData.Headers.Add("decompress_samples");
                
                foreach (var soundFile in soundFiles.BankFiles.File)
                {
                    var row = new Dictionary<string, object?>
                    {
                        ["name"] = soundFile.Name,
                        ["decompress_samples"] = soundFile.DecompressSamples
                    };
                    excelData.Rows.Add(row);
                }
            }
            
            return excelData;
        }

        /// <summary>
        /// 其他新模型的转换器方法可以在此添加
        /// 这些方法将根据需要逐步实现
        /// </summary>
        private static GogAchievementDataDO ConvertExcelToGogAchievementData(ExcelData excelData) => 
            (GogAchievementDataDO)ConvertExcelToModelGeneric(excelData, typeof(GogAchievementDataDO));
        
        private static ExcelData ConvertGogAchievementDataToExcel(GogAchievementDataDO data) => 
            ConvertModelToExcelGeneric(data);
        
        private static ClothMaterialsDO ConvertExcelToClothMaterials(ExcelData excelData) => 
            (ClothMaterialsDO)ConvertExcelToModelGeneric(excelData, typeof(ClothMaterialsDO));
        
        private static ExcelData ConvertClothMaterialsToExcel(ClothMaterialsDO data) => 
            ConvertModelToExcelGeneric(data);
        
        private static DecalSetsDO ConvertExcelToDecalSets(ExcelData excelData) => 
            (DecalSetsDO)ConvertExcelToModelGeneric(excelData, typeof(DecalSetsDO));
        
        private static ExcelData ConvertDecalSetsToExcel(DecalSetsDO data) => 
            ConvertModelToExcelGeneric(data);
        
        private static GlobalStringsDO ConvertExcelToGlobalStrings(ExcelData excelData) => 
            (GlobalStringsDO)ConvertExcelToModelGeneric(excelData, typeof(GlobalStringsDO));
        
        private static ExcelData ConvertGlobalStringsToExcel(GlobalStringsDO data) => 
            ConvertModelToExcelGeneric(data);
        
        private static MusicDO ConvertExcelToMusic(ExcelData excelData) => 
            (MusicDO)ConvertExcelToModelGeneric(excelData, typeof(MusicDO));
        
        private static ExcelData ConvertMusicToExcel(MusicDO data) => 
            ConvertModelToExcelGeneric(data);

        #endregion

        /// <summary>
        /// 通用 Excel 转 模型转换
        /// </summary>
        private static object ConvertExcelToModelGeneric(ExcelData excelData, Type modelType)
        {
            // 使用反射创建实例
            var model = Activator.CreateInstance(modelType);
            if (model == null)
                throw new InvalidOperationException($"无法创建类型 {modelType.Name} 的实例");

            // 获取所有属性
            var properties = modelType.GetProperties()
                .Where(p => p.CanWrite && p.DeclaringType == modelType)
                .ToList();

            foreach (var property in properties)
            {
                // 检查 Excel 中是否有对应的列
                var columnName = property.Name;
                if (excelData.Headers.Contains(columnName))
                {
                    var values = excelData.Rows
                        .Where(r => r.ContainsKey(columnName))
                        .Select(r => r[columnName])
                        .Where(v => v != null)
                        .ToList();

                    if (values.Any())
                    {
                        // 根据属性类型设置值
                        if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(model, values.First()?.ToString());
                        }
                        else if (property.PropertyType == typeof(int))
                        {
                            if (int.TryParse(values.First()?.ToString(), out var intValue))
                                property.SetValue(model, intValue);
                        }
                        else if (property.PropertyType == typeof(float))
                        {
                            if (float.TryParse(values.First()?.ToString(), out var floatValue))
                                property.SetValue(model, floatValue);
                        }
                        else if (property.PropertyType == typeof(bool))
                        {
                            if (bool.TryParse(values.First()?.ToString(), out var boolValue))
                                property.SetValue(model, boolValue);
                        }
                        else if (property.PropertyType == typeof(List<string>))
                        {
                            var list = values.Select(v => v?.ToString()).Where(s => !string.IsNullOrEmpty(s)).ToList();
                            property.SetValue(model, list);
                        }
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// 通用 模型 转 Excel 转换
        /// </summary>
        private static ExcelData ConvertModelToExcelGeneric(object model)
        {
            var excelData = new ExcelData
            {
                Headers = new List<string>(),
                Rows = new List<Dictionary<string, object?>>()
            };

            var modelType = model.GetType();
            var properties = modelType.GetProperties()
                .Where(p => p.CanRead && p.DeclaringType == modelType)
                .ToList();

            // 添加表头
            foreach (var property in properties)
            {
                excelData.Headers.Add(property.Name);
            }

            // 创建一行数据
            var row = new Dictionary<string, object?>();
            foreach (var property in properties)
            {
                var value = property.GetValue(model);
                row[property.Name] = value;
            }
            excelData.Rows.Add(row);

            return excelData;
        }

        /// <summary>
        /// 将模型类型（如 action_types）转换为DO类型名称（如 ActionTypesDO）
        /// </summary>
        private static string ConvertModelTypeToDoTypeName(string modelType)
        {
            if (string.IsNullOrEmpty(modelType))
                return string.Empty;

            // 如果已经是DO类型名称（以"DO"结尾），直接返回
            if (modelType.EndsWith("DO", StringComparison.OrdinalIgnoreCase))
            {
                return modelType;
            }

            // 已知的映射关系
            var mappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["action_types"] = nameof(ActionTypesDO),
                ["combat_parameters"] = nameof(CombatParametersDO),
                ["map_icons"] = nameof(MapIconsDO),
                ["attributes"] = nameof(AttributesDO),
                ["ArrayOfAttributeData"] = nameof(AttributesDO),
                ["crafting_pieces"] = nameof(CraftingPiecesDO),
                ["item_modifiers"] = nameof(ItemModifiersDO),
                ["skills"] = nameof(SkillsDO),
                ["gog_achievement_data"] = nameof(GogAchievementDataDO),
                ["cloth_materials"] = nameof(ClothMaterialsDO),
                ["decal_sets"] = nameof(DecalSetsDO),
                ["global_strings"] = nameof(GlobalStringsDO),
                ["music"] = nameof(MusicDO),
                ["sound_files"] = nameof(SoundFilesDO)
            };

            if (mappings.TryGetValue(modelType, out var doTypeName))
            {
                return doTypeName;
            }

            // 如果没有找到映射，尝试使用命名约定转换
            // 例如：action_types -> ActionTypesDO
            var pascalCase = System.Text.RegularExpressions.Regex.Replace(modelType, @"_(\w)", m => m.Groups[1].Value.ToUpper());
            pascalCase = char.ToUpper(pascalCase[0]) + pascalCase.Substring(1);
            return pascalCase + "DO";
        }
    }
}