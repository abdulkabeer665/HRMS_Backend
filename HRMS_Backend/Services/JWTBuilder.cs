using HRMS_Backend.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HRMS_Backend.Services
{
    public class JWTBuilder
    {

        #region "Get Validation Parameters"

        public static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(EncryptorDecryptorEngine.password))
            };
        }

        #endregion

        #region "Token Generation Method"

        public static Tuple<string, DateTime?> Generation(string loginName, string userName, string roleID, string status)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(EncryptorDecryptorEngine.password);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(UserClaimParameters.LOGINNAME.ToString(), loginName.ToString()),
                    new Claim(UserClaimParameters.USERNAME.ToString(), userName.ToString()),
                    new Claim(UserClaimParameters.ROLEID.ToString(), roleID.ToString()),
                    new Claim(UserClaimParameters.STATUS.ToString(), status.ToString()),
                }),
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new Tuple<string, DateTime?>(tokenString, tokenDescriptor.Expires);
        }

        public static ClaimsPrincipal GetClaimsFromExpiredToken(string authToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();
                validationParameters.ValidateLifetime = false;
                SecurityToken validateToken;
                try
                {
                    ClaimsPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validateToken);

                    var jwtSecurityToken = validateToken as JwtSecurityToken;
                    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                        throw new SecurityTokenException("Invalid Token");

                    return principal;
                }
                catch (Exception ex)
                {

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        #endregion

    }
}
