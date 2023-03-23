using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseProject.Classes
{
    public class Warehouse
    {
        List<Dock> Docks = new List<Dock>();

        Queue<Truck> Entrance = new Queue<Truck>();


        public void Run()
        {
            // Dock naming convetion D + number
            Dock dock1 = new Dock("D01");
            Docks.Add(dock1);
            Truck truck1 = new Truck();
            AddTruck(truck1, dock1);
            Console.WriteLine(Entrance.Count);
            Console.WriteLine(dock1.Line.Count);
        }

        public void AddTruck(Truck truck, Dock dock)
        {
            Entrance.Enqueue(truck);
            dock.JoinLine(truck);
        }



    }
}
