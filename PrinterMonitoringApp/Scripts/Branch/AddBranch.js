﻿function addBranch() {
    try {

        $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Adding...');
        $("#addBtn").attr("disabled", "disabled");

        var branchName = $('#branchName').val();
        var branchCode = $('#branchCode').val();
        var branchAddress = $('#branchAddress').val();

        var data = { Name: branchName, Code: branchCode, Address: branchAddress };
        $.ajax({
            url: settingsManager.websiteURL + 'api/BranchAPI/SaveBranch',
            type: 'POST',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                displayMessage("success", response, "Branch Management");
                $('#branchName').val('');
                $('#branchCode').val('');
                $('#branchAddress').val('');
                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-cog"></i> Add');
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "Branch Management");
                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-cog"></i> Add');
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Branch Management");
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-cog"></i> Add');
    }
}