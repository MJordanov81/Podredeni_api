namespace Api.Web.Services.Implementations
{
    using Microsoft.IdentityModel.Tokens;
    using Models.Config;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public class JwtTokenService : ITokenService
    {
        private readonly JwtConfiguration jwtConf;

        public JwtTokenService(JwtConfiguration jwtConf)
        {
            this.jwtConf = jwtConf;
        }

        public async Task<string> ProvideToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtConf.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtConf.Issuer,
                audience: jwtConf.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Infrastructure.Constants.JwtConstants.ExpiresAfterMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
