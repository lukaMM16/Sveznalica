using System;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }

    
    public string OwnerUsername { get; set; }
}