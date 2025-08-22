using System;
using System.Collections.Generic;

namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// 处理命名约定和冲突解决的映射器
    /// </summary>
    public static class NamingConventionMapper
    {
        /// <summary>
        /// 预定义的命名映射，用于解决冲突和特殊情况
        /// </summary>
        private static readonly Dictionary<string, string> SpecialMappings = new()
        {
            // 系统类型冲突解决
            { "action", "ActionType" },
            { "actions", "ActionTypes" },
            { "object", "GameObjectType" },
            { "objects", "GameObjects" },
            
            // 特殊缩写和约定
            { "mp_crafting_pieces", "MpCraftingPieces" },
            { "mpbodypropertytemplates", "MpBodyPropertyTemplates" },
            { "mpclassdivisions", "MpClassDivisions" },
            { "mpcosmetics", "MpCosmetics" },
            
            // 图形和渲染相关
            { "before_transparents_graph", "BeforeTransparentsGraph" },
            { "thumbnail_postfx_graphs", "ThumbnailPostfxGraphs" },
            { "particle_systems2", "ParticleSystems2" },
            { "particle_systems_basic", "ParticleSystemsBasic" },
            { "particle_systems_general", "ParticleSystemsGeneral" },
            { "particle_systems_hardcoded_misc1", "ParticleSystemsHardcodedMisc1" },
            { "particle_systems_hardcoded_misc2", "ParticleSystemsHardcodedMisc2" },
            { "particle_systems_outdoor", "ParticleSystemsOutdoor" },
            
            // 贴花纹理变体
            { "decal_textures_battle", "DecalTexturesBattle" },
            { "decal_textures_multiplayer", "DecalTexturesMultiplayer" },
            { "decal_textures_town", "DecalTexturesTown" },
            { "decal_textures_worldmap", "DecalTexturesWorldmap" },
            
            // 其他特殊情况
            { "looknfeel", "LookAndFeel" },
            { "flora_layer_sets", "FloraLayerSets" },
            { "prebaked_animations", "PrebakedAnimations" },
            { "prerender", "Prerender" },
            
            // 语言相关XML文件映射到LanguageBaseDO
            { "std_functions", "LanguageBase" },
            { "std_taleworlds_core", "LanguageBase" },
            { "std_taleworlds_mountandblade", "LanguageBase" },
            { "std_taleworlds_mountandblade_view", "LanguageBase" },
            { "std_taleworlds_mountandblade_viewmodelcollection", "LanguageBase" },
            { "std_taleworlds_mountandblade_gauntletui", "LanguageBase" },
            { "std_common_strings_xml", "LanguageBase" },
            { "std_global_strings_xml", "LanguageBase" },
            { "std_module_strings_xml", "LanguageBase" },
            { "std_native_strings_xml", "LanguageBase" },
            { "std_multiplayer_strings_xml", "LanguageBase" },
            { "std_crafting_pieces_xml", "LanguageBase" },
            { "std_item_modifiers_xml", "LanguageBase" },
            { "std_mpbadges_xml", "LanguageBase" },
            { "std_mpcharacters_xml", "LanguageBase" },
            { "std_mpclassdivisions_xml", "LanguageBase" },
            { "std_mpitems_xml", "LanguageBase" },
            { "std_photo_mode_strings_xml", "LanguageBase" },
            { "std_siegeengines_xml", "LanguageBase" }
        };

        /// <summary>
        /// C# 保留关键字列表
        /// </summary>
        private static readonly HashSet<string> ReservedKeywords = new(StringComparer.OrdinalIgnoreCase)
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
            "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else",
            "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for",
            "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock",
            "long", "namespace", "new", "null", "object", "operator", "out", "override", "params",
            "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed",
            "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this",
            "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort",
            "using", "virtual", "void", "volatile", "while"
        };

        /// <summary>
        /// 获取映射后的类名，处理特殊情况和冲突
        /// </summary>
        /// <param name="originalName">原始名称</param>
        /// <returns>映射后的类名</returns>
        public static string GetMappedClassName(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return string.Empty;

            var lowerName = originalName.ToLowerInvariant();

            // 检查特殊映射
            if (SpecialMappings.TryGetValue(lowerName, out var specialMapping))
                return specialMapping;

            // 标准 PascalCase 转换
            var pascalCase = ToPascalCase(originalName);

            // 检查是否为保留关键字
            if (ReservedKeywords.Contains(pascalCase))
                return pascalCase + "Type";

            return pascalCase;
        }

        /// <summary>
        /// 检查名称是否需要特殊处理
        /// </summary>
        /// <param name="name">要检查的名称</param>
        /// <returns>如果需要特殊处理则返回 true</returns>
        public static bool RequiresSpecialHandling(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            var lowerName = name.ToLowerInvariant();
            return SpecialMappings.ContainsKey(lowerName) || 
                   ReservedKeywords.Contains(name);
        }

        /// <summary>
        /// 将 snake_case 字符串转换为 PascalCase
        /// </summary>
        /// <param name="snakeCaseString">snake_case 字符串</param>
        /// <returns>PascalCase 字符串</returns>
        private static string ToPascalCase(string snakeCaseString)
        {
            if (string.IsNullOrEmpty(snakeCaseString))
                return string.Empty;

            var parts = snakeCaseString.Split('_', StringSplitOptions.RemoveEmptyEntries);
            var result = string.Empty;

            foreach (var part in parts)
            {
                if (part.Length > 0)
                {
                    result += char.ToUpperInvariant(part[0]) + part[1..].ToLowerInvariant();
                }
            }

            return result;
        }

        /// <summary>
        /// 获取建议的命名空间，基于文件类型
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>建议的命名空间后缀</returns>
        public static string GetSuggestedNamespace(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "Misc";

            var lowerFileName = fileName.ToLowerInvariant();

            return lowerFileName switch
            {
                var name when name.Contains("particle") => "Engine",
                var name when name.Contains("decal") => "Engine", 
                var name when name.Contains("texture") => "Engine",
                var name when name.Contains("graph") => "Engine",
                var name when name.Contains("render") => "Engine",
                var name when name.Contains("mp") || name.Contains("multiplayer") => "Multiplayer",
                var name when name.Contains("credits") => "Configuration",
                var name when name.Contains("flora") => "Map",
                var name when name.Contains("animation") => "Engine",
                _ => "Data"
            };
        }
    }
}