using _353503_Sebelev_Lab2.Collections;
using _353503_Sebelev_Lab2.Contracts;

namespace _353503_Sebelev_Lab2.Entities;

public delegate void TariffHandler(string message);

public delegate void TicketHandler(string message);

public class AirportTicketSystem : IAirportTicketSystem
{
    private TariffHandler? _tariffHandler;
    private TicketHandler? _ticketHandler;
    private MyCustomCollection<Tariff> _tariffs = new MyCustomCollection<Tariff>();
    private MyCustomCollection<Passenger> _passengers = new MyCustomCollection<Passenger>();
    private MyCustomCollection<Ticket> _tickets = new MyCustomCollection<Ticket>();

    public void RegisterTariffHandler(TariffHandler del)
    {
        _tariffHandler = del;
    }

    public void RegisterTicketHandler(TicketHandler del)
    {
        _ticketHandler = del;
    }
    public void AddTariff(string destination, decimal price)
    {
        Tariff tariff = new Tariff(destination, price);
        _tariffs.Add(tariff);
        _tariffHandler?.Invoke("Tariff has been added");
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
    public void RegisterTicket(string passengerName, string passengerPassportNumber, string passengerDestination, decimal price, DateTime date)
    {
        AddPassenger(passengerName, passengerPassportNumber);
        AddTariff(passengerDestination, price);
        try
        {
            Ticket ticket = new Ticket(_passengers.GetLast(), _tariffs.GetLast(), date);
            _tickets.Add(ticket);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        _ticketHandler?.Invoke("Ticket has been purchased");
    }

    public decimal CalculateTotalCost(string name, string passportNumber)
    {
        Passenger passenger = new Passenger { Name = name, PassportNumber = passportNumber };
        Ticket totalCost = new Ticket(passenger, new Tariff("", 0), new DateTime());
        
        for(int i = 0; i < _tickets.Count; i++)
        {
            if (_tickets[i].Passenger.Name == passenger.Name &&
                _tickets[i].Passenger.PassportNumber == passenger.PassportNumber)
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