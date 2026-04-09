using System;

public class Challenge
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }

   
    public string QuizTitle { get; set; }
    public string FromUsername { get; set; }
    public string ToUsername { get; set; }
}