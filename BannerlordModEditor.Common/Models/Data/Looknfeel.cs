using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class Looknfeel
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("virtual_resolution")]
        public string VirtualResolution { get; set; }

        [XmlElement("widgets")]
        public WidgetsContainer Widgets { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeVirtualResolution() => !string.IsNullOrEmpty(VirtualResolution);
        public bool ShouldSerializeWidgets() => Widgets != null;
    }

    public class WidgetsContainer
    {
        [XmlElement("widget")]
        public List<Widget> WidgetList { get; set; }

        public bool ShouldSerializeWidgetList() => WidgetList != null && WidgetList.Count > 0;
    }

    public class Widget
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

        [XmlAttribute("vertical_alignment")]
        public string VerticalAlignment { get; set; }

        [XmlAttribute("horizontal_alignment")]
        public string HorizontalAlignment { get; set; }

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
        public LooknfeelMeshesContainer Meshes { get; set; }

        [XmlElement("sub_widgets")]
        public SubWidgetsContainer SubWidgets { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeTilingBorderSize() => !string.IsNullOrEmpty(TilingBorderSize);
        public bool ShouldSerializeTileBackgroundAccordingToBorder() => !string.IsNullOrEmpty(TileBackgroundAccordingToBorder);
        public bool ShouldSerializeBackgroundTileSize() => !string.IsNullOrEmpty(BackgroundTileSize);
        public bool ShouldSerializeFocusable() => !string.IsNullOrEmpty(Focusable);
        public bool ShouldSerializeStyle() => !string.IsNullOrEmpty(Style);
        public bool ShouldSerializeTrackAreaInset() => !string.IsNullOrEmpty(TrackAreaInset);
        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);
        public bool ShouldSerializeVerticalAlignment() => !string.IsNullOrEmpty(VerticalAlignment);
        public bool ShouldSerializeHorizontalAlignment() => !string.IsNullOrEmpty(HorizontalAlignment);
        public bool ShouldSerializeTextHighlightColor() => !string.IsNullOrEmpty(TextHighlightColor);
        public bool ShouldSerializeTextColor() => !string.IsNullOrEmpty(TextColor);
        public bool ShouldSerializeFontSize() => !string.IsNullOrEmpty(FontSize);
        public bool ShouldSerializeSize() => !string.IsNullOrEmpty(Size);
        public bool ShouldSerializePosition() => !string.IsNullOrEmpty(Position);
        public bool ShouldSerializeMeshes() => Meshes != null;
        public bool ShouldSerializeSubWidgets() => SubWidgets != null;
    }

    public class LooknfeelMeshesContainer
    {
        [XmlElement("background_mesh")]
        public List<LooknfeelMesh> BackgroundMeshes { get; set; }

        [XmlElement("button_mesh")]
        public List<LooknfeelMesh> ButtonMeshes { get; set; }

        [XmlElement("button_pressed_mesh")]
        public List<LooknfeelMesh> ButtonPressedMeshes { get; set; }

        [XmlElement("highlight_mesh")]
        public List<LooknfeelMesh> HighlightMeshes { get; set; }

        public bool ShouldSerializeBackgroundMeshes() => BackgroundMeshes != null && BackgroundMeshes.Count > 0;
        public bool ShouldSerializeButtonMeshes() => ButtonMeshes != null && ButtonMeshes.Count > 0;
        public bool ShouldSerializeButtonPressedMeshes() => ButtonPressedMeshes != null && ButtonPressedMeshes.Count > 0;
        public bool ShouldSerializeHighlightMeshes() => HighlightMeshes != null && HighlightMeshes.Count > 0;
    }

    public class LooknfeelMesh
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tiling")]
        public string Tiling { get; set; }

        [XmlAttribute("main_mesh")]
        public string MainMesh { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeTiling() => !string.IsNullOrEmpty(Tiling);
        public bool ShouldSerializeMainMesh() => !string.IsNullOrEmpty(MainMesh);
    }

    public class SubWidgetsContainer
    {
        [XmlElement("sub_widget")]
        public List<SubWidget> SubWidgetList { get; set; }

        public bool ShouldSerializeSubWidgetList() => SubWidgetList != null && SubWidgetList.Count > 0;
    }

    public class SubWidget
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

        [XmlAttribute("horizontal_aligment")]
        public string HorizontalAligment { get; set; }

        [XmlAttribute("horizontal_alignment")]
        public string HorizontalAlignment { get; set; }

        [XmlAttribute("text_color")]
        public string TextColor { get; set; }

        [XmlAttribute("text_highlight_color")]
        public string TextHighlightColor { get; set; }

        [XmlAttribute("font_size")]
        public string FontSize { get; set; }

        [XmlElement("meshes")]
        public LooknfeelMeshesContainer Meshes { get; set; }

        public bool ShouldSerializeRef() => !string.IsNullOrEmpty(Ref);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeSize() => !string.IsNullOrEmpty(Size);
        public bool ShouldSerializePosition() => !string.IsNullOrEmpty(Position);
        public bool ShouldSerializeStyle() => !string.IsNullOrEmpty(Style);
        public bool ShouldSerializeVerticalAlignment() => !string.IsNullOrEmpty(VerticalAlignment);
        public bool ShouldSerializeHorizontalAligment() => !string.IsNullOrEmpty(HorizontalAligment);
        public bool ShouldSerializeHorizontalAlignment() => !string.IsNullOrEmpty(HorizontalAlignment);
        public bool ShouldSerializeTextColor() => !string.IsNullOrEmpty(TextColor);
        public bool ShouldSerializeTextHighlightColor() => !string.IsNullOrEmpty(TextHighlightColor);
        public bool ShouldSerializeFontSize() => !string.IsNullOrEmpty(FontSize);
        public bool ShouldSerializeMeshes() => Meshes != null;
    }
}