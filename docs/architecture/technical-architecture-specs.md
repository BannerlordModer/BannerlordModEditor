# 技术架构规范

## 1. 架构原则

### 1.1 设计原则

#### 单一职责原则 (SRP)
- 每个类应该只有一个改变的理由
- 服务、数据模型、UI组件应该职责明确
- 避免创建"万能类"

#### 开放封闭原则 (OCP)
- 对扩展开放，对修改封闭
- 使用接口和抽象类定义契约
- 通过依赖注入实现扩展

#### 里氏替换原则 (LSP)
- 子类应该能够替换父类
- 避免破坏性继承
- 使用组合优于继承

#### 接口隔离原则 (ISP)
- 使用专门的接口而不是通用接口
- 客户端不应该依赖不需要的方法
- 接口应该面向角色设计

#### 依赖倒置原则 (DIP)
- 高层模块不应该依赖低层模块
- 两者都应该依赖抽象
- 抽象不应该依赖细节

### 1.2 架构模式

#### 分层架构
```
┌─────────────────────────────────┐
│         表现层 (Presentation)    │
│  ┌─────────────────────────────┐ │
│  │         UI层 (Avalonia)      │ │
│  └─────────────────────────────┘ │
│  ┌─────────────────────────────┐ │
│  │      ViewModel层 (MVVM)      │ │
│  └─────────────────────────────┘ │
└─────────────────────────────────┘
┌─────────────────────────────────┐
│         应用层 (Application)     │
│  ┌─────────────────────────────┐ │
│  │        服务层 (Services)     │ │
│  └─────────────────────────────┘ │
│  ┌─────────────────────────────┐ │
│  │      工厂模式 (Factories)    │ │
│  └─────────────────────────────┘ │
└─────────────────────────────────┘
┌─────────────────────────────────┐
│         领域层 (Domain)          │
│  ┌─────────────────────────────┐ │
│  │    数据模型 (Data Models)    │ │
│  └─────────────────────────────┘ │
│  ┌─────────────────────────────┐ │
│  │    业务逻辑 (Business)      │ │
│  └─────────────────────────────┘ │
└─────────────────────────────────┘
┌─────────────────────────────────┐
│      基础设施层 (Infrastructure) │
│  ┌─────────────────────────────┐ │
│  │    数据访问 (Data Access)    │ │
│  └─────────────────────────────┘ │
│  ┌─────────────────────────────┐ │
│  │    外部服务 (External)       │ │
│  └─────────────────────────────┘ │
└─────────────────────────────────┘
```

#### 依赖注入模式
```csharp
// 服务注册
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBannerlordServices(this IServiceCollection services)
    {
        // 核心服务 - 单例
        services.AddSingleton<ILogService, LogService>();
        services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        
        // 工厂服务 - 单例
        services.AddSingleton<IEditorFactory, UnifiedEditorFactory>();
        services.AddSingleton<IViewModelFactory, ViewModelFactory>();
        
        // 业务服务 - 作用域
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<IDataBindingService, DataBindingService>();
        
        // 数据服务 - 瞬态
        services.AddTransient<IFileDiscoveryService, FileDiscoveryService>();
        
        return services;
    }
}
```

#### 工厂模式
```csharp
public interface IViewModelFactory
{
    TViewModel CreateViewModel<TViewModel>(params object[] parameters) 
        where TViewModel : ViewModelBase;
    
    TEditorViewModel CreateEditorViewModel<TEditorViewModel, TData, TItem>(
        string xmlFileName, string editorName)
        where TEditorViewModel : BaseEditorViewModel<TData, TItem>
        where TData : class, new()
        where TItem : class, new();
}
```

#### DO/DTO模式
```csharp
// 领域对象
public class AttributeDO : DomainObject
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int DefaultValue { get; set; }
}

// 数据传输对象
public class AttributeDTO : DataTransferObject
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int DefaultValue { get; set; }
}

// 映射器
public class AttributeMapper : BaseMapper<AttributeDO, AttributeDTO>
{
    public override AttributeDTO ToDto(AttributeDO domain)
    {
        return new AttributeDTO
        {
            Id = domain.Id,
            Name = domain.Name,
            Description = domain.Description,
            DefaultValue = domain.DefaultValue,
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt
        };
    }
    
    public override AttributeDO ToDomain(AttributeDTO dto)
    {
        return new AttributeDO
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            DefaultValue = dto.DefaultValue,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }
}
```

## 2. 技术栈规范

