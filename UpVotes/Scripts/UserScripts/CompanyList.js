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
    }

    $.getData = function (request, response, type) {
        if ($.trim(request.term) != "" && $.trim(request.term).length > 0) {
            $.ajax({
                url: $.absoluteurl('/CompanyList/GetDataForAutoComplete?type=' + type),
                dataType: "json",
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

    $.GetCompanyListBasedOnCriteria = function (e) {
        try {
            var location = $("#txtLocationSearch")[0].value == "" ? "0" : $("#txtLocationSearch")[0].value;
            var compid = $("#txtCompanySearch")[0].value == "" ? "0" : $("#txtCompanySearch")[0].value;
            var avgHourlyRate = $("#ddlAvgHourlyRateSearch")[0].value;
            var employeeCount = $("#ddlEmployeesSearch")[0].value;
            var sortby = 'Asc';

            location = location == "0" ? (window.location.pathname.split('/').length == 3 ? window.location.pathname.split('/')[2].replace(/ /g, "-") : location) : location;
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
                data: { companyid: compid, minRate: AvgminRate, maxRate: Avgmaxrate, minEmployee: minEmp, maxEmployee: maxEmp, sortby: sortby, location: location, PageNo: PageNo, PageSize: PageSize, FirstPage: FirstPage, LastPage: LastPage },// Location of the service
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

    $(".userReviews").click(function () {
        //$('#accordian').show();
        //$('#accordian').accordion();
        $.ajax({
            url: $.absoluteurl('/CompanyList/GetUserReviews'),
            mtype: "POST",
            data:
            {
                companyNames: $("#hdnCompanyNames")[0].value
            },
            success: function (data) {
                $('#userReviews').html(data);
            },
            error: function (a, b, c) { debugger; }
        });
    });
});