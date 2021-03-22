using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.HelperModels;
using LawFirmBusinessLogic.Interfaces;
using LawFirmBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LawFirmBusinessLogic.BusinessLogic
{
    public class ReportLogic
    {

        private readonly IBlankStorage _blankStorage;
        private readonly IDocumentStorage _documentStorage;
        private readonly IOrderStorage _orderStorage;
        public ReportLogic(IDocumentStorage documentStorage, IBlankStorage
       blankStorage, IOrderStorage orderStorage)
        {
            _documentStorage = documentStorage;
            _blankStorage = blankStorage;
            _orderStorage = orderStorage;
        }
        /// <summary>
        /// Получение списка компонент с указанием, в каких изделиях используются
        /// </summary>
        /// <returns></returns>104
        public List<ReportDocumentBlankViewModel> GetDocumentBlanks()
        {
            var blanks = _blankStorage.GetFullList();
            var documents = _documentStorage.GetFullList();
            var list = new List<ReportDocumentBlankViewModel>();
            foreach (var blank in blanks)
            {
                var record = new ReportDocumentBlankViewModel
                {
                    BlankName = blank.BlankName,
                    Documents = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var document in documents)
                {
                    if (document.DocumentBlanks.ContainsKey(blank.Id))
                    {
                        record.Documents.Add(new Tuple<string, int>(document.DocumentName,
                       document.DocumentBlanks[blank.Id].Item2));
                        record.TotalCount +=
                       document.DocumentBlanks[blank.Id].Item2;
                    }
                }
                list.Add(record);
            }
            return list;
        }
        /// <summary>
        /// Получение списка заказов за определенный период
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ReportOrdersViewModel> GetOrders(ReportBindingModel model)
        {
            return _orderStorage.GetFilteredList(new OrderBindingModel
            {
                DateFrom =
           model.DateFrom,
                DateTo = model.DateTo
            })
            .Select(x => new ReportOrdersViewModel
            {
                DateCreate = x.DateCreate,
                DocumentName = x.DocumentName,
                Count = x.Count,
                Sum = x.Sum,
                Status = x.Status
            })
           .ToList();
        }
        /// <summary>
        /// Сохранение компонент в файл-Word
        /// </summary>
        /// <param name="model"></param>
        public void SaveBlanksToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список бланков",
                Blanks = _blankStorage.GetFullList()
            });
        }
        /// <summary>
        /// Сохранение компонент с указаеним продуктов в файл-Excel
        /// </summary>
        /// <param name="model"></param>
        public void SaveDocumentBlanksToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список компонент",
                DocumentBlanks = GetDocumentBlanks()
            });
        }

        /// <summary>
        /// Сохранение заказов в файл-Pdf
        /// </summary>
        /// <param name="model"></param>
        [Obsolete]
        public void SaveOrdersToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Orders = GetOrders(model)
            });
        }

    }
}
