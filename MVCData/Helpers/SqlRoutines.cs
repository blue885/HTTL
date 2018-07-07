using System;
using MVCModel.Models;
using MVCCore.Enums;

namespace MVCData.Helpers
{
    public class SqlRoutines
    {
        public enum SqlType : int
        {
            StoredProcedure = 1,
            Text = 2
        }

        private readonly HotelManagerEntities totalBikePortalsEntities;

        public SqlRoutines(HotelManagerEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.UpdateWarehouseBalance();
            this.GetOverStockItems();
            this.WarehouseJournal();
        }



        private void UpdateWarehouseBalance()
        {
            //@UpdateWarehouseBalanceOption: 1 ADD, -1-MINUS
            string queryString = " @UpdateWarehouseBalanceOption Int, @PurchaseInvoiceID Int, @BillingID Int " + "\r\n";

            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";


            #region INIT DATA TO BE INPUT OR OUTPUT
            //INIT DATA TO BE INPUT OR OUTPUT.BEGIN
            queryString = queryString + "       DECLARE @ActionTable TABLE (" + "\r\n";
            queryString = queryString + "           ActionID int NOT NULL ," + "\r\n";
            queryString = queryString + "           ServiceID int NOT NULL ," + "\r\n";
            queryString = queryString + "           HotelID int NOT NULL ," + "\r\n";
            queryString = queryString + "           WarehouseBalanceDate datetime NOT NULL ," + "\r\n";
            queryString = queryString + "           Quantity float NOT NULL ," + "\r\n";
            queryString = queryString + "           AmountCost float NOT NULL ," + "\r\n";
            queryString = queryString + "           Remarks nvarchar (100))" + "\r\n";

            queryString = queryString + "       IF @PurchaseInvoiceID > 0 " + "\r\n";
            queryString = queryString + "           INSERT      @ActionTable " + "\r\n";
            queryString = queryString + "           SELECT      MIN(PurchaseInvoiceDetails.PurchaseInvoiceID), PurchaseInvoiceDetails.ServiceID, PurchaseInvoices.HotelID, MIN(PurchaseInvoices.PurchaseInvoiceDate), SUM(@UpdateWarehouseBalanceOption * PurchaseInvoiceDetails.Quantity), 0 AS AmountCost, '' AS Remarks " + "\r\n";
            queryString = queryString + "           FROM        PurchaseInvoices INNER JOIN PurchaseInvoiceDetails ON PurchaseInvoices.PurchaseInvoiceID = PurchaseInvoiceDetails.PurchaseInvoiceID  " + "\r\n";
            queryString = queryString + "           WHERE       PurchaseInvoices.PurchaseInvoiceID = @PurchaseInvoiceID " + "\r\n";
            queryString = queryString + "           GROUP BY    PurchaseInvoices.HotelID, PurchaseInvoiceDetails.ServiceID" + "\r\n";

            queryString = queryString + "       IF @BillingID > 0 " + "\r\n";
            queryString = queryString + "           INSERT      @ActionTable " + "\r\n";
            queryString = queryString + "           SELECT      MIN(BillingDetail.BillingID), BillingDetail.ServiceID, ListRoom.HotelID, MIN(BillingMaster.DepartureDate), SUM(@UpdateWarehouseBalanceOption * BillingDetail.Quantity), 0 AS AmountCost, '' AS Remarks " + "\r\n";
            queryString = queryString + "           FROM        BillingMaster INNER JOIN BillingDetail ON BillingMaster.BillingID = BillingDetail.BillingID INNER JOIN ListRoom ON BillingMaster.RoomID = ListRoom.RoomID " + "\r\n";
            queryString = queryString + "           WHERE       BillingMaster.BillingID = @BillingID " + "\r\n";
            queryString = queryString + "           GROUP BY    ListRoom.HotelID, BillingDetail.ServiceID" + "\r\n";
            //INIT DATA TO BE INPUT OR OUTPUT.END



            queryString = queryString + "       DECLARE         @WarehouseBalanceDate DateTime " + "\r\n";
            queryString = queryString + "       DECLARE         CursorWarehouseBalanceDate CURSOR LOCAL FOR SELECT MAX(WarehouseBalanceDate) AS WarehouseBalanceDate FROM @ActionTable" + "\r\n";
            queryString = queryString + "       OPEN            CursorWarehouseBalanceDate" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorWarehouseBalanceDate INTO @WarehouseBalanceDate" + "\r\n";
            queryString = queryString + "       CLOSE           CursorWarehouseBalanceDate DEALLOCATE CursorWarehouseBalanceDate " + "\r\n";
            queryString = queryString + "       IF @WarehouseBalanceDate = NULL   RETURN " + "\r\n";//Nothing to update -> Exit immediately
            #endregion



            queryString = queryString + "       DECLARE @WarehouseBalanceDateEveryMonth DateTime, @WarehouseBalanceDateMAX DateTime" + "\r\n";

            queryString = queryString + "       DECLARE         CursorWarehouseBalance CURSOR LOCAL FOR SELECT MAX(WarehouseBalanceDate) AS WarehouseBalanceDate FROM WarehouseBalanceDetail" + "\r\n";
            queryString = queryString + "       OPEN            CursorWarehouseBalance" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorWarehouseBalance INTO @WarehouseBalanceDateMAX" + "\r\n";
            queryString = queryString + "       CLOSE           CursorWarehouseBalance DEALLOCATE CursorWarehouseBalance " + "\r\n";


            queryString = queryString + "       IF @WarehouseBalanceDateMAX IS NULL SET @WarehouseBalanceDateMAX = CONVERT(Datetime, '2014-09-30 14:00:00', 120) " + "\r\n"; //--END OF SEP/ 2014: FIRT MONTH

            queryString = queryString + "       SET @WarehouseBalanceDateEveryMonth = @WarehouseBalanceDateMAX " + "\r\n";//--GET THE MAXIMUM OF WarehouseBalanceDate


            //                                  STEP 1: COPY THE SAME BALANCE/ PRICE FOR EVERY WEEKEND UP TO THE MONTH CONTAIN @WarehouseBalanceDate
            queryString = queryString + "       IF @WarehouseBalanceDate > @WarehouseBalanceDateMAX" + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               WHILE dbo.EOMONTHTIME(@WarehouseBalanceDate, 9999) >= dbo.EOMONTHTIME(@WarehouseBalanceDateEveryMonth, 1)" + "\r\n";
            queryString = queryString + "                   BEGIN" + "\r\n";
            queryString = queryString + "                       SET @WarehouseBalanceDateEveryMonth = dbo.EOMONTHTIME(@WarehouseBalanceDateEveryMonth, 1)" + "\r\n";

            queryString = queryString + "                       INSERT INTO WarehouseBalanceDetail (WarehouseBalanceDate, HotelID, ServiceID, Quantity, AmountCost, Remarks)" + "\r\n";
            queryString = queryString + "                       SELECT      @WarehouseBalanceDateEveryMonth, HotelID, ServiceID, Quantity, AmountCost, Remarks " + "\r\n";
            queryString = queryString + "                       FROM        WarehouseBalanceDetail " + "\r\n";
            queryString = queryString + "                       WHERE       WarehouseBalanceDate = @WarehouseBalanceDateMAX" + "\r\n";

            queryString = queryString + "                       INSERT INTO WarehouseBalancePrice (WarehouseBalanceDate, HotelID, ServiceID, UnitPrice) " + "\r\n";

            queryString = queryString + "                       SELECT      @WarehouseBalanceDateEveryMonth, WarehouseBalancePrice.HotelID, WarehouseBalancePrice.ServiceID, WarehouseBalanceDetail.AmountCost/ WarehouseBalanceDetail.Quantity " + "\r\n";
            queryString = queryString + "                       FROM        WarehouseBalancePrice INNER JOIN" + "\r\n";
            queryString = queryString + "                                   WarehouseBalanceDetail ON WarehouseBalancePrice.WarehouseBalanceDate = @WarehouseBalanceDateMAX AND WarehouseBalancePrice.WarehouseBalanceDate = WarehouseBalanceDetail.WarehouseBalanceDate AND WarehouseBalancePrice.HotelID = WarehouseBalanceDetail.HotelID AND WarehouseBalancePrice.ServiceID = WarehouseBalanceDetail.ServiceID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               SET @WarehouseBalanceDateMAX = @WarehouseBalanceDateEveryMonth " + "\r\n";//--SET THE MAXIMUM OF WarehouseBalanceDate
            queryString = queryString + "           END " + "\r\n";


            //                                  STEP 2: UPDATE NEW QUANTITY FOR THESE ITEMS CURRENTLY EXIST IN WarehouseBalanceDetail
            queryString = queryString + "       UPDATE  WarehouseBalanceDetail" + "\r\n";//NO NEED TO UPDATE AmountCost. It will be calculated later in the process
            queryString = queryString + "       SET     WarehouseBalanceDetail.Quantity = WarehouseBalanceDetail.Quantity + ActionTable.Quantity" + "\r\n";
            queryString = queryString + "       FROM    WarehouseBalanceDetail INNER JOIN" + "\r\n";
            queryString = queryString + "               @ActionTable ActionTable ON WarehouseBalanceDetail.HotelID = ActionTable.HotelID AND WarehouseBalanceDetail.ServiceID = ActionTable.ServiceID AND WarehouseBalanceDetail.WarehouseBalanceDate >= @WarehouseBalanceDate" + "\r\n";



            //                                  STEP 3: INSERT INTO WarehouseBalanceDetail FOR THESE ITEMS NOT CURRENTLY EXIST IN WarehouseBalanceDetail
            queryString = queryString + "       SET     @WarehouseBalanceDateEveryMonth = dbo.EOMONTHTIME(@WarehouseBalanceDate, 9999)" + "\r\n";//--FIND THE FIRST @WarehouseBalanceDateEveryMonth WHICH IS GREATER OR EQUAL TO @WarehouseBalanceDate

            queryString = queryString + "       WHILE @WarehouseBalanceDateEveryMonth <= @WarehouseBalanceDateMAX" + "\r\n";
            queryString = queryString + "           BEGIN" + "\r\n";

            queryString = queryString + "               INSERT INTO     WarehouseBalanceDetail (WarehouseBalanceDate, HotelID, ServiceID, Quantity, AmountCost, Remarks)" + "\r\n";
            queryString = queryString + "               SELECT          @WarehouseBalanceDateEveryMonth, ActionTable.HotelID, ActionTable.ServiceID, ActionTable.Quantity, ActionTable.AmountCost, ActionTable.Remarks" + "\r\n";
            queryString = queryString + "               FROM            @ActionTable ActionTable LEFT JOIN" + "\r\n";
            queryString = queryString + "                               WarehouseBalanceDetail ON ActionTable.HotelID = WarehouseBalanceDetail.HotelID AND ActionTable.ServiceID = WarehouseBalanceDetail.ServiceID AND WarehouseBalanceDetail.WarehouseBalanceDate = @WarehouseBalanceDateEveryMonth" + "\r\n";
            queryString = queryString + "               WHERE           WarehouseBalanceDetail.ServiceID IS NULL " + "\r\n"; //--ADD NOT-IN-LIST ITEM"

            queryString = queryString + "               SET     @WarehouseBalanceDateEveryMonth = dbo.EOMONTHTIME(@WarehouseBalanceDateEveryMonth, 1)" + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       DELETE FROM WarehouseBalanceDetail WHERE Quantity = 0 " + "\r\n";



            #region Update Warehouse balance average price + ending amount

            queryString = queryString + "       DECLARE     @LastDayOfPreviousMonth DateTime" + "\r\n";
            queryString = queryString + "       DECLARE     @WarehouseInputCollection TABLE (HotelID int NOT NULL, ServiceID int NOT NULL, Quantity float NOT NULL, PurchaseInvoiceQuantity float NOT NULL, AmountCost float NOT NULL, UnitPrice float NOT NULL) " + "\r\n";
            queryString = queryString + "       DECLARE     @WarehouseInputAveragePrice TABLE (HotelID int NOT NULL, ServiceID int NOT NULL, UnitPrice float NOT NULL) " + "\r\n";

            queryString = queryString + "       SET         @WarehouseBalanceDateEveryMonth = dbo.EOMONTHTIME(@WarehouseBalanceDate, 9999)" + "\r\n";//--FIND THE FIRST @WarehouseBalanceDateEveryMonth WHICH IS GREATER OR EQUAL TO @WarehouseBalanceDate



            queryString = queryString + "       DECLARE     @NeedToGenerateAverageUnitPrice bit " + "\r\n"; //DELETE and RE-UPDATE WarehouseBalancePrice OF THESE ITEMS INCLUDE IN @ActionTable WHEN (@PurchaseInvoiceID > 0 AND GoodsReceiptTypeID = GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice) (MEAN WHEN WAREHOUSE INPUT BY PurchaseInvoice)   OR   (@BillingID AND @WarehouseBalanceDateEveryMonth < @WarehouseBalanceDateMAX) (MEAN WAREHOUSE OUTPUT OCCUR ON THE MONTH BEFORE THE LAST MONTH) IN ORDER TO RECALCULATE WarehouseBalancePrice
            queryString = queryString + "       SET         @NeedToGenerateAverageUnitPrice = IIF(@PurchaseInvoiceID > 0, 1, IIF (@BillingID > 0 AND @WarehouseBalanceDateEveryMonth < @WarehouseBalanceDateMAX, 1, 0)) " + "\r\n";


            queryString = queryString + "       IF @NeedToGenerateAverageUnitPrice = 1 " + "\r\n";
            queryString = queryString + "                   DELETE FROM WarehouseBalancePrice FROM WarehouseBalancePrice INNER JOIN @ActionTable ActionTable ON WarehouseBalancePrice.WarehouseBalanceDate >= @WarehouseBalanceDateEveryMonth AND WarehouseBalancePrice.HotelID = ActionTable.HotelID AND WarehouseBalancePrice.ServiceID = ActionTable.ServiceID " + "\r\n"; //(USING @ActionTable AS A FILTER)



            queryString = queryString + "       WHILE @WarehouseBalanceDateEveryMonth <= @WarehouseBalanceDateMAX" + "\r\n";
            queryString = queryString + "           BEGIN" + "\r\n";

            queryString = queryString + "               SET     @LastDayOfPreviousMonth = dbo.EOMONTHTIME(@WarehouseBalanceDateEveryMonth, -1) " + "\r\n";

            //                                          STEP 1: GET COLLECTION OF BEGIN BALANCE + INPUT (Quantity, AmountCost). Note: PurchaseInvoiceQuantity AND AmountCost effected by BEGINING + INPUT FOR GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice ONLY
            queryString = queryString + "               INSERT INTO     @WarehouseInputCollection (HotelID, ServiceID, Quantity, PurchaseInvoiceQuantity, AmountCost, UnitPrice) " + "\r\n";
            queryString = queryString + "               SELECT          HotelID, ServiceID, SUM(Quantity), SUM(PurchaseInvoiceQuantity), SUM(AmountCost), 0 AS UnitPrice " + "\r\n";

            queryString = queryString + "               FROM            (" + "\r\n";

            queryString = queryString + "                               SELECT      WarehouseBalanceDetail.HotelID, WarehouseBalanceDetail.ServiceID, WarehouseBalanceDetail.Quantity, WarehouseBalanceDetail.Quantity AS PurchaseInvoiceQuantity, WarehouseBalanceDetail.AmountCost AS AmountCost" + "\r\n";
            queryString = queryString + "                               FROM        WarehouseBalanceDetail INNER JOIN" + "\r\n"; //BEGIN BALANCE (USING @ActionTable AS A FILTER)
            queryString = queryString + "                                           @ActionTable ActionTable ON WarehouseBalanceDetail.HotelID = ActionTable.HotelID AND WarehouseBalanceDetail.ServiceID = ActionTable.ServiceID AND WarehouseBalanceDetail.WarehouseBalanceDate = @LastDayOfPreviousMonth " + "\r\n";

            queryString = queryString + "                               UNION ALL " + "\r\n";

            queryString = queryString + "                               SELECT      ActionTable.HotelID, ActionTable.ServiceID, PurchaseInvoiceDetails.Quantity, PurchaseInvoiceDetails.Quantity AS PurchaseInvoiceQuantity, ROUND(PurchaseInvoiceDetails.GrossAmount, " + (int)GlobalEnums.rndAmount + ") AS AmountCost" + "\r\n";
            queryString = queryString + "                               FROM        PurchaseInvoices INNER JOIN" + "\r\n";  //INPUT (USING @ActionTable AS A FILTER) + AND PLEASE SPECIAL CONSIDER TO THIS CONDITION CLAUSE: (@UpdateWarehouseBalanceOption = " + GlobalEnums.UpdateWarehouseBalanceOption.Add + " OR PurchaseInvoices.PurchaseInvoiceID <> @PurchaseInvoiceID)      (MEANS: @PurchaseInvoiceID <> 0 AND WHEN GlobalEnums.UpdateWarehouseBalanceOption.Minus: DON'T INCLUDE THIS PurchaseInvoice TO THE COLLECTION FOR CALCULATE THE PRICE + ENDING AMOUNT)
            queryString = queryString + "                                           PurchaseInvoiceDetails ON (@UpdateWarehouseBalanceOption = " + (int)GlobalEnums.UpdateWarehouseBalanceOption.Add + " OR PurchaseInvoices.PurchaseInvoiceID <> @PurchaseInvoiceID) AND PurchaseInvoices.PurchaseInvoiceID = PurchaseInvoiceDetails.PurchaseInvoiceID INNER JOIN " + "\r\n";
            queryString = queryString + "                                           @ActionTable ActionTable ON PurchaseInvoices.HotelID = ActionTable.HotelID AND PurchaseInvoiceDetails.ServiceID = ActionTable.ServiceID AND PurchaseInvoices.PurchaseInvoiceDate > @LastDayOfPreviousMonth AND PurchaseInvoices.PurchaseInvoiceDate <= @WarehouseBalanceDateEveryMonth" + "\r\n";

            queryString = queryString + "                               )WarehouseJournalUnion" + "\r\n";

            queryString = queryString + "               GROUP BY        HotelID, ServiceID " + "\r\n";
            queryString = queryString + "               HAVING          SUM(Quantity) <> 0 " + "\r\n";






            //                                          STEP 2: GENERATE AVERAGE UNIT PRICE FOR THE MONTH (IF NEEDED)--- THE PRICE HERE IS THE SAME PRICE AT: STEP 2.1: UPDATE AVERAGE PRICE
            queryString = queryString + "               IF @NeedToGenerateAverageUnitPrice = 1 " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";

            
            //                                                  A: CALCULATE THE NEW AveragePrice OF THE MONTH
            queryString = queryString + "                       INSERT INTO     @WarehouseInputAveragePrice (HotelID, ServiceID, UnitPrice) " + "\r\n";
            queryString = queryString + "                       SELECT          HotelID, ServiceID, SUM(AmountCost)/ SUM(PurchaseInvoiceQuantity) " + "\r\n";
            queryString = queryString + "                       FROM            @WarehouseInputCollection " + "\r\n";
            queryString = queryString + "                       GROUP BY        HotelID, ServiceID " + "\r\n";




            //                                                  B: SAVE THE NEW AveragePrice
            queryString = queryString + "                       INSERT INTO     WarehouseBalancePrice (WarehouseBalanceDate, HotelID, ServiceID, UnitPrice) " + "\r\n";
            queryString = queryString + "                       SELECT          @WarehouseBalanceDateEveryMonth, HotelID, ServiceID, UnitPrice " + "\r\n";
            queryString = queryString + "                       FROM            @WarehouseInputAveragePrice" + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //@NeedToGenerateAverageUnitPrice = 0 
            queryString = queryString + "                   BEGIN " + "\r\n";

            //                                                  A: GET THE CURRENT SAVED AveragePrice OF THE MONTH
            queryString = queryString + "                       INSERT INTO     @WarehouseInputAveragePrice (HotelID, ServiceID, UnitPrice) " + "\r\n";
            queryString = queryString + "                       SELECT          WarehouseBalancePrice.HotelID, WarehouseBalancePrice.ServiceID, WarehouseBalancePrice.UnitPrice " + "\r\n";
            queryString = queryString + "                       FROM            WarehouseBalancePrice INNER JOIN " + "\r\n";
            queryString = queryString + "                                       @ActionTable ActionTable ON WarehouseBalancePrice.WarehouseBalanceDate = @WarehouseBalanceDateEveryMonth AND WarehouseBalancePrice.HotelID = ActionTable.HotelID AND WarehouseBalancePrice.ServiceID = ActionTable.ServiceID " + "\r\n";

            //                                                  B: REMOVE ROW WITH (HotelID, ServiceID) NO NEED TO UPDATE ENDING CODE (USING @ActionTable AS FILTER)
            queryString = queryString + "                       DELETE          WarehouseInputCollection " + "\r\n"; //Stored nay copy tu TotalBikePortals, vi vay: tinh huong nay se xay ra (WarehouseInputCollection se bao gom tat ca cac kho - so di phai lay tat ca cac kho vi AveragePrice duoc tinh toan tu du lieu cua tat ca cac CHI NHANH - tuc tat ca cac KHO  => Vi le do: se phai xoa bot nhung row khong cua nhung KHO khong can update). TUY NHIEN, doi voi Hotelmanager thi se khong co row nao bi delete!!!
            queryString = queryString + "                       FROM            @WarehouseInputCollection WarehouseInputCollection LEFT JOIN " + "\r\n";
            queryString = queryString + "                                       @ActionTable ActionTable ON WarehouseInputCollection.HotelID = ActionTable.HotelID AND WarehouseInputCollection.ServiceID = ActionTable.ServiceID " + "\r\n";
            queryString = queryString + "                       WHERE           ActionTable.HotelID IS NULL " + "\r\n"; //--ADD NOT-IN-LIST ITEM"
            queryString = queryString + "                   END " + "\r\n";


            //                                          STEP 2.1: UPDATE AVERAGE PRICE FOR THESE HotelID, ServiceID IN @WarehouseInputCollection
            queryString = queryString + "               UPDATE          WarehouseInputCollection " + "\r\n";
            queryString = queryString + "               SET             WarehouseInputCollection.UnitPrice = WarehouseInputAveragePrice.UnitPrice " + "\r\n";
            queryString = queryString + "               FROM            @WarehouseInputCollection WarehouseInputCollection INNER JOIN " + "\r\n";
            queryString = queryString + "                               @WarehouseInputAveragePrice WarehouseInputAveragePrice ON WarehouseInputCollection.HotelID = WarehouseInputAveragePrice.HotelID AND WarehouseInputCollection.ServiceID = WarehouseInputAveragePrice.ServiceID " + "\r\n";




            //                                          STEP 3: RECALCULATE END BALANCE (AmountCost)
            queryString = queryString + "               UPDATE          WarehouseBalanceDetail " + "\r\n";
            queryString = queryString + "               SET             WarehouseBalanceDetail.AmountCost = ROUND(WarehouseBalanceAmount.AmountCost, 0) " + "\r\n";
            queryString = queryString + "               FROM            WarehouseBalanceDetail INNER JOIN " + "\r\n";
            queryString = queryString + "                              (SELECT          HotelID, ServiceID, SUM(Quantity) AS Quantity, SUM(AmountCost) AS AmountCost " + "\r\n";

            queryString = queryString + "                               FROM           (" + "\r\n";

            queryString = queryString + "                                               SELECT      WarehouseInputCollection.HotelID, WarehouseInputCollection.ServiceID, WarehouseInputCollection.Quantity, ROUND(WarehouseInputCollection.Quantity * WarehouseInputCollection.UnitPrice, " + (int)GlobalEnums.rndAmount + ") AS AmountCost" + "\r\n";
            queryString = queryString + "                                               FROM        @WarehouseInputCollection WarehouseInputCollection " + "\r\n";

            queryString = queryString + "                                               UNION ALL " + "\r\n";

            queryString = queryString + "                                               SELECT      WarehouseInputCollection.HotelID, WarehouseInputCollection.ServiceID, -BillingDetail.Quantity AS Quantity, -ROUND(BillingDetail.Quantity * WarehouseInputCollection.UnitPrice, " + (int)GlobalEnums.rndAmount + ") AS AmountCost" + "\r\n";
            queryString = queryString + "                                               FROM        BillingMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                                           BillingDetail ON BillingMaster.BillingID = BillingDetail.BillingID INNER JOIN " + "\r\n";
            queryString = queryString + "                                                           ListRoom ON BillingMaster.RoomID = ListRoom.RoomID INNER JOIN  " + "\r\n";
            queryString = queryString + "                                                           @WarehouseInputCollection WarehouseInputCollection ON ListRoom.HotelID = WarehouseInputCollection.HotelID AND BillingDetail.ServiceID = WarehouseInputCollection.ServiceID AND BillingMaster.DepartureDate > @LastDayOfPreviousMonth AND BillingMaster.DepartureDate <= @WarehouseBalanceDateEveryMonth" + "\r\n";

            queryString = queryString + "                                              )WarehouseBalanceUnion" + "\r\n";

            queryString = queryString + "                               GROUP BY        HotelID, ServiceID " + "\r\n";

            queryString = queryString + "                              )WarehouseBalanceAmount ON WarehouseBalanceDetail.WarehouseBalanceDate = @WarehouseBalanceDateEveryMonth AND WarehouseBalanceDetail.HotelID = WarehouseBalanceAmount.HotelID AND WarehouseBalanceDetail.ServiceID = WarehouseBalanceAmount.ServiceID " + "\r\n";


            //                                          STEP 4: INIT VARIBLE FOR NEW LOOP
            queryString = queryString + "               DELETE FROM     @WarehouseInputCollection " + "\r\n";
            queryString = queryString + "               DELETE FROM     @WarehouseInputAveragePrice " + "\r\n";
            queryString = queryString + "               SET     @WarehouseBalanceDateEveryMonth = dbo.EOMONTHTIME(@WarehouseBalanceDateEveryMonth, 1)" + "\r\n";

            queryString = queryString + "           END " + "\r\n";

            #endregion Update Warehouse balance average price + ending amount


            queryString = queryString + "   END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("UpdateWarehouseBalance", queryString);
        }



