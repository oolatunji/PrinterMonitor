﻿$(document).ready(function () {

    if (window.localStorage.getItem("loggedInUser") === null) {
        window.location = '../';
        alert("Your session has expired. Kindly login again.");
    } else {

        var user = JSON.parse(window.localStorage.getItem("loggedInUser"));
        $('#user').html(user.Username);
        $('#userRole').html(user.Role);
    }
});

function logout() {
    window.localStorage.removeItem("loggedInUser");
    window.location = ("../");
}