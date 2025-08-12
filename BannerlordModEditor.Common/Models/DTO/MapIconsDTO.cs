using System;
using System.Collections.Generic;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class MapIconsDTO
    {
        public string? Type { get; set; }

        public MapIconsContainerDTO MapIconsContainer { get; set; } = new MapIconsContainerDTO();

        // ShouldSerialize方法（对应DO层）
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);

        // 便捷属性
        public bool HasType => !string.IsNullOrEmpty(Type);
    }

    public class MapIconsContainerDTO
    {
        public List<MapIconDTO> MapIconList { get; set; } = new List<MapIconDTO>();

        // ShouldSerialize方法
        public bool ShouldSerializeMapIconList() => MapIconList != null && MapIconList.Count > 0;

        // 便捷属性
        public int MapIconCount => MapIconList?.Count ?? 0;
    }

    public class MapIconDTO
    {
        public string? Id { get; set; }
        public string? IdStr { get; set; }
        public string? Flags { get; set; }
        public string? MeshName { get; set; }
        public string? MeshScale { get; set; }
        public string? SoundNo { get; set; }
        public string? OffsetPos { get; set; }
        public string? DirtName { get; set; }

        // ShouldSerialize方法
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeIdStr() => !string.IsNullOrEmpty(IdStr);
        public bool ShouldSerializeFlags() => !string.IsNullOrEmpty(Flags);
        public bool ShouldSerializeMeshName() => !string.IsNullOrEmpty(MeshName);
        public bool ShouldSerializeMeshScale() => !string.IsNullOrEmpty(MeshScale);
        public bool ShouldSerializeSoundNo() => !string.IsNullOrEmpty(SoundNo);
        public bool ShouldSerializeOffsetPos() => !string.IsNullOrEmpty(OffsetPos);
        public bool ShouldSerializeDirtName() => !string.IsNullOrEmpty(DirtName);

        // 类型安全的便捷属性
        public bool HasId => !string.IsNullOrEmpty(Id);
        public bool HasIdStr => !string.IsNullOrEmpty(IdStr);
        public bool HasFlags => !string.IsNullOrEmpty(Flags);
        public bool HasMeshName => !string.IsNullOrEmpty(MeshName);
        public bool HasSoundNo => !string.IsNullOrEmpty(SoundNo);
        public bool HasOffsetPos => !string.IsNullOrEmpty(OffsetPos);
        public bool HasDirtName => !string.IsNullOrEmpty(DirtName);

        public float? MeshScaleFloat => float.TryParse(MeshScale, out float val) ? val : (float?)null;
        public int? SoundNoInt => int.TryParse(SoundNo, out int val) ? val : (int?)null;

        // 设置方法
        public void SetMeshScaleFloat(float? value) => MeshScale = value?.ToString();
        public void SetSoundNoInt(int? value) => SoundNo = value?.ToString();
    }
}