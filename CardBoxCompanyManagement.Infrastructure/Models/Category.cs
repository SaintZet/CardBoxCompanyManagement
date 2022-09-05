using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBoxCompanyManagement.Infrastructure;

public class Category
{
    public Category(int number, string name)
    {
        Number = number;
        Name = name;
    }

    public string Name { get; set; } = string.Empty;
    public int Number { get; set; }
}