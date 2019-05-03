using System;

namespace Prestige.Kernel.Common.Models.Authentication
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }

        public DateTime? Expires { get; set; }
    }
}
