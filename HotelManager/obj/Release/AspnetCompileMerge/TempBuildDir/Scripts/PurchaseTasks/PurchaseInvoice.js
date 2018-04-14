//BEGIN Validation

$(document).ready(function () {
    if ($("#isValid").val() == 1) {
        $("#div-alert").css("display", "block");
    }
});



$("form").submit(function (event) {

    if ($("#isSubmited").val().toLowerCase() == "true") {

        if (!$(this).valid()) {
            $("#div-alert").css("display", "block");
        }
        else {
            $("#div-alert").css("display", "none");

            var kendoGrid = $("#purchaseInvoiceDetailsGrid").data("kendoGrid");

            if (kendoGrid.dataSource.hasChanges()) {

                var validatable = $("#purchaseInvoiceDetailsGrid").kendoValidator().data("kendoValidator");
                if (validatable.validate()) {

                    EnforceKendoValidator(kendoGrid);
                    return validatable.validate();
                }
                else {
                    return false;
                }

            }
            else {
                return true;
            }
        }
    }
});


function alert_click() {
    $(".validation-summary-errors").toggle("slow", "swing");
}

//END Validation








//BEGIN Grid data source helper
function index(dataItem) {

    var data = $("#purchaseInvoiceDetailsGrid").data("kendoGrid").dataSource.data();
    return data.indexOf(dataItem);
}

var rowNumber = 0;
function purchaseInvoiceDetailsGrid_DataBound(e) {
    rowNumber = 0;
}

function RowNumber(data) {
    return ++rowNumber;
}
//END Grid data source helper



//BEGIN Autocomplete
function onAdditionalListServiceData(e) {
    return {//Send parameter to controller search action
        text: $("#ListServiceDescription").data("kendoAutoComplete").value()
    };
}

function onAdditionalPurchaseInvoiceData(e) {
    return {//Send parameter to controller search action
        text: $("#SupplierName").data("kendoAutoComplete").value()
    };
}


function ListService_Select(e) {

    var dataItem = this.dataItem(e.item.index());

    var grid = $("#purchaseInvoiceDetailsGrid").data("kendoGrid");
    var data = grid.dataSource.data();

    var currentRow = data[$(".k-edit-cell").closest("tr").index()];

    currentRow.set("ServiceID", dataItem.ServiceID);

    $("#ListServiceDescriptionTemp").val(dataItem.Description);
}
//END Autocomplete





//BEGIN Handle Grid Events
function purchaseInvoiceDetailsGrid_DataSourceChange(e) {    

    if (e.action != undefined) {
        if (e.action == "remove") {
            updateModelProperty();
        }
        else {
            for (var i = 0; i < e.items.length; i++) {

                var dataRow = e.items[0];

                if ($("#ListServiceDescription").val() != $("#ListServiceDescriptionTemp").val() && $("#ListServiceDescription").prop("kendoBindingTarget") != undefined) {
                    dataRow.set("ListServiceDescription", $("#ListServiceDescriptionTemp").val());
                }

                updateCalculatedCell(dataRow);
            }
        }
    }
}

//Important: This function run ok only when one cell in edit mode
//This avoid redrawing the Grid rows when use the 'dataItem.set()' method
//Note: When use the 'dataItem.set()' method: right after call 'set' the grid is repainted and the rows are receiving different uid attributes i.e. the previously selected row does not exist anymore. This will raise error when handle for the next selected row.
function purchaseInvoiceDetailsGrid_Change(e) {    

    var editingCellRowIndex = $(".k-edit-cell").closest("tr").index(); //Check current editing row index

    if (editingCellRowIndex >= 0) {//Current editing cell found

        var editingCellColIndex = $(".k-edit-cell").first().index(); //Check current editing col index
        var selectedRows = this.select();

        if (editingCellColIndex > 0 && selectedRows.length > 1) { //Current editing cell found && user drag to select more than 1 row.

            //Get the current editing field name
            var gridHeader = this.thead;
            var thGridHeader = $(gridHeader).find("th").eq(editingCellColIndex);
            var editingCellFieldName = $(thGridHeader).data("field");

            //Get the current editing cell data
            var gridData = this.dataSource.data();
            var editingDataRow = gridData[editingCellRowIndex];
            var editingDataCell = editingDataRow.get(editingCellFieldName);
                           
            for (var i = 0; i < selectedRows.length; i++) {

                var dataItem = this.dataItem(selectedRows[i]);
                dataItem.set(editingCellFieldName, editingDataCell)//Set all selected rows the same value of the current editing cell
            }            

            e.sender.refresh();
        }
    }
}


