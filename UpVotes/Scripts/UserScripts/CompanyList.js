$(document).ready(function () {
       

    $.setData = function (ui, type) {
        if (type == 1) {
            $("#txtCompanySearch")[0].value = ui.item.value;
            $("#txtCompanySearch").change();
        }
        else if (type == 2) {
            $("#txtLocationSearch")[0].value = ui.item.value;
            $("#txtLocationSearch").change();
        }
        else if (type == 3) {
            $("#txtCompanyReviewSearch")[0].value = ui.item.value;
            $("#txtCompanyReviewSearch").change();
        }
    }

    $.getData = function (request, response, type) {
        if ($.trim(request.term) != "" && $.trim(request.term).length > 0) {
            $.ajax({
                url: $.absoluteurl('/CompanyList/GetDataForAutoComplete?type=' + type),
                dataType: "json",
                async: false,
                data:
                {
                    featureClass: "leftalign",
                    style: "full",
                    maxRows: 5,
                    starts_with: request.term
                },
                success: function (data) {
                    response(
                        $.map(data,
                            function (item) {
                                return {
                                    label: item.Name,
                                    value: item.Name,
                                }
                            }))
                }
            });
        }
    }

    $("#txtCompanySearch").autocomplete({
        source: function (request, response) {
            $.getData(request, response, 1)
        },
        delay: 0,
        focus: function () {
            return false;
        },
        minLength: 3,
        select: function (event, ui) {
            $.setData(ui, 1);
        }
    });

    $("#txtCompanyReviewSearch").autocomplete({
        source: function (request, response) {
            $.getData(request, response, 1)
        },
        delay: 0,
        focus: function () {
            return false;
        },
        minLength: 3,
        select: function (event, ui) {
            $.setData(ui, 3);
        }
    });

    $("#txtLocationSearch").autocomplete({
        source: function (request, response) {
            $.getData(request, response, 2)
        },
        delay: 0,
        focus: function () {
            return false;
        },
        minLength: 3,
        select: function (event, ui) {
            $.setData(ui, 2);
        }
    });

    $("#txtCompanySearch").change(function () {
        $.GetCompanyListBasedOnCriteria(this);
    });

    $("#txtLocationSearch").change(function () {
        $.GetCompanyListBasedOnCriteria(this);
    });

    $("#ddlAvgHourlyRateSearch").change(function () {
        $.GetCompanyListBasedOnCriteria(this);
    });

    $("#ddlEmployeesSearch").change(function () {
        $.GetCompanyListBasedOnCriteria(this);
    });

    $("#txtCompanyReviewSearch").change(function () {
        $.GetUserReviewListBasedOnCriteria(this);
    });       

    $.GetCompanyListBasedOnCriteria = function (e) {
        try {
            
            var location = $("#txtLocationSearch")[0].value == "" ? "0" : $("#txtLocationSearch")[0].value;
            var compid = $("#txtCompanySearch")[0].value == "" ? "0" : $("#txtCompanySearch")[0].value;
            var avgHourlyRate = $("#ddlAvgHourlyRateSearch")[0].value;
            var employeeCount = $("#ddlEmployeesSearch")[0].value;
            var sortby = 'Asc';
            var subFocusArea = $('#hdnSubFocusArea').val();
            location = location == "0" ? $('#hdnLocation').val() : location;
            //(window.location.pathname.split('/').length == 3 ? window.location.pathname.split('/')[2].replace(/ /g, "-") : location)
            var hourlyRateArray = avgHourlyRate.split("-");
            var employeeArray = employeeCount.split("-");

            var AvgminRate = hourlyRateArray[0];
            var Avgmaxrate = hourlyRateArray[1] == undefined ? '0' : hourlyRateArray[1];
            var minEmp = employeeArray[0];
            var maxEmp = employeeArray[1] == undefined ? '0' : employeeArray[1];

            var PageNo = e.className.indexOf('Pagenumber') == -1 ? 1 : parseInt($(e).attr('page'));//1;//parseInt($(this).attr('page'));            
            var PageSize = 10;
            var FirstPage = parseInt($('.FirstPageindex').attr('page'));
            var LastPage = isNaN(parseInt($('.LastPageindex').attr('page'))) ? 1 : parseInt($('.LastPageindex').attr('page'));

            $.ajax({
                type: "POST",
                url: $.absoluteurl('/CompanyList/CompanyList'),
                cache: false,
                async: false,
                data: { companyid: compid, minRate: AvgminRate, maxRate: Avgmaxrate, minEmployee: minEmp, maxEmployee: maxEmp, sortby: sortby, location: location, subFocusArea: subFocusArea, PageNo: PageNo, PageSize: PageSize, FirstPage: FirstPage, LastPage: LastPage },// Location of the service
                success: function (json) {
                    $('#complist').html(json);
                    $('.lazy').lazy();
                },
                error: function (a, b, c) {
                    debugger;
                }
            });
        }
        catch (e) { debugger; }
    }

    $('#btnLoadMoreReviews').click(function () {
        var pagesize = $('#btnLoadMoreReviews').attr('pagesize');
        $('#btnLoadMoreReviews').attr('pagesize', parseInt(pagesize) + 10);
        $.GetUserReviewListBasedOnCriteria(this);
    });

    $.GetUserReviewListBasedOnCriteria = function (e) {
        try {
            var companyname = $("#txtCompanyReviewSearch").val();
            var pagesize = $('#btnLoadMoreReviews').attr('pagesize');

            $.ajax({
                type: "POST",
                url: $.absoluteurl('/CompanyList/GetUserReviews'),
                cache: false,
                async: false,
                data: { companyname: companyname, PageSize: pagesize },
                success: function (json) {
                    $('#divUserReviews').html(json);
                    if (companyname != "") {
                        $('#btnLoadMoreReviews').attr('pagesize', 10);
                    }                    
                },
                error: function (a, b, c) {
                    debugger;
                }
            });
        }
        catch (e) { debugger; }
    }

    //$(".userReviews").click(function () {
    //    $.ajax({
    //        url: $.absoluteurl('/CompanyList/GetCompanyNames'),
    //        mtype: "POST",
    //        cache: false,
    //        success: function (data) {
    //            $('#userReviews').html("");
    //            $('#userReviews').show();
    //            $('#userReviews').html(data);
    //        },
    //        error: function (a, b, c) { debugger; }
    //    });
    //});
    //$(".userReviews").click(function () {
    //    $('#userReviews').css("display", "")
    //});
    //$(".Over-view").click(function () {   
    //    $('#Overview').css("display", "")
    //    $('#userReviews').css("display", "none")
    //});
    //$(".classquote").click(function () {        
    //    $('#CustomQuote').css("display", "");
    //    $('#userReviews').css("display", "none")
    //});
    //$.GetCompanyReviews = function (companyName) {
    //    $.ajax({
    //        type: "POST",
    //        url: $.absoluteurl('/CompanyList/GetUserReviews'),
    //        cache: false,
    //        async: false,
    //        data: { companyName: companyName },// Location of the service
    //        success: function (json) {
    //            companyName = companyName.replace(/ /g, "");
    //            $('#' + companyName + '1').html(json);
    //            $('.lazy').lazy();
    //        },
    //        error: function (a, b, c) {
    //            debugger;
    //        }
    //    });
    //};

    //$('.panel-title a').bind('click', function () {
    //    $(this).find('i').toggleClass('fa-plus fa-minus')
    //        .closest('panel').siblings('panel')
    //        .find('i')
    //        .removeClass('fa-minus').addClass('fa-plus');
    //});


});

function GetNews(title) {
    var baseAddress = $.absoluteurl(window.location.origin + '/news/' + encodeURI(title));
    window.open(baseAddress, '_blank')
}