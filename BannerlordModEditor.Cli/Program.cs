using CliFx;
using BannerlordModEditor.Cli.Commands;
using BannerlordModEditor.Cli.Helpers;
using BannerlordModEditor.Cli.Services;
using BannerlordModEditor.Common.Repositories.Testing;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.Common.Services.Testing;
using BannerlordModEditor.Common.Utils.Testing;
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
        // 注册核心服务
        services.AddSingleton<QualityMonitoringService>();
        services.AddTransient<IFileDiscoveryService, FileDiscoveryService>();
        services.AddTransient<IExcelXmlConverterService, EnhancedExcelXmlConverterService>();
        services.AddTransient<ErrorMessageService>();
        
        // 注册测试相关服务
        services.AddTransient<TestExecutionMonitorService>();
        services.AddTransient<TestResultAnalysisService>();
        services.AddTransient<QualityGateService>();
        services.AddTransient<TestResultRepository>(provider => 
            new TestResultRepository(Path.Combine(Directory.GetCurrentDirectory(), "test-results")));
        services.AddTransient<TestExecutionUtils>();
        services.AddTransient<TestReportGenerator>();
        services.AddTransient<TestNotificationService>();
        
        // 注册辅助服务
        services.AddTransient<CliOutputHelper>();
        
        // 注册命令
        services.AddTransient<ConvertCommand>();
        services.AddTransient<RecognizeCommand>();
        services.AddTransient<ListModelsCommand>();
        services.AddTransient<TestRateCommand>();
        services.AddTransient<TestResultsCommand>();
        services.AddTransient<QualityGatesCommand>();
    }
}