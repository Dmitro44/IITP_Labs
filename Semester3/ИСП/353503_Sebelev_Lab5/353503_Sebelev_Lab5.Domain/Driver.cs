namespace _353503_Sebelev_Lab5.Domain;

public class Driver : IEquatable<Driver>
{
    public string? Name { get; set; }
    public string? LicenseNumber { get; set; }
    public int YearsOfExperience { get; set; }

    public Driver(string? name, string? licenseNumber, int yearsOfExperience)
    {
        Name = name;
        LicenseNumber = licenseNumber;
        YearsOfExperience = yearsOfExperience;
    }
    
    public Driver()
    {
    }

    public bool Equals(Driver? other)
    {
        if (other == null) return false;
        return Name == other.Name &&
               LicenseNumber == other.LicenseNumber &&
               YearsOfExperience == other.YearsOfExperience;
    }
}