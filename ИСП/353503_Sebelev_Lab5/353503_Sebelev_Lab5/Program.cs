using _353503_Sebelev_Lab5.Domain;
using Microsoft.Extensions.Configuration;
using SerializerLib;
using Microsoft.Extensions.Configuration.Json;

namespace _353503_Sebelev_Lab5;

class Program
{
    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        string FileNameBase = configuration["FileName"];
        
        List<Bus> buses = new List<Bus>();
        buses.Add(new Bus("123", "Mercedes", 50, new List<Driver> { new("Ivan", "123", 5) }));
        buses.Add(new Bus("283", "Volvo", 40, new List<Driver> { new("Petr", "456", 3) }));
        buses.Add(new Bus("333", "Scania", 30, new List<Driver> { new("Vasya", "789", 7) }));
        buses.Add(new Bus("444", "MAN", 45, new List<Driver> { new("Alex", "321", 4) }));
        buses.Add(new Bus("555", "Iveco", 35, new List<Driver> { new("John", "654", 6) }));
        buses.Add(new Bus("666", "DAF", 25, new List<Driver> { new("Mike", "987", 2) }));
        
        ISerializer serializer = new Serializer();
        serializer.SerializeByLINQ(buses, $"{FileNameBase}_LINQ.xml");
        serializer.SerializeXML(buses, $"{FileNameBase}.xml");
        serializer.SerializeJSON(buses, $"{FileNameBase}.json");
        
        List<Bus> busesByLINQ = serializer.DeSerializeByLINQ($"{FileNameBase}_LINQ.xml").ToList();
        List<Bus> busesXML = serializer.DeSerializeXML($"{FileNameBase}.xml").ToList();
        List<Bus> busesJSON = serializer.DeSerializeJSON($"{FileNameBase}.json").ToList();
        
        Console.WriteLine("Buses by LINQ:");
        foreach (var bus in busesByLINQ)
        {
            Console.WriteLine($"Bus number: {bus.BusNumber}");
            Console.WriteLine($"Model: {bus.Model}");
            Console.WriteLine($"Capacity: {bus.Capacity}");
            foreach (var driver in bus.Drivers)
            {
                Console.WriteLine($"Driver name: {driver.Name}");
                Console.WriteLine($"License number: {driver.LicenseNumber}");
                Console.WriteLine($"Years of experience: {driver.YearsOfExperience}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("Buses by XML:");
        foreach (var bus in busesXML)
        {
            Console.WriteLine($"Bus number: {bus.BusNumber}");
            Console.WriteLine($"Model: {bus.Model}");
            Console.WriteLine($"Capacity: {bus.Capacity}");
            foreach (var driver in bus.Drivers)
            {
                Console.WriteLine($"Driver name: {driver.Name}");
                Console.WriteLine($"License number: {driver.LicenseNumber}");
                Console.WriteLine($"Years of experience: {driver.YearsOfExperience}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("Buses by JSON:");
        foreach (var bus in busesJSON)
        {
            Console.WriteLine($"Bus number: {bus.BusNumber}");
            Console.WriteLine($"Model: {bus.Model}");
            Console.WriteLine($"Capacity: {bus.Capacity}");
            foreach (var driver in bus.Drivers)
            {
                Console.WriteLine($"Driver name: {driver.Name}");
                Console.WriteLine($"License number: {driver.LicenseNumber}");
                Console.WriteLine($"Years of experience: {driver.YearsOfExperience}");
            }
        }


        if (buses.SequenceEqual(busesByLINQ))
            Console.WriteLine("LINQ deserialization matches the original data.");
        else
            Console.WriteLine("LINQ deserialization does not match the original data.");

        if (buses.SequenceEqual(busesXML))
            Console.WriteLine("XML deserialization matches the original data.");
        else
            Console.WriteLine("XML deserialization does not match the original data.");

        if (buses.SequenceEqual(busesJSON))
            Console.WriteLine("JSON deserialization matches the original data.");
        else
            Console.WriteLine("JSON deserialization does not match the original data.");
    }
}