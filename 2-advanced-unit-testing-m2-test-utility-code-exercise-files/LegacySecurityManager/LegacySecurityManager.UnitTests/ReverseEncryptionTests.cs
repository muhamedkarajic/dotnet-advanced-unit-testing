using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Extensions;
using Ploeh.Samples.Kata.LegacySecurityManager;
using Xunit;

namespace Ploeh.Samples.Kata.LegacySecurityManager.UnitTests
{
    public class ReverseEncryptionTests
    {
        [Theory]
        [InlineData("Ploeh")]
        [InlineData("Fnaah")]
        [InlineData("Ndøh")]
        public void EncryptReturnsCorrectResult(
            string input)
        {
            // Fixture setup
            var sut = new ReverseEncryption();
            // Exercise system
            string actual = sut.Encrypt(input);
            // Verify outcome
            var expected = new string(input.Reverse().ToArray());
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void SutIsEncryption()
        {
            var sut = new ReverseEncryption();
            Assert.IsAssignableFrom<IEncryption>(sut);
        }
    }
}
