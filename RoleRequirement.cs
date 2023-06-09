using Microsoft.AspNetCore.Authorization;

namespace TokenAndRoleBased;

public class RoleRequirement : IAuthorizationRequirement
{
    public RoleRequirement(string role) => Role = role;

    public string Role { get; }
}
