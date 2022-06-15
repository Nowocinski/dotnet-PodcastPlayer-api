using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication.Infrastructure.Settings;

namespace WebApplication.Infrastructure.Services.User.JwtToken
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSettings _jwtSettings;
        public JwtHandler(IConfiguration configuration/*IOptions<JwtSettings> jwtSettings*/)
        {
            //_jwtSettings = jwtSettings.Value;
            // TODO: Pobieranie danych powinno odbywać się przez IOptions<JwtSettings>, ale nie potrafiłem tego skonfigurować.
            this._jwtSettings = new JwtSettings
            {
                ExpiryInMinutes = int.Parse(configuration.GetSection("Jwt:ExpiryInMinutes").Value),
                SigningKey = configuration.GetSection("Jwt:SigningKey").Value,
                Site = configuration.GetSection("Jwt:Site").Value
            };

        }
        public string CreateToken(Guid UserId, string role)
        {
            DateTime now = DateTime.UtcNow;
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, UserId.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                // TODO: Kod pokazuję błąd, więc zakomentowałem. Do sprawdzenia na później czy to coś ważnego.
                //new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString())
            };

            DateTime expires = now.AddMinutes(_jwtSettings.ExpiryInMinutes);
            SigningCredentials signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey)),
                SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Site,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: signingCredentials
                );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }
    }
}
