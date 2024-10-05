using System.Numerics;
using _353503_Sebelev_Lab2.Interfaces;

namespace _353503_Sebelev_Lab2.Entities;

public class Tariff : IAdditionOperators<Tariff, Tariff, Tariff>
{
    public string Destination { get; set; }
    public decimal Price { get; set; }

    public Tariff(string destination, decimal price)
    {
        Destination = destination;
        Price = price;
    }

    public static Tariff operator +(Tariff a, Tariff b)
    {
        return new Tariff(a.Destination, a.Price + b.Price);
    }
}