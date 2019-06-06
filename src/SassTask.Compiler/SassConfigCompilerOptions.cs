namespace Sass
{
    public class SassCompilerOptions
    {
        public CssStyle? Style { get; set; } = null;
        public bool SourceMap { get; set; } = true;
        public string SourceDir { get; set; } = null;
        public string OutDir { get; set; } = null;
    }
}
