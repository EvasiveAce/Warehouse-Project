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

        PriorityQueue<Truck, int> Entrance = new PriorityQueue<Truck, int>(Comparer<int>.Create((a, b) => a - b)); // increasing priorty for the company weezer

        Truck[] Trucks = new Truck[50];

        int timeIncrement = 0;

        string input = string.Empty;

        int longestLine = 0;

        public void Run()
        {
            var csv = new StringBuilder();

            Console.WriteLine("How many docks to simulate? (1-15)");
            input = Console.ReadLine();
            DynamicDock(Int32.Parse(input!));

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

                TruckSpawn();


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
                    if(item.Processing)
                    {
                        item.TimeInUse += 1;
                        var truckToUnload = item.Line.Peek();
                        if(truckToUnload.Trailer.Count != 0)
                        {
                            var crate = truckToUnload.Unload();
                            item.TotalCrates++;
                            item.TotalSales += crate.Price;
                            if (truckToUnload.Trailer.Count > 0)
                            {
                                csv.AppendLine($"{timeIncrement}, {truckToUnload.Driver}, {truckToUnload.DeliveryCompany}, {crate.Id}, {crate.Price}, Crate unloaded and more to come");
                            }
                            else
                            {
                                if (item.Line.Count != 0)
                                {
                                    csv.AppendLine($"{timeIncrement}, {truckToUnload.Driver}, {truckToUnload.DeliveryCompany}, {crate.Id}, {crate.Price}, Crate unloaded and empty, and another driver is here");
                                }
                                else
                                {
                                    csv.AppendLine($"{timeIncrement}, {truckToUnload.Driver}, {truckToUnload.DeliveryCompany}, {crate.Id}, {crate.Price}, Crate unloaded and empty, and another driver is not here");
                                }
                                
                            }
                            
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

        public void AddTruck(Truck truck)
        {
            if(truck.DeliveryCompany == "Weezer")
            {
                Entrance.Enqueue(truck, 1);
            }
            else
            {
                Entrance.Enqueue(truck, 0);
            }
            
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

        public Dock FindShortestPath()
        {
            Dock shortestPath = new Dock("");
            int startingCount = 100000;
            foreach (var item in Docks)
            {
                if(item.Line.Count <= startingCount)
                {
                    startingCount = item.Line.Count;
                    shortestPath = item;
                }
            };

            return shortestPath;
        }


        public void TruckSpawn()
        {
            Random rnd = new Random();
            if (timeIncrement > 12 || timeIncrement < 36)
            {
                var rndTemp = rnd.Next(0, 4);
                if (rndTemp != 0)
                {
                    AddTruck(RandomTruckPull());
                }
            }
            else
            {
                var rndTemp = rnd.Next(0, 4);
                if (rndTemp != 0 && rndTemp != 1)
                {
                    AddTruck(RandomTruckPull());
                }
            }
        }

        public void DynamicDock(int userInput)
        {
            if(userInput > 15)
            {
                userInput = 15;
            } else if (userInput < 1)
            {
                userInput = 1;
            }

            for (int i = 0; i < userInput; i++)
            {
                // Dock naming convetion D + number
                Dock dock = new Dock($"D0{i}");
                Docks.Add(dock);
            }
        }

        public void LongestLineCheck(Dock dock)
        {
            if (dock.Line.Count > longestLine)
            {
                longestLine = dock.Line.Count;
            }
        }


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
            reportToReturn += $"\tAverage Crate Value:\t ${Math.Round(GetTotalValue()/GetTotalCrates(), 2)}";

            reportToReturn += "\n";
            reportToReturn += $"\tAverage Truck Value:\t ${Math.Round(GetTruckAverage(), 2)}";

            reportToReturn += "\n";
            reportToReturn += $"\tTotal Dock Usetime:\t {GetTotalUsetime()}";

            reportToReturn += "\n";
            reportToReturn += $"\tTotal Dock Downtime:\t {GetTotalDowntime()}";


            reportToReturn += "\n";
            reportToReturn += $"\tAverage Dock Usetime:\t {GetTotalUsetime()/Int32.Parse(input)}";


            reportToReturn += "\n";
            reportToReturn += $"\tTotal Dock Cost:\t ${GetTotalTrucks() * 100}";

            reportToReturn += "\n";
            reportToReturn += $"\tTotal Revenue:\t\t ${Math.Round(GetTotalValue() - (GetTotalTrucks() * 100),2)}";

            reportToReturn += "\n";
            reportToReturn += "\t-.-^-.-^-.-^-.- REPORT -.-^-.-^-.-^-.-";
            return reportToReturn;
        }

        public int GetTotalTrucks()
        {
            int totalTrucks = 0;
            foreach (var dock in Docks)
            {
                totalTrucks += dock.TotalTrucks;
            }
            return totalTrucks;
        }

        public int GetTotalCrates()
        {
            int totalCrates = 0;
            foreach (var dock in Docks)
            {
                totalCrates += dock.TotalCrates;
            }
            return totalCrates;
        }

        public double GetTotalValue()
        {
            double price = 0;
            foreach (var dock in Docks)
            {
                price += dock.TotalSales;
            }
            return price;
        }
        
        public double GetTruckAverage()
        {
            double truckPrice = 0;
            // probably a really bad way of implementing this but it is all I could come up with at the moment
            foreach (var truck in Trucks)
            {
                if(truck.Processed)
                {
                    foreach (var crate in truck.crates)
                    {
                        truckPrice += crate.Price;
                    }
                }
            }

            return truckPrice/GetTotalTrucks();
        }

        public int GetTotalDowntime()
        {
            int totalDowntime = 0;
            foreach (var dock in Docks)
            {
                totalDowntime += dock.TimeNotInUse;
            }
            return totalDowntime;
        }

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
