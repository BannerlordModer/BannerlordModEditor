# API规范和接口设计

## API设计原则

### 1. 一致性
- 所有API遵循一致的命名约定
- 参数类型和返回值类型保持一致
- 错误处理机制统一
- 接口版本控制

### 2. 可扩展性
- 支持插件化扩展
- 接口设计考虑未来需求
- 向后兼容性保证
- 配置驱动的功能

### 3. 类型安全
- 泛型约束确保类型安全
- 明确的接口定义
- 强类型的参数和返回值
- 编译时错误检查

### 4. 性能
- 异步操作支持
- 内存使用优化
- 批量处理支持
- 延迟加载机制

## 编辑器接口定义

### IEditorViewModel
```csharp
using CommunityToolkit.Mvvm.ComponentModel;

/// <summary>
/// 编辑器视图模型基础接口
/// </summary>
public interface IEditorViewModel
{
    /// <summary>
    /// XML文件名
    /// </summary>
    string XmlFileName { get; set; }
    
    /// <summary>
    /// 编辑器标题
    /// </summary>
    string Title { get; }
    
    /// <summary>
    /// 编辑器描述
    /// </summary>
    string Description { get; }
    
    /// <summary>
    /// 是否正在加载
    /// </summary>
    bool IsLoading { get; }
    
    /// <summary>
    /// 是否有未保存的更改
    /// </summary>
    bool HasUnsavedChanges { get; }
    
    /// <summary>
    /// 状态消息
    /// </summary>
    string StatusMessage { get; }
    
    /// <summary>
    /// 加载XML文件
    /// </summary>
    /// <param name="fileName">XML文件名</param>
    /// <returns>异步任务</returns>
    Task LoadXmlFileAsync(string fileName);
    
    /// <summary>
    /// 保存XML文件
    /// </summary>
    /// <returns>异步任务</returns>
    Task SaveXmlFileAsync();
    
    /// <summary>
    /// 验证数据
    /// </summary>
    /// <returns>验证结果</returns>
    bool ValidateData();
    
    /// <summary>
    /// 刷新数据
    /// </summary>
    /// <returns>异步任务</returns>
    Task RefreshAsync();
    
    /// <summary>
    /// 撤销操作
    /// </summary>
    void Undo();
    
    /// <summary>
    /// 重做操作
    /// </summary>
    void Redo();
}
```

### IBaseEditorViewModel
```csharp
/// <summary>
/// 基础编辑器视图模型接口
/// </summary>
public interface IBaseEditorViewModel : IEditorViewModel
{
    /// <summary>
    /// 编辑器类型
    /// </summary>
    string EditorType { get; }
    
    /// <summary>
    /// 支持的文件扩展名
    /// </summary>
    IEnumerable<string> SupportedExtensions { get; }
    
    /// <summary>
    /// 是否支持批量操作
    /// </summary>
    bool SupportsBatchOperations { get; }
    
    /// <summary>
    /// 是否支持撤销/重做
    /// </summary>
    bool SupportsUndoRedo { get; }
    
    /// <summary>
    /// 获取编辑器统计信息
    /// </summary>
    /// <returns>统计信息</returns>
    EditorStatistics GetStatistics();
}
```

### IGenericEditorViewModel<T>
```csharp
/// <summary>
/// 通用编辑器视图模型接口
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public interface IGenericEditorViewModel<T> : IBaseEditorViewModel
    where T : class, new()
{
    /// <summary>
    /// 当前编辑的数据
    /// </summary>
    T? CurrentData { get; set; }
    
    /// <summary>
    /// 数据集合
    /// </summary>
    ObservableCollection<T> DataCollection { get; }
    
    /// <summary>
    /// 选中的项目
    /// </summary>
    T? SelectedItem { get; set; }
    
    /// <summary>
    /// 添加新项目
    /// </summary>
    /// <returns>新创建的项目</returns>
    T AddNewItem();
    
    /// <summary>
    /// 删除项目
    /// </summary>
    /// <param name="item">要删除的项目</param>
    void RemoveItem(T item);
    
    /// <summary>
    /// 复制项目
    /// </summary>
    /// <param name="item">要复制的项目</param>
    /// <returns>复制的项目</returns>
    T CopyItem(T item);
    
    /// <summary>
    /// 搜索项目
    /// </summary>
    /// <param name="searchTerm">搜索词</param>
    /// <returns>匹配的项目</returns>
    IEnumerable<T> SearchItems(string searchTerm);
}
```

