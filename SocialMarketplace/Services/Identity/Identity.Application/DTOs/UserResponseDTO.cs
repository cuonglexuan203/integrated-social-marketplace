namespace Identity.Application.DTOs
{
    public class UserResponseDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        //public ICollection<string> Roles { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ProfileUrl { get; set; }
        public int Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public ICollection<string> Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
