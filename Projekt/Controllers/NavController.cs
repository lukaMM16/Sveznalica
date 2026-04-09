using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class NavController : Controller
{
    private readonly CategoryRepository _catRepo = new CategoryRepository();
    private readonly QuizRepository _quizRepo = new QuizRepository();
    private readonly QuestionRepository _qRepo = new QuestionRepository();

    // Renderira navbar menu 
    public ActionResult Menu(string current)
    {
        var vm = new NavMenuVM
        {
            Current = current ?? "",

            CategoriesCount = _catRepo.GetAll().Count,
            QuizzesCount = _quizRepo.GetAll().Count,
            QuestionsCount = _qRepo.GetAll().Count
        };

        return PartialView("_NavMenu", vm);
    }
}