﻿using MVCModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace MVCDTO.CommonTasks
{
    public class ListChargeRatePrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public int GetID() { return this.ChargeRateID; }
        public void SetID(int id) { this.ChargeRateID = id; }

        public int ChargeRateID { get; set; }
        [Display(Name = "Loại phòng")]
        public int RoomTypeID { get; set; }
        [Display(Name = "Loại phòng")]
        public string ListRoomTypeDescription { get; set; }
        [Display(Name = "Đơn vị tính")]
        public int ChargeTypeID { get; set; }
        [Display(Name = "Đơn vị tính")]
        public string ListChargeTypeDescription { get; set; }
        public int ChargeVolumn { get; set; }
        [Display(Name = "Đơn giá")]
        public double ChargeRate { get; set; }
        [Display(Name = "Đơn giá phụ thu")]
        public double ChargeRateUpper { get; set; }
        [Display(Name = "Diễn giải")]
        public string Description { get; set; }
        [Display(Name = "Ghi chú")]
        public string Remarks { get; set; }
            
    }

    public class ListChargeRateDTO : ListChargeRatePrimitiveDTO
    {
        public ListChargeRateDTO()
        {
            
        }
    }   
}
