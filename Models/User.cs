using System.ComponentModel.DataAnnotations;

namespace Med.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Category { get; set; }

        public string Password { get; set; }
        public string Role { get; set; }

        public int IsVerified { get; set; }
        public int IsBlocked { get; set; }
        public int IsEmailConfirmed { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int Count { get; set; }

        public List<PassedTest> PassedTests { get; set; } = new();
    }
}
