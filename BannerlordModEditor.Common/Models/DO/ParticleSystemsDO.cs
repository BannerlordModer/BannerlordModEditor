using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("particle_effects")]
    public class ParticleSystemsDO
    {
        [XmlElement("effect")]
        public List<EffectDO> Effects { get; set; } = new List<EffectDO>();

        public bool ShouldSerializeEffects() => Effects != null && Effects.Count > 0;
    }

    public class EffectDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("guid")]
        public string? Guid { get; set; }

        [XmlAttribute("sound_code")]
        public string? SoundCode { get; set; }

        [XmlElement("emitters")]
        public EmittersDO? Emitters { get; set; }

        public bool ShouldSerializeName() => Name != null;
        public bool ShouldSerializeGuid() => Guid != null;
        public bool ShouldSerializeSoundCode() => SoundCode != null;
        public bool ShouldSerializeEmitters() => Emitters != null;
    }

    public class EmittersDO
    {
        [XmlElement("emitter")]
        public List<EmitterDO> EmitterList { get; set; } = new List<EmitterDO>();

        public bool ShouldSerializeEmitterList() => EmitterList != null && EmitterList.Count > 0;
    }

    public class EmitterDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("_index_")]
        public string? Index { get; set; }

        [XmlElement("children")]
        public ChildrenDO? Children { get; set; }

        [XmlElement("flags")]
        public ParticleFlagsDO? Flags { get; set; }

        [XmlElement("parameters")]
        public ParametersDO? Parameters { get; set; }

        // 运行时标记属性，用于序列化控制
        [XmlIgnore]
        public bool HasEmptyChildren { get; set; } = false;

        [XmlIgnore]
        public bool HasEmptyFlags { get; set; } = false;

        [XmlIgnore]
        public bool HasEmptyParameters { get; set; } = false;

        public bool ShouldSerializeName() => Name != null;
        public bool ShouldSerializeIndex() => Index != null;
        public bool ShouldSerializeChildren() => Children != null || HasEmptyChildren;
        public bool ShouldSerializeFlags() => Flags != null || HasEmptyFlags;
        public bool ShouldSerializeParameters() => Parameters != null || HasEmptyParameters;
    }

    public class ChildrenDO
    {
        [XmlElement("emitter")]
        public List<EmitterDO> EmitterList { get; set; } = new List<EmitterDO>();

        // 运行时标记属性，用于序列化控制
        [XmlIgnore]
        public bool HasEmptyEmitters { get; set; } = false;

        public bool ShouldSerializeEmitterList() => EmitterList != null && EmitterList.Count > 0;
        
        // 针对空children元素的序列化控制
        public bool ShouldSerializeChildren() => (EmitterList != null && EmitterList.Count > 0) || HasEmptyEmitters;
    }

    public class ParticleFlagsDO
    {
        [XmlElement("flag")]
        public List<ParticleFlagDO> FlagList { get; set; } = new List<ParticleFlagDO>();

        // 运行时标记属性，用于序列化控制
        [XmlIgnore]
        public bool HasEmptyFlags { get; set; } = false;

        public bool ShouldSerializeFlagList() => FlagList != null && FlagList.Count > 0;
    }

    public class ParticleFlagDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }

        public bool ShouldSerializeName() => Name != null;
        public bool ShouldSerializeValue() => Value != null;
    }

    public class ParametersDO
    {
        [XmlElement("parameter", Order = 1)]
        public List<ParameterDO> ParameterList { get; set; } = new List<ParameterDO>();

        [XmlElement("decal_materials", Order = 2)]
        public DecalMaterialsDO? DecalMaterials { get; set; }

        // 运行时标记属性，用于序列化控制
        [XmlIgnore]
        public bool HasDecalMaterials { get; set; } = false;

        [XmlIgnore]
        public bool HasEmptyParameters { get; set; } = false;

        public bool ShouldSerializeParameterList() => ParameterList != null && ParameterList.Count > 0;
        public bool ShouldSerializeDecalMaterials() => DecalMaterials != null || HasDecalMaterials;
        
        // 针对空parameters元素的序列化控制
        public bool ShouldSerializeParameters() => (ParameterList != null && ParameterList.Count > 0) || (DecalMaterials != null) || HasEmptyParameters;
    }

    public class ParameterDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }

        [XmlAttribute("base")]
        public string? Base { get; set; }

        [XmlAttribute("bias")]
        public string? Bias { get; set; }

        [XmlAttribute("curve")]
        public string? Curve { get; set; }

        [XmlElement("curve", Order = 1)]
        public List<CurveDO> ParameterCurves { get; set; } = new List<CurveDO>();

        [XmlElement("color", Order = 2)]
        public ColorDO? ColorElement { get; set; }

        [XmlElement("alpha", Order = 3)]
        public AlphaDO? AlphaElement { get; set; }

        // 运行时标记属性，用于序列化控制
        [XmlIgnore]
        public bool HasEmptyCurves { get; set; } = false;

        [XmlIgnore]
        public bool HasEmptyColor { get; set; } = false;

        [XmlIgnore]
        public bool HasEmptyAlpha { get; set; } = false;

        public bool ShouldSerializeName() => Name != null;
        public bool ShouldSerializeValue() => Value != null;
        public bool ShouldSerializeBase() => !string.IsNullOrEmpty(Base);
        public bool ShouldSerializeBias() => !string.IsNullOrEmpty(Bias);
        public bool ShouldSerializeCurve() => !string.IsNullOrEmpty(Curve);
        public bool ShouldSerializeParameterCurves() => ParameterCurves != null || HasEmptyCurves;
        
        // 为了向后兼容，添加ParameterCurve属性
        [XmlIgnore]
        public CurveDO? ParameterCurve
        {
            get => ParameterCurves?.FirstOrDefault();
            set
            {
                if (value != null)
                {
                    ParameterCurves ??= new List<CurveDO>();
                    if (ParameterCurves.Count == 0)
                    {
                        ParameterCurves.Add(value);
                    }
                    else
                    {
                        ParameterCurves[0] = value;
                    }
                }
            }
        }
        
        [XmlIgnore]
        public bool HasEmptyCurve
        {
            get => HasEmptyCurves;
            set => HasEmptyCurves = value;
        }
        
        // 为了向后兼容，添加ShouldSerializeParameterCurve方法
        public bool ShouldSerializeParameterCurve() => (ParameterCurves != null && ParameterCurves.Count > 0) || HasEmptyCurves;
        public bool ShouldSerializeColorElement() => ColorElement != null || HasEmptyColor;
        public bool ShouldSerializeAlphaElement() => AlphaElement != null || HasEmptyAlpha;
    }

    public class CurveDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("version")]
        public string? Version { get; set; }

        [XmlAttribute("default")]
        public string? Default { get; set; }

        [XmlAttribute("curve_multiplier")]
        public string? CurveMultiplier { get; set; }

        [XmlElement("keys")]
        public KeysDO? Keys { get; set; }

        // 运行时标记属性，用于序列化控制
        [XmlIgnore]
        public bool HasEmptyKeys { get; set; } = false;

        public bool ShouldSerializeName() => Name != null;
        public bool ShouldSerializeVersion() => !string.IsNullOrEmpty(Version);
        public bool ShouldSerializeDefault() => !string.IsNullOrEmpty(Default);
        public bool ShouldSerializeCurveMultiplier() => !string.IsNullOrEmpty(CurveMultiplier);
        public bool ShouldSerializeKeys() => Keys != null || HasEmptyKeys;
    }

    public class KeysDO
    {
        [XmlElement("key")]
        public List<KeyDO> KeyList { get; set; } = new List<KeyDO>();

        public bool ShouldSerializeKeyList() => KeyList != null && KeyList.Count > 0;
    }

    public class KeyDO
    {
        [XmlAttribute("time")]
        public string? Time { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }

        [XmlAttribute("position")]
        public string? Position { get; set; }

        [XmlAttribute("tangent")]
        public string? Tangent { get; set; }

        public bool ShouldSerializeTime() => Time != null;
        public bool ShouldSerializeValue() => Value != null;
        public bool ShouldSerializePosition() => !string.IsNullOrEmpty(Position);
        public bool ShouldSerializeTangent() => !string.IsNullOrEmpty(Tangent);
    }

    public class ColorDO
    {
        [XmlElement("keys")]
        public KeysDO? Keys { get; set; }

        // 运行时标记属性，用于序列化控制
        [XmlIgnore]
        public bool HasEmptyKeys { get; set; } = false;

        public bool ShouldSerializeKeys() => Keys != null || HasEmptyKeys;
    }

    public class AlphaDO
    {
        [XmlElement("keys")]
        public KeysDO? Keys { get; set; }

        // 运行时标记属性，用于序列化控制
        [XmlIgnore]
        public bool HasEmptyKeys { get; set; } = false;

        public bool ShouldSerializeKeys() => Keys != null || HasEmptyKeys;
    }

    public class DecalMaterialsDO
    {
        [XmlElement("decal_material")]
        public List<DecalMaterialDO> DecalMaterialList { get; set; } = new List<DecalMaterialDO>();

        // 运行时标记属性，用于序列化控制
        [XmlIgnore]
        public bool HasEmptyDecalMaterials { get; set; } = false;

        public bool ShouldSerializeDecalMaterialList() => DecalMaterialList != null && DecalMaterialList.Count > 0;
        
        // 针对空decal_materials元素的序列化控制
        public bool ShouldSerializeDecalMaterials() => (DecalMaterialList != null && DecalMaterialList.Count > 0) || HasEmptyDecalMaterials;
    }

    public class DecalMaterialDO
    {
        [XmlAttribute("value")]
        public string? Value { get; set; }

        public bool ShouldSerializeValue() => Value != null;
    }
}