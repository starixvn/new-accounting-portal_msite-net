"use strict";

$(document).ready(function () {

	$('.icon-view-log').click(function () {

        var appId = $(this).data("appid");
        var appName = $(this).data("appname");
        popUpManager.openPopup(
            '/Mail/GetMailConfigLogs?' +
            $.param({
                appId: appId
            }),
            'Log > ' + appName,
            1000,
            500,
            true,
            false,
            parent.window,
            3000,
            null,
            null,
            ['center', 80]);
	});

});