/* global NAP: false */

"use strict";
/* jshint camelcase: false,
unused: vars */
var sharedScriptManager;
var SharedScriptManager = function () {
    var self = this;
    self.cachedData = {
        pagingOptions: [50, 100, 200, 500],
        defaultItemsPerPage: 50,
        alertSuccessTime: 60000
    };

    self.callAjax = function (currentHandler,
        url,
        requestParams,
        successFunction,
        errorFunction,
        completeFunction) {
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: requestParams ? JSON.stringify(requestParams) : null,
            beforeSend: function () {
            },
            success: function (rs) {
                if (successFunction && typeof (successFunction) === 'function') {
                    successFunction.call(currentHandler, rs);
                }
            },
            error: function (jqXHR, event, ajaxSettings) {
                if (errorFunction && typeof (errorFunction) === 'function') {
                    errorFunction.call(currentHandler);
                }
            },
            complete: function () {
                if (completeFunction && typeof (completeFunction) === 'function') {
                    completeFunction();
                }
            }
        });
    };

    self.blockUI = function () {
        NAP.loadingBox.show();
    };
    self.unblockUI = function () {
        NAP.loadingBox.close();
    };
    self.renderActionResult = function (result, successFunction) {
        var options = {
            closeBtn: result.Code,
            sticky: result.Code
        };

        sharedScriptManager.unblockUI();
        if (result.Code === 0) {
            NAP.headlineMsgBox.showInfo(result.Message, options);
            if (successFunction) {
                successFunction();
            }

            return;
        }

        NAP.headlineMsgBox.showErr(result.Message, options);
    };
    self.isValidEmail = function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    };

    self.createValidationSummary = function (errorList, isSuccess) {
        var validationSum = $('#validationSummary');
        var html = ["<ul>"];
        for (var i = 0; i < errorList.length; i++) {
            html.push("<li>");
            html.push(NAP.htmlDecode(errorList[i]));
            html.push("</li>");
        }

        html.push("</ul>");
        validationSum.html(html.join(''));
        validationSum.removeClass();
        if (isSuccess) {
            validationSum.addClass("validation-summary-valid success");
        } else {
            validationSum.addClass("validation-summary-errors");
        }

        $('#validation-summary-row').removeClass('display-none');
    }

    self.hideValidateSummary = function () {
        $('#validationSummary').empty();
    }

    self.showNonQueryResult = function (result, url, msec, callback) {
        var msg = [];
        msg.push(result.Message);
        self.createValidationSummary(msg, result.IsSuccessful);
        if (result.IsSuccessful) {
            if ($.isFunction(callback)) {
                callback();
            }

            if (url !== null && url !== undefined && url !== '') {
                NAP.showSuccessMsgCountDown(url, msec / 1000);
            }
        }
    }

    self.currentTimeZone = function () {
        var offset = new Date().getTimezoneOffset();
        var timezone = offset / -60;
        return timezone > 0 ? '+' + timezone : timezone;
    }

    self.toLocalDate = function (utcDateString) {
        return utcDateString === null || utcDateString === '' ? '' : new Date(utcDateString + 'Z');
    }
};

sharedScriptManager = new SharedScriptManager();