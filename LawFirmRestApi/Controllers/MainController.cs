using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.BusinessLogic;
using LawFirmBusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LawFirmRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly OrderLogic _order;

        private readonly DocumentLogic _document;

        private readonly OrderLogic _main;

        public MainController(OrderLogic order, DocumentLogic document, OrderLogic main)
        {
            _order = order;
            _document = document;
            _main = main;
        }

        [HttpGet]
        public List<DocumentViewModel> GetDocumentList() => _document.Read(null)?.ToList();

        [HttpGet]
        public DocumentViewModel GetDocument(int documentId) => _document.Read(new DocumentBindingModel { Id = documentId })?[0];

        [HttpGet]
        public List<OrderViewModel> GetOrders(int clientId) => _order.Read(new OrderBindingModel { ClientId = clientId });

        [HttpPost]
        public void CreateOrder(CreateOrderBindingModel model) => _main.CreateOrder(model);
    }
}