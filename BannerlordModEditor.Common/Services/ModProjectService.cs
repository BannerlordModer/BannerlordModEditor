using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.SubModule;

namespace BannerlordModEditor.Common.Services
{
    public class ModProjectOptions
    {
        public string ModName { get; set; } = string.Empty;
        public string ModId { get; set; } = string.Empty;
        public string OutputDirectory { get; set; } = string.Empty;
        public bool UseButrTemplate { get; set; } = true;
        public string GameVersion { get; set; } = "Latest";
        public bool CreateSubModuleXml { get; set; } = true;
        public bool CreateCsproj { get; set; } = true;
        public bool CreateModuleDataFolder { get; set; } = true;
        public List<string> DependedModules { get; set; } = new() { "Native" };
    }

    public class ModProjectResult
    {
        public bool Success { get; set; }
        public string ProjectPath { get; set; } = string.Empty;
        public List<string> CreatedFiles { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }

    public interface IModProjectService
    {
        Task<ModProjectResult> CreateModProjectAsync(ModProjectOptions options);
        ModProjectResult CreateModProject(ModProjectOptions options);
        bool ValidateModName(string modName);
        string GenerateModId(string modName);
    }

    public class ModProjectService : IModProjectService
    {
        private readonly ISubModuleLoader _subModuleLoader;

        public ModProjectService(ISubModuleLoader subModuleLoader)
        {
            _subModuleLoader = subModuleLoader;
        }

        public async Task<ModProjectResult> CreateModProjectAsync(ModProjectOptions options)
        {
            return await Task.Run(() => CreateModProject(options));
        }

        public ModProjectResult CreateModProject(ModProjectOptions options)
        {
            var result = new ModProjectResult();

            try
            {
                if (!ValidateModName(options.ModName))
                {
                    result.Success = false;
                    result.ErrorMessage = "Invalid mod name. Use only letters, numbers, and underscores.";
                    result.Errors.Add(result.ErrorMessage);
                    return result;
                }

                var modId = string.IsNullOrEmpty(options.ModId) 
                    ? GenerateModId(options.ModName) 
                    : options.ModId;

                var projectPath = Path.Combine(options.OutputDirectory, options.ModName);
                if (Directory.Exists(projectPath))
                {
                    result.Success = false;
                    result.ErrorMessage = $"Directory already exists: {projectPath}";
                    result.Errors.Add(result.ErrorMessage);
                    return result;
                }

                Directory.CreateDirectory(projectPath);
                result.ProjectPath = projectPath;

                if (options.CreateSubModuleXml)
                {
                    var subModule = _subModuleLoader.CreateDefault(options.ModName, modId);
                    subModule.DependedModules = options.DependedModules
                        .Select(d => new Models.SubModule.DependedModuleDO { Id = d })
                        .ToList();

                    var subModulePath = Path.Combine(projectPath, "SubModule.xml");
                    _subModuleLoader.Save(subModule, subModulePath);
                    result.CreatedFiles.Add(subModulePath);
                }

                if (options.CreateCsproj)
                {
                    var csprojPath = Path.Combine(projectPath, $"{options.ModName}.csproj");
                    var csprojContent = GenerateCsprojContent(options.ModName, modId, options.UseButrTemplate);
                    File.WriteAllText(csprojPath, csprojContent);
                    result.CreatedFiles.Add(csprojPath);
                }

                if (options.CreateModuleDataFolder)
                {
                    var moduleDataPath = Path.Combine(projectPath, "ModuleData");
                    Directory.CreateDirectory(moduleDataPath);
                    result.CreatedFiles.Add(moduleDataPath);

                    var emptyXmlPath = Path.Combine(moduleDataPath, "example.xml");
                    File.WriteAllText(emptyXmlPath, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<root>\n</root>");
                    result.CreatedFiles.Add(emptyXmlPath);
                }

                var binPath = Path.Combine(projectPath, "bin");
                Directory.CreateDirectory(binPath);

                var objPath = Path.Combine(projectPath, "obj");
                Directory.CreateDirectory(objPath);

                var gitignorePath = Path.Combine(projectPath, ".gitignore");
                File.WriteAllText(gitignorePath, "bin/\nobj/\n*.user\n.vs/");
                result.CreatedFiles.Add(gitignorePath);

                var readmePath = Path.Combine(projectPath, "README.md");
                File.WriteAllText(readmePath, $"# {options.ModName}\n\nA Bannerlord Mod created with BannerlordModEditor.");
                result.CreatedFiles.Add(readmePath);

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.Errors.Add(ex.Message);
            }

            return result;
        }

        public bool ValidateModName(string modName)
        {
            if (string.IsNullOrWhiteSpace(modName))
                return false;

            if (modName.Length < 2 || modName.Length > 50)
                return false;

            return modName.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-');
        }

        public string GenerateModId(string modName)
        {
            var id = modName.Replace(" ", "").Replace("-", "");
            return id.Length > 0 ? id : "Mod";
        }

        private string GenerateCsprojContent(string modName, string modId, bool useButrTemplate)
        {
            var targetFramework = "net9.0";
            var packageReferences = useButrTemplate 
                ? @"
    <PackageReference Include=""Bannerlord.ButterLib"" Version=""2.*"" />
    <PackageReference Include=""Bannerlord.Harmony"" Version=""2.*"" />"
                : "";

            return $@"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>{targetFramework}</TargetFramework>
    <AssemblyName>{modName}</AssemblyName>
    <RootNamespace>{modName}</RootNamespace>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""Bannerlord.MountAndBlade"" Version=""1.*"" />{packageReferences}
  </ItemGroup>

</Project>";
        }
    }
}
