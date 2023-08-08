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
        ;
    }

    public void ShowAllAvaliableFlights()
    {
        flightController.ShowAllAvaliableFlights();
    }
    public void InitUI()
    {
        Console.ResetColor();
        Console.Clear();
    }


}
