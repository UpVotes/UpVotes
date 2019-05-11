﻿$(document).ready(function () {
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

});