using Microsoft.IdentityModel.Tokens;
using NewsAppNet.Models.DataModels;
using NewsAppNet.Models.ViewModels;
using NewsAppNet.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsAppNet.Services
{
    public class AuthService : IAuthService
    {
        readonly string jwtSecret;
        readonly int jwtLifespan;

        public AuthService(string jwtSecret, int jwtLifespan)
        {
            this.jwtSecret = jwtSecret;
            this.jwtLifespan = jwtLifespan;
        }

        public UserAuthData GetUserAuthData(User user)
        {
            var expirationTime = DateTime.UtcNow.AddSeconds(jwtLifespan);

            UserView userView = new(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.UserType)
                }),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return new UserAuthData
            {
                Token = token,
                TokenExpirationTime = ((DateTimeOffset)expirationTime).ToString(),
                User = userView
            };
        }
    }
}
