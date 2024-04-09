

//----script for slideshow functionality: arrows and automatic change-------
let slideIndex = 1;
let slideInterval; // Variable to store the interval ID

function showSlides(n = slideIndex) {
    let i;
    let slides = document.getElementsByClassName("mySlides");
    let dots = document.getElementsByClassName("dot");

    // Reset the interval every time showSlides is called to ensure consistent timing
    clearInterval(slideInterval);
    slideInterval = setInterval(function () { plusSlides(1); }, 7000);

    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }

    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }

    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
}
showSlides(slideIndex);

// Next/previous controls
function plusSlides(n) {
    showSlides(slideIndex += n);
}

// Thumbnail image controls
function currentSlide(n) {
    showSlides(slideIndex = n);

}
//----------------------------------------------------------------
//--------------------topnav-----------------------------------
function myFunction() {
    var x = document.getElementById("myTopnav");
    if (x.className === "topnav") {
        x.className += " responsive";
    } else {
        x.className = "topnav";
    }
}
//-------------------------------------------------------
//------------------login validation----------------------
$(document).ready(function () {
    // Assuming your form has an ID 'loginForm'
    $('#loginForm').on('submit', checkLogin);
});

function checkLogin(event) {
    event.preventDefault(); // Correctly call preventDefault on the event object
    $("#LoginValidationErrors").empty();
    $("#username, #password").removeClass("form-group");
    // Optionally, re-enable the submit button if disabled
    // Assuming 'username' and 'password' are the IDs of the input fields
    var uname = $('#username').val(); // Use jQuery to get the value
    var pass = $('#password').val(); // Use jQuery to get the value
    
    $.ajax({
        type: "POST",
        url: "/User/Login",
        data: {
            username: uname,
            password: pass // Make sure the variable is correctly spelled here
        },
        success: function (response) {
            if (response.success) {
                startCustogglePopup("LoginConfPopup");
                window.location.href = response.redirectUrl;
            } else {
                // If login fails, keep the popup open and show error messages.
                $("#LoginValidationErrors").html("Invalid username or password").show();
            }
        },
        error: function () {
            // Handle any errors that occur during the request.
            $("#LoginValidationErrors").html("An error occurred. Please try again.").show();
        }
    });

}
     

(function () {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = "https://bringthemhomenow.net/1.0.8/hostages-ticker.js";
    script.setAttribute(
        "integrity",
        "sha384-jQVW0E+wZK5Rv1fyN+b89m7cYY8txH4s3uShzHf1T51hdBTPo7yKL6Yizgr+Gp8C"
    );
    script.setAttribute("crossorigin", "anonymous");
    document.getElementsByTagName("head")[0].appendChild(script);
})();
//----------------------------------------------------------------
// ------JavaScript function to close all popups------------------
function closePopups() {
    var popups = document.getElementsByClassName("popup");
    for (var i = 0; i < popups.length; i++) {
        popups[i].style.display = "none";
    }
}
function togglePopup() {
    closePopups();
    var popup = document.getElementById("loginPopup");
    popup.style.display = (popup.style.display === "none") ? "block" : "none";
}
function toggleSignupPopup() {
    closePopups();
    var popup = document.getElementById("SignupPopup");
    popup.style.display = (popup.style.display === "none") ? "block" : "none";
}
//-----------------------------------------------------------------------------
//----- register valdation with ajax, test----------------------------------

$(document).ready(function () {
    console.log("jQuery is loaded");
    $('#signupForm').on('submit.signupForm', checkSignup);
});

function checkSignup(event) {
    event.preventDefault();
    $("#registervalidationErrors").empty();
    // Updated IDs to match the HTML

    var uname = $('#Regusername').val();
    
    if (uname.length < 3 || uname.length > 10 || uname.includes("guest")) {
        $("#registervalidationErrors").html("Invalid username").show();
        return;
    }
    var pass = $('#Regpassword').val();
    if (pass.length < 6 || pass.length > 10) {
        $("#registervalidationErrors").html("Invalid password").show();
        return;
    }
    var mail = $('#Regemail').val(); // Fixed selector
    var regex = /^[^\s@]+@[a-zA-Z]+\.(co\.il|com)$/;
    if (!(regex.test(mail))) {
        $("#registervalidationErrors").html("Invalid email").show();
        return;
    }
    startCustogglePopup("SignUConfPopup");

    $.ajax({
        type: "POST",
        url: "/User/SignUp",
        data: {
            username: uname,
            password: pass,
            email: mail
        },
        success: function (response) {
            if (response.success) {
                window.location.href = response.redirectUrl;
            } else {
                // If login fails, keep the popup open and show error messages.
                $("#registervalidationErrors").html(response.errorMsg).show();
            }
        },
        error: function () {
            // Handle any errors that occur during the request.
            $("#registervalidationErrors").html("An error occurred. Please try again.").show();
        }
    });
}

function redirectToBlank() {
    window.location.href = 'https://www.disney.com/';

}
//---------------signup confirm popup---------------------------------------
function startCustogglePopup(divId) {
    closePopups();
    var popup = document.getElementById((divId));
    popup.style.display = (popup.style.display === "none") ? "table" : "none";
    
    
}
//------------------------------Age verification---------------------------

function getIn() {
    document.getElementById('welcomePopup').style.display = 'none';
}

function startCustomer(divId) {
    document.getElementById(divId).style.display = 'none';

}

window.onload = function () {
    
    if (!localStorage.getItem('welcomePopupShown')) {
        document.getElementById('welcomePopup').style.display = 'block';
        localStorage.setItem('welcomePopupShown', 'true');
    }
}

function gotoGuest() {
    window.location.href='/User/LoginGuest';
}
