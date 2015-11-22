$(document).ready(function () {
    try {
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
            getUsersAndDisplaySmartCards();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Smart Card Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function getUsersAndDisplaySmartCards() {
    try {
        $.ajax({
            url: settingsManager.websiteURL + 'api/UserAPI/RetrieveUsers',
            type: 'GET',
            async: true,
            cache: false,
            success: function (response) {
                var users = [];
                users = response.data;
                getSmartCards(users);
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "Smart Card Management");
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Smart Card Management");
    }
}

function getSmartCards(users) {
    $('#example tfoot th').each(function () {
        var title = $('#example thead th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    if ($.fn.DataTable.isDataTable('#example')) {

        var table = $('#example').DataTable();
        table.ajax.url(settingsManager.websiteURL + 'api/SmartCardAPI/RetrieveSmartCards').load();

    } else {
        var table = $('#example').DataTable({

            "processing": true,

            "ajax": settingsManager.websiteURL + 'api/SmartCardAPI/RetrieveSmartCards',

            "columns": [
                {
                    "className": 'edit-control',
                    "orderable": false,
                    "data": null,
                    "defaultContent": ''
                },
                { "data": "SmartCardID" },
                { "data": "Allocated" },
                { "data": "User.TheUser" },
                {
                    "data": "User.ID",
                    "visible": false
                },
                {
                    "data": "ID",
                    "visible": false
                }
            ],

            "order": [[1, "asc"]],

            "sDom": 'T<"clear">lrtip',

            "oTableTools": {
                "sSwfPath": settingsManager.websiteURL + "images/copy_csv_xls_pdf.swf",
                "aButtons": [
                    {
                        "sExtends": "copy",
                        "sButtonText": "Copy to Clipboard",
                        "oSelectorOpts": { filter: 'applied', order: 'current' },
                        "mColumns": "visible"
                    },
                    {
                        "sExtends": "csv",
                        "sButtonText": "Save to CSV",
                        "oSelectorOpts": { filter: 'applied', order: 'current' },
                        "mColumns": "visible"
                    },
                    {
                        "sExtends": "xls",
                        "sButtonText": "Save for Excel",
                        "oSelectorOpts": { filter: 'applied', order: 'current' },
                        "mColumns": "visible"
                    }
                ]
            }
        });

        $('#example tbody').on('click', 'td.edit-control', function () {
            var tr = $(this).closest('tr');
            var row = table.row(tr);

            function closeAll() {
                var e = $('#example tbody tr.shown');
                var rows = table.row(e);
                if (tr != e) {
                    e.removeClass('shown');
                    rows.child.hide();
                }
            }

            if (row.child.isShown()) {
                closeAll();
            }
            else {
                closeAll();

                row.child(format(row.data(), users)).show();
                tr.addClass('shown');
            }
        });

        $("#example tfoot input").on('keyup change', function () {
            table
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
};

$(document).ready(function () {
    $('#dataTables-example').DataTable({
        responsive: true
    });
});

function format(d, users) {
    
    var table = '<table width="100%" class="cell-border" cellpadding="5" cellspacing="0" border="2" style="padding-left:50px;">';
    table += '<tr>';
    if (d.Allocated == true)
        table += '<th colspan="2" style="color:navy;width:20%;font-family:Arial;">De-Allocate Smart Card From User</th>';
    else
        table += '<th colspan="2" style="color:navy;width:20%;font-family:Arial;">Select User to Allocate Smart Card To</th>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">User:</td>';
    table += '<td><select class="form-control" name="user" id="userID">';
    if (d.Allocated == false) {
        table += '<option selected="selected" value="">Select User</option>';
        $.each(users, function (key, value) {
            table += '<option value="' + value.ID + '">' + value.Lastname + ' ' + value.Othernames + ' ' + '[' + value.Username + ']' + '</option>';
        });
    } else {
        table += '<option selected="selected" value="' + d.User.ID + '">' + d.User.TheUser + '</option>';
    }
    table += '</select></td></tr>';
    table += '<tr>';
    table += '<td style="display:none">ID:</td>';
    table += '<td style="display:none"><input class="form-control" id="id" value="' + d.ID + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="display:none">ID:</td>';
    if (d.Allocated == true)
        table += '<td style="display:none"><input class="form-control" id="status" value="false"/></td>';
    else
        table += '<td style="display:none"><input class="form-control" id="status" value="true"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Calibri;"></td>';
    table += '<td><button type="button"  id="updateBtn" class="btn btn-red" style="float:right;" onclick="update();"><i class="fa fa-cog"></i> Update</button></td>';
    table += '</tr>';
    table += '</table>';

    return table;
}

function update() {
    try {
        var userID = $('#userID').val();
        var smartCardID = $('#id').val();
        var status = $('#status').val();

        if (userID == "") {
            displayMessage("error", 'Kindly Select a User', "Smart Card Management");
        } else {
            $('#updateBtn').html('<i class="fa fa-spinner fa-spin"></i> Updating...');
            $("#updateBtn").attr("disabled", "disabled");

            var data = { SmartCardID: smartCardID, UserID: userID, Status: status };
            $.ajax({
                url: settingsManager.websiteURL + 'api/SmartCardAPI/UpdateSmartCard',
                type: 'PUT',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (response) {
                    displayMessage("success", response, "Smart Card Management");
                    getUsersAndDisplaySmartCards();
                    $("#updateBtn").removeAttr("disabled");
                    $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
                },
                error: function (xhr) {
                    displayMessage("error", 'Error experienced: ' + xhr.responseText, "Smart Card Management");
                    $("#updateBtn").removeAttr("disabled");
                    $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
                }
            });
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Smart Card Management");
        $("#updateBtn").removeAttr("disabled");
        $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
    }
}