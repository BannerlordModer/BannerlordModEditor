using System.Xml.Linq;
using System.Xml.XPath;
using BannerlordModEditor.Common.Services.HybridXml.Dto;

namespace BannerlordModEditor.Common.Services.HybridXml.Mappers
{
    /// <summary>
    /// Looknfeel XML映射器
    /// 负责LooknfeelEditDto与XElement之间的转换
    /// </summary>
    public class LooknfeelMapper : IXElementToDtoMapper<LooknfeelEditDto>
    {
        /// <summary>
        /// 将XElement转换为LooknfeelEditDto
        /// </summary>
        /// <param name="element">源XElement</param>
        /// <returns>转换后的LooknfeelEditDto</returns>
        public LooknfeelEditDto FromXElement(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var dto = new LooknfeelEditDto
            {
                Type = GetAttributeValue(element, "type"),
                VirtualResolution = GetAttributeValue(element, "virtual_resolution")
            };

            // 处理widgets容器
            var widgetsElement = element.Element("widgets");
            if (widgetsElement != null)
            {
                dto.Widgets = new WidgetsEditContainer
                {
                    WidgetList = widgetsElement.Elements("widget")
                        .Select(WidgetFromXElement)
                        .ToList()
                };
            }

            return dto;
        }

        /// <summary>
        /// 将LooknfeelEditDto转换为XElement
        /// </summary>
        /// <param name="dto">源LooknfeelEditDto</param>
        /// <returns>转换后的XElement</returns>
        public XElement ToXElement(LooknfeelEditDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var element = new XElement("base");

            // 设置属性
            if (!string.IsNullOrEmpty(dto.Type))
            {
                element.SetAttributeValue("type", dto.Type);
            }

            if (!string.IsNullOrEmpty(dto.VirtualResolution))
            {
                element.SetAttributeValue("virtual_resolution", dto.VirtualResolution);
            }

            // 添加widgets容器
            if (dto.Widgets != null)
            {
                var widgetsElement = new XElement("widgets");
                
                if (dto.Widgets.WidgetList != null)
                {
                    foreach (var widget in dto.Widgets.WidgetList)
                    {
                        widgetsElement.Add(WidgetToXElement(widget));
                    }
                }

                element.Add(widgetsElement);
            }

            return element;
        }

        /// <summary>
        /// 生成原始DTO和修改后DTO之间的差异补丁
        /// </summary>
        /// <param name="original">原始DTO</param>
        /// <param name="modified">修改后的DTO</param>
        /// <returns>差异补丁</returns>
        public XmlPatch GeneratePatch(LooknfeelEditDto original, LooknfeelEditDto modified)
        {
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            if (modified == null)
            {
                throw new ArgumentNullException(nameof(modified));
            }

            var patch = new XmlPatch();

            // 比较根属性
            GenerateAttributePatch(original.Type, modified.Type, "/base", "type", patch);
            GenerateAttributePatch(original.VirtualResolution, modified.VirtualResolution, "/base", "virtual_resolution", patch);

            // 比较widgets容器
            if (original.Widgets != null && modified.Widgets != null)
            {
                GenerateWidgetsPatch(original.Widgets, modified.Widgets, "/base/widgets", patch);
            }

            return patch;
        }

        /// <summary>
        /// 验证DTO的结构完整性
        /// </summary>
        /// <param name="dto">要验证的DTO</param>
        /// <returns>验证结果</returns>
        public ValidationResult Validate(LooknfeelEditDto dto)
        {
            return dto.Validate();
        }

