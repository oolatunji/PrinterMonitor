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
    displayMessage("success", "Functions added successfully.", "Functions Management");
    $("#addBtn").removeAttr("disabled");
}