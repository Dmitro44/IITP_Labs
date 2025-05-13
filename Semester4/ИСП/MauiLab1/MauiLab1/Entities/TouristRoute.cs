using SQLite;

namespace MauiLab1.Entities;

[Table("TouristRoutes")]
public class TouristRoute
{
    [PrimaryKey, AutoIncrement, Indexed]
    public int Id { get; set; }

    [NotNull]
    public string Name { get; set; }
    
    public double Distance { get; set; }
    
    public int DurationMinutes { get; set; }
    
    public string Difficulty { get; set; }
    
    public string Region { get; set; }
    
    public bool IsActive { get; set; }
    
    public DateTime CreatedDate { get; set; }
}