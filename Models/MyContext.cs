using Microsoft.EntityFrameworkCore;
namespace ProductsCategories.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Association> Associations { get; set; }
    }
}