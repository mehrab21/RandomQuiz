using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandomQuizAnswer.Models;
using RandomQuizAnswer.Models.Domain;
using RandomQuizAnswer.Service;


namespace RandomQuizAnswer.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
       
        private readonly IAdminService _AdminService;

        public AdminController(IAdminService adminService)
        {
            
            _AdminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            var re = await _AdminService.GetAllQuestions();
            return View(re);
        }
        public IActionResult AddQuestion()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddQuestion(AddQuestionDto model)
        {
            var result = await _AdminService.PostQuestion(model);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var result =await  _AdminService.GetById(id);
            return View(result);
        }
        public IActionResult Delete(int id)
        {
            var result = _AdminService.DeleteQuestionAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditQuestion(int id)
        {
            var result = await _AdminService.GetById(id);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> EditQuestion(int id, Question model)
        {
            var result = await _AdminService.EditQuestionAsync(id,model);
            return RedirectToAction("Index");
        }
    }
}
