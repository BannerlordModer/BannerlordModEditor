using System;
using Microsoft.Extensions.DependencyInjection;
using BannerlordModEditor.UI.Extensions;
using BannerlordModEditor.UI.Factories;

public class DependencyInjectionTest
{
    public static void Main()
    {
        Console.WriteLine("=== 依赖注入问题验证测试 ===");
        
        try
        {
            Console.WriteLine("1. 测试 AddMinimalEditorManagerServices...");
            var services = new ServiceCollection();
            services.AddMinimalEditorManagerServices();
            
            Console.WriteLine("2. 尝试构建 ServiceProvider...");
            var serviceProvider = services.BuildServiceProvider();
            
            Console.WriteLine("3. 尝试获取 IEditorManagerFactory...");
            var factory = serviceProvider.GetService<IEditorManagerFactory>();
            Console.WriteLine($"   IEditorManagerFactory: {(factory != null ? "成功" : "失败")}");
            
            Console.WriteLine("4. 尝试获取 EditorManagerViewModel...");
            var viewModel = serviceProvider.GetService<EditorManagerViewModel>();
            Console.WriteLine($"   EditorManagerViewModel: {(viewModel != null ? "成功" : "失败")}");
            
            Console.WriteLine("5. 检查已注册的服务...");
            var registeredServices = new[]
            {
                typeof(ILogService),
                typeof(IErrorHandlerService),
                typeof(IEditorFactory),
                typeof(IValidationService),
                typeof(IDataBindingService)
            };
            
            foreach (var serviceType in registeredServices)
            {
                var service = serviceProvider.GetService(serviceType);
                Console.WriteLine($"   {serviceType.Name}: {(service != null ? "已注册" : "未注册")}");
            }
            
            Console.WriteLine("=== 测试完成 ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 错误: {ex.GetType().Name}");
            Console.WriteLine($"   消息: {ex.Message}");
            Console.WriteLine($"   堆栈: {ex.StackTrace}");
        }
    }
}