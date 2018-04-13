using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using MVCCore.Repositories.CommonTasks;
using MVCDTO.CommonTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Api.CommonTasks
{  

    public class ListServicesApiController : Controller
    {
        private readonly IListServiceRepository listServiceRepository;

        public ListServicesApiController(IListServiceRepository listServiceRepository)
        {
            this.listServiceRepository = listServiceRepository;
        }       

        public JsonResult GetListServices([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<ListService> listServices = this.listServiceRepository.GetAll();

            DataSourceResult response = listServices.ToDataSourceResult(request, o => new ListServicePrimitiveDTO
            {
                ServiceID = o.ServiceID,
                UnitForSales = o.UnitForSales,
                UnitPrice = o.UnitPrice,                          
                Description = o.Description,
                Remarks = o.Remarks 
            });
            return Json(response, JsonRequestBehavior.AllowGet);
        }       


        public JsonResult SearchListServiceByName(string text)
        {
            var result = listServiceRepository.SearchServicesByName(text).Select(s => new { s.ServiceID, s.Description });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}