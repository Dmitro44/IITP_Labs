namespace _353503_Sebelev_Lab4;

class Program
{
    static void Main(string[] args)
    {
        string path = @"..\..\..\Sebelev_Lab4";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }

            Console.WriteLine("All directories and files deleted in " + path);
        }

        string[] extensions = { ".txt", ".rtf", ".dat", ".inf" };
        Random random = new Random();

        for (int i = 0; i < 10; ++i)
        {
            string fileName = Path.GetRandomFileName();
            string extension = extensions[random.Next(extensions.Length)];
            string fullPath = Path.Combine(path, Path.ChangeExtension(fileName, extension));
            
            File.Create(fullPath).Dispose();
            Console.WriteLine("Created file: " + fullPath);
        }
        
        DirectoryInfo dirr = new DirectoryInfo(path);
        foreach (FileInfo file in dirr.GetFiles())
        {
            Console.WriteLine($"Файл: {file.Name} имеет расширение {file.Extension}");
        }

        List<CourseParticipants> courseParticipants = new();
        
        courseParticipants.Add(new CourseParticipants(23, true, "Dmitry"));
        courseParticipants.Add(new CourseParticipants(45, false, "Roman"));
        courseParticipants.Add(new CourseParticipants(13, true, "Ivan"));
        courseParticipants.Add(new CourseParticipants(28, true, "Artem"));
        courseParticipants.Add(new CourseParticipants(33, false, "Kristina"));
        courseParticipants.Add(new CourseParticipants(41, false, "Petr"));

        FileService<CourseParticipants> fileService = new FileService<CourseParticipants>();
        FileInfo[] files = dirr.GetFiles();
        FileInfo randFile = files[random.Next(files.Length)];
        fileService.SaveData(courseParticipants, $"{randFile.FullName}");
        Console.WriteLine($"Data saved in file: {randFile.FullName}\n");

        randFile.MoveTo(@"..\..\..\Sebelev_Lab4\renamedFile" + randFile.Extension, true);

        List<CourseParticipants> participantsForRenamedFile = fileService.ReadFile(randFile.FullName).ToList();

        MyCustomComparer<CourseParticipants> cmp = new MyCustomComparer<CourseParticipants>();
        var sortedListOfParticipants = participantsForRenamedFile
            .OrderBy(participant => participant, cmp)
            .ToList();

        Console.WriteLine("Original collection:");
        foreach (var p in participantsForRenamedFile)
        {
            if (p.Name != null) 
            {
                Console.WriteLine($"Name: {p.Name}, Age: {p.Age}, HasPaid: {p.HasPaid}");
            }
        }

        Console.WriteLine("Collection sorted by name:");
        foreach (var p in sortedListOfParticipants)
        {
            if (p.Name != null) 
            {
                Console.WriteLine($"Name: {p.Name}, Age: {p.Age}, HasPaid: {p.HasPaid}");
            }
        }

        Console.WriteLine('\n');

        var sortedParticipantByAge = participantsForRenamedFile
            .OrderBy(participant => participant.Age)
            .ToList();

        Console.WriteLine("Collection sorted by age:");
        foreach (var p in sortedParticipantByAge)
        {
            if (p.Name != null) 
            {
                Console.WriteLine($"Name: {p.Name}, Age: {p.Age}, HasPaid: {p.HasPaid}");
            }
        }
    }
}