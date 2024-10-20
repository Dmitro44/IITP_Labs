using _353503_Sebelev_Lab5.Domain;
using System.Text.Json;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace SerializerLib;

public class Serializer : ISerializer
{
    public IEnumerable<Bus> DeSerializeByLINQ(string fileName)
    {
        XDocument xdoc = XDocument.Load(fileName);
        return xdoc.Descendants("Bus")
            .Select(x => new Bus(
                x.Element("BusNumber")?.Value,
                x.Element("Model")?.Value,
                int.Parse(x.Element("Capacity")?.Value ?? "0"),
            
                x.Descendants("Driver")
                    .Select(d => new Driver(
                        d.Element("Name")?.Value,
                        d.Element("LicenseNumber")?.Value,
                        int.Parse(d.Element("YearsOfExperience")?.Value ?? "0")))
                    .ToList()))
            .ToList();
    }

    public IEnumerable<Bus> DeSerializeXML(string fileName)
    {
        var serializer = new XmlSerializer(typeof(List<Bus>));
        using (var reader = new StreamReader(fileName))
        {
            return (List<Bus>)serializer.Deserialize(reader);
        }
    }

    public IEnumerable<Bus> DeSerializeJSON(string fileName)
    {
        var json = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize<List<Bus>>(json);
    }

    public void SerializeByLINQ(IEnumerable<Bus> buses, string filename)
    {
        XDocument xdoc = new XDocument(new XElement("Buses",
            buses.Select(bus => new XElement("Bus",
                new XElement("BusNumber", bus.BusNumber),
                new XElement("Model", bus.Model),
                new XElement("Capacity", bus.Capacity),
                new XElement("Drivers",
                    bus.Drivers.Select(driver => new XElement("Driver",
                        new XElement("Name", driver.Name),
                        new XElement("LicenseNumber", driver.LicenseNumber),
                        new XElement("YearsOfExperience", driver.YearsOfExperience))))))));
        xdoc.Save(filename);
    }

    public void SerializeXML(IEnumerable<Bus> buses, string fileName)
    {
        var serializer = new XmlSerializer(typeof(List<Bus>));
        using (var writer = new StreamWriter(fileName))
        {
            serializer.Serialize(writer, buses.ToList());
        }
    }

    public void SerializeJSON(IEnumerable<Bus> buses, string fileName)
    {
        var json = JsonSerializer.Serialize(buses, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileName, json);
    }
}