using System.ComponentModel.DataAnnotations;

namespace Med.Models
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        public int Points { get; set; }

        public int DrugId { get; set; }
        public Drug Drug { get; set; }

        public List<Question> Questions { get; set; } = new();
        public List<PassedTest> PassedTests { get; set; } = new();
    }
}
