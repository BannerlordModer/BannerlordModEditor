using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers;

public static class BeforeTransparentsGraphMapper
{
    public static BeforeTransparentsGraphDTO ToDTO(BeforeTransparentsGraphDO source)
    {
        if (source == null) return null;

        return new BeforeTransparentsGraphDTO
        {
            Type = source.Type,
            PostfxGraphs = BeforeTransparentsGraphMapper.ToDTO(source.PostfxGraphs)
        };
    }

    public static BeforeTransparentsGraphDO ToDO(BeforeTransparentsGraphDTO source)
    {
        if (source == null) return null;

        return new BeforeTransparentsGraphDO
        {
            Type = source.Type,
            PostfxGraphs = BeforeTransparentsGraphMapper.ToDO(source.PostfxGraphs)
        };
    }

    public static PostfxGraphsDTO ToDTO(PostfxGraphsDO source)
    {
        if (source == null) return null;

        return new PostfxGraphsDTO
        {
            PostfxGraphList = source.PostfxGraphList?
                .Select(BeforeTransparentsGraphMapper.ToDTO)
                .ToList() ?? new List<PostfxGraphDTO>()
        };
    }

    public static PostfxGraphsDO ToDO(PostfxGraphsDTO source)
    {
        if (source == null) return null;

        return new PostfxGraphsDO
        {
            PostfxGraphList = source.PostfxGraphList?
                .Select(BeforeTransparentsGraphMapper.ToDO)
                .ToList() ?? new List<PostfxGraphDO>()
        };
    }

    public static PostfxGraphDTO ToDTO(PostfxGraphDO source)
    {
        if (source == null) return null;

        return new PostfxGraphDTO
        {
            Id = source.Id,
            PostfxNodes = source.PostfxNodes?
                .Select(BeforeTransparentsGraphMapper.ToDTO)
                .ToList() ?? new List<PostfxNodeDTO>()
        };
    }

    public static PostfxGraphDO ToDO(PostfxGraphDTO source)
    {
        if (source == null) return null;

        return new PostfxGraphDO
        {
            Id = source.Id,
            PostfxNodes = source.PostfxNodes?
                .Select(BeforeTransparentsGraphMapper.ToDO)
                .ToList() ?? new List<PostfxNodeDO>()
        };
    }

    public static PostfxNodeDTO ToDTO(PostfxNodeDO source)
    {
        if (source == null) return null;

        return new PostfxNodeDTO
        {
            Id = source.Id,
            Class = source.Class,
            Shader = source.Shader,
            Format = source.Format,
            Size = source.Size,
            Width = source.Width,
            Height = source.Height,
            Outputs = source.Outputs?
                .Select(BeforeTransparentsGraphMapper.ToDTO)
                .ToList() ?? new List<PostfxNodeOutputDTO>(),
            Inputs = source.Inputs?
                .Select(BeforeTransparentsGraphMapper.ToDTO)
                .ToList() ?? new List<PostfxNodeInputDTO>(),
            Preconditions = BeforeTransparentsGraphMapper.ToDTO(source.Preconditions)
        };
    }

    public static PostfxNodeDO ToDO(PostfxNodeDTO source)
    {
        if (source == null) return null;

        return new PostfxNodeDO
        {
            Id = source.Id,
            Class = source.Class,
            Shader = source.Shader,
            Format = source.Format,
            Size = source.Size,
            Width = source.Width,
            Height = source.Height,
            Outputs = source.Outputs?
                .Select(BeforeTransparentsGraphMapper.ToDO)
                .ToList() ?? new List<PostfxNodeOutputDO>(),
            Inputs = source.Inputs?
                .Select(BeforeTransparentsGraphMapper.ToDO)
                .ToList() ?? new List<PostfxNodeInputDO>(),
            Preconditions = BeforeTransparentsGraphMapper.ToDO(source.Preconditions)
        };
    }

    public static PostfxNodeInputDTO ToDTO(PostfxNodeInputDO source)
    {
        if (source == null) return null;

        return new PostfxNodeInputDTO
        {
            Index = source.Index,
            Type = source.Type,
            Source = source.Source
        };
    }

    public static PostfxNodeInputDO ToDO(PostfxNodeInputDTO source)
    {
        if (source == null) return null;

        return new PostfxNodeInputDO
        {
            Index = source.Index,
            Type = source.Type,
            Source = source.Source
        };
    }

    public static PostfxNodeOutputDTO ToDTO(PostfxNodeOutputDO source)
    {
        if (source == null) return null;

        return new PostfxNodeOutputDTO
        {
            Index = source.Index,
            Type = source.Type,
            Name = source.Name
        };
    }

    public static PostfxNodeOutputDO ToDO(PostfxNodeOutputDTO source)
    {
        if (source == null) return null;

        return new PostfxNodeOutputDO
        {
            Index = source.Index,
            Type = source.Type,
            Name = source.Name
        };
    }

    public static PostfxNodePreconditionsDTO ToDTO(PostfxNodePreconditionsDO source)
    {
        if (source == null) return null;

        return new PostfxNodePreconditionsDTO
        {
            Configs = source.Configs?
                .Select(BeforeTransparentsGraphMapper.ToDTO)
                .ToList() ?? new List<PostfxNodeConfigDTO>()
        };
    }

    public static PostfxNodePreconditionsDO ToDO(PostfxNodePreconditionsDTO source)
    {
        if (source == null) return null;

        return new PostfxNodePreconditionsDO
        {
            Configs = source.Configs?
                .Select(BeforeTransparentsGraphMapper.ToDO)
                .ToList() ?? new List<PostfxNodeConfigDO>()
        };
    }

    public static PostfxNodeConfigDTO ToDTO(PostfxNodeConfigDO source)
    {
        if (source == null) return null;

        return new PostfxNodeConfigDTO
        {
            Name = source.Name
        };
    }

    public static PostfxNodeConfigDO ToDO(PostfxNodeConfigDTO source)
    {
        if (source == null) return null;

        return new PostfxNodeConfigDO
        {
            Name = source.Name
        };
    }
}