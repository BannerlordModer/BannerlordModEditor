using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class LooknfeelMapper
    {
        public static LooknfeelDTO ToDTO(LooknfeelDO source)
        {
            if (source == null) return null;

            return new LooknfeelDTO
            {
                Type = source.Type,
                VirtualResolution = source.VirtualResolution,
                Widgets = WidgetsContainerMapper.ToDTO(source.Widgets)
            };
        }

        public static LooknfeelDO ToDO(LooknfeelDTO source)
        {
            if (source == null) return null;

            return new LooknfeelDO
            {
                Type = source.Type,
                VirtualResolution = source.VirtualResolution,
                Widgets = WidgetsContainerMapper.ToDO(source.Widgets)
            };
        }
    }

    public static class WidgetsContainerMapper
    {
        public static WidgetsContainerDTO ToDTO(WidgetsContainerDO source)
        {
            if (source == null) return null;

            return new WidgetsContainerDTO
            {
                WidgetList = source.WidgetList?
                    .Select(WidgetMapper.ToDTO)
                    .ToList() ?? new List<WidgetDTO>()
            };
        }

        public static WidgetsContainerDO ToDO(WidgetsContainerDTO source)
        {
            if (source == null) return null;

            return new WidgetsContainerDO
            {
                WidgetList = source.WidgetList?
                    .Select(WidgetMapper.ToDO)
                    .ToList() ?? new List<WidgetDO>()
            };
        }
    }

    public static class WidgetMapper
    {
        public static WidgetDTO ToDTO(WidgetDO source)
        {
            if (source == null) return null;

            return new WidgetDTO
            {
                Type = source.Type,
                Name = source.Name,
                TilingBorderSize = source.TilingBorderSize,
                TileBackgroundAccordingToBorder = source.TileBackgroundAccordingToBorder,
                BackgroundTileSize = source.BackgroundTileSize,
                Focusable = source.Focusable,
                Style = source.Style,
                TrackAreaInset = source.TrackAreaInset,
                Text = source.Text,
                InitialState = source.InitialState,
                NumOfCols = source.NumOfCols,
                NumOfRows = source.NumOfRows,
                MaxNumOfRows = source.MaxNumOfRows,
                BorderSize = source.BorderSize,
                ShowScrollBars = source.ShowScrollBars,
                ScrollAreaInset = source.ScrollAreaInset,
                CellSize = source.CellSize,
                LayoutStyle = source.LayoutStyle,
                LayoutAlignment = source.LayoutAlignment,
                AutoShowScrollBars = source.AutoShowScrollBars,
                IncrementVec = source.IncrementVec,
                InitialValue = source.InitialValue,
                MaxAllowedDigit = source.MaxAllowedDigit,
                MinAllowedValue = source.MinAllowedValue,
                MaxAllowedValue = source.MaxAllowedValue,
                StepValue = source.StepValue,
                MinValue = source.MinValue,
                MaxValue = source.MaxValue,
                VerticalAlignment = source.VerticalAlignment,
                HorizontalAlignment = source.HorizontalAlignment,
                HorizontalAligment = source.HorizontalAligment,
                TextHighlightColor = source.TextHighlightColor,
                TextColor = source.TextColor,
                FontSize = source.FontSize,
                Size = source.Size,
                Position = source.Position,
                Meshes = LooknfeelMeshesContainerMapper.ToDTO(source.Meshes),
                ButtonMesh = source.ButtonMesh,
                SubWidgetsList = source.SubWidgetsList?
                    .Select(SubWidgetsContainerMapper.ToDTO)
                    .ToList() ?? new List<SubWidgetsContainerDTO>()
            };
        }

        public static WidgetDO ToDO(WidgetDTO source)
        {
            if (source == null) return null;

            return new WidgetDO
            {
                Type = source.Type,
                Name = source.Name,
                TilingBorderSize = source.TilingBorderSize,
                TileBackgroundAccordingToBorder = source.TileBackgroundAccordingToBorder,
                BackgroundTileSize = source.BackgroundTileSize,
                Focusable = source.Focusable,
                Style = source.Style,
                TrackAreaInset = source.TrackAreaInset,
                Text = source.Text,
                InitialState = source.InitialState,
                NumOfCols = source.NumOfCols,
                NumOfRows = source.NumOfRows,
                MaxNumOfRows = source.MaxNumOfRows,
                BorderSize = source.BorderSize,
                ShowScrollBars = source.ShowScrollBars,
                ScrollAreaInset = source.ScrollAreaInset,
                CellSize = source.CellSize,
                LayoutStyle = source.LayoutStyle,
                LayoutAlignment = source.LayoutAlignment,
                AutoShowScrollBars = source.AutoShowScrollBars,
                IncrementVec = source.IncrementVec,
                InitialValue = source.InitialValue,
                MaxAllowedDigit = source.MaxAllowedDigit,
                MinAllowedValue = source.MinAllowedValue,
                MaxAllowedValue = source.MaxAllowedValue,
                StepValue = source.StepValue,
                MinValue = source.MinValue,
                MaxValue = source.MaxValue,
                VerticalAlignment = source.VerticalAlignment,
                HorizontalAlignment = source.HorizontalAlignment,
                HorizontalAligment = source.HorizontalAligment,
                TextHighlightColor = source.TextHighlightColor,
                TextColor = source.TextColor,
                FontSize = source.FontSize,
                Size = source.Size,
                Position = source.Position,
                Meshes = LooknfeelMeshesContainerMapper.ToDO(source.Meshes),
                ButtonMesh = source.ButtonMesh,
                SubWidgetsList = source.SubWidgetsList?
                    .Select(SubWidgetsContainerMapper.ToDO)
                    .ToList() ?? new List<SubWidgetsContainerDO>()
            };
        }
    }

    public static class LooknfeelMeshesContainerMapper
    {
        public static LooknfeelMeshesContainerDTO ToDTO(LooknfeelMeshesContainerDO source)
        {
            if (source == null) return null;

            return new LooknfeelMeshesContainerDTO
            {
                BackgroundMeshes = source.BackgroundMeshes?
                    .Select(LooknfeelMeshMapper.ToDTO)
                    .ToList() ?? new List<LooknfeelMeshDTO>(),
                ButtonMeshes = source.ButtonMeshes?
                    .Select(LooknfeelMeshMapper.ToDTO)
                    .ToList() ?? new List<LooknfeelMeshDTO>(),
                ButtonPressedMeshes = source.ButtonPressedMeshes?
                    .Select(LooknfeelMeshMapper.ToDTO)
                    .ToList() ?? new List<LooknfeelMeshDTO>(),
                HighlightMeshes = source.HighlightMeshes?
                    .Select(LooknfeelMeshMapper.ToDTO)
                    .ToList() ?? new List<LooknfeelMeshDTO>(),
                CursorMeshes = source.CursorMeshes?
                    .Select(LooknfeelMeshMapper.ToDTO)
                    .ToList() ?? new List<LooknfeelMeshDTO>(),
                LeftBorderMeshes = source.LeftBorderMeshes?
                    .Select(LooknfeelMeshMapper.ToDTO)
                    .ToList() ?? new List<LooknfeelMeshDTO>(),
                RightBorderMeshes = source.RightBorderMeshes?
                    .Select(LooknfeelMeshMapper.ToDTO)
                    .ToList() ?? new List<LooknfeelMeshDTO>()
            };
        }

        public static LooknfeelMeshesContainerDO ToDO(LooknfeelMeshesContainerDTO source)
        {
            if (source == null) return null;

            return new LooknfeelMeshesContainerDO
            {
                BackgroundMeshes = source.BackgroundMeshes?
                    .Select(LooknfeelMeshMapper.ToDO)
                    .ToList() ?? new List<LooknfeelMeshDO>(),
                ButtonMeshes = source.ButtonMeshes?
                    .Select(LooknfeelMeshMapper.ToDO)
                    .ToList() ?? new List<LooknfeelMeshDO>(),
                ButtonPressedMeshes = source.ButtonPressedMeshes?
                    .Select(LooknfeelMeshMapper.ToDO)
                    .ToList() ?? new List<LooknfeelMeshDO>(),
                HighlightMeshes = source.HighlightMeshes?
                    .Select(LooknfeelMeshMapper.ToDO)
                    .ToList() ?? new List<LooknfeelMeshDO>(),
                CursorMeshes = source.CursorMeshes?
                    .Select(LooknfeelMeshMapper.ToDO)
                    .ToList() ?? new List<LooknfeelMeshDO>(),
                LeftBorderMeshes = source.LeftBorderMeshes?
                    .Select(LooknfeelMeshMapper.ToDO)
                    .ToList() ?? new List<LooknfeelMeshDO>(),
                RightBorderMeshes = source.RightBorderMeshes?
                    .Select(LooknfeelMeshMapper.ToDO)
                    .ToList() ?? new List<LooknfeelMeshDO>()
            };
        }
    }

    public static class LooknfeelMeshMapper
    {
        public static LooknfeelMeshDTO ToDTO(LooknfeelMeshDO source)
        {
            if (source == null) return null;

            return new LooknfeelMeshDTO
            {
                Name = source.Name,
                Tiling = source.Tiling,
                MainMesh = source.MainMesh,
                Position = source.Position
            };
        }

        public static LooknfeelMeshDO ToDO(LooknfeelMeshDTO source)
        {
            if (source == null) return null;

            return new LooknfeelMeshDO
            {
                Name = source.Name,
                Tiling = source.Tiling,
                MainMesh = source.MainMesh,
                Position = source.Position
            };
        }
    }

    public static class SubWidgetsContainerMapper
    {
        public static SubWidgetsContainerDTO ToDTO(SubWidgetsContainerDO source)
        {
            if (source == null) return null;

            return new SubWidgetsContainerDTO
            {
                SubWidgetList = source.SubWidgetList?
                    .Select(SubWidgetMapper.ToDTO)
                    .ToList() ?? new List<SubWidgetDTO>()
            };
        }

        public static SubWidgetsContainerDO ToDO(SubWidgetsContainerDTO source)
        {
            if (source == null) return null;

            return new SubWidgetsContainerDO
            {
                SubWidgetList = source.SubWidgetList?
                    .Select(SubWidgetMapper.ToDO)
                    .ToList() ?? new List<SubWidgetDO>()
            };
        }
    }

    public static class SubWidgetMapper
    {
        public static SubWidgetDTO ToDTO(SubWidgetDO source)
        {
            if (source == null) return null;

            return new SubWidgetDTO
            {
                Ref = source.Ref,
                Name = source.Name,
                Size = source.Size,
                Position = source.Position,
                Style = source.Style,
                VerticalAlignment = source.VerticalAlignment,
                HorizontalAlignment = source.HorizontalAlignment,
                HorizontalAligment = source.HorizontalAligment,
                ScrollSpeed = source.ScrollSpeed,
                CellSize = source.CellSize,
                LayoutStyle = source.LayoutStyle,
                LayoutAlignment = source.LayoutAlignment,
                Text = source.Text,
                TextColor = source.TextColor,
                TextHighlightColor = source.TextHighlightColor,
                FontSize = source.FontSize,
                Meshes = LooknfeelMeshesContainerMapper.ToDTO(source.Meshes),
                SubWidgetsList = source.SubWidgetsList?
                    .Select(SubWidgetsContainerMapper.ToDTO)
                    .ToList() ?? new List<SubWidgetsContainerDTO>()
            };
        }

        public static SubWidgetDO ToDO(SubWidgetDTO source)
        {
            if (source == null) return null;

            return new SubWidgetDO
            {
                Ref = source.Ref,
                Name = source.Name,
                Size = source.Size,
                Position = source.Position,
                Style = source.Style,
                VerticalAlignment = source.VerticalAlignment,
                HorizontalAlignment = source.HorizontalAlignment,
                HorizontalAligment = source.HorizontalAligment,
                ScrollSpeed = source.ScrollSpeed,
                CellSize = source.CellSize,
                LayoutStyle = source.LayoutStyle,
                LayoutAlignment = source.LayoutAlignment,
                Text = source.Text,
                TextColor = source.TextColor,
                TextHighlightColor = source.TextHighlightColor,
                FontSize = source.FontSize,
                Meshes = LooknfeelMeshesContainerMapper.ToDO(source.Meshes),
                SubWidgetsList = source.SubWidgetsList?
                    .Select(SubWidgetsContainerMapper.ToDO)
                    .ToList() ?? new List<SubWidgetsContainerDO>()
            };
        }
    }
}