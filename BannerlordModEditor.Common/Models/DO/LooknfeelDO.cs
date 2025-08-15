using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class LooknfeelDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("virtual_resolution")]
        public string VirtualResolution { get; set; }

        [XmlElement("widgets")]
        public WidgetsContainerDO Widgets { get; set; }

        [XmlIgnore]
        public bool HasEmptyWidgets { get; set; } = false;

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeVirtualResolution() => !string.IsNullOrEmpty(VirtualResolution);
        public bool ShouldSerializeWidgets() => HasEmptyWidgets || Widgets != null;
    }

    public class WidgetsContainerDO
    {
        [XmlElement("widget")]
        public List<WidgetDO> WidgetList { get; set; }

        [XmlIgnore]
        public bool HasEmptyWidgetList { get; set; } = false;

        public bool ShouldSerializeWidgetList() => HasEmptyWidgetList || (WidgetList != null && WidgetList.Count > 0);
    }

    public class WidgetDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tiling_border_size")]
        public string TilingBorderSize { get; set; }

        [XmlAttribute("tile_background_according_to_border")]
        public string TileBackgroundAccordingToBorder { get; set; }

        [XmlAttribute("background_tile_size")]
        public string BackgroundTileSize { get; set; }

        [XmlAttribute("focusable")]
        public string Focusable { get; set; }

        [XmlAttribute("style")]
        public string Style { get; set; }

        [XmlAttribute("track_area_inset")]
        public string TrackAreaInset { get; set; }

        [XmlAttribute("text")]
        public string Text { get; set; }

        [XmlAttribute("initial_state")]
        public string InitialState { get; set; }

        [XmlAttribute("num_of_cols")]
        public string NumOfCols { get; set; }

        [XmlAttribute("num_of_rows")]
        public string NumOfRows { get; set; }

        [XmlAttribute("max_num_of_rows")]
        public string MaxNumOfRows { get; set; }

        [XmlAttribute("border_size")]
        public string BorderSize { get; set; }

        [XmlAttribute("show_scroll_bars")]
        public string ShowScrollBars { get; set; }

        [XmlAttribute("scroll_area_inset")]
        public string ScrollAreaInset { get; set; }

        [XmlAttribute("cell_size")]
        public string CellSize { get; set; }

        [XmlAttribute("layout_style")]
        public string LayoutStyle { get; set; }

        [XmlAttribute("layout_alignment")]
        public string LayoutAlignment { get; set; }

        [XmlAttribute("auto_show_scroll_bars")]
        public string AutoShowScrollBars { get; set; }

        [XmlAttribute("increment_vec")]
        public string IncrementVec { get; set; }

        [XmlAttribute("initial_value")]
        public string InitialValue { get; set; }

        [XmlAttribute("max_allowed_digit")]
        public string MaxAllowedDigit { get; set; }

        [XmlAttribute("min_allowed_value")]
        public string MinAllowedValue { get; set; }

        [XmlAttribute("max_allowed_value")]
        public string MaxAllowedValue { get; set; }

        [XmlAttribute("step_value")]
        public string StepValue { get; set; }

        [XmlAttribute("min_value")]
        public string MinValue { get; set; }

        [XmlAttribute("max_value")]
        public string MaxValue { get; set; }

        [XmlAttribute("vertical_alignment")]
        public string VerticalAlignment { get; set; }

        [XmlAttribute("horizontal_alignment")]
        public string HorizontalAlignment { get; set; }

        [XmlAttribute("horizontal_aligment")]
        public string HorizontalAligment { get; set; }

        [XmlAttribute("text_highlight_color")]
        public string TextHighlightColor { get; set; }

        [XmlAttribute("text_color")]
        public string TextColor { get; set; }

        [XmlAttribute("font_size")]
        public string FontSize { get; set; }

        [XmlAttribute("size")]
        public string Size { get; set; }

        [XmlAttribute("position")]
        public string Position { get; set; }

        [XmlElement("meshes")]
        public LooknfeelMeshesContainerDO Meshes { get; set; }

        [XmlAttribute("button_mesh")]
        public string ButtonMesh { get; set; }

        [XmlElement("sub_widgets")]
        public SubWidgetsContainerDO SubWidgets { get; set; }

        [XmlIgnore]
        public bool HasEmptyMeshes { get; set; } = false;

        [XmlIgnore]
        public bool HasEmptySubWidgets { get; set; } = false;

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeTilingBorderSize() => !string.IsNullOrEmpty(TilingBorderSize);
        public bool ShouldSerializeTileBackgroundAccordingToBorder() => !string.IsNullOrEmpty(TileBackgroundAccordingToBorder);
        public bool ShouldSerializeBackgroundTileSize() => !string.IsNullOrEmpty(BackgroundTileSize);
        public bool ShouldSerializeFocusable() => !string.IsNullOrEmpty(Focusable);
        public bool ShouldSerializeStyle() => !string.IsNullOrEmpty(Style);
        public bool ShouldSerializeTrackAreaInset() => !string.IsNullOrEmpty(TrackAreaInset);
        public bool ShouldSerializeText() => Text != null;
        public bool ShouldSerializeInitialState() => !string.IsNullOrEmpty(InitialState);
        public bool ShouldSerializeNumOfCols() => !string.IsNullOrEmpty(NumOfCols);
        public bool ShouldSerializeNumOfRows() => !string.IsNullOrEmpty(NumOfRows);
        public bool ShouldSerializeMaxNumOfRows() => !string.IsNullOrEmpty(MaxNumOfRows);
        public bool ShouldSerializeBorderSize() => !string.IsNullOrEmpty(BorderSize);
        public bool ShouldSerializeShowScrollBars() => !string.IsNullOrEmpty(ShowScrollBars);
        public bool ShouldSerializeScrollAreaInset() => !string.IsNullOrEmpty(ScrollAreaInset);
        public bool ShouldSerializeCellSize() => !string.IsNullOrEmpty(CellSize);
        public bool ShouldSerializeLayoutStyle() => !string.IsNullOrEmpty(LayoutStyle);
        public bool ShouldSerializeLayoutAlignment() => !string.IsNullOrEmpty(LayoutAlignment);
        public bool ShouldSerializeAutoShowScrollBars() => !string.IsNullOrEmpty(AutoShowScrollBars);
        public bool ShouldSerializeIncrementVec() => !string.IsNullOrEmpty(IncrementVec);
        public bool ShouldSerializeInitialValue() => !string.IsNullOrEmpty(InitialValue);
        public bool ShouldSerializeMaxAllowedDigit() => !string.IsNullOrEmpty(MaxAllowedDigit);
        public bool ShouldSerializeMinAllowedValue() => !string.IsNullOrEmpty(MinAllowedValue);
        public bool ShouldSerializeMaxAllowedValue() => !string.IsNullOrEmpty(MaxAllowedValue);
        public bool ShouldSerializeStepValue() => !string.IsNullOrEmpty(StepValue);
        public bool ShouldSerializeMinValue() => !string.IsNullOrEmpty(MinValue);
        public bool ShouldSerializeMaxValue() => !string.IsNullOrEmpty(MaxValue);
        public bool ShouldSerializeVerticalAlignment() => !string.IsNullOrEmpty(VerticalAlignment);
        public bool ShouldSerializeHorizontalAlignment() => !string.IsNullOrEmpty(HorizontalAlignment);
        public bool ShouldSerializeHorizontalAligment() => !string.IsNullOrEmpty(HorizontalAligment);
        public bool ShouldSerializeTextHighlightColor() => !string.IsNullOrEmpty(TextHighlightColor);
        public bool ShouldSerializeTextColor() => !string.IsNullOrEmpty(TextColor);
        public bool ShouldSerializeFontSize() => !string.IsNullOrEmpty(FontSize);
        public bool ShouldSerializeSize() => !string.IsNullOrEmpty(Size);
        public bool ShouldSerializePosition() => !string.IsNullOrEmpty(Position);
        public bool ShouldSerializeMeshes() => HasEmptyMeshes || Meshes != null;
        public bool ShouldSerializeButtonMesh() => !string.IsNullOrEmpty(ButtonMesh);
        public bool ShouldSerializeSubWidgets() => HasEmptySubWidgets || SubWidgets != null;
    }

    public class LooknfeelMeshesContainerDO
    {
        [XmlElement("background_mesh")]
        public List<LooknfeelMeshDO> BackgroundMeshes { get; set; }

        [XmlElement("button_mesh")]
        public List<LooknfeelMeshDO> ButtonMeshes { get; set; }

        [XmlElement("button_pressed_mesh")]
        public List<LooknfeelMeshDO> ButtonPressedMeshes { get; set; }

        [XmlElement("highlight_mesh")]
        public List<LooknfeelMeshDO> HighlightMeshes { get; set; }

        [XmlElement("cursor_mesh")]
        public List<LooknfeelMeshDO> CursorMeshes { get; set; }

        [XmlElement("left_border_mesh")]
        public List<LooknfeelMeshDO> LeftBorderMeshes { get; set; }

        [XmlElement("right_border_mesh")]
        public List<LooknfeelMeshDO> RightBorderMeshes { get; set; }

        [XmlIgnore]
        public bool HasEmptyBackgroundMeshes { get; set; } = false;
        [XmlIgnore]
        public bool HasEmptyButtonMeshes { get; set; } = false;
        [XmlIgnore]
        public bool HasEmptyButtonPressedMeshes { get; set; } = false;
        [XmlIgnore]
        public bool HasEmptyHighlightMeshes { get; set; } = false;
        [XmlIgnore]
        public bool HasEmptyCursorMeshes { get; set; } = false;
        [XmlIgnore]
        public bool HasEmptyLeftBorderMeshes { get; set; } = false;
        [XmlIgnore]
        public bool HasEmptyRightBorderMeshes { get; set; } = false;

        public bool ShouldSerializeBackgroundMeshes() => HasEmptyBackgroundMeshes || (BackgroundMeshes != null && BackgroundMeshes.Count > 0);
        public bool ShouldSerializeButtonMeshes() => HasEmptyButtonMeshes || (ButtonMeshes != null && ButtonMeshes.Count > 0);
        public bool ShouldSerializeButtonPressedMeshes() => HasEmptyButtonPressedMeshes || (ButtonPressedMeshes != null && ButtonPressedMeshes.Count > 0);
        public bool ShouldSerializeHighlightMeshes() => HasEmptyHighlightMeshes || (HighlightMeshes != null && HighlightMeshes.Count > 0);
        public bool ShouldSerializeCursorMeshes() => HasEmptyCursorMeshes || (CursorMeshes != null && CursorMeshes.Count > 0);
        public bool ShouldSerializeLeftBorderMeshes() => HasEmptyLeftBorderMeshes || (LeftBorderMeshes != null && LeftBorderMeshes.Count > 0);
        public bool ShouldSerializeRightBorderMeshes() => HasEmptyRightBorderMeshes || (RightBorderMeshes != null && RightBorderMeshes.Count > 0);
    }

    public class LooknfeelMeshDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tiling")]
        public string Tiling { get; set; }

        [XmlAttribute("main_mesh")]
        public string MainMesh { get; set; }

        [XmlAttribute("position")]
        public string Position { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeTiling() => !string.IsNullOrEmpty(Tiling);
        public bool ShouldSerializeMainMesh() => !string.IsNullOrEmpty(MainMesh);
        public bool ShouldSerializePosition() => !string.IsNullOrEmpty(Position);
    }

    public class SubWidgetsContainerDO
    {
        [XmlElement("sub_widget")]
        public List<SubWidgetDO> SubWidgetList { get; set; }

        [XmlIgnore]
        public bool HasEmptySubWidgetList { get; set; } = false;

        public bool ShouldSerializeSubWidgetList() => HasEmptySubWidgetList || (SubWidgetList != null && SubWidgetList.Count > 0);
    }

    public class SubWidgetDO
    {
        [XmlAttribute("ref")]
        public string Ref { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("size")]
        public string Size { get; set; }

        [XmlAttribute("position")]
        public string Position { get; set; }

        [XmlAttribute("style")]
        public string Style { get; set; }

        [XmlAttribute("vertical_alignment")]
        public string VerticalAlignment { get; set; }

        [XmlAttribute("horizontal_alignment")]
        public string HorizontalAlignment { get; set; }

        [XmlAttribute("horizontal_aligment")]
        public string HorizontalAligment { get; set; }

        [XmlAttribute("scroll_speed")]
        public string ScrollSpeed { get; set; }

        [XmlAttribute("cell_size")]
        public string CellSize { get; set; }

        [XmlAttribute("layout_style")]
        public string LayoutStyle { get; set; }

        [XmlAttribute("layout_alignment")]
        public string LayoutAlignment { get; set; }

        [XmlAttribute("text")]
        public string Text { get; set; }

        [XmlAttribute("text_color")]
        public string TextColor { get; set; }

        [XmlAttribute("text_highlight_color")]
        public string TextHighlightColor { get; set; }

        [XmlAttribute("font_size")]
        public string FontSize { get; set; }

        [XmlElement("meshes")]
        public LooknfeelMeshesContainerDO Meshes { get; set; }

        [XmlElement("sub_widgets")]
        public SubWidgetsContainerDO SubWidgets { get; set; }

        [XmlIgnore]
        public bool HasEmptyMeshes { get; set; } = false;
        [XmlIgnore]
        public bool HasEmptySubWidgets { get; set; } = false;

        public bool ShouldSerializeRef() => !string.IsNullOrEmpty(Ref);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeSize() => !string.IsNullOrEmpty(Size);
        public bool ShouldSerializePosition() => !string.IsNullOrEmpty(Position);
        public bool ShouldSerializeStyle() => !string.IsNullOrEmpty(Style);
        public bool ShouldSerializeVerticalAlignment() => !string.IsNullOrEmpty(VerticalAlignment);
        public bool ShouldSerializeHorizontalAlignment() => !string.IsNullOrEmpty(HorizontalAlignment);
        public bool ShouldSerializeHorizontalAligment() => !string.IsNullOrEmpty(HorizontalAligment);
        public bool ShouldSerializeScrollSpeed() => !string.IsNullOrEmpty(ScrollSpeed);
        public bool ShouldSerializeCellSize() => !string.IsNullOrEmpty(CellSize);
        public bool ShouldSerializeLayoutStyle() => !string.IsNullOrEmpty(LayoutStyle);
        public bool ShouldSerializeLayoutAlignment() => !string.IsNullOrEmpty(LayoutAlignment);
        public bool ShouldSerializeText() => Text != null;
        public bool ShouldSerializeTextColor() => !string.IsNullOrEmpty(TextColor);
        public bool ShouldSerializeTextHighlightColor() => !string.IsNullOrEmpty(TextHighlightColor);
        public bool ShouldSerializeFontSize() => !string.IsNullOrEmpty(FontSize);
        public bool ShouldSerializeMeshes() => HasEmptyMeshes || Meshes != null;
        public bool ShouldSerializeSubWidgets() => HasEmptySubWidgets || SubWidgets != null;
    }
}