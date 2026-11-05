namespace Backend.Application.DTOs.Requests;

public record CreateUserRequest(string Username, string Password, string PasswordRepeat);