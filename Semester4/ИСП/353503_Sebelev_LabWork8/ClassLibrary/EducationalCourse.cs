namespace ClassLibrary;

public class EducationalCourse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int NumOfListeners { get; set; }

    public EducationalCourse(int id, string? name, int numOfListeners)
    {
        Id = id;
        Name = name;
        NumOfListeners = numOfListeners;
    }
}