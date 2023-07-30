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
            case "3":
                EditProduct();
                break;
            default:
                Console.WriteLine("Invalid. Please try again.");
                break;
        }
    }

    private static void ReturnToMainMenu()
    {
        string? userInput;
        Console.Write("Press 0 to exit to the main menu: ");
        do
        {
            userInput = Console.ReadLine();
            Console.Write("PLEASE ENTER A VAILD INPUT!!!!: ");
        } while (userInput != "0");
        ShowMenu();

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
        initUI();
        Console.WriteLine("****View all products****");

        int index = 1;
        foreach (var product in products)
        {
            Console.Write($"{index++}: ");
            Console.WriteLine(product.ViewProductDetail());
        }
        ReturnToMainMenu();
    }

    private static void EditProduct()
    {
        initUI();
        string? nameIO;
        Console.WriteLine("****Edit a product****");
        int index = 1;
        foreach (var product in products)
        {
            Console.Write($"{index++}: ");
            Console.WriteLine(product.Name);
        }
        Console.Write("PLease write the product's name");
        nameIO = Console.ReadLine();
        Product? productToEdit = products.Where(p => p.Name == nameIO).FirstOrDefault();
        if (productToEdit != null)
        {
            Console.WriteLine("Please choose the number of what you want to change. (You can choose multiple choices, just separate them with commas.):");
            Console.Write("1: Name\n2: Price\n3: Quantity\n: ");
            string? optionsIO = Console.ReadLine();
            List<string> itemsList = new List<string>(optionsIO?.Split(',') ?? Array.Empty<string>());
            do
            {
                switch (itemsList[0])
                {
                    case "1":
                        Console.Write("Please enter the new name: ");
                        string? name = Console.ReadLine();
                        productToEdit.Name = name;
                        break;
                    case "2":
                        Console.Write("Please enter the new price: ");
                        int price = int.Parse(Console.ReadLine() ?? "0");
                        productToEdit.Price = price;
                        break;
                    case "3":
                        Console.Write("Please enter the new quantity: ");
                        int quantity = int.Parse(Console.ReadLine() ?? "0");
                        productToEdit.Quantity = quantity;
                        break;
                }
                itemsList.RemoveAt(0);
            } while (itemsList.Count != 0);
            Console.WriteLine("The product changed successfully.....\n");
        }
        else
        {
            Console.WriteLine("The product could bot be found please check the name ant try again.");
        }
        ReturnToMainMenu();

    }
}
