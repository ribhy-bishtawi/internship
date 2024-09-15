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
            Console.WriteLine(IsLoggedIn ? $"Welcome, {Username}!" : "Access Denied: Authentication Required");
            Console.WriteLine("===============================================");
            Console.WriteLine("Please choose an action from the following options:");
            Console.WriteLine(IsLoggedIn ? "1. Show all avaliable flights." : "1. Login.");
            Console.WriteLine(IsLoggedIn ? "2. Show all my bookings." : "2. Register a new account");
            Console.WriteLine(IsLoggedIn ? "3. Logout." : "3. Return to the main menu.");
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
        Console.WriteLine("Account Registration");
        Console.WriteLine("===============================================");
        Console.WriteLine("Please provide your username and password for authentication.");
        Console.Write("Username: ");
        string? username = Console.ReadLine();

        Console.Write("Password: ");
        string? password = Console.ReadLine();

        AccountStatus accountStatus = passengerController.AddPassenger(username, password);
        if (accountStatus == AccountStatus.Success)
        {
            Console.WriteLine("Account has been successfully created.");
        }
        else if (accountStatus == AccountStatus.ValidationError)
        {
            Console.WriteLine("Please review your username and password and try again.");
        }
        else if (accountStatus == AccountStatus.AlreadyRegistered)
        {
            Console.WriteLine("Username is already in use. Please choose another one.");
        }
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
        Console.WriteLine("Available Flights");
        Console.WriteLine("===============================================");

        List<Flight> flights = flightController.ShowAllAvaliableFlights();

        if (flights.Count == 0)
        {
            Console.WriteLine("No available flights found.");
            WaitForUserInput("Press any key to continue.... ");
            return;
        }

        PrintFlightTableHeader();
        PrintFlightList(flights);

        ShowFlightOptions();



    }
    private void PrintFlightTableHeader()
    {
        Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15} {4,-15} {5,-15} {6,-10}", "Flight #", "Price", "Departure Date", "Dep. Country", "Dep. Airport", "Arr. Airport", "Flight Class");
        Console.WriteLine(new string('-', 95));
    }
    private void PrintFlightList(List<Flight> flights)
    {
        int tempIndex = 1;
        foreach (var flight in flights)
        {
            Console.Write("{0,-10}", tempIndex++);
            Console.WriteLine(flight);
        }
    }
    private void ShowFlightOptions()
    {
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
    private void WaitForUserInput(string prompt)
    {
        Console.Write(prompt);
        Console.ReadLine();
    }


    public void BookFlight()
    {
        Console.Write("Please enter the number of the flight: ");
        int flightNumber;

        while (!int.TryParse(Console.ReadLine(), out flightNumber))
        {
            Console.Write("Please enter a valid number: ");
        }

        Flight flight = flightController.FindFlight(flightNumber);

        if (flight == null)
        {
            Console.WriteLine("Flight not found.");
            WaitForUserInput("Press Enter to return...");
            return;
        }

        HandleUpgradeOrBooking(flight);
    }

    private void HandleUpgradeOrBooking(Flight flight)
    {
        Flight flightCopy = flight.DeepCopy();
        Console.Write("Would you like to upgrade the flight class to Business or First Class? Please respond with 'yes' or 'no': ");
        string? choice;
        do
        {
            choice = GetUserInput();

            switch (choice)
            {
                case "yes":
                    UpgradeFlightClass(flightCopy);
                    return;
                case "no":
                    FlightState bookState = passengerController.BookFlight(flightCopy);
                    Console.Write(bookState == FlightState.Success
                        ? "The flight has been booked successfully. Please enter '3' to return: "
                        : "The flight is already booked. You can make changes in the booked flights screen. Please enter '3' to return: ");
                    return;
                default:
                    Console.Write("Invalid choice. Please choose a valid option: ");
                    break;
            }
        } while (choice != "no");
    }

    private string GetUserInput()
    {
        return Console.ReadLine()?.ToLower() ?? string.Empty;
    }

    public void UpgradeFlightClass(Flight flight)
    {
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. Upgrade to First Class(+100)");
        Console.WriteLine("2. Upgrade to Business Class(+50)");
        Console.Write("Enter your choice (1, or 2): ");
        string? choice;

        Dictionary<string, Action> optionActions = new Dictionary<string, Action>
    {
        { "1", () => UpgradeToClass(flight, TripClass.FirstClass, 100) },
        { "2", () => UpgradeToClass(flight, TripClass.Business, 50) },
    };

        while (true)
        {
            choice = GetUserChoice();
            if (optionActions.TryGetValue(choice, out Action? action))
            {
                action.Invoke();
                break;
            }
            else
            {
                Console.Write("Invalid choice. Please choose a valid option: ");
            }
        }
        FlightState bookState = passengerController.BookFlight(flight);
        Console.Write(bookState == FlightState.Success
            ? "The flight has been booked successfully. Please enter '3' to return: "
            : "The flight is already booked. You can make changes in the booked flights screen. Please enter '3' to return: ");
    }

    private void UpgradeToClass(Flight flight, TripClass newClass, int priceIncrease)
    {
        flight.TripClass = newClass;
        flight.Price += priceIncrease;
    }

    private string GetUserChoice()
    {
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }
    public void SearchForFlight()
    {
        Console.WriteLine("You can search using any of the following parameters (leave it blank if you don't wish to use it):");

        Flight searchFlight = CreateSearchFlight();


        List<Flight>? filteredFlights;
        filteredFlights = GetFilteredFlightsForPassenger(searchFlight);

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



    private List<Flight>? GetFilteredFlightsForPassenger(Flight searchFlight)
    {
        List<Flight>? filteredFlights = flightController.FiltterFlightsByParameters(searchFlight);

        if (filteredFlights?.Count > 0)
        {
            PrintFlights(filteredFlights);
            return filteredFlights;
        }

        return null;
    }

    private void PrintFilteredFlights(List<Flight>? filteredFlights)
    {
        if (filteredFlights?.Count > 0)
        {
            Console.Write("The flights were found successfully. Please enter '3' to return: ");
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
    private void PrintFlights(List<Flight> flights)
    {
        foreach (var flight in flights)
        {
            Console.WriteLine("\t" + flight);
        }
    }
    private void WaitForUser(string prompt)
    {
        Console.Write(prompt);
        Console.ReadKey();
    }

    public void ShowAllBookings()
    {
        InitUI();
        List<Flight>? bookedFlights = passengerController.PassengerBookings();
        Console.WriteLine("Booked flights");
        Console.WriteLine("===============================================");

        if (bookedFlights?.Count > 0)
        {
            DisplayFlightList(bookedFlights);

            Dictionary<string, Action> optionActions = new Dictionary<string, Action>
        {
            { "1", Cancel },
            { "2", Modify },
            { "3", () => {} } // Empty delegate to exit the loop
        };
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Cancel the flight");
            Console.WriteLine("2. Modify the flight");
            Console.WriteLine("3. Back");
            Console.Write("Enter your choice (1, 2, or 3): ");
            string choice = GetUserChoiceWithValidation(optionActions);
            optionActions[choice]?.Invoke();
        }
        else
        {
            WaitForUser("No available flights found.");
        }
    }
    private void DisplayFlightList(List<Flight> flights)
    {
        Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15} {4,-15} {5,-15} {6,-10}", "Flight #", "Price", "Departure Date", "Dep. Country", "Dep. Airport", "Arr. Airport", "Flight Class");
        Console.WriteLine(new string('-', 95));
        int tempIndex = 1;
        foreach (var flight in flights)
        {
            Console.Write("{0,-10}", tempIndex++);
            Console.WriteLine(flight);
        }
    }
    private string GetUserChoiceWithValidation(Dictionary<string, Action> optionActions)
    {
        while (true)
        {
            string? choice = GetUserChoice();
            if (optionActions.ContainsKey(choice))
            {
                return choice;
            }
            else
            {
                Console.Write("Invalid choice. Please choose a valid option: ");
            }
        }
    }

    public void Cancel()
    {
        Console.Write("Please enter the flight number to cancel it: ");
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int flightNum))
            {
                List<Flight>? bookedFlights = passengerController.PassengerBookings();
                bool deleted = flightController.CancelFlight(bookedFlights, flightNum);

                if (deleted)
                {
                    WaitForUser("The flight has been successfully canceled. Please press any key to continue....");
                    break;
                }
                else
                {
                    Console.Write("The flight could not be canceled. Please try again or press '3' to return: ");
                }
            }
            else
            {
                Console.Write("Please enter a valid number: ");
            }
        }
    }

    public void Modify()
    {
        int flightNum = GetValidFlightNumber();
        if (flightNum == -1) return;

        List<Flight>? bookedFlights = passengerController.PassengerBookings();
        Flight? flightToModify = bookedFlights?.ElementAt(--flightNum);
        if (flightToModify == null) return;

        int selectedChoice = GetSelectedChoice(flightToModify);
        if (selectedChoice == -1) return;

        PerformModification(flightToModify, selectedChoice);
        WaitForUser("The flight has been successfully updated. Please press any key to continue....");
    }

    private int GetValidFlightNumber()
    {
        Console.Write("Please enter the flight number to modify it: ");
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int flightNum))
                return flightNum;
            else
                Console.Write("Please enter a valid number: ");
        }
    }

    private int GetSelectedChoice(Flight flightToModify)
    {
        Console.WriteLine("Please select an option:");
        int selectedChoice = 0;

        switch (flightToModify.TripClass)
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
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int choice))
                return selectedChoice + choice;
            else
                Console.Write("Please enter a valid number: ");
        }
    }

    private void PerformModification(Flight flightToModify, int selectedChoice)
    {
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
            case 6:
                flightController.ChangeClass(flightToModify, TripClass.Economy);
                break;
            default:
                Console.Write("Invalid choice. Please choose a valid option: ");
                break;
        }
    }
    public void InitUI()
    {
        Console.ResetColor();
        Console.Clear();
    }


}