        private void UpdateWarehouseBalance_OLD()
        {
            //@UpdateWarehouseBalanceOption: 1 ADD, -1-MINUS
            string queryString = " @UpdateWarehouseBalanceOption Int, @PurchaseInvoiceID Int, @BillingID Int " + "\r\n";

            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            #region INIT DATA TO BE INPUT OR OUTPUT
            //INIT DATA TO BE INPUT OR OUTPUT.BEGIN
            queryString = queryString + "       DECLARE @ActionTable TABLE (" + "\r\n";
            queryString = queryString + "           ActionID int NOT NULL ," + "\r\n";
            queryString = queryString + "           ServiceID int NOT NULL ," + "\r\n";
            queryString = queryString + "           HotelID int NOT NULL ," + "\r\n";
            queryString = queryString + "           ActionDate datetime NOT NULL ," + "\r\n";
            queryString = queryString + "           Quantity float NOT NULL ," + "\r\n";
            queryString = queryString + "           AmountCost float NOT NULL ," + "\r\n";
            queryString = queryString + "           Remarks nvarchar (100))" + "\r\n";

            queryString = queryString + "       IF @PurchaseInvoiceID > 0 " + "\r\n";
            queryString = queryString + "           INSERT      @ActionTable " + "\r\n";
            queryString = queryString + "           SELECT      MIN(PurchaseInvoiceDetails.PurchaseInvoiceID), PurchaseInvoiceDetails.ServiceID, PurchaseInvoices.HotelID, MIN(PurchaseInvoices.PurchaseInvoiceDate), SUM(CASE @UpdateWarehouseBalanceOption WHEN 1 THEN PurchaseInvoiceDetails.Quantity ELSE -PurchaseInvoiceDetails.Quantity END), SUM(CASE @UpdateWarehouseBalanceOption WHEN 1 THEN PurchaseInvoiceDetails.GrossAmount ELSE -PurchaseInvoiceDetails.GrossAmount END), '' AS Remarks " + "\r\n";
            queryString = queryString + "           FROM        PurchaseInvoices INNER JOIN PurchaseInvoiceDetails ON PurchaseInvoices.PurchaseInvoiceID = PurchaseInvoiceDetails.PurchaseInvoiceID " + "\r\n";
            queryString = queryString + "           WHERE       PurchaseInvoices.PurchaseInvoiceID = @PurchaseInvoiceID " + "\r\n";
            queryString = queryString + "           GROUP BY    PurchaseInvoices.HotelID, PurchaseInvoiceDetails.ServiceID" + "\r\n";

            queryString = queryString + "       IF @BillingID > 0 " + "\r\n";
            queryString = queryString + "           INSERT      @ActionTable " + "\r\n";
            queryString = queryString + "           SELECT      MIN(BillingDetail.BillingID), BillingDetail.ServiceID, ListRoom.HotelID, MIN(BillingMaster.DepartureDate), SUM(CASE @UpdateWarehouseBalanceOption WHEN 1 THEN BillingDetail.Quantity ELSE -BillingDetail.Quantity END), 0 AS AmountCost, '' AS Remarks " + "\r\n";
            queryString = queryString + "           FROM        BillingMaster INNER JOIN BillingDetail ON BillingMaster.BillingID = BillingDetail.BillingID INNER JOIN ListRoom ON BillingMaster.RoomID = ListRoom.RoomID " + "\r\n";
            queryString = queryString + "           WHERE       BillingMaster.BillingID = @BillingID " + "\r\n";
            queryString = queryString + "           GROUP BY    HotelID, ServiceID" + "\r\n";
            //INIT DATA TO BE INPUT OR OUTPUT.END



            queryString = queryString + "       DECLARE @ActionDate DateTime " + "\r\n";
            queryString = queryString + "       DECLARE CursorActionDate CURSOR LOCAL FOR SELECT MAX(ActionDate) AS ActionDate FROM @ActionTable" + "\r\n";
            queryString = queryString + "       OPEN CursorActionDate" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorActionDate INTO @ActionDate" + "\r\n";
            queryString = queryString + "       CLOSE CursorActionDate DEALLOCATE CursorActionDate " + "\r\n";
            queryString = queryString + "       IF @ActionDate = NULL   Return " + "\r\n";//Nothing to update -> Exit
            #endregion

            queryString = queryString + "       DECLARE @WarehouseBalanceEveryMonth DateTime, @WarehouseBalanceDateMAX DateTime" + "\r\n";

            queryString = queryString + "       DECLARE CursorWarehouseBalance CURSOR LOCAL FOR SELECT MAX(WarehouseBalanceDate) AS WarehouseBalanceDate FROM WarehouseBalanceDetail" + "\r\n";
            queryString = queryString + "       OPEN CursorWarehouseBalance" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorWarehouseBalance INTO @WarehouseBalanceDateMAX" + "\r\n";
            queryString = queryString + "       CLOSE CursorWarehouseBalance DEALLOCATE CursorWarehouseBalance " + "\r\n";


            queryString = queryString + "       IF @WarehouseBalanceDateMAX IS NULL SET @WarehouseBalanceDateMAX = CONVERT(Datetime, '2014-09-30 14:00:00', 120) " + "\r\n"; //--END OF SEP/ 2014: FIRT MONTH

            queryString = queryString + "       SET @WarehouseBalanceEveryMonth = @WarehouseBalanceDateMAX " + "\r\n";//--GET THE MAXIMUM OF WarehouseBalanceDate


            //                                  STEP 1: COPY THE SAME BALANCE/ PRICE FOR EVERY WEEKEND UP TO THE MONTH CONTAIN @ActionDate
            queryString = queryString + "       IF @ActionDate > @WarehouseBalanceDateMAX" + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               WHILE dbo.EOMONTHTIME(@ActionDate, 9999) >= dbo.EOMONTHTIME(@WarehouseBalanceEveryMonth, 1)" + "\r\n";
            queryString = queryString + "                   BEGIN" + "\r\n";
            queryString = queryString + "                       SET @WarehouseBalanceEveryMonth = dbo.EOMONTHTIME(@WarehouseBalanceEveryMonth, 1)" + "\r\n";

            queryString = queryString + "                       INSERT INTO WarehouseBalanceDetail (WarehouseBalanceDate, HotelID, ServiceID, Quantity, AmountCost, Remarks)" + "\r\n";
            queryString = queryString + "                       SELECT      @WarehouseBalanceEveryMonth, HotelID, ServiceID, Quantity, AmountCost, Remarks " + "\r\n";
            queryString = queryString + "                       FROM        WarehouseBalanceDetail " + "\r\n";
            queryString = queryString + "                       WHERE       WarehouseBalanceDate = @WarehouseBalanceDateMAX" + "\r\n";

            queryString = queryString + "                       INSERT INTO WarehouseBalancePrice (WarehouseBalanceDate, HotelID, ServiceID, UnitPrice) " + "\r\n";
            queryString = queryString + "                       SELECT      @WarehouseBalanceEveryMonth, WarehouseBalancePrice.HotelID, WarehouseBalancePrice.ServiceID, WarehouseBalancePrice.UnitPrice" + "\r\n";
            queryString = queryString + "                       FROM        WarehouseBalancePrice INNER JOIN" + "\r\n";
            queryString = queryString + "                                   WarehouseBalanceDetail ON WarehouseBalancePrice.WarehouseBalanceDate = @WarehouseBalanceDateMAX AND WarehouseBalancePrice.WarehouseBalanceDate = WarehouseBalanceDetail.WarehouseBalanceDate AND WarehouseBalancePrice.HotelID = WarehouseBalanceDetail.HotelID AND WarehouseBalancePrice.ServiceID = WarehouseBalanceDetail.ServiceID " + "\r\n";

            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               SET @WarehouseBalanceDateMAX = @WarehouseBalanceEveryMonth " + "\r\n";//--SET THE MAXIMUM OF WarehouseBalanceDate
            queryString = queryString + "           END " + "\r\n";


            //                                  STEP 2: UPDATE NEW QUANTITY FOR THESE ITEMS CURRENTLY EXIST IN WarehouseBalanceDetail
            queryString = queryString + "       UPDATE WarehouseBalanceDetail" + "\r\n";//NO NEED TO UPDATE AmountCost. It will be calculated later in the process
            queryString = queryString + "       SET WarehouseBalanceDetail.Quantity = WarehouseBalanceDetail.Quantity + ActionTable.Quantity" + "\r\n";
            queryString = queryString + "       FROM    WarehouseBalanceDetail INNER JOIN" + "\r\n";
            queryString = queryString + "               @ActionTable ActionTable ON WarehouseBalanceDetail.HotelID = ActionTable.HotelID AND WarehouseBalanceDetail.ServiceID = ActionTable.ServiceID AND WarehouseBalanceDetail.WarehouseBalanceDate >= @ActionDate" + "\r\n";



            //                                  STEP 3: INSERT INTO WarehouseBalanceDetail FOR THESE ITEMS NOT CURRENTLY EXIST IN WarehouseBalanceDetail
            queryString = queryString + "       SET @WarehouseBalanceEveryMonth = dbo.EOMONTHTIME(@ActionDate, 9999)" + "\r\n";//--FIND THE FIRST @WarehouseBalanceEveryMonth WHICH IS GREATER OR EQUAL TO @ActionDate

            queryString = queryString + "       WHILE @WarehouseBalanceEveryMonth <= @WarehouseBalanceDateMAX" + "\r\n";
            queryString = queryString + "           BEGIN" + "\r\n";

            queryString = queryString + "               INSERT INTO     WarehouseBalanceDetail (WarehouseBalanceDate, HotelID, ServiceID, Quantity, AmountCost, Remarks)" + "\r\n";
            queryString = queryString + "               SELECT          @WarehouseBalanceEveryMonth, ActionTable.HotelID, ActionTable.ServiceID, ActionTable.Quantity, ActionTable.AmountCost, ActionTable.Remarks" + "\r\n";
            queryString = queryString + "               FROM            @ActionTable ActionTable LEFT JOIN" + "\r\n";
            queryString = queryString + "                               WarehouseBalanceDetail ON ActionTable.HotelID = WarehouseBalanceDetail.HotelID AND ActionTable.ServiceID = WarehouseBalanceDetail.ServiceID AND WarehouseBalanceDetail.WarehouseBalanceDate = @WarehouseBalanceEveryMonth" + "\r\n";
            queryString = queryString + "               WHERE           WarehouseBalanceDetail.ServiceID IS NULL " + "\r\n"; //--ADD NOT-IN-LIST ITEM"

            queryString = queryString + "               SET     @WarehouseBalanceEveryMonth = dbo.EOMONTHTIME(@WarehouseBalanceEveryMonth, 1)" + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       DELETE FROM WarehouseBalanceDetail WHERE Quantity = 0 " + "\r\n";



            #region Update Warehouse balance average price + ending amount

            queryString = queryString + "       DECLARE @LastDayOfPreviousMonth DateTime" + "\r\n";
            queryString = queryString + "       DECLARE @WarehouseInputCollection TABLE (HotelID int NOT NULL, ServiceID int NOT NULL, Quantity float NOT NULL, AmountCost float NOT NULL, UnitPrice float NOT NULL) " + "\r\n";

            queryString = queryString + "       SET @WarehouseBalanceEveryMonth = dbo.EOMONTHTIME(@ActionDate, 9999)" + "\r\n";//--FIND THE FIRST @WarehouseBalanceEveryMonth WHICH IS GREATER OR EQUAL TO @ActionDate

            queryString = queryString + "       DECLARE @NeedToGenerateAverageUnitPrice bit " + "\r\n"; //DELETE and RE-UPDATE WarehouseBalancePrice OF THESE ITEMS INCLUDE IN @ActionTable WHEN @PurchaseInvoiceID > 0 (MEAN WHEN WAREHOUSE INPUT) OR @WarehouseBalanceEveryMonth < @WarehouseBalanceDateMAX (MEAN WAREHOUSE OUTPUT OCCUR ON THE MONTH BEFORE THE LAST MONTH) IN ORDER TO RECALCULATE WarehouseBalancePrice
            queryString = queryString + "       SET @NeedToGenerateAverageUnitPrice = IIF (@PurchaseInvoiceID > 0 OR @WarehouseBalanceEveryMonth < @WarehouseBalanceDateMAX, 1, 0) " + "\r\n";

            queryString = queryString + "       IF @NeedToGenerateAverageUnitPrice = 1 " + "\r\n"; 
            queryString = queryString + "           DELETE FROM WarehouseBalancePrice FROM WarehouseBalancePrice INNER JOIN @ActionTable ActionTable ON WarehouseBalancePrice.WarehouseBalanceDate >= @WarehouseBalanceEveryMonth AND WarehouseBalancePrice.HotelID = ActionTable.HotelID AND WarehouseBalancePrice.ServiceID = ActionTable.ServiceID " + "\r\n"; //(USING @ActionTable AS A FILTER)

            queryString = queryString + "       WHILE @WarehouseBalanceEveryMonth <= @WarehouseBalanceDateMAX" + "\r\n";
            queryString = queryString + "           BEGIN" + "\r\n";

            queryString = queryString + "               SET     @LastDayOfPreviousMonth = dbo.EOMONTHTIME(@WarehouseBalanceEveryMonth, -1) " + "\r\n";

            //                                          STEP 1: GET COLLECTION OF BEGIN BALANCE + INPUT (Quantity, AmountCost)
            queryString = queryString + "               INSERT INTO     @WarehouseInputCollection (HotelID, ServiceID, Quantity, AmountCost, UnitPrice) " + "\r\n";
            queryString = queryString + "               SELECT          HotelID, ServiceID, SUM(Quantity), SUM(AmountCost), SUM(AmountCost)/ SUM(Quantity) " + "\r\n";

            queryString = queryString + "               FROM            (" + "\r\n";

            queryString = queryString + "                               SELECT      ActionTable.HotelID, ActionTable.ServiceID, WarehouseBalanceDetail.Quantity, WarehouseBalanceDetail.AmountCost AS AmountCost" + "\r\n";
            queryString = queryString + "                               FROM        WarehouseBalanceDetail INNER JOIN" + "\r\n"; //BEGIN BALANCE (USING @ActionTable AS A FILTER)
            queryString = queryString + "                                           @ActionTable ActionTable ON WarehouseBalanceDetail.HotelID = ActionTable.HotelID AND WarehouseBalanceDetail.ServiceID = ActionTable.ServiceID AND WarehouseBalanceDetail.WarehouseBalanceDate = @LastDayOfPreviousMonth" + "\r\n";

            queryString = queryString + "                               UNION ALL " + "\r\n";

            queryString = queryString + "                               SELECT      ActionTable.HotelID, ActionTable.ServiceID, PurchaseInvoiceDetails.Quantity, PurchaseInvoiceDetails.GrossAmount AS AmountCost" + "\r\n";
            queryString = queryString + "                               FROM        PurchaseInvoices INNER JOIN" + "\r\n";  //INPUT (USING @ActionTable AS A FILTER) + AND PLEASE SPECIAL CONSIDER TO THIS CONDITION CLAUSE: (@UpdateWarehouseBalanceOption = " + GlobalEnums.UpdateWarehouseBalanceOption.Add + " OR PurchaseInvoices.PurchaseInvoiceID <> @PurchaseInvoiceID)      (MEANS: @PurchaseInvoiceID <> 0 AND WHEN GlobalEnums.UpdateWarehouseBalanceOption.Minus: DON'T INCLUDE THIS PurchaseInvoice TO THE COLLECTION FOR CALCULATE THE PRICE + ENDING AMOUNT)
            queryString = queryString + "                                           PurchaseInvoiceDetails ON (@UpdateWarehouseBalanceOption = " + (int)GlobalEnums.UpdateWarehouseBalanceOption.Add + " OR PurchaseInvoices.PurchaseInvoiceID <> @PurchaseInvoiceID) AND PurchaseInvoices.PurchaseInvoiceID = PurchaseInvoiceDetails.PurchaseInvoiceID INNER JOIN " + "\r\n";
            queryString = queryString + "                                           @ActionTable ActionTable ON PurchaseInvoices.HotelID = ActionTable.HotelID AND PurchaseInvoiceDetails.ServiceID = ActionTable.ServiceID AND PurchaseInvoices.PurchaseInvoiceDate > @LastDayOfPreviousMonth AND PurchaseInvoices.PurchaseInvoiceDate <= @WarehouseBalanceEveryMonth" + "\r\n";

            queryString = queryString + "                               )WarehouseJournalUnion" + "\r\n";

            queryString = queryString + "               GROUP BY        HotelID, ServiceID " + "\r\n";
            queryString = queryString + "               HAVING          SUM(Quantity) <> 0 " + "\r\n";

            //                                          STEP 2: GENERATE AVERAGE UNIT PRICE
            queryString = queryString + "               IF @NeedToGenerateAverageUnitPrice = 1 " + "\r\n";
            queryString = queryString + "                   INSERT INTO     WarehouseBalancePrice (WarehouseBalanceDate, HotelID, ServiceID, UnitPrice) " + "\r\n";
            queryString = queryString + "                   SELECT          @WarehouseBalanceEveryMonth, HotelID, ServiceID, UnitPrice " + "\r\n";
            queryString = queryString + "                   FROM            @WarehouseInputCollection" + "\r\n";


            //                                          STEP 3: RECALCULATE END BALANCE (AmountCost)
            queryString = queryString + "               UPDATE          WarehouseBalanceDetail " + "\r\n";
            queryString = queryString + "               SET             WarehouseBalanceDetail.AmountCost = ROUND(WarehouseBalanceAmount.AmountCost, 0) " + "\r\n";
            queryString = queryString + "               FROM            WarehouseBalanceDetail INNER JOIN " + "\r\n";
            queryString = queryString + "                              (SELECT          HotelID, ServiceID, SUM(Quantity) AS Quantity, SUM(AmountCost) AS AmountCost " + "\r\n";

            queryString = queryString + "                               FROM           (" + "\r\n";

            queryString = queryString + "                                               SELECT      WarehouseInputCollection.HotelID, WarehouseInputCollection.ServiceID, WarehouseInputCollection.Quantity, WarehouseInputCollection.AmountCost AS AmountCost" + "\r\n";
            queryString = queryString + "                                               FROM        @WarehouseInputCollection WarehouseInputCollection " + "\r\n";

            queryString = queryString + "                                               UNION ALL " + "\r\n";

            queryString = queryString + "                                               SELECT      WarehouseInputCollection.HotelID, WarehouseInputCollection.ServiceID, -BillingDetail.Quantity AS Quantity, -ROUND(BillingDetail.Quantity * WarehouseInputCollection.UnitPrice, 0) AS AmountCost" + "\r\n";
            queryString = queryString + "                                               FROM        BillingMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                                           BillingDetail ON BillingMaster.BillingID = BillingDetail.BillingID INNER JOIN " + "\r\n";
            queryString = queryString + "                                                           ListRoom ON BillingMaster.RoomID = ListRoom.RoomID INNER JOIN  " + "\r\n";
            queryString = queryString + "                                                           @WarehouseInputCollection WarehouseInputCollection ON ListRoom.HotelID = WarehouseInputCollection.HotelID AND BillingDetail.ServiceID = WarehouseInputCollection.ServiceID AND BillingMaster.DepartureDate > @LastDayOfPreviousMonth AND BillingMaster.DepartureDate <= @WarehouseBalanceEveryMonth" + "\r\n";

            queryString = queryString + "                                              )WarehouseBalanceUnion" + "\r\n";

            queryString = queryString + "                               GROUP BY        HotelID, ServiceID " + "\r\n";

            queryString = queryString + "                              )WarehouseBalanceAmount ON WarehouseBalanceDetail.HotelID = WarehouseBalanceAmount.HotelID AND WarehouseBalanceDetail.ServiceID = WarehouseBalanceAmount.ServiceID AND WarehouseBalanceDetail.WarehouseBalanceDate = @WarehouseBalanceEveryMonth " + "\r\n";


            //                                          STEP 4: INIT VARIBLE FOR NEW LOOP
            queryString = queryString + "               DELETE FROM     @WarehouseInputCollection " + "\r\n";
            queryString = queryString + "               SET     @WarehouseBalanceEveryMonth = dbo.EOMONTHTIME(@WarehouseBalanceEveryMonth, 1)" + "\r\n";

            queryString = queryString + "           END " + "\r\n";

            #endregion Update Warehouse balance average price + ending amount


            queryString = queryString + "   END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("UpdateWarehouseBalance", queryString);
        }


