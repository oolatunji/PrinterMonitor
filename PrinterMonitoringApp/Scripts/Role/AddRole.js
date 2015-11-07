$(document).ready(function () {
    try {
        $('#dynamicfunctions').html('<p style="font-family:Calibri;color:red;"><i class="fa fa-cog fa-spin"></i> Loading functions...</p>');
        $.ajax({
            url: settingsManager.websiteURL + 'api/FunctionAPI/RetrieveFunctions',
            type: 'GET',
            async: true,
            cache: false,
            success: function (response) {
                var functions = response.data;
                var html = '';
                $.each(functions, function (key, value) {                   
                    html += '<input type="checkbox" name="functions" value="' + value.ID + '"/>' + value.Name + '<br/>';
                });
                $('#dynamicfunctions').html('');
                $('#dynamicfunctions').append(html);
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "Roles Management");
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Roles Management");
    }
});

function addRole() {
    try {

        $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Adding...');

        var roleName = $('#roleName').val();
        var roleFunctions = [];
        $("input:checkbox[name=functions]:checked").each(function () {
            var roleFunction = {};
            var _function = $(this).val();
            roleFunction = { FunctionID: _function };
            roleFunctions.push(roleFunction);
        });

        var data = { Name: roleName, RoleFunctions: roleFunctions };
        $.ajax({
            url: settingsManager.websiteURL + 'api/RoleAPI/SaveRole',
            type: 'POST',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                displayMessage("success", response, "Roles Management");
                $('#roleName').val('');
                $('#dynamicfunctions input[type=checkbox]').removeAttr('checked');
                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-cog"></i> Add');
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "Roles Management");
                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-cog"></i> Add');
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Roles Management");
    }
}