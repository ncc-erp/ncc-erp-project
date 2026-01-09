namespace ProjectManagement.APIs.Public.Dto
{
    public class PunishmentRecordsDto
    {
        public string MezonId { get; set; } 
        public string Email { get; set; }    
        public string Date { get; set; }     
        public decimal Amount { get; set; }  
        public string Reason { get; set; }  
    }
}
