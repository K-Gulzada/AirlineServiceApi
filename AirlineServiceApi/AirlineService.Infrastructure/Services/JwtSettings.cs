namespace AirlineService.Infrastructure.Services;

/// <summary>
/// JWT configuration settings.
/// </summary>
public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    /// <summary>
    /// Secret key for signing tokens (min 32 characters).
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Token issuer.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Token audience.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration in hours.
    /// </summary>
    public int ExpirationHours { get; set; } = 1;
}
