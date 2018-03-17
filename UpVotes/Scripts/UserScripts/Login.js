$(document).ready(function () {

    $.LoginWithTwitterAndLinkedIn = function (companyID, calledPage, url) {
        $.ajax({
            type: "POST",   //GET or POST or PUT or DELETE verb
            url: $.absoluteurl(url),//?companyid='+compid,
            data: { companyid: companyID, calledPage: calledPage },// Location of the service
            success: function (json) {
                 //On Successful service call
                window.location.href = json;
            }

        });
    }
});