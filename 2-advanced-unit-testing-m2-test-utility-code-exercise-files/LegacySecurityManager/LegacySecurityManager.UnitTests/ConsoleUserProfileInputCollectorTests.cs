using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;
using Ploeh.Samples.Kata.LegacySecurityManager;

namespace Ploeh.Samples.Kata.LegacySecurityManager.UnitTests
{
    public class ConsoleUserProfileInputCollectorTests
    {
        [Theory]
        [InlineData("ploeh", "Mark Seemann", "idutei", "ieir8")]
        [InlineData("fnaah", "John Doe", "dis", "2383jciFARE")]
        [InlineData("ndøh", "Jane Doe", "ir3992jcncueuAFA", "t")]
        [RestoreConsoleAfterRun]
        public void CollectUserProfileReturnsCorrectUserData(
            string userName,
            string fullName,
            string password,
            string passwordRepeated)
        {
            // Fixture setup
            var input = string.Join(
                Environment.NewLine,
                userName, fullName, password, passwordRepeated);
            using (var @out = new StringWriter())
            using (var @in = new StringReader(input))
            {
                Console.SetOut(@out);
                Console.SetIn(@in);

                var sut = new ConsoleUserProfileInputCollector();
                // Exercise system
                UserProfileInput actual = sut.CollectUserProfile();
                // Verify outcome
                Assert.Equal(userName, actual.UserName);
                Assert.Equal(fullName, actual.FullName);
                Assert.Equal(password, actual.Password);
                Assert.Equal(passwordRepeated, actual.PasswordRepeated);
                // Teardown
            }
        }

        [Fact]
        [RestoreConsoleAfterRun]
        public void CollectUserProfilePromptsCorrectly()
        {
            // Fixture setup
            var input = string.Join(
                Environment.NewLine,
                "dummy", "dummy", "dummy", "dummy");
            using (var @out = new StringWriter())
            using (var @in = new StringReader(input))
            {
                Console.SetOut(@out);
                Console.SetIn(@in);

                var sut = new ConsoleUserProfileInputCollector();
                // Exercise system
                sut.CollectUserProfile();
                // Verify outcome
                var expected = string.Join(
                    Environment.NewLine,
                    "Enter a username",
                    "Enter your full name",
                    "Enter your password",
                    "Re-enter your password",
                    "");
                Assert.Equal(expected, @out.ToString());
                // Teardown
            }
        }

        [Fact]
        public void SutIsUserProfileInputCollector()
        {
            var sut = new ConsoleUserProfileInputCollector();
            Assert.IsAssignableFrom<IUserProfileInputCollector>(sut);
        }
    }
}
