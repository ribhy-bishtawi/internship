namespace C__SIMS;

class Product
{
    public Product(string? name, int price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }

    public string? Name { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }

    public string ViewProductDetail()
    {
        return $"Product Name: {Name}, Price: {Price:C}, Quantity:{Quantity}";
    }

}
