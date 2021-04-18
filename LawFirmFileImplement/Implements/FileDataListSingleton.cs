using LawFirmListImplement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Text;
using LawFirmBusinessLogic.Enums;

namespace LawFirmFileImplement.Implements
{
    public class FileDataListSingleton
    {
        private static FileDataListSingleton instance;
        private readonly string BlankFileName = "Blank.xml";
        private readonly string OrderFileName = "Order.xml";
        private readonly string DocumentFileName = "Document.xml";
        private readonly string StorageFileName = "Storage.xml";
        public List<Blank> Blanks { get; set; }
        public List<Order> Orders { get; set; }
        public List<Document> Documents { get; set; }
        public List<Storage> Storages { get; set; }

        private FileDataListSingleton()
        {
            Blanks = LoadBlanks();
            Orders = LoadOrders();
            Documents = LoadDocuments();
            Storages = LoadStorage();
        }
        public static FileDataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new FileDataListSingleton();
            }
            return instance;
        }
        ~FileDataListSingleton()
        {
            SaveBlanks();
            SaveOrders();
            SaveDocuments();
            SaveStorages();
        }
        private List<Storage> LoadStorage()
        {
            var list = new List<Storage>();
            if (File.Exists(StorageFileName))
            {
                XDocument xDocument = XDocument.Load(StorageFileName);
                var xElements = xDocument.Root.Elements("Storage").ToList();

                foreach (var storage in xElements)
                {
                    var storageBlanks = new Dictionary<int, int>();

                    foreach (var material in storage.Element("StorageBlanks").Elements("StorageBlank").ToList())
                    {
                        storageBlanks.Add(Convert.ToInt32(material.Element("Key").Value), Convert.ToInt32(material.Element("Value").Value));
                    }

                    list.Add(new Storage
                    {
                        Id = Convert.ToInt32(storage.Attribute("Id").Value),
                        StorageName = storage.Element("StorageName").Value,
                        StorageManager = storage.Element("StorageManager").Value,
                        DateCreate = Convert.ToDateTime(storage.Element("DateCreate").Value),
                        StorageBlanks = storageBlanks
                    });
                }
            }
            return list;

        }
        private List<Blank> LoadBlanks()
        {
            var list = new List<Blank>();
            if (File.Exists(BlankFileName))
            {
                XDocument xDocument = XDocument.Load(BlankFileName);
                var xElements = xDocument.Root.Elements("Blank").ToList();

                foreach (var elem in xElements)
                {
                    list.Add(new Blank
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        BlankName = elem.Element("BlankName").Value
                    });
                }
            }
            return list;
        }
        private List<Order> LoadOrders()
        {
            var list = new List<Order>();
            if (File.Exists(OrderFileName))
            {
                XDocument xDocument = XDocument.Load(OrderFileName);
                var xElements = xDocument.Root.Elements("Order").ToList();

                foreach (var elem in xElements)
                {
                    OrderStatus status = (OrderStatus)0;
                    switch ((elem.Element("Status").Value))
                    {
                        case "Принят":
                            status = (OrderStatus)0;
                            break;
                        case "Выполняется":
                            status = (OrderStatus)1;
                            break;
                        case "Готов":
                            status = (OrderStatus)2;
                            break;
                        case "Оплачен":
                            status = (OrderStatus)3;
                            break;
                    }
                    Order order = new Order
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        DocumentId = Convert.ToInt32(elem.Element("DocumentId").Value),
                        Count = Convert.ToInt32(elem.Element("Count").Value),
                        Sum = Convert.ToDecimal(elem.Element("Sum").Value),
                        Status = status,
                        DateCreate = Convert.ToDateTime(elem.Element("DateCreate").Value)
                    };

                    if (!string.IsNullOrEmpty(elem.Element("DateImplement").Value))
                    {
                        order.DateImplement = Convert.ToDateTime(elem.Element("DateImplement").Value);
                    }
                    list.Add(order);
                }
            }
            return list;
        }
        private List<Document> LoadDocuments()
        {
            var list = new List<Document>();
            if (File.Exists(DocumentFileName))
            {
                XDocument xDocument = XDocument.Load(DocumentFileName);
                var xElements = xDocument.Root.Elements("Document").ToList();
                foreach (var elem in xElements)
                {
                    var documentBlanks = new Dictionary<int, int>();
                    foreach (var blank in
                   elem.Element("DocumentBlanks").Elements("DocumentBlank").ToList())
                    {
                        documentBlanks.Add(Convert.ToInt32(blank.Element("Key").Value),
                       Convert.ToInt32(blank.Element("Value").Value));
                    }
                    list.Add(new Document
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        DocumentName = elem.Element("DocumentName").Value,
                        Price = Convert.ToDecimal(elem.Element("Price").Value),
                        DocumentBlanks = documentBlanks
                    });
                }
            }
            return list;
        }
        private void SaveBlanks()
        {
            if (Blanks != null)
            {
                var xElement = new XElement("Blanks");
                foreach (var blank in Blanks)
                {
                    xElement.Add(new XElement("Blank",
                    new XAttribute("Id", blank.Id),
                    new XElement("BlankName", blank.BlankName)));
                }

                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(BlankFileName);
            }
        }
        private void SaveOrders()
        {
            if (Orders != null)
            {
                var xElement = new XElement("Orders");
                foreach (var order in Orders)
                {
                    xElement.Add(new XElement("Order",
                    new XAttribute("Id", order.Id),
                    new XElement("DocumentId", order.DocumentId),
                    new XElement("Sum", order.Sum),
                    new XElement("Count", order.Count),
                    new XElement("DateCreate", order.DateCreate),
                    new XElement("DateImplement", order.DateImplement),
                    new XElement("Status", order.Status)));
                }

                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(OrderFileName);
            }
        }
        private void SaveDocuments()
        {
            if (Documents != null)
            {
                var xElement = new XElement("Documents");
                foreach (var document in Documents)
                {
                    var blankElement = new XElement("DocumentBlanks");
                    foreach (var blank in document.DocumentBlanks)
                    {
                        blankElement.Add(new XElement("DocumentBlank",
                        new XElement("Key", blank.Key),
                        new XElement("Value", blank.Value)));
                    }
                    xElement.Add(new XElement("Document",
                     new XAttribute("Id", document.Id),
                     new XElement("DocumentName", document.DocumentName),
                     new XElement("Price", document.Price),
                     blankElement));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(DocumentFileName);
            }
        }
        private void SaveStorages()
        {
            if (Storages != null)
            {
                var xElement = new XElement("Storages");
                foreach (var storage in Storages)
                {
                    var blankElement = new XElement("StorageBlanks");
                    foreach (var blank in storage.StorageBlanks)
                    {
                        blankElement.Add(new XElement("StorageBlank",
                        new XElement("Key", blank.Key),
                        new XElement("Value", blank.Value)));
                    }
                    xElement.Add(new XElement("Storage",
                     new XAttribute("Id", storage.Id),
                     new XElement("StorageManager", storage.StorageManager),
                     new XElement("StorageName", storage.StorageName),
                     new XElement("DateCreate", storage.DateCreate),
                     blankElement));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(StorageFileName);
            }
        }
    }
}
