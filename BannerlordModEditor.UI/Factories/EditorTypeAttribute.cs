using System;

namespace BannerlordModEditor.UI.Factories;

/// <summary>
/// 编辑器类型属性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class EditorTypeAttribute : Attribute
{
    public string EditorType { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string XmlFileName { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
    public bool SupportsDto { get; set; } = false;
}