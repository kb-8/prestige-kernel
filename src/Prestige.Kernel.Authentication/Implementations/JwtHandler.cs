using Microsoft.IdentityModel.Tokens;

using Prestige.Kernel.Authentication.Interfaces;
using Prestige.Kernel.Common.Models.Authentication;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Prestige.Kernel.Authentication.Implementations
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly JwtOptions options;
        private readonly SecurityKey issuerSigningKey;
        private readonly SigningCredentials signingCredentials;
        private readonly JwtHeader jwtHeader;
        private readonly TokenValidationParameters tokenValidationParameters;

        public JwtHandler(JwtOptions options)
        {
            this.options = options;
            this.issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.options.SecretKey));
            this.signingCredentials = new SigningCredentials(this.issuerSigningKey, SecurityAlgorithms.HmacSha256);
            this.jwtHeader = new JwtHeader(this.signingCredentials);
            this.tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = issuerSigningKey,
                ValidIssuer = this.options.Issuer,
                ValidAudience = this.options.ValidAudience,
                ValidateAudience = this.options.ValidateAudience,
                ValidateLifetime = this.options.ValidateLifetime
            };
        }

        public JsonWebToken CreateToken(Guid userId, string role)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            DateTime expires = DateTime.UtcNow.AddMinutes(this.options.ExpiryMinutes);

            var jwt = new JwtSecurityToken(
                issuer: this.options.Issuer,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: this.signingCredentials
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JsonWebToken
            {
                AccessToken = token,
                Expires = expires
            };
        }

        public JsonWebTokenPayload GetTokenPayload(string accessToken)
        {
            this.jwtSecurityTokenHandler.ValidateToken(accessToken, this.tokenValidationParameters,
                out SecurityToken validatedSecurityToken);

            var jwt = validatedSecurityToken as JwtSecurityToken;

            return new JsonWebTokenPayload
            {
                Subject = jwt.Subject,
                Claims = jwt.Claims,
                Expires = jwt.ValidTo
            };
        }
    }
}
