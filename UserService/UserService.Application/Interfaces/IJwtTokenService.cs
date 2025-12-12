using UserService.Application.Contracts;

namespace UserService.Application.Interfaces;

public interface IJwtTokenService
{
    UserClaimsDto ExtractClaimsFromToken(string token);
}