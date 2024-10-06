namespace _353503_Sebelev_Lab4;

public class MyCustomComparer<T> : IComparer<T> where T : CourseParticipants
{
    public int Compare(T? x, T? y)
    {
        if (x == null || y == null)
        {
            throw new ArgumentException("Arguments cannot be null");
        }

        return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
    }
}