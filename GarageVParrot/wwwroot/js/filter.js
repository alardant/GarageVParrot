// Hide and show each filter's category'

var downArrow = document.querySelector(".bi-chevron-down");
var upArrow = document.querySelector(".bi-chevron-up")

function hideFilter() {

    var hideBox = document.querySelector(".filter-box-open");
    hideBox.classList.add("filter-box-close");
    hideBox.classList.remove("filter-box-open");
    downArrow.style.display = "block";
    upArrow.style.display = "none";
}

function showFilter() {

    var showBox = document.querySelector(".filter-box-close");
    showBox.classList.add("filter-box-open");
    showBox.classList.remove("filter-box-close");
    downArrow.style.display = "none";
    upArrow.style.display = "block";
}
