using Microsoft.EntityFrameworkCore;
using LoginRegistration.Models;

namespace LoginRegistration.Models
{
    public class LoginRegContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public LoginRegContext(DbContextOptions<LoginRegContext> options) : base(options) { }
        public DbSet<UserObject> Users { get; set; }
    }
}