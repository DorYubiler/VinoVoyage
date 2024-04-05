{
    button.preventDefault();
    var uname = button.username;
    var pass = button.password;

    $.ajax({
        type: "POST",
        url: "Login",
        data = {
            username: uname,
            password: pas
        },
        success: function(response) {
            if (response.success) {
                window.location.href = response.redirectUrl;
            }
            else {
                // If login fails, keep the popup open and show error messages.
                $("#LoginValidationErrors").html("Invalid username or password").show();
            }
        },
        error: function() {
            // Handle any errors that occur during the request.
            $("#LoginValidationErrors").html("An error occurred. Please try again.").show();
        }
    });

}
