using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseProject.Classes
{
    public class Crate
    {
        public string Id { get; set; }
        public double Price { get; set; } = new Random().Next(50, 501);
    }
}
