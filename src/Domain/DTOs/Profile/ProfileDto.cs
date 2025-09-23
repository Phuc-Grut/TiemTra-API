namespace Domain.DTOs.Profile
{
    public class ProfileDto
    {
        public Guid UserId { get; init; }
        public string UserCode { get; init; }
        public string Email { get; init; }
        public string FullName { get; init; }
        public string? PhoneNumber { get; init; }
        public int? Age { get; init; }
        public string? Address { get; init; }
        public string? Avatar { get; init; }
    }
}