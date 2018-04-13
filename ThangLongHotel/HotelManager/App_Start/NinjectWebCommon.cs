[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(HotelManager.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(HotelManager.App_Start.NinjectWebCommon), "Stop")]

namespace HotelManager.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    using MVCModel.Models;
    using MVCCore.Services.PurchaseTasks;
    using MVCService.PurchaseTasks;
    using MVCCore.Repositories.PurchaseTasks;
    using MVCData.Repositories.PurchaseTasks;
    using MVCCore.Repositories.CommonTasks;
    using MVCData.Repositories.CommonTasks;
    using HotelManager.Builders;
    using MVCCore.Services.CommonTasks;
    using MVCService.CommonTasks;
    using HotelManager.Builders.CommonTasks;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                kernel.Bind<IPurchaseInvoiceService>().To<PurchaseInvoiceService>();
                kernel.Bind<IPurchaseInvoiceRepository>().To<PurchaseInvoiceRepository>();
                kernel.Bind<IPurchaseInvoiceViewModelSelectListBuilder>().To<PurchaseInvoiceViewModelSelectListBuilder>();

                kernel.Bind<IHotelRepository>().To<HotelRepository>();
                kernel.Bind<IHotelSelectListBuilder>().To<HotelSelectListBuilder>();

                kernel.Bind<IListChargeRateBuilder>().To<ListChargeRateBuilder>();
                kernel.Bind<IListChargeRateService>().To<ListChargeRateService>();
                kernel.Bind<IListChargeRateRepository>().To<ListChargeRateRepository>();

                kernel.Bind<IListRoomTypeBuilder>().To<ListRoomTypeBuilder>();
                kernel.Bind<IListRoomTypeService>().To<ListRoomTypeService>();
                kernel.Bind<IListRoomTypeRepository>().To<ListRoomTypeRepository>();

                kernel.Bind<IListChargeTypeBuilder>().To<ListChargeTypeBuilder>();
                kernel.Bind<IListChargeTypeService>().To<ListChargeTypeService>();
                kernel.Bind<IListChargeTypeRepository>().To<ListChargeTypeRepository>();

                kernel.Bind<IListRoomBuilder>().To<ListRoomBuilder>();
                kernel.Bind<IListRoomService>().To<ListRoomService>();
                kernel.Bind<IListRoomRepository>().To<ListRoomRepository>();

                kernel.Bind<IListRoomStatusesBuilder>().To<ListRoomStatusesBuilder>();
                kernel.Bind<IListRoomStatusesService>().To<ListRoomStatusesService>();
                kernel.Bind<IListRoomStatusesRepository>().To<ListRoomStatusesRepository>();

                kernel.Bind<IListServiceBuilder>().To<ListServiceBuilder>();
                kernel.Bind<IListServiceService>().To<ListServiceService>();
                kernel.Bind<IListServiceRepository>().To<ListServiceRepository>();

                kernel.Bind<HotelManagerEntities>().ToSelf().InRequestScope();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }        
    }
}
