using System.ComponentModel.DataAnnotations;

namespace Med.Models
{
    public class Drug
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImageUrl { get; set; }

        public List<Test> Tests { get; set; } = new();
    }
}
