
using Microsoft.EntityFrameworkCore;

namespace University.Models
{
  public class UniversityContext : DbContext
  {
    //public DbSet<Item> Items { get; set; }

    public UniversityContext(DbContextOptions options) : base(options) { }
  }
}