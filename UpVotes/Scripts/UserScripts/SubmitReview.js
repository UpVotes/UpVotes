
$(document).ready(function () {

    $("#txtCompanyBeingReviewed").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: $.absoluteurl('/SubmitReview/GetCompanySoftwareAutoComplete'),type: "POST",
                dataType: "json",
                async: false,
                data:{     
                    search: request.term,
                    type:2
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
            if (ui.item != null)
            {
                $("#hdnCompanyID").val(ui.item.id);
            }
            else {
                $("#txtCompanyBeingReviewed").val('');
                $("#hdnCompanyID").val('0');

            }
                  
        },
        change: function (event, ui) {
            if (ui.item != null) {
                $("#hdnCompanyID").val(ui.item.id);
            }
            else {
                $("#txtCompanyBeingReviewed").val('');
                $("#hdnCompanyID").val('0');

            }
        }
    });

    $("#txtSoftwareBeingReviewed").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: $.absoluteurl('/SubmitReview/GetCompanySoftwareAutoComplete'), type: "POST",
                dataType: "json",
                async: false,
                data: {
                    search: request.term,
                    type: 1
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
                $("#hdnCompanyID").val(ui.item.id);
            }
            else {
                $("#txtSoftwareBeingReviewed").val('');
                $("#hdnSoftwareID").val('0');

            }

        },
        change: function (event, ui) {
            if (ui.item != null) {
                $("#hdnCompanyID").val(ui.item.id);
            }
            else {
                $("#txtSoftwareBeingReviewed").val('');
                $("#hdnSoftwareID").val('0');

            }
        }
    });

    $.clearCompanyCreateReviewControls = function () {
        $('#txtCompanyBeingReviewed').val('');
        $('#hdnCompanyID').val('0');
        $('#ddlServiceCategory').val('0');
        $('#txtServiceProjectName').val('');
        $('#txtCompanyFeedBackSummary').val('');
        $('#UlCompanyRating').attr('value', '0');
        $('#txtCompanyReviewerUserName').val('');
        $('#txtCompanyReviewerDesignation').val('');
        $('#txtReviewercompanyName').val('');
        $('#txtReviewerCompanyEmail').val('');
        $('#txtReviewerCompanyPhoneNumber').val('');
    }

    $.clearSoftwareCreateReviewControls = function () {
        $('#txtSoftwareBeingReviewed').val('');
        $('#hdnSoftwareID').val('0');
        $('#ddlSoftwareCategory').val('0');
        $('#txtSoftwareFeedBackSummary').val('');
        $('#UlSoftwareRating').attr('value', '0');
        $('#txtSoftwareReviewerUserName').val('');
        $('#txtSoftwareReviewerDesignation').val('');
        $('#txtReviewerSoftwareCompanyName').val('');
        $('#txtReviewerSoftwareCompanyEmail').val('');
        $('#txtSoftwarePhoneNumber').val('');
    }

    $('.CompanyStar').click(function () {
        $('#UlCompanyRating').attr('class','');
        $('#UlCompanyRating').addClass('star-rate'); 
        var classname = 'rating-'+ $(this).attr('value');        
        $('#UlCompanyRating').addClass(classname);
        $('#UlCompanyRating').attr('value', $(this).attr('value'));
    });

    $('.SoftwareStar').click(function () {
        $('#UlSoftwareRating').attr('class', '');
        $('#UlSoftwareRating').addClass('star-rate');
        var classname = 'rating-' + $(this).attr('value');
        $('#UlSoftwareRating').addClass(classname);
        $('#UlSoftwareRating').attr('value', $(this).attr('value'));
    });

    $.ValidateCompanyInputFields = function () {
        var isValid = true;       
        var isEmailValid = true;

        if ($('#txtCompanyBeingReviewed').val() == '') {
            isValid = false;            
        }

        if ($('#hdnCompanyID').val() == '' || $('#hdnCompanyID').val() == '0') {
            isValid = false;           
        }

        if ($('#ddlServiceCategory').val() == '0') {
            isValid = false;
        }

        if ($('#txtServiceProjectName').val() == '0') {
            isValid = false;
        }

        if ($('#txtCompanyFeedBackSummary').val() == '') {
            isValid = false;            
        }        

        if ($('#UlCompanyRating').attr('value') == '0') {
            isValid = false;            
        }

        if ($('#txtCompanyReviewerUserName').val() == '') {
            isValid = false;            
        }

        if ($('#txtCompanyReviewerDesignation').val() == '') {
            isValid = false;            
        }

        if ($('#txtReviewercompanyName').val() == '') {
            isValid = false;
        }

        if ($('#txtReviewerCompanyEmail').val() == '') {
            isValid = false;
        }
        else {
            if (!ValidateEmail($('#txtReviewerCompanyEmail').val())) {
                isEmailValid = false;                
            }
        }

        if (!isValid) {
            $('#errCompanyMandatory').html('All fields marked with * are mandatory');
            $('#errCompanyMandatory').show();
        }
        else if(!isEmailValid)
        {
            $('#errCompanyMandatory').html("Invalid email address.");
            $('#errCompanyMandatory').show();
        }
        else {
            $('#errCompanyMandatory').html("");
            $('#errCompanyMandatory').hide();
        }

        return (isValid && isEmailValid);
    }

    $.ValidateSoftwareInputFields = function () {
        var isValid = true;
        var isEmailValid = true;

        if ($('#txtSoftwareBeingReviewed').val() == '') {
            isValid = false;
        }

        if ($('#hdnSoftwareID').val() == '' || $('#hdnSoftwareID').val() == '0') {
            isValid = false;
        }

        if ($('#ddlSoftwareCategory').val() == '0') {
            isValid = false;
        }

        if ($('#txtSoftwareFeedBackSummary').val() == '') {
            isValid = false;
        }

        if ($('#UlSoftwareRating').attr('value') == '0') {
            isValid = false;
        }

        if ($('#txtSoftwareReviewerUserName').val() == '') {
            isValid = false;
        }

        if ($('#txtSoftwareReviewerDesignation').val() == '') {
            isValid = false;
        }

        if ($('#txtReviewerSoftwareCompanyName').val() == '') {
            isValid = false;
        }

        if ($('#txtReviewerSoftwareCompanyEmail').val() == '') {
            isValid = false;
        }
        else {
            if (!ValidateEmail($('#txtReviewerSoftwareCompanyEmail').val())) {
                isEmailValid = false;
            }
        }

        if (!isValid) {
            $('#errSoftwareMandatory').html('All fields marked with * are mandatory');
            $('#errSoftwareMandatory').show();
        }
        else if (!isEmailValid) {
            $('#errSoftwareMandatory').html("Invalid email address.");
            $('#errSoftwareMandatory').show();
        }
        else {
            $('#errSoftwareMandatory').html("");
            $('#errSoftwareMandatory').hide();
        }

        return (isValid && isEmailValid);
    }

    function ValidateEmail(email) {
        var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        return expr.test(email);
    };


    $("#btnSaveCompanyReviews").click(function () {
        if ($.ValidateCompanyInputFields()) {
            var CompanyName = $('#txtCompanyBeingReviewed').val();
            var CompanyID = $('#hdnCompanyID').val();
            var FocusAreaID = $('#ddlServiceCategory').val();
            var ProjectName = $('#txtServiceProjectName').val();
            var FeedBack = $('#txtCompanyFeedBackSummary').val();
            var Rating = $('#UlCompanyRating').attr('value');
            var ReviewerFullName = $('#txtCompanyReviewerUserName').val();
            var Designation = $('#txtCompanyReviewerDesignation').val();
            var ReviewerCompanyName = $('#txtReviewercompanyName').val();
            var Email = $('#txtReviewerCompanyEmail').val();
            var PhoneNumber = $('#txtReviewerCompanyPhoneNumber')[0].value;


            //var companyReviewModel = '{"CompanyID":"' + parseInt(CompanyID) + '","FocusAreaID":"' + parseInt(FocusAreaID) + '","ReviewerCompanyName":"' + ReviewerCompanyName + '","Email":\"' + Email + '","PhoneNumber":"' + PhoneNumber + '","Designation":"' + Designation + '","ProjectName":"' + ProjectName + '","UserName":"' + ReviewerFullName + '", "FeedBack":"' + FeedBack + '", "Rating":"' + parseInt(Rating) + '"}';
            //var companyReviewModel = '{"CompanyID":' + parseInt(CompanyID) + '}';
            $.ajax({
                url: $.absoluteurl('/SubmitReview/AddCompanyReview'),
                mtype: 'POST',
                cache: false,
                datatype: 'json',
                data: { "CompanyName": CompanyName, "CompanyID": parseInt(CompanyID), "FocusAreaID": parseInt(FocusAreaID), "ReviewerCompanyName": ReviewerCompanyName, "Email": Email, "PhoneNumber": PhoneNumber, "Designation": Designation, "ProjectName": ProjectName, "UserName": ReviewerFullName, "FeedBack": FeedBack, "Rating": parseInt(Rating) },
                success: function (data) {
                    if (data == 'Success') {
                        $('#divCompanyThank').removeClass('hide');
                        $('#spnCompanyThank').html('Thank you for your review to "' + $('#txtCompanyBeingReviewed').val() + '"');
                    }
                    else {
                        alert(data);
                    }
                },
                error: function (a, b, c) {
                    alert(a + '-' + b + '-' + c);
                    $('#divErrorMessage').append("<p>Error- '" + a + '-' + b + '-' + c + + "'.</p>");
                    $('#divErrorMessage').show();
                }
            });
        }
    });

    $("#btnSaveSoftwareReviews").click(function () {
        if ($.ValidateSoftwareInputFields()) {
            var SoftwareName = $('#txtSoftwareBeingReviewed').val();
            var SoftwareID = $('#hdnSoftwareID').val();
            var ServiceCategoryID = $('#ddlSoftwareCategory').val();
            var ProjectName = '';
            var FeedBack = $('#txtSoftwareFeedBackSummary').val();
            var Rating = $('#UlSoftwareRating').attr('value');
            var ReviewerFullName = $('#txtSoftwareReviewerUserName').val();
            var Designation = $('#txtSoftwareReviewerDesignation').val();
            var ReviewerCompanyName = $('#txtReviewerSoftwareCompanyName').val();
            var Email = $('#txtReviewerSoftwareCompanyEmail').val();
            var PhoneNumber = $('#txtSoftwarePhoneNumber')[0].value;
            
            $.ajax({
                url: $.absoluteurl('/SubmitReview/AddSoftwareReview'),
                mtype: 'POST',
                cache: false,
                datatype: 'json',
                data: { "SoftwareName": SoftwareName, "SoftwareID": parseInt(SoftwareID), "ServiceCategoryID": parseInt(ServiceCategoryID), "ReviewerCompanyName": ReviewerCompanyName, "Email": Email, "PhoneNumber": PhoneNumber, "Designation": Designation, "ProjectName": ProjectName, "UserName": ReviewerFullName, "FeedBack": FeedBack, "Rating": parseInt(Rating) },
                success: function (data) {
                    if (data == 'Success') {
                        $('#divSoftwareThank').removeClass('hide');
                        $('#spnSoftwareThank').html('Thank you for your review to "' + $('#txtSoftwareBeingReviewed').val() + '"');
                    }
                    else {
                        alert(data);
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