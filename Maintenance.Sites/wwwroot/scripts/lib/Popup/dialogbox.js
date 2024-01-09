/* jshint strict: false,
camelcase: false,
unused: vars */

var currentExecFrame = null;
var popUpManager = {
    openPopup: function (url,
        title,
        width,
        height,
        modal,
        resizable,
        currWindow,
        duration,
        fnRecall,
        fnOnClose,
        position,
        beforeCloseEvent) {
        $("#dialog-modal").dialog("destroy").remove();
        var dialoghtml = $('<div id="dialog-modal" title="' + title + '" style=" overflow:hidden"></div>');
        if (url !== "") {
            $('<iframe id="innerFrame" allowtransparency="true" style="width: ' + width + 'px;" />')
                .attr({ "frameborder": "0", "scrolling": "auto" })
                .appendTo(dialoghtml);
        }

        dialoghtml.appendTo($("body"));
        $("#dialog-modal").dialog({
            resizable: resizable,
            modal: modal,
            width: width,
            height: height,
            autoOpen: false,
            position: position || ['center', 'center'],
            open: function (event, ui) {
                if ($(this).width()) {
                    $("#innerFrame").css({ "width": $(this).css("width") }).show();
                }

                if ($(this).height()) {
                    $("#innerFrame").css({ "height": $(this).css("height") }).show();
                }

                $("#innerFrame").attr({ "src": url });
            },
            dragStart: function (event, ui) {
                $("#innerFrame").hide();
            },
            dragStop: function (event, ui) {
                if ($(this).width()) {
                    $("#innerFrame").css({ "width": $(this).css("width") }).show();
                }

                if ($(this).height()) {
                    $("#innerFrame").css({ "height": $(this).css("height") }).show();
                }

                $("#innerFrame").show();
            },
            resizeStart: function (event, ui) {
                $("#innerFrame").hide();
            },
            resizeStop: function (event, ui) {
                if ($(this).width()) {
                    $("#innerFrame").css({ "width": $(this).css("width") }).show();
                }

                if ($(this).height()) {
                    $("#innerFrame").css({ "height": $(this).css("height") }).show();
                }

                $("#innerFrame").show();
            },
            beforeClose: function () {
                if (beforeCloseEvent && typeof (beforeCloseEvent) === "function") {
                    return beforeCloseEvent();
                }

                return true;
            },
            close: function (event, ui) {
                $(this).find("#innerFrame").attr("src", "");
                if (currentExecFrame != null && currentExecFrame.onclose) {
                    currentExecFrame.onclose();
                    try {
                        setTimeout(function () {
                            currentExecFrame = null;
                        }, 500);
                    } catch (e) {
                    }
                }

            }
        });

        $("#dialog-modal").dialog("open");
        if (currWindow != null) {
            currentExecFrame = currWindow;
            currentExecFrame.duration = duration;
            if (fnRecall) {
                currentExecFrame.callback = fnRecall;
            }

            if (fnOnClose) {
                currentExecFrame.onclose = fnOnClose;
            }
        }
    },

    closePopup: function () {
        $("#dialog-modal").dialog('close');
    },

    afterUpdate: function (autoClose) {
        try {
            if (currentExecFrame != null) {
                var duration = currentExecFrame.duration;
                if (duration == null) {
                    duration = 0;
                }

                if (currentExecFrame.callback) {
                    try {
                        setTimeout(function () {
                            try { currentExecFrame.callback(); } catch (e) { }
                        }, duration);
                    } catch (e) { }
                }

                setTimeout(function () {
                    currentExecFrame = null;
                }, duration + 1000);
            }
            if (autoClose) {
                setTimeout(function () {
                    popUpManager.closePopup();
                }, 2000);
            }
        } catch (e) {
        }
    }
};