### 2.1 核心技术

#### .NET 9.0
- 使用最新的C# 13.0特性
- 启用Nullable引用类型
- 使用record类型进行数据建模
- 使用模式匹配和switch表达式

#### Avalonia UI 11.3
- 使用MVVM模式
- 使用数据绑定和命令
- 使用样式和主题
- 使用响应式设计

#### 依赖注入
- 使用Microsoft.Extensions.DependencyInjection
- 使用构造函数注入
- 避免服务定位器模式
- 明确服务生命周期

### 2.2 架构组件

#### 服务层
```csharp
// 服务接口
public interface IService
{
    Task InitializeAsync();
    Task ShutdownAsync();
}

// 基础服务类
public abstract class BaseService : IService
{
    protected readonly ILogService LogService;
    protected readonly IErrorHandlerService ErrorHandlerService;
    
    protected BaseService(ILogService logService, IErrorHandlerService errorHandlerService)
    {
        LogService = logService;
        ErrorHandlerService = errorHandlerService;
    }
    
    public virtual async Task InitializeAsync()
    {
        await OnInitializeAsync();
    }
    
    public virtual async Task ShutdownAsync()
    {
        await OnShutdownAsync();
    }
    
    protected virtual Task OnInitializeAsync() => Task.CompletedTask;
    protected virtual Task OnShutdownAsync() => Task.CompletedTask;
}
```

#### ViewModel层
```csharp
// ViewModel基类
public abstract class ViewModelBase : ObservableObject
{
    protected IErrorHandlerService ErrorHandler { get; }
    protected ILogService LogService { get; }

    public ViewModelBase(
        IErrorHandlerService? errorHandler = null,
        ILogService? logService = null)
    {
        ErrorHandler = errorHandler ?? new ErrorHandlerService();
        LogService = logService ?? new LogService();
    }
    
    protected async Task<bool> ExecuteSafelyAsync(Func<Task> action, string context = "")
    {
        try
        {
            await action();
            return true;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex, context);
            return false;
        }
    }
    
    protected async Task HandleErrorAsync(Exception exception, string context = "")
    {
        LogService.LogException(exception, context);
        await ErrorHandler.HandleExceptionAsync(exception, context);
    }
}

// 编辑器ViewModel基类
public abstract class BaseEditorViewModel<TData, TItem> : ViewModelBase
    where TData : class, new()
    where TItem : class, new()
{
    [ObservableProperty]
    private ObservableCollection<TItem> items = new();

    [ObservableProperty]
    private string filePath = string.Empty;

    [ObservableProperty]
    private bool hasUnsavedChanges;

    protected BaseEditorViewModel(string xmlFileName, string editorName,
        IErrorHandlerService? errorHandler = null,
        ILogService? logService = null)
        : base(errorHandler, logService)
    {
        XmlFileName = xmlFileName;
        EditorName = editorName;
    }
    
    // 抽象方法
    protected abstract TData LoadDataFromFile(string path);
    protected abstract void SaveDataToFile(TData data, string path);
    protected abstract IEnumerable<TItem> GetItemsFromData(TData data);
    protected abstract TData CreateDataFromItems(ObservableCollection<TItem> items);
    protected abstract TItem CreateNewItem();
    protected abstract TItem CreateErrorItem(string errorMessage);
    protected abstract bool MatchesSearchFilter(TItem item, string filter);
}
```

#### 数据层
```csharp
// 数据访问接口
public interface IDataRepository<T>
{
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
}

// XML序列化接口
public interface IXmlSerializer<T>
{
    Task<T> DeserializeAsync(string xmlContent);
    Task<T> DeserializeFromFileAsync(string filePath);
    Task<string> SerializeAsync(T obj);
    Task SerializeToFileAsync(T obj, string filePath);
    Task<bool> ValidateAsync(string xmlContent);
}

// 基础XML序列化器
public abstract class BaseXmlSerializer<T> : IXmlSerializer<T>
{
    protected readonly ILogService LogService;
    protected readonly IErrorHandlerService ErrorHandlerService;
    
    protected BaseXmlSerializer(ILogService logService, IErrorHandlerService errorHandlerService)
    {
        LogService = logService;
        ErrorHandlerService = errorHandlerService;
    }
    
    public virtual async Task<T> DeserializeAsync(string xmlContent)
    {
        try
        {
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xmlContent);
            var result = (T)serializer.Deserialize(reader);
            return result;
        }
        catch (Exception ex)
        {
            await ErrorHandlerService.HandleExceptionAsync(ex, $"Failed to deserialize {typeof(T).Name}");
            throw;
        }
    }
    
    // ... 其他方法实现
}
```

