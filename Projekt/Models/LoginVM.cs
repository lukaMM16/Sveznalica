using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

public class LoginVM
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
