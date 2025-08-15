using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BannerlordModEditor.Common.Services.HybridXml
{
    /// <summary>
    /// XmlDocument合并服务接口
    /// 负责使用XmlDocument进行弱类型XML合并和加载
    /// </summary>
    public interface IXmlDocumentMerger
    {
        /// <summary>
        /// 合并多个模块的XML文件
        /// </summary>
        /// <param name="modulePaths">模块文件路径列表</param>
        /// <returns>合并后的XmlDocument</returns>
        XmlDocument MergeModules(IEnumerable<string> modulePaths);

        /// <summary>
        /// 加载基础文件并合并覆盖文件
        /// </summary>
        /// <param name="basePath">基础文件路径</param>
        /// <param name="overridePaths">覆盖文件路径列表</param>
        /// <returns>合并后的XmlDocument</returns>
        XmlDocument LoadAndMerge(string basePath, IEnumerable<string> overridePaths);

        /// <summary>
        /// 保存XmlDocument到原始位置
        /// </summary>
        /// <param name="document">要保存的XmlDocument</param>
        /// <param name="originalPath">原始文件路径</param>
        void SaveToOriginalLocation(XmlDocument document, string originalPath);

        /// <summary>
        /// 从XmlDocument中提取指定XPath的节点作为XElement
        /// </summary>
        /// <param name="document">XmlDocument</param>
        /// <param name="xpath">XPath表达式</param>
        /// <returns>提取的XElement</returns>
        XElement? ExtractElementForEditing(XmlDocument document, string xpath);

        /// <summary>
        /// 将修改后的XElement应用回XmlDocument
        /// </summary>
        /// <param name="document">目标XmlDocument</param>
        /// <param name="xpath">目标XPath</param>
        /// <param name="modifiedElement">修改后的XElement</param>
        void ApplyElementChanges(XmlDocument document, string xpath, XElement modifiedElement);
    }

    /// <summary>
    /// XElement到DTO映射器接口
    /// 负责强类型对象与XElement之间的转换
    /// </summary>
    /// <typeparam name="T">DTO类型</typeparam>
    public interface IXElementToDtoMapper<T> where T : class
    {
        /// <summary>
        /// 将XElement转换为强类型DTO
        /// </summary>
        /// <param name="element">源XElement</param>
        /// <returns>转换后的DTO</returns>
        T FromXElement(XElement element);

        /// <summary>
        /// 将强类型DTO转换为XElement
        /// </summary>
        /// <param name="dto">源DTO</param>
        /// <returns>转换后的XElement</returns>
        XElement ToXElement(T dto);

        /// <summary>
        /// 生成原始DTO和修改后DTO之间的差异补丁
        /// </summary>
        /// <param name="original">原始DTO</param>
        /// <param name="modified">修改后的DTO</param>
        /// <returns>差异补丁</returns>
        XmlPatch GeneratePatch(T original, T modified);

        /// <summary>
        /// 验证DTO的结构完整性
        /// </summary>
        /// <param name="dto">要验证的DTO</param>
        /// <returns>验证结果</returns>
        ValidationResult Validate(T dto);
    }

    /// <summary>
    /// 操作类型枚举
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 更新属性
        /// </summary>
        UpdateAttribute,

        /// <summary>
        /// 更新文本内容
        /// </summary>
        UpdateText,

        /// <summary>
        /// 添加元素
        /// </summary>
        AddElement,

        /// <summary>
        /// 删除元素
        /// </summary>
        RemoveElement,

        /// <summary>
        /// 移动元素
        /// </summary>
        MoveElement,

        /// <summary>
        /// 更新元素顺序
        /// </summary>
        UpdateElementOrder
    }

    /// <summary>
    /// XML节点操作
    /// </summary>
    public class XmlNodeOperation
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType Type { get; set; }

        /// <summary>
        /// 目标XPath
        /// </summary>
        public string XPath { get; set; } = string.Empty;

        /// <summary>
        /// 操作值（文本内容或属性值）
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// 属性字典（用于属性操作）
        /// </summary>
        public Dictionary<string, string>? Attributes { get; set; }

        /// <summary>
        /// 元素名称（用于元素操作）
        /// </summary>
        public string? ElementName { get; set; }

        /// <summary>
        /// 目标位置（用于移动操作）
        /// </summary>
        public string? TargetXPath { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string? Description { get; set; }
    }

    /// <summary>
    /// XML差异补丁
    /// </summary>
    public class XmlPatch
    {
        /// <summary>
        /// 操作列表
        /// </summary>
        public List<XmlNodeOperation> Operations { get; } = new();

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="operation">要添加的操作</param>
        public void AddOperation(XmlNodeOperation operation)
        {
            Operations.Add(operation);
        }

        /// <summary>
        /// 应用补丁到XmlDocument
        /// </summary>
        /// <param name="document">目标XmlDocument</param>
        public void ApplyTo(XmlDocument document)
        {
            foreach (var operation in Operations)
            {
                ApplyOperation(document, operation);
            }
        }

        /// <summary>
        /// 应用补丁到XElement
        /// </summary>
        /// <param name="element">目标XElement</param>
        public void ApplyTo(XElement element)
        {
            foreach (var operation in Operations)
            {
                ApplyOperation(element, operation);
            }
        }

        private void ApplyOperation(XmlDocument document, XmlNodeOperation operation)
        {
            switch (operation.Type)
            {
                case OperationType.UpdateAttribute:
                    UpdateAttribute(document, operation);
                    break;
                case OperationType.UpdateText:
                    UpdateText(document, operation);
                    break;
                case OperationType.AddElement:
                    AddElement(document, operation);
                    break;
                case OperationType.RemoveElement:
                    RemoveElement(document, operation);
                    break;
                case OperationType.MoveElement:
                    MoveElement(document, operation);
                    break;
                case OperationType.UpdateElementOrder:
                    UpdateElementOrder(document, operation);
                    break;
            }
        }

        private void ApplyOperation(XElement element, XmlNodeOperation operation)
        {
            // XElement版本的操作实现
            switch (operation.Type)
            {
                case OperationType.UpdateAttribute:
                    if (operation.Attributes != null)
                    {
                        foreach (var attr in operation.Attributes)
                        {
                            element.SetAttributeValue(attr.Key, attr.Value);
                        }
                    }
                    break;
                case OperationType.UpdateText:
                    element.Value = operation.Value ?? string.Empty;
                    break;
                case OperationType.AddElement:
                    if (!string.IsNullOrEmpty(operation.ElementName))
                    {
                        var newElement = new XElement(operation.ElementName);
                        if (operation.Attributes != null)
                        {
                            foreach (var attr in operation.Attributes)
                            {
                                newElement.SetAttributeValue(attr.Key, attr.Value);
                            }
                        }
                        element.Add(newElement);
                    }
                    break;
                case OperationType.RemoveElement:
                    var toRemove = element.XPathSelectElement(operation.XPath);
                    toRemove?.Remove();
                    break;
            }
        }

        private void UpdateAttribute(XmlDocument document, XmlNodeOperation operation)
        {
            var node = document.SelectSingleNode(operation.XPath);
            if (node is XmlElement element && operation.Attributes != null)
            {
                foreach (var attr in operation.Attributes)
                {
                    element.SetAttribute(attr.Key, attr.Value);
                }
            }
        }

        private void UpdateText(XmlDocument document, XmlNodeOperation operation)
        {
            var node = document.SelectSingleNode(operation.XPath);
            if (node != null)
            {
                node.InnerText = operation.Value ?? string.Empty;
            }
        }

        private void AddElement(XmlDocument document, XmlNodeOperation operation)
        {
            var parent = document.SelectSingleNode(operation.XPath);
            if (parent is XmlElement parentElement && !string.IsNullOrEmpty(operation.ElementName))
            {
                var newElement = document.CreateElement(operation.ElementName);
                if (operation.Attributes != null)
                {
                    foreach (var attr in operation.Attributes)
                    {
                        newElement.SetAttribute(attr.Key, attr.Value);
                    }
                }
                parentElement.AppendChild(newElement);
            }
        }

        private void RemoveElement(XmlDocument document, XmlNodeOperation operation)
        {
            var node = document.SelectSingleNode(operation.XPath);
            node?.ParentNode?.RemoveChild(node);
        }

        private void MoveElement(XmlDocument document, XmlNodeOperation operation)
        {
            var node = document.SelectSingleNode(operation.XPath);
            var target = document.SelectSingleNode(operation.TargetXPath);
            if (node != null && target is XmlElement targetElement)
            {
                targetElement.AppendChild(node);
            }
        }

        private void UpdateElementOrder(XmlDocument document, XmlNodeOperation operation)
        {
            // 实现元素顺序更新逻辑
            var parent = document.SelectSingleNode(operation.XPath);
            if (parent is XmlElement parentElement)
            {
                // 重新排序子元素
                var children = parentElement.ChildNodes.Cast<XmlNode>().ToList();
                parentElement.RemoveAll();
                
                foreach (var child in children)
                {
                    parentElement.AppendChild(child);
                }
            }
        }
    }

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
        /// 错误消息列表
        /// </summary>
        public List<string> Errors { get; } = new();

        /// <summary>
        /// 警告消息列表
        /// </summary>
        public List<string> Warnings { get; } = new();

        /// <summary>
        /// 添加错误消息
        /// </summary>
        /// <param name="error">错误消息</param>
        public void AddError(string error)
        {
            Errors.Add(error);
            IsValid = false;
        }

        /// <summary>
        /// 添加警告消息
        /// </summary>
        /// <param name="warning">警告消息</param>
        public void AddWarning(string warning)
        {
            Warnings.Add(warning);
        }
    }

    /// <summary>
    /// XML编辑管理器
    /// 负责协调XmlDocument合并器和DTO映射器的工作
    /// </summary>
    /// <typeparam name="T">DTO类型</typeparam>
    public class XmlEditorManager<T> where T : class
    {
        private readonly IXmlDocumentMerger _documentMerger;
        private readonly IXElementToDtoMapper<T> _mapper;
        private XmlDocument? _mergedDocument;

        public XmlEditorManager(IXmlDocumentMerger documentMerger, IXElementToDtoMapper<T> mapper)
        {
            _documentMerger = documentMerger;
            _mapper = mapper;
        }

        /// <summary>
        /// 加载XML并准备编辑
        /// </summary>
        /// <param name="xmlPath">XML文件路径</param>
        /// <param name="elementXPath">要编辑的元素XPath</param>
        /// <returns>编辑用的DTO</returns>
        public async Task<T> LoadForEditAsync(string xmlPath, string elementXPath)
        {
            return await Task.Run(() =>
            {
                _mergedDocument = _documentMerger.LoadAndMerge(xmlPath, Enumerable.Empty<string>());
                var element = _documentMerger.ExtractElementForEditing(_mergedDocument, elementXPath);
                
                if (element == null)
                {
                    throw new InvalidOperationException($"无法找到XPath为 {elementXPath} 的元素");
                }

                return _mapper.FromXElement(element);
            });
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="xmlPath">XML文件路径</param>
        /// <param name="modifiedDto">修改后的DTO</param>
        /// <param name="originalDto">原始DTO</param>
        public async Task SaveChangesAsync(string xmlPath, T modifiedDto, T originalDto)
        {
            await Task.Run(() =>
            {
                if (_mergedDocument == null)
                {
                    throw new InvalidOperationException("没有加载的文档可以保存");
                }

                // 生成差异补丁
                var patch = _mapper.GeneratePatch(originalDto, modifiedDto);
                
                // 将修改后的DTO转换为XElement
                var modifiedElement = _mapper.ToXElement(modifiedDto);
                
                // 应用变更到XmlDocument
                _documentMerger.ApplyElementChanges(_mergedDocument, "/*", modifiedElement);
                
                // 应用差异补丁
                patch.ApplyTo(_mergedDocument);
                
                // 保存到文件
                _documentMerger.SaveToOriginalLocation(_mergedDocument, xmlPath);
            });
        }

        /// <summary>
        /// 获取合并后的XmlDocument
        /// </summary>
        /// <returns>合并后的XmlDocument</returns>
        public XmlDocument? GetMergedDocument()
        {
            return _mergedDocument;
        }

        /// <summary>
        /// 验证DTO
        /// </summary>
        /// <param name="dto">要验证的DTO</param>
        /// <returns>验证结果</returns>
        public ValidationResult Validate(T dto)
        {
            return _mapper.Validate(dto);
        }
    }
}