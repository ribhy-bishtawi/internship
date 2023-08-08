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

    public bool ShowMainScreen()
    {
        int choice;
        do
        {
            InitUI();
            Console.WriteLine(IsLoggedIn ? $"Welcome {Username}" : "Access Denied: Authentication Required");
            Console.WriteLine("===============================================");
            Console.WriteLine("Please choose an action from the following options:");
            Console.WriteLine(IsLoggedIn ? "1. Show all avaliable flights." : "1. Login.");
            Console.WriteLine(IsLoggedIn ? "2. Show all my bookings." : "2. Register a new account");
            Console.WriteLine(IsLoggedIn ? "3. Logot." : "3. Return to the main menu.");
            Console.Write("Kindly enter your choice (1, 2, or 3): ");
            choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    if (IsLoggedIn) ShowAllAvaliableFlights();
                    else Login();
                    break;
                case 2:
                    if (IsLoggedIn) ShowAllBookings();
                    else Register();
                    break;
                case 3:
                    Logout();
                    return false;
                default:
                    Console.Write("Invalid choice. Please choose a valid option: ");
                    break;
            }
        } while (choice != 3);

        return false;

    }
    public void Register()
    {
        InitUI();
        Console.WriteLine("Account Register");
        Console.WriteLine("===============================================");
        Console.WriteLine("Please provide your username and password for authentication.");
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();
        AccountStatus accountStatus = passengerController.AddPassenger(username, password);
        Console.WriteLine(accountStatus == AccountStatus.Success ? "Account has been successfully created." : accountStatus == AccountStatus.ValidationError ? "Please review your username and password, and try again." : "Username already in used please try another one.");
        Console.Write("Press any key to continue....");
        Console.ReadLine();
    }
    public void Login()
    {
        InitUI();
        Console.WriteLine("Account Login");
        Console.WriteLine("===============================================");
        Console.WriteLine("Please provide your username and password for authentication.");
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();
        IsLoggedIn = passengerController.Login(username, password) ? true : IsLoggedIn;
        Username = IsLoggedIn ? username : Username;
        Console.WriteLine(IsLoggedIn ? "Login successful." : "The account could not be found. Please verify your username and password and try again.");
        Console.Write("Press any key to continue....");
        Console.ReadLine();
    }

    public void Logout()
    {
        InitUI();
        IsLoggedIn = passengerController.Logout() ? IsLoggedIn : false;
        Username = IsLoggedIn ? string.Empty : Username;
        return;
    }


    public void ShowAllAvaliableFlights()
    {
        InitUI();
        Console.WriteLine("Available flights");
        Console.WriteLine("===============================================");
        List<Flight> flights = flightController.ShowAllAvaliableFlights();
        if (flights.Count == 0)
        {
            Console.WriteLine("No available flights found.");
            Console.Write("Enter '0' to return: ");
            int choice;
            do
            {
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        return;
                        break;
                    default:
                        Console.Write("Invalid choice. Please choose a valid option: ");
                        break;
                }
            } while (choice != 0);

        }
        else
        {
            Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15} {4,-15} {5,-15} {6,-10}", "Flight #", "Price", "Departure Date", "Dep. Country", "Dep. Airport", "Arr. Airport", "Flight Class");
            Console.WriteLine(new string('-', 85));
            int tempIndex = 1;

            foreach (var flight in flights)
            {
                Console.WriteLine(flight);
            }
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Book a flight");
            Console.WriteLine("2. Search for a flight");
            Console.WriteLine("3. Back");
            Console.Write("Enter your choice (1, 2, or 3): ");
            int choice;
            do
            {
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        BookFlight();
                        break;
                    case 2:
                        SearchForFlight();
                        break;
                    case 3:
                        return;
                    default:
                        Console.Write("Invalid choice. Please choose a valid option: ");
                        break;
                }
            } while (choice != 3);
        }
    }
    public void BookFlight()
    {
        Console.Write("Please enter the number of flight: ");
        int flightNumberIn = Convert.ToInt32(Console.ReadLine());
        Flight flight = flightController.FindFlight(flightNumberIn);
        passengerController.BookFlight(flight);
        Console.Write("The flight has been booked successfully. Please enter '3' to return: ");
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

        List<Flight> filteredFlights = flightController.FiltterFlightsByParameters(price, departureDate, departureCountry, departureAirport, arrivalAirport, flightClass);
        foreach (var flight in filteredFlights)
        {
            Console.WriteLine(flight);
        }
        Console.Write(filteredFlights.Count != 0 ? "The flights were found successfully. Please press '3' to return: " : "The flights could not be found. Please try again or press '3' to return: ");

    }
    public void ShowAllBookings()
    {
        InitUI();
        List<Flight> bookedFlights = passengerController.PassengerBookings();
        Console.WriteLine("Booked flights");
        Console.WriteLine("===============================================");
        if (bookedFlights.Count == 0)
        {
            Console.WriteLine("No available flights found.");
            Console.Write("Enter '0' to return: ");
            int choice;
            do
            {
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        return;
                        break;
                    default:
                        Console.Write("Invalid choice. Please choose a valid option: ");
                        break;
                }
            } while (choice != 0);
        }
        else
        {
            Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15} {4,-15} {5,-15} {6,-10}", "Flight #", "Price", "Departure Date", "Dep. Country", "Dep. Airport", "Arr. Airport", "Flight Class");
            Console.WriteLine(new string('-', 85));
            int tempIndex = 1;
            foreach (var flight in bookedFlights)
            {
                Console.WriteLine(flight);
            }
            Console.WriteLine("1. Cancel the flight");
            Console.WriteLine("2. Modify the flight");
            Console.WriteLine("3. Back");
            Console.Write("Enter your choice (1, 2, or 3): ");
            int choice;
            do
            {
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Cancel();
                        break;
                    case 2:
                        // TODO
                        // Modify();
                        break;
                    case 3:
                        return;
                    default:
                        Console.Write("Invalid choice. Please choose a valid option: ");
                        break;
                }
            } while (choice != 3);
        }
    }

    public void Cancel()
    {
        Console.Write("Please enter the flight number to cancel it: ");
        int flightNum = Convert.ToInt32(Console.ReadLine());
        List<Flight> bookedFlights = passengerController.PassengerBookings();
        bool deleted = flightController.CancelFlight(bookedFlights, flightNum);
        Console.Write(deleted ? "The flight has been successfully canceled. Please press '3' to return: " : "The flight could not be canceled. Please try again or press '3' to return: ");
    }
    public void InitUI()
    {
        Console.ResetColor();
        Console.Clear();
    }


}
