using Backend.Application.DTOs.Requests;
using Backend.Domain.Entities;

namespace Backend.Application.Mappings;

public static class UserMapper
{
    public static UserDTO ToDto(this User user) => 
        new(user.UserId, user.Username);
    public static User ToEntity(this CreateUserRequest request) => new() 
    { 
        UserId = Guid.NewGuid(), 
        Username = request.Username, 
        PasswordHash = request.PasswordHash
    };

}
