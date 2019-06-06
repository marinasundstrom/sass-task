using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sass
{
    public class SassConfig
    {
        public SassCompilerOptions CompilerOptions { get; private set; }
        public IEnumerable<string> Files { get; private set; } = null;

        public async Task LoadAsync(Stream stream)
        {
            var sassConfigJsonDoc = await JsonDocument.ParseAsync(stream);

            var sassArgsStringBuilder = new StringBuilder();

            var rootElement = sassConfigJsonDoc.RootElement;

            if (rootElement.TryGetProperty("compilerOptions", out var compilerOptionsElement))
            {
                if (compilerOptionsElement
                    .TryGetProperty("style", out var styleElement))
                {
                    GetCompilerOptions().Style = (CssStyle)Enum.Parse(typeof(CssStyle), styleElement.GetString(), true);
                }
                if (compilerOptionsElement
                    .TryGetProperty("sourceMap", out var sourceMapElement))
                {
                    GetCompilerOptions().SourceMap = sourceMapElement.GetBoolean();
                }

                if (compilerOptionsElement
                    .TryGetProperty("sourceDir", out var sourceDirElement))
                {
                    GetCompilerOptions().SourceDir = sourceDirElement.GetString();
                }

                if (compilerOptionsElement
                    .TryGetProperty("outDir", out var outDirElement))
                {
                    GetCompilerOptions().OutDir = outDirElement.GetString();
                }
            }

            if (rootElement
                        .TryGetProperty("files", out var filesElement))
            {
                Files = filesElement
                    .EnumerateArray()
                    .Select(item => item.GetString());
            }
        }

        private SassCompilerOptions GetCompilerOptions()
        {
            return CompilerOptions ?? (CompilerOptions = new SassCompilerOptions());
        }
    }
}
