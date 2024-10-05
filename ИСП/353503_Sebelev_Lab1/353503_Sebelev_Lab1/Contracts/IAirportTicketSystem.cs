using System.Numerics;
using _353503_Sebelev_Lab1.Collections;
using _353503_Sebelev_Lab1.Entities;

namespace _353503_Sebelev_Lab1.Contracts;

public interface IAirportTicketSystem
{
    void AddTariff(Tariff tariff);
    void AddPassenger(Passenger passenger);
    void RegisterTicket(Passenger passenger, Tariff tariff, DateTime flightDate);
    decimal CalculateTotalCost<T>(Passenger passenger) where T : IAdditionOperators<Tariff, Tariff, Tariff>;
    MyCustomCollection<Passenger> PassengersByDate(DateTime flightDate);
}