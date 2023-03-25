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

        Truck[] Trucks = new Truck[50];

        int timeIncrement = 0;

        public void Run()
        {
            // Dock naming convetion D + number
            Dock dock1 = new Dock("D01");
            Docks.Add(dock1);

            Dock dock2 = new Dock("D02");
            Docks.Add(dock2);

            Dock dock3 = new Dock("D03");
            Docks.Add(dock3);

            for (int i = 0; i < Trucks.Length; i++)
            {
                Truck truckToAdd = new Truck();
                Trucks[i] = truckToAdd;
            }


            // LAter on
            // AddTruck(truck1, dock1);


            // this is where everything will happen i think
            while(timeIncrement < 48)
            {
                AddTruck(RandomTruckPull());
                // for each item in a list
                foreach (var item in Docks)
                {

                    if (item.Line.Count == 0 && Entrance.Count > 0)
                    {
                        // Leaving entrance line to dock line to actually be processed
                        item.JoinLine(RemoveTruck());
                        item.TimeNotInUse += 1;
                        item.Processing = true;

                    }
                    else if(item.Processing)
                    {
                        item.TimeInUse += 1;
                        var truckToUnload = item.Line.Peek();
                        if(truckToUnload.Trailer.Count != 0)
                        {
                            var crate = truckToUnload.Unload();
                            item.TotalCrates++;
                            item.TotalSales += crate.Price;
                        }
                        else
                        {
                            item.SendOff();
                            item.Processing = false;
                        }
                    }
                    else
                    {
                        item.TimeNotInUse += 1;
                    }
                }
                timeIncrement++;
            }
            Console.WriteLine(dock1.TotalSales);
            Console.WriteLine(dock1.TimeNotInUse);
            Console.WriteLine(dock1.TimeInUse);
            Console.WriteLine(" ");
            Console.WriteLine(dock2.TotalSales);
            Console.WriteLine(dock2.TimeNotInUse);
            Console.WriteLine(dock2.TimeInUse);
            Console.WriteLine(" ");
            Console.WriteLine(dock3.TotalSales);
            Console.WriteLine(dock3.TimeNotInUse);
            Console.WriteLine(dock3.TimeInUse);
        }

        public void AddTruck(Truck truck)
        {
            Entrance.Enqueue(truck);
            //dock.JoinLine(truck);
        }


        public Truck RemoveTruck()
        {
            return Entrance.Dequeue();
            //dock.SendOff();
        }


        public Truck RandomTruckPull()
        {
            Random rand = new Random();

            int truckIndex = rand.Next(0, Trucks.Length);
            Truck truckToAdd = Trucks[truckIndex];
            if (truckToAdd.Processed)
            {
                RandomTruckPull();
            }
            Trucks[truckIndex].Processed = true;
            return truckToAdd;
            
        }
    }
}
