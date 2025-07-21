using System;

namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// 表示一个未适配的 XML 文件
    /// </summary>
    public class UnadaptedFile
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 完整路径
        /// </summary>
        public string FullPath { get; set; } = string.Empty;

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 预期的模型类名
        /// </summary>
        public string ExpectedModelName { get; set; } = string.Empty;

        /// <summary>
        /// 适配复杂度
        /// </summary>
        public AdaptationComplexity Complexity { get; set; }

        /// <summary>
        /// 是否需要分块处理
        /// </summary>
        public bool RequiresChunking => FileSize > 1024 * 1024; // 1MB

        public override string ToString()
        {
            return $"{FileName} ({FileSize:N0} bytes) -> {ExpectedModelName}";
        }
    }

    /// <summary>
    /// 适配复杂度枚举
    /// </summary>
    public enum AdaptationComplexity
    {
        /// <summary>
        /// 简单 - 基本结构，少量属性
        /// </summary>
        Simple,

        /// <summary>
        /// 中等 - 嵌套结构，多个容器类
        /// </summary>
        Medium,

        /// <summary>
        /// 复杂 - 深度嵌套，特殊要求
        /// </summary>
        Complex,

        /// <summary>
        /// 大型 - 需要分块处理
        /// </summary>
        Large
    }
}