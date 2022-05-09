using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Kata.LegacySecurityManager
{
    public class ReverseEncryption : IEncryption
    {
        public string Encrypt(string input)
        {
            return new string(input.Reverse().ToArray());
        }
    }
}
