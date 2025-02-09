using System.Text;
using System.Text.Json;

namespace ClassLibrary;

public class StreamService<T>
{
    public async Task WriteToStreamAsync(Stream stream, IEnumerable<T> data, IProgress<string> progress)
    {
        progress.Report($"Поток {Thread.CurrentThread.ManagedThreadId}: Вход в метод WriteToStreamAsync");
        progress.Report($"Поток {Thread.CurrentThread.ManagedThreadId}: Начало записи данных в поток");
        await JsonSerializer.SerializeAsync(stream, data, new JsonSerializerOptions{WriteIndented = true});
        await Task.Delay(4000);
        progress.Report($"Поток {Thread.CurrentThread.ManagedThreadId}: Завершение записи данных в поток");
    }

    public async Task CopyFromStreamAsync(Stream stream, string filename, IProgress<string> progress)
    {
        progress.Report($"Поток {Thread.CurrentThread.ManagedThreadId}: Вход в метод CopyFromStreamAsync");
        await using FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
        progress.Report($"Поток {Thread.CurrentThread.ManagedThreadId}: Начало записи данных в поток");
        stream.Position = 0;
        await stream.CopyToAsync(fileStream);
        progress.Report($"Поток {Thread.CurrentThread.ManagedThreadId}: Завершение записи данных в поток");
    }

    public async Task<int> GetStatisticsAsync(string filename, Func<T, bool> filter)
    {
        await using FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
        var objects = await JsonSerializer.DeserializeAsync<List<T>>(fileStream) ?? new List<T>();

        return objects.Count(filter);
    }
}