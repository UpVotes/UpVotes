
$(document).ready(function ()
{
    $('.date-control input').datepicker();
    
    $("#txtCompanySoftware").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: $.absoluteurl('/SubmitReview/GetCompanySoftwareAutoComplete'),
                type: "POST",
                dataType: "json",
                async: false,
                data: {
                    search: request.term,
                    type: $('#ddlProvider').val() == "1" ? 2 : 1
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Name,
                            id: item.ID,
                        }
                    }))
                }
            });
        },
        delay: 0,
        minLength: 3,
        select: function (event, ui) {
            if (ui.item != null) {
                $("#hdnCompanySoftwareID").val(ui.item.id);
            }
            else {
                $("#txtCompanySoftware").val('');
                $("#hdnCompanySoftwareID").val('0');
            }
        },
        change: function (event, ui) {
            if (ui.item != null) {
                $("#hdnCompanySoftwareID").val(ui.item.id);
            }
            else {
                $("#txtCompanySoftware").val('');
                $("#hdnCompanySoftwareID").val('0');

            }
        }
    });

    $('#ddlProvider').change(function () {
        $("#txtCompanySoftware").val('');
        $("#hdnCompanySoftwareID").val('0');
        $('#ddlSponsorshipCategory').val('0');        
        $("#txtStartDate").val('');
        $("#txtEndDate").val('');
    });

    $.DisplayMessage = function (isError, displayText)
    {
        if (isError)
        {
            $("#divFailureMessage").removeClass('hide');
            $('#spnMessage').css('display', 'block');
            $('#spnMessage').text(displayText);
            $('html, body').animate({ scrollTop: 0 }, 'slow');
        }
        else
        {
            $("#divFailureMessage").addClass('hide');
            $('#spnMessage').html("");
        }
    }

    $.SaveModeValidations = function ()
    {        
        try
        {
            var status = 0;            
            if (($('#txtCompanySoftware').val() === "" || $('#txtCompanySoftware').val() === undefined) && $('#hdnCompanySoftwareID').val() != "0")
            {
                status = 1;
            }

            if ($('#ddlSponsorshipCategory').val() == '0')
            {
                status = 1;
            }

            if ($('#txtStartDate').val() == "" || $('#txtEndDate').val() == "")
            {
                status = 1;
            }
            else if (!isDate($('#txtStartDate').val()) || !isDate($('#txtStartDate').val())) {
                status = 2;
            }
            else {
                var startDate = new Date($('#txtStartDate').val());
                var endDate = new Date($('#txtEndDate').val());
                if (startDate > endDate) {
                    status = 3;
                }
            }

            if (status === 1)
            {
                $.DisplayMessage(true, "Fields marked with * are mandatory.");
                return false;
            }
            else if (status === 2)
            {
                $.DisplayMessage(true, "Invalid date.");
                return false;
            }
            else if (status === 3) {
                $.DisplayMessage(true, "Start date cannot be greater than end date.");
                return false;
            }
            else
            {
                $.DisplayMessage(false, "");
                return true;
            }

        } catch (e)
        {

        }
    }

    $('#btnScheduler').click(function () {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/AdminSponsorship/SchedulerToDeactivateSponsor'),
            type: "POST",
            success: function (response) {
                $('#ajax_loaderDashboard').hide();
                if (response.IsSuccess) {
                    alert('All expired sponsors are deactivated');
                    $('#showExpiredSponsorShipForm').click();
                }
            },
            error: function (e) {
                alert('Some error has occured');
                $('#ajax_loaderDashboard').hide();
            }
        });
    });

    $('#btnSponsorship').click(function () {
        $.DisplayMessage(false, "");
        $('#ajax_loaderDashboard').show();
        if ($.SaveModeValidations()) {            
            var Provider = $('#ddlProvider').val();
            var SponsorshipCategoryID = $('#ddlSponsorshipCategory').val();
            var CompanyOrSoftwareID = $('#hdnCompanySoftwareID').val();
            var StartDate = $("#txtStartDate").val();
            var EndDate = $("#txtEndDate").val();
            var CompanyOrSoftwareName = $('#txtCompanySoftware').val();
            $.ajax({
                url: $.absoluteurl('/AdminSponsorship/ApplySponsorship'),
                data: { Provider: Provider, SponsorshipCategoryID: SponsorshipCategoryID, CompanyOrSoftwareID: CompanyOrSoftwareID,CompanyOrSoftwareName:CompanyOrSoftwareName, StartDate: StartDate, EndDate: EndDate },
                type: "POST",
                success: function (response) {
                    $('#ajax_loaderDashboard').hide();
                    if (response.IsSuccess) {
                        alert('Successfully applied.');
                    }
                },
                error: function (e) {
                    alert('Some error has occured');
                    $('#ajax_loaderDashboard').hide();
                }
            });

        } else {
            $('#ajax_loaderDashboard').hide();
        }
    });

});


function isDate(txtDate)
{
    var currVal = txtDate;
    if (currVal === '')
        return false;

    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/; //Declare Regex
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;

    //Checks for mm/dd/yyyy format.
    var dtMonth = dtArray[1];
    var dtDay = dtArray[3];
    var dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth === 4 || dtMonth === 6 || dtMonth === 9 || dtMonth === 11) && dtDay === 31)
        return false;
    else if (dtMonth === 2) 
    {
        var isleap = (dtYear % 4 === 0 && (dtYear % 100 !== 0 || dtYear % 400 === 0));
        if (dtDay > 29 || (dtDay === 29 && !isleap))
            return false;
    }
    return true;
}