        /// <summary>
        /// 从XElement转换为WidgetEditDto
        /// </summary>
        /// <param name="element">源XElement</param>
        /// <returns>转换后的WidgetEditDto</returns>
        private WidgetEditDto WidgetFromXElement(XElement element)
        {
            var widget = new WidgetEditDto
            {
                Type = GetAttributeValue(element, "type"),
                Name = GetAttributeValue(element, "name"),
                TilingBorderSize = GetAttributeValue(element, "tiling_border_size"),
                TileBackgroundAccordingToBorder = GetAttributeValue(element, "tile_background_according_to_border"),
                BackgroundTileSize = GetAttributeValue(element, "background_tile_size"),
                Focusable = GetAttributeValue(element, "focusable"),
                Style = GetAttributeValue(element, "style"),
                TrackAreaInset = GetAttributeValue(element, "track_area_inset"),
                Text = GetAttributeValue(element, "text"),
                InitialState = GetAttributeValue(element, "initial_state"),
                NumOfCols = GetAttributeValue(element, "num_of_cols"),
                NumOfRows = GetAttributeValue(element, "num_of_rows"),
                MaxNumOfRows = GetAttributeValue(element, "max_num_of_rows"),
                BorderSize = GetAttributeValue(element, "border_size"),
                ShowScrollBars = GetAttributeValue(element, "show_scroll_bars"),
                ScrollAreaInset = GetAttributeValue(element, "scroll_area_inset"),
                CellSize = GetAttributeValue(element, "cell_size"),
                LayoutStyle = GetAttributeValue(element, "layout_style"),
                LayoutAlignment = GetAttributeValue(element, "layout_alignment"),
                AutoShowScrollBars = GetAttributeValue(element, "auto_show_scroll_bars"),
                IncrementVec = GetAttributeValue(element, "increment_vec"),
                InitialValue = GetAttributeValue(element, "initial_value"),
                MaxAllowedDigit = GetAttributeValue(element, "max_allowed_digit"),
                MinAllowedValue = GetAttributeValue(element, "min_allowed_value"),
                MaxAllowedValue = GetAttributeValue(element, "max_allowed_value"),
                StepValue = GetAttributeValue(element, "step_value"),
                MinValue = GetAttributeValue(element, "min_value"),
                MaxValue = GetAttributeValue(element, "max_value"),
                VerticalAlignment = GetAttributeValue(element, "vertical_alignment"),
                HorizontalAlignment = GetAttributeValue(element, "horizontal_alignment"),
                TextHighlightColor = GetAttributeValue(element, "text_highlight_color"),
                TextColor = GetAttributeValue(element, "text_color"),
                FontSize = GetAttributeValue(element, "font_size"),
                Size = GetAttributeValue(element, "size"),
                Position = GetAttributeValue(element, "position"),
                ButtonMesh = GetAttributeValue(element, "button_mesh")
            };

            // 处理meshes容器
            var meshesElement = element.Element("meshes");
            if (meshesElement != null)
            {
                widget.Meshes = MeshesFromXElement(meshesElement);
            }

            // 处理sub_widgets容器
            var subWidgetsElement = element.Element("sub_widgets");
            if (subWidgetsElement != null)
            {
                widget.SubWidgets = new SubWidgetsEditContainer
                {
                    SubWidgetList = subWidgetsElement.Elements("sub_widget")
                        .Select(WidgetFromXElement)
                        .ToList()
                };
            }

            return widget;
        }

        /// <summary>
        /// 从XElement转换为MeshesEditContainer
        /// </summary>
        /// <param name="element">源XElement</param>
        /// <returns>转换后的MeshesEditContainer</returns>
        private MeshesEditContainer MeshesFromXElement(XElement element)
        {
            var meshes = new MeshesEditContainer
            {
                BackgroundMeshes = element.Elements("background_mesh")
                    .Select(MeshFromXElement)
                    .ToList(),
                ButtonMeshes = element.Elements("button_mesh")
                    .Select(MeshFromXElement)
                    .ToList(),
                ButtonPressedMeshes = element.Elements("button_pressed_mesh")
                    .Select(MeshFromXElement)
                    .ToList(),
                HighlightMeshes = element.Elements("highlight_mesh")
                    .Select(MeshFromXElement)
                    .ToList(),
                CursorMeshes = element.Elements("cursor_mesh")
                    .Select(MeshFromXElement)
                    .ToList(),
                LeftBorderMeshes = element.Elements("left_border_mesh")
                    .Select(MeshFromXElement)
                    .ToList(),
                RightBorderMeshes = element.Elements("right_border_mesh")
                    .Select(MeshFromXElement)
                    .ToList()
            };

            return meshes;
        }

        /// <summary>
        /// 从XElement转换为MeshEditDto
        /// </summary>
        /// <param name="element">源XElement</param>
        /// <returns>转换后的MeshEditDto</returns>
        private MeshEditDto MeshFromXElement(XElement element)
        {
            return new MeshEditDto
            {
                Name = GetAttributeValue(element, "name"),
                Tiling = GetAttributeValue(element, "tiling"),
                MainMesh = GetAttributeValue(element, "main_mesh"),
                Position = GetAttributeValue(element, "position")
            };
        }

