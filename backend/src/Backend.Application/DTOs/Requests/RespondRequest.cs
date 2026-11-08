using Backend.Domain.Enums;

namespace Backend.Application.DTOs.Requests;

public record RespondRequest(InvitationStatus status);