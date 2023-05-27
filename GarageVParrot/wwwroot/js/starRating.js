function CRateOut(rating) {
    for (var i = 1; i <= rating; i++) {
        $("#i" + i).attr('class', 'bi bi-star');
    }
}

function CRateOver(rating) {
    for (var i = 1; i <= rating; i++) {
        $("#i" + i).attr('class', 'bi bi-star-fill');
    }
}

function CRateClick(rating) {
    $("#lblRating").val(rating);
    for (var i = 1; i <= rating; i++) {
        $("#i" + i).attr('class', 'bi bi-star-fill');
    }

    for (var i = rating + 1; i <= 5; i++) {
        $("#i" + i).attr('class', 'bi bi-star');
    }
}

function CRateSelected() {
    var rating = $("#lblRating").val();
    for (i = 1; i < rating; i++) {
        $("#i" + i).attr('class', 'bi bi-star-fill');
    }
}

function VerifyRating() {
    var rating = $("#lblRating").val();
    if (rating == 0) {
            return false
    }
}