using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseProject.Classes
{
    public class Dock
    {
        string Id { get; set; }
        public Queue<Truck> Line { get; set; } = new Queue<Truck>();
        double TotalSales { get; set; }
        int TotalCrates { get; set; }
        int TotalTrucks { get; set; }
        int TimeInUse { get; set; }
        int TimeNotInUse { get; set; }

        //Total Trucks *  100

        public void JoinLine(Truck truck)
        {
            Line.Enqueue(truck);
        }

        public Truck SendOff()
        {
            return Line.Dequeue();
        }

        public Dock(string id)
        {
            this.Id = id;    
        }
    }
}
