using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class PlayQuestionVM
{
    public int AttemptId { get; set; }
    public int QuizId { get; set; }

    public int QuestionIndex { get; set; }      
    public int TotalQuestions { get; set; }

    public int QuestionId { get; set; }
    public string QuestionText { get; set; }
    public int Points { get; set; }

    public List<AnswerOption> Options { get; set; } = new List<AnswerOption>();
    public int? SelectedOptionId { get; set; }  // user izbor
}
