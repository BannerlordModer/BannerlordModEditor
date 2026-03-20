using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BannerlordModEditor.Common.Services
{
    public class TpacModelInfo
    {
        public string ModelId { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string ModelType { get; set; } = string.Empty;
        public Dictionary<string, string> Properties { get; set; } = new();
    }

    public class TpacFileInfo
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public List<TpacModelInfo> Models { get; set; } = new();
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public interface ITpacParserService
    {
        Task<TpacFileInfo> ParseTpacFileAsync(string filePath);
        TpacFileInfo ParseTpacFile(string filePath);
        List<TpacModelInfo> ExtractModels(string tpacFilePath);
        bool IsTpacFile(string filePath);
    }

    public class TpacParserService : ITpacParserService
    {
        private const string TpacExtension = ".tpac";

        public async Task<TpacFileInfo> ParseTpacFileAsync(string filePath)
        {
            return await Task.Run(() => ParseTpacFile(filePath));
        }

        public TpacFileInfo ParseTpacFile(string filePath)
        {
            var info = new TpacFileInfo
            {
                FilePath = filePath,
                FileName = Path.GetFileName(filePath)
            };

            if (!File.Exists(filePath))
            {
                info.IsValid = false;
                info.ErrorMessage = "File not found";
                return info;
            }

            if (!IsTpacFile(filePath))
            {
                info.IsValid = false;
                info.ErrorMessage = "Not a valid .tpac file";
                return info;
            }

            try
            {
                info.Models = ExtractModels(filePath);
                info.IsValid = true;
            }
            catch (Exception ex)
            {
                info.IsValid = false;
                info.ErrorMessage = ex.Message;
            }

            return info;
        }

        public List<TpacModelInfo> ExtractModels(string tpacFilePath)
        {
            var models = new List<TpacModelInfo>();

            if (!File.Exists(tpacFilePath))
                return models;

            try
            {
                var content = File.ReadAllText(tpacFilePath);
                
                var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                var currentModel = new TpacModelInfo();

                foreach (var line in lines)
                {
                    var trimmed = line.Trim();

                    if (trimmed.StartsWith("model_id:", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(currentModel.ModelId))
                        {
                            models.Add(currentModel);
                        }
                        currentModel = new TpacModelInfo
                        {
                            ModelId = trimmed["model_id:".Length..].Trim()
                        };
                    }
                    else if (trimmed.StartsWith("model_name:", StringComparison.OrdinalIgnoreCase))
                    {
                        currentModel.ModelName = trimmed["model_name:".Length..].Trim();
                    }
                    else if (trimmed.StartsWith("model_type:", StringComparison.OrdinalIgnoreCase))
                    {
                        currentModel.ModelType = trimmed["model_type:".Length..].Trim();
                    }
                    else if (trimmed.Contains(':'))
                    {
                        var parts = trimmed.Split(':', 2);
                        if (parts.Length == 2)
                        {
                            currentModel.Properties[parts[0].Trim()] = parts[1].Trim();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(currentModel.ModelId))
                {
                    models.Add(currentModel);
                }
            }
            catch
            {
            }

            return models;
        }

        public bool IsTpacFile(string filePath)
        {
            return Path.GetExtension(filePath).Equals(TpacExtension, StringComparison.OrdinalIgnoreCase);
        }
    }
}
