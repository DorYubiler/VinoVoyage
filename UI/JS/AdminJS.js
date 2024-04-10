

//* Loop through all dropdown buttons to toggle between hiding and showing its dropdown content - This allows the user to have multiple dropdowns without any conflict */
var dropdown = document.getElementsByClassName("dropdown-btn");
var i;

for (i = 0; i < dropdown.length; i++) {
    dropdown[i].addEventListener("click", function () {
        this.classList.toggle("active");
        var dropdownContent = this.nextElementSibling;
        if (dropdownContent.style.display === "block") {
            dropdownContent.style.display = "none";
        } else {
            dropdownContent.style.display = "block";
        }
    });
}



function showSection(sectionId) {
    // Hide all sections
    document.querySelectorAll('.content div').forEach(function (div) {
        div.style.display = 'none';
    });

    // Show the selected section
    document.getElementById(sectionId).style.display = 'block';
}

function editRowUser(button) {
    var row = $(button).closest('tr');
    var username = row.find('td:eq(0)').text();
    var deleteButton = document.getElementById('userDeleteButton' + username); // Assuming you've added a 'deleteButton' class to your delete buttons
    deleteButton.disabled = true; // Hide the delete button
    // Start looping from the 1st index to skip the UserName field
    row.find('td:not(:last-child)').each(function (index) {
        if (index > 0 &&index!=2) { // Skip the first column (Username)
            var text = $(this).text();
            $(this).html('<input type="text" value="' + text + '" />');
        }
    });

    // Change "Edit" button to "Save"
    $(button).text('Save').attr('onclick', 'saveRowUser(this)');

}

function saveRowUser(button) {
    var row = $(button).closest('tr');
    var username = row.find('td:eq(0)').text();
    var deleteButton = document.getElementById('userDeleteButton' + username); // Assuming you've added a 'deleteButton' class to your delete buttons
    deleteButton.disabled = false;// Hide the delete button
    var password = row.find('td:eq(1) input').val();
    if (password.length < 6 || password.length > 10) {
        showmessage("password must be at least 6 letters and not more then 10!");
        return;
    }
    var role = row.find('td:eq(2) input').text();
    var email = row.find('td:eq(3) input').val();
    var regex = /^[^\s@]+@[a-zA-Z]+\.(co\.il|com)$/;
    const re = /^(([^<>()\[\]\\.,;:\s@@"]+(\.[^<>()\[\]\\.,;:\s@@"]+)*)|(".+"))@@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!(regex.test(email))) {
        showmessage("The email is not valid. It must be in the format name@domain.co.il or name@domain.com");
        return;
    }
    

    var userData = {
        UserName: username,
        Password: password,
        Role: role,
        Email: email,
    };
    console.log(userData);

    $.ajax({
        url: 'UpdateUser',
        type: 'POST',
        data: { model: userData },

        success: function (response) {
            if (response.success) {
                // Convert input fields back to text for all but the first column

                row.find('td:not(:last-child)').each(function (index) {
                    if (index > 0) {
                        var input = $(this).find('input');
                        $(this).text(input.val());
                    }
                });

                // Change "Save" button back to "Edit"
                $(button).text('Edit').attr('onclick', 'editRowUser(this)');
            } else {
                console.error("Server error: " + error);
                alert("An error occurred. Please try again or contact support if the problem persists.");

            }
        },

        error: function (xhr, status, error) { }
    });

}

function deleteRowUser(button) {
        var row = button.closest('tr')
        var usernameTd = row.querySelector('td');
        var username = usernameTd.innerHTML;

        $.ajax({
            url: 'DeleteUser',
            type: 'POST',
            data: { usern: username },
            success: function (response) {
                if (response.success) {
                    row.remove();

                } else {
                    alert('Error deleting user.');

                }
            },
            error: function (xhr, status, error) { }
        });
    
}

function deleteRowProduct(button) {
        var row = button.closest('tr');
        var prodidTd = row.querySelector('td');
        var prodid = prodidTd.innerHTML;
        var prodnameTd = row.querySelector('td:nth-child(2)');
        var prodname = prodnameTd.innerHTML;

        $.ajax({
            url: 'DeleteProduct',
            type: 'POST',
            data: {
                prod: prodid,
                prodn: prodname
            },
            success: function (response) {
                if (response.success) {
                    row.remove();
                } else {
                    alert('Error deleting product.');
                }
            },
            error: function (xhr, status, error) { }
        });
    
}

