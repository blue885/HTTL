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
    public class ListChargeTypesApiController : Controller
    {
        private readonly IListChargeTypeRepository listChargeTypeRepository;

        public ListChargeTypesApiController(IListChargeTypeRepository listChargeTypeRepository)
        {
            this.listChargeTypeRepository = listChargeTypeRepository;
        }

        public JsonResult GetListChargeTypes([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<ListChargeType> listChargeTypes = this.listChargeTypeRepository.GetAll();

            DataSourceResult response = listChargeTypes.ToDataSourceResult(request, o => new ListChargeTypePrimitiveDTO
            {
                ChargeTypeID = o.ChargeTypeID,
                HoursPerBlock = o.HoursPerBlock,
                GraceMinutes = o.GraceMinutes,                     
                Description = o.Description,
                Remarks = o.Remarks 
            });
            return Json(response, JsonRequestBehavior.AllowGet);
        }       
    
    }
}