using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sass
{
    public sealed class SassConfigLoader
    {
        public async Task<SassConfig> LoadAsync(Stream stream)
        {
            var sassConfigJsonDoc = await JsonSerializer.DeserializeAsync<SassConfig>(stream, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            });
            return sassConfigJsonDoc;
        }
    }
}
