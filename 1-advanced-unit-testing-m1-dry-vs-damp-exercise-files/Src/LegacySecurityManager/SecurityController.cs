using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.Kata.LegacySecurityManager
{
    public class SecurityController
    {
        private readonly IRenderer renderer;
        private readonly IEncryption encryption;

        public SecurityController(IRenderer renderer, IEncryption encryption)
        {
            this.renderer = renderer;
            this.encryption = encryption;
        }

        public void CreateUser(UserProfileInput input)
        {
            var validationMessage = input.Validate();
            if (validationMessage != "")
            {
                this.renderer.Render(validationMessage);
                return;
            }

            this.renderer.Render(
                String.Format("Saving Details for User ({0}, {1}, {2})\n",
                input.UserName,
                input.FullName,
                encryption.Encrypt(input.Password)));
        }
    }
}
