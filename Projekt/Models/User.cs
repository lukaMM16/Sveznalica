using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string AvatarUrl { get; set; }

    public string PasswordHash { get; set; }
    public string Role { get; set; } // admin ili user
}
