using Microsoft.IdentityModel.Tokens;

using System.Text;

namespace Prestige.Kernel.Common.Models.Authentication
{
    public static class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
            => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
    }
}
