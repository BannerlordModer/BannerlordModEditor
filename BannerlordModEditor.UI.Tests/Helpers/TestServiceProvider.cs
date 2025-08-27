using Microsoft.Extensions.DependencyInjection;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.Tests.Extensions;
using System;
using System.Linq;

namespace BannerlordModEditor.UI.Tests.Helpers;

/// <summary>
/// 测试辅助类，提供依赖注入服务
/// 
/// 这个类为测试项目提供统一的依赖注入配置，确保所有测试都能获得一致的依赖服务。
/// 主要功能：
/// - 注册所有核心服务（日志、错误处理、验证等）
/// - 注册所有编辑器ViewModel
/// - 注册模拟工厂和服务
/// - 提供服务重置功能以确保测试隔离
/// 
/// 使用方式：
/// <code>
/// // 获取服务实例
/// var viewModel = TestServiceProvider.GetService<SkillEditorViewModel>();
/// 
/// // 重置服务提供者（在测试之间调用）
/// TestServiceProvider.Reset();
/// </code>
/// </summary>
public class TestServiceProvider
{
    private static IServiceProvider? _serviceProvider;
    private static readonly object _lock = new object();

    /// <summary>
    /// 获取服务提供者实例
    /// </summary>
    /// <returns>配置好的服务提供者</returns>
    /// <remarks>
    /// 使用双重检查锁定模式确保线程安全和性能
    /// </remarks>
    public static IServiceProvider GetServiceProvider()
    {
        if (_serviceProvider == null)
        {
            lock (_lock)
            {
                if (_serviceProvider == null)
                {
                    _serviceProvider = CreateServiceProvider();
                }
            }
        }
        
        return _serviceProvider;
    }

    /// <summary>
    /// 创建并配置服务提供者
    /// </summary>
    /// <returns>配置好的服务提供者</returns>
    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddTestServices();
        return services.BuildServiceProvider();
    }

    
    /// <summary>
    /// 获取指定类型的服务实例
    /// </summary>
    /// <typeparam name="T">服务类型</typeparam>
    /// <returns>服务实例</returns>
    /// <exception cref="InvalidOperationException">当服务未注册时抛出</exception>
    public static T GetService<T>() where T : notnull
    {
        var serviceProvider = GetServiceProvider();
        try
        {
            return serviceProvider.GetRequiredService<T>();
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(
                $"无法获取服务 '{typeof(T).Name}'。请确保该服务已在TestServiceProvider中正确注册。", 
                ex);
        }
    }

    /// <summary>
    /// 尝试获取指定类型的服务实例
    /// </summary>
    /// <typeparam name="T">服务类型</typeparam>
    /// <param name="service">获取到的服务实例，如果成功</param>
    /// <returns>如果成功获取服务返回true，否则返回false</returns>
    public static bool TryGetService<T>(out T? service) where T : notnull
    {
        var serviceProvider = GetServiceProvider();
        service = serviceProvider.GetService<T>();
        return service != null;
    }

    /// <summary>
    /// 获取所有指定类型的服务实例
    /// </summary>
    /// <typeparam name="T">服务类型</typeparam>
    /// <returns>服务实例集合</returns>
    public static IEnumerable<T> GetServices<T>() where T : notnull
    {
        var serviceProvider = GetServiceProvider();
        return serviceProvider.GetServices<T>();
    }

    /// <summary>
    /// 创建新的作用域服务提供者
    /// </summary>
    /// <returns>新的作用域服务提供者</returns>
    /// <remarks>
    /// 用于需要测试作用域服务的场景
    /// </remarks>
    public static IServiceProvider CreateScope()
    {
        var serviceProvider = GetServiceProvider();
        return serviceProvider.CreateScope().ServiceProvider;
    }

    /// <summary>
    /// 重置服务提供者
    /// </summary>
    /// <remarks>
    /// 在测试开始时调用此方法可以确保测试之间的隔离性。
    /// 这会清除所有缓存的 singleton 服务。
    /// </remarks>
    public static void Reset()
    {
        lock (_lock)
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// 验证服务配置
    /// </summary>
    /// <returns>验证结果</returns>
    /// <remarks>
    /// 用于调试服务配置问题
    /// </remarks>
    public static bool ValidateConfiguration()
    {
        try
        {
            var serviceProvider = GetServiceProvider();
            return serviceProvider.ValidateServiceConfiguration();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"服务配置验证失败: {ex.Message}");
            return false;
        }
    }
}