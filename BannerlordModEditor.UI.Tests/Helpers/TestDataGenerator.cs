using BannerlordModEditor.UI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BannerlordModEditor.UI.Tests.Helpers;

/// <summary>
/// 测试数据生成器，用于创建各种测试用的数据和对象
/// </summary>
public static class TestDataGenerator
{
    /// <summary>
    /// 创建默认的EditorManagerOptions
    /// </summary>
    public static EditorManagerOptions CreateDefaultEditorManagerOptions()
    {
        return EditorManagerOptions.Default;
    }

    /// <summary>
    /// 创建完整的EditorManagerOptions（包含所有服务）
    /// </summary>
    public static EditorManagerOptions CreateFullEditorManagerOptions()
    {
        var serviceProvider = CreateTestServiceProvider();
        return EditorManagerOptions.ForDependencyInjection(serviceProvider);
    }

    /// <summary>
    /// 创建测试用的EditorManagerCreationOptions
    /// </summary>
    public static EditorManagerCreationOptions CreateEditorManagerCreationOptions(
        bool enablePerformanceMonitoring = false,
        bool enableHealthChecks = true,
        bool enableDiagnostics = false,
        bool enablePostCreationConfiguration = true,
        int creationTimeout = 30000)
    {
        return new EditorManagerCreationOptions
        {
            EnablePerformanceMonitoring = enablePerformanceMonitoring,
            EnableHealthChecks = enableHealthChecks,
            EnableDiagnostics = enableDiagnostics,
            EnablePostCreationConfiguration = enablePostCreationConfiguration,
            CreationTimeout = creationTimeout
        };
    }

    /// <summary>
    /// 创建测试用的EditorManagerServiceOptions
    /// </summary>
    public static EditorManagerServiceOptions CreateEditorManagerServiceOptions(
        bool useLogService = true,
        bool useErrorHandlerService = true,
        bool useValidationService = true,
        bool useDataBindingService = true,
        bool useEditorFactory = true,
        bool enablePerformanceMonitoring = false,
        bool enableHealthChecks = true,
        bool enableDiagnostics = false)
    {
        return new EditorManagerServiceOptions
        {
            UseLogService = useLogService,
            UseErrorHandlerService = useErrorHandlerService,
            UseValidationService = useValidationService,
            UseDataBindingService = useDataBindingService,
            UseEditorFactory = useEditorFactory,
            EnablePerformanceMonitoring = enablePerformanceMonitoring,
            EnableHealthChecks = enableHealthChecks,
            EnableDiagnostics = enableDiagnostics
        };
    }

