using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

public class ProfileVM
{
    public int Id { get; set; }

    public string Username { get; set; }

    [Display(Name = "Avatar URL")]
    [StringLength(255)]
    public string AvatarUrl { get; set; }

    public int AttemptsCount { get; set; }

    public int BestScore { get; set; }

    public string LastQuizName { get; set; }
}