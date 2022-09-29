namespace CardBox.ApiClient.Models;

public class Category
{
    public Category(int number, string name)
    {
        Number = number;
        Name = name;
    }

    public string Name { get; }
    public int Number { get; set; }
}