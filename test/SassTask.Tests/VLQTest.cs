using Xunit;
using Sass;

namespace SassTask.Tests
{
    public class VLQTest
    {
        [Fact]
        public void Test1()
        {
            var foo = VLQ.Encode(200);
        }
    }
}
