namespace Sass
{
    public sealed class SassCompilerOptions
    {
        public CssStyle? Style { get; set; } = null;
        public bool Update { get; set; } = false;
        public bool SourceMap { get; set; } = false;
        public SourceMapUrls? SourceMapUrls { get; set; } = null;
        public bool EmbedSources { get; set; } = false;
        public bool EmbedSourceMap { get; set; } = false;
        public string SourceDir { get; set; } = null;
        public string OutDir { get; set; } = null;
    }
}
