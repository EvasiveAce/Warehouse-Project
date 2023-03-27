using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseProject.Classes;

namespace WarehouseProject
{
    /// <summary>
    /// Warehouse project, by Ethan Hensley and Patrick Vergason
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            Warehouse warehouse = new Warehouse();

            warehouse.Run();
        }
    }

}