using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Extensions;
using Ploeh.Samples.Kata.LegacySecurityManager;
using Xunit;

namespace Ploeh.Samples.Kata.LegacySecurityManager.UnitTests
{
    public class ConsoleRendererTests
    {
        [Theory]
        [InlineData("foo")]
        [InlineData("bar")]
        [InlineData("baz")]
        [RestoreConsoleAfterRun]
        public void RenderCorrectlyWritesToConsole(string expected)
        {
            // Fixture setup
            using (var @out = new StringWriter())
            {
                Console.SetOut(@out);

                var sut = new ConsoleRenderer();
                // Exercise system
                sut.Render(expected);
                // Verify outcome
                Assert.Equal(expected, @out.ToString());
                // Teardown
            }
        }

        [Fact]
        public void SutIsRenderer()
        {
            var sut = new ConsoleRenderer();
            Assert.IsAssignableFrom<IRenderer>(sut);
        }
    }
}
