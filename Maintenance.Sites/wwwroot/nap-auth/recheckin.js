function ReCheckin() {
    var urlRecheckIn = window.rootUrl + 'Auth/ReCheckIn';
    var urlLogout = window.rootUrl + 'Auth/LogOut';
    var urlOffline = window.rootUrl + 'Auth/Offline';
    var xmlhttp = new XMLHttpRequest();

    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState === XMLHttpRequest.DONE) {   // XMLHttpRequest.DONE == 4
            var result = JSON.parse(xmlhttp.responseText);

            if (xmlhttp.status === 200) {
                if (result.IsKickedOut) {
                    alert('You have logged in from another session or your login session has expired!');
                    window.location.href = urlLogout;
                }
            }
            else {
                if (result.code === 999999) {
                    window.location.href = urlOffline;
                }
            }

            setTimeout(ReCheckin, 60000);
        }
    };

    xmlhttp.open('POST', urlRecheckIn, true);
    xmlhttp.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
    xmlhttp.send();
}
setTimeout(ReCheckin, 60000);