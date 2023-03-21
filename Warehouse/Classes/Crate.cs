using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Classes
{
    public class Crate
    {
        string Id { get; set; }
        double Price { get; set; } = new Random().Next(50, 501);
    }
}
