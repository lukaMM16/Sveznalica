using System.Linq;
using System.Web.Mvc;

public class TeamController : Controller
{
    private readonly TeamRepository _teamRepo = new TeamRepository();
    private readonly TeamMemberRepository _teamMemberRepo = new TeamMemberRepository();
    private readonly UserRepository _userRepo = new UserRepository();

    private bool IsLoggedIn() => Session["UserId"] != null;

    public ActionResult Index()
    {
        if (!IsLoggedIn())
            return RedirectToAction("Login", "Auth");

        int userId = (int)Session["UserId"];
        var teams = _teamRepo.GetByOwnerId(userId);

        return View(teams);
    }

    public ActionResult Create()
    {
        if (!IsLoggedIn())
            return RedirectToAction("Login", "Auth");

        int currentUserId = (int)Session["UserId"];
        var users = _userRepo.GetAll()
            .Where(u => u.Id != currentUserId)
            .ToList();

        ViewBag.Users = new MultiSelectList(users, "Id", "Username");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(string name, int[] memberIds)
    {
        if (!IsLoggedIn())
            return RedirectToAction("Login", "Auth");

        if (string.IsNullOrWhiteSpace(name))
        {
            ModelState.AddModelError("", "Unesite ime tima.");
        }

        int currentUserId = (int)Session["UserId"];
        var users = _userRepo.GetAll()
            .Where(u => u.Id != currentUserId)
            .ToList();

        if (!ModelState.IsValid)
        {
            ViewBag.Users = new MultiSelectList(users, "Id", "Username", memberIds);
            return View();
        }

        int teamId = _teamRepo.Insert(new Team
        {
            Name = name,
            OwnerId = currentUserId
        });

        // owner  automatski član 
        if (!_teamMemberRepo.Exists(teamId, currentUserId))
            _teamMemberRepo.Insert(teamId, currentUserId);

        // ostali članovi
        if (memberIds != null)
        {
            foreach (var memberId in memberIds)
            {
                if (!_teamMemberRepo.Exists(teamId, memberId))
                    _teamMemberRepo.Insert(teamId, memberId);
            }
        }

        return RedirectToAction("Details", new { id = teamId });
    }

    public ActionResult Details(int id)
    {
        if (!IsLoggedIn())
            return RedirectToAction("Login", "Auth");

        var team = _teamRepo.GetById(id);
        if (team == null)
            return HttpNotFound();

        ViewBag.Members = _teamMemberRepo.GetByTeamId(id);
        return View(team);
    }
}