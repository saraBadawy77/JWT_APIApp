using System.ComponentModel.DataAnnotations;

namespace JWT_App.Models
{
    public class RegistrationModel
    {
        [Required, StringLength(100)]
        public string FristName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }
        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        [Required, StringLength(226)]
        public string password { get; set; }
    }
}
