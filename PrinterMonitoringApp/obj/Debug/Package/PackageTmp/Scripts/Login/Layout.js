$(document).ready(function () {

    if (window.sessionStorage.getItem("loggedInUser") === null) {
        window.location = '../';
        alert("Your session has expired. Kindly login again.");
    } else {

        var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
        $('#theLoggedInUser').html(user.Username);
        $('#theLoggedInUserRole').html(user.Role);
    }
});

function logout() {
    window.sessionStorage.removeItem("loggedInUser");
    window.location = ("../");
}