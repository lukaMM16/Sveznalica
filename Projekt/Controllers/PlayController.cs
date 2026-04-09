using System;
using System.Linq;
using System.Web.Mvc;

namespace Projekt.Controllers
{
    public class PlayController : Controller
    {
        private readonly QuizRepository _quizRepo = new QuizRepository();
        private readonly QuestionRepository _qRepo = new QuestionRepository();
        private readonly AnswerOptionRepository _aRepo = new AnswerOptionRepository();
        private readonly AttemptRepository _attRepo = new AttemptRepository();
        private readonly AttemptAnswerRepository _aaRepo = new AttemptAnswerRepository();
        private readonly ChallengeRepository _challengeRepo = new ChallengeRepository();
        private readonly UserRepository _userRepo = new UserRepository();
        private readonly CategoryRepository _catRepo = new CategoryRepository();

        private bool IsLoggedIn() => Session["UserId"] != null;

        public ActionResult Challenge(int quizId)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            int currentUserId = (int)Session["UserId"];

            var users = _userRepo.GetAll()
                .Where(u => u.Id != currentUserId)
                .ToList();

            ViewBag.QuizId = quizId;
            ViewBag.Users = new SelectList(users, "Id", "Username");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Challenge(int quizId, int toUserId)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            int currentUserId = (int)Session["UserId"];

            _challengeRepo.Insert(new Challenge
            {
                QuizId = quizId,
                FromUserId = currentUserId,
                ToUserId = toUserId,
                Status = "Pending"
            });

            TempData["Success"] = "Izazov je uspješno poslan.";
            return RedirectToAction("MyChallenges");
        }

        public ActionResult MyChallenges()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            int currentUserId = (int)Session["UserId"];
            var challenges = _challengeRepo.GetReceivedByUserId(currentUserId);

            return View(challenges);
        }

        public ActionResult AcceptChallenge(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            var challenge = _challengeRepo.GetById(id);
            if (challenge == null)
                return HttpNotFound();

            _challengeRepo.UpdateStatus(id, "Accepted");

            return RedirectToAction("Start", new { quizId = challenge.QuizId, challenge = true });
        }


        // GET: /Play

        public ActionResult Index(int? categoryId, int? difficulty)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            var quizzes = _quizRepo.GetAll();

            if (categoryId.HasValue)
                quizzes = quizzes.Where(q => q.CategoryId == categoryId.Value).ToList();

            if (difficulty.HasValue)
                quizzes = quizzes.Where(q => q.Difficulty == difficulty.Value).ToList();

            ViewBag.Categories = new SelectList(_catRepo.GetAll(), "Id", "Name", categoryId);
            ViewBag.SelectedDifficulty = difficulty;

            return View("SelectQuiz", quizzes);
        }

        
        public ActionResult SelectQuiz()
        {
            return RedirectToAction("Index");
        }
        public ActionResult Start(int quizId, bool? challenge)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            int userId = (int)Session["UserId"];

            int attemptId = _attRepo.CreateAttempt(quizId, userId);

            Session["attemptId"] = attemptId;
            Session["quizId"] = quizId;
            Session["qIndex"] = 0;
            Session["score"] = 0;

            return RedirectToAction("Question");
        }

        // GET: /Play/Question
        public ActionResult Question()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            if (Session["quizId"] == null ||
                Session["attemptId"] == null ||
                Session["qIndex"] == null)
            {
                return RedirectToAction("Index");
            }

            int quizId = (int)Session["quizId"];
            int attemptId = (int)Session["attemptId"];
            int qIndex = (int)Session["qIndex"];

            var questions = _qRepo.GetByQuizId(quizId);

            if (questions.Count == 0)
            {
                ClearQuizSession();
                return Content("Ovaj kviz nema pitanja.");
            }

            if (qIndex >= questions.Count)
            {
                int score = (int)Session["score"];
                _attRepo.FinishAttempt(attemptId, score);
                ClearQuizSession();
                return RedirectToAction("Result", new { id = attemptId });
            }

            var q = questions[qIndex];
            var opts = _aRepo.GetByQuestionId(q.Id);

            var vm = new PlayQuestionVM
            {
                AttemptId = attemptId,
                QuizId = quizId,
                QuestionIndex = qIndex,
                TotalQuestions = questions.Count,
                QuestionId = q.Id,
                QuestionText = q.Text,
                Points = q.Points,
                Options = opts
            };

            return View(vm);
        }

        // POST: /Play/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(PlayQuestionVM model)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            if (Session["attemptId"] == null ||
                Session["quizId"] == null ||
                Session["qIndex"] == null ||
                Session["score"] == null)
            {
                return RedirectToAction("Index");
            }

            int attemptId = (int)Session["attemptId"];
            int qIndex = (int)Session["qIndex"];
            int score = (int)Session["score"];

            bool isCorrect = false;

            if (model.SelectedOptionId.HasValue)
            {
                var opts = _aRepo.GetByQuestionId(model.QuestionId);
                var chosen = opts.FirstOrDefault(o => o.Id == model.SelectedOptionId.Value);

                if (chosen != null && chosen.IsCorrect)
                {
                    score += model.Points;
                    isCorrect = true;
                }

                _aaRepo.SaveAnswer(
                    attemptId,
                    model.QuestionId,
                    model.SelectedOptionId,
                    isCorrect
                );

                Session["score"] = score;
            }
            else
            {
                _aaRepo.SaveAnswer(
                    attemptId,
                    model.QuestionId,
                    null,
                    false
                );
            }

            Session["qIndex"] = qIndex + 1;
            return RedirectToAction("Question");
        }

        // GET: /Play/Result/5
        public ActionResult Result(int? id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            if (!id.HasValue)
                return RedirectToAction("Index");

            var attempt = _attRepo.GetById(id.Value);
            if (attempt == null)
                return RedirectToAction("Index");

            if (attempt.UserId != (int)Session["UserId"])
                return RedirectToAction("Index");

            return View(attempt);
        }

        private void ClearQuizSession()
        {
            Session.Remove("attemptId");
            Session.Remove("quizId");
            Session.Remove("qIndex");
            Session.Remove("score");
        }
    }
}