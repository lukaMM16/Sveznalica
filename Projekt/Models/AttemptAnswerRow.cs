using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AttemptAnswerRow
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; }

    public int? SelectedOptionId { get; set; }
    public string SelectedText { get; set; }

    public int CorrectOptionId { get; set; }
    public string CorrectText { get; set; }

    public bool IsCorrect { get; set; }
    public int Points { get; set; }
}