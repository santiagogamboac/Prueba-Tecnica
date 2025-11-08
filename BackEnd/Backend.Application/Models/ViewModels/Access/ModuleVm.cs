namespace Backend.Application.Models.ViewModels.Access
{
    public class ModuleVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlImg { get; set; }
        public string Path { get; set; }
        public int Order { get; set; }
        public RoleModuleVm Access { get; set; }
        public IReadOnlyList<ModuleVm> ChildModules { get; set; }
    }
}
