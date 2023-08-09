using AirportTrackingSystem.Models;
using System.ComponentModel.DataAnnotations;
using AirportTrackingSystem.Enums;


namespace AirportTrackingSystem.Controllers;

public class PassengerController
{
    private List<Passenger> passengers = new List<Passenger>();
    public bool IsLoggedIn { set; get; } = false;
    private Passenger CurrentPassenger { set; get; } = new Passenger();
    public bool AddPassengersFromCsvFile(string filePath)
    {
        int tempCount = 0;
        try
        {
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    tempCount++;
                    var line = reader.ReadLine();
                    var fields = line.Split(',');

                    if (fields.Length >= 2) // Check if there are enough fields
                    {
                        var passenger = new Passenger
                        {
                            Name = fields[0],
                            Password = fields[1],
                            Flights = new List<Flight>()
                        };
                        var validationContext = new ValidationContext(passenger);
                        var validationResults = new List<ValidationResult>();
                        if (!Validator.TryValidateObject(passenger, validationContext, validationResults, validateAllProperties: true))
                        {
                            Console.WriteLine($"The following validation errors were detected in the values entered on line {tempCount}:");
                            foreach (var validationResult in validationResults)
                            {
                                Console.WriteLine($"\t{validationResult.ErrorMessage}");
                            }
                            continue;
                        }

                        passengers.Add(passenger);

                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading CSV file: " + ex.Message);
            return false;
        }

    }
    public void WritePassengersToCSV(string filePath, Passenger passenger)
    {
        using (StreamWriter writer = new StreamWriter(filePath, append: true))
        {
            writer.WriteLine($"{passenger.Name},{passenger.Password}");
        }
    }

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
            WritePassengersToCSV("Data/Passengers.csv", passenger);
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
