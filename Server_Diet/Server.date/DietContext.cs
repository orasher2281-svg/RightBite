using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.date
{
    public class DietContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<UserMeal> UserMeals { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //         => optionsBuilder.UseSqlServer("Server=DESKTOP-8MJ87A7;Database=DietServer;Trusted_Connection=True;TrustServerCertificate=True");
        public DietContext(DbContextOptions<DietContext> options) : base(options)
        {
        }
       














    }
}
