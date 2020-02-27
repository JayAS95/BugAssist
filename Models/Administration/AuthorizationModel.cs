using System.ComponentModel.DataAnnotations;

namespace BugAssist.Models.Administration
{
    public class AuthorizationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string[] AddIds { get; set; }
        public string[] DeleteIds { get; set; }
    }
}