var isFirstCall = false; //This flag 'isFirstCall' to detect the first time this function is called by grid.DataSource.Change. This prevent the script from multiple call to updateFooterTemplate, updateModelProperty -> to improve the performance
function updateCalculatedCell(dataRow) {//Recalculate calculated cell
    
    if (dataRow != undefined) {

        var isNeedExtraWork = false;
        if (!isFirstCall) {
            isFirstCall = true;
            isNeedExtraWork = true;
        }

        dataRow.set("Amount", dataRow.UnitPrice * dataRow.Quantity);
        dataRow.set("VATAmount", (dataRow.Amount * dataRow.VATPercent) / 100);
        dataRow.set("GrossAmount", dataRow.Amount + dataRow.VATAmount);

        if (isNeedExtraWork) {
            isFirstCall = false;

            updateFooterTemplate();
            updateModelProperty();
        }
    }
}

function updateFooterTemplate() {//Refresh FooterTemplate for display
    
    var grid = $("#purchaseInvoiceDetailsGrid").data("kendoGrid");
    var rowFooter = $(".k-footer-template").children("td");

    if (rowFooter != undefined) {//Footer found

        for (var i = 0; i < grid.columns.length; i++) {

            var colName = $("th", this.thead).eq(i).data("field");
            column = grid.columns[i];

            var format = column.format;

            if (format == undefined) {
                format = "{0:n}";
            }

            var cell = rowFooter.eq(i);
            var template = column.footerTemplate;

            if (template != undefined)
                switch (colName) {
                    case "Quantity":
                        cell.html(kendo.format(format, grid.dataSource.aggregates().Quantity.sum));
                        break;
                    case "UnitPrice":
                        cell.html(kendo.format(format, grid.dataSource.aggregates().UnitPrice.sum));
                        break;
                    case "Amount":
                        cell.html(kendo.format(format, grid.dataSource.aggregates().Amount.sum));
                        break;
                    case "VATAmount":
                        cell.html(kendo.format(format, grid.dataSource.aggregates().VATAmount.sum));
                        break;
                    case "GrossAmount":
                        cell.html(kendo.format(format, grid.dataSource.aggregates().GrossAmount.sum));
                        break;
                    default:
                        break;
                }

        }
    }
}

function updateModelProperty() {//Update model property for post data

    var grid = $("#purchaseInvoiceDetailsGrid").data("kendoGrid");

    var dataSource = grid.dataSource;

    //records on current view / page   
    var recordsOnCurrentView = dataSource.view().length;
           
    if (recordsOnCurrentView == 0) {
        $("#TotalQuantity").val(0);
        $("#TotalAmount").val(0);
        $("#TotalVATAmount").val(0);
        $("#TotalGrossAmount").val(0);
    }
    else {
        if (grid.dataSource.aggregates().Amount != undefined && grid.dataSource.aggregates().Amount != null) {
            $("#TotalQuantity").val(grid.dataSource.aggregates().Quantity.sum);
            $("#TotalAmount").val(grid.dataSource.aggregates().Amount.sum);
            $("#TotalVATAmount").val(grid.dataSource.aggregates().VATAmount.sum);
            $("#TotalGrossAmount").val(grid.dataSource.aggregates().GrossAmount.sum);
        }
    }
}



function error_handler(e) {
    if (e.errors) {
        var message = "Errors:\n";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });
        alert(message);
    }
}

//END Handle Grid Events