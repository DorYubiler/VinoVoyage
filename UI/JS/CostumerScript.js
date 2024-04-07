


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



