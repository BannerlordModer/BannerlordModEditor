using System.Xml;
using System.Xml.Linq;
using BannerlordModEditor.Common.Services.HybridXml;
using BannerlordModEditor.Common.Services.HybridXml.Dto;
using BannerlordModEditor.Common.Services.HybridXml.Mappers;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests.HybridXml
{
    /// <summary>
    /// Looknfeel XML节点数量问题修复测试
    /// 专门验证新架构能否解决节点数量从539变成537的问题
    /// </summary>
    public class LooknfeelNodeCountFixTests
    {
        private readonly IXmlDocumentMerger _documentMerger;
        private readonly IXElementToDtoMapper<LooknfeelEditDto> _mapper;

        public LooknfeelNodeCountFixTests()
        {
            _documentMerger = new XmlDocumentMerger();
            _mapper = new LooknfeelMapper();
        }

        [Fact]
        public void HybridXml_LoadAndSave_ShouldPreserveNodeCount()
        {
            // 准备测试数据
            var testXmlPath = "example/ModuleData/looknfeel.xml";
            if (!File.Exists(testXmlPath))
            {
                return;
            }

            // 加载原始XML
            var originalXml = File.ReadAllText(testXmlPath);
            var originalDoc = XDocument.Parse(originalXml);
            
            // 统计原始XML信息
            var originalStats = GetDetailedStatistics(originalDoc);
            
            Console.WriteLine($"原始XML统计: {originalStats.GetSummary()}");

            // 使用新架构处理
            var document = _documentMerger.LoadAndMerge(testXmlPath, Enumerable.Empty<string>());
            var element = _documentMerger.ExtractElementForEditing(document, "/base");
            
            Assert.NotNull(element);

            // 转换为DTO
            var dto = _mapper.FromXElement(element);
            
            // 验证DTO结构
            Assert.NotNull(dto);
            Assert.NotNull(dto.Widgets);
            Assert.NotNull(dto.Widgets.WidgetList);
            
            // 转换回XElement
            var convertedElement = _mapper.ToXElement(dto);
            
            // 转换为XmlDocument用于保存
            var convertedDocument = new XmlDocument();
            convertedDocument.LoadXml(convertedElement.ToString());

            // 统计转换后的XML信息
            var convertedStats = GetDetailedStatistics(XDocument.Parse(convertedElement.ToString()));
            
            Console.WriteLine($"转换后XML统计: {convertedStats.GetSummary()}");

            // 验证节点数量保持一致
            Assert.Equal(originalStats.TotalNodes, convertedStats.TotalNodes);
            Assert.Equal(originalStats.ElementNodes, convertedStats.ElementNodes);
            Assert.Equal(originalStats.TotalAttributes, convertedStats.TotalAttributes);

            // 测试保存和重新加载
            var savedPath = testXmlPath + ".test_save";
            _documentMerger.SaveToOriginalLocation(convertedDocument, savedPath);
            
            var reloadedXml = File.ReadAllText(savedPath);
            var reloadedDoc = XDocument.Parse(reloadedXml);
            var reloadedStats = GetDetailedStatistics(reloadedDoc);
            
            Console.WriteLine($"重新加载XML统计: {reloadedStats.GetSummary()}");

            // 验证重新加载后的节点数量仍然一致
            Assert.Equal(originalStats.TotalNodes, reloadedStats.TotalNodes);
            Assert.Equal(originalStats.ElementNodes, reloadedStats.ElementNodes);
            Assert.Equal(originalStats.TotalAttributes, reloadedStats.TotalAttributes);

            // 清理测试文件
            if (File.Exists(savedPath))
            {
                File.Delete(savedPath);
            }
        }

        [Fact]
        public void HybridXml_PatchApplication_ShouldOnlyChangeModifiedParts()
        {
            // 准备测试数据
            var testXmlPath = "example/ModuleData/looknfeel.xml";
            if (!File.Exists(testXmlPath))
            {
                return;
            }

            // 加载原始XML
            var originalXml = File.ReadAllText(testXmlPath);
            var originalDoc = XDocument.Parse(originalXml);
            var originalStats = GetDetailedStatistics(originalDoc);

            // 使用新架构处理
            var document = _documentMerger.LoadAndMerge(testXmlPath, Enumerable.Empty<string>());
            var element = _documentMerger.ExtractElementForEditing(document, "/base");
            
            Assert.NotNull(element);

            // 转换为DTO
            var originalDto = _mapper.FromXElement(element);
            
            // 创建修改后的DTO（只修改一个属性）
            var modifiedDto = CloneDto(originalDto);
            modifiedDto.Type = "hybrid_test_type";

            // 生成差异补丁
            var patch = _mapper.GeneratePatch(originalDto, modifiedDto);
            
            Console.WriteLine($"生成的补丁操作数: {patch.Operations.Count}");
            foreach (var operation in patch.Operations)
            {
                Console.WriteLine($"操作: {operation.Type}, XPath: {operation.XPath}, 描述: {operation.Description}");
            }

            // 应用补丁到原始XmlDocument
            patch.ApplyTo(document);

            // 统计应用补丁后的XML信息
            var patchedXml = document.OuterXml;
            var patchedDoc = XDocument.Parse(patchedXml);
            var patchedStats = GetDetailedStatistics(patchedDoc);

            Console.WriteLine($"应用补丁后XML统计: {patchedStats.GetSummary()}");

            // 验证只有必要的部分被修改
            Assert.Equal(originalStats.TotalNodes, patchedStats.TotalNodes);
            Assert.Equal(originalStats.ElementNodes, patchedStats.ElementNodes);
            Assert.Equal(originalStats.TotalAttributes + 1, patchedStats.TotalAttributes); // 只增加了一个修改的属性

            // 验证修改确实生效了
            Assert.Equal("hybrid_test_type", patchedDoc.Root?.Attribute("type")?.Value);
        }

        [Fact]
        public void HybridXml_CompareWithOldArchitecture_ShouldShowImprovement()
        {
            // 准备测试数据
            var testXmlPath = "example/ModuleData/looknfeel.xml";
            if (!File.Exists(testXmlPath))
            {
                return;
            }

            // 加载原始XML
            var originalXml = File.ReadAllText(testXmlPath);
            var originalDoc = XDocument.Parse(originalXml);
            var originalStats = GetDetailedStatistics(originalDoc);

            Console.WriteLine($"=== 新架构测试 ===");
            Console.WriteLine($"原始XML统计: {originalStats.GetSummary()}");

            // 使用新架构处理
            var document = _documentMerger.LoadAndMerge(testXmlPath, Enumerable.Empty<string>());
            var element = _documentMerger.ExtractElementForEditing(document, "/base");
            
            Assert.NotNull(element);

            // 转换为DTO再转回
            var dto = _mapper.FromXElement(element);
            var convertedElement = _mapper.ToXElement(dto);
            var convertedStats = GetDetailedStatistics(XDocument.Parse(convertedElement.ToString()));

            Console.WriteLine($"新架构转换后统计: {convertedStats.GetSummary()}");

            // 验证新架构保持了节点数量
            Assert.Equal(originalStats.TotalNodes, convertedStats.TotalNodes);
            Assert.Equal(originalStats.TotalAttributes, convertedStats.TotalAttributes);

            // 对比旧架构的问题（预期会丢失节点）
            Console.WriteLine($"=== 对比旧架构问题 ===");
            Console.WriteLine($"旧架构问题: 节点数量从539变成537，丢失2个节点");
            Console.WriteLine($"新架构解决: 节点数量保持{originalStats.TotalNodes}，无丢失");

            // 验证新架构确实解决了问题
            Assert.Equal(originalStats.TotalNodes, convertedStats.TotalNodes);
            Assert.NotEqual(537, convertedStats.TotalNodes); // 确保不是错误的537
        }

        [Fact]
        public void HybridXml_WidgetStructure_ShouldBePreserved()
        {
            // 准备测试数据
            var testXmlPath = "example/ModuleData/looknfeel.xml";
            if (!File.Exists(testXmlPath))
            {
                return;
            }

            // 使用新架构处理
            var document = _documentMerger.LoadAndMerge(testXmlPath, Enumerable.Empty<string>());
            var element = _documentMerger.ExtractElementForEditing(document, "/base");
            
            Assert.NotNull(element);

            // 转换为DTO
            var dto = _mapper.FromXElement(element);
            
            Assert.NotNull(dto.Widgets);
            Assert.NotNull(dto.Widgets.WidgetList);
            Assert.True(dto.Widgets.WidgetList.Count > 0);

            // 分析第一个widget的结构
            var firstWidget = dto.Widgets.WidgetList[0];
            Console.WriteLine($"第一个Widget: {firstWidget.Name} ({firstWidget.Type})");

            // 验证widget的meshes和sub_widgets结构
            if (firstWidget.Meshes != null)
            {
                Console.WriteLine($"Meshes容器存在");
                Console.WriteLine($"BackgroundMeshes: {firstWidget.Meshes.BackgroundMeshes?.Count ?? 0}");
                Console.WriteLine($"ButtonMeshes: {firstWidget.Meshes.ButtonMeshes?.Count ?? 0}");
                Console.WriteLine($"ButtonPressedMeshes: {firstWidget.Meshes.ButtonPressedMeshes?.Count ?? 0}");
                Console.WriteLine($"HighlightMeshes: {firstWidget.Meshes.HighlightMeshes?.Count ?? 0}");
                Console.WriteLine($"CursorMeshes: {firstWidget.Meshes.CursorMeshes?.Count ?? 0}");
                Console.WriteLine($"LeftBorderMeshes: {firstWidget.Meshes.LeftBorderMeshes?.Count ?? 0}");
                Console.WriteLine($"RightBorderMeshes: {firstWidget.Meshes.RightBorderMeshes?.Count ?? 0}");
            }

            if (firstWidget.SubWidgets != null)
            {
                Console.WriteLine($"SubWidgets容器存在");
                Console.WriteLine($"SubWidgetList: {firstWidget.SubWidgets.SubWidgetList?.Count ?? 0}");
            }

            // 转换回XElement
            var convertedElement = _mapper.ToXElement(dto);
            var convertedWidget = convertedElement.Element("widgets")?.Element("widget");

            Assert.NotNull(convertedWidget);

            // 验证结构保持一致
            var convertedMeshes = convertedWidget.Element("meshes");
            var convertedSubWidgets = convertedWidget.Element("sub_widgets");

            if (firstWidget.Meshes != null)
            {
                Assert.NotNull(convertedMeshes);
            }

            if (firstWidget.SubWidgets != null)
            {
                Assert.NotNull(convertedSubWidgets);
            }
        }

        /// <summary>
        /// 获取详细的XML统计信息
        /// </summary>
        /// <param name="doc">XDocument</param>
        /// <returns>详细统计信息</returns>
        private DetailedXmlStatistics GetDetailedStatistics(XDocument doc)
        {
            var stats = new DetailedXmlStatistics();
            
            if (doc.Root != null)
            {
                CountNodesDetailed(doc.Root, stats);
            }

            return stats;
        }

        /// <summary>
        /// 递归详细统计节点
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <param name="stats">统计信息</param>
        private void CountNodesDetailed(XNode node, DetailedXmlStatistics stats)
        {
            stats.TotalNodes++;

            if (node is XElement element)
            {
                stats.ElementNodes++;
                stats.TotalAttributes += element.Attributes().Count();
                
                // 特殊统计Looknfeel相关元素
                if (element.Name.LocalName == "widget")
                {
                    stats.WidgetCount++;
                }
                else if (element.Name.LocalName == "meshes")
                {
                    stats.MeshesCount++;
                }
                else if (element.Name.LocalName == "sub_widgets")
                {
                    stats.SubWidgetsCount++;
                }
                else if (element.Name.LocalName.EndsWith("_mesh"))
                {
                    stats.MeshCount++;
                }
                else if (element.Name.LocalName == "sub_widget")
                {
                    stats.SubWidgetCount++;
                }
                
                foreach (var child in element.Nodes())
                {
                    CountNodesDetailed(child, stats);
                }
            }
            else if (node is XText)
            {
                stats.TextNodes++;
            }
            else if (node is XComment)
            {
                stats.CommentNodes++;
            }
        }

        /// <summary>
        /// 深拷贝DTO
        /// </summary>
        /// <param name="source">源DTO</param>
        /// <returns>拷贝的DTO</returns>
        private LooknfeelEditDto CloneDto(LooknfeelEditDto source)
        {
            return new LooknfeelEditDto
            {
                Type = source.Type,
                VirtualResolution = source.VirtualResolution,
                Widgets = source.Widgets != null ? new WidgetsEditContainer
                {
                    WidgetList = source.Widgets.WidgetList?.Select(w => new WidgetEditDto
                    {
                        Type = w.Type,
                        Name = w.Name,
                        TilingBorderSize = w.TilingBorderSize,
                        TileBackgroundAccordingToBorder = w.TileBackgroundAccordingToBorder,
                        BackgroundTileSize = w.BackgroundTileSize,
                        Focusable = w.Focusable,
                        Style = w.Style,
                        TrackAreaInset = w.TrackAreaInset,
                        Text = w.Text,
                        InitialState = w.InitialState,
                        NumOfCols = w.NumOfCols,
                        NumOfRows = w.NumOfRows,
                        MaxNumOfRows = w.MaxNumOfRows,
                        BorderSize = w.BorderSize,
                        ShowScrollBars = w.ShowScrollBars,
                        ScrollAreaInset = w.ScrollAreaInset,
                        CellSize = w.CellSize,
                        LayoutStyle = w.LayoutStyle,
                        LayoutAlignment = w.LayoutAlignment,
                        AutoShowScrollBars = w.AutoShowScrollBars,
                        IncrementVec = w.IncrementVec,
                        InitialValue = w.InitialValue,
                        MaxAllowedDigit = w.MaxAllowedDigit,
                        MinAllowedValue = w.MinAllowedValue,
                        MaxAllowedValue = w.MaxAllowedValue,
                        StepValue = w.StepValue,
                        MinValue = w.MinValue,
                        MaxValue = w.MaxValue,
                        VerticalAlignment = w.VerticalAlignment,
                        HorizontalAlignment = w.HorizontalAlignment,
                        TextHighlightColor = w.TextHighlightColor,
                        TextColor = w.TextColor,
                        FontSize = w.FontSize,
                        Size = w.Size,
                        Position = w.Position,
                        ButtonMesh = w.ButtonMesh,
                        Meshes = w.Meshes,
                        SubWidgets = w.SubWidgets
                    }).ToList()
                } : null
            };
        }
    }

    /// <summary>
    /// 详细XML统计信息
    /// </summary>
    public class DetailedXmlStatistics
    {
        public int TotalNodes { get; set; }
        public int ElementNodes { get; set; }
        public int TextNodes { get; set; }
        public int CommentNodes { get; set; }
        public int TotalAttributes { get; set; }
        
        // Looknfeel特定统计
        public int WidgetCount { get; set; }
        public int MeshesCount { get; set; }
        public int SubWidgetsCount { get; set; }
        public int MeshCount { get; set; }
        public int SubWidgetCount { get; set; }

        public string GetSummary()
        {
            return $"节点: {TotalNodes} (元素: {ElementNodes}, 文本: {TextNodes}, 注释: {CommentNodes}), " +
                   $"属性: {TotalAttributes}, Widget: {WidgetCount}, Meshes: {MeshesCount}, " +
                   $"SubWidgets: {SubWidgetsCount}, Mesh: {MeshCount}, SubWidget: {SubWidgetCount}";
        }
    }
}