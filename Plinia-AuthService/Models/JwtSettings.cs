namespace Plinia_AuthService.Models;

public class JwtSettings
{
    public JwtSettings(string secretKey, string issuer, string audience)
    {
        SecretKey = secretKey;
        Issuer = issuer;
        Audience = audience;
    }

    public string SecretKey { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }
}