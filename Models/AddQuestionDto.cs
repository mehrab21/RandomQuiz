using System.ComponentModel.DataAnnotations;

namespace RandomQuizAnswer.Models
{
    public class AddQuestionDto
    {
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
