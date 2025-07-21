using System.Collections.Generic;
using System.Threading.Tasks;

namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// 用于发现未适配 XML 文件的服务接口
    /// </summary>
    public interface IFileDiscoveryService
    {
        /// <summary>
        /// 异步查找所有未适配的 XML 文件
        /// </summary>
        /// <returns>未适配文件的列表</returns>
        Task<List<UnadaptedFile>> FindUnadaptedFilesAsync();

        /// <summary>
        /// 将 XML 文件名转换为对应的 C# 模型类名
        /// </summary>
        /// <param name="xmlFileName">XML 文件名</param>
        /// <returns>转换后的模型类名</returns>
        string ConvertToModelName(string xmlFileName);

        /// <summary>
        /// 检查指定的模型是否已存在
        /// </summary>
        /// <param name="modelName">模型类名</param>
        /// <param name="searchDirectories">搜索目录数组</param>
        /// <returns>如果模型存在则返回 true</returns>
        bool ModelExists(string modelName, string[] searchDirectories);
    }
}