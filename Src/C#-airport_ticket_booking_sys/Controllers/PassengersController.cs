using AirportTrackingSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace AirportTrackingSystem.Controllers;

public class PassengerController
{
    private List<Passenger> passengers = new List<Passenger>();
    public bool IsLoggedIn { set; get; } = false;
    private Passenger CurrentPassenger { set; get; } = new Passenger();


    public bool AddPassenger(string name, string password)
    {
        Passenger passenger = new Passenger { Name = name, Password = password, Flights = new List<Flight>() };
        var validationContext = new ValidationContext(passenger);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(passenger, validationContext, validationResults, validateAllProperties: true))
        {
            foreach (var validationResult in validationResults)
            {
                Console.WriteLine($"{validationResult.ErrorMessage}");
            }
            return IsLoggedIn;
            ;
        }
        passengers.Add(passenger);
        IsLoggedIn = true;
        CurrentPassenger = passenger;
        return IsLoggedIn;
    }
    public bool Login(string name, string password)
    {
        Passenger passenger = passengers.SingleOrDefault(passenger =>
        (passenger.Name == name) &&
        (passenger.Password == password));
        IsLoggedIn = passenger != null ? true : false;
        CurrentPassenger = passenger;
        return IsLoggedIn;
    }



    public bool Logout()
    {
        IsLoggedIn = false;
        CurrentPassenger = null;
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
    public List<Passenger> ReturnPassengers()
    {
        return passengers;
    }



}
