var CompanyID = 0;
var uploadedCompanyLogo;

$(document).ready(function () {    
    var isAddMode = true;
    var isAdmin = false;
    var countryList = '';

    $("#divCompanyTabs").tabs();
    $("#txtCompanySummary").Editor();
    $("#txtKeyClients").Editor();
    $('#ImgLoading').attr("src", $.absoluteurl('/images/ajax-loader.gif'));

    $(".numericOnly").keypress(function (e) {
        return $(this).IsValidNumber(e);
    });

    $.LoadCountry = function () {
        if (countryList != undefined && countryList != null && countryList != '' && countryList.length > 0) {
            $.ClearDropdown($('#ddlCountry_0'));
            $('#ddlCountry_0').LoadOptions(countryList, 'CountryName', 'CountryID');
        }

        $.ajax({
            url: $.absoluteurl('/UserCompanyList/GetCountry'),
            cache: false,
            async: false,
            datatype: 'json',
            type: 'GET',
            success: function (response) {
                $.ClearDropdown($('#ddlCountry_0'));
                $('#ddlCountry_0').LoadOptions(response.countryList, 'CountryName', 'CountryID');
                countryList = response.countryList;
            },
            error: function (e) {
                $('#spnMessage').css('display', 'block');
                $('#spnMessage').html("Some error has occured. Unable to get the countries. Please contact admin.");
            }
        });
    }

    $('#ddlCountry_0').change(function () {
        var countryID = $('#ddlCountry_0')[0].value;
        if (countryID != 0) {
            $.LoadState(countryID, 0);
        }
    });

    $.LoadStatesOnBranchCountry = function (branchNum) {
        var countryID = $('#ddlCountry_' + branchNum)[0].value;
        if (countryID != 0) {
            $.LoadState(countryID, branchNum);
        }
    };

    $.LoadState = function (countryID, branchNum) {
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/GetStates?countryID=' + countryID),
            cache: false,
            async: false,
            datatype: 'json',
            type: 'GET',
            success: function (response) {
                $.ClearDropdown($('#ddlStates_' + branchNum));
                $('#ddlStates_' + branchNum).LoadOptions(response.statesList, 'StateName', 'StateID');
            },
            error: function (e) {
                debugger;
                $('#spnMessage').css('display', 'block');
                $('#spnMessage').html("Some error has occured. Unable to get the states. Please contact admin.");
            }
        });
    }

    $.ValidateEmail = function (sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) {
            return true;
        }
        else {
            return false;
        }
    }

    $.ValidateWorkEmailID = function (sEmail) {
        var reg = /^([\w-\.]+@(?!gmail.com)(?!yahoo.com)(?!hotmail.com)(?!yahoo.co.in)(?!aol.com)(?!abc.com)(?!xyz.com)(?!pqr.com)(?!rediffmail.com)(?!live.com)(?!outlook.com)(?!me.com)(?!msn.com)(?!ymail.com)([\w-]+\.)+[\w-]{2,4})?$/;
        if (reg.test(sEmail)) {
            return true;
        }
        else {
            return false;
        }
    }

    var focusAreaObject = '';
    var actualFocusAreaArray = [];
    var companyBranchArray = [];

    $.GetFocusAreaObject = function (obj) {
        focusAreaObject = obj;
    }

    $.DisplayMessage = function (isError, displayText) {
        if (isError) {
            $("#divFailureMessage").show();
            $('#spnMessage').css('display', 'block');
            $('#spnMessage').html(displayText);

            $("#divSuccessMessage").hide();
            $('#spnSuccessMessage').css('display', 'none');
            $('#spnSuccessMessage').html("");
        }
        else {
            $("#divFailureMessage").hide();
            $('#spnMessage').css('display', 'none');
            $('#spnMessage').html("");
        }
    }

    $.SaveModeValidations = function () {
        try {
            var status = 0;
            var fileMessage = '';
            if ($("#txtCompanyName")[0].value == "" || $("#txtCompanyName")[0].value == undefined) {
                status = 1;
            }

            if ($("#UplAttachment")[0].value != "" && $("#UplAttachment")[0].value != undefined) {
                fileMessage = ValidateUploadedFile();
            }


            if ($("#txtTagLine")[0].value == "" || $("#txtTagLine")[0].value == undefined) {
                status = 1;
            }

            if ($("#txtFoundedYear")[0].value == "" || $("#txtFoundedYear")[0].value == undefined) {
                status = 1;
            }

            if ($("#txtWorkEmail")[0].value == "" || $("#txtWorkEmail")[0].value == undefined) {
                status = 1;
            }
            else {
                var isValidWorkEmail = $.ValidateEmail($("#txtWorkEmail")[0].value);
                if (isValidWorkEmail) {
                    var isBusinessEmail = $.ValidateWorkEmailID($("#txtWorkEmail")[0].value);
                    if (!isBusinessEmail) {
                        $.DisplayMessage(true, "Please enter correct work email id.");
                        return false;
                    }
                }
                else {
                    $.DisplayMessage(true, "Email is not valid.");
                    return false;
                }
            }

            if ($("#ddlEmployees option:selected").val() == '0') {
                status = 1;
            }

            if ($("#ddlAvgHourlyRate option:selected").val() == '0') {
                status = 1;
            }

            if ($("#txtWebsite")[0].value == "" || $("#txtWebsite")[0].value == undefined) {
                status = 1;
            }

            if ($("#txtLinkedInProfile")[0].value == "" || $("#txtLinkedInProfile")[0].value == undefined) {
                status = 1;
            }

            if ($("#txtTwitterProfile")[0].value == "" || $("#txtTwitterProfile")[0].value == undefined) {
                status = 1;
            }

            if ($("#txtFacebookProfile")[0].value == "" || $("#txtFacebookProfile")[0].value == undefined) {
                status = 1;
            }

            if ($("#txtGooglePlusProfile")[0].value == "" || $("#txtGooglePlusProfile")[0].value == undefined) {
                status = 1;
            }

            if (($("#txtCompanySummary").Editor("getText") == "<br>" || $("#txtCompanySummary").Editor("getText") == undefined) && status == 0) {
                status = 1;
            }

            if (status != 1) {
                //Focus Area Section
                var primaryFocusPercentageTotal = 0;
                actualFocusAreaArray = [];
                for (var i = 0; i < focusAreaObject.length; i++) {
                    if (focusAreaObject[i].FocusType == "P") {
                        if ($("#txtFocus_" + focusAreaObject[i].FocusAreaID)[0].value != "") {
                            var actualFocusAreaObj = new Object();
                            actualFocusAreaObj.FocusAreaID = focusAreaObject[i].FocusAreaID;
                            actualFocusAreaObj.CompanyFocusID = $("#hdnCompanyFocusID_" + focusAreaObject[i].FocusAreaID)[0].value == "" ? 0 : $("#hdnCompanyFocusID_" + focusAreaObject[i].FocusAreaID)[0].value;
                            actualFocusAreaObj.FocusAreaPercentage = $("#txtFocus_" + focusAreaObject[i].FocusAreaID)[0].value;
                            actualFocusAreaObj.FocusType = focusAreaObject[i].FocusType;
                            actualFocusAreaObj.FocusAreaName = focusAreaObject[i].FocusAreaName;
                            actualFocusAreaObj.SubFocusAreaEntity = focusAreaObject[i].SubFocusAreaEntity;

                            primaryFocusPercentageTotal = primaryFocusPercentageTotal + parseFloat(actualFocusAreaObj.FocusAreaPercentage);
                            if (primaryFocusPercentageTotal > 100) {
                                $.DisplayMessage(true, "Company focus percentage cannot be greater than 100%");
                                return false;
                            } else {
                                $.DisplayMessage(false, "");
                                actualFocusAreaArray.push(actualFocusAreaObj);
                            }
                        }
                    } else {
                        var actualFocusAreaObj = new Object();
                        actualFocusAreaObj.FocusAreaID = focusAreaObject[i].FocusAreaID;
                        actualFocusAreaObj.CompanyFocusID = $("#hdnCompanyFocusID_" + focusAreaObject[i].FocusAreaID)[0].value == "" ? 0 : $("#hdnCompanyFocusID_" + focusAreaObject[i].FocusAreaID)[0].value;
                        actualFocusAreaObj.FocusAreaPercentage = 0;
                        actualFocusAreaObj.FocusType = focusAreaObject[i].FocusType;
                        actualFocusAreaObj.FocusAreaName = focusAreaObject[i].FocusAreaName;
                        actualFocusAreaObj.SubFocusAreaEntity = focusAreaObject[i].SubFocusAreaEntity;
                        actualFocusAreaArray.push(actualFocusAreaObj);
                    }
                }
                //

                //Sub-Focus Area Section
                for (var k = 0; k < actualFocusAreaArray.length; k++) {
                    var percentageTotal = 0;
                    if (actualFocusAreaArray[k].SubFocusAreaEntity.length > 0) {

                        for (var l = 0; l < actualFocusAreaArray[k].SubFocusAreaEntity.length; l++) {
                            if ($("#txtSubFocus_" + actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID)[0].value != "" && $("#txtSubFocus_" + actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID)[0].value != undefined) {
                                actualFocusAreaArray[k].SubFocusAreaEntity[l].CompanySubFocusID = $("#hdnCompanySubFocusAreaID_" + actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID)[0].value == "" ? 0 : $("#hdnCompanySubFocusAreaID_" + actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID)[0].value;

                                actualFocusAreaArray[k].SubFocusAreaEntity[l].CompanyFocusID = $("#hdnCompanyFocusID_" + actualFocusAreaArray[k].FocusAreaID)[0].value == "" ? 0 : $("#hdnCompanyFocusID_" + actualFocusAreaArray[k].FocusAreaID)[0].value;

                                actualFocusAreaArray[k].SubFocusAreaEntity[l].FocusAreaID = actualFocusAreaArray[k].FocusAreaID;

                                actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID = actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID;

                                actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaPercentage = $("#txtSubFocus_" + actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID)[0].value;

                                if (actualFocusAreaArray[k].FocusType == "P") {
                                    percentageTotal = percentageTotal + parseFloat(actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaPercentage);
                                    if (percentageTotal > 100) {
                                        $.DisplayMessage(true, actualFocusAreaArray[k].FocusAreaName + " focus percentage cannot be greater than 100%");
                                        return false;
                                    }
                                    else {
                                        $.DisplayMessage(false, "");
                                    }
                                }

                                if (actualFocusAreaArray[k].FocusType == "I") {
                                    percentageTotal = percentageTotal + parseFloat(actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaPercentage);
                                    if (percentageTotal > 100) {
                                        $.DisplayMessage(true, "Industry focus percentage cannot be greater than 100%");
                                        return false;
                                    } else {
                                        $.DisplayMessage(false, "");
                                    }
                                }

                                if (actualFocusAreaArray[k].FocusType == "C") {
                                    percentageTotal = percentageTotal + parseFloat(actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaPercentage);
                                    if (percentageTotal > 100) {
                                        $('#spnMessage').css('display', 'block');
                                        $('#spnMessage').html("Client focus percentage cannot be greater than 100%");
                                        $.DisplayMessage(true, "Client focus percentage cannot be greater than 100%");
                                        return false;
                                    } else {
                                        $.DisplayMessage(false, "");
                                    }
                                }

                            }
                        }
                    }
                }
                //

                //Branch Section
                companyBranchArray = []; var isHeadQuartersSelected = false;
                $('#dvLocationBranch [id*=dvLocationBranches_]').each(function (index, value) {
                    if ($('#txtBranch_' + index).val() == '' || $('#ddlCountry_' + index).val() == '0' || $('#ddlStates_' + index).val() == '0' || $('#txtstreet_' + index).val() == '' || $('#txtcity_' + index).val() == '' || $('#txtpostal_' + index).val() == '' || $('#txtphone_' + index).val() == '' || $('#txtEmail_' + index).val() == '') {
                        status = 1;
                        return;
                    }
                    else {

                        if ($('#ChkIsHeadQuarters_' + index)[0].checked) {
                            isHeadQuartersSelected = true;
                        }

                        if (!$.ValidateEmail($('#txtEmail_' + index).val())) {
                            $.DisplayMessage(true, "Email is not valid.");
                            return false;
                        }

                        var branchObject = new Object();
                        branchObject.BranchID = $('#hdnBranchID_' + index).val() == "" ? 0 : $('#hdnBranchID_' + index).val();
                        branchObject.BranchName = $('#txtBranch_' + index).val();
                        branchObject.CompanyID = isAddMode == true ? 0 : (($("#hdnUserCompanyID")[0].value == "" || $("#hdnUserCompanyID")[0].value == undefined) ? 0 : $("#hdnUserCompanyID")[0].value);
                        branchObject.CountryID = $('#ddlCountry_' + index).val();
                        branchObject.StateID = $('#ddlStates_' + index).val();
                        branchObject.Address = $('#txtstreet_' + index).val();
                        branchObject.City = $('#txtcity_' + index).val();
                        branchObject.PostalCode = $('#txtpostal_' + index).val();
                        branchObject.PhoneNumber = $('#txtphone_' + index).val();
                        branchObject.Email = $('#txtEmail_' + index).val();
                        branchObject.IsHeadQuarters = $('#ChkIsHeadQuarters_' + index)[0].checked;
                        companyBranchArray.push(branchObject);
                    }
                });
                //
            }

            if (status == 1) {
                $.DisplayMessage(true, "Fields marked with * are mandatory.");
                return false;
            } else if (fileMessage != '') {
                $.DisplayMessage(true, fileMessage);
                return false;
            } else if (actualFocusAreaArray.length == 0) {
                $.DisplayMessage(true, "Please fill the focus section.");
                return false;
            }
            else if (!isHeadQuartersSelected) {
                $.DisplayMessage(true, "Head Quarters is missing in the branch section.");
                return false;
            }
            else {
                $.DisplayMessage(false, "");
                return true;
            }
        } catch (e) {
            debugger;
        }
    }

    $("#btnCompanySave").click(function () {
        $.DisplayMessage(false, "");
        $('#divLoading').dialog({ modal: true, title: '', width: '115', height: '115', open: function () { $('.ui-widget-overlay').addClass('custom-overlay'); $(".ui-dialog-titlebar-close").hide(); $(".ui-widget-header").hide(); }, close: function () { $('.ui-widget-overlay').removeClass('custom-overlay'); $(".ui-dialog-titlebar-close").show(); $(".ui-widget-header").show(); } });
        if ($.SaveModeValidations()) {
            CompanyID = isAddMode == true ? 0 : (($("#hdnUserCompanyID")[0].value == "" || $("#hdnUserCompanyID")[0].value == undefined) ? 0 : $("#hdnUserCompanyID")[0].value);

            var companyProfileData = new Object();
            companyProfileData.CompanyID = CompanyID;
            companyProfileData.CompanyName = $("#txtCompanyName")[0].value;
            companyProfileData.LogoName = '';
            companyProfileData.TagLine = $("#txtTagLine")[0].value;
            companyProfileData.FoundedYear = $("#txtFoundedYear")[0].value;
            companyProfileData.WorkEmail = $('#txtWorkEmail')[0].value;
            companyProfileData.TotalEmployees = $("#ddlEmployees")[0].value;
            companyProfileData.AveragHourlyRate = $("#ddlAvgHourlyRate")[0].value;
            companyProfileData.URL = $("#txtWebsite")[0].value;
            companyProfileData.LinkedInProfileURL = $("#txtLinkedInProfile")[0].value;
            companyProfileData.TwitterProfileURL = $("#txtTwitterProfile")[0].value;
            companyProfileData.FacebookProfileURL = $("#txtFacebookProfile")[0].value;
            companyProfileData.GooglePlusProfileURL = $("#txtGooglePlusProfile")[0].value;
            companyProfileData.Summary = ($("#txtCompanySummary").Editor("getText"));
            companyProfileData.KeyClients = $("#txtKeyClients").Editor("getText") == "<br>" ? "" : ($("#txtKeyClients").Editor("getText"));
            companyProfileData.IsAdminUser = isAdmin;
            companyProfileData.CompanyFocus = [];

            for (var i = 0; i < actualFocusAreaArray.length; i++) {
                var focusAreaObj = new Object();
                focusAreaObj.CompanyFocusID = actualFocusAreaArray[i].CompanyFocusID;
                focusAreaObj.CompanyID = CompanyID;
                focusAreaObj.FocusAreaID = actualFocusAreaArray[i].FocusAreaID;
                focusAreaObj.FocusAreaPercentage = actualFocusAreaArray[i].FocusAreaPercentage;
                focusAreaObj.CompanySubFocus = [];

                for (var j = 0; j < actualFocusAreaArray[i].SubFocusAreaEntity.length; j++) {
                    if (actualFocusAreaArray[i].SubFocusAreaEntity[j].SubFocusAreaPercentage != "" && actualFocusAreaArray[i].SubFocusAreaEntity[j].SubFocusAreaPercentage != undefined) {
                        var subFocusAreaObj = new Object();
                        subFocusAreaObj.CompanySubFocusID = actualFocusAreaArray[i].SubFocusAreaEntity[j].CompanySubFocusID;
                        subFocusAreaObj.CompanyFocusID = focusAreaObj.CompanyFocusID;
                        subFocusAreaObj.SubFocusAreaID = actualFocusAreaArray[i].SubFocusAreaEntity[j].SubFocusAreaID;
                        subFocusAreaObj.SubFocusAreaPercentage = actualFocusAreaArray[i].SubFocusAreaEntity[j].SubFocusAreaPercentage;
                        focusAreaObj.CompanySubFocus.push(subFocusAreaObj);
                    }
                }

                companyProfileData.CompanyFocus.push(focusAreaObj);
            }

            companyProfileData.CompanyBranches = companyBranchArray;

            var form = $('#myForm')[0];
            var data = new FormData(form);
            data.append('UploadedFile', uploadedCompanyLogo)

            var myProfileData = JSON.stringify(companyProfileData);
            data.append('ProfileData', myProfileData);

            $.ajax({
                url: $.absoluteurl('/UserCompanyList/SaveCompanyData'),
                data: data,
                cache: false,
                datatype: 'json',
                type: 'POST',
                contentType: false,
                processData: false,
                success: function (response) {
                    debugger;
                    $('#divLoading').dialog('close'); $(".ui-dialog-titlebar-close").show();
                    if (response.IsSuccess) {
                        $("#divSuccessMessage").show();
                        $('#spnSuccessMessage').css('display', 'block');
                        if (isAdmin) {
                            //replace localhost with actial published URL.
                            window.location.href = window.location.origin + '/UserCompanyList/UserCompanyList';
                        }
                        else {
                            if (CompanyID == 0) {
                                $('#spnSuccessMessage').html("Thanks for creating a company at upvotes.co. Please verify your company by clicking on the link provided in your work email received from upvotes.co!!");
                            }
                            else {
                                $('#spnSuccessMessage').html("Saved Successfully & pending for Admin Approval!!");
                            }
                        }
                    } else {
                        $.DisplayMessage(true, "Failed to save.");
                    }
                },
                error: function (e) {
                    $('#spnMessage').css('display', 'block');
                    $.DisplayMessage(true, "Some error has occured. Failed to save. Please contact admin.");
                    $('#divLoading').dialog('close'); $(".ui-dialog-titlebar-close").show();
                }
            });
        }
        else {
            $('#divLoading').dialog('close'); $(".ui-dialog-titlebar-close").show();
        }
    });

    $.UpdateRejectionComments = function (companyID) {
        var companyRejectCommentsObj = new Object();
        companyRejectCommentsObj.CompanyID = $("#hdnUserCompanyID")[0].value;
        companyRejectCommentsObj.CompanyName = $("#txtCompanyName")[0].value;
        companyRejectCommentsObj.RejectComments = $("#txtRejectComments")[0].value;

        var companyRejectComments = JSON.stringify(companyRejectCommentsObj);

        $.ajax({
            url: $.absoluteurl('/UserCompanyList/UpdateRejectionComments'),
            data: { companyRejectComments: companyRejectComments },
            cache: false,
            datatype: 'json',
            type: 'POST',
            success: function (response) {    
                //replace localhost with actial published URL.
                window.location.href = window.location.origin + '/UserCompanyList/UserCompanyList';
            },
            error: function (e) {                
                $.DisplayMessage(true, "Some error occured");
            }
        });
    }

    $("#btnCompanyCancel").click(function () {
        if (isAdmin && ($("#txtRejectComments")[0].value == "" || $("#txtRejectComments")[0].value == undefined)) {
            $.DisplayMessage(true, "Please fill comment for rejection in profile section.")
        }
        else if (isAdmin && $("#txtRejectComments")[0].value != "" && $("#txtRejectComments")[0].value != undefined) {
            $.UpdateRejectionComments(CompanyID);
        }
        else {
            //replace localhost with actial published URL.
            window.location.href = window.location.origin + '/UserCompanyList/UserCompanyList';
        }
    });

    $('#btnAddBranch').click(function () {
        var idLastBranch = 0;
        var BranchNum = 0;
        if ($("#dvLocationBranch [id*=dvLocationBranches_]:last").attr('id') != undefined) {
            idLastBranch = $("#dvLocationBranch [id*=dvLocationBranches_]:last").attr('id');
            BranchNum = parseInt(idLastBranch.substr(idLastBranch.length - 2).replace('_', ''));
        }
        BranchNum++;
        if (BranchNum <= 4) {
            var duplicate = $("#dvLocationBranches_rNum").clone(true).find('input:text').val('').end();
            $(duplicate).find('[id*=_rNum]').each(function () {
                var oldID = $(this).attr('id');
                $(this).attr('id', oldID.replace('_rNum', '_' + BranchNum));
            });
            $(duplicate).attr("id", "dvLocationBranches_" + BranchNum).show();

            $(duplicate).find('#locationnumber').text('' + parseInt(BranchNum + 1));

            $("#dvLocationBranch").append(duplicate);

            $.ClearDropdown($('#ddlCountry_' + BranchNum));
            $('#ddlCountry_' + BranchNum).LoadOptions(countryList, 'CountryName', 'CountryID');
            $('#ddlCountry_' + BranchNum).bind('change', function () {
                $.LoadStatesOnBranchCountry(BranchNum);
            });
        }
    });

    $(".removeBranch").click(function () {
        var elementToBeDeleted = $(this).closest('[id*=dvLocationBranches_]');
        elementToBeDeleted.nextAll('[id*=dvLocationBranches_]').each(function () {
            var index = parseInt($(this).attr('id').substr($(this).length - 2).replace('_', ''));
            $(this).attr('id', $(this).attr('id').replace('' + index, '' + index - 1));
            $('#hdnBranchID_' + index).attr('id', $('#hdnBranchID_' + index).attr('id').replace('' + index, '' + index - 1));
            $('#txtBranch_' + index).attr('id', $('#txtBranch_' + index).attr('id').replace('' + index, '' + index - 1));
            $('#ddlCountry_' + index).attr('id', $('#ddlCountry_' + index).attr('id').replace('' + index, '' + index - 1));
            $('#txtstreet_' + index).attr('id', $('#txtstreet_' + index).attr('id').replace('' + index, '' + index - 1));
            $('#txtcity_' + index).attr('id', $('#txtcity_' + index).attr('id').replace('' + index, '' + index - 1));
            $('#ddlStates_' + index).attr('id', $('#ddlStates_' + index).attr('id').replace('' + index, '' + index - 1));
            $('#txtpostal_' + index).attr('id', $('#txtpostal_' + index).attr('id').replace('' + index, '' + index - 1));
            $('#txtphone_' + index).attr('id', $('#txtphone_' + index).attr('id').replace('' + index, '' + index - 1));
            $('#txtEmail_' + index).attr('id', $('#txtEmail_' + index).attr('id').replace('' + index, '' + index - 1));
            $('#ChkIsHeadQuarters_' + index).attr('id', $('#ChkIsHeadQuarters_' + index).attr('id').replace('' + index, '' + index - 1));
            $(this).find('#locationnumber').text('' + index - 1);
        });
        elementToBeDeleted.remove();

    });

    $.GetCompanyData = function (companyName) {
        isAddMode = false;
        $('#divLoading').dialog({ modal: true, title: '', width: '115', height: '115', open: function () { $('.ui-widget-overlay').addClass('custom-overlay'); $(".ui-dialog-titlebar-close").hide(); $(".ui-widget-header").hide(); }, close: function () { $('.ui-widget-overlay').removeClass('custom-overlay'); $(".ui-dialog-titlebar-close").show(); $(".ui-widget-header").show(); } });
        if (companyName != "" && companyName != null && companyName != undefined) {
            $.ajax({
                url: $.absoluteurl('/UserCompanyList/GetUserCompanyData?companyName=' + $.EncryptString(companyName)),
                datatype: 'json',
                content: "application/json; charset=utf-8",
                type: 'POST',
                success: function (data) {
                    if (data != null && data.companyData != null) {
                        if (data.isAdminUser) {
                            isAdmin = true;
                            $('#btnCompanySave')[0].value = "Approve";
                            $('#btnCompanyCancel')[0].value = "Reject";
                            $("#divRejectionComments").show();
                        }
                        else {
                            isAdmin = false;
                            $('#btnCompanySave')[0].value = "Save";
                            $('#btnCompanyCancel')[0].value = "Cancel";
                            $("#divRejectionComments").hide();
                        }

                        $("#hdnUserCompanyID")[0].value = data.companyData.CompanyID;
                        $('#txtCompanyName').val(data.companyData.CompanyName);
                        $('#txtTagLine').val(data.companyData.TagLine);
                        $('#txtFoundedYear').val(data.companyData.FoundedYear);
                        $('#txtLinkedInProfile').val(data.companyData.LinkedInProfileURL);
                        $('#txtTwitterProfile').val(data.companyData.TwitterProfileURL);
                        $('#txtFacebookProfile').val(data.companyData.FacebookProfileURL);
                        $('#txtGooglePlusProfile').val(data.companyData.GooglePlusProfileURL);
                        $('#txtWebsite').val(data.companyData.URL);
                        $('#txtWorkEmail').val(data.companyData.WorkEmail);
                        $("#txtRejectComments").val(data.companyData.Remarks);
                        document.getElementById('ddlEmployees').value = data.companyData.TotalEmployees;
                        document.getElementById('ddlAvgHourlyRate').value = data.companyData.AveragHourlyRate;
                        $("#txtCompanySummary").Editor("setText", (data.companyData.Summary));
                        $("#txtKeyClients").Editor("setText", (data.companyData.KeyClients));

                        for (var i = 0; i < data.companyData.CompanyFocus.length; i++) {
                            $("#hdnCompanyFocusID_" + data.companyData.CompanyFocus[i].FocusAreaID)[0].value = data.companyData.CompanyFocus[i].CompanyFocusID;
                            $("#txtFocus_" + data.companyData.CompanyFocus[i].FocusAreaID)[0].value = data.companyData.CompanyFocus[i].FocusAreaPercentage;
                            $("#txtFocus_" + data.companyData.CompanyFocus[i].FocusAreaID).blur();
                        }

                        for (var j = 0; j < data.companyData.CompanySubFocus.length; j++) {
                            $("#hdnCompanySubFocusAreaID_" + data.companyData.CompanySubFocus[j].SubFocusAreaID)[0].value = data.companyData.CompanySubFocus[j].CompanyFocusID;
                            $("#txtSubFocus_" + data.companyData.CompanySubFocus[j].SubFocusAreaID)[0].value = data.companyData.CompanySubFocus[j].SubFocusAreaPercentage;
                        }

                        for (var k = 0; k < data.companyData.IndustialCompanyFocus.length; k++) {
                            $("#hdnCompanySubFocusAreaID_" + data.companyData.IndustialCompanyFocus[k].FocusAreaID)[0].value = data.companyData.IndustialCompanyFocus[k].CompanyFocusID;
                            $("#txtSubFocus_" + data.companyData.IndustialCompanyFocus[k].FocusAreaID)[0].value = data.companyData.IndustialCompanyFocus[k].FocusAreaPercentage;
                        }

                        for (var l = 0; l < data.companyData.CompanyClientFocus.length; l++) {
                            $("#hdnCompanySubFocusAreaID_" + data.companyData.CompanyClientFocus[l].FocusAreaID)[0].value = data.companyData.CompanyClientFocus[l].CompanyFocusID;
                            $("#txtSubFocus_" + data.companyData.CompanyClientFocus[l].FocusAreaID)[0].value = data.companyData.CompanyClientFocus[l].FocusAreaPercentage;
                        }

                        for (var m = 0; m < data.companyData.CompanyBranches.length; m++) {
                            var locationsAdd = data.companyData.CompanyBranches.length - 1;
                            if (m < locationsAdd) {
                                $('#btnAddBranch').click();
                            }
                            $("#hdnBranchID_" + m)[0].value = data.companyData.CompanyBranches[m].BranchID;
                            $("#txtBranch_" + m)[0].value = data.companyData.CompanyBranches[m].BranchName;
                            $("#txtstreet_" + m)[0].value = data.companyData.CompanyBranches[m].Address;
                            $("#txtcity_" + m)[0].value = data.companyData.CompanyBranches[m].City;
                            $("#ddlCountry_" + m)[0].value = data.companyData.CompanyBranches[m].CountryID;
                            $("#ddlCountry_" + m).change();
                            $("#ddlStates_" + m)[0].value = data.companyData.CompanyBranches[m].StateID;
                            $("#txtpostal_" + m)[0].value = data.companyData.CompanyBranches[m].PostalCode;
                            $("#txtphone_" + m)[0].value = data.companyData.CompanyBranches[m].PhoneNumber;
                            $("#txtEmail_" + m)[0].value = data.companyData.CompanyBranches[m].Email;
                            $("#ChkIsHeadQuarters_" + m)[0].checked = data.companyData.CompanyBranches[m].IsHeadQuarters;
                        }

                        $('#divLoading').dialog('close'); $(".ui-dialog-titlebar-close").show();
                    }
                },
                error: function (e) {
                    debugger
                    $('#divLoading').dialog('close'); $(".ui-dialog-titlebar-close").show();
                }
            });
        }
        else {
            $('#divLoading').dialog('close'); $(".ui-dialog-titlebar-close").show();
        }
    }
});

