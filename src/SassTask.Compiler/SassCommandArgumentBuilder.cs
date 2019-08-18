using System;
using System.IO;
using System.Text;

namespace Sass
{
    public sealed class SassCommandArgumentBuilder
    {
        private readonly string[] SASS_FILES_EXTENSIONS = new string[] {
                ".sass", ".scss"
            };

        private readonly SassConfig config;
        private readonly string directory;

        public SassCommandArgumentBuilder(SassConfig config, string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException("message", nameof(directory));
            }

            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.directory = directory;
        }

        public string BuildArgs()
        {
            StringBuilder argsStringBuilder = new StringBuilder();

            if (config.CompilerOptions?.Style != null)
            {
                argsStringBuilder.Append($" --style={config.CompilerOptions.Style.ToString().ToLower()}");
            }

            if (!config.CompilerOptions?.SourceMap ?? false)
            {
                argsStringBuilder.Append(" --no-source-map");
            }

            ParseFiles(argsStringBuilder);

            return argsStringBuilder.ToString();
        }


        private void ParseFiles(StringBuilder argsStringBuilder)
        {
            if (config.Files != null)
            {
                foreach (var fileName in config.Files)
                {
                    if (fileName.Contains("*"))
                    {
                        ProcessWildcardFilePath(argsStringBuilder, fileName);
                    }
                    else
                    {
                        ProcessSingleFilePath(argsStringBuilder, fileName);
                    }
                }
            }
            else
            {
                // Add all default files
                foreach (var extension in SASS_FILES_EXTENSIONS)
                {
                    ProcessWildcardFilePath(argsStringBuilder, $"*{extension}");
                }
            }
        }

        private void ProcessSingleFilePath(StringBuilder argsStringBuilder, string fileName)
        {
            string sourceDirPath = directory;
            if (config.CompilerOptions?.SourceDir != null)
            {
                sourceDirPath = ToRootedPath(config.CompilerOptions.SourceDir);
            }
            var filePath = Path.Combine(sourceDirPath, fileName);
            ProcessFilePath(argsStringBuilder, filePath);
        }

        private void ProcessWildcardFilePath(StringBuilder argsStringBuilder, string filePath)
        {
            string sourceDirPath = directory;
            if (config.CompilerOptions?.SourceDir != null)
            {
                sourceDirPath = ToRootedPath(config.CompilerOptions.SourceDir);
            }
            foreach (var fileName in Directory.GetFiles(sourceDirPath, filePath, SearchOption.AllDirectories))
            {
                ProcessFilePath(argsStringBuilder, fileName);
            }
        }

        private string ToRootedPath(string path) => PathHelpers.ToRootedPath(path, directory);

        private void ProcessFilePath(StringBuilder argsStringBuilder, string inputFileName)
        {
            var outDirPath = directory;
            if (config.CompilerOptions?.OutDir != null)
            {
                outDirPath = ToRootedPath(config.CompilerOptions.OutDir);
            }
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFileName);
            var outFilePath = Path.Combine(outDirPath, $"{fileNameWithoutExtension}.css");
            argsStringBuilder.Append($" \"{inputFileName}\":\"{outFilePath}\"");
        }
    }
}
