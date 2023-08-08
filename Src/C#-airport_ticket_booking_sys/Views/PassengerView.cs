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
            Console.WriteLine("Press 1 to view the available flights");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    ShowAllAvaliableFlights();
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
                Console.WriteLine("Please enter the number of flight: ");
                int flightNumberIn = Convert.ToInt32(Console.ReadLine());
                BookFlight(flightNumberIn);
                break;
            case 2:
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
                break;
            case 0:
                ShowAllPassengers();
                break;

            default:
                Console.WriteLine("Invalid choice. Please choose a valid option.");
                break;
        }

    }
    public void BookFlight(int flightNum)
    {
        Flight flight = flightController.FindFlight(flightNum);
        passengerController.BookFlight(Username, flight);
    }

    public void InitUI()
    {
        Console.ResetColor();
        Console.Clear();
    }


}
