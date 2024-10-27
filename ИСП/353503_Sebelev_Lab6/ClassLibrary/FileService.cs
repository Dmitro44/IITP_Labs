using System.Text.Json;
using _353503_Sebelev_Lab6;

namespace ClassLibrary;

public class FileService<T> : IFileService<T> where T : class
{
    public void SaveData(IEnumerable<T> data, string fileName)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileName, json);
    }

    public IEnumerable<T> ReadFile(string fileName)
    {
        var json = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize<List<T>>(json);
    }
}