using System;
using System.Collections.Generic;
using System.Linq;

namespace BannerlordModEditor.UI.Services;

/// <summary>
/// 空验证服务实现，用于当不使用验证服务时的依赖注入
/// </summary>
public class NullValidationService : IValidationService
{
    /// <summary>
    /// 验证对象
    /// </summary>
    /// <param name="obj">要验证的对象</param>
    /// <returns>验证结果</returns>
    public ValidationResult Validate(object obj)
    {
        return new ValidationResult { IsValid = true, Errors = new List<string>(), PropertyErrors = new Dictionary<string, List<string>>() };
    }

    /// <summary>
    /// 验证属性
    /// </summary>
    /// <param name="obj">要验证的对象</param>
    /// <param name="propertyName">属性名称</param>
    /// <returns>验证错误列表</returns>
    public List<string> ValidateProperty(object obj, string propertyName)
    {
        return new List<string>();
    }

    /// <summary>
    /// 创建响应式验证包装器
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="obj">要包装的对象</param>
    /// <returns>包装后的对象</returns>
    public T CreateValidatableWrapper<T>(T obj) where T : class
    {
        return obj;
    }

    /// <summary>
    /// 添加自定义验证规则
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="rule">验证规则</param>
    /// <param name="errorMessage">错误消息</param>
    public void AddValidationRule<T>(Func<T, bool> rule, string errorMessage)
    {
        // 空实现，不添加任何验证规则
    }
}