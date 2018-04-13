namespace MVCCore.Enums
{
    public static class GlobalEnums
    {
        public static int rndQuantity = 0;
        public static int rndAmount = 0;

        public enum NMVNTaskID
        {
            PurchaseInvoice = 1,
            BillingMaster = 2
        };

        public enum GoodsReceiptVoucherTypeID
        {
            PurchaseInvoice = 1,
            GoodsReturn = 2,
            GoodsTransfer = 3,
            InventoryAdjustment = 4
        };

        public enum UpdateWarehouseBalanceOption
        {
            Add = 1,
            Minus = -1
        };


    }
}
