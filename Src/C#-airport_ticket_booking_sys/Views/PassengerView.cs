using AirportTrackingSystem.Models;
using AirportTrackingSystem.Controllers;
using AirportTrackingSystem.Enums;

namespace AirportTrackingSystem.Views;

public class PassengerView
{
    private PassengerController passengerController = new PassengerController();
    private bool IsLoggedIn { set; get; } = false;

    public void ShowMainScreen()
    {
        InitUI();
        if (IsLoggedIn)
        {

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
                    PassengerViewLogin();
                    break;
                case 0:
                    PassengerViewShowAllPassengers();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please choose a valid option.");
                    break;
            }
        }

    }
    public void PassengerViewLogin()
    {
        InitUI();
        Console.WriteLine("Please enter you username and password");
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();
        IsLoggedIn = passengerController.AddPassenger(username, password) ? true : IsLoggedIn;
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
    public void PassengerViewShowAllPassengers()
    {
        InitUI();
        passengerController.ShowPassengers();
    }


    public void InitUI()
    {
        Console.ResetColor();
        Console.Clear();
    }


}
