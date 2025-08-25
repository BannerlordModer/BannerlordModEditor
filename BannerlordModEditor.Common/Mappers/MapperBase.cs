using System.Collections.ObjectModel;

namespace BannerlordModEditor.Common.Mappers;

/// <summary>
/// 映射器基类，提供通用的映射功能
/// </summary>
/// <typeparam name="TSource">源类型</typeparam>
/// <typeparam name="TTarget">目标类型</typeparam>
public abstract class MapperBase<TSource, TTarget> : IMapper<TSource, TTarget>
{
    /// <summary>
    /// 将源对象映射到目标对象
    /// </summary>
    public abstract TTarget Map(TSource source);

    /// <summary>
    /// 将目标对象映射回源对象
    /// </summary>
    public abstract TSource MapBack(TTarget target);

    /// <summary>
    /// 批量映射源对象到目标对象
    /// </summary>
    public ObservableCollection<TTarget> MapCollection(ObservableCollection<TSource> sourceCollection)
    {
        if (sourceCollection == null)
            return new ObservableCollection<TTarget>();

        var result = new ObservableCollection<TTarget>();
        foreach (var item in sourceCollection)
        {
            result.Add(Map(item));
        }
        return result;
    }

    /// <summary>
    /// 批量映射目标对象回源对象
    /// </summary>
    public ObservableCollection<TSource> MapBackCollection(ObservableCollection<TTarget> targetCollection)
    {
        if (targetCollection == null)
            return new ObservableCollection<TSource>();

        var result = new ObservableCollection<TSource>();
        foreach (var item in targetCollection)
        {
            result.Add(MapBack(item));
        }
        return result;
    }

    /// <summary>
    /// 安全映射单个对象，处理null情况
    /// </summary>
    protected TTarget SafeMap(TSource? source)
    {
        return source != null ? Map(source) : default(TTarget)!;
    }

    /// <summary>
    /// 安全反向映射单个对象，处理null情况
    /// </summary>
    protected TSource SafeMapBack(TTarget? target)
    {
        return target != null ? MapBack(target) : default(TSource)!;
    }
}

/// <summary>
/// 列表映射器基类
/// </summary>
/// <typeparam name="TSource">源类型</typeparam>
/// <typeparam name="TTarget">目标类型</typeparam>
public abstract class ListMapperBase<TSource, TTarget> : IMapper<List<TSource>, List<TTarget>>
{
    public abstract List<TTarget> Map(List<TSource> source);
    public abstract List<TSource> MapBack(List<TTarget> target);

    /// <summary>
    /// 安全映射列表，处理null情况
    /// </summary>
    protected List<TTarget> SafeMap(List<TSource>? source)
    {
        return source != null ? Map(source) : new List<TTarget>();
    }

    /// <summary>
    /// 安全反向映射列表，处理null情况
    /// </summary>
    protected List<TSource> SafeMapBack(List<TTarget>? target)
    {
        return target != null ? MapBack(target) : new List<TSource>();
    }
}