function EditCompany(companyName) {
    debugger;
    if (companyName != "" && companyName != null && companyName != undefined) {
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/GetUserCompanyData?companyName=' + $.EncryptString(companyName)),
            datatype: 'json',
            content: "application/json; charset=utf-8",
            type: 'POST',
            success: function (data) {
                debugger
                if (data != null) {
                    $("#hdnUserCompanyID")[0].value = data.CompanyID;
                    $('#txtCompanyName').val(data.CompanyName);
                    $('#txtTagLine').val(data.TagLine);
                    $('#txtFoundedYear').val(data.FoundedYear);
                    $('#txtLinkedInProfile').val(data.LinkedInURL);
                    $('#txtTwitterProfile').val(data.TwitterURL);
                    $('#txtFacebookProfile').val(data.FacebookURL);
                    $('#txtGooglePlusProfile').val(data.GoogleURL);
                    $('#txtWebsite').val(data.URL);
                    document.getElementById('ddlEmployees').value = data.TotalEmployees;
                    document.getElementById('ddlAvgHourlyRate').value = data.AveragHourlyRate;
                    $("#txtCompanySummary").Editor("setText", data.Summary);
                    $("#txtKeyClients").Editor("setText", data.KeyClients);

                    for (var i = 0; i < data.CompanyFocus.length; i++) {
                        $("#hdnCompanyFocus_" + data.CompanyFocus[i].FocusAreaID)[0].value = data.CompanyFocus[i].CompanyFocusID;
                        $("#txtFocus_" + data.CompanyFocus[i].FocusAreaID)[0].value = data.CompanyFocus[i].FocusAreaPercentage;
                    }
                }
            },
            error: function (e) {
                debugger
            }
        });
    }
}

