function EnforceKendoValidator(kendoGrid) {

    var rows = kendoGrid.tbody.find("tr");    // See if there are any insert rows
    var listrows = kendoGrid.dataSource.data();
    for (var i = 0; i < listrows.length; i++) {
        if ((listrows[i].isNew() || listrows[i].dirty)) {//Check only IsNew or IsDirty rows
            var cols = $(rows[i]).find("td");
            for (var j = 0; j < cols.length; j++) {
                kendoGrid.editCell($(cols[j]));
                if (kendoGrid.editable != null) {
                    if (!kendoGrid.editable.end()) {// By calling editable end we will make validation fire
                        return; //Return when first invalid cell found
                    }
                    else {
                        kendoGrid.closeCell();// Take cell out of edit mode
                    }
                }
            }
        }
    }
}

function CreatePopUp(popuptitle, widthsize, heightsize, popupWindow) {
    popupWindow.append("<div id='window'></div>");
    var mywindow = $("#window")
    .kendoWindow({
        width: widthsize,
        height: heightsize,
        actions: ["Pin", "Close"],
        title: popuptitle,
        //draggable: false,
        resizable: false,
        modal: true,
        iframe: true,
        pinned: true,
        //open: function(){
        //    this.wrapper.css({ top: 20 });
        //},
        deactivate: function () {
            this.destroy();
            this.close();
        }
    }).data("kendoWindow");
    return mywindow;
}

var rABS = typeof FileReader !== "undefined" && typeof FileReader.prototype !== "undefined" && typeof FileReader.prototype.readAsBinaryString !== "undefined";
var use_worker = typeof Worker !== 'undefined';

function fixdata(data) {
    var o = "", l = 0, w = 10240;
    for (; l < data.byteLength / w; ++l) o += String.fromCharCode.apply(null, new Uint8Array(data.slice(l * w, l * w + w)));
    o += String.fromCharCode.apply(null, new Uint8Array(data.slice(l * w)));
    return o;
}

function ab2str(data) {
    var o = "", l = 0, w = 10240;
    for (; l < data.byteLength / w; ++l) o += String.fromCharCode.apply(null, new Uint16Array(data.slice(l * w, l * w + w)));
    o += String.fromCharCode.apply(null, new Uint16Array(data.slice(l * w)));
    return o;
}

function s2ab(s) {
    var b = new ArrayBuffer(s.length * 2), v = new Uint16Array(b);
    for (var i = 0; i != s.length; ++i) v[i] = s.charCodeAt(i);
    return [v, b];
}


function xlsxworker(data, cb) {
    transferable = use_worker;
    if (transferable) xlsxworker_xfer(data, cb);
    else xlsxworker_noxfer(data, cb);
}

function to_json(workbook) {
    var result = {};
    workbook.SheetNames.forEach(function (sheetName) {
        var roa = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
        if (roa.length > 0) {
            result[sheetName] = roa;
        }
    });
    return result;
}

function to_csv(workbook) {
    var result = [];
    workbook.SheetNames.forEach(function (sheetName) {
        var csv = XLSX.utils.sheet_to_csv(workbook.Sheets[sheetName]);
        if (csv.length > 0) {
            result.push("SHEET: " + sheetName);
            result.push("");
            result.push(csv);
        }
    });
    return result.join("\n");
}

function to_formulae(workbook) {
    var result = [];
    workbook.SheetNames.forEach(function (sheetName) {
        var formulae = XLSX.utils.get_formulae(workbook.Sheets[sheetName]);
        if (formulae.length > 0) {
            result.push("SHEET: " + sheetName);
            result.push("");
            result.push(formulae.join("\n"));
        }
    });
    return result.join("\n");
}


function handleFile(e) {
    var files = e.target.files;
    var i, f;
    for (i = 0, f = files[i]; i != files.length; ++i) {
        var reader = new FileReader();
        var name = f.name;

        if (typeof process_wb == 'undefined') {
            return false;
        }

        reader.onload = function (e) {
            if (typeof console !== 'undefined') console.log("onload", new Date(), rABS, use_worker);
            var data = e.target.result;


            if (use_worker) {
                xlsxworker(data, process_wb);
            } else {
                var wb;
                if (rABS) {
                    wb = XLSX.read(data, { type: 'binary' });
                } else {
                    var arr = fixdata(data);
                    wb = XLSX.read(btoa(arr), { type: 'base64' });
                }

                process_wb(wb);
            }
        };

        if (rABS) reader.readAsBinaryString(f);
        else reader.readAsArrayBuffer(f);
    }
}

function tableToExcel(table, sheetname) {
    var uri = 'data:application/vnd.ms-excel;base64,'
      , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head><body><table>{table}</table></body></html>'
      , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
      , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
    if (!table.nodeType) table = document.getElementById(table)
    var ctx = { worksheet: sheetname || 'Worksheet', table: table.innerHTML }
    window.location.href = uri + base64(format(template, ctx))
}

