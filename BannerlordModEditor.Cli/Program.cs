using CliFx;
using BannerlordModEditor.Cli.Commands;
using BannerlordModEditor.Cli.Services;
using BannerlordModEditor.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BannerlordModEditor.Cli;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            // 设置依赖注入
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            
            // 使用 CliFx 启动应用程序
            await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .UseTypeActivator(serviceProvider.GetRequiredService)
                .Build()
                .RunAsync(args);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"严重错误：{ex.Message}");
            Console.Error.WriteLine($"堆栈跟踪：{ex.StackTrace}");
            Environment.Exit(1);
        }
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        // 注册服务
        services.AddTransient<IFileDiscoveryService, FileDiscoveryService>();
        services.AddTransient<IExcelXmlConverterService, EnhancedExcelXmlConverterService>();
        services.AddTransient<ErrorMessageService>();
        
        // 注册命令
        services.AddTransient<ConvertCommand>();
        services.AddTransient<RecognizeCommand>();
        services.AddTransient<ListModelsCommand>();
    }
}