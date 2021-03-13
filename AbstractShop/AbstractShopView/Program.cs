using LawFirmBusinessLogic.BusinessLogic;
using LawFirmBusinessLogic.Interfaces;
using LawFirmListImplement.Implements;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;
namespace LawFirmView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<IBlankStorage, BlankStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IOrderStorage, OrderStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IDocumentStorage, DocumentStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<BlankLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<OrderLogic>(new 
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<DocumentLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IStorageStorage, StorageStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<StorageLogic>(new
           HierarchicalLifetimeManager());
            return currentContainer;
        }

    }
}
