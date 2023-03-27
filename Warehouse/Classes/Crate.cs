using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseProject.Classes
{
    /// <summary>
    /// Small class to make a crate, with an ID and Price, which is randomized.
    /// </summary>
    public class Crate
    {
        public string Id { get; set; }
        public double Price { get; set; } = new Random().Next(50, 501);
    }
}
