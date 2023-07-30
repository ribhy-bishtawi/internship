namespace C__SIMS;

class Inventory
{
    private static List<Product> inventory = new();

    internal static void ShowMenu()
    {
        Console.ResetColor();
        Console.Clear();
        Console.WriteLine("****Simple Inventory Management System****");
        Console.WriteLine("Please Select an action to perform");
        Console.WriteLine("******************************************");
        Console.WriteLine("1:Add a product");
        Console.WriteLine("2:View all products");
        Console.WriteLine("3:Edit a product");
        Console.WriteLine("4:Delete a product");
        Console.WriteLine("5:Search for a product");
        Console.WriteLine("6:Exit");

        string? chosenOption = Console.ReadLine();
    }
}
