using System.Numerics;
using _353503_Sebelev_Lab1.Collections;
using _353503_Sebelev_Lab1.Contracts;
using _353503_Sebelev_Lab1.Interfaces;

namespace _353503_Sebelev_Lab1.Entities;

public class AirportTicketSystem : IAirportTicketSystem
{
    private MyCustomCollection<Tariff> _tariffs = new MyCustomCollection<Tariff>();
    private MyCustomCollection<Passenger> _passengers = new MyCustomCollection<Passenger>();
    private MyCustomCollection<Ticket> _tickets = new MyCustomCollection<Ticket>();

    public void AddTariff(Tariff tariff)
    {
        _tariffs.Add(tariff);
    }

    public void AddPassenger(Passenger passenger)
    {
        _passengers.Add(passenger);
    }

    public void RegisterTicket(Passenger passenger, Tariff tariff, DateTime date)
    {
        var ticket = new Ticket(passenger, tariff, date);
        _tickets.Add(ticket);
    }

    public decimal CalculateTotalCost<T>(Passenger passenger)
        where T : IAdditionOperators<Tariff, Tariff, Tariff>
    {
        Ticket totalCost = new Ticket(passenger, new Tariff("", 0), new DateTime());
        
        for(int i = 0; i < _tickets.Count; i++)
        {
            if (_tickets[i].Passenger == passenger)
            {
                totalCost += _tickets[i];
            }
        }

        return totalCost.Tariff.Price;
    }

    public MyCustomCollection<Passenger> PassengersByDate(DateTime flightDate)
    {
        MyCustomCollection<Passenger> passengersByDate = new MyCustomCollection<Passenger>();
        
        for (int i = 0; i < _tickets.Count; i++)
        {
            if (_tickets[i].FlightDate == flightDate)
            {
                passengersByDate.Add(_tickets[i].Passenger);
            }
        }

        return passengersByDate;
    }
}