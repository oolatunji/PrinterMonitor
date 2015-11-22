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
            getFunctionsAndDisplayRoles();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Roles Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};


function getFunctionsAndDisplayRoles() {
    try {        
        $.ajax({
            url: settingsManager.websiteURL + 'api/FunctionAPI/RetrieveFunctions',
            type: 'GET',
            async: true,
            cache: false,
            success: function (response) {
                var functions = [];
                functions = response.data;
                getRoles(functions);
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "Roles Management");                
            }            
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Roles Management");       
    }
}

function getRoles(functions) {
    $('#example tfoot th').each(function () {
        var title = $('#example thead th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    if ($.fn.DataTable.isDataTable('#example')) {

        var table = $('#example').DataTable();
        table.ajax.url(settingsManager.websiteURL + 'api/RoleAPI/RetrieveRoles').load();

    } else {
        var table = $('#example').DataTable({

            "processing": true,

            "ajax": settingsManager.websiteURL + 'api/RoleAPI/RetrieveRoles',

            "columns": [
                {
                    "className": 'details-control',
                    "orderable": false,
                    "data": null,
                    "defaultContent": ''
                },
                {
                    "className": 'edit-control',
                    "orderable": false,
                    "data": null,
                    "defaultContent": ''
                },
                { "data": "Name" },
                {
                    "data": "Functions",
                    "visible": false
                },
                {
                    "data": "ID",
                    "visible": false
                }
            ],

            "order": [[2, "asc"]],

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

        $('#example tbody').on('click', 'td.details-control', function () {
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

                row.child(formatDetails(row.data(), functions)).show();
                tr.addClass('shown');
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

                row.child(format(row.data(), functions)).show();
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

function format(d, allfunctions) {
    var table = '<table width="100%" class="cell-border" cellpadding="5" cellspacing="0" border="2" style="padding-left:50px;">';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Name:</td>';
    table += '<td><input class="form-control" placeholder="Enter Role Name" id="name" value="' + d.Name + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Functions:</td>';
    table += '<td>';
    $.each(allfunctions, function (key, value) {
        var checked = false;
        $.each(d.Functions, function (key1, value1) {
            if (value.ID == value1) {
                checked = true;
            }
        });
        if (checked)
            table += '<input type="checkbox" name="functions" checked="checked" style="font-size:12px" value="' + value.ID + '" />' + value.Name + '<br/>';
        else
            table += '<input type="checkbox" name="functions" style="font-size:12px" value="' + value.ID + '" />' + value.Name + '<br/>';
    });
    table += '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="display:none">ID:</td>';
    table += '<td style="display:none"><input class="form-control" id="id" value="' + d.ID + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Calibri;"></td>';
    table += '<td><button type="button"  id="updateBtn" class="btn btn-red" style="float:right;" onclick="update();"><i class="fa fa-cog"></i> Update</button></td>';
    table += '</tr>';
    table += '</table>';

    return table;
}

function formatDetails(d, allfunctions) {
    var table = '<table width="100%" class="cell-border" cellpadding="5" cellspacing="0" border="2" style="padding-left:50px;">';
    table += '<tr>';
    table += '<th style="color:navy;width:20%;font-family:Arial;">Functions</th>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="font-family:Arial;">';   
    $.each(allfunctions, function (key, value) {
        var checked = false;
        $.each(d.Functions, function (key1, value1) {
            if (value.ID == value1) {
                checked = true;
            }
        });
        if (checked)
            table += value.Name + '<br/>';
    });
    table += '</td>';
    table += '</tr>';
    table += '</table>';
    return table;
}

function update() {
    try{
        $('#updateBtn').html('<i class="fa fa-spinner fa-spin"></i> Updating...');
        $("#updateBtn").attr("disabled", "disabled");

        var name = $('#name').val();

        var roleFunctions = [];
        $("input:checkbox[name=functions]:checked").each(function () {
            var roleFunction = {};
            var _function = $(this).val();
            roleFunction = { FunctionID: _function };
            roleFunctions.push(roleFunction);
        });

        var id = $('#id').val();
    
        var data = { Name: name, RoleFunctions: roleFunctions, ID: id };
        $.ajax({
            url: settingsManager.websiteURL + 'api/RoleAPI/UpdateRole',
            type: 'PUT',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                displayMessage("success", response, "Roles Management");
                getFunctionsAndDisplayRoles();
                $("#updateBtn").removeAttr("disabled");
                $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "Roles Management");
                $("#updateBtn").removeAttr("disabled");
                $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Roles Management");
        $("#updateBtn").removeAttr("disabled");
        $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
    }
}