using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

public class QuestionEditVM
{
    public int Id { get; set; }

    [Required]
    public int QuizId { get; set; }

    [Required]
    [StringLength(500)]
    public string Text { get; set; }

    [Range(1, 50)]
    public int Points { get; set; }

    
    public List<AnswerOption> Options { get; set; } = new List<AnswerOption>();
}