## 3. 编码规范

### 3.1 命名约定

#### 类和接口
- 使用PascalCase
- 接口以I开头
- 抽象类以Base开头
- 泛型参数使用T开头

```csharp
// 正确
public class UserService : BaseService
{
}

public interface IUserService
{
}

public abstract class BaseViewModel
{
}

public class Repository<T>
{
}

// 错误
public class user_service
{
}

public interface Iuserservice
{
}
```

#### 方法和属性
- 使用PascalCase
- 方法使用动词开头
- 属性使用名词
- 布尔属性使用Is/Has/Can开头

```csharp
// 正确
public class UserService
{
    public User GetById(int id) { }
    public bool IsAuthenticated { get; }
    public bool HasPermission(string permission) { }
    public bool CanAccess(string resource) { }
}

// 错误
public class UserService
{
    public user getbyid(int id) { }
    public bool authenticated { get; }
    public bool has_permission(string permission) { }
}
```

#### 变量和参数
- 使用camelCase
- 私有字段使用_开头
- 常量使用大写字母和下划线

```csharp
// 正确
public class UserService
{
    private readonly IUserRepository _userRepository;
    private const int MAX_RETRY_COUNT = 3;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public User GetUser(int userId)
    {
        var user = _userRepository.GetById(userId);
        return user;
    }
}

// 错误
public class UserService
{
    private readonly IUserRepository UserRepository;
    private const int MaxRetryCount = 3;
    
    public UserService(IUserRepository userRepository)
    {
        UserRepository = userRepository;
    }
}
```

### 3.2 代码组织

#### 文件组织
```
BannerlordModEditor/
├── BannerlordModEditor.Common/
│   ├── Models/
│   │   ├── DO/
│   │   ├── DTO/
│   │   └── Data/
│   ├── Services/
│   ├── Interfaces/
│   └── Mappers/
├── BannerlordModEditor.UI/
│   ├── ViewModels/
│   │   ├── Editors/
│   │   └── Base/
│   ├── Views/
│   │   ├── Editors/
│   │   └── Base/
│   ├── Services/
│   ├── Factories/
│   └── Controls/
└── BannerlordModEditor.Tests/
    ├── Unit/
    ├── Integration/
    └── Mocks/
```

#### 类组织
```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BannerlordModEditor.UI.ViewModels.Editors;

/// <summary>
/// 属性编辑器视图模型
/// </summary>
/// <remarks>
/// 这个类负责处理属性编辑的业务逻辑
/// </remarks>
public class AttributeEditorViewModel : BaseEditorViewModel<AttributeData, Attribute>
{
    // 字段
    private readonly IAttributeService _attributeService;
    
    // 属性
    [ObservableProperty]
    private string _searchText = string.Empty;
    
    // 构造函数
    public AttributeEditorViewModel(
        string xmlFileName,
        string editorName,
        IAttributeService attributeService,
        IErrorHandlerService? errorHandler = null,
        ILogService? logService = null)
        : base(xmlFileName, editorName, errorHandler, logService)
    {
        _attributeService = attributeService ?? throw new ArgumentNullException(nameof(attributeService));
    }
    
    // 公共方法
    public async Task LoadAttributesAsync()
    {
        await ExecuteSafelyAsync(async () =>
        {
            var attributes = await _attributeService.GetAllAsync();
            Items.Clear();
            
            foreach (var attribute in attributes)
            {
                Items.Add(attribute);
            }
        }, "LoadAttributesAsync");
    }
    
    // 私有方法
    private async Task ValidateAttributeAsync(Attribute attribute)
    {
        if (string.IsNullOrWhiteSpace(attribute.Name))
        {
            throw new ArgumentException("Attribute name cannot be empty");
        }
        
        if (attribute.DefaultValue < 0)
        {
            throw new ArgumentException("Attribute default value cannot be negative");
        }
    }
    
    // 重写方法
    protected override Attribute LoadDataFromFile(string path)
    {
        // 实现
        throw new NotImplementedException();
    }
    
    protected override void SaveDataToFile(AttributeData data, string path)
    {
        // 实现
        throw new NotImplementedException();
    }
    
    // ... 其他重写方法
}
```

### 3.3 异步编程

