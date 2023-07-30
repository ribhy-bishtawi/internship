namespace C__SIMS;

class Inventory
{
    private static List<Product> products = new();

    private static void initUI()
    {
        Console.ResetColor();
        Console.Clear();
    }

    internal static void ShowMenu()
    {
        initUI();
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

        switch (chosenOption)
        {
            case "1":
                AddProduct();
                break;
            case "2":
                ViewAllProducts();
                break;
            default:
                Console.WriteLine("Invalid. Please try again.");
                break;
        }
    }

    private static void AddProduct()
    {
        initUI();
        int quantity,
            price;
        bool parseSuccess;
        string? userSelection,
            userName;
        do
        {
            Console.WriteLine("****Add a product****");
            Console.Write("Enter the product name: ");
            userName = Console.ReadLine();
            Console.Write("Enter the product price: ");
            parseSuccess = int.TryParse(Console.ReadLine(), out int productPrice);
            price = parseSuccess ? productPrice : 0;
            Console.Write("Enter the product quantity: ");
            parseSuccess = int.TryParse(Console.ReadLine(), out int productQuantity);
            quantity = parseSuccess ? productQuantity : 0;
            products.Add(new Product(userName, price, quantity));
            Console.Write(
                "The product added successfully.....\nPress 1 to continue or 0 to exit to the main menu: "
            );
            userSelection = Console.ReadLine();
        } while (userSelection != "0");
        ShowMenu();
    }

    private static void ViewAllProducts()
    {
        string? userInput;
        initUI();
        int index = 1;
        foreach (var product in products)
        {
            Console.Write($"{index++}: ");
            Console.WriteLine(product.ViewProductDetail());
        }
        Console.Write("Press 0 to exit to the main menu: ");
        userInput = Console.ReadLine();
        do
        {
            Console.Write("PLEASE ENTER A VAILD INPUT!!!!: ");
            userInput = Console.ReadLine();
        } while (userInput != "0");
        ShowMenu();

    }
}
