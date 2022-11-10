using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Plinia_AuthService.Secure;

namespace Plinia_AuthService.Services;

public class TokenService
{
    private readonly JwtSettings _settings;

    public TokenService(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public string GetAccessToken(string email)
    {
        var payload = new Dictionary<string, object>
        {
            { "sub", email },
            { "email", email }
        };
        return GenerateToken(payload);
    }

    private string GenerateToken(Dictionary<string, object> payload)
    {
        var secretKey = _settings.SecretKey;

        payload.Add("iss", _settings.Issuer);
        payload.Add("aud", _settings.Audience);
        payload.Add("nbf", ConvertToUnixTimestamp(DateTime.Now));
        payload.Add("iat", ConvertToUnixTimestamp(DateTime.Now));
        payload.Add("exp", ConvertToUnixTimestamp(DateTime.Now.AddDays(7)));

        IJwtAlgorithm algorithm = new NoneAlgorithm();
        IJsonSerializer serializer = new JsonNetSerializer();
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

        return encoder.Encode(payload, secretKey);
    }

    private object ConvertToUnixTimestamp(DateTime dateTime)
    {
        var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var diff = dateTime.ToUniversalTime() - origin;
        return Math.Floor(diff.TotalSeconds);
    }

    public string GetIdToken(IdentityUser user)
    {
        var payload = new Dictionary<string, object>
        {
            { "id", user.Id },
            { "sub", user.Email },
            { "email", user.Email },
            { "emailConfirmed", user.EmailConfirmed },
        };
        return GenerateToken(payload);
    }
}