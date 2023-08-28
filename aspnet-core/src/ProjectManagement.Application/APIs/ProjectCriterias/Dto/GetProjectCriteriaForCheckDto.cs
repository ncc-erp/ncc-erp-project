namespace ProjectManagement.APIs.ProjectCriterias.Dto
{
    public class GetProjectCriteriaForCheckDto
    {
        public string Guideline { get; set; }

        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsExist { get; set; }

        public bool IsActive { get; set; }
    }
}