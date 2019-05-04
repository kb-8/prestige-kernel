using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Prestige.Kernel.Common.Models.Authentication
{
    public class JsonWebTokenPayload
    {
        public string Subject { get; set; }

        public IEnumerable<Claim> Claims { get; set; }

        public DateTime Expires { get; set; }
    }
}
