using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("music_parameters")]
    public class MusicParameters
    {
        [XmlElement("music_parameter")]
        public List<MusicParameter> Parameters { get; set; } = new List<MusicParameter>();
    }

    public class MusicParameter
    {
       private string? _id;
       private bool _idExists;
       private string? _value;
       private bool _valueExists;

       [XmlAttribute("id")]
       public string? Id
       {
           get => _idExists ? _id : null;
           set
           {
               _id = value;
               _idExists = true;
           }
       }

       [XmlAttribute("value")]
       public string? Value
       {
           get => _valueExists ? _value : null;
           set
           {
               _value = value;
               _valueExists = true;
           }
       }

       public bool ShouldSerializeId() => _idExists;
       public bool ShouldSerializeValue() => _valueExists;
    }
} 