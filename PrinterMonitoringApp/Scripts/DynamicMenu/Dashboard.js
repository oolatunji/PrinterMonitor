$(document).ready(function () {
    //For index menu     
    var userLinks = [];
    var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
    var userFunctions = user.Function;

    $.each(userFunctions, function (key, userfunction) {
        userLinks.push(userfunction.PageLink);
    });

    dashboardMenu(userLinks);
});

function dashboardMenu(userLinks) {

    var links = $("[name='ilinks']");
    $.each(links, function (key, link) {
        var URL = $(link).attr('id');
        
        if (userLinks.indexOf(URL) < 0)
            $(link).remove();
    });

    var links = $("[name='iparent_link'] .panel-footer");
    var pa = $("[name='iparent_link']");
    $.each(links, function (key, value) {
        var length = $(value).children().length;
        if (length <= 2)
            $(pa[key]).remove();
    });
}