        /// <summary>
        /// 将WidgetEditDto转换为XElement
        /// </summary>
        /// <param name="widget">源WidgetEditDto</param>
        /// <returns>转换后的XElement</returns>
        private XElement WidgetToXElement(WidgetEditDto widget)
        {
            var element = new XElement("widget");

            // 设置属性
            SetAttributeIfNotEmpty(element, "type", widget.Type);
            SetAttributeIfNotEmpty(element, "name", widget.Name);
            SetAttributeIfNotEmpty(element, "tiling_border_size", widget.TilingBorderSize);
            SetAttributeIfNotEmpty(element, "tile_background_according_to_border", widget.TileBackgroundAccordingToBorder);
            SetAttributeIfNotEmpty(element, "background_tile_size", widget.BackgroundTileSize);
            SetAttributeIfNotEmpty(element, "focusable", widget.Focusable);
            SetAttributeIfNotEmpty(element, "style", widget.Style);
            SetAttributeIfNotEmpty(element, "track_area_inset", widget.TrackAreaInset);
            SetAttributeIfNotEmpty(element, "text", widget.Text);
            SetAttributeIfNotEmpty(element, "initial_state", widget.InitialState);
            SetAttributeIfNotEmpty(element, "num_of_cols", widget.NumOfCols);
            SetAttributeIfNotEmpty(element, "num_of_rows", widget.NumOfRows);
            SetAttributeIfNotEmpty(element, "max_num_of_rows", widget.MaxNumOfRows);
            SetAttributeIfNotEmpty(element, "border_size", widget.BorderSize);
            SetAttributeIfNotEmpty(element, "show_scroll_bars", widget.ShowScrollBars);
            SetAttributeIfNotEmpty(element, "scroll_area_inset", widget.ScrollAreaInset);
            SetAttributeIfNotEmpty(element, "cell_size", widget.CellSize);
            SetAttributeIfNotEmpty(element, "layout_style", widget.LayoutStyle);
            SetAttributeIfNotEmpty(element, "layout_alignment", widget.LayoutAlignment);
            SetAttributeIfNotEmpty(element, "auto_show_scroll_bars", widget.AutoShowScrollBars);
            SetAttributeIfNotEmpty(element, "increment_vec", widget.IncrementVec);
            SetAttributeIfNotEmpty(element, "initial_value", widget.InitialValue);
            SetAttributeIfNotEmpty(element, "max_allowed_digit", widget.MaxAllowedDigit);
            SetAttributeIfNotEmpty(element, "min_allowed_value", widget.MinAllowedValue);
            SetAttributeIfNotEmpty(element, "max_allowed_value", widget.MaxAllowedValue);
            SetAttributeIfNotEmpty(element, "step_value", widget.StepValue);
            SetAttributeIfNotEmpty(element, "min_value", widget.MinValue);
            SetAttributeIfNotEmpty(element, "max_value", widget.MaxValue);
            SetAttributeIfNotEmpty(element, "vertical_alignment", widget.VerticalAlignment);
            SetAttributeIfNotEmpty(element, "horizontal_alignment", widget.HorizontalAlignment);
            SetAttributeIfNotEmpty(element, "text_highlight_color", widget.TextHighlightColor);
            SetAttributeIfNotEmpty(element, "text_color", widget.TextColor);
            SetAttributeIfNotEmpty(element, "font_size", widget.FontSize);
            SetAttributeIfNotEmpty(element, "size", widget.Size);
            SetAttributeIfNotEmpty(element, "position", widget.Position);
            SetAttributeIfNotEmpty(element, "button_mesh", widget.ButtonMesh);

            // 添加meshes容器
            if (widget.Meshes != null)
            {
                element.Add(MeshesToXElement(widget.Meshes));
            }

            // 添加sub_widgets容器
            if (widget.SubWidgets != null)
            {
                var subWidgetsElement = new XElement("sub_widgets");
                if (widget.SubWidgets.SubWidgetList != null)
                {
                    foreach (var subWidget in widget.SubWidgets.SubWidgetList)
                    {
                        subWidgetsElement.Add(WidgetToXElement(subWidget));
                    }
                }
                element.Add(subWidgetsElement);
            }

            return element;
        }

