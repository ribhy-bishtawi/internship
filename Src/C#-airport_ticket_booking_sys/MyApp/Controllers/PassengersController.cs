using AirportTrackingSystem.Models;
using System.ComponentModel.DataAnnotations;
using AirportTrackingSystem.Enums;
using AirportTrackingSystem.Interfaces;


namespace AirportTrackingSystem.Controllers;

public class PassengerController
{

    private readonly IFileReader fileReader;
    private readonly IFileWriter fileWriter;
    private readonly IPassengerValidator passengerValidator;
    private readonly ILogger logger;

    public PassengerController(IFileReader fileReader, IFileWriter fileWriter, IPassengerValidator passengerValidator, ILogger logger)
    {
        this.fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
        this.fileWriter = fileWriter ?? throw new ArgumentNullException(nameof(fileWriter));
        this.passengerValidator = passengerValidator ?? throw new ArgumentNullException(nameof(passengerValidator));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));


    }

    private List<Passenger> passengers = new List<Passenger>();
    public bool IsLoggedIn { set; get; } = false;
    private Passenger? CurrentPassenger { set; get; } = new Passenger();
    public bool AddPassengersFromCsvFile(string filePath)
    {
        int lineNum = 0;
        try
        {
            var lines = fileReader.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                lineNum++;
                var fields = line?.Split(',');
                if (fields != null && fields.Length >= 2)
                {
                    if (TryCreatePassengerFromFields(fields, out Passenger? passenger, lineNum))
                    {
                        passengers.Add(passenger!);
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error reading CSV file.", ex);
        }

    }

    private bool TryCreatePassengerFromFields(string[] fields, out Passenger? passenger, int lineNum)
    {
        passenger = null;
        passenger = new Passenger
        {
            Name = fields[0],
            Password = fields[1],
            Flights = new List<Flight>()
        };
        if (!passengerValidator.Validate(passenger, out var validationResults))
        {
            logger.Log($"The following validation errors were detected in the values entered on line {lineNum}:");
            foreach (var validationResult in validationResults)
            {
                throw new InvalidOperationException($"The following validation errors were detected in the values entered on line {lineNum}: {string.Join(", ", validationResults.Select(r => r.ErrorMessage))}");
            }
            return false;
        }
        return true;
    }
    public void WritePassengersToCSV(string filePath, Passenger passenger)
    {
        fileWriter.WriteToFile(filePath, $"{passenger.Name},{passenger.Password}");
    }

    public AccountStatus AddPassenger(string? name, string? password)
    {
        bool alreadyRegistered = passengers.SingleOrDefault(passenger => passenger.Name == name) != null ? true : false;
        if (!alreadyRegistered)
        {
            Passenger passenger = new Passenger { Name = name, Password = password, Flights = new List<Flight>() };
            if (!passengerValidator.Validate(passenger, out var validationResults))
            {
                foreach (var validationResult in validationResults)
                {
                    logger.Log($"{validationResult.ErrorMessage}");
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
    public bool Login(string? name, string? password)
    {
        Passenger? passenger = passengers.SingleOrDefault(passenger =>
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
        Flight? alreadyBooked = CurrentPassenger?.Flights?.SingleOrDefault(flight =>
        (flight.DepartureDate == flightToBook.DepartureDate) &&
        (flight.DepartureCountry == flightToBook.DepartureCountry) &&
        (flight.DepartureAirport == flightToBook.DepartureAirport) &&
        (flight.ArrivalAirport == flightToBook.ArrivalAirport));
        if (alreadyBooked == null)
            CurrentPassenger?.Flights?.Add(flightToBook);
        return alreadyBooked == null ? FlightState.Success : FlightState.AlreadyBooked;

    }
    public List<Flight>? PassengerBookings()
    {
        return CurrentPassenger?.Flights;
    }
    public List<Passenger>? ReturnPassengers()
    {
        return passengers;
    }

    public Passenger? ReturnPassengerByName(string? name)
    {
        return passengers.SingleOrDefault(passenger => passenger.Name == name);
    }

}
