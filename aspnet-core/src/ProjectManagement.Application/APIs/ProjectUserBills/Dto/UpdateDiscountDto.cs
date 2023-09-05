using Abp.AutoMapper;
using ProjectManagement.Entities;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class UpdateDiscountDto
    {
        public long ProjectId { get; set; }
        public float Discount { get; set; }
    }
}
