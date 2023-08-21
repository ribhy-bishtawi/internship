using AirportTrackingSystem.Models;
using AirportTrackingSystem.Controllers;
using AirportTrackingSystem.Enums;

namespace AirportTrackingSystem.Views;

public class ManagerView
{
    private FlightController flightController;
    private PassengerController passengerController;
    private PassengerView passengerView;

    public ManagerView(PassengerController passengerController, FlightController flightController)
    {
        this.passengerController = passengerController;
        this.flightController = flightController;
        this.passengerView = new PassengerView(passengerController, flightController);

    }


    public bool ShowMainScreen()
    {
        string? choice;
        do
        {
            InitUI();
            Console.WriteLine("Logged in as manager.");
            Console.WriteLine("===============================================");
            Console.WriteLine("Please select an action from the following options:");
            Console.WriteLine("1. Display a list of all currently booked flights");
            Console.WriteLine("2. Import flights from a CSV file");
            Console.WriteLine("3. Logout");
            Console.Write("Please enter the number corresponding to your desired action (1, 2, or 3): ");
            choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ShowBookedFlights();
                    break;
                case "2":
                    AddFlights();
                    break;
                case "3":
                    return false;
                default:
                    Console.Write("Invalid choice. Please choose a valid option: ");
                    break;
            }
        } while (choice != "3");
        return false;
    }
    public void AddFlights()
    {
        bool addedSuccessfully = false;
        string? filePath;

        InitUI();
        Console.WriteLine("Import Flights from a CSV File");
        Console.WriteLine("===================================");

        do
        {
            Console.Write("Please enter the file path to import flights from a CSV file,");
            Console.Write("or enter '0' to exit: ");
            filePath = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("File path cannot be empty. Please try again.");
                continue;
            }

            if (filePath == "0")
            {
                return;
            }

            addedSuccessfully = flightController.AddFlightsFromCsvFile(filePath);
            if (addedSuccessfully)
            {
                return;
            }
        } while (filePath != "0");
    }

    public void ShowBookedFlights()
    {
        InitUI();
        Console.WriteLine("Booked Flights");
        Console.WriteLine("===================================");

        List<Passenger>? passengers = passengerController.ReturnPassengers();

        if (passengers?.Count == 0)
        {
            Console.WriteLine("No signed up passengers yet.");
            WaitForUser();
            return;
        }

        bool printed = false;
        foreach (var passenger in passengers!)
        {
            if (passenger.Flights?.Count > 0)
            {
                Console.WriteLine($"{passenger.Name}:");
                PrintFlights(passenger.Flights);
                printed = true;
            }
        }

        if (printed)
        {
            ShowOptions();
        }
        else
        {
            Console.WriteLine("No booked flights yet.");
        }
    }

    private void PrintFlights(List<Flight> flights)
    {
        foreach (var flight in flights)
        {
            Console.WriteLine("\t" + flight);
        }
    }

    private void ShowOptions()
    {
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. Search for a flight");
        Console.WriteLine("2. Back");
        Console.Write("Enter your choice (1, or 2): ");

        string? choice;
        do
        {
            choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    SearchForFlight();
                    break;
                case "2":
                    return;
                default:
                    Console.Write("Invalid choice. Please choose a valid option: ");
                    break;
            }
        } while (choice != "2");
    }

    private void WaitForUser()
    {
        Console.Write("Press any key to exit...");
        Console.ReadKey();
    }

    public void SearchForFlight()
    {
        Console.WriteLine("You can search using any of the following parameters (leave it blank if you don't wish to use it):");

        string? passengerName = GetUserInput("Enter the passenger name: ");
        Passenger? passengerPassed = passengerController.ReturnPassengerByName(passengerName);

        Flight searchFlight = CreateSearchFlight();


        List<Flight>? filteredFlights;
        List<Passenger>? passengers = passengerController.ReturnPassengers();

        if (passengers?.Count == 0)
        {
            Console.Write("No signed up passengers");
            WaitForUser();
            return;
        }

        if (passengerPassed == null)
        {
            filteredFlights = GetFilteredFlightsForAllPassengers(searchFlight);
        }
        else
        {
            filteredFlights = GetFilteredFlightsForPassenger(passengerPassed, searchFlight);
        }

        PrintFilteredFlights(filteredFlights);

    }
    private Flight CreateSearchFlight()
    {
        int? price = GetNullableIntInput("Enter the desired price: ");
        DateTime? departureDate = GetNullableDateTimeInput("Enter the preferred departure date: ");
        string? departureCountry = GetUserInput("Enter the departure country: ");
        string? departureAirport = GetUserInput("Enter the departure airport: ");
        string? arrivalAirport = GetUserInput("Enter the arrival airport: ");
        TripClass? flightClass = GetNullableEnumInput<TripClass>("Enter the preferred flight class: ");

        return new Flight
        {
            Price = price,
            DepartureDate = departureDate,
            DepartureCountry = departureCountry,
            DepartureAirport = departureAirport,
            ArrivalAirport = arrivalAirport,
            TripClass = flightClass
        };
    }

    private List<Flight>? GetFilteredFlightsForAllPassengers(Flight searchFlight)
    {
        List<Passenger>? passengers = passengerController.ReturnPassengers();
        if (passengers == null)
        {
            return null;
        }

        List<Flight>? allFilteredFlights = new List<Flight>();

        foreach (var passenger in passengers)

        {
            List<Flight>? passengerFilteredFlights = GetFilteredFlightsForPassenger(passenger, searchFlight);
            if (passengerFilteredFlights != null)
            {
                allFilteredFlights.AddRange(passengerFilteredFlights);
            }
        }

        return allFilteredFlights;
    }

    private List<Flight>? GetFilteredFlightsForPassenger(Passenger passenger, Flight searchFlight)
    {
        List<Flight>? filteredFlights = flightController.FiltterFlightsByParameters(searchFlight, passenger);

        if (filteredFlights?.Count > 0)
        {
            Console.WriteLine($"{passenger.Name}:");
            PrintFlights(filteredFlights);
            return filteredFlights;
        }

        return null;
    }

    private void PrintFilteredFlights(List<Flight>? filteredFlights)
    {
        if (filteredFlights?.Count > 0)
        {
            Console.Write("The flights were found successfully. Please enter '2' to return: ");
        }
        else
        {
            Console.Write("The flights could not be found. Please try again or enter '2' to return: ");
        }
    }
    private TEnum? GetNullableEnumInput<TEnum>(string prompt) where TEnum : struct
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input) && Enum.TryParse(input, out TEnum parsedValue))
        {
            return parsedValue;
        }
        return null;
    }

    private int? GetNullableIntInput(string prompt)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int parsedValue))
        {
            return parsedValue;
        }
        return null;
    }

    private DateTime? GetNullableDateTimeInput(string prompt)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input) && DateTime.TryParse(input, out DateTime parsedValue))
        {
            return parsedValue;
        }
        return null;
    }

    private string? GetUserInput(string prompt)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        return string.IsNullOrEmpty(input) ? null : input;
    }
    public void InitUI()
    {
        Console.ResetColor();
        Console.Clear();
    }


}
