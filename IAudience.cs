namespace TokenAndRoleBased;

public interface IAudience
{
    string Secret { get; init; }
    string ExpiresAt { get; init; }
    string Iss { get; init; }
    string Aud { get; init; }
}

