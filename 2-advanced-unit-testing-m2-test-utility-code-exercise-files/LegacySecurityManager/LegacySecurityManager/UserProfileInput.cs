using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Kata.LegacySecurityManager
{
    public class UserProfileInput
    {
        private readonly string userName;
        private readonly string fullName;
        private readonly string password;
        private readonly string passwordRepeated;

        public UserProfileInput(
            string userName,
            string fullName,
            string password,
            string passwordRepeated)
        {
            this.userName = userName;
            this.fullName = fullName;
            this.password = password;
            this.passwordRepeated = passwordRepeated;
        }

        public string UserName
        {
            get { return this.userName; }
        }

        public string FullName
        {
            get { return this.fullName; }
        }

        public string Password
        {
            get { return this.password; }
        }

        public string PasswordRepeated
        {
            get { return this.passwordRepeated; }
        }

        public string Validate()
        {
            if (this.password != this.passwordRepeated)
                return "The passwords don't match" + Environment.NewLine;
            if (this.password.Length < 8)
                return "Password must be at least 8 characters in length" +
                    Environment.NewLine;
            return "";
        }
    }
}