        private void GetOverStockItems()
        {
            string queryWhere = " ((@PurchaseInvoiceID = 0 AND @BillingID = 0) OR ServiceID IN (SELECT ServiceID FROM @ListServiceAction))" + "\r\n";

            queryWhere = " 1 = 1 " + "\r\n"; //05.04.2011---TAM THOI KHONG XAI DIEU KIEN TREN, NHAM MUC DICH GIUP FUNCTION CHAY NHANH HON RAT NHIEU. SAU NAY KHI DA CODE HOAN CHINH RANGE FOR SERVICE DUA VAO @PurchaseInvoiceID, @BillingID THI NEN SU DUNG LAI CAU LENH TREN

            string queryString = " (@ActionDate DateTime, @PurchaseInvoiceID Int, @BillingID Int) " + "\r\n";
            queryString = queryString + " RETURNS @OverStockTable TABLE (OverStockDate DateTime NOT NULL, HotelID int NOT NULL, HotelName nvarchar(100) NOT NULL, ServiceID int NOT NULL, Description nvarchar(100) NOT NULL, Quantity float NOT NULL) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            // --FILTER: ListService NEED TO BE CHECK ONLY

            //05.04.2011---: KHONG XAI DIEU KIEN NAY: queryString = queryString + "       DECLARE @ListServiceAction TABLE (ServiceID int NOT NULL) " + "\r\n"; //05.04.2011---: KHONG XAI DIEU KIEN NAY: DO XET TAT CA SP, NEN KHONG CAN XAI @ListServiceAction

            //**************TAM THOI SU DUNG KHAI BAO NHU THE NAY 2 CHO: DANH SACH MAT HANG CAN KTRA VA THOI DIEM CAN KIEM TRA

            //TAM THOI: KTRA TAT CA.BEGIN

            //05.04.2011---: KHONG XAI DIEU KIEN NAY: queryString = queryString + "       INSERT @ListServiceAction SELECT ServiceID FROM ListService " + "\r\n";

            //TAM THOI: KTRA TAT CA
            //LOI O DAY LA: KHI EDIT (CHU KG PHAI KHI DELETE)
            //BOI VI: KHI DELETE: KTRA NHUNG MAT HANG CO TRONG DANH SACH,
            //        TRONG KHI DO: KHI EDIT: CHI KTRA NHUNG MAT HANG SAU KHI EDIT, TRONG KHI NHUNG MAT HANG BI BO DI KHONG CO TRONG DANH SACH SAU KHI EDIT KHONG DUOC KIEM TRA
            //                      PHAI HIEU RANG: NHUNG MAT HANG BI BO DI SAU KHI EDIT: HOAN TOAN TUONG DUONG BI DELETE: KHONG KTRA NEN SAI HOAN TOAN
            //                      TUM LAI: PHAI CODE THE NAO DE DAM BAO (HIEN TAI CHUA DAM BAO): PHAI UNION CA 2 DANH SACH: DANH SACH TRUOC KHI EDIT VA DANH SACH SAU KHI EDIT
            //DO DO: TAM THOI: KTRA TAT CA, SAU NAY SUY NGHI THEM


            //queryString = queryString + "       IF @PurchaseInvoiceID <> 0 "
            //queryString = queryString + "           INSERT @ListServiceAction SELECT ServiceID FROM PurchaseInvoiceDetails WHERE PurchaseInvoiceID = @PurchaseInvoiceID "
            //queryString = queryString + "       IF @BillingID <> 0 "
            //queryString = queryString + "           INSERT @ListServiceAction SELECT ServiceID FROM BillingDetail WHERE BillingID = @BillingID "
            //TAM THOI: KTRA TAT CA.END



            queryString = queryString + "       DECLARE @TempDate DateTime " + "\r\n";
            queryString = queryString + "       DECLARE @BackLogDateMax DateTime " + "\r\n";
            // --GET THE FIRST DATE NEED TO CHECK OVER STOCK.END
            // --THE FIRST DATE IS: THE @ActionDate OR THE DATE OF BACKLOG COLLECTION
            // --OPEN THE BACKLOG COLLECTION TO GET THE MIN(DATE)
            // --LATER: IF THERE IS MORE BACKLOG, LIKE DELIVERYADVICE: ADD HERE

            //*****queryString = queryString + "       DECLARE CursorBacklog CURSOR LOCAL FOR SELECT MIN(DeliveryDate) AS DeliveryDateMIN, MAX(DeliveryDate) AS DeliveryDateMAX FROM SKUTransferAdviceDetail WHERE (ROUND(Quantity - QuantityInput, 0) > 0)" + "\r\n";
            //*****queryString = queryString + "       OPEN CursorBacklog" + "\r\n";
            //*****queryString = queryString + "       FETCH NEXT FROM CursorBacklog INTO @TempDate, @BackLogDateMax " + "\r\n";
            //*****queryString = queryString + "       CLOSE CursorBacklog DEALLOCATE CursorBacklog" + "\r\n";

            //*****queryString = queryString + "       IF @ActionDate > @TempDate SET @ActionDate = @TempDate" + "\r\n";


            //**************TAM THOI SU DUNG KHAI BAO NHU THE NAY 2 CHO: DANH SACH MAT HANG CAN KTRA VA THOI DIEM CAN KIEM TRA

            //TAM THOI: KTRA TU 31/05/2009: LY DO TUONG TU NHU DANH SACH MAT HANG CAN KTRA
            //BOI VI: KHI DELETE: KTRA TU NGAY SKUActionDate: NHU VAY LA OK
            //        NHUNG KHI EDIT:  SUA NGAY (NGAY SAU EDIT > NGAY TRUOC DO)
            //                         THEO CACH HIEN TAI: CHI KTRA KE TU NGAY SAU EDIT MA THOI
            //                         NHU VAY: SAI HOAN TOAN: VI DANG LY RA PHAI KTRA KE TU NGAY TRUOC KHI EDIT (NGAY CU)
            //                         TUM LAI: PHAI CODE THE NAO DE DAM BAO (HIEN TAI CHUA DAM BAO): SO SANH NGAY TRUOC VA NGAY SAU EDIT: NGAY NAO < HON THI BAT DAU KIEM TRA KE TU NGAY DO
            //
            //         LUU Y THEM: NEU CHI XET DUA TREN BACKLOG LA KHONG THOA MAN (VAN BI LOI)
            //                     BOI VI: CO THE NHUNG MAT HANG TRONG BACKLOG KTRA VAN DU
            //                     NHUNG TON KHO < 0 TAI NHUNG THOI DIEM NAO DO TRUOC BACKLOG (<0 TRONG TABLE WarehouseBalanceDetail): PURE OVER STOCK (CHUA XUAT CUNG DA <0 ROI)
            //                     DIEU NAY TUONG TU DOI VOI DANH SACH MAT HANG CAN KTRA O PHAN TREN
            //TAM THOI: KTRA TU 31/05/2009

            //queryString = queryString + "CONVERT(smalldatetime, '" & Format(pfLockedDate, "dd/mm/yyyy") & "',103) " + "\r\n";

            queryString = queryString + "       SET @ActionDate = CONVERT(Datetime, '2014-09-30 14:00:00', 120) " + "\r\n"; //--END OF SEP/ 2014: FIRT START


            // --GET THE FIRST DATE NEED TO CHECK OVER STOCK.END


            // --GET THE BEGIN BALANCE IF AVAILABLE.BEGIN
            queryString = queryString + "       DECLARE @BalanceTable TABLE (HotelID int NOT NULL, ServiceID int NOT NULL, Quantity float NOT NULL)" + "\r\n";

            queryString = queryString + "       DECLARE @WarehouseBalanceDateBEGIN DateTime" + "\r\n";
            queryString = queryString + "       DECLARE CursorWarehouseBalance CURSOR LOCAL FOR SELECT MAX(WarehouseBalanceDate) AS WarehouseBalanceDate FROM WarehouseBalanceDetail WHERE WarehouseBalanceDate <= @ActionDate" + "\r\n";
            queryString = queryString + "       OPEN CursorWarehouseBalance" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorWarehouseBalance INTO @WarehouseBalanceDateBEGIN" + "\r\n";
            queryString = queryString + "       CLOSE CursorWarehouseBalance DEALLOCATE CursorWarehouseBalance" + "\r\n";


            queryString = queryString + "       IF NOT @WarehouseBalanceDateBEGIN IS NULL" + "\r\n";
            queryString = queryString + "           INSERT  @BalanceTable SELECT HotelID, ServiceID, Quantity FROM WarehouseBalanceDetail WHERE WarehouseBalanceDate = @WarehouseBalanceDateBEGIN AND " + queryWhere + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           SET @WarehouseBalanceDateBEGIN = CONVERT(Datetime, '2014-09-30 14:00:00', 120) " + "\r\n"; //CONVERT(smalldatetime, '" & Format(pdDateNULL, "dd/mm/yyyy") & "',103) 
            // --GET THE BEGIN BALANCE IF AVAILABLE.END


            // --GET THE DATE RANGE NEED TO BE CHECKED.BEGIN
            queryString = queryString + "       DECLARE @ActionDateEND DateTime" + "\r\n";
            queryString = queryString + "       DECLARE CursorWarehouseBalance CURSOR LOCAL FOR SELECT MAX(WarehouseBalanceDate) AS WarehouseBalanceDate FROM WarehouseBalanceDetail " + "\r\n";
            queryString = queryString + "       OPEN CursorWarehouseBalance" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorWarehouseBalance INTO @ActionDateEND" + "\r\n";
            queryString = queryString + "       CLOSE CursorWarehouseBalance DEALLOCATE CursorWarehouseBalance" + "\r\n";

            queryString = queryString + "       IF @ActionDateEND IS NULL OR @ActionDateEND < @ActionDate SET @ActionDateEND = @ActionDate " + "\r\n";  //--CHECK UNTIL THE LAST BALANCE
            queryString = queryString + "       IF @ActionDateEND < @BackLogDateMax SET @ActionDateEND = @BackLogDateMax " + "\r\n"; //--OR CHECK UNTIL THE LAST DATE OF BACKLOG

            // --GET THE DATE RANGE NEED TO BE CHECKED.END



            queryString = queryString + "       SET @TempDate = @ActionDate " + "\r\n";
            queryString = queryString + "       WHILE @TempDate <= @ActionDateEND" + "\r\n";
            queryString = queryString + "           BEGIN" + "\r\n";

            // --BALANCE AT: @WarehouseBalanceDateBEGIN: LOOK ON WarehouseBalanceDetail ONLY
            // --BALANCE AT: @TempDate > @WarehouseBalanceDateBEGIN: WarehouseBalanceDetail + SUM(INPUT) - SUM(Output)
            queryString = queryString + "               INSERT INTO @OverStockTable" + "\r\n";
            queryString = queryString + "               SELECT      @TempDate, HotelID, N'', ServiceID, N'', ROUND(SUM(Quantity), 0) AS Quantity" + "\r\n";
            queryString = queryString + "               FROM        (" + "\r\n";
            // --OPENNING
            queryString = queryString + "                           SELECT      WarehouseBalanceDetail.HotelID, WarehouseBalanceDetail.ServiceID, WarehouseBalanceDetail.Quantity" + "\r\n";
            queryString = queryString + "                           FROM        @BalanceTable WarehouseBalanceDetail" + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";
            // --INPUT
            queryString = queryString + "                           SELECT      PurchaseInvoices.HotelID, PurchaseInvoiceDetails.ServiceID, SUM(PurchaseInvoiceDetails.Quantity) AS Quantity" + "\r\n";
            queryString = queryString + "                           FROM        PurchaseInvoices INNER JOIN PurchaseInvoiceDetails ON PurchaseInvoices.PurchaseInvoiceID = PurchaseInvoiceDetails.PurchaseInvoiceID " + "\r\n";
            queryString = queryString + "                           WHERE       PurchaseInvoices.PurchaseInvoiceDate > @WarehouseBalanceDateBEGIN AND PurchaseInvoices.PurchaseInvoiceDate <= @TempDate AND " + queryWhere + "\r\n";
            queryString = queryString + "                           GROUP BY    PurchaseInvoices.HotelID, PurchaseInvoiceDetails.ServiceID" + "\r\n";

            queryString = queryString + "                           UNION ALL" + "\r\n";
            // --OUTPUT: Output
            queryString = queryString + "                           SELECT      ListRoom.HotelID, BillingDetail.ServiceID, SUM(-BillingDetail.Quantity) AS Quantity" + "\r\n";
            queryString = queryString + "                           FROM        BillingMaster INNER JOIN BillingDetail ON BillingMaster.BillingID = BillingDetail.BillingID INNER JOIN ListRoom ON BillingMaster.RoomID = ListRoom.RoomID " + "\r\n";
            queryString = queryString + "                           WHERE       BillingMaster.DepartureDate > @WarehouseBalanceDateBEGIN AND BillingMaster.DepartureDate <= @TempDate AND " + queryWhere + "\r\n";
            queryString = queryString + "                           GROUP BY    ListRoom.HotelID, BillingDetail.ServiceID" + "\r\n";

            queryString = queryString + "                           )TableOverStock" + "\r\n";
            queryString = queryString + "               GROUP BY    HotelID, ServiceID " + "\r\n";
            queryString = queryString + "               HAVING      ROUND(SUM(Quantity), 0) < 0 " + "\r\n";

            queryString = queryString + "               DECLARE             @COUNTOverStock Int SET @COUNTOverStock = 0" + "\r\n";
            queryString = queryString + "               DECLARE             CursorOverStock CURSOR LOCAL FOR SELECT COUNT(*) AS COUNTOverStock FROM @OverStockTable" + "\r\n";
            queryString = queryString + "               OPEN                CursorOverStock" + "\r\n";
            queryString = queryString + "               FETCH NEXT FROM     CursorOverStock INTO @COUNTOverStock" + "\r\n";
            queryString = queryString + "               CLOSE               CursorOverStock DEALLOCATE CursorOverStock" + "\r\n";

            queryString = queryString + "               IF @COUNTOverStock > 0 " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   UPDATE TableOverStock SET TableOverStock.Description = ListService.Description FROM @OverStockTable TableOverStock INNER JOIN ListService ON TableOverStock.ServiceID = ListService.ServiceID " + "\r\n";
            queryString = queryString + "                   UPDATE TableOverStock SET TableOverStock.HotelName = ListHotel.Description FROM @OverStockTable TableOverStock INNER JOIN ListHotel ON TableOverStock.HotelID = ListHotel.HotelID " + "\r\n";
            queryString = queryString + "                   BREAK" + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "               SET @TempDate = DATEADD(Day, 1, @TempDate)" + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       RETURN " + "\r\n";
            queryString = queryString + "   END " + "\r\n";


            this.totalBikePortalsEntities.CreateUserDefinedFunction("GetOverStockItems", queryString);
        }


