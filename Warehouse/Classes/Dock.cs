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
        public double TotalSales { get; set; }
        public int TotalCrates { get; set; }
        public int TotalTrucks { get; set; }
        public int TimeInUse { get; set; } 
        public int TimeNotInUse { get; set; } 

        public bool Processing = false;

        //Total Trucks *  100

        public void JoinLine(Truck truck)
        {
            TotalTrucks += 1;
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
