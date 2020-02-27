using System.ComponentModel.DataAnnotations;

namespace BugAssist.Models.Administration
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