        private void WarehouseJournal()
        {
            string SWI = " AND (@ServiceIDList = '' OR ServiceID IN (SELECT * FROM FNSplitUpIds(@ServiceIDList))) " + "\r\n";
            SWI = SWI + " AND (@HotelIDList = '' OR ListHotel.HotelID IN (SELECT * FROM FNSplitUpIds(@HotelIDList))) " + "\r\n";
            SWI = "" + "\r\n";

            string queryString = " @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @IsAmountIncluded bit " + "\r\n";
            queryString = queryString + "       IF (DATEADD(second, -1, @FromDate) = dbo.EOMONTHTIME(DATEADD(second, -1, @FromDate), 9999) AND @ToDate = dbo.EOMONTHTIME(@ToDate, 9999) AND @ToDate = dbo.EOMONTHTIME( DATEADD(second, -1, @FromDate), 1)) SET @IsAmountIncluded = 1 ELSE SET @IsAmountIncluded = 0 " + "\r\n";

            // --GET THE BEGIN BALANCE IF AVAILABLE.BEGIN
            queryString = queryString + "       DECLARE @WarehouseBalanceDate DateTime" + "\r\n";
            queryString = queryString + "       DECLARE CursorWarehouseBalance CURSOR LOCAL FOR SELECT MAX(WarehouseBalanceDate) AS WarehouseBalanceDate FROM WarehouseBalanceDetail WHERE WarehouseBalanceDate < @FromDate " + "\r\n"; // < OR <= ??? XEM XET LAI NHE!!!!
            queryString = queryString + "       OPEN CursorWarehouseBalance" + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CursorWarehouseBalance INTO @WarehouseBalanceDate" + "\r\n";
            queryString = queryString + "       CLOSE CursorWarehouseBalance DEALLOCATE CursorWarehouseBalance" + "\r\n";

            queryString = queryString + "       IF @WarehouseBalanceDate IS NULL  SET @WarehouseBalanceDate = CONVERT(smalldatetime, '" + (new DateTime(1990, 01, 01, 14, 00, 00)).ToString("yyyy-MM-dd HH:mm:ss") + "', 120) " + "\r\n";
            // --GET THE BEGIN BALANCE IF AVAILABLE.END

            queryString = queryString + "       SELECT      WarehouseJournalMaster.NMVNTaskID, WarehouseJournalMaster.JournalPrimaryID, WarehouseJournalMaster.JournalDate, WarehouseJournalMaster.JournalReference, LEFT(WarehouseJournalMaster.JournalDescription, 200) AS JournalDescription, " + "\r\n";
            queryString = queryString + "                   ListService.ServiceID, ListService.Description, ListService.UnitForSales, WarehouseJournalMaster.HotelID, ListHotel.Description AS HotelName, " + "\r\n";
            queryString = queryString + "                   WarehouseJournalMaster.QuantityBegin, WarehouseJournalMaster.AmountBegin, WarehouseJournalMaster.QuantityInput, WarehouseJournalMaster.UnitPriceInput, WarehouseJournalMaster.VATAmountInput, WarehouseJournalMaster.GrossAmountInput, WarehouseJournalMaster.QuantityOutput, WarehouseJournalMaster.AmountOutput, WarehouseJournalMaster.QuantityBegin + WarehouseJournalMaster.QuantityInput - WarehouseJournalMaster.QuantityOutput AS QuantityEnd, CASE WHEN (@IsAmountIncluded = 1) THEN WarehouseJournalMaster.AmountBegin + WarehouseJournalMaster.GrossAmountInput - WarehouseJournalMaster.AmountOutput ELSE 0 END AS AmountEnd, WarehouseJournalMaster.QuantityEndNext, WarehouseJournalMaster.AmountEndNext " + "\r\n";

            queryString = queryString + "       FROM        ( " + "\r\n";

            queryString = queryString + "                       SELECT      NMVNTaskID, JournalPrimaryID, MAX(JournalDate) AS JournalDate, MAX(JournalReference) AS JournalReference, MAX(JournalDescription) AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                   ServiceID, HotelID, SUM(QuantityBegin) AS QuantityBegin, SUM(AmountBegin) AS AmountBegin, SUM(QuantityInput) AS QuantityInput, AVG(UnitPriceInput) AS UnitPriceInput, SUM(VATAmountInput) AS VATAmountInput, SUM(GrossAmountInput) AS GrossAmountInput, SUM(QuantityOutput) AS QuantityOutput, SUM(AmountOutput) AS AmountOutput, SUM(QuantityEndNext) AS QuantityEndNext, SUM(AmountEndNext) AS AmountEndNext " + "\r\n";

            queryString = queryString + "                       FROM       (" + "\r\n";
            // --OPENNING: PURE OPENNING   //BEGIN
            queryString = queryString + "                           SELECT      0 AS NMVNTaskID, 0 AS JournalPrimaryID, @FromDate - 1 AS JournalDate, '' AS JournalReference, '' AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       WarehouseBalanceDetail.ServiceID, WarehouseBalanceDetail.HotelID, IIF(WarehouseBalanceDate = @WarehouseBalanceDate, WarehouseBalanceDetail.Quantity, 0) AS QuantityBegin, CASE WHEN (@IsAmountIncluded = 1 AND WarehouseBalanceDate = @WarehouseBalanceDate) THEN WarehouseBalanceDetail.AmountCost ELSE 0 END AS AmountBegin, 0 AS QuantityInput, 0 AS UnitPriceInput, 0 AS VATAmountInput, 0 AS GrossAmountInput, 0 AS QuantityOutput, 0 AS AmountOutput, IIF(WarehouseBalanceDate <> @WarehouseBalanceDate, WarehouseBalanceDetail.Quantity, 0) AS QuantityEndNext, CASE WHEN (@IsAmountIncluded = 1 AND WarehouseBalanceDate <> @WarehouseBalanceDate) THEN WarehouseBalanceDetail.AmountCost ELSE 0 END AS AmountEndNext " + "\r\n";
            queryString = queryString + "                           FROM        WarehouseBalanceDetail " + "\r\n";
            queryString = queryString + "                           WHERE       (WarehouseBalanceDate = @WarehouseBalanceDate OR (@IsAmountIncluded = 1 AND WarehouseBalanceDate = dbo.EOMONTHTIME(@ToDate, 9999))) " + SWI + "\r\n";
            // --OPENNING: PURE OPENNING   //END

            // --INPUT: IN-TERM OPENNING + INPUT   //BEGIN
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      CASE WHEN PurchaseInvoices.PurchaseInvoiceDate >= @FromDate THEN " + (int)GlobalEnums.NMVNTaskID.PurchaseInvoice + " ELSE 0 END AS NMVNTaskID, CASE WHEN PurchaseInvoices.PurchaseInvoiceDate >= @FromDate THEN PurchaseInvoiceDetails.PurchaseInvoiceDetailID ELSE 0 END AS JournalPrimaryID, CASE WHEN PurchaseInvoices.PurchaseInvoiceDate >= @FromDate THEN PurchaseInvoices.PurchaseInvoiceDate ELSE @FromDate - 1 END AS JournalDate, CASE WHEN PurchaseInvoices.PurchaseInvoiceDate >= @FromDate THEN PurchaseInvoices.PurchaseInvoiceReference ELSE '' END AS JournalReference, CASE WHEN PurchaseInvoices.PurchaseInvoiceDate >= @FromDate THEN PurchaseInvoices.Description ELSE '' END AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       PurchaseInvoiceDetails.ServiceID, PurchaseInvoices.HotelID, CASE WHEN PurchaseInvoices.PurchaseInvoiceDate < @FromDate THEN PurchaseInvoiceDetails.Quantity ELSE 0 END AS QuantityBegin, 0 AS AmountBegin, CASE WHEN PurchaseInvoices.PurchaseInvoiceDate >= @FromDate THEN PurchaseInvoiceDetails.Quantity ELSE 0 END AS QuantityInput, CASE WHEN PurchaseInvoices.PurchaseInvoiceDate >= @FromDate THEN PurchaseInvoiceDetails.UnitPrice ELSE 0 END AS UnitPriceInput, CASE WHEN PurchaseInvoices.PurchaseInvoiceDate >= @FromDate THEN PurchaseInvoiceDetails.VATAmount ELSE 0 END AS VATAmountInput, CASE WHEN PurchaseInvoices.PurchaseInvoiceDate >= @FromDate THEN ROUND(PurchaseInvoiceDetails.GrossAmount, " + (int)GlobalEnums.rndAmount + ") ELSE 0 END AS GrossAmountInput, 0 AS QuantityOutput, 0 AS AmountOutput, 0 AS QuantityEndNext, 0 AS AmountEndNext " + "\r\n";
            queryString = queryString + "                           FROM        PurchaseInvoices INNER JOIN " + "\r\n";
            queryString = queryString + "                                       PurchaseInvoiceDetails ON PurchaseInvoices.PurchaseInvoiceID = PurchaseInvoiceDetails.PurchaseInvoiceID " + "\r\n";
            queryString = queryString + "                           WHERE       PurchaseInvoices.PurchaseInvoiceDate > @WarehouseBalanceDate AND PurchaseInvoices.PurchaseInvoiceDate <= @ToDate " + SWI + "\r\n";
            // --INPUT: IN-TERM OPENNING + INPUT   //END

            // --OUTPUT: IN-TERM OPENNING + OUTPUT //BEGIN
            queryString = queryString + "                           UNION ALL" + "\r\n";

            queryString = queryString + "                           SELECT      CASE WHEN BillingMaster.DepartureDate >= @FromDate THEN " + (int)GlobalEnums.NMVNTaskID.BillingMaster + " ELSE 0 END AS NMVNTaskID, 0 JournalPrimaryID, @FromDate - 1 AS JournalDate, '' JournalReference, '' AS JournalDescription, " + "\r\n";
            queryString = queryString + "                                       BillingDetail.ServiceID, ListRoom.HotelID, CASE WHEN BillingMaster.DepartureDate < @FromDate THEN -BillingDetail.Quantity ELSE 0 END AS QuantityBegin, 0 AS AmountBegin, 0 AS QuantityInput, 0 AS UnitPriceInput, 0 AS VATAmountInput, 0 AS AmountGrossInput, CASE WHEN BillingMaster.DepartureDate >= @FromDate THEN BillingDetail.Quantity ELSE 0 END AS QuantityOutput, CASE WHEN BillingMaster.DepartureDate >= @FromDate AND (@IsAmountIncluded = 1) THEN ROUND(BillingDetail.Quantity * WarehouseBalancePrice.UnitPrice, 0) ELSE 0 END AS AmountOutput, 0 AS QuantityEndNext, 0 AS AmountEndNext " + "\r\n";
            queryString = queryString + "                           FROM        BillingMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                                       BillingDetail ON BillingMaster.BillingID = BillingDetail.BillingID INNER JOIN " + "\r\n";
            queryString = queryString + "                                       ListRoom ON BillingMaster.RoomID = ListRoom.RoomID LEFT JOIN " + "\r\n";
            queryString = queryString + "                                       WarehouseBalancePrice ON WarehouseBalancePrice.WarehouseBalanceDate = dbo.EOMONTHTIME(@ToDate, 9999) AND ListRoom.HotelID = WarehouseBalancePrice.HotelID AND BillingDetail.ServiceID = WarehouseBalancePrice.ServiceID " + "\r\n";
            queryString = queryString + "                           WHERE       BillingMaster.DepartureDate > @WarehouseBalanceDate AND BillingMaster.DepartureDate <= @ToDate " + SWI + "\r\n";
            // --OUTPUT: IN-TERM OPENNING + OUTPUT //END

            queryString = queryString + "                           )WarehouseJournalUnion" + "\r\n";

            queryString = queryString + "                       GROUP BY        NMVNTaskID, JournalPrimaryID, ServiceID, HotelID " + "\r\n";
            queryString = queryString + "                   ) WarehouseJournalMaster INNER JOIN " + "\r\n";

            queryString = queryString + "                   ListService ON WarehouseJournalMaster.ServiceID = ListService.ServiceID INNER JOIN " + "\r\n";
            queryString = queryString + "                   ListHotel ON WarehouseJournalMaster.HotelID = ListHotel.HotelID  " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("WarehouseJournal", queryString);
        }

    }
}
