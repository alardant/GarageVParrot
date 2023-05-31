$('.owl-carousel').owlCarousel({
    loop: true,
    margin: 10,
    nav: true,
    responsive: {
        // breakpoint from 0 up
        0: {
            items: 1
        },
        // breakpoint from 575 up
        575: {
            items: 1
        },
        // breakpoint from 768 up
        768: {
            items: 2
        },
        // breakpoint from 992 up
        992: {
            items: 3
        },
        // breakpoint from 1200 up
        1200: {
            items: 4
        }
    }
});