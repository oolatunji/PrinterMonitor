/*global $ */
$(document).ready(function () {
    "use strict";
    $('#form-validation').bootstrapValidator({
        message: 'This value is not valid',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {                        
            functionName: {
                validators: {
                    notEmpty: {
                        message: 'The Function Name is required and cannot be empty'
                    }
                }
            },
            pageLink: {
                validators: {
                    notEmpty: {
                        message: 'The Function Page Link is required and cannot be empty'
                    }
                }
            }
        }
    });
});

function addFunction() {
    try {

        $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Adding...');

        var functionName = $('#functionName').val();
        var functionPageLink = $('#functionPageLink').val();

        var data = { Name: functionName, PageLink: functionPageLink };
        $.ajax({
            url: settingsManager.websiteURL + 'api/FunctionAPI/SaveFunction',
            type: 'POST',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                displayMessage("success", response, "Functions Management");
                $('#functionName').val('');
                $('#functionPageLink').val('');
                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-cog"></i> Add');
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "Functions Management");
                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-cog"></i> Add');
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Functions Management");
    }
}