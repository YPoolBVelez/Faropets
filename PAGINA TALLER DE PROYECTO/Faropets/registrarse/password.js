const password = document.querySelector("#password");
const warning = document.querySelector("#warning");

password.addEventListener("keyup", function (e) {
    if (e.getModifierState("CapsLock")) {
        warning.style.display = "block";
    } else {
        warning.style.display = "none";
    }
    console.log("Password: " + password.value);
});

const pass2 = document.querySelector("#pass2");
const peligto = document.querySelector("#peligro");

password.addEventListener("keyup", function (e) {
    if (e.getModifierState("CapsLock")) {
        peligro.style.display = "block";
    } else {
        peligro.style.display = "none";
    }
    console.log("password: " + pass2.value);
});


$(document).ready(function () {

    $('#vpassword').keyup(function () {

        var pass1 = $('#password').val();
        var pass2 = $('#vpassword').val();

        if (pass1 == pass2) {
            $('#error2').css("background", "url(images/check.png)");
        } else {
            $('#error2').css("background", "url(images/check-.png)");
        }

    });

    $(function () {

        var mayus = new RegExp("^(?=.*[A-Z])");
        var special = new RegExp("^(?=.*[!@#$%&*])");
        var numbers = new RepExp("^(?=.*[0-9])");
        var minus = new RepExp("^(?.*[a-z])");
        var len = new RepExp("^(?.({8,})");

    });


});
