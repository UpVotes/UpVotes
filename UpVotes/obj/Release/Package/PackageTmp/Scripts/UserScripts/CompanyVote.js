$(document).ready(function () {
    $.VoteForCompany = function (companyID, companyName) {
        $.ajax({
            url: $.absoluteurl('/Company/VoteForCompany'),
            cache: false,
            async: false,
            datatype: 'json',
            data: { companyID: companyID },
            type: 'POST',
            success: function (response) {
                response = eval(response);
                if (response == '0') {
                    $('#myModal').modal();
                }
                else {
                    alert(response);
                    window.location.reload();
                }
            }
        });
    }

    $('#btnVote').click(function () {
        var companyID = $('#hdnCompanyID')[0].value;
        var companyName = $('#hdnCompanyName')[0].value;
        $.VoteForCompany(companyID, companyName);
    });

});