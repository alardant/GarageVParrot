// reduce/expand the filter section if the screen is smaller/larger than 575 pixels

// Variables to track the screen size threshold and initial loading
var isAboveThreshold = false;
var firstLoading = true;

// Function to handle the visibility of the filter boxes based on screen size
function handleFilterBoxVisibility() {
    var screenWidth = window.innerWidth;
    // Screen is larger than 575px
    if (screenWidth > 575 && (firstLoading || !isAboveThreshold)) {
        isAboveThreshold = true;
        firstLoading = false;
        var filterBoxes = document.querySelectorAll(".filter-box-close");
        filterBoxes.forEach(function (box) {
            toggleFilterBox(box, true);
        });
        // Screen is smaller than or equal to 575px
    } else if (screenWidth <= 575 && (firstLoading || isAboveThreshold)) {
        isAboveThreshold = false;
        firstLoading = false;
        var filterBoxes = document.querySelectorAll(".filter-box-open");
        filterBoxes.forEach(function (box) {
            toggleFilterBox(box, false);
        });
    }
}

// Hide and show each filter's category on button click

// Function to hide a filter box
function hideFilter(filterName) {
    var filterBox = document.querySelector("#" + filterName + "-filter-box");
    toggleFilterBox(filterBox, false);
}

// Function to show a filter box
function showFilter(filterName) {
    var filterBox = document.querySelector("#" + filterName + "-filter-box");
    toggleFilterBox(filterBox, true);
}

// Function to toggle the visibility of a filter box
function toggleFilterBox(box, open) {
    box.classList.toggle("filter-box-close", !open);
    box.classList.toggle("filter-box-open", open);
    var filterUpArrow = box.querySelector(".bi-chevron-up");
    var filterDownArrow = box.querySelector(".bi-chevron-down");
    filterUpArrow.style.display = open ? "block" : "none";
    filterDownArrow.style.display = open ? "none" : "block";
}

// Event listeners to handle filter box visibility on resize and DOM load
window.addEventListener("resize", handleFilterBoxVisibility);
window.addEventListener("DOMContentLoaded", handleFilterBoxVisibility);