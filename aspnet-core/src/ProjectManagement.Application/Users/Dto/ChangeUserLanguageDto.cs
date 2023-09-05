using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}