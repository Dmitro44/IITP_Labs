using System;
using _353503_Sebelev_Lab3.Collections;

namespace _353503_Sebelev_Lab3.Entities;

public class Journal
{
    private MyCustomCollection<string> _events;

    public void OnTariffChanged(string message)
    {
        Console.WriteLine($"Journal: {message}");
    }
    public Journal()
    {
        _events = new MyCustomCollection<string>();
    }

    public void LogEvent(string info)
    {
        _events.Add(info);
    }

    public void PrintAll()
    {
        foreach (var str in _events)
        {
            Console.WriteLine(str);
        }
    }
}