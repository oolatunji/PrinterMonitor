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
        var noCommunicationPrinterStatus = [];

        $("#offlinePrinterPanel").html('');
        $("#lowRibbonPrinterPanel").html('');
        $("#onlinePrinterPanel").html('');
        $("#communicationPrinterPanel").html('');

        var lowRibbonPrinter = '';
        var offlinePrinter = '';
        var onlinePrinter = '';
        var communicationPrinter = '';

        $.each(printerStatuses, function (key, printerStatus) {
            if (printerStatus.overDue == 1) {

                communicationPrinter += '<div style="cursor:pointer;float:left;background-color:orange;width:auto;height:17px;margin:5px" data-toggle="modal" data-target="#modalHeaderLightOrange" >';
                communicationPrinter += '<div style="color:white;margin-left:5px;margin-right:5px;"><p class="blink" style="font-family:Calibri;font-weight:bold;">' + printerStatus.branchName + '</p></div>';
                communicationPrinter += '</div>';

                noCommunicationPrinterStatus.push(printerStatus);

            } else if (printerStatus.overDue == 0) {

                if (printerStatus.status == 0) {

                    offlinePrinter += '<div style="cursor:pointer;float:left;background-color:red;width:auto;height:17px;margin:5px" data-toggle="modal" data-target="#modalHeaderLightBlue" >';
                    offlinePrinter += '<div style="color:white;margin-left:5px;margin-right:5px;"><p class="blink" style="font-family:Calibri;font-weight:bold;">' + printerStatus.branchName + '</p></div>';
                    offlinePrinter += '</div>';

                    offlinePrinterStatus.push(printerStatus);

                } else if (printerStatus.status == 1) {

                    var ribbonStatus = printerStatus.ribbonStatus.toString().length < 2 ? "0" + printerStatus.ribbonStatus : printerStatus.ribbonStatus;
                    if (printerStatus.ribbonStatus <= 400) {
                        lowRibbonPrinter += '<div style="cursor:pointer;float:left;background-color:purple;width:200px;height:17px;margin:5px"  data-toggle="modal" data-target="#modalHeaderLightPurple">';
                        lowRibbonPrinter += '<div style="color:white;width:50%;margin-left:5px;margin-right:5px;float:left"><p class="blink" style="font-family:Calibri;font-weight:bold;">' + printerStatus.branchName + ' </p></div>';
                        lowRibbonPrinter += '<div style="color:white;margin-right:10px;float:left;"><span style="font-family:Calibri;font-weight:bold;margin-right:10px;">' + ribbonStatus + ' |</span>';
                        lowRibbonPrinter += '<span style="font-family:Calibri;font-weight:bold;">' + printerStatus.printedCards + '</span></div>';
                        lowRibbonPrinter += '</div>';

                        lowRibbonPrinterStatus.push(printerStatus);
                    }
                    else if (printerStatus.ribbonStatus > 400) {

                        onlinePrinter += '<div style="float:left;background-color:green;width:200px;height:17px;margin:5px">';
                        onlinePrinter += '<div style="color:white;width:50%;margin-left:5px;margin-right:10px;float:left"><p style="font-family:Calibri;font-weight:bold;">' + printerStatus.branchName + ' </p></div>';
                        onlinePrinter += '<div style="color:white;margin-right:10px;float:left;"><span style="font-family:Calibri;font-weight:bold;margin-right:10px;">' + ribbonStatus + ' |</span>';
                        onlinePrinter += '<span style="font-family:Calibri;font-weight:bold;">' + printerStatus.printedCards + '</span></div>';
                        onlinePrinter += '</div>';

                    }
                }
            }
        });

        if (communicationPrinter != '') {
            $('#communicationPrinter').show();
            $("#communicationPrinterPanel").html(communicationPrinter);
        } else if (communicationPrinter == '') {
            $('#communicationPrinter').hide();
        }

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
        noCommunicationData(noCommunicationPrinterStatus);

    };
    $.connection.hub.start().done(function () {
        printerMonitoringHub.server.getLatestPrinterStatus();
    });
}

function offlineData(printerData) {

    try {
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

function noCommunicationData(printerData) {

    try {

        $('#nocommunicationproperties tfoot th').each(function () {
            var title = $('#nocommunicationproperties tfoot th').eq($(this).index()).text();
            if (title != "")
                $(this).html('<input type="text" placeholder="Search ' + title + '" />');
        });

        if ($.fn.DataTable.isDataTable('#nocommunicationproperties')) {
            var table = $('#nocommunicationproperties').DataTable();
            table.clear().draw();
            table.rows.add(printerData); // Add new data
            table.columns.adjust().draw(); // R            
        }
        else {
            var table = $('#nocommunicationproperties').DataTable({

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
