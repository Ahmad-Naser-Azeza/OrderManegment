using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SharedKernel;
public sealed class IdentitySettings : IIdentitySettings
{
    public AzureSamlIdentitySettings AzureSamlIdentitySettings { get; set; }
    public JwtSettings JwtSettings { get; set; }
    public UserLoginSetting UserLoginSetting { get; set; }

    private IdentitySettings()
    {
        AzureSamlIdentitySettings = new AzureSamlIdentitySettings();
        JwtSettings = new JwtSettings();
        UserLoginSetting = new UserLoginSetting();
    }
    public IdentitySettings(IHostEnvironment env)
    {
        var configurationBuilder = new ConfigurationBuilder();
        var instance = new IdentitySettings();

        string identitySettingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, "IdentitySettings", "identitySettings.json");

        if (File.Exists(identitySettingsFilePath))
        {
            configurationBuilder.AddJsonFile(identitySettingsFilePath, false, true);
            configurationBuilder.Build().Bind(instance);

            JwtSettings = instance.JwtSettings;
            AzureSamlIdentitySettings = instance.AzureSamlIdentitySettings;
            UserLoginSetting = instance.UserLoginSetting;
        }
        else
        {
            throw new FileNotFoundException($"Identity settings file not found at {identitySettingsFilePath}");
        }
    }
}
public sealed class AzureSamlIdentitySettings : IAzureSamlIdentitySettings
{
    string issuerBaseUrl { get; set; }
    string loginBaseUrl { get; set; }
    public int TokenExpiryInSeconds { get; set; }
    public string IssuerBaseUrl { get { return issuerBaseUrl; } set { issuerBaseUrl = value.NormalizeUrl(); } }
    public string LoginBaseUrl { get { return loginBaseUrl; } set { loginBaseUrl = value.NormalizeUrl(); } }
    public string AllowedHosts { get; set; }
    public int SessionVolatilityInSeconds { get { return sessionVolatilityInSeconds == 0 ? 15 : sessionVolatilityInSeconds; } set { sessionVolatilityInSeconds = value; } }
    int sessionVolatilityInSeconds = 0;
}
public sealed class JwtSettings : IJwtSettings
{
    string secretKey = "";
    public string SecretKey { get { return string.IsNullOrWhiteSpace(secretKey) ? "jwtmiddlewaresecret" : secretKey; } set { secretKey = value; } }
    int tokenExpiryInSeconds = 0;
    public int TokenExpiryInSeconds { get { return tokenExpiryInSeconds == 0 ? 15 : tokenExpiryInSeconds; } set { tokenExpiryInSeconds = value; } }
}

public sealed class UserLoginSetting : IUserLoginSetting
{
    public int TokenRefreshExpiryInMinutes { get; set; }
    public int TokenExpiryInSeconds { get; set; }
    public int DelayBeforeLoggingOutInMinutes { get; set; }
}