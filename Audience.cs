namespace TokenAndRoleBased;

public record Audience : IAudience
{
    public string Secret { get; init; } = string.Empty;
    public string ExpiresAt { get; init; } = string.Empty;
    public string Iss { get; init; } = string.Empty;
    public string Aud { get; init; } = string.Empty;
}