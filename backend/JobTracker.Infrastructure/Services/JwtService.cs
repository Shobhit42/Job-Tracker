using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JobTracker.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JobTracker.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(Guid userId, string email)
    {
        // Read JWT settings from appsettings.json
        var secret = _configuration["JwtSettings:Secret"]!;
        var issuer = _configuration["JwtSettings:Issuer"]!;
        var audience = _configuration["JwtSettings:Audience"]!;
        var expiryMinutes = int.Parse(_configuration["JwtSettings:ExpiryMinutes"]!);

        // The signing key — server uses this to sign the token
        // Any server with this same secret can verify the token is genuine
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        // The algorithm used to sign — HMAC SHA256 is the standard choice
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Claims = the payload inside the JWT
        // These are readable by anyone who has the token (base64 encoded, not encrypted)
        // Never put sensitive data like passwords in claims
        var claims = new[]
        {
            // sub = subject = who this token belongs to
            // CurrentUserService reads this claim to get UserId
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),

            // Standard email claim
            new Claim(JwtRegisteredClaimNames.Email, email),

            // jti = unique ID for this specific token
            // Useful for token revocation and preventing replay attacks
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Build the token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes), // always UTC
            signingCredentials: credentials
        );

        // Serialize the token object into the string format: xxxxx.yyyyy.zzzzz
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        // Generate 64 cryptographically random bytes
        // RandomNumberGenerator is secure — unlike Random which is predictable
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        // Convert to base64 string — safe to store in DB and send over HTTP
        return Convert.ToBase64String(randomBytes);
    }
}