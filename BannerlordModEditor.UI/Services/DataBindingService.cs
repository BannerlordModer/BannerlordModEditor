using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive;

namespace BannerlordModEditor.UI.Services;

/// <summary>
/// 数据绑定事件参数
/// </summary>
public class DataBindingEventArgs : EventArgs
{
    public string PropertyName { get; set; } = string.Empty;
    public object? OldValue { get; set; }
    public object? NewValue { get; set; }
    public bool IsChanging { get; set; }
}

/// <summary>
/// 数据绑定服务接口（简化版，不依赖System.Reactive）
/// </summary>
public interface IDataBindingService
{
    /// <summary>
    /// 创建双向数据绑定
    /// </summary>
    IDisposable CreateBinding<TSource, TTarget>(
        ObservableObject source,
        string sourceProperty,
        ObservableObject target,
        string targetProperty,
        bool twoWay = true);

    /// <summary>
    /// 创建集合绑定
    /// </summary>
    IDisposable CreateCollectionBinding<T>(
        ObservableCollection<T> source,
        ObservableCollection<T> target);

    /// <summary>
    /// 创建验证绑定
    /// </summary>
    IDisposable CreateValidationBinding(
        ObservableObject source,
        string propertyName,
        Action<List<string>> validationCallback);

    /// <summary>
    /// 批量更新属性
    /// </summary>
    void BatchUpdate(ObservableObject obj, Action updateAction);
}

/// <summary>
/// 数据绑定服务实现
/// </summary>
public class DataBindingService : IDataBindingService
{
    private readonly Dictionary<string, List< IDisposable>> _bindings = new();

    public IDisposable CreateBinding<TSource, TTarget>(
        ObservableObject source,
        string sourceProperty,
        ObservableObject target,
        string targetProperty,
        bool twoWay = true)
    {
        var disposables = new List<IDisposable>();

        // 检查null对象
        if (source == null || target == null)
        {
            return new CompositeDisposable(disposables);
        }

        // 源到目标的绑定
        var sourceHandler = new PropertyChangedEventHandler((s, e) =>
        {
            if (e.PropertyName == sourceProperty)
            {
                var sourceValue = GetPropertyValue(source, sourceProperty);
                SetPropertyValue(target, targetProperty, sourceValue);
            }
        });
        source.PropertyChanged += sourceHandler;
        disposables.Add(new DisposableAction(() => source.PropertyChanged -= sourceHandler));

        // 双向绑定
        if (twoWay)
        {
            var targetHandler = new PropertyChangedEventHandler((s, e) =>
            {
                if (e.PropertyName == targetProperty)
                {
                    var targetValue = GetPropertyValue(target, targetProperty);
                    SetPropertyValue(source, sourceProperty, targetValue);
                }
            });
            target.PropertyChanged += targetHandler;
            disposables.Add(new DisposableAction(() => target.PropertyChanged -= targetHandler));
        }

        // 创建复合可释放对象
        return new CompositeDisposable(disposables);
    }

    public IDisposable CreateCollectionBinding<T>(
        ObservableCollection<T> source,
        ObservableCollection<T> target)
    {
        var disposables = new List<IDisposable>();

        // 检查null集合
        if (source == null || target == null)
        {
            return new CompositeDisposable(disposables);
        }

        // 初始同步
        SyncCollections(source, target);

        // 监听源集合变化
        var handler = new NotifyCollectionChangedEventHandler((s, e) =>
        {
            SyncCollections(source, target);
        });
        source.CollectionChanged += handler;
        disposables.Add(new DisposableAction(() => source.CollectionChanged -= handler));

        return new CompositeDisposable(disposables);
    }

    public IDisposable CreateValidationBinding(
        ObservableObject source,
        string propertyName,
        Action<List<string>> validationCallback)
    {
        // 检查null参数
        if (source == null || validationCallback == null)
        {
            return new DisposableAction(() => { });
        }

        var handler = new PropertyChangedEventHandler((s, e) =>
        {
            if (e.PropertyName == propertyName)
            {
                // 简单的防抖处理
                Task.Delay(300).ContinueWith(_ =>
                {
                    var validationService = new ValidationService();
                    var errors = validationService.ValidateProperty(source, propertyName);
                    validationCallback(errors);
                });
            }
        });

        source.PropertyChanged += handler;

        return new DisposableAction(() => source.PropertyChanged -= handler);
    }

    public void BatchUpdate(ObservableObject obj, Action updateAction)
    {
        // 这里可以实现批量更新逻辑，比如暂停通知
        updateAction();
        // 更新完成后可以触发相关的属性变化通知
    }

    /// <summary>
    /// 观察属性变化（简化实现，返回一个简单的可观察对象）
    /// </summary>
    public IObservable<DataBindingEventArgs> ObservePropertyChanges(ObservableObject source, string propertyName)
    {
        return new PropertyChangeObservable(source, propertyName);
    }

    private void SyncCollections<T>(ObservableCollection<T> source, ObservableCollection<T> target)
    {
        // 清空目标集合
        target.Clear();

        // 复制所有元素
        foreach (var item in source)
        {
            target.Add(item);
        }
    }

    private object? GetPropertyValue(ObservableObject obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName)?.GetValue(obj);
    }

    private void SetPropertyValue(ObservableObject obj, string propertyName, object? value)
    {
        obj.GetType().GetProperty(propertyName)?.SetValue(obj, value);
    }
}

/// <summary>
/// 简单的可释放动作包装器
/// </summary>
public class DisposableAction : IDisposable
{
    private readonly Action _disposeAction;
    private bool _disposed;

    public DisposableAction(Action disposeAction)
    {
        _disposeAction = disposeAction ?? throw new ArgumentNullException(nameof(disposeAction));
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposeAction?.Invoke();
            _disposed = true;
        }
    }
}

/// <summary>
/// 复合可释放对象
/// </summary>
public class CompositeDisposable : IDisposable
{
    private readonly List<IDisposable> _disposables;
    private bool _disposed;

    public CompositeDisposable(List<IDisposable> disposables)
    {
        _disposables = disposables ?? new List<IDisposable>();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }
            _disposed = true;
        }
    }
}

/// <summary>
/// 简单的属性变化观察者实现
/// </summary>
public class PropertyChangeObservable : IObservable<DataBindingEventArgs>
{
    private readonly ObservableObject _source;
    private readonly string _propertyName;
    private object? _lastKnownValue;

    public PropertyChangeObservable(ObservableObject source, string propertyName)
    {
        _source = source;
        _propertyName = propertyName;
        _lastKnownValue = GetPropertyValue(source, propertyName);
    }

    public IDisposable Subscribe(IObserver<DataBindingEventArgs> observer)
    {
        var handler = new PropertyChangedEventHandler((s, e) =>
        {
            if (e.PropertyName == _propertyName)
            {
                var oldValue = _lastKnownValue;
                var newValue = GetPropertyValue(_source, _propertyName);
                
                var args = new DataBindingEventArgs
                {
                    PropertyName = e.PropertyName ?? string.Empty,
                    OldValue = oldValue,
                    NewValue = newValue,
                    IsChanging = true
                };
                
                _lastKnownValue = newValue;
                observer.OnNext(args);
            }
        });

        _source.PropertyChanged += handler;
        return new DisposableAction(() => _source.PropertyChanged -= handler);
    }

    private object? GetPropertyValue(ObservableObject obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName)?.GetValue(obj);
    }
}