#### 异步方法规范
- 异步方法以Async结尾
- 使用async/await模式
- 避免async void（除了事件处理器）
- 使用ConfigureAwait(false)进行库代码

```csharp
// 正确
public class UserService
{
    public async Task<User> GetUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId).ConfigureAwait(false);
        return user;
    }
    
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync().ConfigureAwait(false);
        return users;
    }
}

// 错误
public class UserService
{
    public User GetUser(int userId)
    {
        return _userRepository.GetByIdAsync(userId).Result;
    }
    
    public async void LoadUsers()
    {
        var users = await _userRepository.GetAllAsync();
        // 错误：使用async void
    }
}
```

#### 异步异常处理
- 使用try-catch处理异步异常
- 使用AggregateException处理多个异常
- 使用Task.WhenAll进行并行操作

```csharp
public class UserService
{
    public async Task<bool> ProcessUsersAsync(IEnumerable<int> userIds)
    {
        try
        {
            var tasks = userIds.Select(id => ProcessUserAsync(id));
            await Task.WhenAll(tasks);
            return true;
        }
        catch (AggregateException ex)
        {
            foreach (var innerException in ex.InnerExceptions)
            {
                LogService.LogError($"Failed to process user: {innerException.Message}");
            }
            return false;
        }
        catch (Exception ex)
        {
            LogService.LogError($"Failed to process users: {ex.Message}");
            return false;
        }
    }
    
    private async Task ProcessUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        // 处理用户
    }
}
```

## 4. 测试规范

### 4.1 单元测试

#### 测试命名约定
- 使用描述性测试名称
- 使用Given-When-Then模式
- 测试类以Tests结尾

```csharp
[TestFixture]
public class UserServiceTests
{
    private IUserService _userService;
    private IUserRepository _userRepository;
    private Mock<ILogger> _mockLogger;
    
    [SetUp]
    public void SetUp()
    {
        _userRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger>();
        _userService = new UserService(_userRepository.Object, _mockLogger.Object);
    }
    
    [Test]
    public async Task GetUserAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var userId = 1;
        var expectedUser = new User { Id = userId, Name = "John Doe" };
        _userRepository.Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync(expectedUser);
        
        // Act
        var result = await _userService.GetUserAsync(userId);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(userId));
        Assert.That(result.Name, Is.EqualTo("John Doe"));
    }
    
    [Test]
    public async Task GetUserAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var userId = 999;
        _userRepository.Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync((User)null);
        
        // Act
        var result = await _userService.GetUserAsync(userId);
        
        // Assert
        Assert.That(result, Is.Null);
    }
}
```

#### Mock和Stub
- 使用Moq进行Mock
- 使用测试数据构建器
- 避免过度Mock

```csharp
public class UserTestDataBuilder
{
    private User _user = new User();
    
    public UserTestDataBuilder WithId(int id)
    {
        _user.Id = id;
        return this;
    }
    
    public UserTestDataBuilder WithName(string name)
    {
        _user.Name = name;
        return this;
    }
    
    public UserTestDataBuilderWithEmail(string email)
    {
        _user.Email = email;
        return this;
    }
    
    public User Build()
    {
        return _user;
    }
}

// 使用测试数据构建器
[Test]
public async Task CreateUserAsync_WithValidUser_ShouldCreateUser()
{
    // Arrange
    var userToCreate = new UserTestDataBuilder()
        .WithName("John Doe")
        .WithEmail("john@example.com")
        .Build();
    
    _userRepository.Setup(r => r.AddAsync(userToCreate))
        .ReturnsAsync(userToCreate);
    
    // Act
    var result = await _userService.CreateUserAsync(userToCreate);
    
    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.Name, Is.EqualTo("John Doe"));
    Assert.That(result.Email, Is.EqualTo("john@example.com"));
}
```

### 4.2 集成测试

#### 数据库测试
- 使用内存数据库
- 使用事务回滚
- 清理测试数据

```csharp
[TestFixture]
public class UserRepositoryIntegrationTests
{
    private AppDbContext _dbContext;
    private IUserRepository _userRepository;
    
    [SetUp]
    public async Task SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
            
        _dbContext = new AppDbContext(options);
        _userRepository = new UserRepository(_dbContext);
        
        // 添加测试数据
        await _dbContext.Users.AddRangeAsync(GetTestUsers());
        await _dbContext.SaveChangesAsync();
    }
    
    [TearDown]
    public async Task TearDown()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.DisposeAsync();
    }
    
    [Test]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Act
        var users = await _userRepository.GetAllAsync();
        
        // Assert
        Assert.That(users.Count(), Is.EqualTo(3));
    }
    
    private IEnumerable<User> GetTestUsers()
    {
        return new List<User>
        {
            new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
            new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" },
            new User { Id = 3, Name = "Bob Johnson", Email = "bob@example.com" }
        };
    }
}
```