function editRowProduct(button) {
    var row = $(button).closest('tr');
    // Start looping from the 1st index to skip the ProductID field
    var productID = row.find('td:eq(0)').text();
    var deleteButton = document.getElementById('ProductDeleteButton' + productID); // Assuming you've added a 'deleteButton' class to your delete buttons
    deleteButton.disabled = true;

    row.find('td:not(:last-child)').each(function (index) {
        if (index > 1) { 
            var text = $(this).text();
            $(this).html('<input  type="text" class="small-input" value="' + text + '" />');
        }
    });

    // Change "Edit" button to "Save"
    $(button).text('Save').attr('onclick', 'saveRowProduct(this)');


}


function saveRowProduct(button) {
    var row = $(button).closest('tr');
    var productID = row.find('td:eq(0)').text();
    var deleteButton = document.getElementById('ProductDeleteButton' + productID);
    deleteButton.disabled = false;
    var productName = row.find('td:eq(1)').text();

    var winery = row.find('td:eq(2) input').val();
    if (winery.length < 2 || winery.length > 30) {
        showmessage('The winery input is incorrect');
    }
    var type = row.find('td:eq(3) input').val();
    if (type.toLowerCase() != 'white' && type.toLowerCase() != 'red' && type.toLowerCase() != 'rose') {
        showmessage('The type input is incorrect');
        return;
    }

    var description = row.find('td:eq(4) input').val();
    if (description.length < 4 || description.length > 500) {
        showmessage('The description input is incorrect')
    }
    var origin = row.find('td:eq(5) input').val();
    if (origin.toLowerCase() != 'israel' && origin.toLowerCase() != 'france' && origin.toLowerCase() != 'italy' && origin.toLowerCase() != 'spain') {
        showmessage("The origin input is incorrect");
        return;
    }
    var amount = row.find('td:eq(6) input').val();
    if (amount <= 0) {
        showmessage("amount must be more then 0 ");
        return;
    }
    var Price = row.find('td:eq(7) input').val();
    if (Price <= 0) {
        showmessage("The price input is incorrect");
        return;
    }
    var newPrice = row.find('td:eq(8) input').val();
    if (newPrice < 0) {
        showmessage("The new price input is incorrect");
        return;
    }
    var rating = row.find('td:eq(9) input').val();
    if (rating < 1 || rating > 5) {
        showmessage("The rating muse be 1-5")
        return;
    }
    var prodData = {
        ProductID: productID,
        ProductName: productName,
        Winery: winery,
        Type: type,
        Description: description,
        Origin: origin,
        Amount: amount,
        Price: Price,
        NewPrice: newPrice,
        Rating: rating,
    };
    console.log(prodData);

    $.ajax({
        url: 'UpdateProduct',
        type: 'POST',
        data: { pmodel: prodData },

        success: function (response) {
            if (response.success) {
                // Convert input fields back to text for all but the first column

                row.find('td:not(:last-child)').each(function (index) {
                    if (index > 0) {
                        var input = $(this).find('input');
                        $(this).text(input.val());
                    }
                });

                // Change "Save" button back to "Edit"
                $(button).text('Edit').attr('onclick', 'editRowProduct(this)');
            } else {

                alert("An error occurred. Please try again or contact support if the problem persists.");

            }
        },
        error: function (xhr, status, error) { }
    });

}

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('sendorder').addEventListener('click', sendOrder);
});
function sendOrder() {
    var orderForm = document.getElementById('orderform');
    var inputValues = Array.from(orderForm.querySelectorAll('input')).map(input => Number(input.value));
    var oneFilled = false;
    console.log((inputValues));
    inputValues.forEach(input => {
        if (input != 0) {
            oneFilled = true;
        }
    });
    inputValues.forEach(input => {
        if (input < 0) {
            showmessage("Incorrect input.")
            return;
        }
    });
    if (!oneFilled) {
        alert('Please fill in at least one quantity with a number greater than 0.');
        return;
    }
    toggleConfirmPopup('orderproductPopup');
    setTimeout(function () {
        window.location.reload();
    }, 3000);

}




function showmessage(message) {
    alert(message);

}

