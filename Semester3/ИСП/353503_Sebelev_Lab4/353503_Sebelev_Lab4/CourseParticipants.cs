namespace _353503_Sebelev_Lab4;

public class CourseParticipants
{
    public int Age { get; set; }
    
    public bool HasPaid { get; set; }
    
    public string? Name { get; set; }

    public CourseParticipants(int age, bool hasPaid, string name)
    {
        Age = age;
        HasPaid = hasPaid;
        Name = name;
    }
}