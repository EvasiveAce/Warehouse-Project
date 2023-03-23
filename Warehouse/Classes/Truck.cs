using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseProject.Classes
{
    public class Truck
    {
        string[] FirstNames = { "Jon", "Marcus", "Murdoc", "2-D", "Noodle", "Russels", "Rivers", "Matt", "Brian", "Patrick", "Scott", "Mikey", "Francis" };
        string[] LastNames = { "Cuomo", "Wilson", "Nicals", "Hensley", "Gillenwater", "Sharp", "Bell", "Tipton", "Locke", "McReynolds" };

        string[] Companies = { "Weezer", "Gorillaz", "Pixies", "The Rentals", "Ween" };

        string Driver { get; set; }
        string DeliveryCompany { get; set; }
        Stack<Crate> Trailer { get; set; } = new Stack<Crate>();


        public void Load(Crate crate)
        {
            Trailer.Push(crate);
        }

        public Crate Unload()
        {
            return Trailer.Pop();
        }

        // how many max crates?????

        public Truck()
        {
            Random rand = new Random();
            var CrateAmount = rand.Next(1, 11);
            Driver = $"{FirstNames[rand.Next(0, FirstNames.Length)]} {LastNames[rand.Next(0, LastNames.Length)]}";
            DeliveryCompany = Companies[rand.Next(0, Companies.Length)];
            for (int i = 0; i < CrateAmount; i++)
            {
                Crate crateToAdd = new Crate();
                crateToAdd.Id = $"C{i+1}";
                //Console.WriteLine(crateToAdd.Id);
                //Console.WriteLine($"${crateToAdd.Price}");
                Load(crateToAdd);
            }
        }
    }
}
