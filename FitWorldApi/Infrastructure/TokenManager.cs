using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitWorldApi.Infrastructure
{
    public class TokenManager
    {
        private const string ISSUER = "api.fit-world.com";
        private const string AUDIENCE = "fit.world";

        internal static string secret = "H%2bUUi3BRJ0GXC3Y1KgS7OA%3d%3d";

        public static string issuer = ISSUER;
        public static string audience = AUDIENCE;

        /*
        public TokenManager()
        {
            private readonly byte[] key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key);
            // string base64secret = Convert.ToBase64String(key).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            // string urlEncoded = base64secret.TrimEnd('=').Replace('+', '-').Replace('/', '_');
            // secret = urlEncoded;
        }
        */

        public string GenerateJWT(dynamic student)
        {
            if (student.Email is null)
            {
                throw new ArgumentNullException(nameof(student.Email));
            }

            // Creating credentials
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secret));

            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha512);

            // Creating the object containing the info stored in the token
            Claim[] claims = new Claim[]
            {
                // TODO: access this claim to get the user/student id to see whether he has clearance to view someone else's details and/or change them
                new Claim(ClaimTypes.Sid, student.StudentId.ToString()),
                new Claim(ClaimTypes.Email, student.Email),
                new Claim(ClaimTypes.Role, student.GetType().Name),
            };

            // Generate the token => NuGet : System.IdentityModel.Tokens.Jwt
            JwtSecurityToken token = new(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials,
                issuer: issuer,
                audience: audience
                );

            JwtSecurityTokenHandler tokenHandler = new();

            return tokenHandler.WriteToken(token);
        }
    }
}
