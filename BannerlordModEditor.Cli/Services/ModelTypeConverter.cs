using System.Reflection;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Cli.Services
{
    /// <summary>
    /// 专门用于处理特定模型类型的转换器
    /// </summary>
    public class ModelTypeConverter
    {
        /// <summary>
        /// 将 Excel 数据转换为指定的 XML 模型对象
        /// </summary>
        public static object ConvertExcelToModel(ExcelData excelData, Type modelType)
        {
            var modelTypeName = modelType.Name;
            
            // 根据模型类型调用相应的转换方法
            switch (modelTypeName)
            {
                case nameof(ActionTypesDO):
                    return ConvertExcelToActionTypes(excelData);
                case nameof(CombatParametersDO):
                    return ConvertExcelToCombatParameters(excelData);
                case nameof(MapIconsDO):
                    return ConvertExcelToMapIcons(excelData);
                default:
                    // 使用反射进行通用转换
                    return ConvertExcelToModelGeneric(excelData, modelType);
            }
        }

        /// <summary>
        /// 将 XML 模型对象转换为 Excel 数据
        /// </summary>
        public static ExcelData ConvertModelToExcel(object modelObject, string modelType)
        {
            switch (modelType)
            {
                case nameof(ActionTypesDO):
                    return ConvertActionTypesToExcel((ActionTypesDO)modelObject);
                case nameof(CombatParametersDO):
                    return ConvertCombatParametersToExcel((CombatParametersDO)modelObject);
                case nameof(MapIconsDO):
                    return ConvertMapIconsToExcel((MapIconsDO)modelObject);
                default:
                    // 使用反射进行通用转换
                    return ConvertModelToExcelGeneric(modelObject);
            }
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
            var excelData = new ExcelData
            {
                Headers = new List<string> { "name", "type", "usage_direction", "action_stage" },
                Rows = new List<Dictionary<string, object?>>()
            };
            
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
                        else if (property.PropertyType == typeof(List<string>))
                        {
                            var list = values.Select(v => v?.ToString()).Where(s => !string.IsNullOrEmpty(s)).ToList();
                            property.SetValue(model, list);
                        }
                        // 可以添加更多类型支持
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
    }
}