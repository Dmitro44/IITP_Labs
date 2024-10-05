using _353503_Sebelev_Lab1.Collections;
using _353503_Sebelev_Lab1.Entities;

namespace _353503_Sebelev_Lab1;

class Program
{
    static void Main(string[] args)
    {
        Tariff tariff1 = new Tariff("Moscow", 100);
        Tariff tariff2 = new Tariff("Berlin", 200);
        Passenger passenger1 = new Passenger { Name = "Ivan", PassportNumber = "123456" };
        Passenger passenger2 = new Passenger { Name = "Petr", PassportNumber = "654321" };
        
        
        AirportTicketSystem airportTicketSystem = new AirportTicketSystem();
        airportTicketSystem.AddTariff(tariff1);
        airportTicketSystem.AddTariff(tariff2);
        airportTicketSystem.AddPassenger(passenger1);
        airportTicketSystem.AddPassenger(passenger2);
        airportTicketSystem.RegisterTicket(passenger1, tariff1, new DateTime(2034, 05, 12));
        airportTicketSystem.RegisterTicket(passenger2, tariff2, new DateTime(2019, 11, 23));
        airportTicketSystem.RegisterTicket(passenger1, tariff2, new DateTime(2020, 12, 31));
        airportTicketSystem.RegisterTicket(passenger2, tariff1, new DateTime(2020, 12, 31));

        Console.WriteLine($"Total cost for {passenger1.Name}: {airportTicketSystem.CalculateTotalCost<Tariff>(passenger1)}");
        Console.WriteLine($"Total cost for {passenger2.Name}: {airportTicketSystem.CalculateTotalCost<Tariff>(passenger2)}");

        Console.WriteLine("Passengers by date 31.12.2020:");
        MyCustomCollection<Passenger> passengersByDate = airportTicketSystem.PassengersByDate(new DateTime(2020, 12, 31));
        
        for(int i = 0; i < passengersByDate.Count; i++)
        {
            Console.WriteLine(passengersByDate[i].Name);
        }
    }
}