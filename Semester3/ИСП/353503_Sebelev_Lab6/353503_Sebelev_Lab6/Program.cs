using System.Reflection;

namespace _353503_Sebelev_Lab6;

class Program
{
    static void Main(string[] args)
    {
        List<Employee> employees = new List<Employee>();
        employees.Add(new Employee("Ivan", 29, true));
        employees.Add(new Employee("Petr", 45, false));
        employees.Add(new Employee("Andrey", 21, true));
        employees.Add(new Employee("Vladimir", 37, false));
        employees.Add(new Employee("Daniil", 25, true));
        employees.Add(new Employee("Kirill", 31, false));

        string fileName = "353503_Sebelev_Lab6.json";

        var assembly = Assembly.LoadFrom("ClassLibrary.dll");
        var type = assembly.GetType("ClassLibrary.FileService`1").MakeGenericType(typeof(Employee));
        
        var instance = Activator.CreateInstance(type);
        var saveData = type.GetMethod("SaveData");
        saveData.Invoke(instance, [employees, fileName]);
        var readData = type.GetMethod("ReadFile");
        var listFromFile = readData.Invoke(instance, [fileName]) as IEnumerable<Employee>;
        foreach (var elem in listFromFile)
        {
            Console.WriteLine($"{elem.Name}, {elem.Age}, {elem.IsRemoteWork}");
        }
    }
}