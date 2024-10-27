namespace _353503_Sebelev_Lab6;

public interface IFileService<T> where T:class
{
    IEnumerable<T> ReadFile(string fileName);
    void SaveData(IEnumerable<T> data, string fileName);
}