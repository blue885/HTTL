using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelManager.Models;
using System.Net;
using HotelManager.ViewModels;
using Newtonsoft.Json.Linq;
using System.Web.UI;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using Microsoft.AspNet.Identity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

using MVCCore.Enums;
using MVCModel.Models;
using MVCData.Repositories.CommonTasks;
using HotelManager.Configuration;

namespace HotelManager.Controllers
{

    public partial class HomeController : Controller
    {
        private HotelManagerEntities db = new HotelManagerEntities();
        private SchedulerBillingService schedulerBillingService;
        private SchedulerBookingService schedulerBookingService;


        public HomeController()
        {
            this.schedulerBillingService = new SchedulerBillingService();
            this.schedulerBookingService = new SchedulerBookingService();
        }

        //[OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 10)]
        public ActionResult Index(int? id)
        {
            if (id == null)
                id = Home.GetHotelID(this.HttpContext);
            else
                Home.SetHotelID(this.HttpContext, (int)id);

            var hotelFloorLevel = db.GetHotelFloorLevel(User.Identity.GetUserId(), id);
            var hotelRoom = db.GetHotelRoom(User.Identity.GetUserId(), id);
            var billingLists = db.GetBillingList(User.Identity.GetUserId(), id);
            List<HotelRoom> listHotelRoom = hotelRoom.ToList();
            var phongCoKhach = listHotelRoom.Count(c => c.BillingID > 0 && c.RoomTypeID != 5);
            var phongOK = listHotelRoom.Count(c => c.BillingID == 0 && c.RoomStatusID == 1 && c.RoomTypeID != 5);
            var phongSuaChua = listHotelRoom.Count(c => c.BillingID == 0 && c.RoomStatusID == 2 && c.RoomTypeID != 5);
            var phongDonPhong = listHotelRoom.Count(c => c.BillingID == 0 && c.RoomStatusID == 3 && c.RoomTypeID != 5);

            var homeViewModel = new HomeViewModel()
            {
                HotelFloorLevels = hotelFloorLevel.ToList(),
                HotelRooms = listHotelRoom,
                TongPhongDangCoKhach = phongCoKhach,
                TongPhongTrongOK = phongOK,
                TongPhongSuaChua = phongSuaChua,
                TongPhongDonPhong = phongDonPhong,
                //BillingLists = billingLists.ToList(),
                ListContextMenus = db.ListContextMenus.Where(w => w.ModuleID == 1).OrderBy(o => o.MenuOrderNo).ToList() //ModuleID == 1: Home Module
            };

            return View(homeViewModel);
        }

        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 10)]
        public ActionResult GetRoomList()
        {
            int id = Home.GetHotelID(this.HttpContext);
            var hotelFloorLevel = db.GetHotelFloorLevel(User.Identity.GetUserId(), id);
            var hotelRoom = db.GetHotelRoom(User.Identity.GetUserId(), id);
            //var billingLists = db.GetBillingList(User.Identity.GetUserId(), id);
            List<HotelRoom> listHotelRoom = hotelRoom.ToList();
            var phongCoKhach = listHotelRoom.Count(c => c.BillingID > 0 && c.RoomTypeID != 5);
            var phongOK = listHotelRoom.Count(c => c.BillingID == 0 && c.RoomStatusID == 1 && c.RoomTypeID != 5);
            var phongSuaChua = listHotelRoom.Count(c => c.BillingID == 0 && c.RoomStatusID == 2 && c.RoomTypeID != 5);
            var phongDonPhong = listHotelRoom.Count(c => c.BillingID == 0 && c.RoomStatusID == 3 && c.RoomTypeID != 5);

            var homeViewModel = new HomeViewModel()
            {
                HotelFloorLevels = hotelFloorLevel.ToList(),
                HotelRooms = listHotelRoom,
                TongPhongDangCoKhach = phongCoKhach,
                TongPhongTrongOK = phongOK,
                TongPhongSuaChua = phongSuaChua,
                TongPhongDonPhong = phongDonPhong,
                //BillingLists = billingLists.ToList(),
                ListContextMenus = db.ListContextMenus.Where(w => w.ModuleID == 1).OrderBy(o => o.MenuOrderNo).ToList() //ModuleID == 1: Home Module
            };

            return PartialView("RoomList", homeViewModel);
        }

        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 30)]
        public ActionResult GetScheduler()
        {
            return PartialView("SchedulerView");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteBilling()
        {
            DeleteBillingViewModel deleteBillingViewModel = new DeleteBillingViewModel();
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);            
            var lastPreviousDate = month.AddDays(-1);
            deleteBillingViewModel.DeleteDate = lastPreviousDate;
            deleteBillingViewModel.ErrorMessage = "";

            return View(deleteBillingViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteBilling(DeleteBillingViewModel deleteBillingViewModel)
        {
         
            try
            {
                if (!ModelState.IsValid)
                    return View(deleteBillingViewModel);

                var todayResult = DateTime.Today;
                var monthResult = new DateTime(todayResult.Year, todayResult.Month, 1);
                var lastDateResult = monthResult.AddDays(-1);

                if (deleteBillingViewModel.DeleteDate <= lastDateResult)
                {
                    var result = db.DeleteBilling(deleteBillingViewModel.DeleteDate);                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    throw new Exception("Vui lòng chọn ngày tháng trước!!!");
                }                

            }
            catch (Exception exception)
            {
                deleteBillingViewModel.ErrorMessage = exception.Message;
                ModelState.AddValidationErrors(exception);
                return View(deleteBillingViewModel);
            }
        }

        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 10)]
        public ActionResult GetBillingDetail()
        {
            int id = Home.GetHotelID(this.HttpContext);
            var hotelFloorLevel = db.GetHotelFloorLevel(User.Identity.GetUserId(), id);
            var hotelRoom = db.GetHotelRoom(User.Identity.GetUserId(), id);
            var billingLists = db.GetBillingList(User.Identity.GetUserId(), id);

            var homeViewModel = new HomeViewModel()
            {
                HotelFloorLevels = hotelFloorLevel.ToList(),
                HotelRooms = hotelRoom.ToList(),
                TongPhongDangCoKhach = hotelRoom.Count(c => c.BillingID > 0),
                //BillingLists = billingLists.ToList(),
                ListContextMenus = db.ListContextMenus.Where(w => w.ModuleID == 1).OrderBy(o => o.MenuOrderNo).ToList() //ModuleID == 1: Home Module
            };

            return PartialView("HomeBillingDetail", homeViewModel);
        }


        public ActionResult Print(int? id)
        {
            PrintViewModel printViewModel = new PrintViewModel() { Id = id != null ? (int)id : 0 };
            return View(printViewModel);


            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //BillingMaster billingMaster = db.BillingMasters.Find(id);
            //if (billingMaster == null) { return HttpNotFound(); }

            //var viewModel = new BillingViewModels()
            //{
            //    BillingMaster = billingMaster,
            //    BillingDetailFulls = db.GetBillingDetailFull(id).ToList()
            //};

            //return View(viewModel);
        }

        [HttpPost]
        public int GetActiveBill(int? id)
        {
            var billingID = db.GetActiveBill(id).DefaultIfEmpty(0).First();
            return (int)billingID;
        }

        //Get: Home/ChangeBillingRoom
        public ActionResult ChangeBillingRoom(int? billingID, int? roomID)
        {
            if (billingID == null || roomID == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            ListRoom listRoom = db.ListRooms.FirstOrDefault(room => room.RoomID == roomID);

            var viewModel = new BillingRoomViewModel()
            {
                HotelID = listRoom.HotelID,
                RoomCategoryID = listRoom.ListRoomType.RoomCategoryID,

                BillingID = (int)billingID,
                RoomID = (int)roomID
            };

            return View("ChangeBillingRoom", viewModel);
        }

        [HttpPost]
        public JsonResult ChangeBillingRoom(BillingRoomViewModel billingRoomViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int billingID = this.GetActiveBill(billingRoomViewModel.RoomID);
                    if (billingID != billingRoomViewModel.BillingID) throw new Exception("Không thể chuyển phòng này. Xin vui lòng kiểm tra lại trước khi tiếp tục!");

                    db.SetBillingRoom(billingRoomViewModel.BillingID, billingRoomViewModel.RoomID, billingRoomViewModel.NewRoomID, billingRoomViewModel.Description);

                    return Json(new { Success = 1, BillingID = billingRoomViewModel.BillingID, ex = "" });
                }
                else throw new Exception("Dữ liệu chưa nhập đầy đủ, vui lòng kiểm tra lại.");
            }
            catch (Exception ex)
            {
                return Json(new { Success = 0, ex = ex.Message.ToString() });
            }
        }

        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            BillingMaster billingMaster = db.BillingMasters.Find(id);

            if (billingMaster == null) return HttpNotFound();

            var viewModel = new BillingViewModels()
            {
                BillingMaster = billingMaster,
                BillingDetailFulls = db.GetBillingDetailFull(id).ToList()
            };

            return PartialView(billingMaster.ListRoom.ListRoomType.RoomCategoryID == (int)SettingsManager.RoomCategoryRoomService ? "Details" : "DetailsTable", viewModel);
        }

        // GET: Home/Billing/5 - id: RoomID
        //[ValidateAntiForgeryToken]
        public ActionResult Billing(int roomCategoryID, int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            int billingID = this.GetActiveBill(id);

            BillingMaster billingMaster;


            if (roomCategoryID == (int)SettingsManager.RoomCategoryRoomService && billingID <= 0)
            {
                billingMaster = NewBillingMaster(roomCategoryID, (int)id);

                ViewBag.roomCategoryID = roomCategoryID;
                ViewBag.hotelID = db.ListRooms.FirstOrDefault(room => room.RoomID == id).HotelID;

                return PartialView("Create", billingMaster);

            }
            else
            {
                if (billingID <= 0)
                    billingMaster = NewBillingMaster(roomCategoryID, (int)id);
                else
                {
                    billingMaster = db.BillingMasters.Find(billingID);
                    if (billingMaster == null) { return HttpNotFound(); }
                }

                billingMaster.DepartureDate = DateTime.Now;

                if (roomCategoryID == (int)SettingsManager.RoomCategoryRoomService)
                {
                    var jSonChargeRate = this.GetDurationAmount(billingMaster.RoomID, billingMaster.ChargeTypeID, billingMaster.ArrivalDate, (DateTime)billingMaster.DepartureDate);
                    dynamic chargeRate = JObject.Parse(jSonChargeRate.Data.ToString().Replace("=", ":"));
                    billingMaster.ChargeDuration = chargeRate.ChargeDuration;
                    billingMaster.ChargeAmount = chargeRate.ChargeAmount;
                }


                var viewModel = new BillingViewModels()
                {
                    RoomCategoryID = roomCategoryID,
                    HotelID = db.ListRooms.FirstOrDefault(room => room.RoomID == id).HotelID,

                    BillingMaster = billingMaster,
                    BillingDetailFulls = billingID <= 0 ? new List<BillingDetailFull>() : db.GetBillingDetailFull(billingID).ToList()
                };

                return PartialView(roomCategoryID == (int)SettingsManager.RoomCategoryRoomService ? "Edit" : "EditTable", viewModel);
            }

        }


        public BillingMaster NewBillingMaster(int roomCategoryID, int roomID)
        {
            BillingMaster billingMaster = new BillingMaster();

            billingMaster.RoomID = roomID;
            billingMaster.BillingID = 0;
            billingMaster.BillingReference = "#";
            billingMaster.BillingDate = DateTime.Now;
            billingMaster.ArrivalDate = DateTime.Now;

            if (roomCategoryID != (int)SettingsManager.RoomCategoryRoomService)
            {
                billingMaster.CustomerName = "#";
                billingMaster.CustomerIdentityNo = "#";
            }

            billingMaster.ChargeTypeID = 2;

            billingMaster.ChargeDuration = 0;
            billingMaster.ChargeAmount = 0;
            billingMaster.ServiceAmount = 0;
            billingMaster.OtherAmount = 0;
            billingMaster.DiscountPercent = 0;
            billingMaster.DiscountAmount = 0;
            billingMaster.TotalAmount = 0;
            billingMaster.AdvancePayment = 0;

            billingMaster.IsPaidByATMCard = false;
            billingMaster.IsCheckOut = false;

            return billingMaster;
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(BillingMaster billingmaster)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.BillingMasters.Add(billingmaster);

                        if (billingmaster.BookingID != null && billingmaster.BookingID != 0)
                        {
                            BookingMaster bookingMaster = db.BookingMasters.Find(billingmaster.BookingID);
                            if (bookingMaster != null)
                            {
                                bookingMaster.InActive = true;
                                db.Entry(bookingMaster).State = EntityState.Modified;
                            }
                            else throw new Exception("Fail to find booking obejct to set inactive");
                        }

                        var billingMasterFind = db.BillingMasters.Where(w => w.RoomID == billingmaster.RoomID && w.IsCheckOut == false);

                        if (billingMasterFind.Count() <= 0)
                            db.SaveChanges();

                        dbContextTransaction.Commit();

                        return Json(new { Success = 1, BillingID = billingmaster.BillingID, ex = "" });
                    }
                    else throw new Exception(this.GetModelStateError());
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();// If Sucess== 0 then Unable to perform Save/Update Operation and send Exception to View as JSON
                    return Json(new { Success = 0, ex = ex.Message.ToString() });
                }
            }
        }




        [HttpPost]
        public JsonResult Save(BillingMaster billingmaster)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        // Perform Save for NON SettingsManager.RoomCategoryRoomService (Coffee)
                        if (billingmaster.BillingID <= 0)
                        {
                            var billingMasterFind = db.BillingMasters.Where(w => w.RoomID == billingmaster.RoomID && w.IsCheckOut == false);

                            if (billingMasterFind.Count() > 0) throw new System.ArgumentException("", "Bàn đã có khách.");

                            db.BillingMasters.Add(billingmaster);
                        }
                        else
                        {
                            var currentBillingMaster = db.BillingMasters.Where(b => b.BillingID == billingmaster.BillingID && !b.IsCheckOut);
                            if (currentBillingMaster.Count() <= 0) throw new System.ArgumentException("", "Khách thuê đã trả phòng.");

                            this.UpdateWarehouseBalance(billingmaster, GlobalEnums.UpdateWarehouseBalanceOption.Add);


                            var CurrentBillingDetail = db.BillingDetails.Where(p => p.BillingID == billingmaster.BillingID);
                            foreach (BillingDetail ss in CurrentBillingDetail)
                                db.BillingDetails.Remove(ss);

                            db.SetRoomStatus(billingmaster.RoomID, 3);
                        }


                        int serialID = 0;
                        foreach (BillingDetail ss in billingmaster.BillingDetails)
                        {
                            ss.SerialID = serialID++;
                            db.BillingDetails.Add(ss);
                        }

                        if (billingmaster.BillingID > 0)
                            db.Entry(billingmaster).State = EntityState.Modified;

                        db.SaveChanges();

                        this.UpdateWarehouseBalance(billingmaster, GlobalEnums.UpdateWarehouseBalanceOption.Minus);

                        this.CheckOverStock(billingmaster.DepartureDate.Value.Date, 0, billingmaster.BillingID);

                        dbContextTransaction.Commit();

                        return Json(new { Success = 1, BillingID = billingmaster.BillingID, ex = "" });// If Sucess== 1 then Save/Update Successfull else there it has Exception
                    }
                    else throw new Exception(this.GetModelStateError());
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return Json(new { Success = 0, ex = ex.Message.ToString() });
                }
            }
        }


        [HttpPost]

        public JsonResult GetDurationAmount(int roomID, int chargeTypeID, DateTime arrivalDate, DateTime departureDate)
        {
            try
            {
                var roomTypeID = db.ListRooms.Where(r => r.RoomID == roomID).Single().RoomTypeID;
                var listChargeType = db.ListChargeTypes.Where(ct => ct.ChargeTypeID == chargeTypeID).First();

                if (listChargeType == null) throw new Exception("Tính toán tiền phòng: Lỗi không xác định. Vui lòng kiểm tra lại.");

                //Calculate charge voulumn
                double chargeVolumn = 0; double chargeExtraValue = 0;
                if (chargeTypeID == 2) //By Day
                {
                    DateTime checkinDate = new DateTime(arrivalDate.Year, arrivalDate.Month, arrivalDate.Day, 12, 0, 0);
                    DateTime checkoutDate = new DateTime(departureDate.Year, departureDate.Month, departureDate.Day, 12, 0, 0);

                    if (checkoutDate != checkinDate)
                    {
                        if (departureDate > checkoutDate.AddMinutes(listChargeType.GraceMinutes))
                            checkoutDate = checkoutDate.AddDays(1);
                        else

                            //Hard code to set chargeExtraValue: After 13.00: add 30%, after 15.00: add 60%
                            if (departureDate > checkoutDate.AddMinutes(180))
                                chargeExtraValue = 0.5;
                            else
                                if (departureDate > checkoutDate.AddMinutes(60))
                                    chargeExtraValue = 0.3;
                                else
                                    chargeExtraValue = 0;

                    }

                    chargeVolumn = checkoutDate == checkinDate ? 1 : checkoutDate.Subtract(checkinDate).TotalDays;
                }
                else //By Block of hour(s)
                {
                    if (departureDate <= arrivalDate.AddMinutes(listChargeType.GraceMinutes))
                        chargeVolumn = 1;
                    else
                    {
                        TimeSpan span = departureDate - arrivalDate;
                        double totalBlock = span.TotalHours / listChargeType.HoursPerBlock;
                        chargeVolumn = (int)totalBlock + ((totalBlock - (int)totalBlock) * listChargeType.HoursPerBlock * 60 > listChargeType.GraceMinutes ? 1 : 0);
                    }
                }


                //Get the charge rate
                var volumnChargeRates = db.GetChargeRate(roomTypeID, chargeTypeID, (int)chargeVolumn).ToList();

                if (volumnChargeRates != null)
                {
                    var volumnChargeRate = volumnChargeRates.First();
                    var chagrgeVolumnNext = ((int)chargeVolumn - volumnChargeRate.ChargeVolumn - volumnChargeRate.ChargeVolumnFirst); //DO BO SUNG ChargeVolumnFirst => VI VAY: ((int)chargeVolumn - volumnChargeRate.ChargeVolumn - volumnChargeRate.ChargeVolumnFirst) CO THE < 0 (BOI VI db.GetChargeRate CHI LAY DIEU KIEN: ChargeVolumn <= @ChargeVolumn)
                    return Json(new
                    {
                        ChargeDuration = (int)chargeVolumn,
                        ChargeAmount = volumnChargeRate.ChargeRate + (chagrgeVolumnNext > 0 ? chagrgeVolumnNext * volumnChargeRate.ChargeRateUpper : 0) + chargeExtraValue * volumnChargeRate.ChargeRate
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new
                    {
                        ChargeDuration = 0,
                        ChargeAmount = 0
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        private void UpdateWarehouseBalance(BillingMaster entity, GlobalEnums.UpdateWarehouseBalanceOption updateWarehouseBalanceOption)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("UpdateWarehouseBalanceOption", (int)updateWarehouseBalanceOption), new ObjectParameter("PurchaseInvoiceID", 0), new ObjectParameter("BillingID", entity.BillingID) };

            ((IObjectContextAdapter)db).ObjectContext.ExecuteFunction("UpdateWarehouseBalance", parameters);
        }

        private bool CheckOverStock(DateTime? actionDate, int? purchaseInvoiceID, int? billingID)
        {
            WarehouseRepository warehouseRepository = new WarehouseRepository(this.db);
            return warehouseRepository.CheckOverStock(actionDate, purchaseInvoiceID, billingID);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //
        // GET: /Home/HotelMenu
        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult HotelMenu()
        {
            var listHotels = db.ListHotels.ToList().OrderBy(o => o.HotelOrderNo);

            return PartialView(listHotels);
        }

        public JsonResult GetBillingDetail_Read([DataSourceRequest] DataSourceRequest dataSourceRequest)
        {

            //db.Configuration.ProxyCreationEnabled = false;
            int id = Home.GetHotelID(this.HttpContext);
            var billingLists = db.GetBillingList(User.Identity.GetUserId(), id).ToList();
            //db.Configuration.ProxyCreationEnabled = true;

            return Json(billingLists.ToDataSourceResult(dataSourceRequest));
        }

        //public ActionResult GetOrderDetails1([DataSourceRequest] DataSourceRequest dataSourceRequest)
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    var listChargeTypesList = db.ListChargeTypes;
        //    return Json(listChargeTypesList.ToDataSourceResult(dataSourceRequest));
        //}

        //public JsonResult GetOrderDetails([DataSourceRequest] DataSourceRequest dataSourceRequest)
        //{
        //    Mapper.CreateMap<ListChargeType, ListChargeTypeViewModel>();
        //    db.Configuration.ProxyCreationEnabled = false;
        //    IQueryable<ListChargeType> listChargeTypes = db.ListChargeTypes;
        //    return Json(listChargeTypes.ToDataSourceResult(dataSourceRequest, Mapper.Map<ListChargeTypeViewModel>), JsonRequestBehavior.AllowGet);
        //}

        private string GetModelStateError()
        {
            var query = from state in ModelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            return "Dữ liệu chưa nhập đầy đủ, vui lòng kiểm tra lại.\n\n" + string.Join("\n", query.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            schedulerBookingService.Dispose();
            schedulerBillingService.Dispose();
            base.Dispose(disposing);
        }

    }
}