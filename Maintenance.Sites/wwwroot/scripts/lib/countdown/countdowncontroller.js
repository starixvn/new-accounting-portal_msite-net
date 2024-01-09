
var CountdownController = function () {
    var self = this;
    self.cachedData = {
        refreshIntervalEvent: 0,
        updateCountdownEvent: 0,
        refreshIntervalCounter: 0,
        countdownTime: 0,
        refreshEvent: null
    };
    self.viewResources = {
        Not_Refresh: 'Not Refresh'
    };

    self.initPage = function () {
        var refreshDropdownIcon = $(".refreshBox .refreshDropdownIcon");
        refreshDropdownIcon.unbind("click");
        refreshDropdownIcon.click(function () {
            self.showCountDownOptions($(this));
        });

        var countDownOptions = $(".countDownOptionContainer .countDownOption");
        countDownOptions.unbind("click");
        countDownOptions.click(function () {
            self.saveCountDownRefreshOption(this);
        });

        var refreshIcon = $(".refreshBox .refreshIcon");
        refreshIcon.unbind("click");
        refreshIcon.click(function () {
            if (self.cachedData.refreshIntervalCounter > 1) {
                var refreshIcon = $(".refreshBox .refreshIcon");
                self.cachedData.countdownTime = self.cachedData.refreshIntervalCounter / 1000;
                window.clearInterval(self.cachedData.refreshIntervalEvent);
                self.setRefreshIntervalEvent();
                refreshIcon.html(self.cachedData.countdownTime);
                if (self.cachedData.refreshEvent) {
                    self.cachedData.refreshEvent();
                }
            }
        });
    };
    self.showCountDownOptions = function (btnDown) {
        var pnlDown = $('.countDownOptionContainer');
        var arrBtnPos = self.findPosition(btnDown.parent());
        if (pnlDown.css("display") === 'none') {
            pnlDown.css("left", arrBtnPos[0] + 'px');
            pnlDown.css("top", (arrBtnPos[1] + 20) + 'px');
            pnlDown.css("display", 'inline-block');
        }
        else {
            pnlDown.css("display", 'none');
        }
    };
    self.saveCountDownRefreshOption = function (btnCountDownOption) {
        btnCountDownOption = $(btnCountDownOption);
        $(".countDownOption.countDownOptionCheck").removeClass("countDownOptionCheck");
        btnCountDownOption.addClass("countDownOptionCheck");
        setTimeout(function () {
            $('.countDownOptionContainer').css("display", 'none');
            var refreshIcon = $(".refreshBox .refreshIcon");
            var coundownAfter = btnCountDownOption.attr("value");
            if (coundownAfter === self.viewResources.Not_Refresh) {
                refreshIcon.css("text-align", 'left');
                refreshIcon.css("padding-left", '8px');
                refreshIcon.css("width", '85px');
                refreshIcon.html(coundownAfter);

                self.cachedData.refreshIntervalCounter = 0;
                if (self.cachedData.refreshIntervalEvent > 0) {
                    window.clearInterval(self.cachedData.refreshIntervalEvent);
                    window.clearInterval(self.cachedData.updateCountdownEvent);
                }
            }
            else {
                refreshIcon.css("text-align", 'center');
                refreshIcon.css("padding-left", '0');
                refreshIcon.css("width", '93px');
                refreshIcon.html(coundownAfter);

                if (self.cachedData.refreshIntervalEvent > 0) {
                    window.clearInterval(self.cachedData.refreshIntervalEvent);
                    window.clearInterval(self.cachedData.updateCountdownEvent);
                }

                self.cachedData.refreshIntervalCounter = parseInt(coundownAfter * 1000);
                self.cachedData.countdownTime = parseInt(coundownAfter);
                self.cachedData.updateCountdownEvent = setInterval(function () {
                    if (self.cachedData.countdownTime > 1) {
                        self.cachedData.countdownTime--;
                    } else {
                        self.cachedData.countdownTime = self.cachedData.refreshIntervalCounter / 1000;
                    }

                    refreshIcon.html(self.cachedData.countdownTime);
                }, 1000);

                self.setRefreshIntervalEvent();
            }
        }, 200);
    };

    self.setRefreshIntervalEvent = function () {
        self.cachedData.refreshIntervalEvent = setInterval(function () {
            self.cachedData.countdownTime = self.cachedData.refreshIntervalCounter / 1000;
            if (self.cachedData.refreshEvent) {
                self.cachedData.refreshEvent();
            }
        }, self.cachedData.refreshIntervalCounter);
    };
    self.findPosition = function (oElement) {
        if (typeof (oElement.offsetParent()) !== 'undefined') {
            for (var posX = 0, posY = 0; oElement && oElement.length; oElement = oElement.offsetParent()) {
                posX += oElement.offset().left;
                posY += oElement.offset().top;
                if (oElement[0].nodeName.toLowerCase() === 'body') {
                    break;
                }
            }

            return [posX, posY];
        } else {
            return [oElement.x, oElement.y];
        }
    };
    self.setRefreshEvent = function (refreshEvent) {
        self.cachedData.refreshEvent = refreshEvent;
    };
};

var countdownController = new CountdownController();