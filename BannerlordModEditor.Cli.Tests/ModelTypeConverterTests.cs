using BannerlordModEditor.Cli.Services;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Cli.Tests
{
    /// <summary>
    /// ModelTypeConverter 的单元测试
    /// </summary>
    public class ModelTypeConverterTests
    {
        [Fact]
        public void ConvertExcelToModel_WithActionTypesData_ReturnsValidActionTypesDO()
        {
            // Arrange
            var excelData = new ExcelData
            {
                Headers = new List<string> { "name", "type", "usage_direction", "action_stage" },
                Rows = new List<Dictionary<string, object?>>
                {
                    new Dictionary<string, object?>
                    {
                        ["name"] = "test_action_1",
                        ["type"] = "test_type_1",
                        ["usage_direction"] = "test_direction_1",
                        ["action_stage"] = "test_stage_1"
                    },
                    new Dictionary<string, object?>
                    {
                        ["name"] = "test_action_2",
                        ["type"] = "test_type_2"
                        // usage_direction 和 action_stage 为空
                    }
                }
            };

            // Act
            var result = ModelTypeConverter.ConvertExcelToModel(excelData, typeof(ActionTypesDO));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionTypesDO>(result);
            
            var actionTypes = (ActionTypesDO)result;
            Assert.Equal(2, actionTypes.Actions.Count);
            
            var firstAction = actionTypes.Actions[0];
            Assert.Equal("test_action_1", firstAction.Name);
            Assert.Equal("test_type_1", firstAction.Type);
            Assert.Equal("test_direction_1", firstAction.UsageDirection);
            Assert.Equal("test_stage_1", firstAction.ActionStage);
            
            var secondAction = actionTypes.Actions[1];
            Assert.Equal("test_action_2", secondAction.Name);
            Assert.Equal("test_type_2", secondAction.Type);
            Assert.Null(secondAction.UsageDirection);
            Assert.Null(secondAction.ActionStage);
        }

        [Fact]
        public void ConvertActionTypesToExcel_WithValidActionTypesDO_ReturnsValidExcelData()
        {
            // Arrange
            var actionTypes = new ActionTypesDO
            {
                Actions = new List<ActionTypeDO>
                {
                    new ActionTypeDO
                    {
                        Name = "test_action_1",
                        Type = "test_type_1",
                        UsageDirection = "test_direction_1",
                        ActionStage = "test_stage_1"
                    },
                    new ActionTypeDO
                    {
                        Name = "test_action_2",
                        Type = "test_type_2"
                        // usage_direction 和 action_stage 为空
                    }
                }
            };

            // Act
            var result = ModelTypeConverter.ConvertModelToExcel(actionTypes, "ActionTypesDO");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Headers.Count);
            Assert.Contains("name", result.Headers);
            Assert.Contains("type", result.Headers);
            Assert.Contains("usage_direction", result.Headers);
            Assert.Contains("action_stage", result.Headers);
            
            Assert.Equal(2, result.Rows.Count);
            
            var firstRow = result.Rows[0];
            Assert.Equal("test_action_1", firstRow["name"]);
            Assert.Equal("test_type_1", firstRow["type"]);
            Assert.Equal("test_direction_1", firstRow["usage_direction"]);
            Assert.Equal("test_stage_1", firstRow["action_stage"]);
            
            var secondRow = result.Rows[1];
            Assert.Equal("test_action_2", secondRow["name"]);
            Assert.Equal("test_type_2", secondRow["type"]);
            Assert.Null(secondRow["usage_direction"]);
            Assert.Null(secondRow["action_stage"]);
        }

        [Fact]
        public void ConvertExcelToModel_WithCombatParametersData_ReturnsValidCombatParametersDO()
        {
            // Arrange
            var excelData = new ExcelData
            {
                Headers = new List<string> { "type", "def_name", "def_val" },
                Rows = new List<Dictionary<string, object?>>
                {
                    new Dictionary<string, object?>
                    {
                        ["type"] = "combat_parameters"
                    },
                    new Dictionary<string, object?>
                    {
                        ["def_name"] = "test_def_1",
                        ["def_val"] = "test_val_1"
                    },
                    new Dictionary<string, object?>
                    {
                        ["def_name"] = "test_def_2",
                        ["def_val"] = "test_val_2"
                    }
                }
            };

            // Act
            var result = ModelTypeConverter.ConvertExcelToModel(excelData, typeof(CombatParametersDO));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CombatParametersDO>(result);
            
            var combatParams = (CombatParametersDO)result;
            Assert.Equal("combat_parameters", combatParams.Type);
            Assert.True(combatParams.HasDefinitions);
            Assert.Equal(2, combatParams.Definitions.Defs.Count);
            
            var firstDef = combatParams.Definitions.Defs[0];
            Assert.Equal("test_def_1", firstDef.Name);
            Assert.Equal("test_val_1", firstDef.Value);
            
            var secondDef = combatParams.Definitions.Defs[1];
            Assert.Equal("test_def_2", secondDef.Name);
            Assert.Equal("test_val_2", secondDef.Value);
        }

        [Fact]
        public void ConvertCombatParametersToExcel_WithValidCombatParametersDO_ReturnsValidExcelData()
        {
            // Arrange
            var combatParams = new CombatParametersDO
            {
                Type = "combat_parameters",
                HasDefinitions = true,
                Definitions = new DefinitionsDO
                {
                    Defs = new List<DefDO>
                    {
                        new DefDO { Name = "test_def_1", Value = "test_val_1" },
                        new DefDO { Name = "test_def_2", Value = "test_val_2" }
                    }
                }
            };

            // Act
            var result = ModelTypeConverter.ConvertModelToExcel(combatParams, "CombatParametersDO");

            // Assert
            Assert.NotNull(result);
            Assert.Contains("type", result.Headers);
            Assert.Contains("def_name", result.Headers);
            Assert.Contains("def_val", result.Headers);
            
            // 查找 type 行
            var typeRow = result.Rows.FirstOrDefault(r => r.ContainsKey("type") && r["type"]?.ToString() == "combat_parameters");
            Assert.NotNull(typeRow);
            
            // 查找 definition 行
            var defRows = result.Rows.Where(r => r.ContainsKey("def_name")).ToList();
            Assert.Equal(2, defRows.Count);
            
            Assert.Equal("test_def_1", defRows[0]["def_name"]);
            Assert.Equal("test_val_1", defRows[0]["def_val"]);
            Assert.Equal("test_def_2", defRows[1]["def_name"]);
            Assert.Equal("test_val_2", defRows[1]["def_val"]);
        }

        [Fact]
        public void ConvertExcelToModel_WithMapIconsData_ReturnsValidMapIconsDO()
        {
            // Arrange
            var excelData = new ExcelData
            {
                Headers = new List<string> { "id", "id_str", "flags", "mesh_name", "mesh_scale", "sound_no", "offset_pos" },
                Rows = new List<Dictionary<string, object?>>
                {
                    new Dictionary<string, object?>
                    {
                        ["id"] = "map_icon_test_1",
                        ["id_str"] = "test_1",
                        ["flags"] = "0x0",
                        ["mesh_name"] = "test_mesh",
                        ["mesh_scale"] = "0.15",
                        ["sound_no"] = "17",
                        ["offset_pos"] = "0.15, 0.17, 0.0"
                    }
                }
            };

            // Act
            var result = ModelTypeConverter.ConvertExcelToModel(excelData, typeof(MapIconsDO));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MapIconsDO>(result);
            
            var mapIcons = (MapIconsDO)result;
            Assert.Single(mapIcons.MapIconsContainer.MapIconList);
            
            var mapIcon = mapIcons.MapIconsContainer.MapIconList[0];
            Assert.Equal("map_icon_test_1", mapIcon.Id);
            Assert.Equal("test_1", mapIcon.IdStr);
            Assert.Equal("0x0", mapIcon.Flags);
            Assert.Equal("test_mesh", mapIcon.MeshName);
            Assert.Equal("0.15", mapIcon.MeshScale);
            Assert.Equal("17", mapIcon.SoundNo);
            Assert.Equal("0.15, 0.17, 0.0", mapIcon.OffsetPos);
        }

        [Fact]
        public void ConvertMapIconsToExcel_WithValidMapIconsDO_ReturnsValidExcelData()
        {
            // Arrange
            var mapIcons = new MapIconsDO
            {
                MapIconsContainer = new MapIconsContainerDO
                {
                    MapIconList = new List<MapIconDO>
                {
                    new MapIconDO
                    {
                        Id = "map_icon_test_1",
                        IdStr = "test_1",
                        Flags = "0x0",
                        MeshName = "test_mesh",
                        MeshScale = "0.15",
                        SoundNo = "17",
                        OffsetPos = "0.15, 0.17, 0.0"
                    }
                }
                }
            };

            // Act
            var result = ModelTypeConverter.ConvertModelToExcel(mapIcons, "MapIconsDO");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Headers.Count);
            Assert.Contains("id", result.Headers);
            Assert.Contains("id_str", result.Headers);
            Assert.Contains("flags", result.Headers);
            Assert.Contains("mesh_name", result.Headers);
            Assert.Contains("mesh_scale", result.Headers);
            Assert.Contains("sound_no", result.Headers);
            Assert.Contains("offset_pos", result.Headers);
            
            Assert.Single(result.Rows);
            
            var row = result.Rows[0];
            Assert.Equal("map_icon_test_1", row["id"]);
            Assert.Equal("test_1", row["id_str"]);
            Assert.Equal("0x0", row["flags"]);
            Assert.Equal("test_mesh", row["mesh_name"]);
            Assert.Equal("0.15", row["mesh_scale"]);
            Assert.Equal("17", row["sound_no"]);
            Assert.Equal("0.15, 0.17, 0.0", row["offset_pos"]);
        }

        [Fact]
        public void ConvertExcelToModel_WithEmptyData_ReturnsEmptyModel()
        {
            // Arrange
            var excelData = new ExcelData
            {
                Headers = new List<string> { "name", "type" },
                Rows = new List<Dictionary<string, object?>>()
            };

            // Act
            var result = ModelTypeConverter.ConvertExcelToModel(excelData, typeof(ActionTypesDO));

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionTypesDO>(result);
            
            var actionTypes = (ActionTypesDO)result;
            Assert.Empty(actionTypes.Actions);
        }

        [Fact]
        public void ConvertModelToExcel_WithEmptyModel_ReturnsEmptyExcelData()
        {
            // Arrange
            var actionTypes = new ActionTypesDO
            {
                Actions = new List<ActionTypeDO>()
            };

            // Act
            var result = ModelTypeConverter.ConvertModelToExcel(actionTypes, "ActionTypesDO");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Headers.Count); // ActionTypesDO 有 4 个属性
            Assert.Empty(result.Rows);
        }
    }
}