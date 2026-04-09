using System.ComponentModel.DataAnnotations;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Unesite naziv kategorije")]
    [StringLength(80)]
    public string Name { get; set; }
}
