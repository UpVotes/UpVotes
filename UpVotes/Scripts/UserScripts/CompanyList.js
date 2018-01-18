$(document).ready(function () {

    $.setData = function (ui) {
        $("#txtCompanySearch")[0].value = ui.item.value;
        $("#txtCompanySearch").change();
    }

    $.getData = function (request, response, type) {
        if ($.trim(request.term) != "" && $.trim(request.term).length > 0) {
            $.ajax({
                url: $.absoluteurl('/CompanyList/GetData?type=' + type + '&focusArea=' + $("#hdnFocusArea")[0].value),
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
                                if (type == 1) {
                                    return {
                                        label: item.Name,
                                        value: item.Name,                                        
                                    }
                                }
                                else if (type == 2) {
                                    return {
                                        label: item.Location,
                                        value: item.City,
                                    }
                                }
                            }))
                }
            })
        }
    }

    $("#txtCompanySearch").autocomplete({
        source: function (request, response) {
            $.getData(request, response, 1)
        },
        focus: function () {
            return false;
        },
        //minLength: 3,
        select: function (event, ui) {
            $.setData(ui);
        }
    });



    var companyName = "";
    var avgHourlyRate = "";
    var employeeCount = "";
    var location = "";
    var sortBy = 'ASC';



    $("#txtCompanySearch").change(function () {
        debugger;

        companyName = $("#txtCompanySearch")[0].value;

        $.ajax({
            type: "POST",
            url: $.absoluteurl('/' + jQuery.parseJSON('@Html.Raw(Json.Encode(Session["FocusAreaName"]))')),
            data: { companyid: companyName, minRate: 0, maxRate: 0, minEmployee: 0, maxEmployee: 0, sortby: sortBy, location: location },// Location of the service
            success: function (json) {//On Successful service call
                $('#complist').html(json);
            }
        });
    });

    $("#ddlAvgHourlyRateSearch").change(function () {
        debugger;

        $.ajax({
            type: "POST",
            url: $.absoluteurl('/' + $("#hdnFocusArea")[0].value),
            data: { companyid: companyName, minRate: 0, maxRate: 0, minEmployee: 0, maxEmployee: 0, sortby: sortBy, location: location },// Location of the service
            success: function (json) {//On Successful service call
                $('#complist').html(json);
            }
        });
    });
});