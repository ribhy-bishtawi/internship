using AirportTrackingSystem.Models;
using AirportTrackingSystem.Controllers;
using AirportTrackingSystem.Enums;

namespace AirportTrackingSystem.Views;

public class PassengerView
{
    private PassengerController passengerController;
    private FlightController flightController;

    public PassengerView(PassengerController passengerController, FlightController flightController)
    {
        this.passengerController = passengerController;
        this.flightController = flightController;

    }
    private bool IsLoggedIn { set; get; } = false;
    private string Username { set; get; } = string.Empty;

    public void ShowMainScreen()
    {
        InitUI();
        if (IsLoggedIn)
        {
            Console.WriteLine($"Welcome {Username}");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Show all avaliable flights");
            Console.WriteLine("2. Show all my bookings");
            Console.Write("Enter your choice (1 or 2): ");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    ShowAllAvaliableFlights();
                    break;
                case 2:
                    ShowAllBookings();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please choose a valid option.");
                    break;
            }


        }
        else
        {
            Console.WriteLine("Press 1 to login to your account");
            Console.WriteLine("Press 0 to register if you don't have one");
            Console.Write("Enter your choice (1 or 0): ");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Login();
                    break;
                case 0:
                    ShowAllPassengers();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please choose a valid option.");
                    break;
            }
        }

    }
    public void Login()
    {
        InitUI();
        Console.WriteLine("Please enter you username and password");
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();
        IsLoggedIn = passengerController.AddPassenger(username, password) ? true : IsLoggedIn;
        Username = IsLoggedIn ? username : Username;
        ShowMainScreen();
    }
    public void PassengerViewRegister()
    {
        InitUI();
        Console.WriteLine("Please enter you username and password");
        Console.Write("Username: ");
        string userName = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();

    }
    public void ShowAllPassengers()
    {
        InitUI();
        passengerController.ShowPassengers();
    }

    public void ShowAllAvaliableFlights()
    {
        flightController.ShowAllAvaliableFlights();
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. Book a flight");
        Console.WriteLine("2. Search for a flight");
        Console.Write("Enter your choice (1 or 2): ");
        int choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1:
                BookFlight();
                break;
            case 2:
                SearchForFlight();
                break;
            case 0:
                ShowAllPassengers();
                break;

            default:
                Console.WriteLine("Invalid choice. Please choose a valid option.");
                break;
        }
        ShowMainScreen();
    }
    public void BookFlight()
    {
        Console.WriteLine("Please enter the number of flight: ");
        int flightNumberIn = Convert.ToInt32(Console.ReadLine());
        Flight flight = flightController.FindFlight(flightNumberIn);
        passengerController.BookFlight(flight);
    }

    public void SearchForFlight()
    {
        Console.WriteLine("You have chosen to search for a flight.");
        Console.WriteLine("You can search using any of the following parameters (leave it blank if you don't wish to use it):");

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

        flightController.FiltterFlightsByParameters(price, departureDate, departureCountry, departureAirport, arrivalAirport, flightClass);
    }
    public void ShowAllBookings()
    {
        List<Flight> bookedFlights = passengerController.PassengerBookings();
        Console.WriteLine("Available Flights:");
        Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15} {4,-15} {5,-15} {6,-10}", "Flight #", "Price", "Departure Date", "Dep. Country", "Dep. Airport", "Arr. Airport", "Flight Class");
        Console.WriteLine(new string('-', 85));
        int tempIndex = 1;
        foreach (var flight in bookedFlights)
        {

            Console.WriteLine("{0,-10} {1,-8:C} {2,-15} {3,-20} {4,-15} {5,-15} {6,-10} ", tempIndex++, flight.Price, flight.DepartureDate, flight.DepartureCountry, flight.DepartureAirport, flight.ArrivalAirport, flight.TripClass);
        }
        Console.WriteLine("Please select an option 1 if you want to cancel a flight or 2 to modify it:");
        Console.Write("Enter your choice (1 or 2): ");
        int choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1:
                Cancel();
                break;
            case 2:
                // Modify();
                break;
            case 0:
                ShowAllPassengers();
                break;

            default:
                Console.WriteLine("Invalid choice. Please choose a valid option.");
                break;
        }
        ShowMainScreen();



    }

    public void Cancel()
    {
        Console.WriteLine("Please enter the flight num: ");
        int flightNum = Convert.ToInt32(Console.ReadLine());
        List<Flight> bookedFlights = passengerController.PassengerBookings();
        flightController.CancelFlight(bookedFlights, flightNum);
        ShowAllBookings();
    }
    public void InitUI()
    {
        Console.ResetColor();
        Console.Clear();
    }


}
