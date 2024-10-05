using System;
using System.Collections.Generic;
using System.Linq;
using _353503_Sebelev_Lab3.Contracts;

namespace _353503_Sebelev_Lab3.Entities;

public delegate void TariffHandler(string message);

public delegate void TicketHandler(string message);

public class AirportTicketSystem : IAirportTicketSystem
{
    public event TariffHandler? TariffAdded;
    public event TicketHandler? TicketPurchased;
    private Dictionary<string, Tariff> _tariffs = new Dictionary<string, Tariff>();
    private List<Passenger> _passengers = new List<Passenger>();
    private List<Ticket> _tickets = new List<Ticket>();
    
    public void AddTariff(string title, string destination, decimal price)
    {
        Tariff tariff = new Tariff(title, destination, price);
        if (!_tariffs.Values.Contains(tariff))
        {
            _tariffs.Add(tariff.Title, tariff);   
        }
        TariffAdded?.Invoke("Tariff has been added");
    }
    
    public void AddPassenger(string name, string passportNumber)
    {
        Passenger passenger = new Passenger { Name = name, PassportNumber = passportNumber };
        _passengers.Add(passenger);
    }
    
    public Passenger GetPassenger(string passportNumber)
    {
        foreach (var p in _passengers)
        {
            if (p.PassportNumber == passportNumber)
            {
                return p;
            }
        }

        throw new ItemNotFoundExeption("Passenger not found");
    }
    
    public void RegisterTicket(string passengerName, string passengerPassportNumber, string tariffType, string passengerDestination, decimal price, DateTime date)
    {
        AddPassenger(passengerName, passengerPassportNumber);
        AddTariff(tariffType, passengerDestination, price);
        Ticket ticket = new Ticket(_passengers.Last(), _tariffs.Last().Value, date);
        _tickets.Add(ticket);
        TicketPurchased?.Invoke("Ticket has been purchased");
    }
    
    public decimal TotalCostOfAllTickets()
    {
        return _tickets
            .Sum(t => t.Tariff.Price);
    }

    public decimal TotalCostOfPassengerTickets(string name, string passportNumber)
    {
        var totalCost = _tickets
            .Where(t => t.Passenger.Name == name && t.Passenger.PassportNumber == passportNumber)
            .Sum(t => t.Tariff.Price);
        return totalCost;
    }
    
    public string GetNameOfPassengerPaidMaxSum()
    {
        return _tickets
            .GroupBy(ticket => ticket.Passenger.Name)
            .Select(group => new
            {
                PassengerName = group.Key,
                TotalPaid = group.Sum(ticket => ticket.Tariff.Price)
            })
            .OrderByDescending(result => result.TotalPaid)
            .First().PassengerName;
    }
    
    public int GetNumOfPassengersPaidMoreThan(decimal price)
    {
        int count = _tickets
            .Where(ticket => ticket.Tariff.Price > price)
            .Select(ticket => ticket.Passenger.Name)
            .Distinct()
            .Count();
        return count;
    }
    
    public Dictionary<string, decimal> GetAmountsPaidByPassenger(string passengerName)
    {
        return _tickets
            .Where(ticket => ticket.Passenger.Name == passengerName)
            .GroupBy(ticket => ticket.Tariff.Destination)
            .Select(group => new
            {
                Destination = group.Key,
                TotalPaid = group.Sum(ticket => ticket.Tariff.Price)
            })
            .ToDictionary(x => x.Destination, x => x.TotalPaid);
    }
    
    public List<string> GetListOfTariffs()
    {
        List<string> output = _tariffs
            .OrderByDescending(t => t.Value.Price)
            .Select(t => t.Key)
            .ToList();
        return output;
    }
    
    public void OnTariffChanged(string message)
    {
        Console.WriteLine($"Airport: {message}");
    }

    public void OnTicketPurchased(string message)
    {
        Console.WriteLine($"Airport: {message}");
    }
}