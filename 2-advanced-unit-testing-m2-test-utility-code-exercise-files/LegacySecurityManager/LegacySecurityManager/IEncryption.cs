using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Kata.LegacySecurityManager
{
    public interface IEncryption
    {
        string Encrypt(string input);
    }
}
