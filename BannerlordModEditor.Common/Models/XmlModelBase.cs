using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models
{
    /// <summary>
    /// 通用XML模型基类，提供字段存在性追踪功能
    /// </summary>
    public abstract class XmlModelBase
    {
        /// <summary>
        /// 跟踪哪些属性在XML中实际存在（即使为空）
        /// </summary>
        [XmlIgnore]
        protected Dictionary<string, bool> PropertyExistence { get; } = new();

        /// <summary>
        /// 标记属性在XML中存在
        /// </summary>
        public void MarkPropertyExists(string propertyName)
        {
            PropertyExistence[propertyName] = true;
        }

        /// <summary>
        /// 检查属性在XML中是否存在
        /// </summary>
        public bool PropertyExists(string propertyName)
        {
            return PropertyExistence.ContainsKey(propertyName) && PropertyExistence[propertyName];
        }

        /// <summary>
        /// 获取所有存在的属性名称
        /// </summary>
        public IEnumerable<string> GetExistingProperties()
        {
            return PropertyExistence.Keys.Where(p => PropertyExistence[p]);
        }
    }

    /// <summary>
    /// 可空字符串属性，能够区分null（不存在）和空字符串（存在但为空）
    /// </summary>
    public class NullableStringProperty
    {
        private string? _value;
        private bool _existsInXml;

        public string? Value
        {
            get => _value;
            set
            {
                _value = value;
                _existsInXml = true; // 标记为在XML中存在
            }
        }

        public bool ExistsInXml => _existsInXml;

        public bool IsNullOrEmpty => string.IsNullOrEmpty(_value);

        public static implicit operator string?(NullableStringProperty prop) => prop?.Value;
        public static implicit operator NullableStringProperty(string? value) => new() { Value = value };

        public override string ToString() => _value ?? string.Empty;
    }

    /// <summary>
    /// 可空数值属性，能够区分0（存在但为0）和null（不存在）
    /// </summary>
    public class NullableNumericProperty<T> where T : struct
    {
        private T? _value;
        private bool _existsInXml;

        public T? Value
        {
            get => _value;
            set
            {
                _value = value;
                _existsInXml = true; // 标记为在XML中存在
            }
        }

        public bool ExistsInXml => _existsInXml;

        public bool HasValue => _value.HasValue;

        public static implicit operator T?(NullableNumericProperty<T> prop) => prop?.Value;
        public static implicit operator NullableNumericProperty<T>(T? value) => new() { Value = value };

        public override string ToString() => _value?.ToString() ?? string.Empty;
    }
}