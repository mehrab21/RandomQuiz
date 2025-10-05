using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RandomQuizAnswer.Data;
using RandomQuizAnswer.Models;
using RandomQuizAnswer.Models.Domain;

namespace RandomQuizAnswer.Service
{

    public interface IAdminService
    {
        public Task<List<Question>> PostQuestion(AddQuestionDto question);
        public Task<List<Question>> GetAllQuestions();
        public Task<IActionResult> DeleteQuestionAsync(int id);
        public Task<Question> GetById(int id);
        public Task<Question> EditQuestionAsync(int id, Question addQuestionDto);
    }
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<IActionResult> DeleteQuestionAsync(int id)
        {
            var result = _context.Questions.FirstOrDefault(q => q.Id == id);
            if (result == null)
            {
                return Task.FromResult((IActionResult)new NotFoundResult());
            }
            else
            {
                _context.Questions.Remove(result);
                _context.SaveChanges();
                return Task.FromResult((IActionResult)new OkResult());
            }
        }

        public Task<Question> EditQuestionAsync(int id, Question addQuestionDto)
        {
                
                var question = new Question()
                {
                    Id = id,
                    Text = addQuestionDto.Text,
                    Deficulty = addQuestionDto.Deficulty,
                    Options = addQuestionDto.Options,
                    Answer = addQuestionDto.Answer
                };
                _context.Questions.Update(question);
                _context.SaveChanges();
                return Task.FromResult(question);
        }

        public Task<List<Question>> GetAllQuestions()
        {
            var result = _context.Questions.ToList();
            return Task.FromResult(result);
        }

        public async Task<Question> GetById(int id)
        {
            var result = await _context.Questions.FindAsync(id);
            if (result == null)
            {
                throw new ArgumentException("Question not found");
            }
            else
            {
                var question = new Question()
                {
                    Id = result.Id,
                    Text = result.Text,
                    Deficulty = result.Deficulty,
                    Options = result.Options,
                    Answer = result.Answer
                };
                return question;
            }
        }

        public Task<List<Question>> PostQuestion(AddQuestionDto question)
        {
            var nameExists = _context.Questions.Any(q => q.Text == question.Text);
            if (nameExists)
            {
                throw new ArgumentException("Question already exists");
            }
            if(question.Options.Count < 2)
            {
                throw new ArgumentException("At least two options are required");
            }
            var newQuestion = new Question()
            {
                Text = question.Text,
                Deficulty = question.Deficulty,
                Options = question.Options,
                Answer = question.Answer
            };
            _context.Questions.Add(newQuestion);
            _context.SaveChanges();
            return GetAllQuestions();
        }
    }
}
