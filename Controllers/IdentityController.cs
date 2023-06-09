using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TokenAndRoleBased.Controllers;

public class User
{
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

[ApiController]
[Route("[controller]")]
public class IndetityController : ControllerBase
{
    private static readonly User[] Users = new[]
    {
        new User(){ Email="superadmin@gmail.com", Role="Admin,Accounting,Product"},
        new User(){ Email="user@gmail.com", Role="User"},
        new User(){ Email="admin@gmail.com", Role="Admin"},
    };

    private readonly ILogger<IndetityController> _logger;
    private readonly IAudience _audience;

    public IndetityController(ILogger<IndetityController> logger, IAudience audience)
    {
        _logger = logger;
        _audience = audience;
    }


    [AllowAnonymous]
    [HttpGet("GetAllowAnonymous")]
    public string Get()
    {
        return "Accessed Allow Annoymous";
    }

    [Authorize("Admin")]
    [Authorize("User")]
    [HttpGet("GetAdminOrUser")]
    public string GetAdminOrUser()
    {
        return "Accessed Admin or User";
    }

    [Authorize("User")]
    [HttpGet("GetUser")]
    public string GetUser()
    {
        return "Accessed User";
    }

    [Authorize("SuperAdmin")]
    [HttpGet("SuperAdminAccess")]
    public string GetSuperAdminAccess()
    {
        return "Accessed SuperAdminAccess";
    }

    [HttpGet("Login/{email}")]
    public string Login(string email)
    {

        var roles = Users.First(x => x.Email == email).Role;
        Claim[] claims = new[]
            {
                new Claim(
                    ClaimTypes.Role,
                    roles),

                new Claim(
                    "id",
                    "test"),
            };

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        SymmetricSecurityKey key = new(Encoding.ASCII.GetBytes(this._audience.Secret));
        SigningCredentials credentals = new(key, SecurityAlgorithms.HmacSha256);
        var expireDateTime = DateTime.UtcNow.AddHours(double.Parse(_audience.ExpiresAt));
        JwtSecurityToken jwt = new(
            claims: claims,
            issuer: this._audience.Iss,
            expires: expireDateTime,
            audience: this._audience.Aud,
            notBefore: DateTime.UtcNow,
            signingCredentials: credentals
        );

        return jwtSecurityTokenHandler.WriteToken(jwt);
    }


}
