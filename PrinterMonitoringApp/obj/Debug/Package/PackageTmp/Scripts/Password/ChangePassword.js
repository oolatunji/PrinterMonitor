function changePassword() {
    try {
        var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));

        var username = user.Username;
        var new_password = $('#newPassword').val();
        var confirm_password = $('#confirmPassword').val();

        if (username === null || username == "") {
            window.location = "../";
            alert("Your session has expired. Kindly login again.")
        } else {
            var err = passwordValidation(new_password, confirm_password);

            if (err != "") {
                displayMessage("error", "Error encountered: " + err, "Password Management");
            } else {

                $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Updating...');
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
                        $('#addBtn').html('<i class="fa fa-user"></i> Change');

                        if (window.sessionStorage.getItem("loggedInUser") != null)
                            window.localStorage.removeItem("loggedInUser");

                        window.location.href = "../";

                        alert("Password was changed successfully. You will be redirected shorthly to login with your new password.");

                        
                    },
                    error: function (xhr) {
                        displayMessage("error", "Error encountered: " + xhr.responseText, "Password Management");
                        $("#addBtn").removeAttr("disabled");
                        $('#addBtn').html('<i class="fa fa-user"></i> Change');
                    }
                });
            }
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Password Management");
        console.log(err);
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-user"></i> Change');
    }
}

function passwordValidation(newpassword, confirmnewpassword) {
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