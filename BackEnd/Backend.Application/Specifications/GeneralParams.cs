namespace Backend.Application.Specifications
{
    public class GeneralParams  : PaginationParams
    {
        public int? CategoryId { get; set; }
        public bool? Discontinued { get; set; }
        public string? SearchTerm{ get; set; }       
    }
}