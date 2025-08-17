using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;
using System.Linq;
using BannerlordModEditor.Common.Models;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public static class XmlTestUtils
    {
        public enum ComparisonMode
        {
            Strict,
            Logical,
            Loose
        }

        public class XmlComparisonOptions
        {
            public ComparisonMode Mode { get; set; } = ComparisonMode.Logical;
            public bool IgnoreComments { get; set; } = true;
            public bool IgnoreWhitespace { get; set; } = true;
            public bool IgnoreAttributeOrder { get; set; } = true;
            public bool AllowCaseInsensitiveBooleans { get; set; } = true;
            public bool AllowNumericTolerance { get; set; } = true;
            public double NumericTolerance { get; set; } = 0.0001;
        }

        public static IReadOnlyList<string> CommonBooleanTrueValues = 
            new[] { "true", "True", "TRUE", "1", "yes", "Yes", "YES", "on", "On", "ON" };
        
        public static IReadOnlyList<string> CommonBooleanFalseValues = 
            new[] { "false", "False", "FALSE", "0", "no", "No", "NO", "off", "Off", "OFF" };
        public static string? ReadTestDataOrSkip(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null; // Signal caller to skip
            }
            return File.ReadAllText(filePath);
        }

        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentException("XML cannot be null or empty", nameof(xml));
                
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xml);
            var obj = (T)serializer.Deserialize(reader)!;
            
            // 特殊处理CombatParametersDO来检测是否有definitions元素和空的combat_parameters元素
            if (obj is BannerlordModEditor.Common.Models.DO.CombatParametersDO combatParams)
            {
                var doc = XDocument.Parse(xml);
                combatParams.HasDefinitions = doc.Root?.Element("definitions") != null;
                var combatParamsElement = doc.Root?.Element("combat_parameters");
                combatParams.HasEmptyCombatParameters = combatParamsElement != null && 
                    (combatParamsElement.Elements().Count() == 0 || combatParamsElement.Elements("combat_parameter").Count() == 0);
            }
            
            // 特殊处理ItemHolstersDO来检测是否有空的item_holsters元素
            if (obj is BannerlordModEditor.Common.Models.DO.ItemHolstersDO itemHolsters)
            {
                var doc = XDocument.Parse(xml);
                var itemHolstersElement = doc.Root?.Element("item_holsters");
                itemHolsters.HasEmptyItemHolsters = itemHolstersElement != null && 
                    (itemHolstersElement.Elements().Count() == 0 || itemHolstersElement.Elements("item_holster").Count() == 0);
            }
            
            // 简化实现：移除复杂的Credits重新排序逻辑，直接保持XML序列化的原始顺序
            // 这种简化实现避免了重新排序导致的Text属性交换问题
            
            // 特殊处理LooknfeelDO来检测是否有空的widgets元素
            if (obj is LooknfeelDO looknfeel)
            {
                var doc = XDocument.Parse(xml);
                var widgetsElement = doc.Root?.Element("widgets");
                looknfeel.HasEmptyWidgets = widgetsElement != null && 
                    (widgetsElement.Elements().Count() == 0 || widgetsElement.Elements("widget").Count() == 0);
                
                // 处理每个widget的空元素状态
                if (looknfeel.Widgets != null && looknfeel.Widgets.WidgetList != null)
                {
                    for (int i = 0; i < looknfeel.Widgets.WidgetList.Count; i++)
                    {
                        var widget = looknfeel.Widgets.WidgetList[i];
                        var widgetElement = widgetsElement?.Elements("widget").ElementAt(i);
                        
                        if (widgetElement != null)
                        {
                            // 检查meshes元素
                            var meshesElement = widgetElement.Element("meshes");
                            widget.HasEmptyMeshes = meshesElement != null;
                            
                            // 设置具体mesh类型的空元素标记
                            if (meshesElement != null && widget.Meshes != null)
                            {
                                widget.Meshes.HasEmptyBackgroundMeshes = meshesElement.Element("background_mesh") != null;
                                widget.Meshes.HasEmptyButtonMeshes = meshesElement.Element("button_mesh") != null;
                                widget.Meshes.HasEmptyButtonPressedMeshes = meshesElement.Element("button_pressed_mesh") != null;
                                widget.Meshes.HasEmptyHighlightMeshes = meshesElement.Element("highlight_mesh") != null;
                                widget.Meshes.HasEmptyCursorMeshes = meshesElement.Element("cursor_mesh") != null;
                                widget.Meshes.HasEmptyLeftBorderMeshes = meshesElement.Element("left_border_mesh") != null;
                                widget.Meshes.HasEmptyRightBorderMeshes = meshesElement.Element("right_border_mesh") != null;
                            }
                            
                            // 检查sub_widgets元素 - 支持多个sub_widgets元素
                            var subWidgetsElements = widgetElement.Elements("sub_widgets").ToList();
                            widget.HasEmptySubWidgetsList = subWidgetsElements.Count > 0;
                            
                            // 处理sub_widget的空元素状态 - 遍历所有sub_widgets容器
                            if (widget.SubWidgetsList != null)
                            {
                                for (int k = 0; k < widget.SubWidgetsList.Count; k++)
                                {
                                    var subWidgetsContainer = widget.SubWidgetsList[k];
                                    var subWidgetsElement = subWidgetsElements.ElementAtOrDefault(k);
                                    
                                    if (subWidgetsContainer.SubWidgetList != null)
                                    {
                                        for (int j = 0; j < subWidgetsContainer.SubWidgetList.Count; j++)
                                        {
                                            var subWidget = subWidgetsContainer.SubWidgetList[j];
                                            var subWidgetElement = subWidgetsElement?.Elements("sub_widget").ElementAt(j);
                                    
                                            if (subWidgetElement != null)
                                            {
                                                // 检查sub_widget的meshes元素
                                                var subMeshesElement = subWidgetElement.Element("meshes");
                                                subWidget.HasEmptyMeshes = subMeshesElement != null;
                                                
                                                // 设置sub_widget中具体mesh类型的空元素标记
                                                if (subMeshesElement != null && subWidget.Meshes != null)
                                                {
                                                    subWidget.Meshes.HasEmptyBackgroundMeshes = subMeshesElement.Element("background_mesh") != null;
                                                    subWidget.Meshes.HasEmptyButtonMeshes = subMeshesElement.Element("button_mesh") != null;
                                                    subWidget.Meshes.HasEmptyButtonPressedMeshes = subMeshesElement.Element("button_pressed_mesh") != null;
                                                    subWidget.Meshes.HasEmptyHighlightMeshes = subMeshesElement.Element("highlight_mesh") != null;
                                                    subWidget.Meshes.HasEmptyCursorMeshes = subMeshesElement.Element("cursor_mesh") != null;
                                                    subWidget.Meshes.HasEmptyLeftBorderMeshes = subMeshesElement.Element("left_border_mesh") != null;
                                                    subWidget.Meshes.HasEmptyRightBorderMeshes = subMeshesElement.Element("right_border_mesh") != null;
                                                }
                                                
                                                // 检查sub_widget的sub_widgets元素 - 也支持多个
                                                var subSubWidgetsElements = subWidgetElement.Elements("sub_widgets").ToList();
                                                subWidget.HasEmptySubWidgetsList = subSubWidgetsElements.Count > 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            // 简化实现：为LooknfeelDO添加精确的XML结构保持逻辑
            // 这解决了序列化时节点数量差异和属性错位的问题
            if (obj is LooknfeelDO looknfeelObj)
            {
                FixLooknfeelNameAttributes(looknfeelObj, xml);
            }
            
            // 特殊处理ActionTypesDO来检测动作列表
            if (obj is ActionTypesDO actionTypes)
            {
                var doc = XDocument.Parse(xml);
                // 检测是否有空的action_types元素（实际上不应该存在，因为这是根元素）
                var actionTypesElement = doc.Root;
                actionTypes.HasEmptyActions = actionTypesElement != null && 
                    (actionTypesElement.Elements().Count() == 0 || actionTypesElement.Elements("action").Count() == 0);
            }
            
            // 特殊处理BannerIconsDO来检测是否有BannerIconData元素
            if (obj is BannerlordModEditor.Common.Models.DO.BannerIconsDO bannerIcons)
            {
                var doc = XDocument.Parse(xml);
                bannerIcons.HasBannerIconData = doc.Root?.Element("BannerIconData") != null;
                
                // 处理BannerIconData的BannerColors标记
                if (bannerIcons.BannerIconData != null)
                {
                    bannerIcons.BannerIconData.HasBannerColors = doc.Root?
                        .Element("BannerIconData")?
                        .Element("BannerColors") != null;
                }
            }
            
            // 特殊处理MpcosmeticsDO来检测是否有空的Cosmetics元素
            if (obj is MpcosmeticsDO mpcosmetics)
            {
                var doc = XDocument.Parse(xml);
                var cosmeticsElement = doc.Root?.Element("Cosmetic");
                mpcosmetics.HasEmptyCosmetics = cosmeticsElement != null && 
                    (cosmeticsElement.Elements().Count() == 0 || cosmeticsElement.Elements("Cosmetic").Count() == 0);
            }
            
            // 特殊处理AttributesDO来检测是否有空的AttributeData元素
            if (obj is AttributesDO attributes)
            {
                var doc = XDocument.Parse(xml);
                var attributeDataElement = doc.Root?.Element("AttributeData");
                attributes.HasEmptyAttributes = attributeDataElement != null && 
                    (attributeDataElement.Elements().Count() == 0 || attributeDataElement.Elements("AttributeData").Count() == 0);
            }
            
            // 特殊处理ParticleSystemsDO来检测和保持复杂的XML结构
            if (obj is ParticleSystemsDO particleSystems)
            {
                var doc = XDocument.Parse(xml);
                
                // 处理每个effect的复杂结构
                if (particleSystems.Effects != null)
                {
                    for (int i = 0; i < particleSystems.Effects.Count; i++)
                    {
                        var effect = particleSystems.Effects[i];
                        var effectElement = doc.Root?.Elements("effect").ElementAt(i);
                        
                        if (effectElement != null && effect.Emitters != null)
                        {
                            // 处理每个emitter的复杂结构
                            for (int j = 0; j < effect.Emitters.EmitterList.Count; j++)
                            {
                                var emitter = effect.Emitters.EmitterList[j];
                                var emitterElement = effectElement.Element("emitters")?.Elements("emitter").ElementAt(j);
                                
                                if (emitterElement != null)
                                {
                                    // 检测空的children元素
                                    emitter.HasEmptyChildren = emitterElement.Element("children") != null;
                                    
                                    // 检测空的flags元素
                                    emitter.HasEmptyFlags = emitterElement.Element("flags") != null;
                                    
                                    // 检测空的parameters元素
                                    emitter.HasEmptyParameters = emitterElement.Element("parameters") != null;
                                    
                                    // 处理parameters中的decal_materials元素
                                    if (emitter.Parameters != null)
                                    {
                                        var parametersElement = emitterElement.Element("parameters");
                                        emitter.Parameters.HasDecalMaterials = parametersElement?.Element("decal_materials") != null;
                                        
                                        // 检测是否有空的parameters元素（即没有parameter子元素但有decal_materials）
                                        emitter.Parameters.HasEmptyParameters = parametersElement != null && 
                                            (parametersElement.Elements("parameter").Count() == 0) && 
                                            (parametersElement.Element("decal_materials") != null);
                                    }
                                    
                                    // 处理children中的空元素状态
                                    if (emitter.Children != null)
                                    {
                                        var childrenElement = emitterElement.Element("children");
                                        emitter.Children.HasEmptyEmitters = childrenElement != null && 
                                            (childrenElement.Elements("emitter").Count() == 0);
                                    }
                                    
                                    // 处理flags中的空元素状态
                                    if (emitter.Flags != null)
                                    {
                                        var flagsElement = emitterElement.Element("flags");
                                        emitter.Flags.HasEmptyFlags = flagsElement != null && 
                                            (flagsElement.Elements("flag").Count() == 0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            return obj;
        }