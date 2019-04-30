using System;

namespace Prestige.Kernel.Authentication.Models
{
    public class JsonWebTokenPayload
    {
        public string Subject { get; set; }

        public string Role { get; set; }

        public DateTime Expires { get; set; }
    }
}
