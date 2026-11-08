using Backend.Domain.Enums;

namespace Backend.Application.DTOs.Responses;

public record InvitationResponse(Guid PlayerId, InvitationStatus Status, DateTime? RespondedAt);