using Microsoft.EntityFrameworkCore;
using Tienda.Models;

namespace Tienda
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Producto> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Usuario>().ToTable("usuario");
      modelBuilder.Entity<Producto>().ToTable("productos");

   
      modelBuilder.Entity<Producto>()
          .Property(p => p.Precio)
          .HasColumnType("decimal(18, 2)");
    }
  }
}


