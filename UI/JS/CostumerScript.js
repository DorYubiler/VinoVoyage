
// Attaches a submit event listener to the payment form.
$(document).ready(function () {
    $('#paymentForm').on('submit.paymentForm', checkPayment);
});

// Validates the payment form fields before submission.
// Extracts and validates form values against regular expressions.
// Displays an error message for the first validation failure encountered.
// On successful validation, triggers the payment confirmation popup and sends the form data to the server via AJAX.
function checkPayment(event) {
    event.preventDefault();
    $("#paymentvalidationErrors").empty();
    var cvv = $('#cvv').val();
    var addressName = $('#username').val();
    var cardNumber = $('#ccnum').val();
    var city = $('#city').val();
    var street = $('#cstreet').val();
    var regexCvv = /^\d{3}$/;
    var regexCardNumber = /^\d{16}$/;
    var regexCity = /\d/;
    var regexStreet = /^[a-zA-Z0-9\s]+$/;;
    var regexNames = /^[a-zA-Z\s]+$/;
    if (!regexCvv.test(cvv)) {
        $("#paymentValidationErrors").html("Invalid CVV").show();
        return;
    }
    if (!regexNames.test(addressName)) {
        $("#paymentValidationErrors").html("Invalid shipping name").show();
        return;
    }
    if (!regexCardNumber.test(cardNumber)) {
        $("#paymentValidationErrors").html("Invalid card number").show();
        return;
    }
    if (!regexStreet.test(street)) {
        $("#paymentValidationErrors").html("Invalid street").show();
        return;
    }
    if (regexCity.test(city)) {
        $("#paymentValidationErrors").html("Invalid city").show();
        return;
    }
    $.ajax({
        type: "POST",
        url: "/Customer/Payment",
        data: {
            cityAddress: city
        },
        success: function (response) {
            if (response.success) {
                toggleConfirmPopup('ShipConfPopup');
                setTimeout(function () {
                    window.location.href = response.redirectUrl;
                }, 3000);
            } else {
                $("#paymentValidationErrors").html("Error at proccess payment.").show();
            }
        },
        error: function () {
            // Handle any errors that occur during the request.
            $("#paymentValidationErrors").html("An error occurred. Please try again.").show();
        }
    });

}


/*------------------------buynow----------------------------*/

// Validates the 'Buy Now' form fields before submission.
// Similar validation logic as checkPayment, tailored for the 'Buy Now' functionality.
// ... Your existing validation and AJAX call logic ...
function checkBuynow() {
    event.preventDefault();
    $("#buynowvalidationErrors").empty();

    var cvv = $('#bcvv').val();
    var addressName = $('#busername').val();
    var cardNumber = $('#bccnum').val();
    var bcity = $('#bcity').val();
    var street = $('#bcstreet').val();

    var regexCvv = /^\d{3}$/;
    var regexCardNumber = /^\d{16}$/;
    var regexCity = /\d/;
    var regexStreet = /^[a-zA-Z0-9\s]+$/;
    var regexNames = /^[a-zA-Z\s]+$/;

    if (!regexCvv.test(cvv)||cvv=='null'||cvv=='') {
        $("#buynowvalidationErrors").html("Invalid CVV").show();
        return;
    }
    if (!regexNames.test(addressName) || addressName == 'null' || addressName == '') {
        $("#buynowvalidationErrors").html("Invalid shipping name").show();
        return;
    }
    if (!regexCardNumber.test(cardNumber) || cardNumber == 'null' || cardNumber == '') {
        $("#buynowvalidationErrors").html("Invalid card number").show();
        return;
    }
    if (!regexStreet.test(street) || street == 'null' || street == '') {
        $("#buynowvalidationErrors").html("Invalid street").show();
        return;
    }
    if (regexCity.test(city) || city == 'null' || city == '') {
        $("#buynowvalidationErrors").html("Invalid city").show();
        return;
    } 
    $.ajax({
        type: "POST",
        url: "/Customer/Buynow",
        data: {
            CityAddress: bcity
        },
        success: function (response) {
            if (response.success) {
                toggleConfirmPopup('ShipConfPopup');
                setTimeout(function () {
                    window.location.href = response.redirectUrl;
                }, 3000);
                
            } else {
                $("#buynowvalidationErrors").html("Error at proccess payment.").show();
            }
        },
        error: function () {
            // Handle any errors that occur during the request.
            $("#buynowvalidationErrors").html("An error occurred. Please try again.").show();
        }
    });
}

