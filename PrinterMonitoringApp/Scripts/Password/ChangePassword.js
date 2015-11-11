function changePassword() {
    var username = window.localStorage.getItem("loggedInUsername");
    var new_password = $('#new_password').val();
    var confirm_password = $('#confirm_password').val();

    if (username === null || username == "") {
        window.location = "../";
        alert("Your session has expired. Kindly login again.")
    } else {
        var err = customValidation(new_password, confirm_password);
        if (err != "") {
            noty({ text: err, layout: 'bottomRight', type: 'warning', timeout: 10000 });
        } else {
            $("#updateBtn").attr("disabled", "disabled");

            var data = { Username: username, NewPassword: new_password };
            $.ajax({
                url: settingsManager.websiteURL + 'api/UserAPI/ChangePassword',
                type: 'PUT',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (data) {
                    //Remove local storages before redirecting to the login page
                    window.localStorage.removeItem("loggedInUsername");

                    if (window.localStorage.removeItem("loggedInUserID") != null)
                        window.localStorage.removeItem("loggedInUserID");

                    window.location.href = "../";

                    alert("Password was changed successfully. You will be redirected shorthly to login with your new password.");

                    $("#updateBtn").removeAttr("disabled");
                },
                error: function (xhr) {
                    noty({ text: 'Error experienced: ' + xhr.responseText, layout: 'bottomRight', type: 'warning', timeout: 10000 });
                    $("#updateBtn").removeAttr("disabled");
                }
            });
        }
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