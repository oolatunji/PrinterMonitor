function configure() {
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

        if (websiteUrl == "") {
            displayMessage("error", 'Kindly enter Application Url', "System Management");
        } else {
            var acknowledge = confirm("Are you sure you want to configure the System with the captured settings?");
            if (acknowledge) {
                $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Configuring System...');

                var data = { WebsiteUrl: websiteUrl, Organization: organization, ApplicationName: applicationName, FromEmailAddress: fromEmailAddress, SmtpUsername: smtpUsername, SmtpPassword: smtpPassword, SmtpHost: smtpHost, SmtpPort: smtpPort, DatabaseServer: databaseServer, DatabaseName: databaseName, DatabaseUser: databaseUser, DatabasePassword: databasePassword };

                $.ajax({
                    url: websiteUrl + 'api/SystemAPI/ConfigureSystem',
                    type: 'POST',
                    data: data,
                    processData: true,
                    async: true,
                    cache: false,
                    success: function (response) {

                        var login = confirm("System configured successfully. Click OK to proceed to Login Page");
                        if (login) {
                            window.location = '../Home/Dashboard';
                        }
                        $("#addBtn").removeAttr("disabled");
                        $('#addBtn').html('<i class="fa fa-cog"></i> Configure');
                    },
                    error: function (xhr) {
                        displayMessage("error", 'Error Experienced, ' + xhr.responseText, "System Management");
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