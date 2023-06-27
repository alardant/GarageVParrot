// Variables to track the screen size threshold and initial loading
var isAboveThreshold = false;
var firstLoading = true;

// Function to handle the visibility of the main filter boxe based on screen size
function handleMainFilterBoxVisibility() {
    var screenWidth = window.innerWidth;
    // Screen is larger than 767px
    if (screenWidth > 767 && (firstLoading || !isAboveThreshold)) {
            var mainFilterBox = document.querySelector("#filter-filter-box");
            mainFilterBox.classList.add("filter-box-open");
            mainFilterBox.classList.remove("filter-box-close");
        // Screen is smaller than or equal to 767px
    } else if (screenWidth <= 767 && (firstLoading || isAboveThreshold)) {
            var mainFilterBox = document.querySelector("#filter-filter-box");
            mainFilterBox.classList.add("filter-box-close");
            mainFilterBox.classList.remove("filter-box-open");
        };
    }

// Function to handle the visibility of the filter boxes based on screen size
function handleFilterBoxVisibility() {
    var screenWidth = window.innerWidth;
    // Screen is larger than 767px
  if (screenWidth > 767 && (firstLoading || !isAboveThreshold)) {
        handleMainFilterBoxVisibility()
        isAboveThreshold = true;
        firstLoading = false;
        var filterBoxes = document.querySelectorAll(".box-filter");
        filterBoxes.forEach(function (filterBox) {
            var filterBoxesId = filterBox.id;
                showFilter(filterBoxesId);
        });
        // Screen is smaller than or equal to 767px
    } else if (screenWidth <= 767 && (firstLoading || isAboveThreshold)) {
        handleMainFilterBoxVisibility()
        isAboveThreshold = false;
        firstLoading = false;
        var filterBoxes = document.querySelectorAll(".box-filter");
        filterBoxes.forEach(function (filterBox) {
            var filterBoxesId = filterBox.id;
            hideFilter(filterBoxesId);
        });
    }
}

//function to show the filters
function showFilter(filterName) {
    var filterBox = document.querySelector("#" + filterName + "-filter-box");
    //remove the class that display the filter box
    filterBox.classList.add('filter-box-open');
    filterBox.classList.remove('filter-box-close');
    var filterNameBox = document.querySelector(".filter-" + filterName)
    //apply style to hide/show the arrows
    var filterDownArrow = filterNameBox.querySelector(".bi-chevron-down");
    filterDownArrow.style.display = "none"
    var filterUpArrow = filterNameBox.querySelector(".bi-chevron-up");
    filterUpArrow.style.display = "block";
}


//function to hide the filters
function hideFilter(filterName) {
    var filterBox = document.querySelector("#" + filterName + "-filter-box");
    //add the class that display the filter box
    filterBox.classList.add('filter-box-close');
    filterBox.classList.remove('filter-box-open')
    var filterNameBox = document.querySelector(".filter-" + filterName)
    //apply style to hide/show the arrows
    var filterDownArrow = filterNameBox.querySelector(".bi-chevron-down");
    filterDownArrow.style.display = "block"
    var filterUpArrow = filterNameBox.querySelector(".bi-chevron-up");
    filterUpArrow.style.display = "none";
}



window.addEventListener("resize", handleFilterBoxVisibility);
window.addEventListener("DOMContentLoaded", handleFilterBoxVisibility);


$('input[type="text"]').keyup(function () {
    var minYearInput = document.getElementById("minYear");
    var maxYearInput = document.getElementById("maxYear");
    var yearError = document.getElementById("year-error");
    var minPriceInput = document.getElementById("minPrice");
    var maxPriceInput = document.getElementById("maxPrice");
    var priceError = document.getElementById("price-error");
    var minKmInput = document.getElementById("minKm");
    var maxKmInput = document.getElementById("maxKm");
    var kmError = document.getElementById("km-error");
    var regex = /^[0-9]+$/;


    if (minYearInput.value && !minYearInput.value.match(regex) || maxYearInput.value && !maxYearInput.value.match(regex)) {
        yearError.style.display = "block";
        // prevent the form to be sent
        return false;
    } else {
        yearError.style.display = "none";
    }

    if (minPriceInput.value && !minPriceInput.value.match(regex) || maxPriceInput.value && !maxPriceInput.value.match(regex)) {
        priceError.style.display = "block";
        return false;
    } else {
        priceError.style.display = "none";
    }

    if (minKmInput.value && !minKmInput.value.match(regex) || maxKmInput.value && !maxKmInput.value.match(regex)) {
        kmError.style.display = "block";
        return false;
    } else {
        kmError.style.display = "none";
    }

    return true
})