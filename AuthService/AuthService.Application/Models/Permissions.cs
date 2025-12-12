namespace AuthService.Application.Models;

/// <summary>
/// Central permission definitions for the entire system
/// </summary>
public static class Permissions
{
    // User Permissions (default for User role)
    public const string ReadOwnProfile = "ReadOwnProfile";
    public const string UpdateOwnProfile = "UpdateOwnProfile";
    public const string DeleteOwnAccount = "DeleteOwnAccount";
    public const string ReadBooks = "ReadBooks";
    public const string AddReview = "AddReview";

    // Moderator Permissions (all User permissions + these)
    public const string ReadAllProfiles = "ReadAllProfiles";
    public const string UpdateAnyProfile = "UpdateAnyProfile";
    public const string CheckReview = "CheckReview";
    public const string ApproveReview = "ApproveReview";
    public const string RejectReview = "RejectReview";
    public const string AddBook = "AddBook";
    public const string DeleteBook = "DeleteBook";
    public const string EditBook = "EditBook";
    public const string AddAuthor = "AddAuthor";
    public const string DeleteAuthor = "DeleteAuthor";
    public const string EditAuthor = "EditAuthor";

    // Admin Permissions (all Moderator permissions + these)
    public const string BlockUser = "BlockUser";
    public const string UnblockUser = "UnblockUser";
    public const string DeleteUser = "DeleteUser";
    public const string ManageRoles = "ManageRoles";
    public const string ViewAnalytics = "ViewAnalytics";
    public const string ManageSystem = "ManageSystem";

    /// <summary>
    /// Get permissions for a specific role
    /// </summary>
    public static List<string> GetPermissionsForRole(UserRole role) => role switch
    {
        UserRole.User => GetUserPermissions(),
        UserRole.Moderator => GetModeratorPermissions(),
        UserRole.Admin => GetAdminPermissions(),
        _ => new List<string>()
    };

    private static List<string> GetUserPermissions() => new()
    {
        ReadOwnProfile,
        UpdateOwnProfile,
        DeleteOwnAccount,
        ReadBooks,
        AddReview
    };

    private static List<string> GetModeratorPermissions()
    {
        var permissions = GetUserPermissions();
        permissions.AddRange(new[]
        {
            ReadAllProfiles,
            UpdateAnyProfile,
            CheckReview,
            ApproveReview,
            RejectReview,
            AddBook,
            DeleteBook,
            EditBook,
            AddAuthor,
            DeleteAuthor,
            EditAuthor
        });
        return permissions;
    }

    private static List<string> GetAdminPermissions()
    {
        var permissions = GetModeratorPermissions();
        permissions.AddRange(new[]
        {
            BlockUser,
            UnblockUser,
            DeleteUser,
            ManageRoles,
            ViewAnalytics,
            ManageSystem
        });
        return permissions;
    }
}