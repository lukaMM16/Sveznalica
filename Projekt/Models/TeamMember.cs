using System;

public class TeamMember
{
    public int Id { get; set; }
    public int TeamId { get; set; }
    public int UserId { get; set; }
    public DateTime JoinedAt { get; set; }

    
    public string Username { get; set; }
}