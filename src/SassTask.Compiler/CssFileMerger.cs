using System.IO;
using System.Threading.Tasks;

namespace Sass
{
    public sealed class CssFileMerger
    {
        public async Task MergeFilesAsync(string[] filePaths, string outFile, bool compress = false, bool deleteOriginals = true, bool stripSourceMapping = true)
        {
            File.Delete(outFile);

            using (var outputStream = File.OpenWrite(outFile))
            using (var streamWriter = new StreamWriter(outputStream))
            {
                foreach (var filePath in filePaths)
                {
                    using (var inputStream = File.OpenRead(filePath))
                    using (var streamReader = new StreamReader(inputStream))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            var line = await streamReader.ReadLineAsync();
                            if (stripSourceMapping && line.StartsWith("/*# sourceMappingURL="))
                            {
                                continue;
                            }
                            if(compress) 
                            {
                                await streamWriter.WriteAsync($"{line} ");
                            } 
                            else 
                            {
                                await streamWriter.WriteLineAsync(line);
                            }
                        }
                    }
                }
            }

            if (deleteOriginals)
            {
                foreach (var filePath in filePaths)
                {
                    File.Delete(filePath);
                    if (stripSourceMapping)
                    {
                        File.Delete($"{filePath}.map");
                    }
                }
            }
        }
    }
}
