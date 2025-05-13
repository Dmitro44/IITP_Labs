using SQLite;

namespace MauiLab1.Entities;

[Table("Attractions")]
public class Attraction
{
    [PrimaryKey, AutoIncrement, Indexed]
    public int Id { get; set; }

    [NotNull]
    public string Name { get; set; }

    [Indexed]
    public int RouteId { get; set; }
    
    public string Type { get; set; }
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
    
    public int VisitDurationMinutes { get; set; }
    
    public string Address { get; set; }
}