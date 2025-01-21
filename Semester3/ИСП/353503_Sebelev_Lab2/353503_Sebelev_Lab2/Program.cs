using _353503_Sebelev_Lab2.Collections;
using _353503_Sebelev_Lab2.Entities;

namespace _353503_Sebelev_Lab2;

class Program
{
    static void Main(string[] args)
    {
        
        AirportTicketSystem airportTicketSystem = new AirportTicketSystem();
        Journal journal = new Journal();    
        
        airportTicketSystem.RegisterTicketHandler(message => Console.WriteLine($"Program: {message}")); // Подписка Program на событие покупки билета
        airportTicketSystem.RegisterTariffHandler(journal.OnTariffChanged); // Подписка Journal на событие изменения списка тарифов
        
        airportTicketSystem.RegisterTicket("Ivan", "123456", "Moscow", 100, new DateTime(2034, 05, 12));
        airportTicketSystem.RegisterTicket("Petr", "654321", "Berlin", 200, new DateTime(2019, 11, 23));
        airportTicketSystem.RegisterTicket("Ivan", "123456", "Moscow", 100, new DateTime(2020, 12, 31));

        try
        {
            Passenger passenger1 = airportTicketSystem.GetPassenger("123486");
            Passenger passenger2 = airportTicketSystem.GetPassenger("654322");
            
            journal.LogEvent($"Ticket registered for {passenger1.Name} on {new DateTime(2034, 05, 12)}");
            journal.LogEvent($"Ticket registered for {passenger2.Name} on {new DateTime(2019, 11, 23)}");
            journal.LogEvent($"Ticket registered for {passenger1.Name} on {new DateTime(2020, 12, 31)}");
            
            journal.PrintAll();

            Console.WriteLine($"Total cost for {passenger1.Name}: {airportTicketSystem.CalculateTotalCost(passenger1.Name, passenger1.PassportNumber)}");
            Console.WriteLine($"Total cost for {passenger2.Name}: {airportTicketSystem.CalculateTotalCost(passenger2.Name, passenger2.PassportNumber)}");
        }
        catch (ItemNotFoundExeption e)
        {
            Console.WriteLine($"Exeption: {e}");
        }
            

        Console.WriteLine("Passengers by date 31.12.2020:");
        MyCustomCollection<Passenger> passengersByDate = airportTicketSystem.PassengersByDate(new DateTime(2020, 12, 31));
        
        for(int i = 0; i < passengersByDate.Count; i++)
        {
            Console.WriteLine(passengersByDate[i].Name);
        }
    }
}