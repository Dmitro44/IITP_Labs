using ClassLibrary;

namespace _353503_Sebelev_LabWork8;

class Program
{
    static async Task Main(string[] args)
    {
        List<EducationalCourse> courses = new List<EducationalCourse>();
        for (int i = 0; i < 1000; ++i)
        {
            EducationalCourse course = new(new Random().Next(), LoremNET.Lorem.Words(1), new Random().Next(1, 100));
            courses.Add(course);
        }
        
        
        const string filename = "courses.json";
        using MemoryStream memoryStream = new MemoryStream();
        Progress<string> progress = new Progress<string>(message => Console.WriteLine(message));
        StreamService<EducationalCourse> streamService = new();

        var task1 = streamService.WriteToStreamAsync(memoryStream, courses, progress);
        
        Thread.Sleep(200);

        var task2 = streamService.CopyFromStreamAsync(memoryStream, filename, progress);

        Task.WaitAll(task1, task2);

        var countTask = streamService.GetStatisticsAsync(filename, course => course.NumOfListeners > 10);
        Console.WriteLine("Ожидание получения статистических данных...");
        await countTask;
        Console.WriteLine($"Количество курсов с более чем 10-ю слушателями: {countTask.Result}");
    }
}