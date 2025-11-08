namespace Backend.Application.Domain
{
    public class Log
    {
        public int Id { get; set; }
        public string JsonData { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
