using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Kata.LegacySecurityManager.UnitTests
{
    public class SecurityManagerCharacterizationTests
    {
        [Theory]
        [InlineData("ploeh", "Mark Seemann", "fnaahndøh")]
        [InlineData("foo", "John Doe", "barbazqux")]
        [InlineData("jane", "Jane Doe", "gnifgnaf")]
        [RestoreConsoleAfterRun]
        public void HappyPathSucceeds(
            string userName,
            string fullName,
            string password)
        {
            // Fixture setup
            var input = string.Join(
                Environment.NewLine,
                userName, fullName, password, password);
            using (var @out = new StringWriter())
            using (var @in = new StringReader(input))
            {
                Console.SetOut(@out);
                Console.SetIn(@in);
                // Exercise system
                new SecurityController(
                    new ConsoleRenderer(),
                    new ReverseEncryption())
                    .CreateUser(
                        new ConsoleUserProfileInputCollector()
                            .CollectUserProfile());
                // Verify outcome
                var expected = string.Join(
                    Environment.NewLine,
                    "Enter a username",
                    "Enter your full name",
                    "Enter your password",
                    "Re-enter your password",
                    string.Format(
                        "Saving Details for User ({0}, {1}, {2})\n",
                        userName,
                        fullName,
                        new string(password.Reverse().ToArray())));
                Assert.Equal(expected, @out.ToString());
                // Teardown
            }
        }

        [Theory]
        [InlineData("ploeh", "Mark Seemann", "fnaahndøh", "fnaahndøl")]
        [InlineData("foo", "John Doe", "barbazqux", "baroazqux")]
        [InlineData("jane", "Jane Doe", "gnifgnaf", "giraffen")]
        [RestoreConsoleAfterRun]
        public void PasswordsDoNotMatch(
            string userName,
            string fullName,
            string password,
            string confirmPassword)
        {
            // Fixture setup
            var input = string.Join(
                Environment.NewLine,
                userName, fullName, password, confirmPassword);
            using (var @out = new StringWriter())
            using (var @in = new StringReader(input))
            {
                Console.SetOut(@out);
                Console.SetIn(@in);
                // Exercise system
                new SecurityController(
                    new ConsoleRenderer(),
                    new ReverseEncryption())
                    .CreateUser(
                        new ConsoleUserProfileInputCollector()
                            .CollectUserProfile()); ;
                // Verify outcome
                var expected = string.Join(
                    Environment.NewLine,
                    "Enter a username",
                    "Enter your full name",
                    "Enter your password",
                    "Re-enter your password",
                    "The passwords don't match",
                    "");
                Assert.Equal(expected, @out.ToString());
                // Teardown
            }
        }

        [Theory]
        [InlineData("ploeh", "Mark Seemann", "fnaah")]
        [InlineData("foo", "John Doe", "barbaz")]
        [InlineData("jane", "Jane Doe", "gnif")]
        [RestoreConsoleAfterRun]
        public void PasswordIsTooShort(
            string userName,
            string fullName,
            string password)
        {
            // Fixture setup
            var input = string.Join(
                Environment.NewLine,
                userName, fullName, password, password);
            using (var @out = new StringWriter())
            using (var @in = new StringReader(input))
            {
                Console.SetOut(@out);
                Console.SetIn(@in);
                // Exercise system
                new SecurityController(
                    new ConsoleRenderer(),
                    new ReverseEncryption())
                    .CreateUser(
                        new ConsoleUserProfileInputCollector()
                            .CollectUserProfile()); ;
                // Verify outcome
                var expected = string.Join(
                    Environment.NewLine,
                    "Enter a username",
                    "Enter your full name",
                    "Enter your password",
                    "Re-enter your password",
                    "Password must be at least 8 characters in length",
                    "");
                Assert.Equal(expected, @out.ToString());
                // Teardown
            }
        }

        [Fact]
        public void InvalidInputCorrectlyRendersValidationMessage()
        {
            // Fixture setup
            var rendererMock = new Mock<IRenderer>();            
            var sut = new SecurityController(
                rendererMock.Object,
                new Mock<IEncryption>().Object);

            var invalidInput = new UserProfileInput(
                userName: "Foo",
                fullName: "Foo Bar",
                password: "Not too short",
                passwordRepeated: "doesn't match");
            // Exercise system
            sut.CreateUser(invalidInput);
            // Verify outcome
            var expected = invalidInput.Validate();
            rendererMock.Verify(r => r.Render(expected));
            // Teardown
        }

        [Fact]
        public void ValidInputCorrectlyRendersMessage()
        {
            // Fixture setup
            var rendererMock = new Mock<IRenderer>();
            var encryptionStub = new Mock<IEncryption>();
            var sut = new SecurityController(
                rendererMock.Object,
                encryptionStub.Object);

            var input = new UserProfileInput(
                userName: "Foo",
                fullName: "Foo Bar",
                password: "Not too short",
                passwordRepeated: "Not too short");

            encryptionStub
                .Setup(e => e.Encrypt(input.Password))
                .Returns("encrypted password");
            // Exercise system
            sut.CreateUser(input);
            // Verify outcome
            var expected =
                string.Format("Saving Details for User ({0}, {1}, {2})\n",
                    input.UserName,
                    input.FullName,
                    "encrypted password");
            rendererMock.Verify(r => r.Render(expected));
            // Teardown
        }
    }
}
