using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NewsProject.Models.Data
{
    public class AppDbContext: IdentityDbContext<IdentityUser>
    {
       
        public AppDbContext(DbContextOptions<AppDbContext> option):base(option)
        {

        }
    }
}
