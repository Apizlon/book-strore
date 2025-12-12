using AuthService.Application.Interfaces;
using AuthService.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthService.DataAccess.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AuthDbContext _dbContext;
    private readonly ILogger<AuthRepository> _logger;

    public AuthRepository(AuthDbContext dbContext, ILogger<AuthRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<AuthUser?> GetAuthUserByIdAsync(string userId)
    {
        return await _dbContext.AuthUsers.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<AuthUser?> GetAuthUserByUsernameAsync(string username)
    {
        return await _dbContext.AuthUsers.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task CreateAuthUserAsync(AuthUser authUser)
    {
        _dbContext.AuthUsers.Add(authUser);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Auth user created in database - {UserId}", authUser.Id);
    }

    public async Task UpdateAuthUserAsync(AuthUser authUser)
    {
        _dbContext.AuthUsers.Update(authUser);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Auth user updated in database - {UserId}", authUser.Id);
    }

    public async Task DeleteAuthUserAsync(string userId)
    {
        var authUser = await _dbContext.AuthUsers.FirstOrDefaultAsync(u => u.Id == userId);
        if (authUser != null)
        {
            _dbContext.AuthUsers.Remove(authUser);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Auth user deleted from database - {UserId}", userId);
        }
    }
}