#### API测试
- 使用测试服务器
- 测试HTTP状态码
- 验证响应内容

```csharp
[TestFixture]
public class UserControllerIntegrationTests
{
    private HttpClient _httpClient;
    private CustomWebApplicationFactory<Program> _factory;
    
    [SetUp]
    public void SetUp()
    {
        _factory = new CustomWebApplicationFactory<Program>();
        _httpClient = _factory.CreateClient();
    }
    
    [TearDown]
    public void TearDown()
    {
        _httpClient?.Dispose();
        _factory?.Dispose();
    }
    
    [Test]
    public async Task GetUser_WhenUserExists_ShouldReturnOk()
    {
        // Arrange
        var userId = 1;
        
        // Act
        var response = await _httpClient.GetAsync($"/api/users/{userId}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var user = JsonConvert.DeserializeObject<User>(content);
        
        Assert.That(user, Is.Not.Null);
        Assert.That(user.Id, Is.EqualTo(userId));
    }
}
```

## 5. 性能优化

### 5.1 内存管理

#### 使用IDisposable
```csharp
public class DataService : IDisposable
{
    private readonly IDbConnection _dbConnection;
    private bool _disposed;
    
    public DataService(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        using var command = _dbConnection.CreateCommand();
        command.CommandText = "SELECT * FROM Users";
        
        using var reader = await command.ExecuteReaderAsync();
        var users = new List<User>();
        
        while (await reader.ReadAsync())
        {
            users.Add(MapToUser(reader));
        }
        
        return users;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbConnection?.Dispose();
            }
            _disposed = true;
        }
    }
    
    ~DataService()
    {
        Dispose(false);
    }
}
```

#### 避免内存泄漏
```csharp
public class EventManager : IDisposable
{
    private readonly List<IDisposable> _subscriptions = new();
    private bool _disposed;
    
    public void Subscribe<T>(IObservable<T> source, Action<T> onNext)
    {
        var subscription = source.Subscribe(onNext);
        _subscriptions.Add(subscription);
    }
    
    public void Dispose()
    {
        if (!_disposed)
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }
            _subscriptions.Clear();
            _disposed = true;
        }
    }
}
```

### 5.2 异步性能

#### 使用ValueTask
```csharp
public class CacheService
{
    private readonly ConcurrentDictionary<string, object> _cache = new();
    
    public ValueTask<T> GetAsync<T>(string key)
    {
        if (_cache.TryGetValue(key, out var value))
        {
            return new ValueTask<T>((T)value);
        }
        
        return new ValueTask<T>(GetFromSourceAsync(key));
    }
    
    private async Task<T> GetFromSourceAsync<T>(string key)
    {
        var value = await LoadFromDatabaseAsync(key);
        _cache[key] = value;
        return value;
    }
}
```

#### 使用Parallel.ForEachAsync
```csharp
public class DataProcessor
{
    public async Task ProcessItemsAsync(IEnumerable<DataItem> items)
    {
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };
        
        await Parallel.ForEachAsync(items, options, async (item, ct) =>
        {
            await ProcessItemAsync(item, ct);
        });
    }
    
    private async Task ProcessItemAsync(DataItem item, CancellationToken ct)
    {
        // 处理单个项目
    }
}
```

### 5.3 缓存策略

#### 内存缓存
```csharp
public class CachedUserService : IUserService
{
    private readonly IUserService _userService;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);
    
    public CachedUserService(IUserService userService, IMemoryCache cache)
    {
        _userService = userService;
        _cache = cache;
    }
    
    public async Task<User> GetUserAsync(int userId)
    {
        var cacheKey = $"user_{userId}";
        
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
            return await _userService.GetUserAsync(userId);
        });
    }
}
```

#### 分布式缓存
```csharp
public class DistributedCacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<DistributedCacheService> _logger;
    
    public DistributedCacheService(IDistributedCache cache, ILogger<DistributedCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }
    
    public async Task<T> GetAsync<T>(string key)
    {
        try
        {
            var value = await _cache.GetAsync(key);
            if (value == null)
            {
                return default(T);
            }
            
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get value from cache");
            return default(T);
        }
    }
    
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var options = new DistributedCacheEntryOptions();
            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration;
            }
            
            var serializedValue = JsonConvert.SerializeObject(value);
            await _cache.SetAsync(key, Encoding.UTF8.GetBytes(serializedValue), options);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set value in cache");
        }
    }
}
```

