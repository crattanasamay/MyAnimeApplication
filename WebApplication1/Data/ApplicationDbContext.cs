using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Anime> Anime { get; set; }

        public DbSet<User> Users { get; set; }  

        public DbSet<UserAnime> UserAnime { get; set; }


       
    }

   



}
