using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sass
{
    public sealed class SassConfig
    {
        public IEnumerable<string> Files { get; set; }

        public SassCompilerOptions CompilerOptions { get; set; } = new SassCompilerOptions();
    }
}
