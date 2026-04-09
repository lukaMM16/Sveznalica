using System.Web.Mvc;

namespace Projekt.Controllers
{
    public class ResultsController : Controller
    {
        private readonly AttemptRepository _attRepo = new AttemptRepository();

        
        public ActionResult My()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Auth");

            int userId = (int)Session["UserId"];
            var list = _attRepo.GetByUserId(userId, 50);

            return View(list);
        }
    }
}
