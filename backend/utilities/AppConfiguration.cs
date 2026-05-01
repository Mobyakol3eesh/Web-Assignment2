using MySqlConnector;

internal static class AppConfiguration
{
    internal static string BuildConnectionString(IConfiguration configuration)
    {
        var baseConnectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        var connBuilder = new MySqlConnectionStringBuilder(baseConnectionString)
        {
            Password = password
        };

        return connBuilder.ConnectionString;
    }

    internal static JwtConfig LoadJwtConfig(IConfiguration configuration)
    {
        var key = GetRequiredSetting(configuration, "JWT_KEY", "Jwt:Key", "JWT key");
        var issuer = GetRequiredSetting(configuration, "JWT_ISSUER", "Jwt:Issuer", "JWT issuer");
        var audience = GetRequiredSetting(configuration, "JWT_AUDIENCE", "Jwt:Audience", "JWT audience");
        var expirySetting = GetSetting(configuration, "JWT_EXPIRY_MINUTES", "Jwt:ExpiryMinutes");
        var expiryMinutes = int.TryParse(expirySetting, out var minutes) ? minutes : 120;

        return new JwtConfig(key, issuer, audience, expiryMinutes);
    }

    private static string GetRequiredSetting(IConfiguration configuration, string envKey, string configKey, string settingName)
    {
        var value = GetSetting(configuration, envKey, configKey);
        return string.IsNullOrWhiteSpace(value)
            ? throw new InvalidOperationException($"{settingName} is not configured.")
            : value;
    }

    private static string? GetSetting(IConfiguration configuration, string envKey, string configKey)
    {
        var envValue = Environment.GetEnvironmentVariable(envKey);
        return string.IsNullOrWhiteSpace(envValue) ? configuration[configKey] : envValue;
    }
}

internal sealed record JwtConfig(string Key, string Issuer, string Audience, int ExpiryMinutes);