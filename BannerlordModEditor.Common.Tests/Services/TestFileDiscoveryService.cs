using BannerlordModEditor.Common.Services;
using System;
using System.Threading.Tasks;

namespace BannerlordModEditor.Common.Tests.Services
{
    /// <summary>
    /// 用于测试文件发现服务的简单控制台程序
    /// </summary>
    public class TestFileDiscoveryService
    {
        public static async Task RunTest()
        {
            try
            {
                var solutionRoot = TestUtils.GetSolutionRoot();
                var xmlDirectory = System.IO.Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData");
                var modelsDirectory = System.IO.Path.Combine(solutionRoot, "BannerlordModEditor.Common", "Models");
                
                var service = new FileDiscoveryService(xmlDirectory, modelsDirectory);
                var unadaptedFiles = await service.FindUnadaptedFilesAsync();

                Console.WriteLine($"发现 {unadaptedFiles.Count} 个未适配的 XML 文件：");
                Console.WriteLine();

                foreach (var file in unadaptedFiles)
                {
                    Console.WriteLine($"文件: {file.FileName}");
                    Console.WriteLine($"  预期模型名: {file.ExpectedModelName}");
                    Console.WriteLine($"  文件大小: {file.FileSize:N0} 字节");
                    Console.WriteLine($"  复杂度: {file.Complexity}");
                    Console.WriteLine($"  需要分块: {(file.RequiresChunking ? "是" : "否")}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
            }
        }
    }
}