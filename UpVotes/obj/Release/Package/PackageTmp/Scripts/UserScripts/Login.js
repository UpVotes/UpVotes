$(document).ready(function () {

    $.LoginWithLinkedIn = function (companyID, calledPage) {
        $.ajax({
            type: "POST",   //GET or POST or PUT or DELETE verb
            url: '/Login/LinkedINcall',//?companyid='+compid,
            data: { companyid: companyID, calledPage: calledPage },// Location of the service
            success: function (json) {
                 //On Successful service call
                window.location.href = json;
            }

        });
    }
});