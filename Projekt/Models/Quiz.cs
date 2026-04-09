using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

public class Quiz
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Unesite naziv kviza")]
    [StringLength(120)]
    public string Title { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Range(1, 3, ErrorMessage = "Težina mora biti 1-3")]
    public int Difficulty { get; set; } // 1 lako 2 srednje 3 teško

    [Range(10, 3600, ErrorMessage = "Time limit 10-3600 sek")]
    public int TimeLimitSec { get; set; } = 60;

   
    public string CategoryName { get; set; }
}
