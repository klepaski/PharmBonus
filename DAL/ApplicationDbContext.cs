using Microsoft.EntityFrameworkCore;
using Med.Models;

namespace Med.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Drug> Drugs { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Test> Tests { get; set; } = null!;
        public DbSet<PassedTest> PassedTests { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Drug>()
                .HasKey(x => x.Id);

            builder.Entity<User>()
                .HasKey(x => x.Id);

            builder.Entity<Question>()
                .HasKey(x => x.Id);

            builder.Entity<Test>()
                .HasKey(x => x.Id);

            builder.Entity<PassedTest>()
                .HasKey(x => x.Id);

            builder.Entity<Question>()
                .HasOne(x => x.Test)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.TestId);

            builder.Entity<Test>()
                .HasOne(x => x.Drug)
                .WithMany(x => x.Tests)
                .HasForeignKey(x => x.DrugId);

            builder.Entity<PassedTest>()
                .HasOne(x => x.User)
                .WithMany(x => x.PassedTests)
                .HasForeignKey(x => x.UserId);

            builder.Entity<PassedTest>()
                .HasOne(x => x.Test)
                .WithMany(x => x.PassedTests)
                .HasForeignKey(x => x.TestId);

        }
    }
}
