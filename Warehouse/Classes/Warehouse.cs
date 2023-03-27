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

        PriorityQueue<Truck, int> Entrance = new PriorityQueue<Truck, int>(Comparer<int>.Create((a, b) => a - b)); // increasing priorty for the company weezer

        Truck[] Trucks = new Truck[50]; // Array of up to 50 Trucks

        int timeIncrement = 0;

        string input = string.Empty;

        int longestLine = 0;


        /// <summary>
        /// Main method for the simulation execution.
        /// 
        /// Instantiates the docks, adds trucks, and unloads crates.
        /// </summary>
        public void Run()
        {
            var csv = new StringBuilder();

            Console.WriteLine("How many docks to simulate? (1-15)");
            input = Console.ReadLine();
            DynamicDock(Int32.Parse(input!)); // User input for number of Docks

            // Add trucks to the array until it's full
            for (int i = 0; i < Trucks.Length; i++)
            {
                Truck truckToAdd = new Truck();

                Trucks[i] = truckToAdd;
            }

            // this is where everything should happen
            while (timeIncrement < 48)
            {
                TruckSpawn(); // Pull a truck from the queue to move to a dock, for unloading

                // If there are more trucks left, at the entrance
                if (Entrance.Count > 0)
                {
                    var item = FindShortestPath();
                    // Leaving entrance line to dock line to actually be processed
                    item.JoinLine(RemoveTruck());
                    item.Processing = true;

                }
                // for each item in a list
                foreach (var item in Docks)
                {
                    LongestLineCheck(item);

                    // if a truck is currently being processed at a dock
                    if (item.Processing)
                    {
                        item.TimeInUse += 1;
                        var truckToUnload = item.Line.Peek(); // Check the contents of the truck currently at the dock
                        if (truckToUnload.Trailer.Count != 0) // If there is stuff to be unloaded
                        {
                            var crate = truckToUnload.Unload(); // Unload a crate
                            item.TotalCrates++; // Update TotalCrates amount
                            item.TotalSales += crate.Price; // Update the sales prices by adding the unloaded crate's price
                            if (truckToUnload.Trailer.Count > 0)
                            {
                                csv.AppendLine($"{timeIncrement}, {truckToUnload.Driver}, {truckToUnload.DeliveryCompany}, {crate.Id}, {crate.Price}, Crate unloaded and more to come");
                            } else
                            {
                                if (item.Line.Count != 0)
                                {
                                    csv.AppendLine($"{timeIncrement}, {truckToUnload.Driver}, {truckToUnload.DeliveryCompany}, {crate.Id}, {crate.Price}, Crate unloaded and empty, and another driver is here");
                                } else
                                {
                                    csv.AppendLine($"{timeIncrement}, {truckToUnload.Driver}, {truckToUnload.DeliveryCompany}, {crate.Id}, {crate.Price}, Crate unloaded and empty, and another driver is not here");
                                }
                            }
                        } else
                        {
                            item.SendOff(); // Truck is fully unloaded and heads away
                            item.Processing = false;
                        }
                    } else
                    {
                        item.TimeNotInUse += 1;
                    }
                }
                timeIncrement++;
            }

            //foreach (var dock in Docks)
            //{
            //    Console.WriteLine(dock.TotalSales);
            //    Console.WriteLine(dock.TimeNotInUse);
            //    Console.WriteLine(dock.TimeInUse);
            //    Console.WriteLine("\n");
            //}

            Console.WriteLine(Report());

            File.WriteAllText(@"../../../Report.csv", csv.ToString());
        }

        /// <summary>
        /// Adds a truck to the Entrance Queue
        /// 
        /// Special priority to Trucks from the Weezer company
        /// </summary>
        /// <param name="truck">A Truck class object, named truck</param>
        public void AddTruck(Truck truck)
        {
            if (truck.DeliveryCompany == "Weezer")
            {
                Entrance.Enqueue(truck, 1);
            } else
            {
                Entrance.Enqueue(truck, 0);
            }

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

        /// <summary>
        /// Finds the shortest path from the truck to the dock
        /// </summary>
        /// <returns>Returns the closest dock</returns>
        public Dock FindShortestPath()
        {
            Dock shortestPath = new Dock("");

            // Randomly high number to work for the first
            int startingCount = 100000;

            // Loop through all docks
            foreach (var item in Docks)
            {
                //If 
                if (item.Line.Count <= startingCount)
                {
                    startingCount = item.Line.Count;
                    shortestPath = item;
                }
            };
            return shortestPath;
        }


        /// <summary>
        /// Spawns new trucks and adds them to the line, based on time of day
        /// </summary>
        public void TruckSpawn()
        {
            Random rnd = new Random();
            // If midday/normal work hours
            if (timeIncrement > 12 || timeIncrement < 36)
            {
                var rndTemp = rnd.Next(0, 4);
                // 75% chance to come in
                if (rndTemp != 0)
                {
                    AddTruck(RandomTruckPull());
                }
            } else
            {
                var rndTemp = rnd.Next(0, 4);

                // if not midday, so 50% chance to come in
                if (rndTemp != 0 && rndTemp != 1)
                {
                    AddTruck(RandomTruckPull());
                }
            }
        }

        /// <summary>
        /// Makes number of docks from userInput value
        /// </summary>
        /// <param name="userInput">Takes userInput for number of docks to spawn</param>
        public void DynamicDock(int userInput)
        {
            // Sanitizes inputs
            if (userInput > 15)
            {
                userInput = 15;
            } else if (userInput < 1)
            {
                userInput = 1;
            }

            for (int i = 0; i < userInput; i++)
            {
                // Dock naming convention D + number
                if (i < 10)
                {
                    Dock dock = new Dock($"D0{i}");
                    Docks.Add(dock);
                } else
                {
                    Dock dock = new Dock($"D{i}");
                    Docks.Add(dock);
                }
            }
        }

        /// <summary>
        /// Checks for longest line
        /// </summary>
        /// <param name="dock">Dock object</param>
        public void LongestLineCheck(Dock dock)
        {
            if (dock.Line.Count > longestLine)
            {
                longestLine = dock.Line.Count;
            }
        }


        /// <summary>
        /// Makes and returns total report for statistics
        /// </summary>
        /// <returns>Returns the report!</returns>
        public string Report()
        {
            var reportToReturn = string.Empty;
            reportToReturn += "\n";
            reportToReturn += "\t-.-^-.-^-.-^-.- REPORT -.-^-.-^-.-^-.-";

            reportToReturn += "\n";
            reportToReturn += $"\tNumber of Docks Open:\t {input}";

            reportToReturn += "\n";
            reportToReturn += $"\tLongest Line:\t\t {longestLine}";

            reportToReturn += "\n";
            reportToReturn += $"\tTotal Number of Trucks:\t {GetTotalTrucks()}";

            reportToReturn += "\n";
            reportToReturn += $"\tTotal Number of Crates:\t {GetTotalCrates()}";

            reportToReturn += "\n";
            reportToReturn += $"\tTotal Crate Value:\t ${GetTotalValue()}";

            reportToReturn += "\n";
            reportToReturn += $"\tAverage Crate Value:\t ${Math.Round(GetTotalValue() / GetTotalCrates(), 2)}";

            reportToReturn += "\n";
            reportToReturn += $"\tAverage Truck Value:\t ${Math.Round(GetTruckAverage(), 2)}";

            reportToReturn += "\n";
            reportToReturn += $"\tTotal Dock Usetime:\t {GetTotalUsetime()}";

            reportToReturn += "\n";
            reportToReturn += $"\tTotal Dock Downtime:\t {GetTotalDowntime()}";


            reportToReturn += "\n";
            reportToReturn += $"\tAverage Dock Usetime:\t {GetTotalUsetime() / Int32.Parse(input)}";


            reportToReturn += "\n";
            reportToReturn += $"\tTotal Dock Cost:\t ${GetTotalTrucks() * 100}";

            reportToReturn += "\n";
            reportToReturn += $"\tTotal Revenue:\t\t ${Math.Round(GetTotalValue() - (GetTotalTrucks() * 100), 2)}";

            reportToReturn += "\n";
            reportToReturn += "\t-.-^-.-^-.-^-.- REPORT -.-^-.-^-.-^-.-";
            return reportToReturn;
        }

        /// <summary>
        /// Finds amount of total trucks
        /// </summary>
        /// <returns>Number of trucks</returns>
        public int GetTotalTrucks()
        {
            int totalTrucks = 0;
            foreach (var dock in Docks)
            {
                totalTrucks += dock.TotalTrucks;
            }
            return totalTrucks;
        }

        /// <summary>
        /// Finds amount of total crates
        /// </summary>
        /// <returns>Number of crates</returns>
        public int GetTotalCrates()
        {
            int totalCrates = 0;
            foreach (var dock in Docks)
            {
                totalCrates += dock.TotalCrates;
            }
            return totalCrates;
        }

        /// <summary>
        /// Finds amount of total value of sales
        /// </summary>
        /// <returns>Number of sales with 2 decimals</returns>
        public double GetTotalValue()
        {
            double price = 0;
            foreach (var dock in Docks)
            {
                price += dock.TotalSales;
            }
            return price;
        }

        /// <summary>
        /// Gets average price per truck
        /// </summary>
        /// <returns>Number of sales per truck with 2 decimals</returns>
        public double GetTruckAverage()
        {
            double truckPrice = 0;
            // probably a really bad way of implementing this but it is all I could come up with at the moment
            foreach (var truck in Trucks)
            {
                if (truck.Processed)
                {
                    foreach (var crate in truck.crates)
                    {
                        truckPrice += crate.Price;
                    }
                }
            }

            return truckPrice / GetTotalTrucks();
        }

        /// <summary>
        /// Gets total downtime of docks in seconds
        /// </summary>
        /// <returns>Number for time in seconds</returns>
        public int GetTotalDowntime()
        {
            int totalDowntime = 0;
            foreach (var dock in Docks)
            {
                totalDowntime += dock.TimeNotInUse;
            }
            return totalDowntime;
        }

        /// <summary>
        /// Gets total uptime of docks in seconds
        /// </summary>
        /// <returns>Number for time in seconds</returns>
        public int GetTotalUsetime()
        {
            int totalUsetime = 0;
            foreach (var dock in Docks)
            {
                totalUsetime += dock.TimeInUse;
            }
            return totalUsetime;
        }
    }
}
