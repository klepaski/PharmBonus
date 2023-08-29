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

            // связи БД
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

            // начальные данные
            builder.Entity<Drug>().HasData(
                new Drug[]
                {
                    new Drug {
                        Id = 1,
                        Title = "Naphazalin",
                        Summary = "Nose spray",
                        Description = "Spray for running nose.",
                        VideoUrl = "https://youtu.be/M8QKjDzb-Os",
                        ImageUrl = "https://www.dropbox.com/scl/fi/i0vh1stcwlhcrw5nspsqo/form_196.jpg?rlkey=oq9tjifcx1kpfl4iqhkgm6fsp&dl=0"
                    },
                    new Drug {
                        Id = 2,
                        Title = "Remantadin",
                        Summary = "White tablets",
                        Description = "Tablets to drink when you catch cold.",
                        VideoUrl = "https://youtu.be/M8QKjDzb-Os",
                        ImageUrl = "https://www.dropbox.com/scl/fi/i0vh1stcwlhcrw5nspsqo/form_196.jpg?rlkey=oq9tjifcx1kpfl4iqhkgm6fsp&dl=0"
                    },
                    new Drug {
                        Id = 3,
                        Title = "Parlazin-Neo",
                        Summary = "Allergy tablets",
                        Description = "The best tablets when you have allergy",
                        VideoUrl = "https://youtu.be/M8QKjDzb-Os",
                        ImageUrl = "https://www.dropbox.com/scl/fi/i0vh1stcwlhcrw5nspsqo/form_196.jpg?rlkey=oq9tjifcx1kpfl4iqhkgm6fsp&dl=0"
                    }
                });

            builder.Entity<User>().HasData(
                new User[]
                {
                    new User {
                        Id = 1,
                        FirstName = "Julia",
                        LastName = "Chistyakova",
                        Email = "julia.klepaski@gmail.com",
                        Region = "Brest region",
                        City = "Brest",
                        Category = "Cardiologist",
                        Password = "1",
                        IsVerified = 0,
                        IsBlocked = 0,
                        IsEmailConfirmed = 1,
                        RegistrationDate = DateTime.Now,
                        LastLoginDate = DateTime.Now,
                        Count = 10,
                        Role = "doctor"
                    },
                    new User {
                        Id = 2,
                        FirstName = "Maxim",
                        LastName = "Dulevich",
                        Email = "maxon@gmail.com",
                        Region = "Brest region",
                        City = "Brest",
                        Category = "Hospital",
                        Password = "1",
                        IsVerified = 1,
                        IsBlocked = 0,
                        IsEmailConfirmed = 0,
                        RegistrationDate = DateTime.Now,
                        LastLoginDate = DateTime.Now,
                        Count = 134,
                        Role = "doctor"
                    }
                });

            builder.Entity<Test>().HasData(
                new Test[]
                {
                    new Test {
                        Id = 1,
                        DrugId = 1,
                        Points = 10
                    }
                });

            builder.Entity<Question>().HasData(
                new Question[]
                {
                    new Question {
                        Id = 1,
                        TestId = 1,
                        QuestionText = "Is nose spray dangerous?",
                        Option1 = "Yes, be careful!",
                        Option2 = "Haha, no, at all.",
                        Option3 = "It's safe if use according to instructions",
                        Answer = "It's safe if use according to instructions"
                    },
                    new Question {
                        Id = 2,
                        TestId = 1,
                        QuestionText = "Do you like naphazalin?",
                        Option1 = "Yes, but no very much...",
                        Option2 = "No, it's terrible!",
                        Option3 = "Yes, it's literally the best!",
                        Answer = "Yes, it's literally the best!"
                    },
                    new Question {
                        Id = 3,
                        TestId = 1,
                        QuestionText = "How long can you take it?",
                        Option1 = "Endlessly",
                        Option2 = "Never",
                        Option3 = "Up to 5 days",
                        Answer = "Up to 5 days"
                    }
                });
        }
    }
}