## 服务接口定义

### IEditorService
```csharp
/// <summary>
/// 编辑器服务接口
/// </summary>
public interface IEditorService
{
    /// <summary>
    /// 获取所有可用的编辑器类型
    /// </summary>
    /// <returns>编辑器类型列表</returns>
    IEnumerable<string> GetAvailableEditorTypes();
    
    /// <summary>
    /// 获取编辑器信息
    /// </summary>
    /// <param name="editorType">编辑器类型</param>
    /// <returns>编辑器信息</returns>
    EditorInfo GetEditorInfo(string editorType);
    
    /// <summary>
    /// 检查是否支持指定文件类型
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <returns>是否支持</returns>
    bool IsFileSupported(string fileName);
    
    /// <summary>
    /// 推荐编辑器类型
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <returns>推荐的编辑器类型</returns>
    string? RecommendEditorType(string fileName);
    
    /// <summary>
    /// 验证编辑器数据
    /// </summary>
    /// <param name="editorType">编辑器类型</param>
    /// <param name="data">数据对象</param>
    /// <returns>验证结果</returns>
    ValidationResult ValidateEditorData(string editorType, object data);
    
    /// <summary>
    /// 获取编辑器配置
    /// </summary>
    /// <param name="editorType">编辑器类型</param>
    /// <returns>编辑器配置</returns>
    EditorConfiguration GetEditorConfiguration(string editorType);
    
    /// <summary>
    /// 保存编辑器配置
    /// </summary>
    /// <param name="editorType">编辑器类型</param>
    /// <param name="configuration">编辑器配置</param>
    void SaveEditorConfiguration(string editorType, EditorConfiguration configuration);
}
```

### IXmlProcessingService
```csharp
/// <summary>
/// XML处理服务接口
/// </summary>
public interface IXmlProcessingService
{
    /// <summary>
    /// 加载XML文件
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="filePath">文件路径</param>
    /// <returns>加载的数据</returns>
    Task<T> LoadXmlAsync<T>(string filePath) where T : class, new();
    
    /// <summary>
    /// 保存XML文件
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="data">数据对象</param>
    /// <param name="filePath">文件路径</param>
    /// <returns>异步任务</returns>
    Task SaveXmlAsync<T>(T data, string filePath) where T : class, new();
    
    /// <summary>
    /// 从字符串反序列化XML
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="xml">XML字符串</param>
    /// <returns>数据对象</returns>
    T DeserializeXml<T>(string xml) where T : class, new();
    
    /// <summary>
    /// 序列化对象为XML字符串
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="data">数据对象</param>
    /// <returns>XML字符串</returns>
    string SerializeXml<T>(T data) where T : class, new();
    
    /// <summary>
    /// 验证XML格式
    /// </summary>
    /// <param name="xml">XML字符串</param>
    /// <returns>验证结果</returns>
    ValidationResult ValidateXmlFormat(string xml);
    
    /// <summary>
    /// 比较两个XML字符串的结构
    /// </summary>
    /// <param name="xml1">第一个XML字符串</param>
    /// <param name="xml2">第二个XML字符串</param>
    /// <returns>比较结果</returns>
    XmlComparisonResult CompareXmlStructures(string xml1, string xml2);
    
    /// <summary>
    /// 格式化XML字符串
    /// </summary>
    /// <param name="xml">XML字符串</param>
    /// <param name="indentation">缩进设置</param>
    /// <returns>格式化的XML字符串</returns>
    string FormatXml(string xml, XmlFormattingOptions? options = null);
}
```

