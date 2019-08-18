namespace Sass
{
    public partial class SourceMap
    {
        public long Version { get; set; }
        public string SourceRoot { get; set; }
        public string[] Sources { get; set; }
        public object[] Names { get; set; }
        public string Mappings { get; set; }
        public string File { get; set; }
    }
}
