using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("base")]
    public class LooknfeelDTO
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("virtual_resolution")]
        public string VirtualResolution { get; set; }

        [XmlElement("widgets")]
        public WidgetsContainerDTO Widgets { get; set; }
    }

    public class WidgetsContainerDTO
    {
        [XmlElement("widget")]
        public List<WidgetDTO> WidgetList { get; set; }
    }

    public class WidgetDTO
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
        public LooknfeelMeshesContainerDTO Meshes { get; set; }

        [XmlAttribute("button_mesh")]
        public string ButtonMesh { get; set; }

        [XmlElement("sub_widgets")]
        public SubWidgetsContainerDTO SubWidgets { get; set; }
    }

    public class LooknfeelMeshesContainerDTO
    {
        [XmlElement("background_mesh")]
        public List<LooknfeelMeshDTO> BackgroundMeshes { get; set; }

        [XmlElement("button_mesh")]
        public List<LooknfeelMeshDTO> ButtonMeshes { get; set; }

        [XmlElement("button_pressed_mesh")]
        public List<LooknfeelMeshDTO> ButtonPressedMeshes { get; set; }

        [XmlElement("highlight_mesh")]
        public List<LooknfeelMeshDTO> HighlightMeshes { get; set; }

        [XmlElement("cursor_mesh")]
        public List<LooknfeelMeshDTO> CursorMeshes { get; set; }

        [XmlElement("left_border_mesh")]
        public List<LooknfeelMeshDTO> LeftBorderMeshes { get; set; }

        [XmlElement("right_border_mesh")]
        public List<LooknfeelMeshDTO> RightBorderMeshes { get; set; }
    }

    public class LooknfeelMeshDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tiling")]
        public string Tiling { get; set; }

        [XmlAttribute("main_mesh")]
        public string MainMesh { get; set; }

        [XmlAttribute("position")]
        public string Position { get; set; }
    }

    public class SubWidgetsContainerDTO
    {
        [XmlElement("sub_widget")]
        public List<SubWidgetDTO> SubWidgetList { get; set; }
    }

    public class SubWidgetDTO
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
        public LooknfeelMeshesContainerDTO Meshes { get; set; }

        [XmlElement("sub_widgets")]
        public SubWidgetsContainerDTO SubWidgets { get; set; }
    }
}