### IMappingService
```csharp
/// <summary>
/// 映射服务接口
/// </summary>
public interface IMappingService
{
    /// <summary>
    /// 将DO对象转换为DTO对象
    /// </summary>
    /// <typeparam name="TDO">DO类型</typeparam>
    /// <typeparam name="TDTO">DTO类型</typeparam>
    /// <param name="source">DO源对象</param>
    /// <returns>DTO目标对象</returns>
    TDTO ToDTO<TDO, TDTO>(TDO source)
        where TDO : class, new()
        where TDTO : class, new();
    
    /// <summary>
    /// 将DTO对象转换为DO对象
    /// </summary>
    /// <typeparam name="TDO">DO类型</typeparam>
    /// <typeparam name="TDTO">DTO类型</typeparam>
    /// <param name="source">DTO源对象</param>
    /// <returns>DO目标对象</returns>
    TDO ToDO<TDO, TDTO>(TDTO source)
        where TDO : class, new()
        where TDTO : class, new();
    
    /// <summary>
    /// 批量转换DO对象为DTO对象
    /// </summary>
    /// <typeparam name="TDO">DO类型</typeparam>
    /// <typeparam name="TDTO">DTO类型</typeparam>
    /// <param name="sources">DO源对象集合</param>
    /// <returns>DTO目标对象集合</returns>
    List<TDTO> ToDTO<TDO, TDTO>(IEnumerable<TDO> sources)
        where TDO : class, new()
        where TDTO : class, new();
    
    /// <summary>
    /// 批量转换DTO对象为DO对象
    /// </summary>
    /// <typeparam name="TDO">DO类型</typeparam>
    /// <typeparam name="TDTO">DTO类型</typeparam>
    /// <param name="sources">DTO源对象集合</param>
    /// <returns>DO目标对象集合</returns>
    List<TDO> ToDO<TDO, TDTO>(IEnumerable<TDTO> sources)
        where TDO : class, new()
        where TDTO : class, new();
    
    /// <summary>
    /// 注册自定义映射器
    /// </summary>
    /// <typeparam name="TDO">DO类型</typeparam>
    /// <typeparam name="TDTO">DTO类型</typeparam>
    /// <param name="mapper">映射器实例</param>
    void RegisterMapper<TDO, TDTO>(IMapper<TDO, TDTO> mapper)
        where TDO : class, new()
        where TDTO : class, new();
    
    /// <summary>
    /// 检查是否支持指定类型的映射
    /// </summary>
    /// <typeparam name="TDO">DO类型</typeparam>
    /// <typeparam name="TDTO">DTO类型</typeparam>
    /// <returns>是否支持</returns>
    bool SupportsMapping<TDO, TDTO>()
        where TDO : class, new()
        where TDTO : class, new();
}
```

### INavigationService
```csharp
/// <summary>
/// 导航服务接口
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// 导航到指定视图
    /// </summary>
    /// <param name="viewKey">视图键</param>
    /// <param name="parameter">导航参数</param>
    void NavigateTo(string viewKey, object? parameter = null);
    
    /// <summary>
    /// 导航到编辑器
    /// </summary>
    /// <param name="editorType">编辑器类型</param>
    /// <param name="fileName">文件名</param>
    void NavigateToEditor(string editorType, string fileName);
    
    /// <summary>
    /// 返回上一页
    /// </summary>
    void GoBack();
    
    /// <summary>
    /// 返回到主页
    /// </summary>
    void GoHome();
    
    /// <summary>
    /// 获取当前视图
    /// </summary>
    /// <returns>当前视图信息</returns>
    ViewInfo GetCurrentView();
    
    /// <summary>
    /// 获取导航历史
    /// </summary>
    /// <returns>导航历史记录</returns>
    IEnumerable<ViewInfo> GetNavigationHistory();
    
    /// <summary>
    /// 清除导航历史
    /// </summary>
    void ClearHistory();
}
```

## 数据绑定接口

