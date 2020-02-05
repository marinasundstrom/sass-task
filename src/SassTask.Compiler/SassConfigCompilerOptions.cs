namespace Sass
{
    public sealed class SassCompilerOptions
    {
        public CssStyle? Style { get; set; } = null;
        public bool SourceMap { get; set; } = false;
        public string SourceDir { get; set; } = null;
        public string OutDir { get; set; } = null;
    }
}
