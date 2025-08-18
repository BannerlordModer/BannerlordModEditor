using System.Collections.Generic;

namespace BannerlordModEditor.Common.Services.HybridXml.Dto
{
    /// <summary>
    /// Looknfeel编辑DTO
    /// 专门用于编辑Looknfeel XML的强类型对象
    /// </summary>
    public class LooknfeelEditDto
    {
        /// <summary>
        /// 类型属性
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// 虚拟分辨率
        /// </summary>
        public string? VirtualResolution { get; set; }

        /// <summary>
        /// 小部件容器
        /// </summary>
        public WidgetsEditContainer? Widgets { get; set; }

        /// <summary>
        /// 验证DTO的结构完整性
        /// </summary>
        /// <returns>验证结果</returns>
        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(Type))
            {
                result.AddWarning("Type属性为空");
            }

            if (Widgets == null)
            {
                result.AddWarning("Widgets容器为空");
            }
            else
            {
                // 验证每个widget
                if (Widgets.WidgetList != null)
                {
                    for (int i = 0; i < Widgets.WidgetList.Count; i++)
                    {
                        var widget = Widgets.WidgetList[i];
                        var widgetResult = widget.Validate();
                        if (!widgetResult.IsValid)
                        {
                            result.AddError($"Widget[{i}] 验证失败: {string.Join(", ", widgetResult.Errors)}");
                        }
                        foreach (var warning in widgetResult.Warnings)
                        {
                            result.AddWarning($"Widget[{i}]: {warning}");
                        }
                    }
                }
            }

