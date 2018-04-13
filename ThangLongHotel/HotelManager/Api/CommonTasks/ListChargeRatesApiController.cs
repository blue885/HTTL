using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using MVCCore.Repositories.PurchaseTasks;
using MVCDTO.PurchaseTasks;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCCore.Repositories.CommonTasks;
using MVCDTO.CommonTasks;

namespace HotelManager.Api.CommonTasks
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ListChargeRatesApiController : Controller
    {
        private readonly IListChargeRateRepository listChargeRateRepository;

        public ListChargeRatesApiController(IListChargeRateRepository listChargeRateRepository)
        {
            this.listChargeRateRepository = listChargeRateRepository;
        }

        public JsonResult GetListChargeRates([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<ListChargeRate> listChargeRates = this.listChargeRateRepository.GetAll();

            DataSourceResult response = listChargeRates.ToDataSourceResult(request, o => new ListChargeRatePrimitiveDTO
            {
                ChargeRateID = o.ChargeRateID,
                RoomTypeID = o.RoomTypeID,
                ListRoomTypeDescription = o.ListRoomType.Description,
                ChargeTypeID = o.ChargeTypeID,
                ListChargeTypeDescription = o.ListChargeType.Description,
                ChargeVolumn = o.ChargeVolumn,
                ChargeRate = o.ChargeRate,
                ChargeRateUpper = o.ChargeRateUpper,                
                Description = o.Description,
                Remarks = o.Remarks 
            });
            return Json(response, JsonRequestBehavior.AllowGet);
        }       
    
    }
}