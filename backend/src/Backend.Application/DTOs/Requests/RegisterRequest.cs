namespace Backend.Application.DTOs.Requests;

public record RegisterRequest(string Username, string Password, string PasswordRepeat);