        /// <summary>
        /// 将MeshesEditContainer转换为XElement
        /// </summary>
        /// <param name="meshes">源MeshesEditContainer</param>
        /// <returns>转换后的XElement</returns>
        private XElement MeshesToXElement(MeshesEditContainer meshes)
        {
            var element = new XElement("meshes");

            AddMeshListIfNotNull(element, meshes.BackgroundMeshes, "background_mesh");
            AddMeshListIfNotNull(element, meshes.ButtonMeshes, "button_mesh");
            AddMeshListIfNotNull(element, meshes.ButtonPressedMeshes, "button_pressed_mesh");
            AddMeshListIfNotNull(element, meshes.HighlightMeshes, "highlight_mesh");
            AddMeshListIfNotNull(element, meshes.CursorMeshes, "cursor_mesh");
            AddMeshListIfNotNull(element, meshes.LeftBorderMeshes, "left_border_mesh");
            AddMeshListIfNotNull(element, meshes.RightBorderMeshes, "right_border_mesh");

            return element;
        }

        /// <summary>
        /// 将MeshEditDto转换为XElement
        /// </summary>
        /// <param name="mesh">源MeshEditDto</param>
        /// <returns>转换后的XElement</returns>
        private XElement MeshToXElement(MeshEditDto mesh)
        {
            var element = new XElement("mesh");
            
            SetAttributeIfNotEmpty(element, "name", mesh.Name);
            SetAttributeIfNotEmpty(element, "tiling", mesh.Tiling);
            SetAttributeIfNotEmpty(element, "main_mesh", mesh.MainMesh);
            SetAttributeIfNotEmpty(element, "position", mesh.Position);

            return element;
        }

        /// <summary>
        /// 生成属性差异补丁
        /// </summary>
        /// <param name="original">原始值</param>
        /// <param name="modified">修改后的值</param>
        /// <param name="xpath">目标XPath</param>
        /// <param name="attributeName">属性名</param>
        /// <param name="patch">补丁对象</param>
        private void GenerateAttributePatch(string? original, string? modified, string xpath, string attributeName, XmlPatch patch)
        {
            if (original != modified)
            {
                patch.AddOperation(new XmlNodeOperation
                {
                    Type = OperationType.UpdateAttribute,
                    XPath = xpath,
                    Attributes = new Dictionary<string, string> { { attributeName, modified ?? string.Empty } },
                    Description = $"更新属性 {attributeName}: '{original}' -> '{modified}'"
                });
            }
        }

        /// <summary>
        /// 生成widgets容器差异补丁
        /// </summary>
        /// <param name="original">原始widgets容器</param>
        /// <param name="modified">修改后的widgets容器</param>
        /// <param name="xpath">目标XPath</param>
        /// <param name="patch">补丁对象</param>
        private void GenerateWidgetsPatch(WidgetsEditContainer original, WidgetsEditContainer modified, string xpath, XmlPatch patch)
        {
            // 简化实现：这里可以扩展为更复杂的widget列表比较
            // 当前实现只处理基本的结构变化
        }

        /// <summary>
        /// 获取元素属性值
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="attributeName">属性名</param>
        /// <returns>属性值</returns>
        private string? GetAttributeValue(XElement element, string attributeName)
        {
            var attribute = element.Attribute(attributeName);
            return attribute?.Value;
        }

        /// <summary>
        /// 如果值不为空则设置属性
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="attributeName">属性名</param>
        /// <param name="value">属性值</param>
        private void SetAttributeIfNotEmpty(XElement element, string attributeName, string? value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                element.SetAttributeValue(attributeName, value);
            }
        }

        /// <summary>
        /// 如果网格列表不为空则添加到元素
        /// </summary>
        /// <param name="parent">父元素</param>
        /// <param name="meshList">网格列表</param>
        /// <param name="elementName">元素名</param>
        private void AddMeshListIfNotNull(XElement parent, List<MeshEditDto>? meshList, string elementName)
        {
            if (meshList != null && meshList.Count > 0)
            {
                foreach (var mesh in meshList)
                {
                    parent.Add(new XElement(elementName,
                        !string.IsNullOrEmpty(mesh.Name) ? new XAttribute("name", mesh.Name) : null,
                        !string.IsNullOrEmpty(mesh.Tiling) ? new XAttribute("tiling", mesh.Tiling) : null,
                        !string.IsNullOrEmpty(mesh.MainMesh) ? new XAttribute("main_mesh", mesh.MainMesh) : null,
                        !string.IsNullOrEmpty(mesh.Position) ? new XAttribute("position", mesh.Position) : null
                    ));
                }
            }
        }
    }
}