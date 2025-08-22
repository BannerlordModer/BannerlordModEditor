using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO.Engine;
using BannerlordModEditor.Common.Models.DTO.Engine;

namespace BannerlordModEditor.Common.Mappers
{
    public static class ParticleSystems2Mapper
    {
        public static ParticleSystems2DTO ToDTO(ParticleSystems2DO source)
        {
            if (source == null) return null;

            return new ParticleSystems2DTO
            {
                ParticleSystems = source.ParticleSystems?.Select(ParticleSystem2ToDTO).ToList() ?? new List<ParticleSystem2DTO>()
            };
        }

        public static ParticleSystems2DO ToDO(ParticleSystems2DTO source)
        {
            if (source == null) return null;

            return new ParticleSystems2DO
            {
                ParticleSystems = source.ParticleSystems?.Select(ParticleSystem2ToDo).ToList() ?? new List<ParticleSystem2DO>(),
                HasParticleSystems = source.ParticleSystems != null && source.ParticleSystems.Count > 0
            };
        }

        public static ParticleSystem2DTO ParticleSystem2ToDTO(ParticleSystem2DO source)
        {
            if (source == null) return null;

            return new ParticleSystem2DTO
            {
                Name = source.Name,
                Version = source.Version,
                Emitter = Emitter2ToDTO(source.Emitter)
            };
        }

        public static ParticleSystem2DO ParticleSystem2ToDo(ParticleSystem2DTO source)
        {
            if (source == null) return null;

            return new ParticleSystem2DO
            {
                Name = source.Name,
                Version = source.Version,
                Emitter = Emitter2ToDo(source.Emitter),
                HasEmitter = source.Emitter != null
            };
        }

        public static Emitter2DTO Emitter2ToDTO(Emitter2DO source)
        {
            if (source == null) return null;

            return new Emitter2DTO
            {
                Flags = Flags2ToDTO(source.Flags),
                Properties = Properties2ToDTO(source.Properties)
            };
        }

        public static Emitter2DO Emitter2ToDo(Emitter2DTO source)
        {
            if (source == null) return null;

            return new Emitter2DO
            {
                Flags = Flags2ToDo(source.Flags),
                Properties = Properties2ToDo(source.Properties),
                HasFlags = source.Flags != null && source.Flags.Flags.Count > 0,
                HasProperties = source.Properties != null && source.Properties.Properties.Count > 0
            };
        }

        public static Flags2DTO Flags2ToDTO(Flags2DO source)
        {
            if (source == null) return null;

            return new Flags2DTO
            {
                Flags = source.Flags?.Select(Flag2ToDTO).ToList() ?? new List<Flag2DTO>()
            };
        }

        public static Flags2DO Flags2ToDo(Flags2DTO source)
        {
            if (source == null) return null;

            return new Flags2DO
            {
                Flags = source.Flags?.Select(Flag2ToDo).ToList() ?? new List<Flag2DO>()
            };
        }

        public static Flag2DTO Flag2ToDTO(Flag2DO source)
        {
            if (source == null) return null;

            return new Flag2DTO
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        public static Flag2DO Flag2ToDo(Flag2DTO source)
        {
            if (source == null) return null;

            return new Flag2DO
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        public static Properties2DTO Properties2ToDTO(Properties2DO source)
        {
            if (source == null) return null;

            return new Properties2DTO
            {
                Properties = source.Properties?.Select(Property2ToDTO).ToList() ?? new List<Property2DTO>()
            };
        }

        public static Properties2DO Properties2ToDo(Properties2DTO source)
        {
            if (source == null) return null;

            return new Properties2DO
            {
                Properties = source.Properties?.Select(Property2ToDo).ToList() ?? new List<Property2DO>()
            };
        }

        public static Property2DTO Property2ToDTO(Property2DO source)
        {
            if (source == null) return null;

            return new Property2DTO
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        public static Property2DO Property2ToDo(Property2DTO source)
        {
            if (source == null) return null;

            return new Property2DO
            {
                Name = source.Name,
                Value = source.Value
            };
        }
    }
}