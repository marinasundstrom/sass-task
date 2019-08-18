using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sass
{
    public sealed class CssSourceMapRewriter
    {   List<string> newSources = new List<string>();
        List<string> newMappings = new List<string>();

        private int LookupFile(string path) 
        {
            var index = newSources.IndexOf(path);
            if(index == -1) 
            {
                newSources.Add(path);
                index = newSources.Count;
            }
            return index;
        }

        public string Rewrite(string[] sourceMaps, int offset)
        {
            newSources.Clear();
            newMappings.Clear();

            var count = 0;

            foreach (var str in sourceMaps)
            {
                var sourceMap = JsonSerializer.Deserialize<SourceMap>(str, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
                    Converters =
                        {
                            new JsonStringEnumConverter()
                        }
                });

                NewMethod(count, sourceMap);

                count += str
                    .Split(new string[]{ Environment.NewLine }, StringSplitOptions.None)
                    .Count();
            }

            var newSourceMap = new SourceMap() {
                Mappings = string.Join(";", string.Join(";", newMappings)),
                Sources = newSources.ToArray()
            };

            return JsonSerializer.Serialize(newSourceMap);
        }

        private void NewMethod(int offset, SourceMap sourceMap)
        {
            var mappings = sourceMap.Mappings.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var mapping in mappings) // lines
            {
                var newEntries = new List<string>();

                var entries = mapping.Split(',');
                foreach (var entry in entries)
                {
                    int column = VLQ.Decode(entry[0].ToString()).First();
                    int sourceFile = VLQ.Decode(entry[1].ToString()).First();
                    int sourceRow = VLQ.Decode(entry[2].ToString()).First();
                    int sourceColumn = VLQ.Decode(entry[3].ToString()).First();

                    var newSourceFile = LookupFile(sourceMap.Sources[0]);
                    var newRow = sourceRow + offset;

                    newEntries.Add($"{VLQ.Encode(column)}{VLQ.Encode(newSourceFile)}{VLQ.Encode(newRow)}{VLQ.Encode(sourceColumn)}");
                }

                newMappings.Add(string.Join(",", newEntries));
            }
        }
    }
}
