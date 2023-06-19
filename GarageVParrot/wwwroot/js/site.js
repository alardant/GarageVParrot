/*var isAboveThreshold = false;

function handleFilterBoxVisibility() {
    var screenWidth = window.innerWidth;

    if (screenWidth >= 576 && !isAboveThreshold) {
        isAboveThreshold = true;
        var filterBoxes = document.querySelectorAll(".filter-box-close");
        filterBoxes.forEach(function (box) {
            box.classList.remove("filter-box-close");
            box.classList.add("filter-box-open");
        });
        console.log("grand");
    } else if (screenWidth < 576 && isAboveThreshold) {
        isAboveThreshold = false;
        var filterBoxes = document.querySelectorAll(".filter-box-open");
        filterBoxes.forEach(function (box) {
            box.classList.remove("filter-box-open");
            box.classList.add("filter-box-close");
        });
        console.log("petit");
    }
}

function checkScreenSize() {
    if (window.innerWidth < 576) {
        isAboveThreshold = false;
        var filterBoxes = document.querySelectorAll(".filter-box-open");
        filterBoxes.forEach(function (box) {
            box.classList.remove("filter-box-open");
            box.classList.add("filter-box-close");
        });
        console.log("petit");
    }
}

window.addEventListener("resize", handleFilterBoxVisibility);
window.addEventListener("DOMContentLoaded", checkScreenSize);

handleFilterBoxVisibility();*/