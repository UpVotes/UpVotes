function addRating(value) {
    $('#hdnRating')[0].value = value;

    $("#divAddRating").find("span").removeClass('rateaddreview');
    $("#divAddRating").find("span").addClass('ratedisabledaddreview');

    for (var i = 1; i <= value; i++) {
        $('#spnRate_' + i).removeClass('ratedisabledaddreview');
        $('#spnRate_' + i).addClass('rateaddreview');
    }
}

$(document).ready(function () {

    $.clearCreateReviewControls = function () {
        $('#txtProjectName')[0].value = '';
        $('#ddlServiceCategory')[0].value = '0';
        $('#txtFeedBackSummary')[0].value = '';
        $('#txtUserName')[0].value = '';
        $('#txtDesignation')[0].value = '';
        $('#txtcompanyName')[0].value = '';
        $('#txtPhoneNumber')[0].value = '';
        $('#txtEmail')[0].value = '';
        $('#hdnRating')[0].value = '';
        $('#txtCompanyBeingReviewed')[0].value = '';
        $("#divAddRating").find("span").removeClass('rateaddreview');
        $("#divAddRating").find("span").addClass('ratedisabledaddreview');
    }

    $.ClearDropdown = function (DdlObject) {
        if (DdlObject[0].options != undefined) {
            var OptionsDdlObject = DdlObject[0].options;
            for (var j = 1; j < OptionsDdlObject.length; j++) {
                OptionsDdlObject.remove(j);
                j--;
            }
        }
    }

    $.fn.LoadOptions = function (data, name, value) {
        if (data != undefined && data != null) {
            this.sel = $(this);
            for (i = 0; i < data.length; i++) {
                this.sel.append($("<option style='color:black;' title='" + $(data[i]).attr(name) + "' value='" + $(data[i]).attr(value) + "' >" + $(data[i]).attr(name) + "</option>"));
            }
        }
    }

    $.getFocusAreas = function () {
        $.ajax({
            url: $.absoluteurl('/Company/GetFocusArea'),
            cache: false,
            async: false,
            datatype: 'json',
            type: 'GET',
            success: function (response) {
                $.ClearDropdown($('#ddlServiceCategory'));
                $('#ddlServiceCategory').LoadOptions(response.focusAreaList, 'FocusAreaName', 'FocusAreaID');
            }
        })
    }

    //Method to create reviews for a company. A user can create as many as reviews.
    $('#btnSubmitReview').click(function () {
        if ($('#hdnCompanyID')[0] == undefined || $('#hdnCompanyID')[0].value == "") {
            alert("Please login and select a company to submit review...")
        }
        else {
            var companyName = document.getElementById("hdnCompanyName").value;
            $('#txtCompanyBeingReviewed')[0].value = companyName;
        }

        $('#dialog').dialog({ beforeClose: function () { return $.clearCreateReviewControls(); }, modal: true, hide: "slide", width: '90%', position: 'center', title: 'Submit Review' });
        $.getFocusAreas();
    });

    $.ValidateInputFields = function () {
        var isValid = true;
        $('#divErrorMessage')[0].innerHTML = '';

        if ($('#txtProjectName')[0].value == '') {
            isValid = false;
            $('#divErrorMessage').append("<p>Project Name is required.</p>");
        }

        if ($('#txtProjectName')[0].value == '') {
            isValid = false;
            $('#divErrorMessage').append("<p>Project Name is required.</p>");
        }

        if ($('#ddlServiceCategory')[0].value == '0') {
            isValid = false;
            $('#divErrorMessage').append("<p>Service Category is required.</p>");
        }

        if ($('#txtFeedBackSummary')[0].value == '') {
            isValid = false;
            $('#divErrorMessage').append("<p>FeedBack is required.</p>");
        }
        else {
            if ($('#txtFeedBackSummary')[0].value.length > 4000) {
                isValid = false;
                $('#divErrorMessage').append("<p>Feedback cannot be greater than 4000 characters.</p>");
            }
        }

        if ($('#hdnRating')[0].value == '') {
            isValid = false;
            $('#divErrorMessage').append("<p>Rating is required.</p>");
        }

        if ($('#txtPhoneNumber')[0].value == '') {
            isValid = false;
            $('#divErrorMessage').append("<p>Phone Number is required.</p>");
        }

        if ($('#txtEmail')[0].value == '') {
            isValid = false;
            $('#divErrorMessage').append("<p>Email is required.</p>");
        }
        else {
            if (!ValidateEmail($('#txtEmail')[0].value)) {
                isValid = false;
                $('#divErrorMessage').append("<p>Invalid email address.</p>");
            }
        }

        if (!isValid) {
            $('#divErrorMessage').show();
        }
        else {
            $('#divErrorMessage')[0].innerHTML = '';
            $('#divErrorMessage').hide();
        }

        return isValid;
    }

    function ValidateEmail(email) {
        var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        return expr.test(email);
    };


    $("#btnSaveReviews").click(function () {
        if ($.ValidateInputFields()) {

            var CompanyID = $('#hdnCompanyID')[0].value;
            var FocusAreaID = $('#ddlServiceCategory')[0].value;
            var ReviewerCompanyName = $('#txtcompanyName')[0].value;
            var Designation = $('#txtDesignation')[0].value;
            var ProjectName = $('#txtProjectName')[0].value;
            var FeedBack = $('#txtFeedBackSummary')[0].value;
            var Rating = $('#hdnRating')[0].value;
            var PhoneNumber = $('#txtPhoneNumber')[0].value;
            var Email = $('#txtEmail')[0].value;            

            var companyReviewModel = '{CompanyID:\'' + parseInt(CompanyID) + '\',FocusAreaID:\'' + parseInt(FocusAreaID) + '\',ReviewerCompanyName:\'' + ReviewerCompanyName + '\',Email:\'' + Email + '\',PhoneNumber:\'' + PhoneNumber + '\',Designation:\'' + Designation + '\',ProjectName:\'' + ProjectName + '\',FeedBack:\'' + FeedBack + '\', Rating:\'' + parseInt(Rating) + '\'}';

            $.ajax({
                url: $.absoluteurl('/Company/AddReview'),
                mtype: 'POST',
                cache: false,
                datatype: 'json',
                data: { companyReviewModel: companyReviewModel },
                success: function (data) {
                    if (data == 'True') {
                        alert('Thank you for your feedback');
                        $("#dialog").dialog('close');
                        window.location.reload();
                    }
                },
                error: function (a, b, c) {
                    alert(a + '-' + b + '-' + c);
                    $('#divErrorMessage').append("<p>Error- '" + a + '-' + b + '-' + c + + "'.</p>");
                    $('#divErrorMessage').show();
                }
            });
        }
    })
});