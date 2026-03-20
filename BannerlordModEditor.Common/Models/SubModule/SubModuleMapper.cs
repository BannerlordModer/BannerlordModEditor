using System.Linq;

namespace BannerlordModEditor.Common.Models.SubModule
{
    public static class SubModuleMapper
    {
        public static SubModuleDTO ToDTO(SubModuleDO source)
        {
            if (source == null)
                return new SubModuleDTO();

            return new SubModuleDTO
            {
                Name = source.Name,
                Id = source.Id,
                Version = source.Version,
                SingleplayerModule = source.SingleplayerModule,
                MultiplayerModule = source.MultiplayerModule,
                DependedModules = source.DependedModules?.Select(d => new DependedModuleDTO { Id = d.Id }).ToList() ?? new(),
                SubModules = source.SubModules?.Select(s => ToSubModuleItemDTO(s)).ToList() ?? new(),
                Xmls = source.Xmls?.Select(x => new XmlNodeDTO { Id = x.Id, Type = x.Type, Path = x.Path }).ToList() ?? new(),
                OptionalDependedModules = source.OptionalDependedModules?.Select(o => new OptionalDependedModuleDTO { Id = o.Id }).ToList() ?? new()
            };
        }

        public static SubModuleDO ToDO(SubModuleDTO source)
        {
            if (source == null)
                return new SubModuleDO();

            return new SubModuleDO
            {
                Name = source.Name,
                Id = source.Id,
                Version = source.Version,
                SingleplayerModule = source.SingleplayerModule,
                MultiplayerModule = source.MultiplayerModule,
                DependedModules = source.DependedModules?.Select(d => new DependedModuleDO { Id = d.Id }).ToList() ?? new(),
                SubModules = source.SubModules?.Select(s => ToSubModuleItemDO(s)).ToList() ?? new(),
                Xmls = source.Xmls?.Select(x => new XmlNodeDO { Id = x.Id, Type = x.Type, Path = x.Path }).ToList() ?? new(),
                OptionalDependedModules = source.OptionalDependedModules?.Select(o => new OptionalDependedModuleDO { Id = o.Id }).ToList() ?? new()
            };
        }

        private static SubModuleItemDTO ToSubModuleItemDTO(SubModuleItemDO source)
        {
            return new SubModuleItemDTO
            {
                Name = source.Name,
                DLLName = source.DLLName,
                SubModuleClassType = source.SubModuleClassType,
                IsOptional = source.IsOptional,
                IsTicked = source.IsTicked,
                Tags = source.Tags?.Select(t => new SubModuleTagDTO { Key = t.Key, Value = t.Value }).ToList() ?? new()
            };
        }

        private static SubModuleItemDO ToSubModuleItemDO(SubModuleItemDTO source)
        {
            return new SubModuleItemDO
            {
                Name = source.Name,
                DLLName = source.DLLName,
                SubModuleClassType = source.SubModuleClassType,
                IsOptional = source.IsOptional,
                IsTicked = source.IsTicked,
                Tags = source.Tags?.Select(t => new SubModuleTagDO { Key = t.Key, Value = t.Value }).ToList() ?? new()
            };
        }
    }
}
