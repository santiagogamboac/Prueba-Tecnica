namespace Backend.Application.Models.ViewModels.Access
{
    public class RoleModuleVm
    {
        public bool Read { get; set; } = false;
        public bool Create { get; set; } = false;
        public bool Edit { get; set; } = false;
        public bool Status { get; set; } = false;
        public bool Download { get; set; } = false;
        public bool Delete { get; set; } = false;
        public bool SpecialConditions { get; set; } = false;
    }
}