### IDataBindingService
```csharp
/// <summary>
/// 数据绑定服务接口
/// </summary>
public interface IDataBindingService
{
    /// <summary>
    /// 绑定属性
    /// </summary>
    /// <param name="source">源对象</param>
    /// <param name="sourceProperty">源属性</param>
    /// <param name="target">目标对象</param>
    /// <param name="targetProperty">目标属性</param>
    /// <param name="bindingMode">绑定模式</param>
    /// <returns>绑定对象</returns>
    IBinding BindProperty(
        object source, 
        string sourceProperty, 
        object target, 
        string targetProperty, 
        BindingMode bindingMode = BindingMode.Default);
    
    /// <summary>
    /// 绑定命令
    /// </summary>
    /// <param name="source">源对象</param>
    /// <param name="commandName">命令名称</param>
    /// <param name="target">目标对象</param>
    /// <param name="targetMethod">目标方法</param>
    /// <returns>绑定对象</returns>
    IBinding BindCommand(
        object source, 
        string commandName, 
        object target, 
        string targetMethod);
    
    /// <summary>
    /// 绑定集合
    /// </summary>
    /// <param name="sourceCollection">源集合</param>
    /// <param name="targetCollection">目标集合</param>
    /// <returns>绑定对象</returns>
    IBinding BindCollection(
        INotifyCollectionChanged sourceCollection, 
        INotifyCollectionChanged targetCollection);
    
    /// <summary>
    /// 解除绑定
    /// </summary>
    /// <param name="binding">绑定对象</param>
    void Unbind(IBinding binding);
    
    /// <summary>
    /// 解除所有绑定
    /// </summary>
    void UnbindAll();
}
```

### IValidationService
```csharp
/// <summary>
/// 验证服务接口
/// </summary>
public interface IValidationService
{
    /// <summary>
    /// 验证对象
    /// </summary>
    /// <param name="obj">要验证的对象</param>
    /// <returns>验证结果</returns>
    ValidationResult ValidateObject(object obj);
    
    /// <summary>
    /// 验证属性
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="propertyName">属性名称</param>
    /// <returns>验证结果</returns>
    ValidationResult ValidateProperty(object obj, string propertyName);
    
    /// <summary>
    /// 验证规则
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="rules">验证规则</param>
    /// <returns>验证结果</returns>
    ValidationResult ValidateRules(object value, IEnumerable<IValidationRule> rules);
    
    /// <summary>
    /// 添加验证规则
    /// </summary>
    /// <param name="typeName">类型名称</param>
    /// <param name="propertyName">属性名称</param>
    /// <param name="rule">验证规则</param>
    void AddValidationRule(string typeName, string propertyName, IValidationRule rule);
    
    /// <summary>
    /// 移除验证规则
    /// </summary>
    /// <param name="typeName">类型名称</param>
    /// <param name="propertyName">属性名称</param>
    void RemoveValidationRule(string typeName, string propertyName);
}
```

## 事件和消息系统

### IEventBus
```csharp
/// <summary>
/// 事件总线接口
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    /// <param name="handler">事件处理器</param>
    /// <returns>订阅ID</returns>
    Guid Subscribe<T>(Action<T> handler);
    
    /// <summary>
    /// 取消订阅事件
    /// </summary>
    /// <param name="subscriptionId">订阅ID</param>
    void Unsubscribe(Guid subscriptionId);
    
    /// <summary>
    /// 发布事件
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    /// <param name="event">事件数据</param>
    void Publish<T>(T @event);
    
    /// <summary>
    /// 发布异步事件
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    /// <param name="event">事件数据</param>
    /// <returns>异步任务</returns>
    Task PublishAsync<T>(T @event);
    
    /// <summary>
    /// 清除所有订阅
    /// </summary>
    void ClearSubscriptions();
}
```

