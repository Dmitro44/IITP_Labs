using System.Numerics;
using _353503_Sebelev_Lab3.Interfaces;

namespace _353503_Sebelev_Lab3.Entities;

public class Tariff : IAdditionOperators<Tariff, Tariff, Tariff>
{
    public string Title { get; set; }
    public string Destination { get; set; }
    public decimal Price { get; set; }

    public Tariff(string title, string destination, decimal price)
    {
        Title = title;
        Destination = destination;
        Price = price;
    }

    public static Tariff operator +(Tariff a, Tariff b)
    {
        return new Tariff(a.Title,a.Destination, a.Price + b.Price);
    }
}