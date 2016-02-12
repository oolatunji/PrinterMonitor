$(document).ready(function () {
    var currentUrl = window.location.href;
    var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
    var userFunctions = user.Function;

    var exist = false;
    $.each(userFunctions, function (key, userfunction) {
        var link = settingsManager.websiteURL.trimRight('/') + userfunction.PageLink;
        if (currentUrl == link) {
            exist = true;
        }
    });

    if (!exist)
        window.location.href = '../System/UnAuthorized';
    else
        getSystemSettings();
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function feedTimes() {
    var times = [];
    for (var i = 1; i <= 15; i++) {
        times.push(i);
    }
    return times;
}

function communicationTimes() {
    var times = [];
    for (var i = 5; i <= 60; i = i + 5) {
        times.push(i);
    }
    return times;
}

function getSystemSettings() {
    try {

        //Get System Settings
        $.ajax({
            url: settingsManager.websiteURL + 'api/SystemAPI/GetSystemSettings',
            type: 'GET',
            async: true,
            cache: false,
            success: function (settings) {

                $('#applicationURL').val(settings.GeneralSettings.ApplicationUrl);
                $('#bankName').val(settings.GeneralSettings.Organization);
                $('#applicationName').val(settings.GeneralSettings.ApplicationName);
                $('#logFilePath').val(settings.GeneralSettings.LogFilePath);

                $('#useSmartCardAuthentication').html('');
                var optionsHtml = "";
                if (settings.GeneralSettings.UseSmartCardAuthentication == 'true') {
                    optionsHtml += '<option selected="selected" value="true">True</option>';
                    optionsHtml += '<option value="false">False</option>';
                    $('#useSmartCardAuthentication').append(optionsHtml);
                } else {
                    optionsHtml += '<option value="true">True</option>';
                    optionsHtml += '<option selected="selected" value="false">False</option>';
                    $('#useSmartCardAuthentication').append(optionsHtml);
                }

                $('#printerFeedsPollingTime').html('');
                var timesHtml = "";
                var times = feedTimes();
                $.each(times, function (key, value) {
                    if (settings.GeneralSettings.PrinterFeedsPollingTime == value) {
                        timesHtml += '<option selected="selected" value="' + value + '">' + value + '</option>';
                    } else {
                        timesHtml += '<option value="' + value + '">' + value + '</option>';
                    }
                });
                $('#printerFeedsPollingTime').append(timesHtml);

                $('#timeToCheckForNoCommunication').html('');
                var communicationTimesHtml = "";
                var communicationtimes = communicationTimes();
                $.each(communicationtimes, function (key, value) {
                    if (settings.GeneralSettings.TimeToCheckForNoCommunication === value) {
                        communicationTimesHtml += '<option selected="selected" value="' + value + '">' + value + '</option>';
                    } else {
                        communicationTimesHtml += '<option value="' + value + '">' + value + '</option>';
                    }
                });
                $('#timeToCheckForNoCommunication').append(communicationTimesHtml);

                $('#fromEmailAddress').val(settings.MailSettings.FromEmailAddress);
                $('#smtpUsername').val(settings.MailSettings.SmtpUsername);
                $('#smtpPassword').val(settings.MailSettings.SmtpPassword);
                $('#smtpServer').val(settings.MailSettings.SmtpHost);
                $('#smtpPort').val(settings.MailSettings.SmtpPort);

                $('#databaseServer').val(settings.DatabaseSettings.DatabaseServer);
                $('#databaseName').val(settings.DatabaseSettings.DatabaseName);
                $('#databaseUser').val(settings.DatabaseSettings.DatabaseUser);
                $('#databasePassword').val(settings.DatabaseSettings.DatabasePassword);
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "System Management");
            }
        });

    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "System Management");
    }
}

function configureSystem() {
    try {

        var websiteUrl = $('#applicationURL').val();
        var organization = $('#bankName').val();
        var applicationName = $('#applicationName').val();
        var fromEmailAddress = $('#fromEmailAddress').val();
        var smtpUsername = $('#smtpUsername').val();
        var smtpPassword = $('#smtpPassword').val();
        var smtpHost = $('#smtpServer').val();
        var smtpPort = $('#smtpPort').val();
        var databaseServer = $('#databaseServer').val();
        var databaseName = $('#databaseName').val();
        var databaseUser = $('#databaseUser').val();
        var databasePassword = $('#databasePassword').val();
        var useSmartCardAuthentication = $('#useSmartCardAuthentication').val();
        var printerFeedsPollingTime = $('#printerFeedsPollingTime').val();
        var timeToCheckForNoCommunication = $('#timeToCheckForNoCommunication').val();

        if (websiteUrl == "") {
            displayMessage("error", 'Kindly enter Application Url', "System Management");
        } else {
            var acknowledge = confirm("Are you sure you want to configure the System with the captured settings?");
            if (acknowledge) {
                $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Configuring System...');
                $("#addBtn").attr("disabled", "disabled");

                var data = { WebsiteUrl: websiteUrl, Organization: organization, ApplicationName: applicationName, FromEmailAddress: fromEmailAddress, SmtpUsername: smtpUsername, SmtpPassword: smtpPassword, SmtpHost: smtpHost, SmtpPort: smtpPort, DatabaseServer: databaseServer, DatabaseName: databaseName, DatabaseUser: databaseUser, DatabasePassword: databasePassword, UseSmartCardAuthentication: useSmartCardAuthentication, PrinterFeedsPollingTime: printerFeedsPollingTime, TimeToCheckForNoCommunication: timeToCheckForNoCommunication };

                $.ajax({
                    url: websiteUrl + 'api/SystemAPI/ConfigureSystem',
                    type: 'POST',
                    data: data,
                    processData: true,
                    async: true,
                    cache: false,
                    success: function (response) {
                        displayMessage("success", 'System configured successfully.', "System Management");
                        
                        $("#addBtn").removeAttr("disabled");
                        $('#addBtn').html('<i class="fa fa-cog"></i> Configure');
                    },
                    error: function (xhr) {
                        if (xhr.status == 404)
                            displayMessage("error", 'Error experienced: Incorrect Application Url.', "System Management");
                        else
                            displayMessage("error", 'Error experienced: ' + xhr.responseText, "System Management");
                        console.log(xhr);
                        $("#addBtn").removeAttr("disabled");
                        $('#addBtn').html('<i class="fa fa-cog"></i> Configure');
                    }
                });
            } else {
                displayMessage("info", 'System Configuration Cancelled', "System Management");
            }
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "System Management");
        console.log(err);
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-cog"></i> Configure');
    }
}