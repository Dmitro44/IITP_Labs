namespace _353503_Sebelev_Lab5.Domain;

public class Bus : IEquatable<Bus>
{
    public string? BusNumber { get; set; }
    public string? Model { get; set; }
    public int Capacity { get; set; }
    public List<Driver> Drivers { get; set; }

    public Bus(string? busNumber, string? model, int capacity, List<Driver> drivers)
    {
        BusNumber = busNumber;
        Model = model;
        Capacity = capacity;
        Drivers = drivers;
    }
    
    public Bus()
    {
        Drivers = new List<Driver>();
    }

    public void AddDriver(string name, string licenseNumber, int yearsOfExperience)
    {
        Driver driver = new Driver(name, licenseNumber, yearsOfExperience);
        Drivers.Add(driver);
    }

    public bool Equals(Bus? other)
    {
        if (other == null) return false;
        return BusNumber == other.BusNumber &&
               Model == other.Model &&
               Capacity == other.Capacity &&
               Drivers.SequenceEqual(other.Drivers);
    }
    
    public override bool Equals(object obj) => Equals(obj as Bus);
    public override int GetHashCode() => (BusNumber, Model, Capacity, Drivers).GetHashCode();
}