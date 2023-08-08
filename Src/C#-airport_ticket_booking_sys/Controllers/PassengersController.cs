using AirportTrackingSystem.Models;
namespace AirportTrackingSystem.Controllers;

public class PassengerController
{
    private List<Passenger> passengers = new List<Passenger>();
    public bool IsLoggedIn { set; get; } = false;
    private Passenger CurrentPassenger { set; get; } = new Passenger();


    public bool AddPassenger(string name, string password)
    {
        Passenger passenger = new Passenger { Name = name, Password = password, Flights = new List<Flight>() };
        passengers.Add(passenger);
        IsLoggedIn = true;
        CurrentPassenger = passenger;
        return IsLoggedIn;
    }
    public void BookFlight(Flight flight)
    {
        CurrentPassenger.Flights.Add(flight);

    }
    public List<Flight> PassengerBookings()
    {
        return CurrentPassenger.Flights;
    }
    public void ShowPassengers()
    {
        foreach (var item in passengers)
        {
            Console.WriteLine($"Name={item.Name}");
        }

    }



}
