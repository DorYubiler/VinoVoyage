// checkout functions
$(document).ready(function () {
    // Assuming your form has an ID 'loginForm'
    console.log("jQuery is loaded");
    $('#paymentForm').on('submit.paymentForm', checkPayment);
});

function checkPayment(event) {
    event.preventDefault();
    $("#registervalidationErrors").empty();

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
                window.location.href = response.redirectUrl;
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

function validateMonth(input) {
    var currentMonth = new Date().getMonth() + 1; // Get current month (1-12)
    var currentYear = new Date().getFullYear();    // Get current year
    var currentMonthFormatted = currentYear + "-" + (currentMonth < 10 ? "0" + currentMonth : currentMonth);

    var selectedDate = document.getElementById("expDate").value;

    if (selectedDate < currentMonthFormatted) {
        // Display error message if the selected date is in the past
        document.getElementById("expDateError").style.display = "block";
    } else {
        // Hide error message if the selected date is valid
        document.getElementById("expDateError").style.display = "none";
    }
}


// window to open, window to close 
function openNav(idOpen, idColse) {
    document.getElementById(idOpen.id).style.width = "330px";
    document.getElementById(idColse.id).style.width = "0";
    document.getElementById("main").style.marginRight = "330px";
}

// window to close, window to open 
function closeNav(idColse, idOpen) {
    document.getElementById(idColse.id).style.width = "0";
    document.getElementById("main").style.marginRight = "0";

}

function closeAllPopups() {
    var popups = document.getElementsByClassName("popup");
    for (var i = 0; i < popups.length; i++) {
        popups[i].style.display = "none";
    }
}
function togglePaymentPopup() {
    closeAllPopups();
    var popup = document.getElementById("paymentPopup");
    popup.style.display = (popup.style.display === "none") ? "block" : "none";
}



