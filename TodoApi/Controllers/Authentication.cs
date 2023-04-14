using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Authentication : ControllerBase
{
    private readonly IConfiguration _config;

    public Authentication(IConfiguration config)
    {
        this._config = config;
    }

    public record AuthenticationData(string? UserName, string? Password);

    public record UserData(int Id, string FirstName, string LastName, string UserName);

    [HttpPost("token")]
    [AllowAnonymous]
    public ActionResult<string> Authenticate([FromBody] AuthenticationData data)
    {
        var user = ValidateCredentials(data);

        if(user == null)
        {
            return Unauthorized();
        }

        string token = GenerateToken(user);
        return Ok(token);
    }

    private string GenerateToken(UserData user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
            _config.GetValue<string>("Authentication:SecretKey")));

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new();
        claims.Add(new(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claims.Add(new(JwtRegisteredClaimNames.UniqueName, user.UserName));
        claims.Add(new(JwtRegisteredClaimNames.GivenName, user.FirstName));
        claims.Add(new(JwtRegisteredClaimNames.FamilyName, user.LastName));

        var token = new JwtSecurityToken(
            _config.GetValue<string>("Authentication:Issuer"),
            _config.GetValue<string>("Authentication:Audience"),
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(1),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private UserData? ValidateCredentials(AuthenticationData data)
    {
        // this is not production code - replace with a call to your authentication system
        if(CompareValues(data.UserName, "tcorey") && CompareValues(data.Password, "Test123"))
        {
            return new UserData(1, "Tim", "Corey", data.UserName!);
        }

        if (CompareValues(data.UserName, "sstorm") && CompareValues(data.Password, "Test123"))
        {
            return new UserData(2, "Sue", "Storm", data.UserName!);
        }

        if (CompareValues(data.UserName, "jorge") && CompareValues(data.Password, "Test123"))
        {
            return new UserData(3, "Jorge", "Leon", data.UserName!);
        }

        return null;
    }

    private bool CompareValues(string? actual, string expected)
    {
        if(actual is not null)
        {
            if(actual.Equals(expected))
            {
                return true;
            }
        }
        return false;
    }
}
