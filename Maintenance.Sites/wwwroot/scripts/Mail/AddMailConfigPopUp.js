/* global NAP: false */

"use strict";

var AddMailConfigPopUp = (function ($) {

	return {
		GetListMailConfig: function (mailConfigId) {
			$.ajax({
				url: NAP.resolveClientUrl("Mail/GetMailConfig"),
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
					$("#ConfigId").html("");
					$("#ConfigId").append($("#templateMailConfig").render(result));
					$("#ConfigId").val(mailConfigId);
				},
				error: function (xhr, status, err) {
					NAP.handleAjaxError(xhr, status, err);
				}
			});
		}
	};

} (jQuery));

$(document).ready(function () {

	$('#btnCancelConfigPopUp').click(function () {
		NAP.closePopup();
	});

	$('#btnAddConfigPopUp').click(function () {

		var appName = $('#AppName').val();
		if (appName === '') {
			var tempError = '<div data-valmsg-summary="true" class="validation-summary-errors"><ul><li>The Application Name field is required.</li></ul></div>';
			$('#messageError').html(tempError);
			return;
		}

		var desription = $('#Description').val();
		var settingId = $('#SettingId').val();

		$.ajax({
			url: NAP.resolveClientUrl("Mail/AddMailConfigPopUp"),
			data: { appName: appName, description: desription, settingId: settingId },
			type: 'POST',
			dataType: 'html',
			cache: false,
			beforeSend: function () {
				NAP.loadingBox.show();
			},
			complete: function () {
				NAP.loadingBox.close();
			},
			success: function (result) {
				if (result != null) {
					NAP.closePopup();
					AddMailConfigPopUp.GetListMailConfig(result.AppID);
				}
			},
			error: function (xhr, status, err) {
				NAP.handleAjaxError(xhr, status, err);
			}
		});
	});

});