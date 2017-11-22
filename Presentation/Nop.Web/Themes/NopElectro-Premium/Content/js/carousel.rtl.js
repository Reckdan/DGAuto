
$(window).load(function () {
    function handleOwlCarouselControl(element, show) {
        if (show) {
            $(element).find('.owl-controls').show();
        }
        else {
            $(element).find('.owl-controls').hide();
        }
    }

    var owl0 = $(".home-page .owl-carousel");
    owl0.on('initialized.owl.carousel', function (event) {
        handleOwlCarouselControl(event.target, event.page.count > 1);
    });
    owl0.on('resized.owl.carousel', function (event) {
        handleOwlCarouselControl(event.target, event.page.count > 1);
    });
    owl0.owlCarousel({
        loop: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            900: {
                items: 3
            },
            1000: {
                items: 5
            }
        },
        nav: true,
        rtl: true
    });

    var owl1 = $("#homepage-brands");
    owl1.on('initialized.owl.carousel', function (event) {
        handleOwlCarouselControl(event.target, event.page.count > 1);
    });
    owl1.on('resized.owl.carousel', function (event) {
        handleOwlCarouselControl(event.target, event.page.count > 1);
    });
    owl1.owlCarousel({
        loop: true,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 3
            },
            900: {
                items: 4
            },
            1000: {
                items: 6
            },
            1359: {
                items: 7
            }
        },
        nav: true,
        rtl: true
    });

    var owl2 = $(".sub-category-grid .owl-carousel");
    owl2.on('initialized.owl.carousel', function (event) {
        handleOwlCarouselControl(event.target, event.page.count > 1);
    });
    owl2.on('resized.owl.carousel', function (event) {
        handleOwlCarouselControl(event.target, event.page.count > 1);
    });
    owl2.owlCarousel({
        loop: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            900: {
                items: 3
            },
            1000: {
                items: 4
            }
        },
        nav: true,
        rtl: true
    });

    var owl3 = $(".product-details-page .owl-carousel");
    owl3.on('initialized.owl.carousel', function (event) {
        handleOwlCarouselControl(event.target, event.page.count > 1);
    });
    owl3.on('resized.owl.carousel', function (event) {
        handleOwlCarouselControl(event.target, event.page.count > 1);
    });
    owl3.owlCarousel({
        loop: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            900: {
                items: 3
            },
            1000: {
                items: 4
            },
            1359: {
                items: 5
            }
        },
        nav: true,
        rtl: true
    });

    var owl4 = $(".shopping-cart-page .owl-carousel");
    owl4.on('initialized.owl.carousel', function (event) {
        handleOwlCarouselControl(event.target, event.page.count > 1);
    });
    owl4.on('resized.owl.carousel', function (event) {
        handleOwlCarouselControl(event.target, event.page.count > 1);
    });
    owl4.owlCarousel({
        loop: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            900: {
                items: 3
            },
            1000: {
                items: 4
            },
            1359: {
                items: 5
            }
        },
        nav: true,
        rtl: true
    });
});
