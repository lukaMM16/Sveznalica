using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

public class AnswerOption
{
    public int Id { get; set; }

    [Required]
    public int QuestionId { get; set; }

    [Required(ErrorMessage = "Unesite odgovor")]
    [StringLength(300)]
    public string Text { get; set; }

    public bool IsCorrect { get; set; }
}
