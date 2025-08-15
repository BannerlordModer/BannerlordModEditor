using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BannerlordModEditor.Common.Services.HybridXml
{
    /// <summary>
    /// XmlDocument合并服务实现
    /// 负责使用XmlDocument进行弱类型XML合并和加载
    /// </summary>
    public class XmlDocumentMerger : IXmlDocumentMerger
    {
        private readonly XmlNameTable _nameTable;
        private readonly XmlNamespaceManager _namespaceManager;

        public XmlDocumentMerger()
        {
            _nameTable = new NameTable();
            _namespaceManager = new XmlNamespaceManager(_nameTable);
        }

        /// <summary>
        /// 合并多个模块的XML文件
        /// </summary>
        /// <param name="modulePaths">模块文件路径列表</param>
        /// <returns>合并后的XmlDocument</returns>
        public XmlDocument MergeModules(IEnumerable<string> modulePaths)
        {
            var moduleList = modulePaths.ToList();
            if (!moduleList.Any())
            {
                throw new ArgumentException("至少需要一个模块文件", nameof(modulePaths));
            }

            // 使用第一个文件作为基础
            var baseDoc = new XmlDocument(_nameTable);
            baseDoc.Load(moduleList.First());

            // 合并其他模块
            foreach (var modulePath in moduleList.Skip(1))
            {
                if (File.Exists(modulePath))
                {
                    var overrideDoc = new XmlDocument(_nameTable);
                    overrideDoc.Load(modulePath);
                    MergeDocuments(baseDoc, overrideDoc);
                }
            }

            return baseDoc;
        }

        /// <summary>
        /// 加载基础文件并合并覆盖文件
        /// </summary>
        /// <param name="basePath">基础文件路径</param>
        /// <param name="overridePaths">覆盖文件路径列表</param>
        /// <returns>合并后的XmlDocument</returns>
        public XmlDocument LoadAndMerge(string basePath, IEnumerable<string> overridePaths)
        {
            if (!File.Exists(basePath))
            {
                throw new FileNotFoundException($"基础文件不存在: {basePath}");
            }

            var baseDoc = new XmlDocument(_nameTable);
            baseDoc.Load(basePath);

            // 合并覆盖文件
            foreach (var overridePath in overridePaths.Where(File.Exists))
            {
                var overrideDoc = new XmlDocument(_nameTable);
                overrideDoc.Load(overridePath);
                MergeDocuments(baseDoc, overrideDoc);
            }

            return baseDoc;
        }

        /// <summary>
        /// 保存XmlDocument到原始位置
        /// </summary>
        /// <param name="document">要保存的XmlDocument</param>
        /// <param name="originalPath">原始文件路径</param>
        public void SaveToOriginalLocation(XmlDocument document, string originalPath)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (string.IsNullOrEmpty(originalPath))
            {
                throw new ArgumentException("文件路径不能为空", nameof(originalPath));
            }

            // 确保目录存在
            var directory = Path.GetDirectoryName(originalPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // 保存文件，保持原始格式
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\n",
                Encoding = System.Text.Encoding.UTF8,
                OmitXmlDeclaration = false,
                NewLineOnAttributes = false
            };

            using var writer = XmlWriter.Create(originalPath, settings);
            document.Save(writer);
        }

        /// <summary>
        /// 从XmlDocument中提取指定XPath的节点作为XElement
        /// </summary>
        /// <param name="document">XmlDocument</param>
        /// <param name="xpath">XPath表达式</param>
        /// <returns>提取的XElement</returns>
        public XElement? ExtractElementForEditing(XmlDocument document, string xpath)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (string.IsNullOrEmpty(xpath))
            {
                throw new ArgumentException("XPath不能为空", nameof(xpath));
            }

            var node = document.SelectSingleNode(xpath, _namespaceManager);
            if (node == null)
            {
                return null;
            }

            // 将XmlNode转换为XElement
            return XElement.Parse(node.OuterXml);
        }

        /// <summary>
        /// 将修改后的XElement应用回XmlDocument
        /// </summary>
        /// <param name="document">目标XmlDocument</param>
        /// <param name="xpath">目标XPath</param>
        /// <param name="modifiedElement">修改后的XElement</param>
        public void ApplyElementChanges(XmlDocument document, string xpath, XElement modifiedElement)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (string.IsNullOrEmpty(xpath))
            {
                throw new ArgumentException("XPath不能为空", nameof(xpath));
            }

            if (modifiedElement == null)
            {
                throw new ArgumentNullException(nameof(modifiedElement));
            }

            var targetNode = document.SelectSingleNode(xpath, _namespaceManager);
            if (targetNode == null)
            {
                throw new InvalidOperationException($"无法找到XPath为 {xpath} 的目标节点");
            }

            // 将XElement转换回XmlNode并替换
            var modifiedNode = document.CreateElement(modifiedElement.Name.LocalName);
            
            // 复制属性
            foreach (var attr in modifiedElement.Attributes())
            {
                modifiedNode.SetAttribute(attr.Name.LocalName, attr.Value);
            }

            // 复制子元素
            foreach (var child in modifiedElement.Elements())
            {
                var childNode = document.CreateElement(child.Name.LocalName);
                
                // 递归复制子元素的属性和内容
                CopyXElementToXmlNode(child, childNode, document);
                
                modifiedNode.AppendChild(childNode);
            }

            // 复制文本内容
            if (!string.IsNullOrEmpty(modifiedElement.Value))
            {
                modifiedNode.InnerText = modifiedElement.Value;
            }

            // 替换原始节点
            targetNode.ParentNode?.ReplaceChild(modifiedNode, targetNode);
        }

        /// <summary>
        /// 合并两个XmlDocument
        /// </summary>
        /// <param name="baseDoc">基础文档</param>
        /// <param name="overrideDoc">覆盖文档</param>
        private void MergeDocuments(XmlDocument baseDoc, XmlDocument overrideDoc)
        {
            if (baseDoc.DocumentElement == null || overrideDoc.DocumentElement == null)
            {
                return;
            }

            MergeElements(baseDoc.DocumentElement, overrideDoc.DocumentElement, baseDoc);
        }

        /// <summary>
        /// 合并两个XmlElement
        /// </summary>
        /// <param name="baseElement">基础元素</param>
        /// <param name="overrideElement">覆盖元素</param>
        /// <param name="ownerDoc">拥有者文档</param>
        private void MergeElements(XmlElement baseElement, XmlElement overrideElement, XmlDocument ownerDoc)
        {
            // 合并属性
            foreach (XmlAttribute attr in overrideElement.Attributes)
            {
                baseElement.SetAttribute(attr.Name, attr.Value);
            }

            // 合并子元素 - 基于元素名称匹配
            var baseChildren = baseElement.ChildNodes.Cast<XmlNode>().Where(n => n is XmlElement).ToList();
            var overrideChildren = overrideElement.ChildNodes.Cast<XmlNode>().Where(n => n is XmlElement).ToList();

            foreach (var overrideChild in overrideChildren)
            {
                if (overrideChild is XmlElement overrideChildElement)
                {
                    var matchingBaseChild = baseChildren.FirstOrDefault(c => 
                        c is XmlElement baseChildElement && baseChildElement.Name == overrideChildElement.Name);

                    if (matchingBaseChild != null)
                    {
                        // 递归合并匹配的子元素
                        MergeElements((XmlElement)matchingBaseChild, overrideChildElement, ownerDoc);
                    }
                    else
                    {
                        // 添加新的子元素
                        var newChild = ownerDoc.CreateElement(overrideChildElement.Name);
                        CopyXmlElement(overrideChildElement, newChild, ownerDoc);
                        baseElement.AppendChild(newChild);
                    }
                }
            }

            // 合并文本内容
            if (!string.IsNullOrEmpty(overrideElement.InnerText) && 
                string.IsNullOrEmpty(baseElement.InnerText))
            {
                baseElement.InnerText = overrideElement.InnerText;
            }
        }

        /// <summary>
        /// 复制XmlElement
        /// </summary>
        /// <param name="source">源元素</param>
        /// <param name="target">目标元素</param>
        /// <param name="ownerDoc">拥有者文档</param>
        private void CopyXmlElement(XmlElement source, XmlElement target, XmlDocument ownerDoc)
        {
            // 复制属性
            foreach (XmlAttribute attr in source.Attributes)
            {
                target.SetAttribute(attr.Name, attr.Value);
            }

            // 复制子元素
            foreach (XmlNode child in source.ChildNodes)
            {
                if (child is XmlElement childElement)
                {
                    var newChild = ownerDoc.CreateElement(childElement.Name);
                    CopyXmlElement(childElement, newChild, ownerDoc);
                    target.AppendChild(newChild);
                }
                else if (child is XmlText textNode)
                {
                    target.AppendChild(ownerDoc.CreateTextNode(textNode.Value));
                }
            }
        }

        /// <summary>
        /// 将XElement递归复制到XmlNode
        /// </summary>
        /// <param name="source">源XElement</param>
        /// <param name="target">目标XmlNode</param>
        /// <param name="ownerDoc">拥有者文档</param>
        private void CopyXElementToXmlNode(XElement source, XmlNode target, XmlDocument ownerDoc)
        {
            // 复制属性
            foreach (var attr in source.Attributes())
            {
                if (target is XmlElement element)
                {
                    element.SetAttribute(attr.Name.LocalName, attr.Value);
                }
            }

            // 复制子元素
            foreach (var child in source.Elements())
            {
                var childNode = ownerDoc.CreateElement(child.Name.LocalName);
                CopyXElementToXmlNode(child, childNode, ownerDoc);
                target.AppendChild(childNode);
            }

            // 复制文本内容
            if (!string.IsNullOrEmpty(source.Value))
            {
                target.InnerText = source.Value;
            }
        }

        /// <summary>
        /// 创建XmlDocument的深拷贝
        /// </summary>
        /// <param name="document">源文档</param>
        /// <returns>拷贝的文档</returns>
        public XmlDocument CloneDocument(XmlDocument document)
        {
            var clone = new XmlDocument(_nameTable);
            clone.LoadXml(document.OuterXml);
            return clone;
        }

        /// <summary>
        /// 检查文档是否有变更
        /// </summary>
        /// <param name="original">原始文档</param>
        /// <param name="modified">修改后的文档</param>
        /// <returns>是否有变更</returns>
        public bool HasChanges(XmlDocument original, XmlDocument modified)
        {
            return original.OuterXml != modified.OuterXml;
        }

        /// <summary>
        /// 获取文档的统计信息
        /// </summary>
        /// <param name="document">XmlDocument</param>
        /// <returns>统计信息</returns>
        public DocumentStatistics GetStatistics(XmlDocument document)
        {
            var stats = new DocumentStatistics();
            
            if (document.DocumentElement != null)
            {
                CountNodes(document.DocumentElement, stats);
            }

            return stats;
        }

        /// <summary>
        /// 递归统计节点信息
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <param name="stats">统计信息</param>
        private void CountNodes(XmlNode node, DocumentStatistics stats)
        {
            stats.TotalNodes++;

            if (node is XmlElement element)
            {
                stats.ElementNodes++;
                stats.TotalAttributes += element.Attributes.Count;
            }
            else if (node is XmlText)
            {
                stats.TextNodes++;
            }
            else if (node is XmlComment)
            {
                stats.CommentNodes++;
            }

            foreach (XmlNode child in node.ChildNodes)
            {
                CountNodes(child, stats);
            }
        }
    }

    /// <summary>
    /// 文档统计信息
    /// </summary>
    public class DocumentStatistics
    {
        /// <summary>
        /// 总节点数
        /// </summary>
        public int TotalNodes { get; set; }

        /// <summary>
        /// 元素节点数
        /// </summary>
        public int ElementNodes { get; set; }

        /// <summary>
        /// 文本节点数
        /// </summary>
        public int TextNodes { get; set; }

        /// <summary>
        /// 注释节点数
        /// </summary>
        public int CommentNodes { get; set; }

        /// <summary>
        /// 总属性数
        /// </summary>
        public int TotalAttributes { get; set; }

        /// <summary>
        /// 获取统计摘要
        /// </summary>
        /// <returns>摘要字符串</returns>
        public string GetSummary()
        {
            return $"节点: {TotalNodes} (元素: {ElementNodes}, 文本: {TextNodes}, 注释: {CommentNodes}), 属性: {TotalAttributes}";
        }
    }
}