using BannerlordModEditor.Common.Models.DO.Engine;
using BannerlordModEditor.Common.Models.DTO.Engine;

namespace BannerlordModEditor.Common.Mappers;

public static class PostfxGraphsMapper
{
    public static PostfxGraphsDTO ToDTO(PostfxGraphsDO source)
    {
        if (source == null) return null;

        return new PostfxGraphsDTO
        {
            Type = source.Type,
            PostfxGraphs = PostfxGraphsContainerMapper.ToDTO(source.PostfxGraphs)
        };
    }

    public static PostfxGraphsDO ToDO(PostfxGraphsDTO source)
    {
        if (source == null) return null;

        return new PostfxGraphsDO
        {
            Type = source.Type,
            PostfxGraphs = PostfxGraphsContainerMapper.ToDO(source.PostfxGraphs),
            HasPostfxGraphs = source.PostfxGraphs != null && source.PostfxGraphs.Graphs.Count > 0
        };
    }
}

public static class PostfxGraphsContainerMapper
{
    public static PostfxGraphsContainerDTO ToDTO(PostfxGraphsContainerDO source)
    {
        if (source == null) return null;

        return new PostfxGraphsContainerDTO
        {
            Graphs = source.Graphs?
                .Select(PostfxGraphMapper.ToDTO)
                .ToList() ?? new List<PostfxGraphDTO>()
        };
    }

    public static PostfxGraphsContainerDO ToDO(PostfxGraphsContainerDTO source)
    {
        if (source == null) return null;

        return new PostfxGraphsContainerDO
        {
            Graphs = source.Graphs?
                .Select(PostfxGraphMapper.ToDO)
                .ToList() ?? new List<PostfxGraphDO>()
        };
    }
}

public static class PostfxGraphMapper
{
    public static PostfxGraphDTO ToDTO(PostfxGraphDO source)
    {
        if (source == null) return null;

        return new PostfxGraphDTO
        {
            Id = source.Id,
            Nodes = source.Nodes?
                .Select(PostfxNodeMapper.ToDTO)
                .ToList() ?? new List<PostfxNodeDTO>()
        };
    }

    public static PostfxGraphDO ToDO(PostfxGraphDTO source)
    {
        if (source == null) return null;

        return new PostfxGraphDO
        {
            Id = source.Id,
            Nodes = source.Nodes?
                .Select(PostfxNodeMapper.ToDO)
                .ToList() ?? new List<PostfxNodeDO>()
        };
    }
}

public static class PostfxNodeMapper
{
    public static PostfxNodeDTO ToDTO(PostfxNodeDO source)
    {
        if (source == null) return null;

        return new PostfxNodeDTO
        {
            Id = source.Id,
            Shader = source.Shader,
            Class = source.Class,
            Format = source.Format,
            Size = source.Size,
            Width = source.Width,
            Height = source.Height,
            Outputs = source.Outputs?
                .Select(PostfxOutputMapper.ToDTO)
                .ToList() ?? new List<PostfxOutputDTO>(),
            Inputs = source.Inputs?
                .Select(PostfxInputMapper.ToDTO)
                .ToList() ?? new List<PostfxInputDTO>(),
            Preconditions = PostfxPreconditionsMapper.ToDTO(source.Preconditions)
        };
    }

    public static PostfxNodeDO ToDO(PostfxNodeDTO source)
    {
        if (source == null) return null;

        return new PostfxNodeDO
        {
            Id = source.Id,
            Shader = source.Shader,
            Class = source.Class,
            Format = source.Format,
            Size = source.Size,
            Width = source.Width,
            Height = source.Height,
            Outputs = source.Outputs?
                .Select(PostfxOutputMapper.ToDO)
                .ToList() ?? new List<PostfxOutputDO>(),
            Inputs = source.Inputs?
                .Select(PostfxInputMapper.ToDO)
                .ToList() ?? new List<PostfxInputDO>(),
            Preconditions = PostfxPreconditionsMapper.ToDO(source.Preconditions),
            HasPreconditions = source.Preconditions != null && source.Preconditions.Configs.Count > 0
        };
    }
}

public static class PostfxOutputMapper
{
    public static PostfxOutputDTO ToDTO(PostfxOutputDO source)
    {
        if (source == null) return null;

        return new PostfxOutputDTO
        {
            Index = source.Index,
            Type = source.Type,
            Name = source.Name
        };
    }

    public static PostfxOutputDO ToDO(PostfxOutputDTO source)
    {
        if (source == null) return null;

        return new PostfxOutputDO
        {
            Index = source.Index,
            Type = source.Type,
            Name = source.Name
        };
    }
}

public static class PostfxInputMapper
{
    public static PostfxInputDTO ToDTO(PostfxInputDO source)
    {
        if (source == null) return null;

        return new PostfxInputDTO
        {
            Index = source.Index,
            Type = source.Type,
            Source = source.Source
        };
    }

    public static PostfxInputDO ToDO(PostfxInputDTO source)
    {
        if (source == null) return null;

        return new PostfxInputDO
        {
            Index = source.Index,
            Type = source.Type,
            Source = source.Source
        };
    }
}

public static class PostfxPreconditionsMapper
{
    public static PostfxPreconditionsDTO ToDTO(PostfxPreconditionsDO source)
    {
        if (source == null) return null;

        return new PostfxPreconditionsDTO
        {
            Configs = source.Configs?
                .Select(PostfxConfigMapper.ToDTO)
                .ToList() ?? new List<PostfxConfigDTO>()
        };
    }

    public static PostfxPreconditionsDO ToDO(PostfxPreconditionsDTO source)
    {
        if (source == null) return null;

        return new PostfxPreconditionsDO
        {
            Configs = source.Configs?
                .Select(PostfxConfigMapper.ToDO)
                .ToList() ?? new List<PostfxConfigDO>()
        };
    }
}

public static class PostfxConfigMapper
{
    public static PostfxConfigDTO ToDTO(PostfxConfigDO source)
    {
        if (source == null) return null;

        return new PostfxConfigDTO
        {
            Name = source.Name
        };
    }

    public static PostfxConfigDO ToDO(PostfxConfigDTO source)
    {
        if (source == null) return null;

        return new PostfxConfigDO
        {
            Name = source.Name
        };
    }
}