### INotificationService
```csharp
/// <summary>
/// 通知服务接口
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// 发送通知
    /// </summary>
    /// <param name="message">通知消息</param>
    /// <param name="type">通知类型</param>
    /// <param name="duration">显示持续时间</param>
    void SendNotification(string message, NotificationType type, TimeSpan? duration = null);
    
    /// <summary>
    /// 发送成功通知
    /// </summary>
    /// <param name="message">通知消息</param>
    /// <param name="duration">显示持续时间</param>
    void SendSuccess(string message, TimeSpan? duration = null);
    
    /// <summary>
    /// 发送错误通知
    /// </summary>
    /// <param name="message">通知消息</param>
    /// <param name="duration">显示持续时间</param>
    void SendError(string message, TimeSpan? duration = null);
    
    /// <summary>
    /// 发送警告通知
    /// </summary>
    /// <param name="message">通知消息</param>
    /// <param name="duration">显示持续时间</param>
    void SendWarning(string message, TimeSpan? duration = null);
    
    /// <summary>
    /// 发送信息通知
    /// </summary>
    /// <param name="message">通知消息</param>
    /// <param name="duration">显示持续时间</param>
    void SendInfo(string message, TimeSpan? duration = null);
    
    /// <summary>
    /// 注册通知处理器
    /// </summary>
    /// <param name="handler">通知处理器</param>
    /// <returns>处理器ID</returns>
    string RegisterHandler(Action<Notification> handler);
    
    /// <summary>
    /// 取消注册通知处理器
    /// </summary>
    /// <param name="handlerId">处理器ID</param>
    void UnregisterHandler(string handlerId);
    
    /// <summary>
    /// 清除所有通知
    /// </summary>
    void ClearNotifications();
}
```

## 插件化扩展接口

### IPluginManager
```csharp
/// <summary>
/// 插件管理器接口
/// </summary>
public interface IPluginManager
{
    /// <summary>
    /// 加载插件
    /// </summary>
    /// <param name="pluginPath">插件路径</param>
    /// <returns>加载结果</returns>
    PluginLoadResult LoadPlugin(string pluginPath);
    
    /// <summary>
    /// 卸载插件
    /// </summary>
    /// <param name="pluginId">插件ID</param>
    /// <returns>卸载结果</returns>
    PluginUnloadResult UnloadPlugin(string pluginId);
    
    /// <summary>
    /// 获取所有已加载的插件
    /// </summary>
    /// <returns>插件列表</returns>
    IEnumerable<IPlugin> GetLoadedPlugins();
    
    /// <summary>
    /// 获取插件信息
    /// </summary>
    /// <param name="pluginId">插件ID</param>
    /// <returns>插件信息</returns>
    PluginInfo? GetPluginInfo(string pluginId);
    
    /// <summary>
    /// 启用插件
    /// </summary>
    /// <param name="pluginId">插件ID</param>
    void EnablePlugin(string pluginId);
    
    /// <summary>
    /// 禁用插件
    /// </summary>
    /// <param name="pluginId">插件ID</param>
    void DisablePlugin(string pluginId);
    
    /// <summary>
    /// 检查插件是否已加载
    /// </summary>
    /// <param name="pluginId">插件ID</param>
    /// <returns>是否已加载</returns>
    bool IsPluginLoaded(string pluginId);
}
```

### IEditorPlugin
```csharp
/// <summary>
/// 编辑器插件接口
/// </summary>
public interface IEditorPlugin : IPlugin
{
    /// <summary>
    /// 支持的文件类型
    /// </summary>
    IEnumerable<string> SupportedFileTypes { get; }
    
    /// <summary>
    /// 编辑器类型
    /// </summary>
    string EditorType { get; }
    
    /// <summary>
    /// 创建编辑器视图模型
    /// </summary>
    /// <param name="serviceProvider">服务提供器</param>
    /// <returns>编辑器视图模型</returns>
    IEditorViewModel CreateEditorViewModel(IServiceProvider serviceProvider);
    
    /// <summary>
    /// 创建编辑器视图
    /// </summary>
    /// <param name="serviceProvider">服务提供器</param>
    /// <returns>编辑器视图</returns>
    object CreateEditorView(IServiceProvider serviceProvider);
    
    /// <summary>
    /// 获取编辑器配置
    /// </summary>
    /// <returns>编辑器配置</returns>
    EditorConfiguration GetConfiguration();
    
    /// <summary>
    /// 验证文件是否支持
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <returns>是否支持</returns>
    bool IsFileSupported(string fileName);
}
```

