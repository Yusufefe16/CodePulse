using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CodePulse.API.Repositories.Implementation;

public class TokenRepository : ITokenRepository
{
    private readonly IConfiguration _configuration;

    public TokenRepository(IConfiguration configuration)
    {
        this._configuration = configuration;
    }
    
    public string CreteJwtToken(IdentityUser user, List<string> roles)
    {
        // Create Claims

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // JWT Security Token Parameters
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        
        var creadential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creadential);
        
        // Return Token
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}