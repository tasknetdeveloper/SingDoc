using Microsoft.EntityFrameworkCore;
using Model;
namespace DBWork
{
    
    public class ApplicationContext : DbContext
    {
        public DbSet<Doc>? Doc { get; set; } = null;
        private string connection = "";

        public ApplicationContext(string connection)
        {
            this.connection = connection;
            Database.EnsureCreated();           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(this.connection,

                ServerVersion.AutoDetect(this.connection),
                options => options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null));
        }
    }
}