### IPlugin
```csharp
/// <summary>
/// 插件基础接口
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// 插件ID
    /// </summary>
    string Id { get; }
    
    /// <summary>
    /// 插件名称
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// 插件版本
    /// </summary>
    Version Version { get; }
    
    /// <summary>
    /// 插件描述
    /// </summary>
    string Description { get; }
    
    /// <summary>
    /// 插件作者
    /// </summary>
    string Author { get; }
    
    /// <summary>
    /// 插件依赖项
    /// </summary>
    IEnumerable<PluginDependency> Dependencies { get; }
    
    /// <summary>
    /// 初始化插件
    /// </summary>
    /// <param name="serviceProvider">服务提供器</param>
    void Initialize(IServiceProvider serviceProvider);
    
    /// <summary>
    /// 启动插件
    /// </summary>
    void Start();
    
    /// <summary>
    /// 停止插件
    /// </summary>
    void Stop();
    
    /// <summary>
    /// 卸载插件
    /// </summary>
    void Unload();
}
```

## 数据模型定义

### EditorInfo
```csharp
/// <summary>
/// 编辑器信息
/// </summary>
public class EditorInfo
{
    /// <summary>
    /// 编辑器类型
    /// </summary>
    public string EditorType { get; set; } = string.Empty;
    
    /// <summary>
    /// 编辑器名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 编辑器描述
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// 支持的文件扩展名
    /// </summary>
    public List<string> SupportedExtensions { get; set; } = new();
    
    /// <summary>
    /// 编辑器类别
    /// </summary>
    public string Category { get; set; } = string.Empty;
    
    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; } = string.Empty;
    
    /// <summary>
    /// 是否支持批量操作
    /// </summary>
    public bool SupportsBatchOperations { get; set; }
    
    /// <summary>
    /// 是否支持撤销/重做
    /// </summary>
    public bool SupportsUndoRedo { get; set; }
    
    /// <summary>
    /// 编辑器版本
    /// </summary>
    public Version Version { get; set; } = new Version(1, 0, 0);
}
```

### EditorConfiguration
```csharp
/// <summary>
/// 编辑器配置
/// </summary>
public class EditorConfiguration
{
    /// <summary>
    /// 编辑器类型
    /// </summary>
    public string EditorType { get; set; } = string.Empty;
    
    /// <summary>
    /// 默认视图模式
    /// </summary>
    public string DefaultViewMode { get; set; } = "Grid";
    
    /// <summary>
    /// 是否显示工具栏
    /// </summary>
    public bool ShowToolbar { get; set; } = true;
    
    /// <summary>
    /// 是否显示状态栏
    /// </summary>
    public bool ShowStatusBar { get; set; } = true;
    
    /// <summary>
    /// 自动保存间隔（秒）
    /// </summary>
    public int AutoSaveInterval { get; set; } = 300;
    
    /// <summary>
    /// 最大撤销步骤
    /// </summary>
    public int MaxUndoSteps { get; set; } = 50;
    
    /// <summary>
    /// 自定义设置
    /// </summary>
    public Dictionary<string, object> CustomSettings { get; set; } = new();
}
```

### ValidationResult
```csharp
/// <summary>
/// 验证结果
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// 是否验证成功
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// 错误信息
    /// </summary>
    public List<string> Errors { get; set; } = new();
    
    /// <summary>
    /// 警告信息
    /// </summary>
    public List<string> Warnings { get; set; } = new();
    
    /// <summary>
    /// 信息消息
    /// </summary>
    public List<string> Infos { get; set; } = new();
    
    /// <summary>
    /// 验证详情
    /// </summary>
    public Dictionary<string, ValidationDetail> Details { get; set; } = new();
}
```

