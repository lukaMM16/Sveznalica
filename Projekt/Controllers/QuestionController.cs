using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


[AdminOnly]
public class QuestionController : Controller
{
    private readonly QuestionRepository _qRepo = new QuestionRepository();
    private readonly AnswerOptionRepository _aRepo = new AnswerOptionRepository();
    private readonly QuizRepository _quizRepo = new QuizRepository();

    private void LoadQuizDropDown(object selected = null)
    {
        var quizzes = _quizRepo.GetAll()
            .Select(q => new SelectListItem { Value = q.Id.ToString(), Text = q.Title })
            .ToList();

        ViewBag.QuizId = new SelectList(quizzes, "Value", "Text", selected);
    }

    public ActionResult Index()
    {
        return View(_qRepo.GetAll());
    }

    public ActionResult Create()
    {
        LoadQuizDropDown();

        var vm = new QuestionEditVM
        {
            Points = 1,
            Options = new[]
            {
                new AnswerOption(),
                new AnswerOption(),
                new AnswerOption(),
                new AnswerOption()
            }.ToList()
        };

        // default: prvi odgovor je točan
        ViewBag.CorrectIndex = 0;

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(QuestionEditVM model, int correctIndex = 0)
    {
        // dodatna provjera: sva 4 odgovora moraju biti upisana
        for (int i = 0; i < model.Options.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(model.Options[i]?.Text))
            {
                ModelState.AddModelError("", "Sva 4 odgovora moraju biti upisana.");
                break;
            }
        }

        if (!ModelState.IsValid)
        {
            LoadQuizDropDown(model.QuizId);
            ViewBag.CorrectIndex = correctIndex;
            return View(model);
        }

        // postavi točan odgovor prema correctIndex
        for (int i = 0; i < model.Options.Count; i++)
            model.Options[i].IsCorrect = (i == correctIndex);

        // 1đ Insert pitanje
        int newQuestionId = _qRepo.Insert(new Question
        {
            QuizId = model.QuizId,
            Text = model.Text,
            Points = model.Points
        });

        // 2 Insert odgovori
        _aRepo.ReplaceAll(newQuestionId, model.Options);

        return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
        var q = _qRepo.GetById(id);
        if (q == null) return HttpNotFound();

        var opts = _aRepo.GetByQuestionId(id);

        // osiguraj 4 odgovora za formu
        while (opts.Count < 4) opts.Add(new AnswerOption());

        
        int correctIndex = opts.FindIndex(o => o.IsCorrect);
        if (correctIndex < 0) correctIndex = 0;

        LoadQuizDropDown(q.QuizId);

        var vm = new QuestionEditVM
        {
            Id = q.Id,
            QuizId = q.QuizId,
            Text = q.Text,
            Points = q.Points,
            Options = opts
        };

        ViewBag.CorrectIndex = correctIndex;

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(QuestionEditVM model, int correctIndex = 0)
    {
        for (int i = 0; i < model.Options.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(model.Options[i]?.Text))
            {
                ModelState.AddModelError("", "Sva 4 odgovora moraju biti upisana.");
                break;
            }
        }

        if (!ModelState.IsValid)
        {
            LoadQuizDropDown(model.QuizId);
            ViewBag.CorrectIndex = correctIndex;
            return View(model);
        }

        for (int i = 0; i < model.Options.Count; i++)
            model.Options[i].IsCorrect = (i == correctIndex);

        // update pitanje
        _qRepo.Update(new Question
        {
            Id = model.Id,
            QuizId = model.QuizId,
            Text = model.Text,
            Points = model.Points
        });

        // zamijeni odgovore
        _aRepo.ReplaceAll(model.Id, model.Options);

        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        var q = _qRepo.GetById(id);
        if (q == null) return HttpNotFound();
        return View(q);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        _qRepo.Delete(id);
        return RedirectToAction("Index");
    }
}
