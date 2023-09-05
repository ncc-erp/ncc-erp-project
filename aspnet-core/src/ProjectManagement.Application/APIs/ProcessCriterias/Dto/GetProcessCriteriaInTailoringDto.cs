using System.Collections.Generic;

namespace ProjectManagement.APIs.ProcessCriterias.Dto
{
    public class GetProcessCriteriaInTailoringDto
    {
        public long ProcessCriteriaId { get; set; }
        public string ProcessCriteriaName { get; set; }
        public string ProcessCriteriaCode { get; set; }
        public List<TailoringInforDto> TailoringInfor { get; set; }
    }
}