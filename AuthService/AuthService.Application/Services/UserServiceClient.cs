using AuthService.Application.Interfaces;
using System.Net.Http.Json;
using AuthService.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Services;

public class UserServiceClient : IUserServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserServiceClient> _logger;

    public UserServiceClient(HttpClient httpClient, ILogger<UserServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<UserServiceUserResponse?> GetUserByIdAsync(string userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/users/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserServiceUserResponse>();
            }
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            _logger.LogWarning("UserService returned status {StatusCode} for GetUserById", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling UserService GetUserById");
            throw;
        }
    }

    public async Task<UserServiceUserResponse?> GetUserByUsernameAsync(string username)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/users/by-username/{username}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserServiceUserResponse>();
            }
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            _logger.LogWarning("UserService returned status {StatusCode} for GetUserByUsername", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling UserService GetUserByUsername");
            throw;
        }
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/users/email-exists/{email}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<EmailExistsResponse>();
                return result?.Exists ?? false;
            }

            _logger.LogWarning("UserService returned status {StatusCode} for EmailExists", response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling UserService EmailExists");
            throw;
        }
    }

    public async Task<UserServiceUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/users", request);
            
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogError("UserService returned error {StatusCode}: {Content}", response.StatusCode, content);
                throw new InvalidOperationException($"Failed to create user in UserService: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<UserServiceUserResponse>() 
                ?? throw new InvalidOperationException("UserService returned null response");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling UserService CreateUser");
            throw;
        }
    }

    public async Task<UserServiceUserResponse> UpdateUserAsync(string userId, UpdateUserRequest request)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/users/{userId}", request);
            
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogError("UserService returned error {StatusCode}: {Content}", response.StatusCode, content);
                throw new InvalidOperationException($"Failed to update user in UserService: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<UserServiceUserResponse>() 
                ?? throw new InvalidOperationException("UserService returned null response");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling UserService UpdateUser");
            throw;
        }
    }

    private class EmailExistsResponse
    {
        public bool Exists { get; set; }
    }
}
