using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using BannerlordModEditor.UI.ViewModels.Editors;

namespace BannerlordModEditor.UI.ViewModels;

public class EditorContentConverter : IMultiValueConverter
{
    public static readonly EditorContentConverter Instance = new();

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count < 3) return null;

        var selectedEditor = values[0] as EditorItemViewModel;
        var attributeEditor = values[1] as AttributeEditorViewModel;
        var boneBodyTypeEditor = values[2] as BoneBodyTypeEditorViewModel;

        if (selectedEditor == null) return null;

        return selectedEditor.EditorType switch
        {
            "AttributeEditor" => attributeEditor,
            "BoneBodyTypeEditor" => boneBodyTypeEditor,
            _ => null
        };
    }

    public object[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 