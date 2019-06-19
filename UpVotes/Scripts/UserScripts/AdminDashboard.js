$(document).ready(function () {
    $('#showAddNewsSection').click(function () {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/OverViewAndNews/GetNewsForm'),
            type: "POST",
            success: function (response) {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });

    $('#showClaimListingSection').click(function () {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/GetClaimListings'),
            type: "POST",
            success: function (response) {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });

    $('#showUserSponsorShipForm').click(function () {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/Sponsorship/GetSponsorShipForm'),
            type: "POST",
            success: function (response) {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });

    $('#showExpiredSponsorShipForm').click(function () {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/Sponsorship/GetSchedulerForm'),
            type: "POST",
            success: function (response) {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });

});