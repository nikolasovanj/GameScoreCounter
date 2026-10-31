namespace Backend.Domain;

public static class UserErrors
{
    public static Error NotFound => 
        Error.NotFound("User.NotFound", "User was not found.");
    public static Error UsernameAlreadyExists =>   
        Error.Conflict("User.UsernameExists", "Username already taken.");
    public static Error PasswordTooWeak => 
        Error.Validation("User.PasswordTooWeak", "Password must be alteast 8 charactes with lowercase, uppercase letters and digits.");
}
