namespace Identity.Application.Commands.User.Create
{
    public class LoadExistingUserCommand
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmationPassword { get; set; }
        public ICollection<string>? Roles { get; set; } = ["user"];
        public string? ProfilePictureUrl { get; set; }
        public string? ProfileUrl { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public ICollection<string>? Interests { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
