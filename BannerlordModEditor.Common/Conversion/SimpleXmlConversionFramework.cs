using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BannerlordModEditor.Common.Conversion
{
    /// <summary>
    /// 通用XML转换框架简化版实现
    /// </summary>
    public static class SimpleXmlConversionFramework
    {
        /// <summary>
        /// 将XML转换为二维表格格式
        /// </summary>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <returns>转换结果</returns>
        public static async Task<TableData> XmlToTableAsync(string xmlFilePath)
        {
            var tableData = new TableData();

            try
            {
                if (!System.IO.File.Exists(xmlFilePath))
                {
                    throw new System.IO.FileNotFoundException($"XML文件不存在: {xmlFilePath}");
                }

                // 加载XML文档
                var xmlDoc = XDocument.Load(xmlFilePath);
                var rootElement = xmlDoc.Root;

                if (rootElement == null)
                {
                    throw new InvalidOperationException("XML文件没有根元素");
                }

                // 分析结构并收集列名
                var columns = new HashSet<string>();
                var rows = new List<TableRow>();

                // 处理所有子元素作为记录
                foreach (var recordElement in rootElement.Elements())
                {
                    var row = new TableRow();
                    CollectElementData(recordElement, row, "", columns);
                    rows.Add(row);
                }

                // 设置表格数据
                foreach (var column in columns.OrderBy(c => c))
                {
                    tableData.AddColumn(column);
                }

                foreach (var row in rows)
                {
                    tableData.AddRow(row);
                }

                // 添加元数据
                tableData.Metadata["XmlType"] = rootElement.Name.LocalName;
                tableData.Metadata["RecordCount"] = rows.Count.ToString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"XML转换失败: {ex.Message}", ex);
            }

            return await Task.FromResult(tableData);
        }

        /// <summary>
        /// 将二维表格格式转换为XML
        /// </summary>
        /// <param name="tableData">表格数据</param>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <returns>转换结果</returns>
        public static async Task<bool> TableToXmlAsync(TableData tableData, string xmlFilePath)
        {
            try
            {
                if (tableData == null || tableData.Rows.Count == 0)
                {
                    throw new ArgumentException("表格数据不能为空");
                }

                // 确定根元素名称
                var rootElementName = tableData.Metadata.TryGetValue("XmlType", out var xmlType) ? xmlType : "Root";
                var rowElementName = "Item";

                var rootElement = new XElement(rootElementName);

                foreach (var rowData in tableData.Rows)
                {
                    var rowElement = new XElement(rowElementName);
                    
                    foreach (var column in tableData.Columns)
                    {
                        if (rowData.TryGetValue(column, out var value) && !string.IsNullOrEmpty(value?.ToString()))
                        {
                            rowElement.SetAttributeValue(column, value);
                        }
                    }

                    rootElement.Add(rowElement);
                }

                // 保存XML文件
                var settings = new System.Xml.XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = System.Text.Encoding.UTF8,
                    CheckCharacters = true,
                    CloseOutput = true
                };

                using var writer = System.Xml.XmlWriter.Create(xmlFilePath, settings);
                rootElement.Save(writer);

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"表格转换失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 将XML转换为CSV格式
        /// </summary>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <param name="csvFilePath">CSV文件路径</param>
        /// <returns>转换结果</returns>
        public static async Task<bool> XmlToCsvAsync(string xmlFilePath, string csvFilePath)
        {
            try
            {
                // 先转换为表格
                var tableData = await XmlToTableAsync(xmlFilePath);

                // 写入CSV文件
                using var writer = new System.IO.StreamWriter(csvFilePath, false, System.Text.Encoding.UTF8);
                
                // 写入表头
                await writer.WriteLineAsync(string.Join(",", tableData.Columns));
                
                // 写入数据行
                foreach (var row in tableData.Rows)
                {
                    var values = tableData.Columns.Select(col => 
                    {
                        if (row.TryGetValue(col, out var value))
                        {
                            var stringValue = value?.ToString() ?? "";
                            // 转义CSV特殊字符
                            if (stringValue.Contains(",") || stringValue.Contains("\"") || stringValue.Contains("\n"))
                            {
                                return "\"" + stringValue.Replace("\"", "\"\"") + "\"";
                            }
                            return stringValue;
                        }
                        return "";
                    });
                    
                    await writer.WriteLineAsync(string.Join(",", values));
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"XML转CSV失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 将CSV格式转换为XML
        /// </summary>
        /// <param name="csvFilePath">CSV文件路径</param>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <returns>转换结果</returns>
        public static async Task<bool> CsvToXmlAsync(string csvFilePath, string xmlFilePath)
        {
            try
            {
                var tableData = new TableData();
                var lines = await System.IO.File.ReadAllLinesAsync(csvFilePath, System.Text.Encoding.UTF8);

                if (lines.Length == 0)
                {
                    throw new InvalidOperationException("CSV文件为空");
                }

                // 解析表头
                var headers = ParseCsvLine(lines[0]);
                foreach (var header in headers)
                {
                    tableData.AddColumn(header);
                }

                // 解析数据行
                for (int i = 1; i < lines.Length; i++)
                {
                    var values = ParseCsvLine(lines[i]);
                    var row = new TableRow();

                    for (int j = 0; j < Math.Min(values.Count, tableData.Columns.Count); j++)
                    {
                        row[tableData.Columns[j]] = values[j];
                    }

                    tableData.AddRow(row);
                }

                // 转换为XML
                return await TableToXmlAsync(tableData, xmlFilePath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"CSV转XML失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 分析XML结构
        /// </summary>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <returns>结构信息</returns>
        public static async Task<XmlStructureInfo> AnalyzeXmlStructureAsync(string xmlFilePath)
        {
            var result = new XmlStructureInfo();

            try
            {
                var xmlDoc = XDocument.Load(xmlFilePath);
                var rootElement = xmlDoc.Root;

                if (rootElement != null)
                {
                    result.RootElement = rootElement.Name.LocalName;
                    result.EstimatedRecordCount = rootElement.Elements().Count();

                    // 分析结构
                    AnalyzeElementStructure(rootElement, result, 0);

                    // 确定复杂度
                    result.Complexity = DetermineComplexity(result);
                }
            }
            catch (Exception ex)
            {
                result.Metadata["Error"] = ex.Message;
            }

            return await Task.FromResult(result);
        }

        #region Private Helper Methods

        private static void CollectElementData(XElement element, TableRow row, string parentPath, HashSet<string> columns)
        {
            // 处理属性
            foreach (var attribute in element.Attributes())
            {
                var columnName = string.IsNullOrEmpty(parentPath) ? 
                    attribute.Name.LocalName : $"{parentPath}_{attribute.Name.LocalName}";
                columns.Add(columnName);
                row[columnName] = attribute.Value;
            }

            // 处理子元素
            foreach (var childElement in element.Elements())
            {
                var childPath = string.IsNullOrEmpty(parentPath) ? 
                    childElement.Name.LocalName : $"{parentPath}_{childElement.Name.LocalName}";

                if (childElement.HasElements)
                {
                    CollectElementData(childElement, row, childPath, columns);
                }
                else
                {
                    columns.Add(childPath);
                    row[childPath] = childElement.Value;
                }
            }
        }

        private static List<string> ParseCsvLine(string line)
        {
            var result = new List<string>();
            var currentValue = new System.Text.StringBuilder();
            var inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        // 转义引号
                        currentValue.Append('"');
                        i++; // 跳过下一个引号
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(currentValue.ToString());
                    currentValue.Clear();
                }
                else
                {
                    currentValue.Append(c);
                }
            }

            result.Add(currentValue.ToString());
            return result;
        }

        private static void AnalyzeElementStructure(XElement element, XmlStructureInfo info, int depth)
        {
            // 更新深度
            if (depth > info.EstimatedDepth)
            {
                info.EstimatedDepth = depth;
            }

            // 收集元素名
            if (!info.Elements.Contains(element.Name.LocalName))
            {
                info.Elements.Add(element.Name.LocalName);
            }

            // 收集属性
            foreach (var attribute in element.Attributes())
            {
                var attrName = attribute.Name.LocalName;
                if (!info.Attributes.Contains(attrName))
                {
                    info.Attributes.Add(attrName);
                }
            }

            // 递归分析子元素
            foreach (var childElement in element.Elements())
            {
                AnalyzeElementStructure(childElement, info, depth + 1);
            }
        }

        private static XmlComplexity DetermineComplexity(XmlStructureInfo info)
        {
            if (info.EstimatedDepth <= 1 && info.Elements.Count <= 5)
            {
                return XmlComplexity.Simple;
            }
            else if (info.EstimatedDepth <= 3 && info.Elements.Count <= 20)
            {
                return XmlComplexity.Medium;
            }
            else if (info.EstimatedDepth <= 5 && info.Elements.Count <= 50)
            {
                return XmlComplexity.Complex;
            }
            else
            {
                return XmlComplexity.VeryComplex;
            }
        }

        #endregion
    }
}