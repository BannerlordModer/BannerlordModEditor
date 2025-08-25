using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BannerlordModEditor.UI.Services;

/// <summary>
/// 验证结果
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public Dictionary<string, List<string>> PropertyErrors { get; set; } = new();
}

/// <summary>
/// 数据验证服务
/// </summary>
public interface IValidationService
{
    /// <summary>
    /// 验证对象
    /// </summary>
    ValidationResult Validate(object obj);

    /// <summary>
    /// 验证属性
    /// </summary>
    List<string> ValidateProperty(object obj, string propertyName);

    /// <summary>
    /// 创建响应式验证包装器
    /// </summary>
    T CreateValidatableWrapper<T>(T obj) where T : class;

    /// <summary>
    /// 添加自定义验证规则
    /// </summary>
    void AddValidationRule<T>(Func<T, bool> rule, string errorMessage);
}

/// <summary>
/// 响应式验证包装器基类
/// </summary>
/// <typeparam name="T">包装的对象类型</typeparam>
public abstract class ValidatableWrapper<T> : ObservableObject where T : class
{
    private readonly T _wrappedObject;
    private readonly IValidationService _validationService;

    protected ValidatableWrapper(T wrappedObject, IValidationService validationService)
    {
        _wrappedObject = wrappedObject ?? throw new ArgumentNullException(nameof(wrappedObject));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    }

    protected T WrappedObject => _wrappedObject;

    /// <summary>
    /// 获取包装对象的验证结果
    /// </summary>
    public ValidationResult GetValidationResult()
    {
        return _validationService.Validate(_wrappedObject);
    }

    /// <summary>
    /// 验证特定属性
    /// </summary>
    public List<string> ValidateProperty(string propertyName)
    {
        return _validationService.ValidateProperty(_wrappedObject, propertyName);
    }

    /// <summary>
    /// 获取所有验证错误
    /// </summary>
    public List<string> GetAllErrors()
    {
        var result = GetValidationResult();
        return result.Errors;
    }

    /// <summary>
    /// 检查对象是否有效
    /// </summary>
    public bool IsValid => GetValidationResult().IsValid;

    /// <summary>
    /// 触发属性验证
    /// </summary>
    protected void ValidateAndNotify(string propertyName)
    {
        var errors = ValidateProperty(propertyName);
        OnPropertyChanged(nameof(IsValid));
        OnPropertyChanged(nameof(GetAllErrors));
    }
}

/// <summary>
/// 验证服务实现
/// </summary>
public class ValidationService : IValidationService
{
    private readonly List<Func<object, bool>> _customRules = new();
    private readonly List<string> _customErrorMessages = new();

    public ValidationResult Validate(object obj)
    {
        var result = new ValidationResult();

        if (obj == null)
        {
            result.Errors.Add("对象不能为null");
            return result;
        }

        // 使用DataAnnotations进行验证
        var validationContext = new ValidationContext(obj);
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

        if (!Validator.TryValidateObject(obj, validationContext, validationResults, true))
        {
            foreach (var validationResult in validationResults)
            {
                result.Errors.Add(validationResult.ErrorMessage ?? "验证失败");

                if (validationResult.MemberNames != null)
                {
                    foreach (var memberName in validationResult.MemberNames)
                    {
                        if (!result.PropertyErrors.ContainsKey(memberName))
                        {
                            result.PropertyErrors[memberName] = new List<string>();
                        }
                        result.PropertyErrors[memberName].Add(validationResult.ErrorMessage ?? "验证失败");
                    }
                }
            }
        }

        // 应用自定义验证规则
        for (int i = 0; i < _customRules.Count; i++)
        {
            if (!_customRules[i](obj))
            {
                result.Errors.Add(_customErrorMessages[i]);
            }
        }

        result.IsValid = result.Errors.Count == 0;
        return result;
    }

    public List<string> ValidateProperty(object obj, string propertyName)
    {
        var results = new List<string>();

        if (obj == null)
        {
            results.Add("对象不能为null");
            return results;
        }

        // 使用DataAnnotations验证特定属性
        var validationContext = new ValidationContext(obj)
        {
            MemberName = propertyName
        };

        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        
        var propertyInfo = obj.GetType().GetProperty(propertyName);
        if (propertyInfo == null)
        {
            // 属性不存在，返回空结果
            return results;
        }
        
        Validator.TryValidateProperty(
            propertyInfo.GetValue(obj),
            validationContext,
            validationResults
        );

        foreach (var validationResult in validationResults)
        {
            results.Add(validationResult.ErrorMessage ?? "验证失败");
        }

        return results;
    }

    public T CreateValidatableWrapper<T>(T obj) where T : class
    {
        // 这里需要根据具体的T类型创建对应的包装器
        // 这是一个简化实现，实际项目中可能需要使用工厂模式
        throw new NotImplementedException("需要为特定类型实现包装器");
    }

    public void AddValidationRule<T>(Func<T, bool> rule, string errorMessage)
    {
        _customRules.Add(obj => rule((T)obj));
        _customErrorMessages.Add(errorMessage);
    }
}