using HotelManager.ViewModels.CommonTasks;
using MVCCore.Repositories.CommonTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManager.Builders.CommonTasks
{
    public class ListRoomBuilder : IListRoomBuilder
    {
        private readonly IListRoomStatusesBuilder listRoomStatusesBuilder;
        private readonly IHotelSelectListBuilder hotelSelectListBuilder;
        private readonly IListRoomTypeBuilder listRoomTypeBuilder;
        private readonly IListRoomStatusesRepository listRoomStatusesRepository;
        private readonly IHotelRepository hotelRepository;
        private readonly IListRoomTypeRepository listRoomTypeRepository;        

        public ListRoomBuilder(IListRoomStatusesBuilder listRoomStatusesBuilder,
                                IHotelSelectListBuilder hotelSelectListBuilder,
                                IListRoomTypeBuilder listRoomTypeBuilder,
                                IListRoomStatusesRepository listRoomStatusesRepository,
                                IHotelRepository hotelRepository,
                                IListRoomTypeRepository listRoomTypeRepository)
        {
            this.hotelSelectListBuilder = hotelSelectListBuilder;
            this.listRoomStatusesBuilder = listRoomStatusesBuilder;
            this.listRoomTypeBuilder = listRoomTypeBuilder;
            this.listRoomStatusesRepository = listRoomStatusesRepository;
            this.hotelRepository = hotelRepository;
            this.listRoomTypeRepository = listRoomTypeRepository;            
        }

        public void BuildSelectListsForListRoomViewModel(ListRoomViewModel listRoomViewModel)
        {
            listRoomViewModel.HotelSelectList = hotelSelectListBuilder.BuildSelectListItemsForHotels(hotelRepository.GetAllHotel());
            listRoomViewModel.RoomStatusesSelectList = listRoomStatusesBuilder.BuildSelectListItemsForListRoomStatusess(listRoomStatusesRepository.GetAllListRoomStatuses());
            listRoomViewModel.RoomTypeSelectList = listRoomTypeBuilder.BuildSelectListItemsForListRoomTypes(listRoomTypeRepository.GetAllListRoomType());
        }
    }
}