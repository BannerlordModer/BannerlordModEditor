using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    /// <summary>
    /// Case-insensitive boolean wrapper to preserve exact XML string values
    /// </summary>
    [XmlType(TypeName = "boolean")]
    public class BooleanProperty
    {
        private bool _value;
        private string _originalValue = "";

        [XmlIgnore]
        public bool Value => _value;

        [XmlIgnore]
        public string OriginalValue => _originalValue;

        [XmlText]
        public string XmlValue
        {
            get => _originalValue;
            set
            {
                _originalValue = value ?? "";
                _value = ParseBoolean(value);
            }
        }

        public static bool ParseBoolean(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var normalized = value.Trim().ToLowerInvariant();
            return normalized switch
            {
                "true" or "1" or "yes" or "on" => true,
                "false" or "0" or "no" or "off" => false,
                _ => bool.TryParse(value, out var result) ? result : false
            };
        }

        public static StringValue FromBool(bool value) => new StringValue { Value = value.ToString().ToLowerInvariant() };

        public static implicit operator bool(BooleanProperty prop) => prop?._value ?? false;
        public static implicit operator BooleanProperty(bool value) => new BooleanProperty { XmlValue = value.ToString().ToLowerInvariant() };

        public override string ToString() => _originalValue;
    }

    /// <summary>
    /// Case-insensitive boolean wrapper for better XML compatibility
    /// </summary>
    [XmlType(TypeName = "xmlboolean")]
    public class XmlBoolean
    {
        private bool _value;
        private string _originalValue = "";

        [XmlIgnore]
        public bool Value => _value;

        [XmlIgnore]
        public string OriginalValue => _originalValue;

        [XmlText]
        public string ValueStr
        {
            get => _originalValue;
            set
            {
                _originalValue = value ?? "";
                _value = BooleanProperty.ParseBoolean(value);
            }
        }

        public static implicit operator bool(XmlBoolean b) => b?._value ?? false;
        public static implicit operator XmlBoolean(bool b) => new XmlBoolean { ValueStr = b.ToString().ToLowerInvariant() };

        public override string ToString() => _originalValue;
    }
}