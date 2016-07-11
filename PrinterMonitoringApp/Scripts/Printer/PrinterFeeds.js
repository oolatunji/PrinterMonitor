$(document).ready(function () {
    getLatestUpdates();
    pollPrinterFeeds();
    if (window.sessionStorage.getItem("printerFeedsTimer") != null)
        clearInterval(JSON.parse(window.sessionStorage.getItem("printerFeedsTimer")));
});

function pollPrinterFeeds() {
    $.ajax({
        url: settingsManager.websiteURL + 'api/SystemAPI/GetPrinterFeedsPollingTime',
        type: 'GET',
        async: true,
        cache: false,
        success: function (settings) {
            displayPrinterFeeds(settings.PrinterFeedsPollingTime);
        },
        error: function (xhr) {
            if (xhr.status = 404)
                displayMessage("error", 'Error experienced: Incorrect Application Url.', "Printer Management");
            else
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "Printer Management");
            console.log(xhr);
        }
    });
}

function displayPrinterFeeds(time) {

    var feedsPollingTime = time * 60000;

    $('#MonitorPanel').addClass('wobblebar-loader');
    $('#lowRibbonPrinter').hide();
    $('#offlinePrinter').hide();
    $('#onlinePrinter').hide();
    $('#communicationPrinter').hide();

    getLatestUpdates();

    var timer = setInterval(getLatestUpdates, feedsPollingTime);

    if (window.sessionStorage.getItem("printerFeedsTimer") != null)
        window.sessionStorage.removeItem("printerFeedsTimer");

    window.sessionStorage.setItem("printerFeedsTimer", JSON.stringify(timer));
}

function getLatestUpdates() {
    var printerMonitoringHub = $.connection.PrinterMonitoring;

    $.connection.hub.start();

    printerMonitoringHub.client.latestPrinterStatus = function (printerStatuses) {

        var offlinePrinterStatus = [];
        var lowRibbonPrinterStatus = [];
        var onlinePrinterStatus = [];
        var communicationPrinterStatus = [];

        if (printerStatuses.noCommunicationPrinters.length != 0)
            communicationPrinterStatus = printerStatuses.noCommunicationPrinters;

        if (printerStatuses.offlinePrinters.length != 0)
            offlinePrinterStatus = printerStatuses.offlinePrinters;

        if (printerStatuses.lowRibbonPrinters.length != 0)
            lowRibbonPrinterStatus = printerStatuses.lowRibbonPrinters;

        if (printerStatuses.onlinePrinters.length != 0)
            onlinePrinterStatus = printerStatuses.onlinePrinters;

        
        if (communicationPrinterStatus.length == 0) {
            $('#communicationPrinter').hide();
        } else {
            $('#communicationPrinter').show();
        }

        if (offlinePrinterStatus.length == 0) {
            $('#offlinePrinter').hide();
        } else {
            $('#offlinePrinter').show();
        }

        if (lowRibbonPrinterStatus.length == 0) {
            $('#lowRibbonPrinter').hide();
        } else {
            $('#lowRibbonPrinter').show();
        }

        if (onlinePrinterStatus.length == 0) {
            $('#onlinePrinter').hide();
        } else {
            $('#onlinePrinter').show();
        }

        $('#MonitorPanel').removeClass('wobblebar-loader');
        offlineData(offlinePrinterStatus);
        lowRibbonData(lowRibbonPrinterStatus);
        onlineData(onlinePrinterStatus);
        noCommunicationData(communicationPrinterStatus);
    };
    $.connection.hub.start().done(function () {
        printerMonitoringHub.server.getLatestPrinterStatus();
    });
}

function noCommunicationData(printerData) {

    try {
        
        if ($.fn.DataTable.isDataTable('#communicationproperties')) {
            var table = $('#communicationproperties').DataTable();
            table.clear().draw();
            table.rows.add(printerData); // Add new data
            table.columns.adjust().draw(); // R            
        }
        else {
            var table = $('#communicationproperties').DataTable({

                "processing": true,

                "data": printerData,

                "columns": [
                    { "data": "branchName" },
                    { "data": "ribbonCount" },
                    { "data": "printedCards" },
                    { "data": "dateofReport" }
                ],

                "order": [[1, "asc"]],

                dom: 'Bfrtip',

                buttons: [
                    {
                        extend: 'copyHtml5',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                {
                    extend: 'csvHtml5',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'pdfHtml5',
                    exportOptions: {
                        columns: ':visible'
                    }
                }
                ]
            });


        }
    } catch (err) {
        alert(err);
    }
}

function offlineData(printerData) {

    try {

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
                    { "data": "ribbonCount" },
                    { "data": "printedCards" },
                    { "data": "dateofReport" }
                ],

                "order": [[1, "asc"]],

                dom: 'Bfrtip',

                buttons: [
                    {
                        extend: 'copyHtml5',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                {
                    extend: 'csvHtml5',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'pdfHtml5',
                    exportOptions: {
                        columns: ':visible'
                    }
                }
                ]
            });


        }
    } catch (err) {
        alert(err);
    }
}


function lowRibbonData(printerData) {

    try {

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
                    { "data": "ribbonCount" },
                    { "data": "printedCards" },
                    { "data": "dateofReport" }
                ],

                "order": [[1, "asc"]],

                dom: 'Bfrtip',

                buttons: [
                    {
                        extend: 'copyHtml5',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                {
                    extend: 'csvHtml5',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'pdfHtml5',
                    exportOptions: {
                        columns: ':visible'
                    }
                }
                ]
            });
        }
    } catch (err) {
        alert(err);
    }
}

function onlineData(printerData) {

    try {
        
        if ($.fn.DataTable.isDataTable('#highproperties')) {
            var table = $('#highproperties').DataTable();
            table.clear().draw();
            table.rows.add(printerData); // Add new data
            table.columns.adjust().draw(); // R            
        }
        else {
            var table = $('#highproperties').DataTable({

                "processing": true,

                "data": printerData,

                "columns": [
                    { "data": "branchName" },
                    { "data": "ribbonCount" },
                    { "data": "printedCards" },
                    { "data": "dateofReport" }
                ],

                "order": [[1, "asc"]],

                dom: 'Bfrtip',

                buttons: [
                    {
                        extend: 'copyHtml5',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                {
                    extend: 'csvHtml5',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'pdfHtml5',
                    exportOptions: {
                        columns: ':visible'
                    }
                }
                ]
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
