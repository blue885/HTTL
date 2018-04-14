using System.Collections.Generic;

using AutoMapper;

using MVCModel.Models;
using MVCDTO.PurchaseTasks;
using HotelManager.ViewModels.PurchaseTasks;
using MVCDTO.CommonTasks;
using HotelManager.ViewModels.CommonTasks;


[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(HotelManager.App_Start.AutoMapperConfig), "SetupMappings")]
namespace HotelManager.App_Start
{
    public static class AutoMapperConfig
    {
        public static void SetupMappings()
        {
            Mapper.CreateMap<PurchaseInvoice, PurchaseInvoiceDTO>();
            Mapper.CreateMap<PurchaseInvoice, PurchaseInvoiceViewModel>();
            Mapper.CreateMap<PurchaseInvoiceViewModel, PurchaseInvoice>();
            Mapper.CreateMap<PurchaseInvoicePrimitiveDTO, PurchaseInvoice>();
            Mapper.CreateMap<PurchaseInvoiceDetail, PurchaseInvoiceDetailDTO>();
            Mapper.CreateMap<PurchaseInvoiceDetailDTO, PurchaseInvoiceDetail>();

            Mapper.CreateMap<ListChargeRate, ListChargeRateDTO>();            
            Mapper.CreateMap<ListChargeRate, ListChargeRateViewModel>();
            Mapper.CreateMap<ListChargeRateViewModel, ListChargeRate>();
            Mapper.CreateMap<ListChargeRatePrimitiveDTO, ListChargeRate>();

            Mapper.CreateMap<ListChargeType, ListChargeTypeDTO>();
            Mapper.CreateMap<ListChargeType, ListChargeTypeViewModel>();
            Mapper.CreateMap<ListChargeTypeViewModel, ListChargeType>();
            Mapper.CreateMap<ListChargeTypePrimitiveDTO, ListChargeType>();

            Mapper.CreateMap<ListRoom, ListRoomDTO>();
            Mapper.CreateMap<ListRoom, ListRoomViewModel>();
            Mapper.CreateMap<ListRoomViewModel, ListRoom>();
            Mapper.CreateMap<ListRoomPrimitiveDTO, ListRoom>();

            Mapper.CreateMap<ListService, ListServiceDTO>();
            Mapper.CreateMap<ListService, ListServiceViewModel>();
            Mapper.CreateMap<ListServiceViewModel, ListService>();
            Mapper.CreateMap<ListServicePrimitiveDTO, ListService>();
        }
    }
}