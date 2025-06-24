using Xunit;
using Xunit.Abstractions;

namespace BannerlordModEditor.Common.Tests
{
    public class DebugCreditsTest
    {
        private readonly ITestOutputHelper _output;

        public DebugCreditsTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void DebugCreditsSerializationIssue()
        {
            // 重定向控制台输出到测试输出
            var originalOut = System.Console.Out;
            using (var stringWriter = new System.IO.StringWriter())
            {
                System.Console.SetOut(stringWriter);
                
                DebugCredits.DebugCreditsSerializationDifference();
                
                System.Console.SetOut(originalOut);
                
                _output.WriteLine(stringWriter.ToString());
            }
        }
    }
} 