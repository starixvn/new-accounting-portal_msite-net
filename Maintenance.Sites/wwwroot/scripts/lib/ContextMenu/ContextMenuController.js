
var ContextMenuController = function () {
    var self = this;
    self.bindContextMenuEvents = function () {
        $(document).click(function () {
            self.hideContextMenu();
        });
    };
    self.showContextMenu = function (iconPosition) {
        var actionsGroup = $(".actions");
        actionsGroup.removeClass("display-none");
        actionsGroup.css({
            top: iconPosition.top + "px",
            left: (iconPosition.left - 150) + "px",
            display: "block"
        });
    };

    self.hideContextMenu = function () {
        var actionsGroup = $(".actions");
        actionsGroup.addClass("display-none");
    };
    self.bindEvents = function (events) {
        var actionsGroup = $(".actions");
        actionsGroup.find(".edit-icon").click(events.editEvent);
        actionsGroup.find(".copy-icon").click(events.copyEvent);
        actionsGroup.find(".delete-icon").click(events.deleteEvent);
    };
};

var contextMenuController = new ContextMenuController();
