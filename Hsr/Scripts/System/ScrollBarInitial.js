


function ScrollBar(contentId) {
    $("#" + contentId).mCustomScrollbar({
        axis: "x",
        theme: "3d-thick",
        scrollbarPosition: "inside",
        autoHideScrollbar: false,
        alwaysShowScrollbar: 2,
        mouseWheel: { enable: true },
        contentTouchScroll: true,
        advanced: {
            autoExpandHorizontalScroll: false,
            updateOnContentResize: false,
            updateOnSelectorChange:true
        },
        scrollButtons: { enable: true },
      
    });
}
