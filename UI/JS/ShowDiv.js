

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
        if (index > 0) { // Skip the first column (Username)
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
    var role = row.find('td:eq(2) input').val();
    if (role == 'Customer') {
        role = 'customer';
    }
    if (role == 'Admin') {
        role = 'admin';
    }
    if (role != 'customer' && role != 'admin') {
        showmessage("role must be 'customer' or 'admin'. try again.")
        return;
    }
    var email = row.find('td:eq(3) input').val();
    var regex = /^[^\s@]+@[a-zA-Z]+\.(co\.il|com)$/;
    const re = /^(([^<>()\[\]\\.,;:\s@@"]+(\.[^<>()\[\]\\.,;:\s@@"]+)*)|(".+"))@@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!(regex.test(email))) {
        showmessage("The email is not valid. It must be in the format name@domain.co.il or name@domain.com");
        return;
    }
    //if (!re.test(String(email).toLowerCase())) {
    //    showmessage("email written wrong! try again.")
    //    return;
    //}

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
    if (confirm('Are you sure you want to delete this user?')) {
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
}

function deleteRowProduct(button) {
    if (confirm('Are you sure you want to delete this product?')) {
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
}

function editRowProduct(button) {
    var row = $(button).closest('tr');
    // Start looping from the 1st index to skip the ProductID field
    var productID = row.find('td:eq(0)').text();
    var deleteButton = document.getElementById('ProductDeleteButton' + productID); // Assuming you've added a 'deleteButton' class to your delete buttons
    deleteButton.disabled = true;




    row.find('td:not(:last-child)').each(function (index) {
        if (index > 1) { // Skip the first column (ProductID)
            var text = $(this).text();
            $(this).html('<input type="text" value="' + text + '" />');
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
    alert("Your order has been submitted successfully!\n delivery time: 12 business days.");
    window.location.reload();

}




function showmessage(message) {
    alert(message);

}

function UservalidateForm() {
    var errors = [];

    // Validate username
    var username = document.getElementById('username').value;
    if (!/^[a-zA-Z0-9]{3,10}$/.test(username)) {
        errors.push("Username must be 3-10 letters or digits.");
    }

    // Validate password
    var password = document.getElementById('password').value;
    if (!/^[a-zA-Z0-9]{6,10}$/.test(password)) {
        errors.push("Password must be 6-10 letters or digits.");
    }

    // Validate email
    var email = document.getElementById('email').value;
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/.test(email) || email.length < 12 || email.length > 30) {
        errors.push("Email must be a valid format and 12-30 characters long.");
    }

    // Display errors or submit form
    var errorsDiv = document.getElementById('uservalidationErrors');
    errorsDiv.style.display = "block";
    if (errors.length > 0) {
        errorsDiv.innerHTML = '<p class="error">' + errors.join('<br>') + '</p>';
        return false;
    } else {
        errorsDiv.innerHTML = '';
        return true;
    }
    showmessage("the user has been successfully added!");

}

function ProductValidateForm() {
    var errors = [];
    // Validate Product Name
    var productName = document.getElementById('ProductName').value;
    if (productName.length < 4 || productName.length > 30) {
        errors.push("Product name must be between 4 and 30 characters.");
    }


    var winery = document.getElementById('Winery').value;
    if (winery.length < 2 || winery.length > 30) {
        errors.push("Winery name must be between 2 and 30 characters.");
    }


    var description = document.getElementById('Description').value;
    if (description.length < 4 || description.length > 500) {
        errors.push("Product name must be between 4 and 500 characters.");
    }
    // Validate Amount
    var amount = document.getElementById('Amount').value;
    if (amount <= 0 || amount > 200) {
        errors.push("Amount must be greater than 0, max 200.");
    }

    // Validate Price
    var price = document.getElementById('Price').value;
    if (price <= 0) {
        errors.push("Price must be greater than 0.");
    }


    var newprice = document.getElementById('NewPrice').value;
    if (newprice < 0) {
        errors.push("New price must be positive number.");
    }
    // Display errors or allow form submission
    var errorsDiv = document.getElementById('productValidationErrors');
    errorsDiv.style.display = 'block';
    if (errors.length > 0) {
        errorsDiv.innerHTML = '<p class="error">' + errors.join('<br>') + '</p>';
        return false; // Prevent form submission
    } else {
        errorsDiv.innerHTML = '';
        return true; // Allow form submission
    }
}

 
