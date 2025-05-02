using dkt_learn_core.shared.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace DKT_Learn.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() {}
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<LearningPath> LearningPaths { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Content> Contents { get; set; }
    public DbSet<UserProgress> UserProgress { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LearningPath>()
            .HasMany(lp => lp.Modules)
            .WithOne(m => m.LearningPath)
            .HasForeignKey(m => m.LearningPathId)
            .OnDelete(DeleteBehavior.Cascade); // se deletar trilha, deleta módulos
        
        modelBuilder.Entity<Module>()
            .HasMany(m => m.Contents)
            .WithOne(c => c.Module)
            .HasForeignKey(c => c.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UserProgress>()
            .HasKey(up => new { up.UserId, up.ContentId }
            );

        modelBuilder.Entity<UserProgress>()
            .HasOne<Content>()
            .WithMany()
            .HasForeignKey(up => up.ContentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}