/* jshint freeze: false */

"use strict";

var NAP = (function ($) {

	return {
		NAPRoot: null,
		dialogMsg: null,
		dialogPopup: null,
		headlineMsgBox: null,
		loadingBox: null,
		alertCallback: null,
		confirmCallback: null,
		initCoreValues: function () {
			NAP.NAPRoot = $('#NapAdminRoot').val();
			NAP.dialogMsg = $('#dialog-message');
			NAP.dialogPopup = $('#dialog-popup');
		},
		resolveClientUrl: function (relativeUrl) {
			if (NAP.NAPRoot == null) {
				return relativeUrl;
			}
			return NAP.NAPRoot + relativeUrl;
		},
		// Handles common error when making ajax request
		handleAjaxError: function (xhr, status, err) {
			if (xhr.status === 403) { // unauthorize
				console.warn(err);
			} else {
				console.warn(err);
			}
		},
		/* jshint ignore:start */
		getIeVersion: function () {
			var version = -1; // Return value assumes failure.

			if (window.navigator.userAgent.indexOf("MSIE") > 0) {
				var usrAgent = navigator.userAgent;
				var reg = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");

				if (reg.exec(usrAgent) != null) {
					version = parseFloat(RegExp.$1);
				}
			}

			return version;
		},
		/* jshint ignore:end */
		agentIs: function (name) {
			switch (name.toLowerCase()) {
				case 'ipad':
					return navigator.userAgent.match(/iPad/i);
				case 'iphone':
					return navigator.userAgent.match(/iPhone/i);
				case 'ipod':
					return navigator.userAgent.match(/iPod/i);
				case 'apple':
					return navigator.userAgent.match(/iPad/i) ||
						navigator.userAgent.match(/iPhone/i) ||
						navigator.userAgent.match(/iPod/i);
				default: return null;
			}
		},
		alert: function (title, msg, callback) {
			NAP.dialogMsg.html(msg).dialog({
				buttons: {
					OK: function () {
						if (typeof callback === 'function') {
							callback();
						} else {
							$(this).dialog('close');
						}
					}
				},
				width: 'auto',
				modal: true,
				resizable: false,
				closeOnEscape: false
			});

			NAP.dialogMsg.dialog('option', { title: title });
		},
		confirm: function (title, msg, callback) {
			NAP.dialogMsg.html(msg).dialog({
				buttons: {
					OK: function () {
						if (typeof callback === 'function') {
							callback();
						}
					},
					Cancel: function () { $(this).dialog("close"); }
				},
				width: 'auto',
				modal: true,
				resizable: false
			});

			NAP.dialogMsg.dialog('option', { title: title });
		},
		popup: function (title, html) {
			NAP.dialogPopup.html(html).dialog({
				width: 850,
				height: 600,
				modal: true,
				position: ['center', 160]
			});

			NAP.dialogPopup.dialog('option', { title: title });
		},
		modal: function (title, content, callback) {
			NAP.dialogMsg.html(content).dialog({
				buttons: {
					Ok: function () {
						if (typeof callback === 'function') {
							callback();
							$(this).dialog("close");
						}
					},
					Cancel: function () { $(this).dialog("close"); }
				},
				width: 'auto',
				modal: true,
				resizable: false
			});

			NAP.dialogMsg.dialog('option', { title: title });
		},
		closeDialog: function () {
			NAP.dialogMsg.dialog('close');
		},
		closePopup: function () {
			NAP.dialogPopup.dialog('close');
		},
		redirect: function (url, msec) {
			setTimeout('NAP.navigateTo("' + url + '")', msec);
		},
		navigateTo: function (url) {
			window.location.replace(NAP.resolveClientUrl(url));
		},
		htmlEncode: function (value) {
			return $('<div/>').text(value).html();
		},
		htmlDecode: function (value) {
			return $('<div/>').html(value).text();
		},
		showSuccessMsg: function (msg, url, msec) {
			var msgWrp = $('.validation-summary-valid');

			msgWrp.addClass('success').find('li:first').html(NAP.htmlDecode(msg)).show();

			NAP.showSuccessMsgCountDown(url, msec / 1000);
		},
		showSuccessMsgCountDown: function (url, msec) {
			if (msec-- > 0) {
				setTimeout('NAP.showSuccessMsgCountDown("' + url + '", ' + msec + ')', 1000);
				$('.validation-summary-valid .msec').html(msec);
			} else {
				window.location.replace(NAP.resolveClientUrl(url));
			}
		}
	};

} (jQuery));

$(document).ready(function () {
	NAP.initCoreValues();

	NAP.headlineMsgBox = $.msgbox({
		message: '',
		//containerId: 'msg-bar',
		type: 'err',
		modal: false,
		position: ['top:130', 'center'],
		theme: '',
		zIndex: 9999,
		useAnimation: false
	});

	NAP.loadingBox = $.msgbox({
		message: 'Loading...',
		type: 'warn',
		modal: true,
		position: ['top:0', 'center'],
		closeBtn: false,
		theme: '',
		zIndex: 9999,
		sticky: true,
		useAnimation: false
	});
});

// -- Utilities -- //

// String Format
String.prototype.format = function () {
	var args = arguments;
	return this.replace(/{(\d+)}/g, function (match, number) {
		return typeof args[number] !== 'undefined' ? args[number] : match;
	});
};

// Array Remove - By John Resig (MIT Licensed)
Array.prototype.remove = function (from, to) {
	var rest = this.slice((to || from) + 1 || this.length);
	this.length = from < 0 ? this.length + from : from;
	return this.push.apply(this, rest);
};
