using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Kata.LegacySecurityManager.UnitTests
{
    public class UserProfileInputTests
    {
        [Theory]
        [InlineData("faduirew", "93j33FDS93")]
        [InlineData("393kcEFE", "fkecdi93")]
        [InlineData("#C#¤", "#C=#C(")]
        public void ValidateWhenPasswordsDoNotMatchReturnsCorrectResult(
            string password,
            string passwordRepeated)
        {
            // Fixture setup
            var sut = new UserProfileInput(
                "dummy", "dummy", password, passwordRepeated);
            // Exercise system
            string actual = sut.Validate();
            // Verify outcome
            Assert.Equal(
                "The passwords don't match" + Environment.NewLine,
                actual);
            // Teardown
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("kdaiteatalal")]
        [InlineData("jfa83c#T#T9c fa")]
        public void ValidateWhenPasswordsMatchAndAreLongReturnsCorrectResult(
            string password)
        {
            // Fixture setup
            var sut = new UserProfileInput(
                "dummy", "dummy", password, password);
            // Exercise system
            var actual = sut.Validate();
            // Verify outcome
            Assert.Equal("", actual);
            // Teardown
        }

        [Theory]
        [InlineData("1234567")]
        [InlineData("f")]
        [InlineData("3#¤CFfd")]
        public void ValidateWhenPasswordsMatchButAreTooShortReturnsCorrectResult(
            string password)
        {
            // Fixture setup
            var sut = new UserProfileInput(
                "dummy", "dummy", password, password);
            // Exercise system
            var actual = sut.Validate();
            // Verify outcome
            Assert.Equal(
                "Password must be at least 8 characters in length" +
                    Environment.NewLine,
                actual);
            // Teardown
        }
    }
}
