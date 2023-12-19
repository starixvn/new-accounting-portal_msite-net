/* global NAP: false */

"use strict";

$(document).ready(function () {

	$('.btn-edit').click(function () {

		$.ajax({
			url: "/Mail/AddMailSettingPopUp",
			type: 'GET',
			dataType: 'html',
			cache: false,
			beforeSend: function () {
			},
			complete: function () {
			},
			success: function (result) {
				if (result.length > 0) {
					NAP.popup("Create New Mail Setting", result);
				}
			},
			error: function () {
			}
		});
	}); 

});