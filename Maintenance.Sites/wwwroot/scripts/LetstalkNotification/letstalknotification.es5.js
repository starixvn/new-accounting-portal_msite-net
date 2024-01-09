'use strict';

(function () {
    'use strict';

    var _tables = {
        env: null,
        notify: null,
        message: null,
        btnSubmit: null
    };

    function initPage() {
        cachedDOMElements();
        bindingEvent();
    }

    function cachedDOMElements() {
        _tables.env = $('#filter-select .env:checked').val();
        _tables.btnSubmit = $("#btnSubmit");
        _tables.message = $("#message");
        _tables.notify = $("#notify");
    }

    function bindingEvent() {
        _tables.btnSubmit.unbind("click").click(function () {
            updateData();
        });
    }

    function updateData() {
        var isPro = $("input[name=env]:checked").val();
        $.ajax({
            type: 'POST',
            url: window.rootUrl + 'Testing/LetstalkNotification_Query',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            async: false,
            data: JSON.stringify({
                Message: _tables.message[0].value,
                IsPro: isPro === "production" ? true : false
            }),
            success: function success(result) {
                generateEmail(result);
                generateMessage(result);
            },
            error: function error(xhr) {}
        });
    }

    function generateEmail(result) {

        if (result.EmailSuccess) {

            document.getElementById("notify-email").className = "success";
            document.getElementById("notify-email").innerHTML = result.Email;
        } else {

            document.getElementById("notify-email").className = "unsuccess";
            document.getElementById("notify-email").innerHTML = result.Email;
        }
    }

    function generateMessage(result) {

        if (result.MessageSuccess) {
            document.getElementById("notify-msg").className = "success";
            document.getElementById("notify-msg").innerHTML = result.Message;
        } else {
            document.getElementById("notify-msg").className = "unsuccess";
            document.getElementById("notify-msg").innerHTML = result.Message;
        }
    }

    initPage();
})();

