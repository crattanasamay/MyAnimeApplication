using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebApplication1.Models;
using WebApplication1.Models.ChartModels.TopOneHundredBarModel;
using WebApplication1.Models.ChartModels.TopOneHundredChart;

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