## 6. 安全规范

### 6.1 输入验证

#### 数据注解验证
```csharp
public class CreateUserRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character")]
    public string Password { get; set; }
}
```

#### Fluent验证
```csharp
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(2, 100)
            .WithMessage("Name must be between 2 and 100 characters");
            
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email format");
            
        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(8, 100)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number and one special character");
    }
}
```

### 6.2 权限控制

#### 基于角色的授权
```csharp
[Authorize(Roles = "Admin")]
public class UserController : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "User.Read")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }
    
    [HttpPost]
    [Authorize(Policy = "User.Create")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var user = await _userService.CreateAsync(request);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }
}
```

#### 基于策略的授权
```csharp
public class MinimumAgeRequirement : IAuthorizationRequirement
{
    public int MinimumAge { get; }
    
    public MinimumAgeRequirement(int minimumAge)
    {
        MinimumAge = minimumAge;
    }
}

public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        var dateOfBirthClaim = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth);
        
        if (dateOfBirthClaim == null)
        {
            return Task.CompletedTask;
        }
        
        var dateOfBirth = Convert.ToDateTime(dateOfBirthClaim.Value);
        int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
        if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
        {
            calculatedAge--;
        }
        
        if (calculatedAge >= requirement.MinimumAge)
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}
```

### 6.3 数据加密

#### 密码哈希
```csharp
public class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 10000;
    
    public string HashPassword(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            SaltSize,
            Iterations,
            HashAlgorithmName.SHA256);
        
        var key = algorithm.GetBytes(HashSize);
        var salt = algorithm.Salt;
        
        return Convert.ToBase64String(salt.Concat(key).ToArray());
    }
    
    public bool VerifyPassword(string password, string hash)
    {
        try
        {
            var hashBytes = Convert.FromBase64String(hash);
            var salt = hashBytes.Take(SaltSize).ToArray();
            var key = hashBytes.Skip(SaltSize).ToArray();
            
            using var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256);
            
            var keyToCheck = algorithm.GetBytes(HashSize);
            return key.SequenceEqual(keyToCheck);
        }
        catch
        {
            return false;
        }
    }
}
```

#### 数据加密
```csharp
public class DataEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;
    
    public DataEncryptionService(string key, string iv)
    {
        _key = Convert.FromBase64String(key);
        _iv = Convert.FromBase64String(iv);
    }
    
    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        
        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);
        
        sw.Write(plainText);
        sw.Flush();
        cs.FlushFinalBlock();
        
        return Convert.ToBase64String(ms.ToArray());
    }
    
    public string Decrypt(string cipherText)
    {
        try
        {
            var cipherBytes = Convert.FromBase64String(cipherText);
            
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            
            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            
            return sr.ReadToEnd();
        }
        catch
        {
            throw new CryptographicException("Failed to decrypt data");
        }
    }
}
```

## 7. 日志和监控

### 7.1 结构化日志

#### 日志配置
```csharp
public static class LoggingExtensions
{
    public static ILoggingBuilder AddStructuredLogging(this ILoggingBuilder builder)
    {
        builder.AddJsonConsole(options =>
        {
            options.IncludeScopes = true;
            options.TimestampFormat = "yyyy-MM-dd HH:mm:ss";
            options.JsonWriterOptions = new JsonWriterOptions
            {
                Indented = true
            };
        });
        
        builder.AddFile("logs/app.log", options =>
        {
            options.RollingFileSize = 10 * 1024 * 1024; // 10MB
            options.RetainedFileCountLimit = 5;
            options.FileSizeLimitBytes = 10 * 1024 * 1024; // 10MB
        });
        
        return builder;
    }
}
```

#### 日志使用
```csharp
public class UserService
{
    private readonly ILogger<UserService> _logger;
    
    public UserService(ILogger<UserService> logger)
    {
        _logger = logger;
    }
    
    public async Task<User> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            _logger.LogInformation("Creating user with email {Email}", request.Email);
            
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow
            };
            
            await _userRepository.AddAsync(user);
            
            _logger.LogInformation("User created successfully with ID {UserId}", user.Id);
            
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user with email {Email}", request.Email);
            throw;
        }
    }
}
```

