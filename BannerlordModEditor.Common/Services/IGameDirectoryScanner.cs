using System.Collections.Generic;
using System.Threading.Tasks;

namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// 游戏目录扫描服务接口
    /// 用于自动检测骑砍2的安装目录
    /// </summary>
    public interface IGameDirectoryScanner
    {
        /// <summary>
        /// 异步扫描所有可能的游戏安装目录
        /// </summary>
        /// <returns>找到的游戏目录列表</returns>
        Task<List<GameDirectoryInfo>> ScanForGameDirectoriesAsync();

        /// <summary>
        /// 获取第一个找到的游戏目录
        /// </summary>
        /// <returns>游戏目录路径，未找到则返回null</returns>
        Task<string?> GetFirstGameDirectoryAsync();

        /// <summary>
        /// 验证指定路径是否为有效的骑砍2安装目录
        /// </summary>
        /// <param name="path">要验证的路径</param>
        /// <returns>是否为有效目录</returns>
        bool IsValidGameDirectory(string path);

        /// <summary>
        /// 获取游戏版本信息
        /// </summary>
        /// <param name="gameDirectory">游戏目录</param>
        /// <returns>游戏版本，无法识别则返回null</returns>
        string? GetGameVersion(string gameDirectory);
    }

    /// <summary>
    /// 游戏目录信息
    /// </summary>
    public class GameDirectoryInfo
    {
        /// <summary>
        /// 游戏安装路径
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 安装来源（Steam、GOG、Epic等）
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// 游戏版本号
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 是否为有效目录
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// ModuleData目录路径
        /// </summary>
        public string ModuleDataPath { get; set; } = string.Empty;
    }
}
