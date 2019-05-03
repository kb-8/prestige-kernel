using Prestige.Kernel.Common.Models.Authentication;

using System;

namespace Prestige.Kernel.Authentication.Interfaces
{
    public interface IJwtHandler
    {
        JsonWebToken CreateToken(Guid userId, string role);

        JsonWebTokenPayload GetTokenPayload(string accessToken);
    }
}
