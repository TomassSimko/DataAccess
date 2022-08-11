using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ProductAPI.Configuration.Token;

public class JwtToken : IJwtToken {
    private readonly IConfiguration _config;

    public JwtToken(IConfiguration config)
    {
        _config = config;
    }

    public string CreateToken(List<string> roles, string userId, double duration)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, userId),
        };

        foreach (var i in roles)
            claims.Add(new Claim(ClaimTypes.Role, i));


        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _config.GetSection("JwtConfig:Secret").Value));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(duration),
            signingCredentials: credentials
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}