### XmlComparisonResult
```csharp
/// <summary>
/// XML比较结果
/// </summary>
public class XmlComparisonResult
{
    /// <summary>
    /// 是否结构相等
    /// </summary>
    public bool IsStructurallyEqual { get; set; }
    
    /// <summary>
    /// 节点数量差异
    /// </summary>
    public int NodeCountDifference { get; set; }
    
    /// <summary>
    /// 属性数量差异
    /// </summary>
    public int AttributeCountDifference { get; set; }
    
    /// <summary>
    /// 属性值差异
    /// </summary>
    public List<XmlAttributeDifference> AttributeDifferences { get; set; } = new();
    
    /// <summary>
    /// 文本内容差异
    /// </summary>
    public List<XmlTextDifference> TextDifferences { get; set; } = new();
    
    /// <summary>
    /// 结构差异
    /// </summary>
    public List<XmlStructureDifference> StructureDifferences { get; set; } = new();
}
```

## 枚举类型

### BindingMode
```csharp
/// <summary>
/// 数据绑定模式
/// </summary>
public enum BindingMode
{
    /// <summary>
    /// 默认模式（根据属性类型决定）
    /// </summary>
    Default,
    
    /// <summary>
    /// 单向绑定（源到目标）
    /// </summary>
    OneWay,
    
    /// <summary>
    /// 双向绑定
    /// </summary>
    TwoWay,
    
    /// <summary>
    /// 单次绑定
    /// </summary>
    OneTime,
    
    /// <summary>
    /// 单向到源（目标到源）
    /// </summary>
    OneWayToSource
}
```

### NotificationType
```csharp
/// <summary>
/// 通知类型
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// 信息
    /// </summary>
    Info,
    
    /// <summary>
    /// 成功
    /// </summary>
    Success,
    
    /// <summary>
    /// 警告
    /// </summary>
    Warning,
    
    /// <summary>
    /// 错误
    /// </summary>
    Error
}
```

### EditorViewMode
```csharp
/// <summary>
/// 编辑器视图模式
/// </summary>
public enum EditorViewMode
{
    /// <summary>
    /// 网格视图
    /// </summary>
    Grid,
    
    /// <summary>
    /// 详细视图
    /// </summary>
    Details,
    
    /// <summary>
    /// 树形视图
    /// </summary>
    Tree,
    
    /// <summary>
    /// 分割视图
    /// </summary>
    Split,
    
    /// <summary>
    /// 自定义视图
    /// </summary>
    Custom
}
```

## 扩展方法

### ServiceCollectionExtensions
```csharp
/// <summary>
/// 服务集合扩展方法
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加编辑器服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddEditorServices(this IServiceCollection services)
    {
        services.AddSingleton<IEditorService, EditorService>();
        services.AddSingleton<IXmlProcessingService, XmlProcessingService>();
        services.AddSingleton<IMappingService, MappingService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDataBindingService, DataBindingService>();
        services.AddSingleton<IValidationService, ValidationService>();
        services.AddSingleton<IEventBus, EventBus>();
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<IPluginManager, PluginManager>();
        
        return services;
    }
    
    /// <summary>
    /// 添加编辑器工厂
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddEditorFactory(this IServiceCollection services)
    {
        services.AddSingleton<IEditorFactory, EditorFactory>();
        
        return services;
    }
    
    /// <summary>
    /// 添加编辑器插件支持
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddEditorPlugins(this IServiceCollection services)
    {
        services.AddSingleton<IPluginManager, PluginManager>();
        services.AddSingleton<IPluginLoader, PluginLoader>();
        
        return services;
    }
}
```

本API规范提供了完整的接口定义，支持编辑器的创建、管理、数据绑定、事件处理和插件化扩展。所有接口都遵循一致性、可扩展性和类型安全的设计原则，为Bannerlord Mod Editor提供了强大的架构基础。