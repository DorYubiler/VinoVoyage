//---------------signup confirm popup---------------------------------------
function startCustogglePopup(divId) {
    closePopups();
    var popup = document.getElementById((divId));
    popup.style.display = (popup.style.display === "none") ? "table" : "none";
    popup.style.width = 50 % ;
    popup.style.height = 14 % ;
}
