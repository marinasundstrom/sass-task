using System;
using System.IO;
using System.Text;
using Xunit;
using Sass;
using System.Threading.Tasks;

namespace SassTask.Tests
{
    public class UnitTest1
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
        }

        [Fact]
        public async Task Test2()
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(
                @"
                {
                    ""compilerOptions"": {
                        ""style"": ""compressed"" 
                    }
                }
                "));
                
            memoryStream.Seek(0, SeekOrigin.Begin);

            SassConfig config = new SassConfig();
            await config.LoadAsync(memoryStream);
        }

        [Fact]
        public async Task Test3()
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(
                @"
                {
                    ""files"": [
                        ""file1"",
                        ""file1"",
                        ""file3""
                    ]
                }
                "));
                
            memoryStream.Seek(0, SeekOrigin.Begin);

            SassConfig config = new SassConfig();
            await config.LoadAsync(memoryStream);
        }

         [Fact]
        public async Task Test4()
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(
                @"
                {
                    ""compilerOptions"": {
                        ""style"": ""expanded"",
                        ""sourceMap"": true,
                        ""sourceDir"": ""Content/scss"",
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
        }
    }
}
