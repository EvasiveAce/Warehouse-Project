using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Classes
{
    public class Truck
    {
        string Driver { get; set; }
        string DeliveryCompany { get; set; }
        Stack<Crate> Trailer { get; set; }

        public void Load(Crate crate)
        {
            Trailer.Push(crate);
        }

        public Crate Unload()
        {
            return Trailer.Pop();
        }
    }
}
