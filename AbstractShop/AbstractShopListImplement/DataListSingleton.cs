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
        public List<Client> Clients { set; get; }
        private DataListSingleton()
        {
            Blanks = new List<Blank>();
            Orders = new List<Order>();
            Documents = new List<Document>();
            Clients = new List<Client>();
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