            result.IsValid = !result.Errors.Any();
            return result;
        }
    }

    /// <summary>
    /// 小部件容器
    /// </summary>
    public class WidgetsEditContainer
    {
        /// <summary>
        /// 小部件列表
        /// </summary>
        public List<WidgetEditDto>? WidgetList { get; set; }
    }

    /// <summary>
    /// 小部件编辑DTO
    /// </summary>
    public class WidgetEditDto
    {
        /// <summary>
        /// 小部件类型
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// 小部件名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 平铺边框大小
        /// </summary>
        public string? TilingBorderSize { get; set; }

        /// <summary>
        /// 根据边框平铺背景
        /// </summary>
        public string? TileBackgroundAccordingToBorder { get; set; }

        /// <summary>
        /// 背景平铺大小
        /// </summary>
        public string? BackgroundTileSize { get; set; }

        /// <summary>
        /// 是否可聚焦
        /// </summary>
        public string? Focusable { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        public string? Style { get; set; }

        /// <summary>
        /// 跟踪区域插入
        /// </summary>
        public string? TrackAreaInset { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// 初始状态
        /// </summary>
        public string? InitialState { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        public string? NumOfCols { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        public string? NumOfRows { get; set; }

        /// <summary>
        /// 最大行数
        /// </summary>
        public string? MaxNumOfRows { get; set; }

        /// <summary>
        /// 边框大小
        /// </summary>
        public string? BorderSize { get; set; }

        /// <summary>
        /// 显示滚动条
        /// </summary>
        public string? ShowScrollBars { get; set; }

        /// <summary>
        /// 滚动区域插入
        /// </summary>
        public string? ScrollAreaInset { get; set; }

        /// <summary>
        /// 单元格大小
        /// </summary>
        public string? CellSize { get; set; }

        /// <summary>
        /// 布局样式
        /// </summary>
        public string? LayoutStyle { get; set; }

        /// <summary>
        /// 布局对齐
        /// </summary>
        public string? LayoutAlignment { get; set; }

        /// <summary>
        /// 自动显示滚动条
        /// </summary>
        public string? AutoShowScrollBars { get; set; }

        /// <summary>
        /// 增量向量
        /// </summary>
        public string? IncrementVec { get; set; }

        /// <summary>
        /// 初始值
        /// </summary>
        public string? InitialValue { get; set; }

        /// <summary>
        /// 最大允许位数
        /// </summary>
        public string? MaxAllowedDigit { get; set; }

        /// <summary>
        /// 最小允许值
        /// </summary>
        public string? MinAllowedValue { get; set; }

        /// <summary>
        /// 最大允许值
        /// </summary>
        public string? MaxAllowedValue { get; set; }

        /// <summary>
        /// 步长值
        /// </summary>
        public string? StepValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public string? MinValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public string? MaxValue { get; set; }

        /// <summary>
        /// 垂直对齐
        /// </summary>
        public string? VerticalAlignment { get; set; }

        /// <summary>
        /// 水平对齐
        /// </summary>
        public string? HorizontalAlignment { get; set; }

        /// <summary>
        /// 文本高亮颜色
        /// </summary>
        public string? TextHighlightColor { get; set; }

        /// <summary>
        /// 文本颜色
        /// </summary>
        public string? TextColor { get; set; }

        /// <summary>
        /// 字体大小
        /// </summary>
        public string? FontSize { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public string? Size { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string? Position { get; set; }

        /// <summary>
        /// 按钮网格
        /// </summary>
        public string? ButtonMesh { get; set; }

        /// <summary>
        /// 网格容器
        /// </summary>
        public MeshesEditContainer? Meshes { get; set; }

        /// <summary>
        /// 子小部件容器
        /// </summary>
        public SubWidgetsEditContainer? SubWidgets { get; set; }

        /// <summary>
        /// 验证小部件的结构完整性
        /// </summary>
        /// <returns>验证结果</returns>
        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(Name))
            {
                result.AddWarning("Widget名称为空");
            }

            if (string.IsNullOrEmpty(Type))
            {
                result.AddWarning("Widget类型为空");
            }

            // 验证meshes
            if (Meshes != null)
            {
                var meshesResult = Meshes.Validate();
                if (!meshesResult.IsValid)
                {
                    result.AddError($"Meshes验证失败: {string.Join(", ", meshesResult.Errors)}");
                }
                foreach (var warning in meshesResult.Warnings)
                {
                    result.AddWarning($"Meshes: {warning}");
                }
            }

            // 验证sub_widgets
            if (SubWidgets != null)
            {
                var subWidgetsResult = SubWidgets.Validate();
                if (!subWidgetsResult.IsValid)
                {
                    result.AddError($"SubWidgets验证失败: {string.Join(", ", subWidgetsResult.Errors)}");
                }
                foreach (var warning in subWidgetsResult.Warnings)
                {
                    result.AddWarning($"SubWidgets: {warning}");
                }
            }

            result.IsValid = !result.Errors.Any();
            return result;
        }
    }

    /// <summary>
    /// 网格容器
    /// </summary>
    public class MeshesEditContainer
    {
        /// <summary>
        /// 背景网格列表
        /// </summary>
        public List<MeshEditDto>? BackgroundMeshes { get; set; }

        /// <summary>
        /// 按钮网格列表
        /// </summary>
        public List<MeshEditDto>? ButtonMeshes { get; set; }

        /// <summary>
        /// 按按下网格列表
        /// </summary>
        public List<MeshEditDto>? ButtonPressedMeshes { get; set; }

        /// <summary>
        /// 高亮网格列表
        /// </summary>
        public List<MeshEditDto>? HighlightMeshes { get; set; }

        /// <summary>
        /// 光标网格列表
        /// </summary>
        public List<MeshEditDto>? CursorMeshes { get; set; }

        /// <summary>
        /// 左边框网格列表
        /// </summary>
        public List<MeshEditDto>? LeftBorderMeshes { get; set; }

        /// <summary>
        /// 右边框网格列表
        /// </summary>
        public List<MeshEditDto>? RightBorderMeshes { get; set; }

        /// <summary>
        /// 验证网格容器的结构完整性
        /// </summary>
        /// <returns>验证结果</returns>
        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            // 验证各种网格类型
            ValidateMeshList(BackgroundMeshes, "BackgroundMeshes", result);
            ValidateMeshList(ButtonMeshes, "ButtonMeshes", result);
            ValidateMeshList(ButtonPressedMeshes, "ButtonPressedMeshes", result);
            ValidateMeshList(HighlightMeshes, "HighlightMeshes", result);
            ValidateMeshList(CursorMeshes, "CursorMeshes", result);
            ValidateMeshList(LeftBorderMeshes, "LeftBorderMeshes", result);
            ValidateMeshList(RightBorderMeshes, "RightBorderMeshes", result);

            result.IsValid = !result.Errors.Any();
            return result;
        }

        /// <summary>
        /// 验证网格列表
        /// </summary>
        /// <param name="meshList">网格列表</param>
        /// <param name="listName">列表名称</param>
        /// <param name="result">验证结果</param>
        private void ValidateMeshList(List<MeshEditDto>? meshList, string listName, ValidationResult result)
        {
            if (meshList == null) return;

            for (int i = 0; i < meshList.Count; i++)
            {
                var mesh = meshList[i];
                var meshResult = mesh.Validate();
                if (!meshResult.IsValid)
                {
                    result.AddError($"{listName}[{i}] 验证失败: {string.Join(", ", meshResult.Errors)}");
                }
                foreach (var warning in meshResult.Warnings)
                {
                    result.AddWarning($"{listName}[{i}]: {warning}");
                }
            }
        }
    }

    /// <summary>
    /// 网格编辑DTO
    /// </summary>
    public class MeshEditDto
    {
        /// <summary>
        /// 网格名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 平铺
        /// </summary>
        public string? Tiling { get; set; }

        /// <summary>
        /// 主网格
        /// </summary>
        public string? MainMesh { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string? Position { get; set; }

        /// <summary>
        /// 验证网格的结构完整性
        /// </summary>
        /// <returns>验证结果</returns>
        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(Name))
            {
                result.AddWarning("网格名称为空");
            }

            result.IsValid = !result.Errors.Any();
            return result;
        }
    }

    /// <summary>
    /// 子小部件容器
    /// </summary>
    public class SubWidgetsEditContainer
    {
        /// <summary>
        /// 子小部件列表
        /// </summary>
        public List<WidgetEditDto>? SubWidgetList { get; set; }

        /// <summary>
        /// 验证子小部件容器的结构完整性
        /// </summary>
        /// <returns>验证结果</returns>
        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (SubWidgetList != null)
            {
                for (int i = 0; i < SubWidgetList.Count; i++)
                {
                    var subWidget = SubWidgetList[i];
                    var subWidgetResult = subWidget.Validate();
                    if (!subWidgetResult.IsValid)
                    {
                        result.AddError($"SubWidget[{i}] 验证失败: {string.Join(", ", subWidgetResult.Errors)}");
                    }
                    foreach (var warning in subWidgetResult.Warnings)
                    {
                        result.AddWarning($"SubWidget[{i}]: {warning}");
                    }
                }
            }

            result.IsValid = !result.Errors.Any();
            return result;
        }
    }
}