// Validates the expiration date of a credit card against the current date.
function validateMonth(input) {
    var currentMonth = new Date().getMonth() + 1; 
    var currentYear = new Date().getFullYear();    
    var currentMonthFormatted = currentYear + "-" + (currentMonth < 10 ? "0" + currentMonth : currentMonth);
    var selectedDate = document.getElementById("expDate").value;
    if (selectedDate < currentMonthFormatted) {
        document.getElementById("expDateError").style.display = "block";
    } else {
        document.getElementById("expDateError").style.display = "none";
    }
}


// Opens a side navigation panel and closes another.
function openNav(idOpen, idColse) {
    document.getElementById(idOpen.id).style.width = "330px";
    document.getElementById(idColse.id).style.width = "0";
}

// Closes a side navigation panel.
function closeNav(idColse, idOpen) {
    document.getElementById(idColse.id).style.width = "0";
}

// Closes all popup modal windows.
function closeAllPopups() {
    var popups = document.getElementsByClassName("popup");
    for (var i = 0; i < popups.length; i++) {
        popups[i].style.display = "none";
    }
}

function toggleBuynow(proid, price) {
    closeAllPopups();
    var popup = document.getElementById("buynowPopup");
    popup.style.display = (popup.style.display === "none") ? "block" : "none";
    document.getElementById("popupTotal").innerText = 'Item price: ' + price;

}
function toggleOutOfStockPopup() {
    closeAllPopups();
    var popup = document.getElementById("outOfStockPopup");
    popup.style.display = (popup.style.display === "none") ? "block" : "none";    
}
function togglePaymentPopup() {
    closeAllPopups();
    var popup = document.getElementById("paymentPopup");
    popup.style.display = (popup.style.display === "none") ? "block" : "none";
}

function toggleTrackPopup() {
    closeAllPopups();
    var popup = document.getElementById("trackPopup");
    popup.style.display = (popup.style.display === "none") ? "block" : "none";
}
localStorage.setItem('welcomePopupShown', 'false');

function toggleConfirmPopup(divId) {
    closeAllPopups();
    var popup = document.getElementById(divId);
    popup.style.display = (popup.style.display === "none") ? "block" : "none";
} 
function getIn(divId) {
    document.getElementById(divId).style.display = 'none';
}

// Handles logout action.
function logout() {
    window.location.href = "/Customer/Logout";
}

$(document).ready(function () {
    // Assuming your form has an ID 'loginForm'
    $('#updateinfoForm').on('submit', checkChanges);
});

// Validates changes in user information before submission.
// Validates user information fields (email, password) before submission.
// Displays an error message for any validation failure.
// On successful validation, sends the updated user information to the server via AJAX.
// ... Your existing validation and AJAX call logic ...
function checkChanges(event) {
    event.preventDefault(); 
    $("#changeValidationErrors").empty();
    var uname = $('#usname').text();
    var mail = $('#Cemail').val(); 
    var pass1 = $('#Cpassword').val(); 
    var pass2 = $('#password2').val();
    if (mail.length != 0) {
        if (mail.length < 12 || mail.length > 30) {
            $("#changeValidationErrors").html("Invalid email").show();
            return;
        }
    }
    if (pass1.length != 0 && pass2 != 0) {
        if (pass1 != pass2) {
            $("#changeValidationErrors").html("Passwords do not match").show();
            return;
        }
        if (pass1.length < 6 || pass1.length > 10) {
            $("#changeValidationErrors").html("Invalid password").show();
            return;
        }
    }
    if (mail.length == 0 && pass1.length == 0 && pass2.length == 0) {
        $("#changeValidationErrors").html("No input was entered").show();
        return;
    }

    $.ajax({
        type: "POST",
        url: "/Customer/UpdateInfo",
        data: {
            username: uname,
            password: pass1, // Make sure the variable is correctly spelled here
            email: mail,

        },
        success: function (response) {
            if (response.success) {
                startCustogglePopup("changesConfPopup");

                setTimeout(function () {
                    window.location.reload();
                }, 2000);

            } else {
                // If login fails, keep the popup open and show error messages.
                $("#changeValidationErrors").html("Somthing went wrong.").show();
            }
        },
        error: function () {
            // Handle any errors that occur during the request.
            $("#changeValidationErrors").html("An error occurred. Please try again.").show();
        }
    });

}

// Toggles the visibility of the 'Update Information' popup window.
function changesInfotogglePopup() {
    closePopups();
    var popup = document.getElementById('updateInfoPopup');
    popup.style.display = (popup.style.display === "none") ? "table" : "none";


}