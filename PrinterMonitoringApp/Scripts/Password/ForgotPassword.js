function forgotPassword() {
    var username = $('#username').val();
    var err = customForgotPasswordValidation(username);
    if (err != "") {
        alert(err);
    } else {
        $("#updateBtn").attr("disabled", "disabled");
        var data = { Username: username };
        $.ajax({
            url: settingsManager.websiteURL + 'api/UserAPI/ForgotPassword',
            type: 'PUT',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (data) {
                alert("An email that contains a link to continue with your password reset has been sent to you.")
                $('#username').val("");
                $("#updateBtn").removeAttr("disabled");
            },
            error: function (xhr) {
                alert(xhr.responseText);
                $("#updateBtn").removeAttr("disabled");
            }
        });
    }
}

function customForgotPasswordValidation(username) {
    var err = "";
    var missingFields = "";

    if (username == "") {
        missingFields += "Username";
    }
    if (missingFields != "") {
        err = "The field " + missingFields + " is required.";
    }
    return err;
}