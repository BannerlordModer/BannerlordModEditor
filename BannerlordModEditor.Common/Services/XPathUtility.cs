using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// XPath查询工具类，提供统一的XPath查询处理
    /// </summary>
    public static class XPathUtility
    {
        /// <summary>
        /// 执行XPath查询并返回字符串值列表
        /// </summary>
        /// <param name="doc">XML文档</param>
        /// <param name="xpath">XPath表达式</param>
        /// <returns>匹配的字符串值列表</returns>
        public static List<string> EvaluateXPathToStrings(XDocument doc, string xpath)
        {
            try
            {
                var result = new List<string>();
                var nodes = doc.XPathEvaluate(xpath);
                
                if (nodes == null)
                    return result;
                
                // 处理不同类型的XPath结果
                if (nodes is IEnumerable<XElement> elements)
                {
                    result.AddRange(elements.Select(e => e.Value)
                        .Where(v => !string.IsNullOrEmpty(v)));
                }
                else if (nodes is IEnumerable<XAttribute> attributes)
                {
                    result.AddRange(attributes.Select(a => a.Value)
                        .Where(v => !string.IsNullOrEmpty(v)));
                }
                else if (nodes is System.Collections.IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        if (item is XElement element)
                        {
                            if (!string.IsNullOrEmpty(element.Value))
                                result.Add(element.Value);
                        }
                        else if (item is XAttribute attribute)
                        {
                            if (!string.IsNullOrEmpty(attribute.Value))
                                result.Add(attribute.Value);
                        }
                        else if (item is XPathNavigator navigator)
                        {
                            if (!string.IsNullOrEmpty(navigator.Value))
                                result.Add(navigator.Value);
                        }
                        else if (item != null)
                        {
                            // 尝试将对象转换为字符串
                            var stringValue = item.ToString();
                            if (!string.IsNullOrEmpty(stringValue))
                                result.Add(stringValue);
                        }
                    }
                }
                
                return result.Distinct().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XPath查询失败 '{xpath}': {ex.Message}");
                return new List<string>();
            }
        }
        
        /// <summary>
        /// 执行XPath查询并返回第一个匹配的字符串值
        /// </summary>
        /// <param name="doc">XML文档</param>
        /// <param name="xpath">XPath表达式</param>
        /// <returns>第一个匹配的字符串值，如果没有匹配则返回null</returns>
        public static string EvaluateXPathToString(XDocument doc, string xpath)
        {
            var results = EvaluateXPathToStrings(doc, xpath);
            return results.FirstOrDefault();
        }
        
        /// <summary>
        /// 执行XPath查询并返回元素列表
        /// </summary>
        /// <param name="doc">XML文档</param>
        /// <param name="xpath">XPath表达式</param>
        /// <returns>匹配的XElement列表</returns>
        public static List<XElement> EvaluateXPathToElements(XDocument doc, string xpath)
        {
            try
            {
                var result = new List<XElement>();
                var nodes = doc.XPathEvaluate(xpath);
                
                if (nodes == null)
                    return result;
                
                if (nodes is IEnumerable<XElement> elements)
                {
                    result.AddRange(elements);
                }
                else if (nodes is System.Collections.IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        if (item is XElement element)
                        {
                            result.Add(element);
                        }
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XPath元素查询失败 '{xpath}': {ex.Message}");
                return new List<XElement>();
            }
        }
        
        /// <summary>
        /// 执行XPath查询并返回属性列表
        /// </summary>
        /// <param name="doc">XML文档</param>
        /// <param name="xpath">XPath表达式</param>
        /// <returns>匹配的XAttribute列表</returns>
        public static List<XAttribute> EvaluateXPathToAttributes(XDocument doc, string xpath)
        {
            try
            {
                var result = new List<XAttribute>();
                var nodes = doc.XPathEvaluate(xpath);
                
                if (nodes == null)
                    return result;
                
                if (nodes is IEnumerable<XAttribute> attributes)
                {
                    result.AddRange(attributes);
                }
                else if (nodes is System.Collections.IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        if (item is XAttribute attribute)
                        {
                            result.Add(attribute);
                        }
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XPath属性查询失败 '{xpath}': {ex.Message}");
                return new List<XAttribute>();
            }
        }
    }
}