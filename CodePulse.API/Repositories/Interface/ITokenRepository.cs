using Microsoft.AspNetCore.Identity;

namespace CodePulse.API.Repositories.Interface;

public interface ITokenRepository
{
    string CreteJwtToken(IdentityUser user, List<string> roles);
}