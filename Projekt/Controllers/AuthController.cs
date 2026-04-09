using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class AuthController : Controller
{
    private readonly UserRepository _repo = new UserRepository();

    public ActionResult Login()
    {
        return View(new LoginVM());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(LoginVM model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = _repo.GetByUsername(model.Username);
        if (user == null)
        {
            ModelState.AddModelError("", "Pogrešan username ili lozinka.");
            return View(model);
        }

        var hash = PasswordHelper.Sha256(model.Password);
        if (hash != user.PasswordHash)
        {
            ModelState.AddModelError("", "Pogrešan username ili lozinka.");
            return View(model);
        }

        Session["UserId"] = user.Id;
        Session["Username"] = user.Username;
        Session["Role"] = user.Role;

        return RedirectToAction("Index", "Home");
    }

    public ActionResult Register()
    {
        return View(new RegisterVM());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Register(RegisterVM model)
    {
        if (!ModelState.IsValid) return View(model);

        var existing = _repo.GetByUsername(model.Username);
        if (existing != null)
        {
            ModelState.AddModelError("", "Username već postoji.");
            return View(model);
        }

        var hash = PasswordHelper.Sha256(model.Password);
        _repo.Create(model.Username, hash);

        return RedirectToAction("Login");
    }

    public ActionResult Logout()
    {
        Session.Clear();
        return RedirectToAction("Login");
    }
}
