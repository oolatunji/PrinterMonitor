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
            getBranches();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Branch Management");
    }
});


String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function getBranches() {

    $('#example tfoot th').each(function () {
        var title = $('#example thead th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });


    var table = $('#example').DataTable({

        "processing": true,

        "ajax": settingsManager.websiteURL + 'api/BranchAPI/RetrieveBranches',

        "columns": [
            {
                "className": 'edit-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            { "data": "Name" },
            { "data": "Code" },
            { "data": "Address" },
            {
                "data": "ID",
                "visible": false
            },
        ],

        "order": [[1, "asc"]],

        dom: 'Bfrtip',

        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'csvHtml5',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                }
            }
        ]
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

            row.child(format(row.data())).show();
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

function refreshResult() {
    try {
        var table = $('#example').DataTable();
        table.ajax.reload();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Branch Management");
    }
}

$(document).ready(function () {
    $('#dataTables-example').DataTable({
        responsive: true
    });
});

function format(d) {
    // `d` is the original data object for the row
    return '<table width="100%" class="cell-border" cellpadding="5" cellspacing="0" border="2" style="padding-left:50px;">' +
        '<tr>' +
            '<td style="color:navy;width:20%;font-family:Arial;">Name:</td>' +
            '<td><input class="form-control" placeholder="Enter Branch Name" id="branchName" value="' + d.Name + '"/></td>' +
        '</tr>' +
        '<tr>' +
            '<td style="color:navy;width:20%;font-family:Arial;">Code:</td>' +
            '<td><input class="form-control" placeholder="Enter Branch Code" id="branchCode" value="' + d.Code + '"/></td>' +
        '</tr>' +
         '<tr>' +
            '<td style="color:navy;width:20%;font-family:Arial;">Address:</td>' +
            '<td><input class="form-control" placeholder="Enter Branch Address" id="branchAddress" value="' + d.Address + '"/></td>' +
        '</tr>' +
        '<tr>' +
            '<td style="display:none">ID:</td>' +
            '<td style="display:none"><input class="form-control" id="id" value="' + d.ID + '"/></td>' +
        '</tr>' +
        '<tr>' +
            '<td style="color:navy;width:20%;font-family:Calibri;"></td>' +
            '<td><button type="button" id="updateBtn" class="btn btn-red" style="float:right;" onclick="update();"><i class="fa fa-cog"></i> Update</button></td>' +
        '</tr>' +
    '</table>';
}

function update() {

    try {
        $('#updateBtn').html('<i class="fa fa-spinner fa-spin"></i> Updating...');
        $("#updateBtn").attr("disabled", "disabled");

        var branchName = $('#branchName').val();
        var branchCode = $('#branchCode').val();
        var branchAddress = $('#branchAddress').val();
        var id = $('#id').val();

        var data = { Name: branchName, Code: branchCode, Address: branchAddress, ID: id };
        $.ajax({
            url: settingsManager.websiteURL + 'api/BranchAPI/UpdateBranch',
            type: 'PUT',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                displayMessage("success", response, "Branch Management");
                refreshResult();
                $("#updateBtn").removeAttr("disabled");
                $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "Branch Management");
                $("#updateBtn").removeAttr("disabled");
                $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Branch Management");
        $("#updateBtn").removeAttr("disabled");
        $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
    }
}
