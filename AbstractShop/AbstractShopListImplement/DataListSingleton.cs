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
        public List<Implementer> Implementers { set; get; }
        private DataListSingleton()
        {
            Blanks = new List<Blank>();
            Orders = new List<Order>();
            Documents = new List<Document>();
            Clients = new List<Client>();
            Implementers = new List<Implementer>();
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
