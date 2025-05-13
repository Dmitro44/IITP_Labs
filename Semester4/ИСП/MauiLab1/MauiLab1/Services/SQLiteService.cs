using MauiLab1.Entities;
using SQLite;

namespace MauiLab1.Services;

public class SqLiteService : IDbService
{
    private SQLiteConnection _database;
    private readonly string _dbPath;

    public SqLiteService()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "TouristRoutes.db");
    }

    public IEnumerable<TouristRoute> GetAllTouristRoutes()
    {
        return _database.Table<TouristRoute>().ToList();
    }

    public IEnumerable<Attraction> GetRouteAttractions(int routeId)
    {
        return _database.Table<Attraction>()
            .Where(a => a.RouteId == routeId)
            .ToList();
    }

    public void Init()
    {
        if (File.Exists(_dbPath))
        {
            _database = new SQLiteConnection(_dbPath);
            return;
        }
        
        _database = new SQLiteConnection(_dbPath);
        _database.CreateTable<TouristRoute>();
        _database.CreateTable<Attraction>();
        
        var routes = new List<TouristRoute>
        {
            new TouristRoute
            {
                Name = "Historical City Center",
                Distance = 5.3,
                DurationMinutes = 180,
                Difficulty = "Easy",
                Region = "Downtown",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
            },
            new TouristRoute
            {
                Name = "Nature Reserve Trail",
                Distance = 8.7,
                DurationMinutes = 240,
                Difficulty = "Medium",
                Region = "North Reserve",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
            },
            new TouristRoute
            {
                Name = "Coastal Experience",
                Distance = 4.2,
                DurationMinutes = 150,
                Difficulty = "Easy",
                Region = "Eastern Coast",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
            },
            new TouristRoute
            {
                Name = "Mountain Adventure",
                Distance = 12.5,
                DurationMinutes = 360,
                Difficulty = "Hard",
                Region = "Western Mountains",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
            }
        };
        
        _database.InsertAll(routes);
        
        var cityAttractions = new List<Attraction>
        {
            new Attraction { Name = "Old Town Square", Type = "Historical", RouteId = 1, Latitude = 51.5074, Longitude = -0.1278, VisitDurationMinutes = 45, Address = "Old Town Square, City Center" },
            new Attraction { Name = "National Museum", Type = "Cultural", RouteId = 1, Latitude = 51.5068, Longitude = -0.1340, VisitDurationMinutes = 120, Address = "25 Museum Street, City Center" },
            new Attraction { Name = "Cathedral of St. Mary", Type = "Religious", RouteId = 1, Latitude = 51.5138, Longitude = -0.1245, VisitDurationMinutes = 60, Address = "Cathedral Square 1, City Center" },
            new Attraction { Name = "City Hall", Type = "Historical", RouteId = 1, Latitude = 51.5100, Longitude = -0.1300, VisitDurationMinutes = 45, Address = "City Hall Avenue, City Center" },
            new Attraction { Name = "Central Park", Type = "Natural", RouteId = 1, Latitude = 51.5080, Longitude = -0.1350, VisitDurationMinutes = 60, Address = "Central Park Road, City Center" },
            new Attraction { Name = "Historical Theater", Type = "Cultural", RouteId = 1, Latitude = 51.5065, Longitude = -0.1280, VisitDurationMinutes = 120, Address = "15 Theater Street, City Center" }
        };

        var natureAttractions = new List<Attraction>
        {
            new Attraction { Name = "Woodland Entrance", Type = "Natural", RouteId = 2, Latitude = 51.4800, Longitude = -0.2500, VisitDurationMinutes = 20, Address = "Reserve Entrance, North Reserve" },
            new Attraction { Name = "Bird Watching Tower", Type = "Natural", RouteId = 2, Latitude = 51.4820, Longitude = -0.2550, VisitDurationMinutes = 45, Address = "North Trail, North Reserve" },
            new Attraction { Name = "Lake View Point", Type = "Natural", RouteId = 2, Latitude = 51.4850, Longitude = -0.2580, VisitDurationMinutes = 30, Address = "Lake Trail, North Reserve" },
            new Attraction { Name = "Ancient Oak", Type = "Natural", RouteId = 2, Latitude = 51.4870, Longitude = -0.2600, VisitDurationMinutes = 15, Address = "Oak Path, North Reserve" },
            new Attraction { Name = "Wildlife Center", Type = "Educational", RouteId = 2, Latitude = 51.4890, Longitude = -0.2630, VisitDurationMinutes = 60, Address = "Reserve Center, North Reserve" },
            new Attraction { Name = "Waterfall Cascade", Type = "Natural", RouteId = 2, Latitude = 51.4910, Longitude = -0.2660, VisitDurationMinutes = 40, Address = "Waterfall Path, North Reserve" },
            new Attraction { Name = "Butterfly Garden", Type = "Natural", RouteId = 2, Latitude = 51.4930, Longitude = -0.2690, VisitDurationMinutes = 45, Address = "Garden Path, North Reserve" }
        };

        var coastalAttractions = new List<Attraction>
        {
            new Attraction { Name = "Main Beach", Type = "Natural", RouteId = 3, Latitude = 51.3800, Longitude = 1.3500, VisitDurationMinutes = 120, Address = "Main Beach Road, Eastern Coast" },
            new Attraction { Name = "Lighthouse Point", Type = "Historical", RouteId = 3, Latitude = 51.3830, Longitude = 1.3550, VisitDurationMinutes = 45, Address = "Lighthouse Road, Eastern Coast" },
            new Attraction { Name = "Seafood Restaurant", Type = "Cultural", RouteId = 3, Latitude = 51.3860, Longitude = 1.3580, VisitDurationMinutes = 90, Address = "5 Harbor Street, Eastern Coast" },
            new Attraction { Name = "Marine Museum", Type = "Cultural", RouteId = 3, Latitude = 51.3890, Longitude = 1.3610, VisitDurationMinutes = 60, Address = "12 Coastal Road, Eastern Coast" },
            new Attraction { Name = "Cliff Walk", Type = "Natural", RouteId = 3, Latitude = 51.3920, Longitude = 1.3640, VisitDurationMinutes = 75, Address = "Cliff Road, Eastern Coast" },
            new Attraction { Name = "Coastal Village", Type = "Cultural", RouteId = 3, Latitude = 51.3950, Longitude = 1.3670, VisitDurationMinutes = 120, Address = "Village Center, Eastern Coast" }
        };

        var mountainAttractions = new List<Attraction>
        {
            new Attraction { Name = "Trail Head", Type = "Natural", RouteId = 4, Latitude = 51.7500, Longitude = -3.3700, VisitDurationMinutes = 15, Address = "Mountain Base, Western Mountains" },
            new Attraction { Name = "Mountain Lodge", Type = "Service", RouteId = 4, Latitude = 51.7530, Longitude = -3.3750, VisitDurationMinutes = 45, Address = "Mid-Mountain Path, Western Mountains" },
            new Attraction { Name = "Alpine Meadow", Type = "Natural", RouteId = 4, Latitude = 51.7560, Longitude = -3.3780, VisitDurationMinutes = 30, Address = "Upper Trail, Western Mountains" },
            new Attraction { Name = "Summit Viewpoint", Type = "Natural", RouteId = 4, Latitude = 51.7590, Longitude = -3.3810, VisitDurationMinutes = 60, Address = "Summit Path, Western Mountains" },
            new Attraction { Name = "Mountain Waterfall", Type = "Natural", RouteId = 4, Latitude = 51.7620, Longitude = -3.3840, VisitDurationMinutes = 30, Address = "Waterfall Path, Western Mountains" },
            new Attraction { Name = "Ancient Ruins", Type = "Historical", RouteId = 4, Latitude = 51.7650, Longitude = -3.3870, VisitDurationMinutes = 45, Address = "Heritage Path, Western Mountains" },
            new Attraction { Name = "Echo Canyon", Type = "Natural", RouteId = 4, Latitude = 51.7680, Longitude = -3.3900, VisitDurationMinutes = 40, Address = "Canyon Trail, Western Mountains" },
            new Attraction { Name = "Mountain Cabin", Type = "Service", RouteId = 4, Latitude = 51.7710, Longitude = -3.3930, VisitDurationMinutes = 20, Address = "Safety Route, Western Mountains" }
        };

        _database.InsertAll(cityAttractions);
        _database.InsertAll(natureAttractions);
        _database.InsertAll(coastalAttractions);
        _database.InsertAll(mountainAttractions);
    }
}