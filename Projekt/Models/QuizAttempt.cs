using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class QuizAttempt
{
    public int Id { get; set; }
    public int QuizId { get; set; }

    public int? UserId { get; set; }     
    public string Username { get; set; } 

    public int Score { get; set; }
    public DateTime StartedAt { get; set; }

    public DateTime? FinishedAt { get; set; }  
    public string QuizTitle { get; set; }
}
