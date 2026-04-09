using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

public class Question
{
    public int Id { get; set; }

    [Required]
    public int QuizId { get; set; }

    [Required(ErrorMessage = "Unesite pitanje")]
    [StringLength(500)]
    public string Text { get; set; }

    [Range(1, 50, ErrorMessage = "Bodovi 1-50")]
    public int Points { get; set; } = 1;

    
    public string QuizTitle { get; set; }
}
