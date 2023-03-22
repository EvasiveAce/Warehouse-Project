using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Classes
{
    public class Dock
    {
        string Id { get; set; }
        Queue<Truck> Line { get; set; } = new Queue<Truck>();
        double TotalSales { get; set; }
        int TotalCrates { get; set; }
        int TotalTrucks { get; set; }
        int TimeInUse { get; set; }
        int TimeNotInUse { get; set; }

        public void JoinLine(Truck truck)
        {
            Line.Enqueue(truck);
        }

        public Truck SendOff()
        {
            return Line.Dequeue();
        }
    }
}
