using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseProject.Classes
{
    /// <summary>
    /// Warehouse class, which houses the process of making the docks,
    /// having the trucks go through them, and unload the crates.
    /// </summary>
    public class Warehouse
    {
        List<Dock> Docks = new List<Dock>(); 

        Queue<Truck> Entrance = new Queue<Truck>(); // Entrance Queue for the trucks

        Truck[] Trucks = new Truck[50]; // Array of up to 50 Trucks

        int timeIncrement = 0;

        /// <summary>
        /// Main method for the simulation execution.
        /// 
        /// Instantiates the docks, adds trucks, and unloads crates.
        /// </summary>
        public void Run()
        {
            // Dock naming convention D + number
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


            // Later on
            // AddTruck(truck1, dock1);


            // this is where everything should happen
            while(timeIncrement < 48)
            {
                // Pull a truck from the queue to move to a dock, for unloading
                AddTruck(RandomTruckPull());
                // for each item in a list
                foreach (var item in Docks)
                {
                    // If there is a dock without a truck at it, and there are more trucks left
                    if (item.Line.Count == 0 && Entrance.Count > 0)
                    {
                        // Leaving entrance line to dock line to actually be processed
                        item.JoinLine(RemoveTruck());
                        item.TimeNotInUse += 1;
                        item.Processing = true;

                    }
                    else if(item.Processing) // if a truck is currently being processed at a dock
                    {
                        item.TimeInUse += 1;
                        var truckToUnload = item.Line.Peek(); // Check the contents of the truck currently at the dock
                        if(truckToUnload.Trailer.Count != 0) // If there is stuff to be unloaded
                        {
                            var crate = truckToUnload.Unload(); // Unload a crate
                            item.TotalCrates++; // Update TotalCrates amount
                            item.TotalSales += crate.Price; // Update the sales prices by adding the unloaded crate's price
                        }
                        else
                        {
                            item.SendOff(); // Truck is fully unloaded and heads away
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

        /// <summary>
        /// Adds a truck to the Entrance Queue
        /// </summary>
        /// <param name="truck">A Truck class object, named truck</param>
        public void AddTruck(Truck truck)
        {
            Entrance.Enqueue(truck);
            //dock.JoinLine(truck);
        }


        /// <summary>
        /// Removes a truck from the Entrance Queue
        /// </summary>
        /// <returns>The specific truck to be removed</returns>
        public Truck RemoveTruck()
        {
            return Entrance.Dequeue();
            //dock.SendOff();
        }


        /// <summary>
        /// Pulls a random truck from the Entrance Queue to be processed at the dock
        /// </summary>
        /// <returns>The specific truck to be processed</returns>
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
