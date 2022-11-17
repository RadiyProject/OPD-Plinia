namespace Plinia_AuthService.Models;

public class JwtSettings
{
    public JwtSettings()
    {
        SecretKey = "";
        Issuer = "";
        Audience = "";
    }

    public string SecretKey { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }
}