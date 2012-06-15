var slider;

jQuery(document).ready(function ($) {
        slider = $('#lightbox-slider').advancedSlider({ width: 640,
        height: 437,
        skin: 'glossy-curved-rounded-orange',
        effectType: 'swipe',
        lightbox: false,
        thumbnailLightboxIcon: true,
        thumbnailLightboxIconToggle: true,
        slideButtons: true,
        thumbnailType: 'scroller',
        maximumVisibleThumbnails: 5,
        thumbnailButtons: true,
        thumbnailOrientation: 'vertical',
        captionToggle: true,
        captionSize: 35,
        captionHideEffect: 'slide',
        transitionComplete: onTransitionComplete
    });
});


function previous() {
    slider.previousSlide();
}

function next() {
    slider.nextSlide();
}

function onTransitionComplete(obj) {
    //console.log(obj.type, obj.index, obj.data);
}
	