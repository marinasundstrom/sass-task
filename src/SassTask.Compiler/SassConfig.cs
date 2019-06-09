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
        private SassCompilerOptions compilerOptions;

        public IEnumerable<string> Files { get; private set; } = null;

        public SassCompilerOptions CompilerOptions => compilerOptions ?? (compilerOptions = new SassCompilerOptions());

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
                    CompilerOptions.Style = (CssStyle)Enum.Parse(typeof(CssStyle), styleElement.GetString(), true);
                }
                if (compilerOptionsElement
                    .TryGetProperty("sourceMap", out var sourceMapElement))
                {
                    CompilerOptions.SourceMap = sourceMapElement.GetBoolean();
                }

                if (compilerOptionsElement
                    .TryGetProperty("sourceDir", out var sourceDirElement))
                {
                    CompilerOptions.SourceDir = sourceDirElement.GetString();
                }

                if (compilerOptionsElement
                    .TryGetProperty("outDir", out var outDirElement))
                {
                    CompilerOptions.OutDir = outDirElement.GetString();
                }

                if (compilerOptionsElement
                    .TryGetProperty("outFile", out var outFile))
                {
                    CompilerOptions.OutFile = outFile.GetString();
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
    }
}
