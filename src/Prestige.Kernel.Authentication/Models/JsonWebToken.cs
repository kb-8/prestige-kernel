using System;

namespace Prestige.Kernel.Authentication.Models
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }

        public DateTime? Expires { get; set; }
    }
}
