﻿$(document).ready(function () {
    try {

        $('#printerBranch').html('<option>Loading Branches...</option>');
        $('#printerBranch').prop('disabled', 'disabled');

        //Get Branches
        $.ajax({
            url: settingsManager.websiteURL + 'api/BranchAPI/RetrieveBranches',
            type: 'GET',
            async: true,
            cache: false,
            success: function (response) {
                $('#printerBranch').html('');
                $('#printerBranch').prop('disabled', false);
                $('#printerBranch').append('<option value="">Select Branch</option>');
                var functions = response.data;
                var html = '';
                $.each(functions, function (key, value) {
                    $('#printerBranch').append('<option value="' + value.ID + '">' + value.Name + '</option>');
                });
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "Printer Management");
            }
        });

    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Printer Management");
    }
});

function addPrinter() {
    try {

        $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Adding...');

        var printerUID = $('#printerUID').val();
        var printerSrNo = $('#printerSrNo').val();
        var printerName = $('#printerName').val();
        var printerBrand = $('#printerBrand').val();
        var printerBranch = $('#printerBranch').val();

        var data = { PrinterUID: printerUID, PrinterSrNo: printerSrNo, PrinterName: printerName, PrinterBrand: printerBrand, BranchID: printerBranch };
        $.ajax({
            url: settingsManager.websiteURL + 'api/PrinterAPI/SavePrinter',
            type: 'POST',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                displayMessage("success", response, "Printer Management");
                $('#printerUID').val('');
                $('#printerSrNo').val('');
                $('#printerName').val('');
                $('#printerBrand').val('');
                $('#printerBranch').val('');

                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-cog"></i> Add');
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "Printer Management");
                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-cog"></i> Add');
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Printer Management");
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-cog"></i> Add');
    }
}