$(document).ready(function () {
    $('#addUserForm').on('submit.addUserForm', UservalidateForm);
});
function UservalidateForm(event) {
    event.preventDefault();
    event.stopPropagation();
    $('#uservalidationErrors').empty();
    var uname = $('#username').val();
    if (uname.length < 3 || uname.length > 10) {
        $("#uservalidationErrors").html("Invalid username").show();
        return;
    }
    var pass = $('#password').val();
    if (pass.length < 6 || pass.length > 10) {
        $("#uservalidationErrors").html("Invalid password").show();
        return;
    }
    var mail = $('#email').val(); // Fixed selector
    var regex = /^[^\s@]+@[a-zA-Z]+\.(co\.il|com)$/;
    if (!(regex.test(mail))) {
        $("#uservalidationErrors").html("Invalid email").show();
        return;
    }
    var userData = {
        UserName: uname,
        Password: pass,
        Email: mail,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/AddUser",
        data: { user: userData },

        success: function (respones) {
            if (respones.success) {
                toggleConfirmPopup('adduserPopup');

                setTimeout(function () {
                    window.location.reload();
                }, 3000);
                
            }
            else {
                $("#uservalidationErrors").html("Invalid inputs").show();
            }
        },
        error: function () {
            // Handle any errors that occur during the request.
            $("#uservalidationErrors").html("An error occurred. Please try again.").show();
        }
    });
}

    

$(document).ready(function () {
    $('#addProductForm').on('submit.addProductForm', ProductValidateForm);
});
function ProductValidateForm(event) {
    event.preventDefault();
    event.stopPropagation();
    $('#productValidationErrors').empty();
    var productName = $('#productName').val();
    console.log(productName);
    if (productName.length < 4 || productName.length > 30) {
        $("#productValidationErrors").html("Invalid product name").show();
        return;
    }
    var winery = $('#winery').val();
    if (winery.length < 2 || winery.length > 30) {
        $("#productValidationErrors").html("Invalid winery").show();
        return;
    }
    var type = $('#type').val();
    
    var description = $('#description').val(); // Fixed selector
    if (description.length < 4 || description.length > 500) {
        $("#productValidationErrors").html("Invalid description").show();
        return;
    }
    var origin = $('#origin').val();

    var amount = $('#amount').val();
    if (amount < 0 || amount > 300) {
        $("#productValidationErrors").html("Invalid amount").show();
    }

    var price = $('#price').val();
    if (price < 0 ) {
        $("#productValidationErrors").html("Invalid price").show();
    }

    var newPrice = $('#newPrice').val();
    if (newPrice < 0 ) {
        $("#productValidationErrors").html("Invalid new price").show();
    }
    

    var image = $('#productImage').val();

    var formData = new FormData();
formData.append('ProductName', $('#productName').val());
formData.append('Winery', $('#winery').val());
formData.append('Type', $('#type').val());
formData.append('Description', $('#description').val());
formData.append('Origin', $('#origin').val());
formData.append('Amount', $('#amount').val());
formData.append('Price', $('#price').val());
formData.append('NewPrice', $('#newPrice').val());
// Assuming '#productImage' is the file input's ID
formData.append('ProductImage', $('#productImage')[0].files[0]);


    $.ajax({
        type: "POST",
        url: "AddProduct",
        data: formData,
        processData: false,
        contentType:false,
        
        success: function (respones) {
            if (respones.success) {
                toggleConfirmPopup('addproductPopup');
                
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
                
            }
            else {
                $("#productValidationErrors").html("Invalid inputs").show();
                console.log("###########33");
            }
        },
        error: function (xhr, status, error) {
            // Handle any errors that occur during the request.
            $("#productValidationErrors").html("An error occurred. Please try again.").show();
            console.error("Error - Status:", status, "Error:", error);
        }
    });
}

function closeAllPopups() {
    var popups = document.getElementsByClassName("popup");
    for (var i = 0; i < popups.length; i++) {
        popups[i].style.display = "none";
    }
}

function toggleConfirmPopup(divId) {  
    closeAllPopups();
    var popup = document.getElementById(divId);
    popup.style.display = (popup.style.display === "none") ? "block" : "none";
}
function deleteuser(button) {
    closeAllPopups();
    var popup = document.getElementById('deleteuserPopup');
    popup.style.display = (popup.style.display === "none") ? "block" : "none";
}
 
