using System.ComponentModel.DataAnnotations;

namespace RandomQuizAnswer.Models.Domain
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;
        [Required]
        public string Deficulty { get; set; } = string.Empty;
        [Required]
        public List<string> Options { get; set; } = new List<string>();
        [Required]
        public string Answer { get; set; } = string.Empty;

    }
}
