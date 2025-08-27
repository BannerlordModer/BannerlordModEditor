namespace BannerlordModEditor.Cli.Exceptions
{
    /// <summary>
    /// CLI命令执行异常
    /// </summary>
    public class CliCommandException : Exception
    {
        public CliCommandException(string message) : base(message)
        {
        }

        public CliCommandException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}