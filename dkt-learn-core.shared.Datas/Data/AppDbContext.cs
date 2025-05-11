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
    public DbSet<Reply> Replies { get; set; }
    public DbSet<Like> Likes { get; set; }  
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // LearningPath e Module (1:N)
        modelBuilder.Entity<LearningPath>()
            .HasMany(lp => lp.Modules)
            .WithOne(m => m.LearningPath)
            .HasForeignKey(m => m.LearningPathId)
            .OnDelete(DeleteBehavior.Cascade); // Se deletar trilha, deleta módulos

        // Module e Content (1:N)
        modelBuilder.Entity<Module>()
            .HasMany(m => m.Contents)
            .WithOne(c => c.Module)
            .HasForeignKey(c => c.ModuleId)
            .OnDelete(DeleteBehavior.Cascade); // Se deletar módulo, deleta conteúdos

        // UserProgress (N:N entre Content e "usuário genérico")
        modelBuilder.Entity<UserProgress>()
            .HasKey(up => new { up.UserId, up.ContentId });

        modelBuilder.Entity<UserProgress>()
            .HasOne<Content>()
            .WithMany()
            .HasForeignKey(up => up.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Post e Reply (1:N)
        modelBuilder.Entity<Post>()
            .HasMany(p => p.Replies)
            .WithOne(r => r.Post)
            .HasForeignKey(r => r.PostId)
            .OnDelete(DeleteBehavior.Cascade); // Se deletar post, deleta respostas

        // Like (composta: UserId + PostId)
        modelBuilder.Entity<Like>()
            .HasKey(l => new { l.UserId, l.PostId });

        modelBuilder.Entity<Like>()
            .HasOne(l => l.Post)
            .WithMany(p => p.Curtidas)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade); // Se deletar post, deleta likes
    }

}