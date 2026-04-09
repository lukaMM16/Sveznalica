using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class ProfileController : Controller
{
    private readonly UserRepository _repo = new UserRepository();
    private readonly AttemptRepository _attRepo = new AttemptRepository();

    public ActionResult Index()
    {
        if (Session["UserId"] == null)
            return RedirectToAction("Login", "Auth");

        int userId = (int)Session["UserId"];

        var user = _repo.GetById(userId);
        var attempts = _attRepo.GetByUser(userId);

        var vm = new ProfileVM
        {
            Id = user.Id,
            Username = user.Username,
            AvatarUrl = user.AvatarUrl,

            AttemptsCount = attempts.Count,
            BestScore = attempts.Any() ? attempts.Max(a => a.Score) : 0,
            LastQuizName = attempts.Any()
                ? attempts.OrderByDescending(a => a.FinishedAt).First().QuizTitle
                : "-"
        };

        return View(vm);
    }

    public ActionResult Edit()
    {
        if (Session["UserId"] == null)
            return RedirectToAction("Login", "Auth");

        int userId = (int)Session["UserId"];
        var user = _repo.GetById(userId);

        var vm = new ProfileVM
        {
            Id = user.Id,
            Username = user.Username,
            AvatarUrl = user.AvatarUrl
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(ProfileVM model)
    {
        if (Session["UserId"] == null)
            return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid)
            return View(model);

        int userId = (int)Session["UserId"];
        _repo.UpdateProfile(userId, model.AvatarUrl);

        return RedirectToAction("Index");
    }
}