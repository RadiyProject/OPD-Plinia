using plinia.Models;
using System.Security.Claims;
using System.Text;

namespace plinia.Services
{
    public class TokenService
    {
        public string? accessToken { get; private set; }
        public string? refreshToken { get; private set; }
        public object SecurityAlgorithms { get; private set; }

        private IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> GenerateToken(User? user)
        {
            if (user == null || user.email == null || user.premium == null)
                return "User value is empty.";

            var claims = new List<Claim> { new Claim(ClaimTypes.Email, user.email), new Claim(ClaimTypes.Role, user.premium) };

            var lifeTime = 0;

            int.TryParse(configuration.GetSection("AccessTokens")["LifeTime"], out lifeTime);

            //Создаем JWT-токен
            var jwt = CreateToken(configuration.GetSection("AccessTokens")["Issuer"], 
                configuration.GetSection("AccessTokens")["Audience"],
                claims,TimeSpan.FromMinutes(lifeTime), configuration.GetSection("AccessTokens")["Key"]);

            accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            lifeTime = 0;

            int.TryParse(configuration.GetSection("RefreshToken")["LifeTime"], out lifeTime);

            //Создаем JWT-токен
            jwt = CreateToken(configuration.GetSection("RefreshToken")["Issuer"],
                configuration.GetSection("RefreshToken")["Audience"],
                claims, TimeSpan.FromDays(lifeTime), configuration.GetSection("RefreshToken")["Key"]);

            accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            throw new NotImplementedException();
        }

        JwtSecurityToken CreateToken(string issuer, string audience, IEnumerable<Claim> claims, TimeSpan lifeTime, string key)
        {
            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(lifeTime),
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

            return jwt;
            //throw new NotImplementedException();
        }

        static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }
    }

    internal class SigningCredentials
    {
        private object v;
        private object hmacSha256;

        public SigningCredentials(object v, object hmacSha256)
        {
            this.v = v;
            this.hmacSha256 = hmacSha256;
        }
    }

    internal class JwtSecurityToken
    {
    }

    internal class JwtSecurityTokenHandler
    {
        public JwtSecurityTokenHandler()
        {
        }

        internal string? WriteToken(object jwt)
        {
            throw new NotImplementedException();
        }
    }
}