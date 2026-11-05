namespace Backend.Domain.Result.Errors;

public static class UserErrors
{
    public static Error NotFound => 
        Error.NotFound("User.NotFound", "User was not found.");
    public static Error UsernameAlreadyExists =>   
        Error.Conflict("User.UsernameExists", "Username already taken.");
    public static Error PasswordTooWeak => 
        Error.Validation("User.PasswordTooWeak", "Password must be alteast 8 charactes with lowercase, uppercase letters and digits.");
    public static Error InvalidCredentials => 
        Error.Validation("User.InvalidCredentials", "Username or password is incorrect.");
    public static Error PasswordsDontMatch => 
        Error.Validation("User.PasswordsDontMatch", "Entered password don't match.");
}
