using System.ComponentModel.DataAnnotations;

namespace Med.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }

        public string QuestionText { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Answer { get; set; }
    }
}
