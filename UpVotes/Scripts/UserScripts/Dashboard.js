$(document).ready(function () {
    //Side bar submenu toggle
    $(".sub-nav > .nav-link").click(function () {
        $(".sub-nav").toggleClass("open");
    });

    $(".side-bar .nav-link").click(function () {
        $('.side-bar a').removeClass('active');
        $(this).addClass('active');
    });

    $('#ancServiceListing').click(function () {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/UserCompany'),
            data: { companyName: "" },
            type: "POST",
            success: function (response) {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });

    $('#ancSoftwareListing').click(function () {        
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/UserSoftware'),
            data: { softwareName: "" },
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

    $('#showAddUserNewsSection').click(function () {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/OverViewAndNews/GetUserNewsForm'),
            type: "POST",
            success: function (response) {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });

    $('#showAddTeamMembersSection').click(function () {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/GetAllTeamMembers'),
            type: "POST",
            success: function (response) {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });

    $('#showAddUserPortfolioSection').click(function () {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/GetUserPortfolioForm'),
            type: "POST",
            success: function (response) {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });

    $('#showUserReviewSection').click(function () {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/GetUserReviewApproveForm'),
            type: "POST",
            success: function (response) {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });

});