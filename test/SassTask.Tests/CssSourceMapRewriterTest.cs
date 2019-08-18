using Xunit;
using Sass;
using System.Threading.Tasks;

namespace SassTask.Tests
{
    public class CssSourceMapRewriterTest
    {
        [Fact]
        public void Test1()
        {
            CssSourceMapRewriter mapRewriter = new CssSourceMapRewriter();
            var r = mapRewriter.Rewrite(new [] { 
                "{\"version\":3,\"sourceRoot\":\"\",\"sources\":[\"../../Content/scss/test3.scss\"],\"names\":[],\"mappings\":\"AAAA;EACI\",\"file\":\"test3.css\"}"
            }, 100);
        }
    }
}
