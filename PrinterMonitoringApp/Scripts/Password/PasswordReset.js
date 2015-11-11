var username = "";
$(document).ready(function () {
    try{
        var encncryptedUsername = getQueryStringValue("rq");
        if (encncryptedUsername != "") {
            var data = { Username: encncryptedUsername };
            $.ajax({
                url: settingsManager.websiteURL + 'api/UserAPI/ConfirmUsername',
                type: 'POST',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (data) {
                    username = data.Username;
                },
                error: function (xhr) {
                    alert(xhr.responseText);
                }
            });
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Password Management");
        console.log(err);
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-user"></i> Reset');
    }
});

function resetPassword() {
    try {

        var new_password = $('#newPassword').val();
        var confirm_password = $('#confirmPassword').val();

        var err = customValidation(new_password, confirm_password);
        
        if (err != "") {
            displayMessage("error", "Error encountered: " + err, "Password Management");
        } else {
            $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Reset...');
            $("#addBtn").attr("disabled", "disabled");

            var data = { Username: username, Password: new_password };
            $.ajax({
                url: settingsManager.websiteURL + 'api/UserAPI/ChangePassword',
                type: 'PUT',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (data) {

                    $("#addBtn").removeAttr("disabled");
                    $('#addBtn').html('<i class="fa fa-user"></i> Reset');
                    $('#newPassword').val("");
                    $('#confirmPassword').val("");

                    var redirectToLogin = confirm("Password was changed successfully. Click OK to login with your new password.");
                    if (redirectToLogin) {
                        window.location = "../";
                    }
                    
                },
                error: function (xhr) {
                    displayMessage("error", "Error encountered: " + xhr.responseText, "Password Management");
                    $("#addBtn").removeAttr("disabled");
                    $('#addBtn').html('<i class="fa fa-user"></i> Reset');
                }
            });
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Password Management");
        console.log(err);
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-user"></i> Reset');
    }
}

function customValidation(newpassword, confirmnewpassword) {
    var err = "";
    var missingFields = "";
    var errCount = 0;
    if (newpassword == "") {
        missingFields += "New Password";
        errCount++;
    }
    if (confirmnewpassword == "") {
        missingFields += missingFields == "" ? "Confirm New Password" : ", Confirm New Password";
        errCount++;
    }

    if (missingFields != "" && errCount == 1) {
        err = "The field " + missingFields + " is required.";
    } else if (missingFields != "" && errCount > 1) {
        err = "The following fields " + missingFields + " are required.";
    } else if (missingFields == "" && (confirmnewpassword != newpassword)) {
        err = "Ensure that the two password fields are the same";
    }
    return err;
}

function getQueryStringValue(key) {
    return unescape(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + escape(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
}