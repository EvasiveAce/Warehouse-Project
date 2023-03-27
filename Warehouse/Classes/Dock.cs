using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseProject.Classes
{
    /// <summary>
    /// A Dock, to allow for trucks to unload their crates onto,
    /// and calculate sales of how much a truck makes, via their
    /// unloading of crates.
    /// </summary>
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

        /// <summary>
        /// Adds a truck to the Line Queue
        /// </summary>
        /// <param name="truck">A Truck class object, named truck</param>
        public void JoinLine(Truck truck)
        {
            TotalTrucks += 1;
            Line.Enqueue(truck);
        }

        /// <summary>
        /// Sends a finished truck away, after being processed
        /// </summary>
        /// <returns>Returns a specified truck</returns>
        public Truck SendOff()
        {
            return Line.Dequeue();
        }

        /// <summary>
        /// Dock Constructor
        /// 
        /// Makes a Dock
        /// </summary>
        /// <param name="id">A named ID, being in the specified format</param>
        public Dock(string id)
        {
            this.Id = id;
        }
    }
}
