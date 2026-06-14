using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Manager.OffboardUserManager.Dto
{
    public class UpdateOffboardStatusDto
    {
        [Required]
        public long OffboardHistoryId { get; set; }

        [Required]
        public bool NeedOffboard { get; set; }
    }
}