using _353503_Sebelev_Lab4.Interfaces;
using System.IO;

namespace _353503_Sebelev_Lab4;

public class FileService<T> : IFileService<T>
{
    public IEnumerable<T> ReadFile(string fileName)
    {
        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            while (fs.Position < fs.Length)
            {
                object value = ReadValue(reader);
                if (value != null)
                {
                    yield return (T)value;
                }
            }
        }
    }

    private object ReadValue(BinaryReader reader)
    {
        Type type = typeof(T);
        try
        {
            if (type == typeof(CourseParticipants))
            {
                string name = reader.ReadString();
                int age = reader.ReadInt32();
                bool hasPaid = reader.ReadBoolean();
                return new CourseParticipants(age, hasPaid, name);
            }
            
            throw new NotSupportedException($"Type {type} is not supported");
            
        }
        catch (IOException e)
        {
            Console.WriteLine($"IOException: {e}");
            throw;
        }
        catch (ObjectDisposedException e)
        {
            Console.WriteLine($"Object Disposed Exception: {e}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"General Exception: {e}");
            throw;
        }
    }

    public void SaveData(IEnumerable<T> data, string fileName)
    {
        using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            foreach (T item in data)
            {
                WriteValue(writer, item);
            }
        }
    }

    private void WriteValue(BinaryWriter writer, T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Value cannot be null");
        }
        
        try
        {
            if (typeof(T) == typeof(CourseParticipants))
            {
                CourseParticipants participant = (CourseParticipants)(object)value;
                writer.Write(participant.Name ?? string.Empty);
                writer.Write(participant.Age);
                writer.Write(participant.HasPaid);
            }
            else
            {
                throw new InvalidOperationException("Unsupported type");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine($"IOException: {e}");
        }
        catch (ObjectDisposedException e)
        {
            Console.WriteLine($"Object Disposed Exception: {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"General Exception: {e}");
        }
    }
}