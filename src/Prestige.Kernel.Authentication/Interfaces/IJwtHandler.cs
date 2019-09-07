using Prestige.Kernel.Common.Models.Authentication;

namespace Prestige.Kernel.Authentication.Interfaces
{
    public interface IJwtHandler
    {
        JsonWebToken CreateToken(long userId, string email);

        JsonWebTokenPayload GetTokenPayload(string accessToken);
    }
}
