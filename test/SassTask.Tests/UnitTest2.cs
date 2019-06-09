using System;
using System.IO;
using System.Text;
using Xunit;
using Sass;
using System.Threading.Tasks;

namespace SassTask.Tests
{
    public class UnitTest2
    {
        [Fact]
        public async Task Test1()
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(
                @"
                {
                    
                }
                "));
                
            memoryStream.Seek(0, SeekOrigin.Begin);

            SassConfig config = new SassConfig();
            await config.LoadAsync(memoryStream);

            SassCommandArgumentBuilder commandArgumentBuilder = new SassCommandArgumentBuilder(config, Environment.CurrentDirectory);
            var commandArguments = commandArgumentBuilder.BuildArgs();
        }

        [Fact]
        public async Task Test2()
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(
                @"
                {
                    ""compilerOptions"": {
                        ""style"": ""expanded"",
                        ""sourceMap"": true,
                        ""outDir"": ""wwwroot/css""
                    },
                    ""files"": [
                        ""test.scss""
                    ]
                }
                "));
                
            memoryStream.Seek(0, SeekOrigin.Begin);

            SassConfig config = new SassConfig();
            await config.LoadAsync(memoryStream);

            SassCommandArgumentBuilder commandArgumentBuilder = new SassCommandArgumentBuilder(config, Environment.CurrentDirectory);
            var commandArguments = commandArgumentBuilder.BuildArgs();
        }
    }
}
