using System.CommandLine;

namespace BannerlordModEditor.Cli;

sealed class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Bannerlord Mod Editor CLI");
        Console.WriteLine("============================");
        
        var rootCommand = new RootCommand("Bannerlord Mod Editor - Command Line Interface");
        
        // TODO: Add CLI commands here
        
        await rootCommand.InvokeAsync(args);
    }
}