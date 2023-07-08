// Variable to track is the user has clicked
let enable = true

//display the empty star if the user hasn't clicked yet and the mouse goes out
function CRateOut(rating) {
    //if user has not clicked yet, execute function
    if (enable === true) {
        for (var i = 1; i <= rating; i++) {
            $("#i" + i).attr('class', 'bi bi-star');
        }
    }
}

//display the full star if the user hasn't clicked and the mouse goes over
function CRateOver(rating) {
    //if user has not clicked yet, execute function
    if (enable === true) {
        for (var i = 1; i <= rating; i++) {
            $("#i" + i).attr('class', 'bi bi-star-fill');
        }
        enable = true
    }
}

// Set the display of stars as full or empty based on the star the user has clicked
function CRateClick(rating) {
    $("#lblRating").val(rating);
    // Set the selected stars as empty
    for (var i = 1; i <= rating; i++) {
        $("#i" + i).attr('class', 'bi bi-star-fill');
    }

    // Set the remaining stars as empty
    for (var i = rating + 1; i <= 5; i++) {
        $("#i" + i).attr('class', 'bi bi-star');
    }
     // Set the variable to false to disable the mouseOver and mouseOut functions
    enable = false
}

// Verify if a rating has been selected before submitting the form
function VerifyRating() {
    var rating = $("#lblRating").val();
    if (rating == 0) {
        var ratingValidation = document.querySelector(".rating-validation");
        ratingValidation.style.display = "block";
        return false;
    }
}