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
    private string? Username { set; get; } = string.Empty;

    public bool ShowMainScreen()
    {
        string? choice;
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
            choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    if (IsLoggedIn) ShowAllAvaliableFlights();
                    else Login();
                    break;
                case "2":
                    if (IsLoggedIn) ShowAllBookings();
                    else Register();
                    break;
                case "3":
                    Logout();
                    return false;
                default:
                    Console.Write("Invalid choice. Please choose a valid option: ");
                    break;
            }
        } while (choice != "3");
        return false;
    }
    public void Register()
    {
        InitUI();
        Console.WriteLine("Account Register");
        Console.WriteLine("===============================================");
        Console.WriteLine("Please provide your username and password for authentication.");
        Console.Write("Username: ");
        string? username = Console.ReadLine();
        Console.Write("Password: ");
        string? password = Console.ReadLine();
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
        string? username = Console.ReadLine();
        Console.Write("Password: ");
        string? password = Console.ReadLine();
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
            string? choice;
            do
            {
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "0":
                        return;
                    default:
                        Console.Write("Invalid choice. Please choose a valid option: ");
                        break;
                }
            } while (choice != "0");

        }
        else
        {
            Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15} {4,-15} {5,-15} {6,-10}", "Flight #", "Price", "Departure Date", "Dep. Country", "Dep. Airport", "Arr. Airport", "Flight Class");
            Console.WriteLine(new string('-', 95));
            int tempIndex = 1;

            foreach (var flight in flights)
            {
                Console.Write("{0,-10}", tempIndex++);
                Console.WriteLine(flight);
            }
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Book a flight");
            Console.WriteLine("2. Search for a flight");
            Console.WriteLine("3. Back");
            Console.Write("Enter your choice (1, 2, or 3): ");
            string? choice;
            do
            {
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        BookFlight();
                        break;
                    case "2":
                        SearchForFlight();
                        break;
                    case "3":
                        return;
                    default:
                        Console.Write("Invalid choice. Please choose a valid option: ");
                        break;
                }
            } while (choice != "3");
        }
    }
    public void BookFlight()
    {
        Console.Write("Please enter the number of flight: ");
        int flightNumberIn;
        while (true)
        {
            try
            {
                flightNumberIn = Convert.ToInt32(Console.ReadLine());
                Flight flight = flightController.FindFlight(flightNumberIn);
                Console.Write("Would you like to upgrade the flight class to Business (price + $50) or First Class (price + $100)? Please respond with 'yes' or 'no': ");
                string? choice;
                do
                {
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "yes":
                            UpgradeFlightClass(flight);
                            return;
                        case "no":
                            FlightState bookState = passengerController.BookFlight(flight);
                            Console.Write(bookState == FlightState.Success ? "The flight has been booked successfully. Please enter '3' to return: " : "The flight is already booked. You can make changes in the booked flights screen. Please enter '3' to return: ");
                            return;
                        default:
                            Console.Write("Invalid choice. Please choose a valid option: ");
                            break;
                    }
                } while (choice != "no");
                break;
            }
            catch
            {
                Console.Write("PLeas enter a vaild number: ");

            }
        }
    }

    public void UpgradeFlightClass(Flight flight)
    {
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. Upgrade to first class");
        Console.WriteLine("2. Upgrade to business");
        Console.WriteLine("3. Exit");
        Console.Write("Enter your choice (1, 2, or 3): ");
        Flight flightCopy = flight.DeepCopy();
        string? choice;
        do
        {
            choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    flightCopy.TripClass = TripClass.FirstClass;
                    flightCopy.Price += 100;
                    break;
                case "2":
                    flightCopy.TripClass = TripClass.Business;
                    flightCopy.Price += 50;

                    break;
                case "3":
                    break;
                default:
                    Console.Write("Invalid choice. Please choose a valid option: ");
                    break;
            }
        } while (choice != "1" && choice != "2" && choice != "3");
        FlightState bookState = passengerController.BookFlight(flightCopy);
        Console.Write(bookState == FlightState.Success ? "The flight has been booked successfully. Please enter '3' to return: " : "The flight is already booked. You can make changes in the booked flights screen. Please enter '3' to return: ");
    }

    public void SearchForFlight()
    {
        Console.WriteLine("You have chosen to search for a flight.");
        Console.WriteLine("You can search using any of the following parameters (leave it blank if you don't wish to use it):");

        Console.Write("Enter the desired price: ");
        string? priceInput = Console.ReadLine();
        int? price = !string.IsNullOrEmpty(priceInput) && int.TryParse(priceInput, out int parsedPrice) ? parsedPrice : (int?)null;

        Console.Write("Enter the preferred departure date: ");
        string? departureDateInput = Console.ReadLine();
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
        string? flightClassInput = Console.ReadLine();
        TripClass? flightClass = !string.IsNullOrEmpty(flightClassInput) && Enum.TryParse(flightClassInput, out TripClass parsedFlightClass) ? parsedFlightClass : (TripClass?)null;

        List<Flight>? filteredFlights = flightController.FiltterFlightsByParameters(price, departureDate, departureCountry, departureAirport, arrivalAirport, flightClass);
        if (filteredFlights != null)
        {
            foreach (var flight in filteredFlights)
            {
                Console.WriteLine(flight);
            }
        }
        Console.Write(filteredFlights?.Count != 0 ? "The flights were found successfully. Please press '3' to return: " : "The flights could not be found. Please try again or press '3' to return: ");

    }
    public void ShowAllBookings()
    {
        InitUI();
        List<Flight>? bookedFlights = passengerController.PassengerBookings();
        Console.WriteLine("Booked flights");
        Console.WriteLine("===============================================");
        if (bookedFlights != null)
        {
            if (bookedFlights.Count == 0)
            {
                Console.WriteLine("No available flights found.");
                Console.Write("Enter '0' to return: ");
                string? choice;
                do
                {
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "0":
                            return;
                        default:
                            Console.Write("Invalid choice. Please choose a valid option: ");
                            break;
                    }
                } while (choice != "0");
            }
            else
            {
                Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15} {4,-15} {5,-15} {6,-10}", "Flight #", "Price", "Departure Date", "Dep. Country", "Dep. Airport", "Arr. Airport", "Flight Class");
                Console.WriteLine(new string('-', 95));
                int tempIndex = 1;
                foreach (var flight in bookedFlights)
                {
                    Console.Write("{0,-10}", tempIndex++);
                    Console.WriteLine(flight);
                }
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Cancel the flight");
                Console.WriteLine("2. Modify the flight");
                Console.WriteLine("3. Back");
                Console.Write("Enter your choice (1, 2, or 3): ");
                string? choice;
                do
                {
                    choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Cancel();
                            break;
                        case "2":
                            Modify();
                            break;
                        case "3":
                            return;
                        default:
                            Console.Write("Invalid choice. Please choose a valid option: ");
                            break;
                    }
                } while (choice != "3");
            }
        }
    }

    public void Cancel()
    {
        Console.Write("Please enter the flight number to cancel it: ");
        int flightNum;
        while (true)
        {
            try
            {
                flightNum = Convert.ToInt32(Console.ReadLine());
                List<Flight>? bookedFlights = passengerController.PassengerBookings();
                bool deleted = flightController.CancelFlight(bookedFlights, flightNum);
                Console.Write(deleted ? "The flight has been successfully canceled. Please press '3' to return: " : "The flight could not be canceled. Please try again or press '3' to return: ");
                break;
            }
            catch
            {
                Console.Write("PLeas enter a vaild number: ");
            }
        };

    }
    public void Modify()
    {
        Console.Write("Please enter the flight number to modify it: ");
        int flightNum;
        while (true)
        {
            try
            {
                flightNum = Convert.ToInt32(Console.ReadLine());
                List<Flight>? bookedFlights = passengerController.PassengerBookings();
                Flight? flightToModify = bookedFlights?.ElementAt(--flightNum!);
                Console.WriteLine("Please select an option:");
                int selectedChoice = 0;
                switch (flightToModify?.TripClass)
                {
                    case TripClass.Economy:
                        selectedChoice = 0;
                        Console.WriteLine("1. Upgrade to first class");
                        Console.WriteLine("2. Upgrade to business class");
                        break;
                    case TripClass.Business:
                        selectedChoice = 2;
                        Console.WriteLine("1. Upgrade to first class");
                        Console.WriteLine("2. Downgrade to economy class");
                        break;
                    case TripClass.FirstClass:
                        selectedChoice = 4;
                        Console.WriteLine("1. Downgrade to business class");
                        Console.WriteLine("2. Downgrade to economy class");
                        break;
                }
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice (1, 2, or 3): ");
                int choice;
                while (true)
                {
                    try
                    {
                        choice = Convert.ToInt32(Console.ReadLine());
                        selectedChoice += choice;
                        switch (selectedChoice)
                        {
                            case 1:
                            case 3:
                                flightController.ChangeClass(flightToModify, TripClass.FirstClass);
                                break;
                            case 2:
                            case 5:
                                flightController.ChangeClass(flightToModify, TripClass.Business);
                                break;
                            case 4:
                                flightController.ChangeClass(flightToModify, TripClass.Economy);
                                break;
                            default:
                                Console.Write("Invalid choice. Please choose a valid option: ");
                                break;
                        }
                        Console.Write("The flight has been successfully updated. Please press '3' to return: ");
                        break;
                    }
                    catch
                    {
                        Console.Write("PLeas enter a vaild number: ");
                    }
                };
                break;
            }
            catch
            {
                Console.Write("PLeas enter a vaild number: ");
            }
        }
    }

    public void InitUI()
    {
        Console.ResetColor();
        Console.Clear();
    }


}
