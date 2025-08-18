using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 场景配置的映射器
    /// 用于ScenesDO和ScenesDTO之间的双向转换
    /// </summary>
    public static class ScenesMapper
    {
        public static ScenesDTO ToDTO(ScenesDO source)
        {
            if (source == null) return null;

            return new ScenesDTO
            {
                Type = source.Type,
                Sites = ConvertSites(source.Sites)
            };
        }

        public static ScenesDO ToDO(ScenesDTO source)
        {
            if (source == null) return null;

            var sites = ConvertSites(source.Sites);
            var hasSites = sites.SiteList.Count > 0;

            return new ScenesDO
            {
                Type = source.Type,
                Sites = sites
            };
        }

        private static SitesDTO ConvertSites(Sites source)
        {
            if (source == null || source.SiteList.Count == 0)
                return new SitesDTO();

            var result = new SitesDTO();
            foreach (var site in source.SiteList)
            {
                result.SiteList.Add(ConvertSite(site));
            }
            return result;
        }

        private static Sites ConvertSites(SitesDTO source)
        {
            if (source == null || source.SiteList.Count == 0)
                return new Sites();

            var result = new Sites();
            foreach (var site in source.SiteList)
            {
                result.SiteList.Add(ConvertSite(site));
            }
            return result;
        }

        private static SiteDTO ConvertSite(Site source)
        {
            if (source == null) return null;

            return new SiteDTO
            {
                Id = source.Id,
                Name = source.Name
            };
        }

        private static Site ConvertSite(SiteDTO source)
        {
            if (source == null) return null;

            return new Site
            {
                Id = source.Id,
                Name = source.Name
            };
        }
    }
}