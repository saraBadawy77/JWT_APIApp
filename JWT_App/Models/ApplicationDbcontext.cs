using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWT_App.Models
{
    
        public class ApplicationDbcontext : IdentityDbContext<ApplaicationUser>
        {
            public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options)
            {

            }
        }
    
}
