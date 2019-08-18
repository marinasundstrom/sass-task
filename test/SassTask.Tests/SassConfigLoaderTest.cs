using System;
using System.Linq;
using System.IO;
using System.Text;
using Xunit;
using Sass;
using System.Threading.Tasks;

namespace SassTask.Tests
{
    public class SassConfigLoaderTest
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

            var loader = new SassConfigLoader();
            var config = await loader.LoadAsync(memoryStream);
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

            var loader = new SassConfigLoader();
            var config = await loader.LoadAsync(memoryStream);

            Assert.NotNull(config.CompilerOptions);
            Assert.Equal(CssStyle.Compressed, config.CompilerOptions.Style);
        }

        [Fact]
        public async Task Test3()
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(
                @"
                {
                    ""files"": [
                        ""file1"",
                        ""file2"",
                        ""file3""
                    ]
                }
                "));
                
            memoryStream.Seek(0, SeekOrigin.Begin);

            var loader = new SassConfigLoader();
            var config = await loader.LoadAsync(memoryStream);

            Assert.True(config.Files.SequenceEqual(new [] { "file1", "file2", "file3" }));
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

            var loader = new SassConfigLoader();
            var config = await loader.LoadAsync(memoryStream);
            
            Assert.NotNull(config.CompilerOptions);
            Assert.Equal(CssStyle.Expanded, config.CompilerOptions.Style);
            Assert.True(config.CompilerOptions.SourceMap);
            Assert.Equal("Content/scss", config.CompilerOptions.SourceDir);
            Assert.Equal("wwwroot/css", config.CompilerOptions.OutDir);
            Assert.Equal(new [] { "test.scss" }, config.Files);
        }
    }
}
