namespace _353503_Sebelev_Lab6;

public class Employee
{
    public int Age { get; set; }
    public bool IsRemoteWork { get; set; }
    public string? Name { get; set; }

    public Employee(string name, int age, bool isRemoteWork)
    {
        Name = name;
        Age = age;
        IsRemoteWork = isRemoteWork;
    }
}