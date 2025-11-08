namespace Backend.Application.Models.Common
{
    public class GeneralResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public GeneralResponse(bool status, string value)
        {
            Status = status;
            Message = value;
        }

        public GeneralResponse()
        {
        }
    }

}
