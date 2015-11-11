﻿function LoginIn() {

    try {
        var username = $('#username').val();
        var password = $('#password').val();

        var err = loginValidation(username, password);
        if (err != "") {
            displayMessage("error", "Error encountered: " + err, "User Login");
        } else {

            $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Login...');
            $("#addBtn").attr("disabled", "disabled");

            var data = { Username: username, Password: password };
            $.ajax({
                url: settingsManager.websiteURL + 'api/UserAPI/AuthenticateUser',
                type: 'POST',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (data) {
                    //Remove local storages if they exist before adding new ones
                    if (window.localStorage.getItem("loggedInUser") != null)
                        window.localStorage.removeItem("loggedInUser");

                    //Add new local storages
                    var user = JSON.stringify(data);
                    
                    window.localStorage.setItem("loggedInUser", user);

                    window.location = ("Home/Dashboard");

                },
                error: function (xhr) {
                    displayMessage("error", "Error encountered: " + xhr.responseText, "User Login");
                    $("#addBtn").removeAttr("disabled");
                    $('#addBtn').html('<i class="fa fa-user"></i> Login');
                }
            });
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "User Login");
        console.log(err);
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-user"></i> Login');
    }
}

function loginValidation(username, password) {
    var err = "";
    var missingFields = "";
    var errCount = 0;
    if (username == "") {
        missingFields += "Username";
        errCount++;
    }
    if (password == "") {
        missingFields += missingFields == "" ? "Password" : ", Password";
        errCount++;
    }

    if (missingFields != "" && errCount == 1) {
        err = "The field " + missingFields + " is required.";
    } else if (missingFields != "" && errCount > 1) {
        err = "The following fields " + missingFields + " are required.";
    }
    return err;
}