### 7.2 性能监控

#### 性能计数器
```csharp
public class PerformanceMonitor
{
    private readonly ILogger<PerformanceMonitor> _logger;
    private readonly ConcurrentDictionary<string, Stopwatch> _operations = new();
    
    public PerformanceMonitor(ILogger<PerformanceMonitor> logger)
    {
        _logger = logger;
    }
    
    public void StartOperation(string operationName)
    {
        var stopwatch = Stopwatch.StartNew();
        _operations[operationName] = stopwatch;
        
        _logger.LogDebug("Started operation {OperationName}", operationName);
    }
    
    public void EndOperation(string operationName)
    {
        if (_operations.TryRemove(operationName, out var stopwatch))
        {
            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;
            
            _logger.LogInformation("Operation {OperationName} completed in {ElapsedMs}ms", 
                operationName, elapsed);
        }
    }
    
    public async Task<T> MeasureAsync<T>(string operationName, Func<Task<T>> operation)
    {
        StartOperation(operationName);
        
        try
        {
            var result = await operation();
            EndOperation(operationName);
            return result;
        }
        catch
        {
            EndOperation(operationName);
            throw;
        }
    }
}
```

#### 健康检查
```csharp
public class HealthCheckService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<HealthCheckService> _logger;
    
    public HealthCheckService(IServiceProvider serviceProvider, ILogger<HealthCheckService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task<HealthReport> CheckHealthAsync()
    {
        var results = new List<HealthCheckResult>();
        
        // 检查数据库连接
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.CanConnectAsync();
            results.Add(new HealthCheckResult("Database", HealthStatus.Healthy, "Database connection successful"));
        }
        catch (Exception ex)
        {
            results.Add(new HealthCheckResult("Database", HealthStatus.Unhealthy, $"Database connection failed: {ex.Message}"));
        }
        
        // 检查外部服务
        try
        {
            var externalService = _serviceProvider.GetRequiredService<IExternalService>();
            await externalService.CheckHealthAsync();
            results.Add(new HealthCheckResult("ExternalService", HealthStatus.Healthy, "External service is healthy"));
        }
        catch (Exception ex)
        {
            results.Add(new HealthCheckResult("ExternalService", HealthStatus.Unhealthy, $"External service is unhealthy: {ex.Message}"));
        }
        
        var overallStatus = results.All(r => r.Status == HealthStatus.Healthy) 
            ? HealthStatus.Healthy 
            : HealthStatus.Unhealthy;
            
        return new HealthReport(overallStatus, results);
    }
}
```

## 8. 部署和运维

### 8.1 容器化部署

#### Dockerfile
```dockerfile
# 构建阶段
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# 复制项目文件
COPY *.csproj ./
RUN dotnet restore

# 复制源代码
COPY . .
WORKDIR /app
RUN dotnet publish -c Release -o /app/publish

# 运行阶段
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# 设置环境变量
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# 暴露端口
EXPOSE 8080

# 健康检查
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# 启动应用
ENTRYPOINT ["dotnet", "BannerlordModEditor.UI.dll"]
```

#### docker-compose.yml
```yaml
version: '3.8'

services:
  app:
    build: .
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Host=db;Database=BannerlordModEditor;Username=app;Password=securepassword
    depends_on:
      - db
      - redis
    networks:
      - app-network

  db:
    image: postgres:15
    environment:
      - POSTGRES_DB=BannerlordModEditor
      - POSTGRES_USER=app
      - POSTGRES_PASSWORD=securepassword
    volumes:
      - postgres_data:/var/lib/postgresql/data
    networks:
      - app-network

  redis:
    image: redis:7-alpine
    networks:
      - app-network

volumes:
  postgres_data:

networks:
  app-network:
    driver: bridge
```

### 8.2 CI/CD配置

#### GitHub Actions
```yaml
name: CI/CD Pipeline

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 9.0.x
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Run tests
      run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"
    
    - name: Upload coverage to Codecov
      uses: codecov/codecov-action@v3
      with:
        file: ./coverage.xml

  build:
    needs: test
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 9.0.x
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --configuration Release
    
    - name: Publish
      run: dotnet publish --configuration Release --output ./publish
    
    - name: Build and push Docker image
      run: |
        echo ${{ secrets.DOCKER_PASSWORD }} | docker login -u ${{ secrets.DOCKER_USERNAME }} --password-stdin
        docker build -t bannerlord-mod-editor:${{ github.sha }} .
        docker push bannerlord-mod-editor:${{ github.sha }}

  deploy:
    needs: build
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    
    steps:
    - uses: actions/checkout@v4
    
    - name: Deploy to production
      run: |
        # 部署到生产环境的脚本
        echo "Deploying to production..."
```

