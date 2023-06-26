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
