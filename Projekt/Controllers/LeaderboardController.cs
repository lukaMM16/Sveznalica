using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class LeaderboardController : Controller
{
    private readonly QuizRepository _quizRepo = new QuizRepository();
    private readonly AttemptRepository _attRepo = new AttemptRepository();

    // /Leaderboard
    public ActionResult Index()
    {
        var quizzes = _quizRepo.GetAll();
        return View(quizzes);
    }

    // /Leaderboard/Quiz/3
    public ActionResult Quiz(int id)
    {
        ViewBag.Quiz = _quizRepo.GetById(id);
        var results = _attRepo.GetTopResults(id);
        return View(results);
    }
}