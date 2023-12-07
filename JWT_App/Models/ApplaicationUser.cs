using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JWT_App.Models
{
    public class ApplaicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string FristName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }
    }
}
