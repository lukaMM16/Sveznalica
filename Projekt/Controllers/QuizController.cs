using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

[AdminOnly]
public class QuizController : Controller
{
    private readonly QuizRepository _quizRepo = new QuizRepository();
    private readonly CategoryRepository _catRepo = new CategoryRepository();

    private void LoadCategoriesDropDown(object selected = null)
    {
        var cats = _catRepo.GetAll()
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .ToList();

        ViewBag.CategoryId = new SelectList(cats, "Value", "Text", selected);
    }

    public ActionResult Index(int? categoryId, int? difficulty)
    {
        //  dropdown
        LoadCategoriesDropDown(categoryId);

        var quizzes = _quizRepo.GetAll();

        if (categoryId.HasValue)
            quizzes = quizzes.Where(q => q.CategoryId == categoryId.Value).ToList();

        if (difficulty.HasValue)
            quizzes = quizzes.Where(q => q.Difficulty == difficulty.Value).ToList();

        return View(quizzes);
    }

    public ActionResult Create()
    {
        LoadCategoriesDropDown();
        return View(new Quiz());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Quiz model)
    {
        if (!ModelState.IsValid)
        {
            LoadCategoriesDropDown(model.CategoryId);
            return View(model);
        }

        _quizRepo.Insert(model);
        return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
        var quiz = _quizRepo.GetById(id);
        if (quiz == null) return HttpNotFound();

        LoadCategoriesDropDown(quiz.CategoryId);
        return View(quiz);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Quiz model)
    {
        if (!ModelState.IsValid)
        {
            LoadCategoriesDropDown(model.CategoryId);
            return View(model);
        }

        _quizRepo.Update(model);
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        var quiz = _quizRepo.GetById(id);
        if (quiz == null) return HttpNotFound();

        return View(quiz);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        _quizRepo.Delete(id);
        return RedirectToAction("Index");
    }
}