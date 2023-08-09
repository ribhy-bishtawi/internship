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
        int choice;
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
            choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    ShowBookedFlights();
                    break;
                case 2:
                    AddFlights();
                    break;
                case 3:
                    return false;
                default:
                    Console.Write("Invalid choice. Please choose a valid option: ");
                    break;
            }
        } while (choice != 3);
        return false;
    }
    public void AddFlights()
    {
        bool temp = false;
        string choice;
        InitUI();
        Console.WriteLine("Import Flights from a CSV File");
        Console.WriteLine("===================================");
        Console.Write("Please enter the file path to import flights from a CSV file,");
        Console.Write("or enter '0' to exit: ");
        do
        {
            choice = Console.ReadLine();

            switch (choice)
            {
                case "0":
                    return;
                    break;
                default:
                    temp = flightController.AddFlightsFromCsvFile(choice);
                    if (temp) return;
                    else Console.Write("Please try again or enter '0' to return to the main menu: ");
                    break;
            }

        } while (choice != "0");
    }
    public void ShowBookedFlights()
    {
        InitUI();
        Console.WriteLine("Booked Flights");
        Console.WriteLine("===================================");
        List<Passenger> passengers = passengerController.ReturnPassengers();
        bool printed = false;
        foreach (var passenger in passengers)
        {
            if (passenger.Flights.Count > 0)
                Console.WriteLine($"{passenger.Name}:");
            foreach (var flight in passenger.Flights)
            {
                printed = true;
                Console.WriteLine("\t" + flight);
            }
        }
        if (!printed)
            Console.WriteLine("No booked flights yet.");
        else
        {
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Search for a flight");
            Console.WriteLine("2. Back");
            Console.Write("Enter your choice (1, or 2): ");
            int choice;
            do
            {
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        SearchForFlight();
                        break;
                    case 2:
                        return;
                    default:
                        Console.Write("Invalid choice. Please choose a valid option: ");
                        break;
                }
            } while (choice != 2);

        }
        Console.Write("Press any key to continue.....");
        Console.ReadLine();
    }

    public void SearchForFlight()
    {
        Console.WriteLine("You can search using any of the following parameters (leave it blank if you don't wish to use it):");

        Console.Write("Enter the passenger name: ");
        string? passengerName = Console.ReadLine();
        passengerName = !string.IsNullOrEmpty(passengerName) ? passengerName : null;
        Passenger passenger = passengerController.ReturnPassengerByName(passengerName);


        Console.Write("Enter the desired price: ");
        string priceInput = Console.ReadLine();
        int? price = !string.IsNullOrEmpty(priceInput) && int.TryParse(priceInput, out int parsedPrice) ? parsedPrice : (int?)null;

        Console.Write("Enter the preferred departure date: ");
        string departureDateInput = Console.ReadLine();
        DateTime? departureDate = !string.IsNullOrEmpty(departureDateInput) && DateTime.TryParse(departureDateInput, out DateTime parsedDepartureDate) ? parsedDepartureDate : (DateTime?)null;

        Console.Write("Enter the departure country: ");
        string? departureCountry = Console.ReadLine();
        departureCountry = !string.IsNullOrEmpty(departureCountry) ? departureCountry : null;

        Console.Write("Enter the departure airport: ");
        string? departureAirport = Console.ReadLine();
        departureAirport = !string.IsNullOrEmpty(departureAirport) ? departureAirport : null;


        Console.Write("Enter the arrival airport: ");
        string? arrivalAirport = Console.ReadLine();
        arrivalAirport = !string.IsNullOrEmpty(arrivalAirport) ? arrivalAirport : null;


        Console.Write("Enter the preferred flight class: ");
        string flightClassInput = Console.ReadLine();
        TripClass? flightClass = !string.IsNullOrEmpty(flightClassInput) && Enum.TryParse(flightClassInput, out TripClass parsedFlightClass) ? parsedFlightClass : (TripClass?)null;

        List<Flight> filteredFlights = flightController.FiltterFlightsByParameters(price, departureDate, departureCountry, departureAirport, arrivalAirport, flightClass, passenger);
        foreach (var flight in filteredFlights)
        {
            Console.WriteLine(flight);
        }
        Console.Write(filteredFlights.Count != 0 ? "The flights were found successfully. Please enter '2' to return: " : "The flights could not be found. Please try again or enter '2' to return: ");

    }
    public void InitUI()
    {
        Console.ResetColor();
        Console.Clear();
    }


}
