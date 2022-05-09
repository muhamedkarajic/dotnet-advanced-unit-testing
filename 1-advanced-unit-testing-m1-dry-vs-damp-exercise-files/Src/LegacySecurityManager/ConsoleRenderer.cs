using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Kata.LegacySecurityManager
{
    public class ConsoleRenderer : IRenderer
    {
        public void Render(string text)
        {
            Console.Write(text);
        }
    }
}
