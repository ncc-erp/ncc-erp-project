using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Users.Dto
{
    public class ResetPasswordDto
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
