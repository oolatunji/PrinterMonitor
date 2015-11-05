(function () {
    $('#MonitorPanel').addClass('wobblebar-loader');
    $('#lowRibbonPrinter').hide();
    $('#offlinePrinter').hide();
    $('#onlinePrinter').hide();
   
    getLatestUpdates();    
        
    var timer = setInterval(getLatestUpdates, 60000);    
}());

function getLatestUpdates() {    
    var printerMonitoringHub = $.connection.PrinterMonitoring;
    
    $.connection.hub.start();    

    printerMonitoringHub.client.latestPrinterStatus = function (printerStatuses) {

        var offlinePrinterStatus = [];
        var lowRibbonPrinterStatus = [];

        $("#offlinePrinterPanel").html('');
        $("#lowRibbonPrinterPanel").html('');
        $("#onlinePrinterPanel").html('');

        var lowRibbonPrinter = '';
        var offlinePrinter = '';
        var onlinePrinter = '';
        
        $.each(printerStatuses, function (key, printerStatus) {
            if (printerStatus.status == 0) {

                offlinePrinter += '<div style="cursor:pointer;float:left;background-color:red;width:auto;height:17px;margin:5px" data-toggle="modal" data-target="#modalHeaderLightBlue" >';
                offlinePrinter += '<div style="color:white;margin-left:5px;margin-right:5px;"><p class="blink" style="font-family:Calibri;font-weight:bold;">' + printerStatus.branchName + '</p></div>';
                offlinePrinter += '</div>';

                offlinePrinterStatus.push(printerStatus);

            } else if (printerStatus.status == 1) {                

                var ribbonStatus = printerStatus.ribbonStatus.toString().length < 2 ? "0" + printerStatus.ribbonStatus : printerStatus.ribbonStatus;
                if (printerStatus.ribbonStatus <= 40) {
                    lowRibbonPrinter += '<div style="cursor:pointer;float:left;background-color:purple;width:200px;height:17px;margin:5px"  data-toggle="modal" data-target="#modalHeaderLightPurple">';
                    lowRibbonPrinter += '<div style="color:white;width:50%;margin-left:5px;margin-right:5px;float:left"><p class="blink" style="font-family:Calibri;font-weight:bold;">' + printerStatus.branchName + ' </p></div>';
                    lowRibbonPrinter += '<div style="color:white;margin-right:10px;float:left;"><span style="font-family:Calibri;font-weight:bold;margin-right:10px;">' + ribbonStatus + '% |</span>';
                    lowRibbonPrinter += '<span style="font-family:Calibri;font-weight:bold;">' + printerStatus.printedCards + '</span></div>';
                    lowRibbonPrinter += '</div>';

                    lowRibbonPrinterStatus.push(printerStatus);
                }
                else if (printerStatus.ribbonStatus > 40) {

                    onlinePrinter += '<div style="float:left;background-color:green;width:200px;height:17px;margin:5px">';
                    onlinePrinter += '<div style="color:white;width:50%;margin-left:5px;margin-right:10px;float:left"><p style="font-family:Calibri;font-weight:bold;">' + printerStatus.branchName + ' </p></div>';
                    onlinePrinter += '<div style="color:white;margin-right:10px;float:left;"><span style="font-family:Calibri;font-weight:bold;margin-right:10px;">' + ribbonStatus + '% |</span>';
                    onlinePrinter += '<span style="font-family:Calibri;font-weight:bold;">' + printerStatus.printedCards + '</span></div>';
                    onlinePrinter += '</div>';

                }
            }
        });

        if (offlinePrinter != '') {
            $('#offlinePrinter').show();
            $("#offlinePrinterPanel").html(offlinePrinter);
        } else if (offlinePrinter == '') {
            $('#offlinePrinter').hide();
        }

        if (lowRibbonPrinter != '') {
            $('#lowRibbonPrinter').show();
            $("#lowRibbonPrinterPanel").html(lowRibbonPrinter);
        } else if (lowRibbonPrinter == '') {
            $('#lowRibbonPrinter').hide();
        }

        if (onlinePrinter != '') {
            $('#onlinePrinter').show();
            $("#onlinePrinterPanel").html(onlinePrinter);
        } else if (onlinePrinter == '') {
            $('#onlinePrinter').hide();
        }

        $('#MonitorPanel').removeClass('wobblebar-loader');

        offlineData(offlinePrinterStatus);
        lowRibbonData(lowRibbonPrinterStatus);
        
    };
    $.connection.hub.start().done(function () {        
        printerMonitoringHub.server.getLatestPrinterStatus();
    });   
}

function offlineData(printerData) {
    
    try{           
        $('#properties tfoot th').each(function () {
            var title = $('#properties tfoot th').eq($(this).index()).text();
            if (title != "")
                $(this).html('<input type="text" placeholder="Search ' + title + '" />');
        });

        if ($.fn.DataTable.isDataTable('#properties')) {
            var table = $('#properties').DataTable();
            table.clear().draw();
            table.rows.add(printerData); // Add new data
            table.columns.adjust().draw(); // R            
        }
        else {
            var table = $('#properties').DataTable({

                "processing": true,

                "data": printerData,

                "columns": [
                    { "data": "branchName" },
                    { "data": "ribbonStatus" },
                    { "data": "printedCards" },
                    {
                        "data": "status",
                        "visible": false
                    },
                ],

                "order": [[1, "asc"]],

                "sDom": 'T<"clear">lrtip',

                "oTableTools": {
                    "sSwfPath": "../images/copy_csv_xls_pdf.swf",
                    "aButtons": [
                        {
                            "sExtends": "print",
                            "sButtonText": "Print Data",
                            "oSelectorOpts": { filter: 'applied', order: 'current' }
                        }
                    ]
                }
            });


        }        
    } catch (err) {
        alert(err);
    }
}


function lowRibbonData(printerData) {

    try {

        $('#lowproperties tfoot th').each(function () {
            var title = $('#lowproperties tfoot th').eq($(this).index()).text();
            if (title != "")
                $(this).html('<input type="text" placeholder="Search ' + title + '" />');
        });

        if ($.fn.DataTable.isDataTable('#lowproperties')) {
            var table = $('#lowproperties').DataTable();
            table.clear().draw();
            table.rows.add(printerData); // Add new data
            table.columns.adjust().draw(); // R            
        }
        else {
            var table = $('#lowproperties').DataTable({

                "processing": true,

                "data": printerData,

                "columns": [
                    { "data": "branchName" },
                    { "data": "ribbonStatus" },
                    { "data": "printedCards" },
                    {
                        "data": "status",
                        "visible": false
                    },
                ],

                "order": [[1, "asc"]],

                "sDom": 'T<"clear">lrtip',

                "oTableTools": {
                    "sSwfPath": "../images/copy_csv_xls_pdf.swf",
                    "aButtons": [
                        {
                            "sExtends": "print",
                            "sButtonText": "Print Data",
                            "oSelectorOpts": { filter: 'applied', order: 'current' }
                        }
                    ]
                }
            });
        }
    } catch (err) {
        alert(err);
    }
}

$(document).ready(function () {
    $('#dataTables-properties').DataTable({
        responsive: true
    });
    $('#dataTables-lowproperties').DataTable({
        responsive: true
    });
});