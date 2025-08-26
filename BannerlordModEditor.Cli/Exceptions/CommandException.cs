using System;

namespace BannerlordModEditor.Cli.Exceptions
{
    /// <summary>
    /// CLI命令异常，用于设置退出码
    /// </summary>
    public class CommandException : Exception
    {
        /// <summary>
        /// 退出码
        /// </summary>
        public int ExitCode { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="exitCode">退出码</param>
        public CommandException(string message, int exitCode = 1) : base(message)
        {
            ExitCode = exitCode;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="innerException">内部异常</param>
        /// <param name="exitCode">退出码</param>
        public CommandException(string message, Exception innerException, int exitCode = 1) : base(message, innerException)
        {
            ExitCode = exitCode;
        }
    }
}