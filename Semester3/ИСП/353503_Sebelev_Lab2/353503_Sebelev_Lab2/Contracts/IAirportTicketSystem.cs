using _353503_Sebelev_Lab2.Collections;
using _353503_Sebelev_Lab2.Entities;

namespace _353503_Sebelev_Lab2.Contracts;

public interface IAirportTicketSystem
{
    void AddTariff(string destination, decimal price);
    void AddPassenger(string name, string passportNumber);

    void RegisterTicket(string passengerName, string passengerPassportNumber, string passengerDestination,
        decimal price, DateTime date);
    decimal CalculateTotalCost(string name, string passortNumber);
    MyCustomCollection<Passenger> PassengersByDate(DateTime flightDate);
}