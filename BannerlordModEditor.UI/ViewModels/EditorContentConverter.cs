using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using BannerlordModEditor.UI.ViewModels.Editors;

namespace BannerlordModEditor.UI.ViewModels;

public class EditorContentConverter : IValueConverter
{
    public static readonly EditorContentConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not EditorItemViewModel selectedEditor) return null;

        return selectedEditor.EditorType switch
        {
            "AttributeEditor" => new AttributeEditorViewModel(),
            "BoneBodyTypeEditor" => new BoneBodyTypeEditorViewModel(),
            "SkillEditor" => new SkillEditorViewModel(),
            _ => null
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class BoolToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue && parameter is string paramStr)
        {
            var parts = paramStr.Split('|');
            if (parts.Length == 2)
            {
                return boolValue ? parts[0] : parts[1];
            }
        }
        return value?.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 