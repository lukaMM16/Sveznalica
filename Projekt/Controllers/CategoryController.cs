using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


[AdminOnly]
public class CategoryController : Controller
{
    private readonly CategoryRepository _repo = new CategoryRepository();

    public ActionResult Index()
    {
        return View(_repo.GetAll());
    }

    public ActionResult Create()
    {
        return View(new Category());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Category model)
    {
        if (!ModelState.IsValid) return View(model);

        _repo.Insert(model);
        return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
        var cat = _repo.GetById(id);
        if (cat == null) return HttpNotFound();
        return View(cat);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Category model)
    {
        if (!ModelState.IsValid) return View(model);

        _repo.Update(model);
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        var cat = _repo.GetById(id);
        if (cat == null) return HttpNotFound();
        return View(cat);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        if (_repo.IsUsed(id))
        {
            TempData["Error"] = "Kategorija se ne može obrisati jer je povezana s postojećim kvizovima.";
            return RedirectToAction("Index");
        }

        _repo.Delete(id);
        TempData["Success"] = "Kategorija je uspješno obrisana.";
        return RedirectToAction("Index");
    }
}
