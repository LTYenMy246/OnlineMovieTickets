$(document).ready(function () {
    $(".owl-carousel").owlCarousel({
        items: 3,  // Hiển thị 3 item mỗi lần
        loop: true,
        margin: 10,
        nav: true,
        autoplay: true,
        autoplayTimeout: 3000,
        autoplayHoverPause: true
    });
});