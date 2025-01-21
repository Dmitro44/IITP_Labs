namespace _353503_Sebelev_Lab5.Domain;

public interface ISerializer
{
    IEnumerable<Bus> DeSerializeByLINQ(string fileName);
    IEnumerable<Bus>? DeSerializeXML(string fileName);
    IEnumerable<Bus> DeSerializeJSON(string fileName);
    void SerializeByLINQ(IEnumerable<Bus> buses, string fileName);
    void SerializeXML(IEnumerable<Bus> buses, string fileName);
    void SerializeJSON(IEnumerable<Bus> buses, string fileName);
}