    /// <summary>
    /// 创建模拟的日志服务
    /// </summary>
    public static Mock<ILogService> CreateMockLogService()
    {
        var mock = new Mock<ILogService>();
        mock.Setup(x => x.LogInfo(It.IsAny<string>(), It.IsAny<string>()));
        mock.Setup(x => x.LogDebug(It.IsAny<string>(), It.IsAny<string>()));
        mock.Setup(x => x.LogWarning(It.IsAny<string>(), It.IsAny<string>()));
        mock.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<string>()));
        mock.Setup(x => x.LogException(It.IsAny<Exception>(), It.IsAny<string>()));
        return mock;
    }

    /// <summary>
    /// 创建模拟的错误处理服务
    /// </summary>
    public static Mock<IErrorHandlerService> CreateMockErrorHandlerService()
    {
        var mock = new Mock<IErrorHandlerService>();
        mock.Setup(x => x.HandleError(It.IsAny<Exception>(), It.IsAny<string>()));
        mock.Setup(x => x.ShowErrorMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ErrorSeverity>()));
        return mock;
    }

    /// <summary>
    /// 创建模拟的验证服务
    /// </summary>
    public static Mock<IValidationService> CreateMockValidationService()
    {
        var mock = new Mock<IValidationService>();
        mock.Setup(x => x.Validate(It.IsAny<object>()))
            .Returns(new ValidationResult { IsValid = true });
        return mock;
    }

    /// <summary>
    /// 创建模拟的数据绑定服务
    /// </summary>
    public static Mock<IDataBindingService> CreateMockDataBindingService()
    {
        var mock = new Mock<IDataBindingService>();
        return mock;
    }

    /// <summary>
    /// 创建模拟的编辑器工厂
    /// </summary>
    public static Mock<IEditorFactory> CreateMockEditorFactory()
    {
        var mock = new Mock<IEditorFactory>();
        mock.Setup(x => x.GetRegisteredEditorTypes())
            .Returns(new[] { "AttributeEditor", "SkillEditor", "CombatParameterEditor" });
        mock.Setup(x => x.GetAllEditors())
            .Returns(Enumerable.Empty<ViewModelBase>());
        return mock;
    }

    /// <summary>
    /// 创建测试用的服务提供者
    /// </summary>
    public static IServiceProvider CreateTestServiceProvider()
    {
        var services = new ServiceCollection();
        
        // 添加模拟服务
        services.AddSingleton(CreateMockLogService().Object);
        services.AddSingleton(CreateMockErrorHandlerService().Object);
        services.AddSingleton(CreateMockValidationService().Object);
        services.AddSingleton(CreateMockDataBindingService().Object);
        services.AddSingleton(CreateMockEditorFactory().Object);
        
        return services.BuildServiceProvider();
    }

    /// <summary>
    /// 创建测试用的EditorCategoryViewModel列表
    /// </summary>
    public static List<EditorCategoryViewModel> CreateTestCategories()
    {
        return new List<EditorCategoryViewModel>
        {
            new EditorCategoryViewModel("角色设定", "角色设定编辑器", "👤")
            {
                Editors =
                {
                    new EditorItemViewModel("属性定义", "属性定义编辑器", "attributes.xml", "AttributeEditor", "⚙️"),
                    new EditorItemViewModel("技能系统", "技能系统编辑器", "skills.xml", "SkillEditor", "🎯"),
                    new EditorItemViewModel("骨骼体型", "骨骼体型编辑器", "bone_body_types.xml", "BoneBodyTypeEditor", "🦴")
                }
            },
            new EditorCategoryViewModel("装备物品", "装备物品编辑器", "⚔️")
            {
                Editors =
                {
                    new EditorItemViewModel("物品编辑", "物品编辑器", "items.xml", "ItemEditor", "📦")
                }
            },
            new EditorCategoryViewModel("战斗系统", "战斗系统编辑器", "🛡️")
            {
                Editors =
                {
                    new EditorItemViewModel("战斗参数", "战斗参数编辑器", "combat_parameters.xml", "CombatParameterEditor", "⚔️")
                }
            }
        };
    }

    /// <summary>
    /// 创建测试用的EditorItemViewModel
    /// </summary>
    public static EditorItemViewModel CreateTestEditorItem(
        string name = "测试编辑器",
        string description = "测试编辑器描述",
        string xmlFileName = "test.xml",
        string editorType = "TestEditor",
        string icon = "🧪")
    {
        return new EditorItemViewModel(name, description, xmlFileName, editorType, icon);
    }

    /// <summary>
    /// 创建测试用的异常
    /// </summary>
    public static Exception CreateTestException(string message = "测试异常")
    {
        return new Exception(message);
    }

    /// <summary>
    /// 创建测试用的EditorManagerCreationException
    /// </summary>
    public static EditorManagerCreationException CreateTestEditorManagerCreationException(
        string message = "测试创建异常",
        Exception? innerException = null)
    {
        return new EditorManagerCreationException(message, innerException);
    }

    /// <summary>
    /// 创建测试用的EditorManagerFactoryStatistics
    /// </summary>
    public static EditorManagerFactoryStatistics CreateTestStatistics(
        int instancesCreated = 5,
        DateTime? lastCreationTime = null,
        bool isCreationInProgress = false)
    {
        return new EditorManagerFactoryStatistics
        {
            InstancesCreated = instancesCreated,
            LastCreationTime = lastCreationTime ?? DateTime.Now,
            IsCreationInProgress = isCreationInProgress
        };
    }

    /// <summary>
    /// 创建测试用的ServiceRegistrationValidationResult
    /// </summary>
    public static ServiceRegistrationValidationResult CreateTestValidationResult(
        bool isValid = true,
        bool isLogServiceRegistered = true,
        bool isErrorHandlerServiceRegistered = true,
        bool isValidationServiceRegistered = true,
        bool isDataBindingServiceRegistered = true,
        bool isEditorFactoryRegistered = true,
        bool isEditorManagerFactoryRegistered = true,
        bool isEditorManagerViewModelRegistered = true)
    {
        return new ServiceRegistrationValidationResult
        {
            IsValid = isValid,
            IsLogServiceRegistered = isLogServiceRegistered,
            IsErrorHandlerServiceRegistered = isErrorHandlerServiceRegistered,
            IsValidationServiceRegistered = isValidationServiceRegistered,
            IsDataBindingServiceRegistered = isDataBindingServiceRegistered,
            IsEditorFactoryRegistered = isEditorFactoryRegistered,
            IsEditorManagerFactoryRegistered = isEditorManagerFactoryRegistered,
            IsEditorManagerViewModelRegistered = isEditorManagerViewModelRegistered
        };
    }

    /// <summary>
    /// 创建性能测试用的配置
    /// </summary>
    public static EditorManagerCreationOptions CreatePerformanceTestOptions()
    {
        return new EditorManagerCreationOptions
        {
            EnablePerformanceMonitoring = true,
            EnableHealthChecks = false, // 关闭健康检查以提高性能
            EnableDiagnostics = false, // 关闭诊断以提高性能
            EnablePostCreationConfiguration = false, // 关闭后配置以提高性能
            CreationTimeout = 5000 // 缩短超时时间
        };
    }

    /// <summary>
    /// 创建边界测试用的配置
    /// </summary>
    public static EditorManagerCreationOptions CreateBoundaryTestOptions()
    {
        return new EditorManagerCreationOptions
        {
            CreationTimeout = 1, // 极短超时时间
            EnableHealthChecks = true,
            EnablePostCreationConfiguration = true
        };
    }

    /// <summary>
    /// 创建压力测试用的配置
    /// </summary>
    public static EditorManagerCreationOptions CreateStressTestOptions()
    {
        return new EditorManagerCreationOptions
        {
            EnablePerformanceMonitoring = true,
            EnableHealthChecks = true,
            EnableDiagnostics = true,
            EnablePostCreationConfiguration = true,
            CreationTimeout = 60000 // 长超时时间
        };
    }
}