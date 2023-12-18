/* global NAP: false */

"use strict";

var AddMailSettingPopUp = (function ($) {

	return {
		GetListMailSetting: function (mailSettingId) {
			$.ajax({
				url: NAP.resolveClientUrl("Mail/GetMailSetting"),
				type: 'POST',
				dataType: 'JSON',
				cache: false,
				beforeSend: function () {
					NAP.loadingBox.show();
				},
				complete: function () {
					NAP.loadingBox.close();
				},
				success: function (result) {
					$("#SettingId").html("");
					$("#SettingId").append($("#templateMailSetting").render(result));
					$("#SettingId").val(mailSettingId);
				},
				error: function (xhr, status, err) {
					NAP.handleAjaxError(xhr, status, err);
				}
			});
		}
	};

} (jQuery));

$(document).ready(function () {

	$('#btnCancelAddSetting').click(function () {
		NAP.closePopup();
	});

	$('#btnAddMailSetting').click(function () {

		var server = $('#Server').val();
		var tempError;
		if (server === '') {
			tempError = '<div data-valmsg-summary="true" class="validation-summary-errors"><ul><li>The Server field is required.</li></ul></div>';
			$('#messageError').html(tempError);
			return;
		}
		var port = $('#Port').val();
		if (port === '') {
			tempError = '<div data-valmsg-summary="true" class="validation-summary-errors"><ul><li>The Port field is required.</li></ul></div>';
			$('#messageError').html(tempError);
			return;
		}
		var username = $('#Username').val();
		if (username === '') {
			tempError = '<div data-valmsg-summary="true" class="validation-summary-errors"><ul><li>The Username field is required.</li></ul></div>';
			$('#messageError').html(tempError);
			return;
		}
		var password = $('#Password').val();
		var isAutho = $('#IsAuth').attr('checked');
		if (isAutho === 'checked') {
			isAutho = true;
		}
		else {
			isAutho = false;
		}

		$.ajax({
			url: NAP.resolveClientUrl("Mail/AddMailSettingPopUp"),
			data: { server: server, port: port, username: username, password: password, isautho: isAutho },
			type: 'POST',
			dataType: 'JSON',
			cache: false,
			beforeSend: function () {
				NAP.loadingBox.show();
			},
			complete: function () {
				NAP.loadingBox.close();
			},
			success: function (result) {
				if (result != null) {
					AddMailSettingPopUp.GetListMailSetting(result.SettingID);
					NAP.closePopup();
				}
			},
			error: function (xhr, status, err) {
				NAP.handleAjaxError(xhr, status, err);
			}
		});
	});
});