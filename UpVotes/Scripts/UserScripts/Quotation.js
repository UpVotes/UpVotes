var quotationObj = {};
var featuresObj = [];
$(document).ready(function (event) {

    $('#btnplatform').click(function (e) {

        if ($('#ulplatform .active').length > 0) {
            quotationObj.platform = $('#ulplatform .active').text();
            $('#spnAndroid').css('display', 'none');
        }
        else {
            $('#spnAndroid').css('display', 'block');
            e.stopPropagation();
        }
    });

    $('#btnTheme').click(function (e) {

        if ($('#ulTheme .active').length > 0) {
            quotationObj.Theme = $('#ulTheme .active').text();
            $('#spnTheme').css('display', 'none');
        }
        else {
            $('#spnTheme').css('display', 'block');
            e.stopPropagation();
        }
    });
    $('#btnLoginSecurity').click(function (e) {

        if ($('#ulLoginSecurity .active').length > 0) {
            quotationObj.LoginSecurity = $('#ulLoginSecurity .active').text();
            $('#spnLoginSecurity').css('display', 'none');
        }
        else {
            $('#spnLoginSecurity').css('display', 'block');
            e.stopPropagation();
        }
    });
    $('#btnProfile').click(function (e) {
        if ($('#ulProfile .active').length > 0) {
            quotationObj.Profile = $('#ulProfile .active').text();
            $('#spnProfile').css('display', 'none');
        }
        else {
            $('#spnProfile').css('display', 'block');
            e.stopPropagation();
        }
    });
    $('#btnSecurity').click(function (e) {
        if ($('#ulSecurity .active').length > 0) {
            quotationObj.Security = $('#ulSecurity .active').text();
            $('#spnSecurity').css('display', 'none');
        }
        else {
            $('#spnSecurity').css('display', 'block');
            e.stopPropagation();
        }
    });
    $('#btnReviewRate').click(function (e) {
        if ($('#ulReviewRate .active').length > 0) {
            quotationObj.ReviewRate = $('#ulReviewRate .active').text();
            $('#spnReviewRate').css('display', 'none');
        }
        else {
            $('#spnReviewRate').css('display', 'block');
            e.stopPropagation();
        }
    });
    $('#btnService').click(function (e) {
        if ($('#ulService .active').length > 0) {
            quotationObj.Service = $('#ulService .active').text();
            $('#spnService').css('display', 'none');
        }
        else {
            $('#spnService').css('display', 'block');
            e.stopPropagation();
        }
    });
    $('#btnDatabase').click(function (e) {

        if ($('#ulDatabase .active').length > 0) {
            quotationObj.Database = $('#ulDatabase .active').text();
            $('#spnDatabase').css('display', 'none');
        }
        else {
            $('#spnDatabase').css('display', 'block');
            e.stopPropagation();
        }
    });
    $('#btnFeatures').click(function (e) {

        if ($('#ulFeatures .active').length > 0) {
            var featurestr = '';
            $('#ulFeatures .active').each(function (key, val) {
                featurestr = featurestr + $(this).text() + '|';
            });
            quotationObj.Features = featurestr;
            $('#spnFeatures').css('display', 'none');
        }
        else {
            $('#spnFeatures').css('display', 'block');
            e.stopPropagation();
        }
    });

    $(".os-types li").click(function () {
        $(this).closest("li").addClass("active").siblings().removeClass("active");
    });
    $(".os-typesFeatures li").click(function () {
        if ($(this).closest("li").hasClass('active')) {
            $(this).closest("li").removeClass("active");
        }
        else {
            $(this).closest("li").addClass("active")
        }
    });

    $('#btnReport').click(function (e) {        
        var emailid = $('#txtEmail').val();
        var name = $('#txtName').val();
        if (name.trim() == "") {
            $('#txtName').css('border', '1px solid red');
            return;
        }
        else {
            $('#txtName').css('border', '');
            quotationObj.Name = name.trim();
        }


        if (emailid != "") {
            email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;
            
            if (!email_regex.test(emailid)) {
                $('#txtEmail').css('border', '1px solid red');
                return;
            }
            else {
                $('#txtEmail').css('border', '');
                quotationObj.EmailId = emailid;
            }
        }
        else {
            $('#txtEmail').css('border', '1px solid red');
            return;
        }
        $('#imgloader').css('display', 'block');
        quotationObj.CompanyName = $('#txtCompanyName').val().trim();

        $.ajax({
            url: $.absoluteurl('/Quotation/GetQuote'),
            data: { platform: quotationObj.platform, Theme: quotationObj.Theme, LoginSecurity: quotationObj.LoginSecurity, Profile: quotationObj.Profile, Security: quotationObj.Security, ReviewRate: quotationObj.ReviewRate, Service: quotationObj.Service, Database: quotationObj.Database, featuresstring: quotationObj.Features, EmailId: quotationObj.EmailId, Name: quotationObj.Name, CompanyName: quotationObj.CompanyName },
            type: "POST",
            success: function (response) {
                $('#imgloader').css('display', 'none');
                $('#CustomQuote').html("");
                $('#CustomQuote').html(response);
            }
        });
    });

    $('#btnShowBreakdown').click(function (e) {
        if ($('#divbreakdown').css('display') == 'none') {
            $('.slideDown').slideToggle();
        }
    });
    $('#btnEstimateZ1').click(function (e) {
        $('.clsZone1').css('display', 'block');
        $('.clsZone2').css('display', 'none')
        $('.clsZone3').css('display', 'none')
    });
    $('#btnEstimateZ2').click(function (e) {
        $('.clsZone1').css('display', 'none');
        $('.clsZone2').css('display', 'block')
        $('.clsZone3').css('display', 'none')
    });
    $('#btnEstimateZ3').click(function (e) {
        $('.clsZone1').css('display', 'none');
        $('.clsZone2').css('display', 'none')
        $('.clsZone3').css('display', 'block')
    });
    $('#liClose').click(function (e) {
        
        $.ajax({
            url: $.absoluteurl('/Quotation/GetQuotation'),
            type: "POST",
            success: function (response) {                
                $('#CustomQuote').html("");
                $('#CustomQuote').html(response);
            }
        });
    });
});