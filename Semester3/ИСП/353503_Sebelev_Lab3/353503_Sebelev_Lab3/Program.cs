using _353503_Sebelev_Lab3.Entities;
namespace _353503_Sebelev_Lab3;

class Program
{
    static void Main(string[] args)
    {
        AirportTicketSystem airportTicketSystem = new AirportTicketSystem();
        Journal journal = new Journal();

        airportTicketSystem.TariffAdded += airportTicketSystem.OnTariffChanged;
        airportTicketSystem.TicketPurchased += airportTicketSystem.OnTicketPurchased;
        
        airportTicketSystem.RegisterTicket("Ivan", "123456", "Cheep",
            "Belarus - Moscow", 200, new DateTime(2021, 10, 10));
        airportTicketSystem.RegisterTicket("Petr", "654321", "Expensive",
            "Belarus - USA", 1000, new DateTime(2019, 06, 11));
        airportTicketSystem.RegisterTicket("Dmitry", "152647", "Medium",
            "Belarus - Kiev", 500, new DateTime(2014, 02, 24));
        airportTicketSystem.RegisterTicket("Ivan", "477127", "Very Expensive",
            "Belarus - Netherlands", 4000, new DateTime(2015, 07, 17));
        

        foreach (var t in airportTicketSystem.GetListOfTariffs())
        {
            Console.WriteLine(t);
        }

        Console.WriteLine($"Total cost of all tickets: {airportTicketSystem.TotalCostOfAllTickets()}");

        Console.WriteLine($"Passenger who paid the maximum amount: {airportTicketSystem.GetNameOfPassengerPaidMaxSum()}");

        int threshold = 100;
        Console.WriteLine($"Number of passengers who pais more than {threshold}: {airportTicketSystem.GetNumOfPassengersPaidMoreThan(threshold)}");

        string passengerName = "Ivan";
        Console.WriteLine($"List of the amounts paid by the passenger {passengerName} in each direction:");
        foreach (var o in airportTicketSystem.GetAmountsPaidByPassenger(passengerName))
        {
            Console.WriteLine($"Destination: {o.Key}, Total cost: {o.Value}");
        }
    }
}