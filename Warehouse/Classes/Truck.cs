using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseProject.Classes
{
    /// <summary>
    /// Houses the truck making process, including making a new Driver
    /// and Company, adding crates onto a new truck, and the method to unload them.
    /// </summary>
    public class Truck
    {
        string[] FirstNames = { "Jon", "Marcus", "Murdoc", "2-D", "Noodle", "Russels", "Rivers", "Matt", "Brian", "Patrick", "Scott", "Mikey", "Francis" };
        string[] LastNames = { "Cuomo", "Wilson", "Nicals", "Hensley", "Gillenwater", "Sharp", "Bell", "Tipton", "Locke", "McReynolds" };

        string[] Companies = { "Weezer", "Gorillaz", "Pixies", "The Rentals", "Ween" };

        public string Driver { get; set; }
        public string DeliveryCompany { get; set; }
        public Stack<Crate> Trailer { get; set; } = new Stack<Crate>();

        public bool Processed = false;


        /// <summary>
        /// When called, will put/load a random number of crates onto the truck.
        /// </summary>
        /// <param name="crate">A crate from the truck</param>
        public void Load(Crate crate)
        {
            Trailer.Push(crate);
        }

        /// <summary>
        /// When at a dock, unload a crate from the truck's trailer
        /// </summary>
        /// <returns>Returns a crate to be unloaded</returns>
        public Crate Unload()
        {
            return Trailer.Pop();
        }

        // how many max crates?????

        /// <summary>
        /// Truck constructor
        /// 
        /// Makes a Truck object, with a random Driver, Company, and Crate amount inside the truck.
        /// </summary>
        public Truck()
        {
            Random rand = new Random();
            var CrateAmount = rand.Next(1, 11); // Randomizes the number of crates to be added to a truck
            Driver = $"{FirstNames[rand.Next(0, FirstNames.Length)]} {LastNames[rand.Next(0, LastNames.Length)]}";
            DeliveryCompany = Companies[rand.Next(0, Companies.Length)];
            // When i is less than the number of crates found above, add a crate
            for (int i = 0; i < CrateAmount; i++)
            {
                Crate crateToAdd = new Crate();
                crateToAdd.Id = $"C{i+1}";
                //Console.WriteLine(crateToAdd.Id);
                //Console.WriteLine($"${crateToAdd.Price}");
                Load(crateToAdd);
            }

            // Crates should be loaded onto the truck at the randomized amount found above
        }
    }
}
