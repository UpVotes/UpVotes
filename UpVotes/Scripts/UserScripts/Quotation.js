var quotationObj = {};
var featuresObj = [];
$(document).ready(function (event) {

    //Make steps with fixed screen
    if ($(window).width() <= 1000) {
        $("#triggerFixed").click(function () {
            $('.custom-quote').addClass("fixed");
            $('body').css("overflow", "hidden");
        });
    }

    $("#triggerFixed").click(function () {
        $(this).parents(".step").addClass("hide").next('.step').removeClass("hide");
    });

    $("#btnShowReport").click(function () {
        $(this).parents(".step").addClass("hide").next('.step').removeClass("hide");
    });

    $('.btnPrevious').click(function () {
        $(this).parents(".step").addClass("hide").prev('.step').removeClass("hide");
    });

    $('#btnplatform').click(function (e) {

        if ($('#ulplatform .selected').length > 0) {
            quotationObj.platform = $('#ulplatform .selected').text();
            $('#spnAndroid').css('display', 'none');
            $(this).parents(".step").addClass("hide").next('.step').removeClass("hide");
        }
        else {
            $('#spnAndroid').css('display', 'block');
        }
    });

    $('#btnTheme').click(function (e) {

        if ($('#ulTheme .selected').length > 0) {
            quotationObj.Theme = $('#ulTheme .selected').text();
            $('#spnTheme').css('display', 'none');
            $(this).parents(".step").addClass("hide").next('.step').removeClass("hide");
        }
        else {
            $('#spnTheme').css('display', 'block');
        }
    });
    $('#btnLoginSecurity').click(function (e) {

        if ($('#ulLoginSecurity .selected').length > 0) {
            quotationObj.LoginSecurity = $('#ulLoginSecurity .selected').text();
            $('#spnLoginSecurity').css('display', 'none');
            $(this).parents(".step").addClass("hide").next('.step').removeClass("hide");
        }
        else {
            $('#spnLoginSecurity').css('display', 'block');            
        }
    });
    $('#btnProfile').click(function (e) {
        if ($('#ulProfile .selected').length > 0) {
            quotationObj.Profile = $('#ulProfile .selected').text();
            $('#spnProfile').css('display', 'none');
            $(this).parents(".step").addClass("hide").next('.step').removeClass("hide");
        }
        else {
            $('#spnProfile').css('display', 'block');            
        }
    });
    $('#btnSecurity').click(function (e) {
        if ($('#ulSecurity .selected').length > 0) {
            quotationObj.Security = $('#ulSecurity .selected').text();
            $('#spnSecurity').css('display', 'none');
            $(this).parents(".step").addClass("hide").next('.step').removeClass("hide");
        }
        else {
            $('#spnSecurity').css('display', 'block');            
        }
    });
    $('#btnReviewRate').click(function (e) {
        if ($('#ulReviewRate .selected').length > 0) {
            quotationObj.ReviewRate = $('#ulReviewRate .selected').text();
            $('#spnReviewRate').css('display', 'none');
            $(this).parents(".step").addClass("hide").next('.step').removeClass("hide");
        }
        else {
            $('#spnReviewRate').css('display', 'block');            
        }
    });
    $('#btnService').click(function (e) {
        if ($('#ulService .selected').length > 0) {
            quotationObj.Service = $('#ulService .selected').text();
            $('#spnService').css('display', 'none');
            $(this).parents(".step").addClass("hide").next('.step').removeClass("hide");
        }
        else {
            $('#spnService').css('display', 'block');
        }
    });
    $('#btnDatabase').click(function (e) {

        if ($('#ulDatabase .selected').length > 0) {
            quotationObj.Database = $('#ulDatabase .selected').text();
            $('#spnDatabase').css('display', 'none');
            $(this).parents(".step").addClass("hide").next('.step').removeClass("hide");
        }
        else {
            $('#spnDatabase').css('display', 'block');            
        }
    });
    $('#btnFeatures').click(function (e) {

        if ($('#ulFeatures .selected').length > 0) {
            var featurestr = '';
            $('#ulFeatures .selected').each(function (key, val) {
                featurestr = featurestr + $(this).text() + '|';
            });
            quotationObj.Features = featurestr;
            $('#spnFeatures').css('display', 'none');
            $(this).parents(".step").addClass("hide").next('.step').removeClass("hide");
        }
        else {
            $('#spnFeatures').css('display', 'block');
        }
    });

    //$(".os-types li").click(function () {
    //    $(this).closest("li").addClass("active").siblings().removeClass("active");
    //});
    //$(".os-typesFeatures li").click(function () {
    //    if ($(this).closest("li").hasClass('active')) {
    //        $(this).closest("li").removeClass("active");
    //    }
    //    else {
    //        $(this).closest("li").addClass("active")
    //    }
    //});
    //Back to customer quote remove fixed state
    $(".back-nav").click(function () {
        $('.custom-quote').removeClass("fixed");
        $('.step').addClass("hide");
        $('.step1').removeClass("hide");
        $('body').css("overflow", "auto");
    });
    //Custom quote select option
    $(".custom-quote .option.single li").click(function () {
        $(this).addClass("selected").siblings().removeClass("selected");
    });
    $(".custom-quote .option.multi li").click(function () {
        if ($(this).closest("li").hasClass('selected')) {
            $(this).closest("li").removeClass("selected");
        }
        else {
            $(this).closest("li").addClass("selected")
        }
    });

    $('#btnReport').click(function (e) {
        var publicVar = $(this).attr('public');
        var url = '/Quotation/GetQuote';
        if (publicVar == "public") {
            url = '/Blogs/GetQuote';
        }
        
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
            url: $.absoluteurl(url),
            data: { platform: quotationObj.platform, Theme: quotationObj.Theme, LoginSecurity: quotationObj.LoginSecurity, Profile: quotationObj.Profile, Security: quotationObj.Security, ReviewRate: quotationObj.ReviewRate, Service: quotationObj.Service, Database: quotationObj.Database, featuresstring: quotationObj.Features, EmailId: quotationObj.EmailId, Name: quotationObj.Name, CompanyName: quotationObj.CompanyName },
            type: "POST",
            success: function (response) {
                if (publicVar == "tab") {
                    $('#imgloader').css('display', 'none');
                    $(this).parents(".step").addClass("hide").next('.step').removeClass("hide");
                    $('#CustomQuote').html("");
                    $('#CustomQuote').html(response);
                }
                else {
                    $('#imgloader').css('display', 'none');
                    $('#CustomQuote').html("");
                    $('#CustomQuote').html(response);
                }
            }
        });
    });

    $('#btnShowBreakdown').click(function (e) {
        if ($('#divbreakdown').css('display') == 'none') {
            $('.slideDown').slideToggle();
        }
    });

    $('#btnEstimateZ1').click(function (e) {
        $('.clsZone1').css('display', 'inline-block');
        $('.clsZone2').css('display', 'none');
        $('.clsZone3').css('display', 'none');
        $('.clsTotalZone1').css('display', 'block');
        $('.clsTotalZone2').css('display', 'none');
        $('.clsTotalZone3').css('display', 'none');
        $('.clsFeatureZone1').css('display', 'block');
        $('.clsFeatureZone2').css('display', 'none');
        $('.clsFeatureZone3').css('display', 'none');
    });
    $('#btnEstimateZ2').click(function (e) {
        $('.clsZone1').css('display', 'none');
        $('.clsZone2').css('display', 'inline-block');
        $('.clsZone3').css('display', 'none');
        $('.clsTotalZone1').css('display', 'none');
        $('.clsTotalZone2').css('display', 'block');
        $('.clsTotalZone3').css('display', 'none');
        $('.clsFeatureZone1').css('display', 'none');
        $('.clsFeatureZone2').css('display', 'block');
        $('.clsFeatureZone3').css('display', 'none');
    });
    $('#btnEstimateZ3').click(function (e) {
        $('.clsZone1').css('display', 'none');
        $('.clsZone2').css('display', 'none')
        $('.clsZone3').css('display', 'inline-block');
        $('.clsTotalZone1').css('display', 'none');
        $('.clsTotalZone2').css('display', 'none');
        $('.clsTotalZone3').css('display', 'block');
        $('.clsFeatureZone1').css('display', 'none');
        $('.clsFeatureZone2').css('display', 'none');
        $('.clsFeatureZone3').css('display', 'block');
    });
    $('#liClose').click(function (e) {
        var publicVar = $(this).attr('public');
        var url = '/Quotation/GetQuotation';
        if (publicVar == "public") {
            url = '/Blogs/GetQuotation';
        }
        $.ajax({
            url: $.absoluteurl(url),
            type: "POST",
            success: function (response) {                
                $('#CustomQuote').html("");
                $('#CustomQuote').html(response);            
            }
        });
    });
});