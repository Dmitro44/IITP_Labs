using System;
using System.Numerics;
using _353503_Sebelev_Lab3.Interfaces;

namespace _353503_Sebelev_Lab3.Entities;

public class Ticket : IAdditionOperators<Ticket, Ticket, Ticket>
{
    public Passenger Passenger { get; set; }
    public Tariff Tariff { get; set; }
    public DateTime FlightDate { get; set; }

    public Ticket(Passenger passenger, Tariff tariff, DateTime flightDate)
    {
        Passenger = passenger;
        Tariff = tariff;
        FlightDate = flightDate;
    }

    public static Ticket operator +(Ticket a, Ticket b)
    {
        return new Ticket(a.Passenger, a.Tariff + b.Tariff, a.FlightDate);
    }
}