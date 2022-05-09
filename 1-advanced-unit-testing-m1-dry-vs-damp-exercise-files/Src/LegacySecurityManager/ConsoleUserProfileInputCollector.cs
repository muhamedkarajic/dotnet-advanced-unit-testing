using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Kata.LegacySecurityManager
{
    public class ConsoleUserProfileInputCollector : IUserProfileInputCollector
    {
        public UserProfileInput CollectUserProfile()
        {
            Console.WriteLine("Enter a username");
            var userName = Console.ReadLine();
            Console.WriteLine("Enter your full name");
            var fullName = Console.ReadLine();
            Console.WriteLine("Enter your password");
            var password = Console.ReadLine();
            Console.WriteLine("Re-enter your password");
            var passwordRepeated = Console.ReadLine();
            return new UserProfileInput(
                userName,
                fullName,
                password,
                passwordRepeated);
        }
    }
}
