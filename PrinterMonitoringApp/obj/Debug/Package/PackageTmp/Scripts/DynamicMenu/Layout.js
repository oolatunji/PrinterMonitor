$(document).ready(function () {

    //For index menu     
    var userLinks = [];
    var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
    var userFunctions = user.Function;

    $.each(userFunctions, function (key, userfunction) {
        userLinks.push(userfunction.PageLink);
    });

    sideMenu(userLinks);
});

function sideMenu(userLinks) {

    var links = $("[name='links']");
    $.each(links, function (key, value) {
        var link = value;
        var URL = $(link).attr('id');
        if (userLinks.indexOf(URL) < 0)
            $(link).parent().remove();
    });

    var links = $("[name='parent_links']");
    $.each(links, function (key, value) {
        var link = $(value).parent().children()[1];
        var URL = $(link).attr('id');
        if ($(value).parent().children().length == 2 && $(link).children().length == 0)
            $(value).parent().remove();
    });
}
