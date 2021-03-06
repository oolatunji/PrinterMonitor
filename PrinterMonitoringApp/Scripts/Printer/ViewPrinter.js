﻿$(document).ready(function () {
    try {
        var currentUrl = window.location.href;
        var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
        var userFunctions = user.Function;

        var exist = false;
        $.each(userFunctions, function (key, userfunction) {
            var link = settingsManager.websiteURL.trimRight('/') + userfunction.PageLink;
            if (currentUrl == link) {
                exist = true;
            }
        });

        if (!exist)
            window.location.href = '../System/UnAuthorized';
        else
            getBranchAndDisplayPrinters();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Printer Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};


function getBranchAndDisplayPrinters() {
    try {
        $.ajax({
            url: settingsManager.websiteURL + 'api/BranchAPI/RetrieveBranches',
            type: 'GET',
            async: true,
            cache: false,
            success: function (branchResponse) {
                var branches = [];
                branches = branchResponse.data;

                getPrinters(branches);
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "Printer Management");
            }
        });

    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Printer Management");
    }
}

function getPrinters(branches) {

    $('#example tfoot th').each(function () {
        var title = $('#example tfoot th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    var table = $('#example').DataTable({

        "processing": true,

        "ajax": settingsManager.websiteURL + 'api/PrinterAPI/RetrievePrinters',

        "columns": [
            {
                "className": 'edit-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            { "data": "PrinterSrNo" },
            { "data": "PrinterName" },
            { "data": "PrinterBrand" },
            { "data": "Branch.Name" },
            { "data": "DateofEnrollment" },
            {
                "data": "Branch.ID",
                "visible": false
            },
            {
                "data": "ID",
                "visible": false
            }
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

    $('#example tbody').on('click', 'td.edit-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);

        function closeAll() {
            var e = $('#example tbody tr.shown');
            var rows = table.row(e);
            if (tr != e) {
                e.removeClass('shown');
                rows.child.hide();
            }
        }

        if (row.child.isShown()) {
            closeAll();
        }
        else {
            closeAll();

            row.child(format(row.data(), branches)).show();
            tr.addClass('shown');
        }
    });

    $("#example tfoot input").on('keyup change', function () {
        table
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
};

$(document).ready(function () {
    $('#dataTables-example').DataTable({
        responsive: true
    });
});

function refreshResult() {
    try {
        var table = $('#example').DataTable();
        table.ajax.reload();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Functions Management");
    }
}

function format(d, branches) {
    var table = '<table width="100%" class="cell-border" cellpadding="5" cellspacing="0" border="2" style="padding-left:50px;">';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Printer Serial No:</td>';
    table += '<td><input class="form-control" placeholder="Enter Printer Serial Number" id="printerSrNo" value="' + d.PrinterSrNo + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Printer Name:</td>';
    table += '<td><input class="form-control" placeholder="Enter Printer Name" id="printerName" value="' + d.PrinterName + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Printer Brand:</td>';
    table += '<td><input class="form-control" placeholder="Enter Printer Brand" id="printerBrand" value="' + d.PrinterBrand + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Branch:</td>';
    table += '<td><select class="form-control" name="branch" id="printerBranch">';
    table += '<option value="">Select Branch</option>';
    $.each(branches, function (key, value) {
        if (d.Branch.ID == value.ID)
            table += '<option selected="selected" value="' + value.ID + '">' + value.Name + '</option>';
        else
            table += '<option value="' + value.ID + '">' + value.Name + '</option>';
    });
    table += '</select></td></tr>';
    table += '<tr>';
    table += '<td style="display:none">ID:</td>';
    table += '<td style="display:none"><input class="form-control" id="id" value="' + d.ID + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Calibri;"></td>';
    table += '<td><button type="button"  id="updateBtn" class="btn btn-red" style="float:right;" onclick="update();"><i class="fa fa-cog"></i> Update</button></td>';
    table += '</tr>';
    table += '</table>';

    return table;
}

function update() {
    try {
        $('#updateBtn').html('<i class="fa fa-spinner fa-spin"></i> Updating...');
        $("#updateBtn").attr("disabled", "disabled");

        var printerSrNo = $('#printerSrNo').val();
        var printerName = $('#printerName').val();
        var printerBrand = $('#printerBrand').val();
        var printerBranch = $('#printerBranch').val();
        var id = $('#id').val();

        var data = { PrinterSrNo: printerSrNo, PrinterName: printerName, PrinterBrand: printerBrand, BranchID: printerBranch, ID: id };

        $.ajax({
            url: settingsManager.websiteURL + 'api/PrinterAPI/UpdatePrinter',
            type: 'PUT',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                displayMessage("success", response, "Printer Management");
                refreshResult();
                $("#updateBtn").removeAttr("disabled");
                $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "Printer Management");
                $("#updateBtn").removeAttr("disabled");
                $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Printer Management");
        $("#updateBtn").removeAttr("disabled");
        $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
    }
}