### 8.3 监控和告警

#### 应用性能监控
```csharp
public class ApmService
{
    private readonly ILogger<ApmService> _logger;
    private readonly ConcurrentDictionary<string, MetricData> _metrics = new();
    
    public ApmService(ILogger<ApmService> logger)
    {
        _logger = logger;
    }
    
    public void RecordMetric(string metricName, double value, Dictionary<string, string> tags = null)
    {
        var key = $"{metricName}_{string.Join("_", tags?.Select(t => $"{t.Key}={t.Value}") ?? Array.Empty<string>())}";
        
        _metrics.AddOrUpdate(key, 
            new MetricData { Name = metricName, Value = value, Tags = tags ?? new Dictionary<string, string>() },
            (k, v) => new MetricData { Name = metricName, Value = value, Tags = tags ?? new Dictionary<string, string>() });
        
        _logger.LogDebug("Recorded metric {MetricName} with value {Value}", metricName, value);
    }
    
    public void IncrementCounter(string counterName, Dictionary<string, string> tags = null)
    {
        var key = $"{counterName}_{string.Join("_", tags?.Select(t => $"{t.Key}={t.Value}") ?? Array.Empty<string>())}";
        
        _metrics.AddOrUpdate(key,
            new MetricData { Name = counterName, Value = 1, Tags = tags ?? new Dictionary<string, string>() },
            (k, v) => new MetricData { Name = counterName, Value = v.Value + 1, Tags = tags ?? new Dictionary<string, string>() });
    }
    
    public async Task ExportMetricsAsync()
    {
        try
        {
            // 导出指标到监控系统
            foreach (var metric in _metrics.Values)
            {
                await SendToMonitoringSystem(metric);
            }
            
            _metrics.Clear();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export metrics");
        }
    }
    
    private async Task SendToMonitoringSystem(MetricData metric)
    {
        // 发送到监控系统的实现
        await Task.CompletedTask;
    }
}
```

#### 错误跟踪
```csharp
public class ErrorTrackingService
{
    private readonly ILogger<ErrorTrackingService> _logger;
    private readonly IErrorTrackingClient _errorTrackingClient;
    
    public ErrorTrackingService(ILogger<ErrorTrackingService> logger, IErrorTrackingClient errorTrackingClient)
    {
        _logger = logger;
        _errorTrackingClient = errorTrackingClient;
    }
    
    public async Task TrackErrorAsync(Exception exception, Dictionary<string, object> context = null)
    {
        try
        {
            var errorReport = new ErrorReport
            {
                Exception = exception,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Context = context ?? new Dictionary<string, object>(),
                Timestamp = DateTime.UtcNow,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
            };
            
            await _errorTrackingClient.SendErrorReportAsync(errorReport);
            
            _logger.LogError(exception, "Error tracked: {ErrorMessage}", exception.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to track error");
        }
    }
    
    public async Task TrackCustomErrorAsync(string message, Dictionary<string, object> context = null)
    {
        try
        {
            var errorReport = new ErrorReport
            {
                Message = message,
                Context = context ?? new Dictionary<string, object>(),
                Timestamp = DateTime.UtcNow,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
            };
            
            await _errorTrackingClient.SendErrorReportAsync(errorReport);
            
            _logger.LogError("Custom error tracked: {ErrorMessage}", message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to track custom error");
        }
    }
}
```

## 9. 总结

本技术架构规范为Bannerlord Mod Editor项目提供了全面的技术指导和最佳实践。通过遵循这些规范，我们可以：

1. **提高代码质量**：统一的编码规范和架构模式确保代码的一致性和可维护性
2. **增强可测试性**：依赖注入和Mock支持使代码更易于测试
3. **提升性能**：性能优化和监控确保应用程序的高效运行
4. **保障安全**：安全规范和最佳实践保护应用程序和数据安全
5. **简化部署**：容器化和CI/CD配置简化部署流程
6. **便于维护**：结构化日志和监控便于问题诊断和维护

这些规范应该作为团队的技术标准，并在项目开发过程中严格遵守。随着项目的发展，这些规范也应该定期审查和更新，以适应新的技术和需求。