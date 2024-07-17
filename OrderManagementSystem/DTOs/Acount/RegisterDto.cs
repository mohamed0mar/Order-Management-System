using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.DTOs.Acount
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Phone { get; set; } = null!;
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[A-Za-z\d\W_]{6,}$"
                        , ErrorMessage = "Password Must Like P@ssw0rd")]
        public string Password { get; set; } = null!;
    }
}