function CheckForUploadedFile(obj) {
    uploadedCompanyLogo = obj.files[0];
}


function ValidateUploadedFile() {
    if ($("#UplAttachment")[0].value == "") {
        return 'Please select a file';
    }

    var ExtensionString = "";
    var ExtensionArray = new Array();
    ExtensionArray.push(".jpg", ".jpeg", ".jpe", ".gif", ".png");
    var Name = $("#UplAttachment")[0].value;
    var extension = (Name.substring(Name.length, Name.lastIndexOf("."))).toLowerCase();
    var SupportFlag = false;
    for (i = 0; i < ExtensionArray.length; i++) {
        if (extension == ExtensionArray[i]) {
            SupportFlag = true;
            break;
        }
        ExtensionString = ExtensionString + ExtensionArray[i] + (i == 9 ? "\n" : "  ");
    }
    if (!SupportFlag) {
        $("#UplAttachment")[0].value = '';
        $("#UplAttachment").replaceWith($("#UplAttachment").clone(true));

        return 'Logo format is not supported. Supported formats are ' + "\n" + ExtensionString;
    }
    var UploadedFile = $("#UplAttachment")[0].value;
    var Files = $("#UplAttachment")[0].files
    var FileNames = "";

    for (var i = 0; i < Files.length; i++) {
        var Name = Files[i].name;
        var TempName = Name.substring(Name.indexOf("\\") == -1 ? 0 : Name.lastIndexOf("\\") + 1, Name.length);

        if (TempName.length > 256) {
            var FileNames = FileNames + TempName + ", ";
        }
    }
    if (FileNames != "") {
        FileNames = FileNames.substring(0, FileNames.lastIndexOf(","));
        $("#UplAttachment")[0].value = '';
        $("#UplAttachment").replaceWith($("#UplAttachment").clone(true));
        return 'Logo name should not exceed more than 100 characters ' + ' (' + FileNames + ')';
    }

    FileNames = "";
    var FilesSize = 0;
    for (var i = 0; i < Files.length; i++) {
        var Name = Files[i].name;
        FilesSize = FilesSize + Files[i].size;
    }

    if (FilesSize > 1048576) { //its in byte
        //Conver File Size to MB 
        FilesSize = parseFloat(FilesSize / 1048576).toFixed(2);
        $("#UplAttachment")[0].value = '';
        $("#UplAttachment").replaceWith($("#UplAttachment").clone(true));
        return 'Logo should be 1MB in size. Your file size is ' + FilesSize + 'MB';
    }

    return '';
}

function PercentageFormat(item, event) {
    var result = '';
    var num = item.value;
    num = num.toString().replace(/[^0-9]/g, '');

    if (num.length == 0)
        num = "";
    else if (num.length == 1)
        num = num;
    else if (num.length > 2 && num.charAt(0) == '0')
        num = num.substring(1, num.length);

    var subfocusdivID = $(item).attr('controlid');
    if (parseFloat(num) > 0) {
        item.value = num;
        $('#' + subfocusdivID).show();
    } else {
        item.value = num;
        $('#' + subfocusdivID).hide();
    }

}

function ToggleHeadQuarters(obj) {
    var ChkBoxes = document.getElementsByName('ChkHeadQuarters');
    for (var i = 0; i < ChkBoxes.length; i++) {
        if ($(ChkBoxes[i])[0].id == obj.id) {
            $(ChkBoxes[i])[0].checked = obj.checked;
        }
        else {
            $(ChkBoxes[i])[0].checked = false;
        }
    }
}