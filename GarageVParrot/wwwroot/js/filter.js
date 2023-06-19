// Hide and show each filter's category

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

