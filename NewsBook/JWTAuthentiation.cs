using Microsoft.IdentityModel.Tokens;
using NewsBook.Controllers;
using NewsBook.ModelDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsBook
{
    public interface IJwtUtils
    {
        public string GenerateToken(UserAuthenicaticationDTO user);
        public int? ValidateToken(string token);
    }
    public class JWTAuthentiation : IJwtUtils
    {
        private readonly string key;
        
        public JWTAuthentiation()
        {
            this.key = "This is Secret value";
        }
        
        public string? GenerateToken(UserAuthenicaticationDTO user)
        {
            if(user.Email == "zain" && user.Password == "12") 
            {

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId" , user.UserId.ToString()),
                    //new Claim(ClaimTypes.Name, password.ToString())
                }),
                Expires = DateTime.UtcNow.AddSeconds(40),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha384Signature)
            };
            var token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
            }
            else
            {
                return null;
            }
        }
        public int? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(token);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId =  int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

    }
}
