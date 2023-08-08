using System.ComponentModel.DataAnnotations;

namespace Med.Models
{
    public class PassedTest
    {
        [Key]
        public int Id { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int Tries { get; set; }
        public int IsPassed { get; set; }
    }
}
