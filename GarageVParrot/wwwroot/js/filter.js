// reduce/expand the filter section if the screen if smaller/larger than 575 pixel

//used to determined if the screen has crossed the line of 575pixel to call the fonction only if so.
var isAboveThreshold = false;
var firstLoading = true

function handleFilterBoxVisibility() {
    var screenWidth = window.innerWidth;

    if (screenWidth > 575 && (firstLoading || !isAboveThreshold)) {
        // screen is larger than 575 px
        isAboveThreshold = true;
        firstLoading = false;
        var filterBoxes = document.querySelectorAll(".filter-box-close");
        filterBoxes.forEach(function (box) {
            box.classList.replace("filter-box-close", "filter-box-open");
            var filterUpArrow = document.querySelector(".bi-chevron-up");
            var filterDownArrow = document.querySelector(".bi-chevron-down");
            filterUpArrow.style.display = "none";
            filterDownArrow.style.display = "none";
        });

    } else if (screenWidth <= 575 && (firstLoading || isAboveThreshold)) {
        // screen is smaller than 575 px
        isAboveThreshold = false;
        firstLoading = false;
        var filterBoxes = document.querySelectorAll(".filter-box-open");
        filterBoxes.forEach(function (box) {
            box.classList.replace("filter-box-open", "filter-box-close");
            var filterDownArrow = document.querySelector(".bi-chevron-down");
            filterDownArrow.style.display = "block";
        });
    }
}

window.addEventListener("resize", handleFilterBoxVisibility);
window.addEventListener("DOMContentLoaded", handleFilterBoxVisibility);// reduce/expand the filter section if the screen if smaller/larger than 575 pixel


// Hide and show each filter's category on button click

function hideFilter(filterName) {
    var hideBox = document.querySelector("#" + filterName + "-filter-box");
    hideBox.classList.add("filter-box-close");
    hideBox.classList.remove("filter-box-open");


    var downArrow = document.querySelector("." + filterName + "-filter").querySelector(".bi-chevron-down");
    var upArrow = document.querySelector("." + filterName + "-filter").querySelector(".bi-chevron-up");
    downArrow.style.display = "block";
    upArrow.style.display = "none";
}

function showFilter(filterName) {
    var showBox = document.querySelector("#" + filterName + "-filter-box");
    showBox.classList.add("filter-box-open");
    showBox.classList.remove("filter-box-close");

    var downArrow = document.querySelector("." + filterName + "-filter").querySelector(".bi-chevron-down");
    var upArrow = document.querySelector("." + filterName + "-filter").querySelector(".bi-chevron-up");
    downArrow.style.display = "none";
    upArrow.style.display = "block";
}

