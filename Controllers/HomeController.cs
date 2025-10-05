using Microsoft.AspNetCore.Mvc;
using RandomQuizAnswer.Data;
using RandomQuizAnswer.Models;
using RandomQuizAnswer.Models.Domain;
using RandomQuizAnswer.Service;
using System.Diagnostics;
using System.Text.Json;

namespace RandomQuizAnswer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuizService _quizserver;
        public HomeController(ILogger<HomeController> logger, IQuizService quizService)
        {
            _logger = logger;
            _quizserver = quizService;
        }

        public async Task<IActionResult> Index()
        {
            int result =await _quizserver.TotalQuestionAsync();
            ViewBag.total = result;
            return View();
        }
        public async Task<IActionResult> StartQuiz(string Answer, int id,int index = 0)
        {
            if (HttpContext.Session.GetString("Quiz") == null)
            {
                var question = await _quizserver.GetRandomQuestionAsync(id);
                HttpContext.Session.SetString("Quiz", JsonSerializer.Serialize(question));
                HttpContext.Session.SetString("UserAnswers", JsonSerializer.Serialize(new Dictionary<int, string>()));
                HttpContext.Session.SetInt32("Score", 0);
            }

            var questionList = JsonSerializer.Deserialize<List<Question>>(HttpContext.Session.GetString("Quiz"));
            ViewBag.TotalQuestion = questionList.Count;
            if (!string.IsNullOrEmpty(Answer))
            {
                var userAnswers = JsonSerializer.Deserialize<Dictionary<int, string>>(HttpContext.Session.GetString("UserAnswers"));
                userAnswers[index] = Answer;
                HttpContext.Session.SetString("UserAnswers", JsonSerializer.Serialize(userAnswers));

                if (questionList[index].Answer == Answer)
                {
                    int score = HttpContext.Session.GetInt32("Score") ?? 0;
                    HttpContext.Session.SetInt32("Score", score + 1);
                }
                index++;
            }
            ViewBag.CurrentIndex = index;
            if (index < 0 || index >= questionList.Count)
            {
                return RedirectToAction("ShowResult");
            }

            return View(questionList[index]);
        }

        public IActionResult ShowResult()
        {
            var questionList = JsonSerializer.Deserialize<List<Question>>(HttpContext.Session.GetString("Quiz"));
            var userAnswers = JsonSerializer.Deserialize<Dictionary<int, string>>(HttpContext.Session.GetString("UserAnswers"));
            int score = HttpContext.Session.GetInt32("Score") ?? 0;

            var results = questionList.Select((q, i) => new {
                Question = q.Text,
                CorrectAnswer = q.Answer,
                UserAnswer = userAnswers.ContainsKey(i) ? userAnswers[i] : null,
                IsCorrect = userAnswers.ContainsKey(i) && userAnswers[i] == q.Answer
            }).ToList();

            ViewBag.Score = score;
            ViewBag.Total = questionList.Count;
            ViewBag.Percent = Math.Round(((double)score / questionList.Count) * 100, 2);
            ViewBag.Results = results;

            HttpContext.Session.Remove("Quiz");
            HttpContext.Session.Remove("Score");
            HttpContext.Session.Remove("UserAnswers");

            return View();

        }
    }
}
