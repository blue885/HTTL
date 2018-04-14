using HotelManager.ViewModels.CommonTasks;
using MVCCore.Repositories.CommonTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders.CommonTasks
{
    public class ListChargeRateBuilder : IListChargeRateBuilder
    {
        private readonly IListChargeTypeBuilder listChargeTypeBuilder;
        private readonly IListRoomTypeBuilder listRoomTypeBuilder;
        private readonly IListChargeTypeRepository listChargeTypeRepository;
        private readonly IListRoomTypeRepository listRoomTypeRepository;        

        public ListChargeRateBuilder(IListChargeTypeBuilder listChargeTypeBuilder,
                                     IListRoomTypeBuilder listRoomTypeBuilder,
                                     IListChargeTypeRepository listChargeTypeRepository,
                                     IListRoomTypeRepository listRoomTypeRepository)
        {
            this.listChargeTypeBuilder = listChargeTypeBuilder;
            this.listRoomTypeBuilder = listRoomTypeBuilder;
            this.listChargeTypeRepository = listChargeTypeRepository;
            this.listRoomTypeRepository = listRoomTypeRepository;            
        }

        public void BuildSelectListsForListChargeRateViewModel(ListChargeRateViewModel listChargeRateViewModel)
        {
            listChargeRateViewModel.ChargeTypeSelectList = listChargeTypeBuilder.BuildSelectListItemsForListChargeTypes(listChargeTypeRepository.GetAllListChargeType());
            listChargeRateViewModel.RoomTypeSelectList = listRoomTypeBuilder.BuildSelectListItemsForListRoomTypes(listRoomTypeRepository.GetAllListRoomType());
        }
    }
}