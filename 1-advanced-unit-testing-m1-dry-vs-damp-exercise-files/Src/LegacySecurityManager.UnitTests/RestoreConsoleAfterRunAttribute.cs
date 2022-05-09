using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploeh.Samples.Kata.LegacySecurityManager.UnitTests
{
    public class RestoreConsoleAfterRunAttribute : BeforeAfterTestAttribute
    {
        private TextReader standardIn;
        private TextWriter standardOut;
        private TextWriter standardError;

        public override void Before(MethodInfo methodUnderTest)
        {
            base.Before(methodUnderTest);

            this.standardIn = Console.In;
            this.standardOut = Console.Out;
            this.standardError = Console.Error;
        }

        public override void After(MethodInfo methodUnderTest)
        {
            base.After(methodUnderTest);

            Console.SetIn(this.standardIn);
            Console.SetOut(this.standardOut);
            Console.SetError(this.standardError);
        }
    }
}
