using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieTheaterAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IConfiguration config, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> CreateTokenAsync(User user)
        {
            //var audience = _httpContextAccessor.HttpContext.Items["Audience"].ToString();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                new Claim("UserId", user.Id.ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var audiences = _config.GetSection("JWT:Audie   nce").Get<string[]>();
            foreach (var audience in audiences)
            {
                claims.Add(new Claim("aud", audience));
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                //Audience = _config["JWT:Audience"]
                //Audience = string.Join(",", _config.GetSection("JWT:Audience").Get<string[]>())
                //Audience = _config["JWT:Audience:0"] + "," + _config["JWT:Audience:1"]
                //Audience = audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
