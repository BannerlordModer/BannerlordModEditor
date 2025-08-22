namespace BannerlordModEditor.Common.Mappers;

/// <summary>
/// 通用映射器接口
/// </summary>
/// <typeparam name="TSource">源类型</typeparam>
/// <typeparam name="TTarget">目标类型</typeparam>
public interface IMapper<TSource, TTarget>
{
    /// <summary>
    /// 将源对象映射到目标对象
    /// </summary>
    TTarget Map(TSource source);

    /// <summary>
    /// 将目标对象映射回源对象
    /// </summary>
    TSource MapBack(TTarget target);
}

/// <summary>
/// 异步映射器接口
/// </summary>
/// <typeparam name="TSource">源类型</typeparam>
/// <typeparam name="TTarget">目标类型</typeparam>
public interface IAsyncMapper<TSource, TTarget>
{
    /// <summary>
    /// 异步将源对象映射到目标对象
    /// </summary>
    Task<TTarget> MapAsync(TSource source);

    /// <summary>
    /// 异步将目标对象映射回源对象
    /// </summary>
    Task<TSource> MapBackAsync(TTarget target);
}