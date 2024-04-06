


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



