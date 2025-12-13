using BookService.Application.Models;

namespace BookService.Application.Interfaces;

public interface IJwtTokenService
{
    UserClaimsDto ExtractClaimsFromToken(string token);
}