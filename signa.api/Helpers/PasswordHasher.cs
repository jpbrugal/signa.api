namespace signa.api.Helpers;

using BCrypt = BCrypt.Net.BCrypt;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        return BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Verify(password, hash);
    }
}