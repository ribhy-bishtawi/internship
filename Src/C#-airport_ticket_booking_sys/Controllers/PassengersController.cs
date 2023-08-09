using AirportTrackingSystem.Models;
using System.ComponentModel.DataAnnotations;
using AirportTrackingSystem.Enums;


namespace AirportTrackingSystem.Controllers;

public class PassengerController
{
    private List<Passenger> passengers = new List<Passenger>();
    public bool IsLoggedIn { set; get; } = false;
    private Passenger CurrentPassenger { set; get; } = new Passenger();


    public AccountStatus AddPassenger(string name, string password)
    {
        bool alreadyRegistered = passengers.SingleOrDefault(passenger => passenger.Name == name) != null ? true : false;
        if (!alreadyRegistered)
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
                return AccountStatus.ValidationError;
                ;
            }
            passengers.Add(passenger);
            return AccountStatus.Success;
        }
        return AccountStatus.AlreadyRegistered;
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

    public FlightState BookFlight(Flight flightToBook)
    {
        Flight alreadyBooked = CurrentPassenger.Flights.SingleOrDefault(flight =>
        (flight.DepartureDate == flightToBook.DepartureDate) &&
        (flight.DepartureCountry == flightToBook.DepartureCountry) &&
        (flight.DepartureAirport == flightToBook.DepartureAirport) &&
        (flight.ArrivalAirport == flightToBook.ArrivalAirport));
        if (alreadyBooked == null)
            CurrentPassenger.Flights.Add(flightToBook);
        return alreadyBooked == null ? FlightState.Success : FlightState.AlreadyBooked;

    }
    public List<Flight> PassengerBookings()
    {
        return CurrentPassenger.Flights;
    }
    public List<Passenger> ReturnPassengers()
    {
        return passengers;
    }

    public Passenger ReturnPassengerByName(string? name)
    {
        return passengers.SingleOrDefault(passenger => passenger.Name == name);
    }

}
