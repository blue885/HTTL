using MVCModel.Models;
using MVCCore.Enums;
using MVCDTO.PurchaseTasks;
using MVCCore.Repositories.PurchaseTasks;
using MVCCore.Services.PurchaseTasks;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;


namespace MVCService.PurchaseTasks
{
    public class PurchaseInvoiceService : GenericWithDetailService<PurchaseInvoice, PurchaseInvoiceDetail, PurchaseInvoiceDTO, PurchaseInvoicePrimitiveDTO, PurchaseInvoiceDetailDTO>, IPurchaseInvoiceService
    {
        
        private readonly IPurchaseInvoiceRepository PurchaseInvoiceRepository;

        public PurchaseInvoiceService(IPurchaseInvoiceRepository PurchaseInvoiceRepository)
            : base(PurchaseInvoiceRepository)
        {
            this.PurchaseInvoiceRepository = PurchaseInvoiceRepository;
        }


        public override bool Save(PurchaseInvoiceDTO purchaseInvoiceDTO)
        {
            purchaseInvoiceDTO.PurchaseInvoiceDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(purchaseInvoiceDTO);
        }


        protected override PurchaseInvoice SaveMe(PurchaseInvoiceDTO dto)
        {
            PurchaseInvoice entity = base.SaveMe(dto);

            this.UpdateWarehouseBalance(entity, GlobalEnums.UpdateWarehouseBalanceOption.Add);

            return entity;
        }      


        protected override void SaveUndo(PurchaseInvoiceDTO dto, PurchaseInvoice entity, bool IsDelete)
        {
            this.UpdateWarehouseBalance(entity, GlobalEnums.UpdateWarehouseBalanceOption.Minus);

            base.SaveUndo(dto, entity, IsDelete);
        }

        private void UpdateWarehouseBalance(PurchaseInvoice entity, GlobalEnums.UpdateWarehouseBalanceOption updateWarehouseBalanceOption)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("UpdateWarehouseBalanceOption", (int)updateWarehouseBalanceOption), new ObjectParameter("PurchaseInvoiceID", entity.PurchaseInvoiceID), new ObjectParameter("BillingID", 0) };
            int i = this.PurchaseInvoiceRepository.ExecuteFunction("UpdateWarehouseBalance", parameters);
        }


        protected override void PostSaveValidate(PurchaseInvoice entity)
        {
            this.PurchaseInvoiceRepository.CheckOverStock(entity.PurchaseInvoiceDate, entity.PurchaseInvoiceID, 0);
            base.PostSaveValidate(entity);
        }

        public void DeletePurchaseInvoice(int purchaseInvoiceID)
        {
            this.PurchaseInvoiceRepository.DeletePurchaseInvoice(purchaseInvoiceID);
        }

    }
}
