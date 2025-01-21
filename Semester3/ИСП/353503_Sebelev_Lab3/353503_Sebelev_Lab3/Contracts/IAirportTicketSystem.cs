using System;
using System.Collections.Generic;
using _353503_Sebelev_Lab3.Collections;
using _353503_Sebelev_Lab3.Entities;

namespace _353503_Sebelev_Lab3.Contracts;

public interface IAirportTicketSystem
{
    void AddTariff(string type, string destination, decimal price);
    void AddPassenger(string name, string passportNumber);

    void RegisterTicket(string passengerName, string passengerPassportNumber, string tariffType, string passengerDestination,
        decimal price, DateTime date);
    decimal TotalCostOfAllTickets();
    
    string GetNameOfPassengerPaidMaxSum();
    
    int GetNumOfPassengersPaidMoreThan(decimal price);

    decimal TotalCostOfPassengerTickets(string name, string passportNumber);
    // MyCustomCollection<Passenger> PassengersByDate(DateTime flightDate);

    List<string> GetListOfTariffs();
}