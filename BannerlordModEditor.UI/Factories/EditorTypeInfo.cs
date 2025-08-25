using System;

namespace BannerlordModEditor.UI.Factories;

/// <summary>
/// 编辑器类型信息
/// </summary>
public class EditorTypeInfo
{
    public string EditorType { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string XmlFileName { get; set; } = string.Empty;
    public Type ViewModelType { get; set; } = null!;
    public Type ViewType { get; set; } = null!;
    public bool SupportsDto { get; set; }
    public string Category { get; set; } = "General";
}