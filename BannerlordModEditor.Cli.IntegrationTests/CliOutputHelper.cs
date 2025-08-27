using System.Text.RegularExpressions;

namespace BannerlordModEditor.Cli.IntegrationTests
{
    /// <summary>
    /// CLI输出格式标准化助手类
    /// </summary>
    public static class CliOutputHelper
    {
        /// <summary>
        /// 标准化成功消息格式
        /// </summary>
        public static string StandardizeSuccessMessage(string operation)
        {
            return $"✓ {operation}成功";
        }
        
        /// <summary>
        /// 标准化错误消息格式
        /// </summary>
        public static string StandardizeErrorMessage(string operation)
        {
            return $"✗ {operation}失败";
        }
        
        /// <summary>
        /// 获取标准化的识别成功消息
        /// </summary>
        public static string GetRecognizeSuccessMessage(string modelType)
        {
            return $"✓ 识别成功: {modelType}";
        }
        
        /// <summary>
        /// 获取标准化的转换成功消息
        /// </summary>
        public static string GetConvertSuccessMessage(string fromFormat, string toFormat)
        {
            return $"✓ {fromFormat} 转 {toFormat} 转换成功";
        }
        
        /// <summary>
        /// 获取标准化的验证通过消息
        /// </summary>
        public static string GetValidationSuccessMessage(string format)
        {
            return $"✓ {format} 格式验证通过";
        }
        
        /// <summary>
        /// 从输出中提取模型类型
        /// </summary>
        public static string? ExtractModelTypeFromOutput(string output)
        {
            var match = Regex.Match(output, @"✓ 识别成功: (.+)");
            return match.Success ? match.Groups[1].Value.Trim() : null;
        }
        
        /// <summary>
        /// 从输出中提取模型类型列表
        /// </summary>
        public static List<string> ExtractModelTypesFromOutput(string output)
        {
            var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var modelTypes = new List<string>();
            
            var inModelList = false;
            var passedHeader = false;
            foreach (var line in lines)
            {
                if (line.Contains("支持的模型类型:"))
                {
                    inModelList = true;
                    continue;
                }
                
                if (inModelList)
                {
                    // 跳过第一个"---"分隔线
                    if (line.StartsWith("---") && !passedHeader)
                    {
                        passedHeader = true;
                        continue;
                    }
                    
                    // 遇到第二个"---"或"总计:"时停止
                    if (line.StartsWith("---") && passedHeader || line.StartsWith("总计:"))
                    {
                        break;
                    }
                    
                    if (line.StartsWith("- "))
                    {
                        modelTypes.Add(line.Substring(2).Trim());
                    }
                }
            }
            
            return modelTypes;
        }
    }
}