var username = "";
$(document).ready(function () {
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
});

function resetPassword() {
    var new_password = $('#new_password').val();
    var confirm_password = $('#confirm_Password').val();
    var err = customValidation(new_password, confirm_password);
    if (err != "") {
        alert(err);
    } else {
        var data = { Username: username, NewPassword: new_password };
        $.ajax({
            url: settingsManager.websiteURL + 'api/UserAPI/ChangePassword',
            type: 'PUT',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (data) {
                window.location = "../AdminLogin/SignIn";
                alert("Password was changed successfully. You will be redirected shorthly to login with your new password.");
            },
            error: function (xhr) {
                alert(xhr.responseText);
            }
        });
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