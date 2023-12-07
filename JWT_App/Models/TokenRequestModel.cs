using System.ComponentModel.DataAnnotations;

namespace JWT_App.Models
{
    public class TokenRequestModel
    {
        [Required, StringLength(100)]
        public string Email { get; set; }

        [Required, StringLength(226)]
        public string password { get; set; }
    }
}
