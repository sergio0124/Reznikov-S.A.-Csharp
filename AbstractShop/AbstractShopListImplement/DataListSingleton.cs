using LawFirmListImplement.Models;
using System.Collections.Generic;
namespace LawFirmListImplement
{
    public class DataListSingleton
    {
        private static DataListSingleton instance;
        public List<Blank> Blanks { get; set; }
        public List<Order> Orders { get; set; }
        public List<Document> Documents { get; set; }
        private DataListSingleton()
        {
            Blanks = new List<Blank>();
            Orders = new List<Order>();
            Documents = new List<Document>();
        }
        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }
            return instance;
        }
    }
}
