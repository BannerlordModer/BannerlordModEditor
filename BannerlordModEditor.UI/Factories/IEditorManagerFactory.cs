using System.Threading;
using System.Threading.Tasks;
using BannerlordModEditor.UI.ViewModels;

namespace BannerlordModEditor.UI.Factories;

/// <summary>
/// EditorManagerViewModel工厂接口
/// 
/// 这个接口定义了创建EditorManagerViewModel实例的标准方法，
/// 为依赖注入提供了明确的契约。
/// 
/// 主要职责：
/// - 提供标准化的EditorManagerViewModel创建方法
/// - 支持同步和异步创建
/// - 提供性能监控和统计信息
/// - 确保线程安全的实例创建
/// </summary>
public interface IEditorManagerFactory
{
    /// <summary>
    /// 创建标准的EditorManagerViewModel实例
    /// </summary>
    /// <returns>配置完成的EditorManagerViewModel实例</returns>
    /// <remarks>
    /// 这个方法使用默认配置创建EditorManagerViewModel，适用于大多数场景
    /// </remarks>
    EditorManagerViewModel CreateEditorManager();

    /// <summary>
    /// 使用自定义选项创建EditorManagerViewModel实例
    /// </summary>
    /// <param name="options">创建选项</param>
    /// <returns>配置完成的EditorManagerViewModel实例</returns>
    EditorManagerViewModel CreateEditorManager(EditorManagerCreationOptions options);

    /// <summary>
    /// 异步创建EditorManagerViewModel实例
    /// </summary>
    /// <param name="options">创建选项（可选）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>异步创建的EditorManagerViewModel实例</returns>
    /// <remarks>
    /// 这个方法适用于需要异步初始化的场景，比如从配置文件加载设置
    /// </remarks>
    Task<EditorManagerViewModel> CreateEditorManagerAsync(
        EditorManagerCreationOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取工厂统计信息
    /// </summary>
    /// <returns>工厂统计信息</returns>
    EditorManagerFactoryStatistics GetStatistics();
}