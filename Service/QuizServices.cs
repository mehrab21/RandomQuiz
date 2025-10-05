using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomQuizAnswer.Data;
using RandomQuizAnswer.Models.Domain;

namespace RandomQuizAnswer.Service
{
    public interface IQuizService
    {
        Task<int> TotalQuestionAsync();
        Task<List<Question>> GetRandomQuestionAsync(int r);
    }
    public class QuizServices : IQuizService
    {
        private readonly ApplicationDbContext _context;
        public QuizServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Question>> GetRandomQuestionAsync(int r)
        {
            int total = await _context.Questions.CountAsync();
            if (total == 0 || r <= 0) return new List<Question>();

            var random = new Random();
            var selectedIndexes = new HashSet<int>();

            // Generate unique random indexes
            while (selectedIndexes.Count < Math.Min(r, total))
            {
                selectedIndexes.Add(random.Next(0, total));
            }

            var ans = new List<Question>();

            foreach (var index in selectedIndexes)
            {
                var question = await _context.Questions.Skip(index).FirstOrDefaultAsync();
                if (question != null)
                    ans.Add(question);
            }

            return ans;
        }


        public async Task<int> TotalQuestionAsync()
        {
            int Total = await _context.Questions.CountAsync();
            return Total;

        }
    }
}
