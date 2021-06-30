var CompanyID = 0;
var uploadedCompanyLogo;
var companyOwnedByObj = new Object();
companyOwnedByObj.CreatedBy = 0;
companyOwnedByObj.IsAdminUser = 0;
companyOwnedByObj.LoggedInUser = 0;

$(document).ready(function ()
{
    var isAddMode = true;
    var isAdmin = false;
    var countryList = '';
    //$("#divmylist").tabs();
    //$("#divCompanyTabs").tabs();
    if ($("#txtCompanySummary").length > 0)
    {
        $("#txtCompanySummary").Editor();
        $("#txtKeyClients").Editor();
    }
    //$('#ImgLoading').attr("src", $.absoluteurl('/images/ajax-loader.gif'));

    $(".numericOnly").keypress(function (e)
    {
        return $(this).IsValidNumber(e);
    });

    $("#txtCompanyName").keypress(function(e) {
        $("#error_sp_msg").remove();
        var k = e.keyCode,
            $return = ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
        if (!$return) {
            $("<span/>",
                {
                    "id": "error_sp_msg",
                    "html": "Special characters not allowed !!!!!",
                    "style": "color:red"
                }).insertAfter($(this));
            return false;
        }
    });

    $('.progressChange').change(function ()
    {
        var total = 0;
        $('.progressChange').each(function ()
        {
            total = total + (parseInt($(this).val() || 0));
        });
        $('#divServiceFocusProgress').css('width', total + '%');
        $('#spnServiceFocusPercentage').text(total + '%');
    });

    $('.subFocusProgressChange').change(function ()
    {
        var total = 0;
        var focusid = $(this).attr('focusid');
        $('.subFocusProgressChange').each(function ()
        {
            if (focusid == $(this).attr('focusid'))
            {
                total = total + (parseInt($(this).val() || 0));
            }
        });
        var progressid = "divSubFocusProgress_" + focusid;
        var spnPercentageid = "spnSubFocusPercentage_" + focusid;
        $('#' + progressid).css('width', total + '%');
        $('#' + spnPercentageid).text(total + '%');
    });

    $('.industryFocusProgressChange').change(function ()
    {
        var total = 0;
        $('.industryFocusProgressChange').each(function ()
        {
            total = total + (parseInt($(this).val() || 0));
        });
        $('#divIndustryProgress').css('width', total + '%');
        $('#spnIndustryFocusPercentage').text(total + '%');
    });

    $('.clientFocusProgressChange').change(function ()
    {
        var total = 0;
        $('.clientFocusProgressChange').each(function ()
        {
            total = total + (parseInt($(this).val() || 0));
        });
        $('#divClientProgress').css('width', total + '%');
        $('#spnClientFocusPercentage').text(total + '%');
    });

    $.GetCompanyOwnedDetails = function (obj)
    {
        companyOwnedByObj.CreatedBy = obj.CreatedBy;
        companyOwnedByObj.IsAdminUser = obj.IsAdminUser;
        companyOwnedByObj.LoggedInUser = obj.LoggedInUser;
    }

    $.LoadCountry = function ()
    {
        if (countryList !== undefined && countryList != null && countryList !== '' && countryList.length > 0)
        {
            $.ClearDropdown($('#ddlCountry_0'));
            $('#ddlCountry_0').LoadOptions(countryList, 'CountryName', 'CountryID');
        }

        $.ajax({
            url: $.absoluteurl('/UserCompanyList/GetCountry'),
            cache: false,
            async: false,
            datatype: 'json',
            type: 'GET',
            success: function (response)
            {
                $.ClearDropdown($('#ddlCountry_0'));
                $('#ddlCountry_0').LoadOptions(response.countryList, 'CountryName', 'CountryID');
                countryList = response.countryList;
            },
            error: function (e)
            {
                $('#spnMessage').css('display', 'block');
                $('#spnMessage').html("Some error has occured. Unable to get the countries. Please contact admin.");
            }
        });
    }

    $('#ddlCountry_0').change(function ()
    {
        var countryID = $('#ddlCountry_0')[0].value;
        if (countryID != 0)
        {
            $.LoadState(countryID, 0);
        }
    });

    $.LoadStatesOnBranchCountry = function (branchNum)
    {
        var countryID = $('#ddlCountry_' + branchNum)[0].value;
        if (countryID != 0)
        {
            $.LoadState(countryID, branchNum);
        }
    };

    $.LoadState = function (countryID, branchNum)
    {
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/GetStates?countryID=' + countryID),
            cache: false,
            async: false,
            datatype: 'json',
            type: 'GET',
            success: function (response)
            {
                $.ClearDropdown($('#ddlStates_' + branchNum));
                $('#ddlStates_' + branchNum).LoadOptions(response.statesList, 'StateName', 'StateID');
            },
            error: function (e)
            {
                $('#spnMessage').css('display', 'block');
                $('#spnMessage').html("Some error has occured. Unable to get the states. Please contact admin.");
            }
        });
    }

    $.ValidateEmail = function (sEmail)
    {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,6}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    $.ValidateWorkEmailID = function (sEmail)
    {
        var reg = /^([\w-\.]+@(?!gmail.com)(?!yahoo.com)(?!hotmail.com)(?!yahoo.co.in)(?!aol.com)(?!abc.com)(?!xyz.com)(?!pqr.com)(?!rediffmail.com)(?!live.com)(?!outlook.com)(?!me.com)(?!msn.com)(?!ymail.com)([\w-]+\.)+[\w-]{2,6})?$/;
        if (reg.test(sEmail))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    $.handleSpecialChar = function (value)
    {
        var reg = /^[a-zA-Z0-9\s]*$/;
        if (reg.test(value)) {
            return true;
        } else {
            return false;
        }
    };

    var focusAreaObject = '';
    var actualFocusAreaArray = [];
    var companyBranchArray = [];

    $.GetFocusAreaObject = function (obj)
    {
        focusAreaObject = obj;
    }

    $.DisplayMessage = function (isError, displayText, tabIndex)
    {
        if (isError)
        {
            $("#divFailureMessage").show();
            $('#spnMessage').css('display', 'block');
            $('#spnMessage').html(displayText);

            $("#divSuccessMessage").addClass('hide');
            $('#spnSuccessMessage').css('display', 'none');
            $('#spnSuccessMessage').html("");
            $('html, body').animate({ scrollTop: 0 }, 'slow');
            $(".nav-tabs a:eq(" + tabIndex + ")").tab('show');
            //$("#divCompanyTabs").tabs("option", "active", tabIndex);
        }
        else
        {
            $("#divFailureMessage").hide();
            $('#spnMessage').css('display', 'none');
            $('#spnMessage').html("");
        }
    }

    $.SaveModeValidations = function ()
    {

        try
        {
            var status = 0;
            var fileMessage = '';
            if ($("#txtCompanyName")[0].value === "" || $("#txtCompanyName")[0].value === undefined) {
                status = 1;
            } else {
                if (!$.handleSpecialChar($("#txtCompanyName")[0].value)) {
                    status = 3;
                }
            }

            if ($("#UplAttachment")[0].value !== "" && $("#UplAttachment")[0].value !== undefined)
            {
                fileMessage = ValidateUploadedFile();
            }

            if ($("#txtFoundedYear")[0].value === "" || $("#txtFoundedYear")[0].value === undefined)
            {
                status = 1;
            }

            if (companyOwnedByObj.IsAdminUser == true && companyOwnedByObj.CreatedBy == companyOwnedByObj.LoggedInUser)
            {
                if ($("#txtCompanyDomain")[0].value == "" || $("#txtCompanyDomain")[0].value == undefined)
                {
                    status = 1;
                }
            }
            else
            {
                if ($("#txtWorkEmail")[0].value === "" || $("#txtWorkEmail")[0].value === undefined)
                {
                    status = 1;
                }
                else
                {
                    var isValidWorkEmail = $.ValidateEmail($("#txtWorkEmail")[0].value);
                    if (isValidWorkEmail)
                    {
                        var isBusinessEmail = $.ValidateWorkEmailID($("#txtWorkEmail")[0].value);
                        if (!isBusinessEmail)
                        {
                            $.DisplayMessage(true, "Please enter correct work email id.", 0);
                            return false;
                        }
                    }
                    else
                    {
                        $.DisplayMessage(true, "Email is not valid.", 0);
                        return false;
                    }
                }
            }

            if ($("#ddlEmployees option:selected").val() == '0')
            {
                status = 1;
            }

            if ($("#ddlAvgHourlyRate option:selected").val() == '0')
            {
                status = 1;
            }

            if ($("#txtWebsite")[0].value == "" || $("#txtWebsite")[0].value == undefined)
            {
                status = 1;
            }

            //if (($("#txtCompanySummary")[0].value == "" || $("#txtCompanySummary")[0].value == undefined) && status == 0)
            if (($("#txtCompanySummary").Editor("getText") == "<br>" || $("#txtCompanySummary").Editor("getText") == undefined) && status == 0)            
            {
                status = 1;
            }

            if (status !== 1)
            {
                //Focus Area Section
                var primaryFocusPercentageTotal = 0;
                actualFocusAreaArray = [];
                for (var i = 0; i < focusAreaObject.length; i++)
                {
                    var actualFocusAreaObj;
                    if (focusAreaObject[i].FocusType === "P")
                    {
                        if ($("#txtFocus_" + focusAreaObject[i].FocusAreaID)[0].value != "")
                        {
                            actualFocusAreaObj = new Object();
                            actualFocusAreaObj.FocusAreaID = focusAreaObject[i].FocusAreaID;
                            actualFocusAreaObj.CompanyFocusID = $("#hdnCompanyFocusID_" + focusAreaObject[i].FocusAreaID)[0].value == "" ? 0 : $("#hdnCompanyFocusID_" + focusAreaObject[i].FocusAreaID)[0].value;
                            actualFocusAreaObj.FocusAreaPercentage = $("#txtFocus_" + focusAreaObject[i].FocusAreaID)[0].value;
                            actualFocusAreaObj.FocusType = focusAreaObject[i].FocusType;
                            actualFocusAreaObj.FocusAreaName = focusAreaObject[i].FocusAreaName;
                            actualFocusAreaObj.SubFocusAreaEntity = focusAreaObject[i].SubFocusAreaEntity;

                            primaryFocusPercentageTotal = primaryFocusPercentageTotal + parseFloat(actualFocusAreaObj.FocusAreaPercentage);
                            actualFocusAreaArray.push(actualFocusAreaObj);
                            //if (primaryFocusPercentageTotal > 100) {
                            //    $.DisplayMessage(true, "Company focus percentage must be equal to 100%");
                            //    return false;
                            //}                            
                            //else {
                            //    $.DisplayMessage(false, "");
                            //    actualFocusAreaArray.push(actualFocusAreaObj);
                            //}
                        }
                    } else
                    {
                        actualFocusAreaObj = new Object();
                        actualFocusAreaObj.FocusAreaID = focusAreaObject[i].FocusAreaID;
                        actualFocusAreaObj.CompanyFocusID = $("#hdnCompanyFocusID_" + focusAreaObject[i].FocusAreaID)[0].value == "" ? 0 : $("#hdnCompanyFocusID_" + focusAreaObject[i].FocusAreaID)[0].value;
                        actualFocusAreaObj.FocusAreaPercentage = 0;
                        actualFocusAreaObj.FocusType = focusAreaObject[i].FocusType;
                        actualFocusAreaObj.FocusAreaName = focusAreaObject[i].FocusAreaName;
                        actualFocusAreaObj.SubFocusAreaEntity = focusAreaObject[i].SubFocusAreaEntity;
                        actualFocusAreaArray.push(actualFocusAreaObj);
                    }
                }

                if (primaryFocusPercentageTotal != 100)
                {
                    $.DisplayMessage(true, "Company focus percentage must be equal to 100%", 1);
                    actualFocusAreaArray = [];
                    return false;
                }
                else
                {
                    $.DisplayMessage(false, "", -1);
                }
                //

                //Sub-Focus Area Section
                var subFocusPercentageTotal = 0; var industryFocusPercentageTotal = 0; var clientFocusPercentageTotal = 0; var subFocusCount = 0;
                for (var k = 0; k < actualFocusAreaArray.length; k++)
                {

                    if (actualFocusAreaArray[k].SubFocusAreaEntity.length > 0)
                    {
                        if (actualFocusAreaArray[k].FocusAreaPercentage > 0)
                        {
                            subFocusCount = subFocusCount + 1;
                        }
                        for (var l = 0; l < actualFocusAreaArray[k].SubFocusAreaEntity.length; l++)
                        {
                            if ($("#txtSubFocus_" + actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID)[0].value != "" && $("#txtSubFocus_" + actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID)[0].value != undefined)
                            {
                                actualFocusAreaArray[k].SubFocusAreaEntity[l].CompanySubFocusID = $("#hdnCompanySubFocusAreaID_" + actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID)[0].value == "" ? 0 : $("#hdnCompanySubFocusAreaID_" + actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID)[0].value;

                                actualFocusAreaArray[k].SubFocusAreaEntity[l].CompanyFocusID = $("#hdnCompanyFocusID_" + actualFocusAreaArray[k].FocusAreaID)[0].value == "" ? 0 : $("#hdnCompanyFocusID_" + actualFocusAreaArray[k].FocusAreaID)[0].value;

                                actualFocusAreaArray[k].SubFocusAreaEntity[l].FocusAreaID = actualFocusAreaArray[k].FocusAreaID;

                                actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID = actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID;

                                actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaPercentage = $("#txtSubFocus_" + actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaID)[0].value;

                                if (actualFocusAreaArray[k].FocusType === "P")
                                {
                                    subFocusPercentageTotal = subFocusPercentageTotal + parseFloat(actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaPercentage);
                                }

                                if (actualFocusAreaArray[k].FocusType === "I")
                                {
                                    industryFocusPercentageTotal = industryFocusPercentageTotal + parseFloat(actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaPercentage);
                                }

                                if (actualFocusAreaArray[k].FocusType === "C")
                                {
                                    clientFocusPercentageTotal = clientFocusPercentageTotal + parseFloat(actualFocusAreaArray[k].SubFocusAreaEntity[l].SubFocusAreaPercentage);
                                }

                            }
                        }
                    }
                }


                if (subFocusPercentageTotal !== 100 * subFocusCount)
                {
                    $.DisplayMessage(true, "Sub-focus percentage must be equal to 100%", 1);
                    return false;
                }
                else if (industryFocusPercentageTotal > 0 && industryFocusPercentageTotal < 100)
                {
                    $.DisplayMessage(true, "Industry focus percentage must be equal to 0 or 100%", 2);
                    return false;
                }
                else if (clientFocusPercentageTotal > 0 && clientFocusPercentageTotal < 100)
                {
                    $.DisplayMessage(true, "Client focus percentage must be equal to 0 or 100%", 3);
                    return false;
                }
                else
                {
                    $.DisplayMessage(false, "", -1);
                }
                //

                //Branch Section
                companyBranchArray = []; var isHeadQuartersSelected = false;
                $('#dvLocationBranch [id*=dvLocationBranches_]').each(function (index, value)
                {
                    if ($('#txtBranch_' + index).val() == '' || $('#ddlCountry_' + index).val() == '0' || $('#ddlStates_' + index).val() == '0' || $('#txtstreet_' + index).val() == '' || $('#txtcity_' + index).val() == '' || $('#txtpostal_' + index).val() == '' || $('#txtphone_' + index).val() == '' || $('#txtEmail_' + index).val() == '')
                    {
                        status = 2;
                        return;
                    }
                    else
                    {

                        if ($('#ChkIsHeadQuarters_' + index)[0].checked)
                        {
                            isHeadQuartersSelected = true;
                        }

                        if (!$.ValidateEmail($('#txtEmail_' + index).val()))
                        {
                            $.DisplayMessage(true, "Email is not valid.", 4);
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

            if (status == 1)
            {
                $.DisplayMessage(true, "Fields marked with * are mandatory.", 0);
                return false;
            } else if (status == 2)
            {
                $.DisplayMessage(true, "Fields marked with * are mandatory.", 4);
                return false;
            }
            else if (fileMessage != '')
            {
                $.DisplayMessage(true, fileMessage, 0);
                return false;
            } else if (actualFocusAreaArray.length == 0)
            {
                $.DisplayMessage(true, "Please fill the focus section.", 1);
                return false;
            }
            else if (!isHeadQuartersSelected)
            {
                $.DisplayMessage(true, "Head Quarters is missing in the branch section.", 4);
                return false;
            }
            else if (status === 3) {
                $.DisplayMessage(true, "Company Name must contain only alphanumeric and spaces.", 0);
                return false;
            }
            else
            {
                $.DisplayMessage(false, "", -1);
                return true;
            }
        } catch (e)
        {

        }
    }

    $('#addAdminServiceProfile').click(function ()
    {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/CreateNewCompanyAdmin'),
            type: "POST",
            success: function (response)
            {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });

    $('.btnEditCompany').click(function ()
    {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/GetUserCompanyDetail'),
            data: { companyName: $(this).attr('compname') },
            type: "POST",
            success: function (response)
            {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }
        });
    });

    $('.btnDeleteCompany').click(function ()
    {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/DeleteCompany'),
            data: { companyId: $(this).attr('compid') },
            type: "POST",
            success: function (response)
            {
                debugger;
                $('#ajax_loaderDashboard').hide();
                if (response.IsSuccess)
                {
                    $('#spnSuccessMessage')
                        .html(
                            "Your company profile has been deleted.");
                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                }
            },
            error: function (a, b, c)
            {
                debugger;
            }
        });
    });


    $("#btnCompanySave").click(function ()
    {
        debugger;
        $.DisplayMessage(false, "", -1);
        $('#ajax_loaderDashboard').show();
        //$('#divLoading').dialog({ modal: true, title: '', width: '115', height: '115', open: function () { $('.ui-widget-overlay').addClass('custom-overlay'); $(".ui-dialog-titlebar-close").hide(); $(".ui-widget-header").hide(); }, close: function () { $('.ui-widget-overlay').removeClass('custom-overlay'); $(".ui-dialog-titlebar-close").show(); $(".ui-widget-header").show(); } });
        if ($.SaveModeValidations())
        {
            CompanyID = isAddMode == true ? 0 : (($("#hdnUserCompanyID")[0].value == "" || $("#hdnUserCompanyID")[0].value == undefined) ? 0 : $("#hdnUserCompanyID")[0].value);

            var companyProfileData = new Object();
            companyProfileData.CompanyID = CompanyID;
            companyProfileData.CompanyName = $("#txtCompanyName")[0].value;
            companyProfileData.LogoName = '';
            companyProfileData.TagLine = $("#txtTagLine")[0].value;
            companyProfileData.FoundedYear = $("#txtFoundedYear")[0].value;
            if (companyOwnedByObj.IsAdminUser == true && companyOwnedByObj.CreatedBy == companyOwnedByObj.LoggedInUser)
            {
                companyProfileData.CompanyDomain = $("#txtCompanyDomain")[0].value;
            }
            else
            {
                companyProfileData.WorkEmail = $('#txtWorkEmail')[0].value;
            }
            companyProfileData.TotalEmployees = $("#ddlEmployees")[0].value;
            companyProfileData.AveragHourlyRate = $("#ddlAvgHourlyRate")[0].value;
            companyProfileData.URL = $("#txtWebsite")[0].value;
            companyProfileData.LinkedInProfileURL = $("#txtLinkedInProfile")[0].value;
            companyProfileData.TwitterProfileURL = $("#txtTwitterProfile")[0].value;
            companyProfileData.FacebookProfileURL = $("#txtFacebookProfile")[0].value;
            companyProfileData.GooglePlusProfileURL = $("#txtGooglePlusProfile")[0].value;
            companyProfileData.Summary = encodeURI($("#txtCompanySummary").Editor("getText")); //$("#txtCompanySummary")[0].value;
            companyProfileData.KeyClients = encodeURI($("#txtKeyClients").Editor("getText") == "<br>" ? "" : ($("#txtKeyClients").Editor("getText")));//$("#txtKeyClients")[0].value;
            companyProfileData.IsAdminUser = companyOwnedByObj.IsAdminUser;
            if (CompanyID == 0)
            {
                companyProfileData.CreatedBy = companyOwnedByObj.LoggedInUser;
            }

            companyProfileData.CompanyFocus = [];

            for (var i = 0; i < actualFocusAreaArray.length; i++)
            {
                var focusAreaObj = new Object();
                focusAreaObj.CompanyFocusID = actualFocusAreaArray[i].CompanyFocusID;
                focusAreaObj.CompanyID = CompanyID;
                focusAreaObj.FocusAreaID = actualFocusAreaArray[i].FocusAreaID;
                focusAreaObj.FocusAreaPercentage = actualFocusAreaArray[i].FocusAreaPercentage;
                focusAreaObj.CompanySubFocus = [];

                for (var j = 0; j < actualFocusAreaArray[i].SubFocusAreaEntity.length; j++)
                {
                    if (actualFocusAreaArray[i].SubFocusAreaEntity[j].SubFocusAreaPercentage != "" && actualFocusAreaArray[i].SubFocusAreaEntity[j].SubFocusAreaPercentage != undefined)
                    {
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
                success: function (response)
                {
                    //$('#divLoading').dialog('close'); $(".ui-dialog-titlebar-close").show();
                    $('#ajax_loaderDashboard').hide();
                    if (response.IsSuccess)
                    {
                        $("#divSuccessMessage").removeClass('hide');
                        $('#spnSuccessMessage').css('display', 'block');
                        $("#addServiceData").addClass('hide');
                        if (isAdmin)
                        {
                            $.ajax({
                                url: $.absoluteurl('/UserCompanyList/UserCompany'),
                                data: { companyName: "" },
                                type: "POST",
                                success: function (response)
                                {
                                    $('#ajax_loaderDashboard').hide();
                                    $('#DetailsContent').html("");
                                    $('#DetailsContent').html(response);
                                }
                            });
                        }
                        else
                        {
                            if (CompanyID == 0)
                            {
                                $('#spnSuccessMessage').html("Thanks for creating a company at upvotes.co. Please verify your company by clicking on the link provided in your work email received from upvotes.co!!");
                                $('html, body').animate({ scrollTop: 0 }, 'slow');
                            }
                            else
                            {
                                $('#spnSuccessMessage').html("Thank you for submitting your company in Upvotes, we get back you shortly.");
                                $('html, body').animate({ scrollTop: 0 }, 'slow');
                            }
                        }
                    } else
                    {
                        $.DisplayMessage(true, "Failed to save.", 0);
                    }
                },
                error: function (e)
                {
                    $('#spnMessage').css('display', 'block');
                    $.DisplayMessage(true, "Some error has occured. Failed to save. Please contact admin.", 0);
                    //$('#divLoading').dialog('close'); $(".ui-dialog-titlebar-close").show();
                    $('#ajax_loaderDashboard').hide();
                }
            });
        }
        else
        {
            $('#ajax_loaderDashboard').hide();
            //$('#divLoading').dialog('close'); $(".ui-dialog-titlebar-close").show();
        }
    });

    $.UpdateRejectionComments = function (companyID)
    {
        var companyRejectCommentsObj = new Object();
        companyRejectCommentsObj.CompanyID = $("#hdnUserCompanyID")[0].value;
        companyRejectCommentsObj.CompanyName = $("#txtCompanyName")[0].value;
        companyRejectCommentsObj.RejectComments = $("#txtRejectComments")[0].value;

        var companyRejectComments = JSON.stringify(companyRejectCommentsObj);
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/UpdateRejectionComments'),
            data: { companyRejectComments: companyRejectComments },
            cache: false,
            datatype: 'json',
            type: 'POST',
            success: function (response)
            {
                $.ajax({
                    url: $.absoluteurl('/UserCompanyList/UserCompany'),
                    data: { companyName: "" },
                    type: "POST",
                    success: function (response)
                    {
                        $('#ajax_loaderDashboard').hide();
                        $('#DetailsContent').html("");
                        $('#DetailsContent').html(response);
                    }

                });
            },
            error: function (e)
            {
                $('#ajax_loaderDashboard').hide();
                $.DisplayMessage(true, "Some error occured", 0);
            }
        });
    }

    $(".btnCompanyCancel").click(function ()
    {
        if (isAdmin && ($("#txtRejectComments")[0].value == "" || $("#txtRejectComments")[0].value == undefined))
        {
            $.DisplayMessage(true, "Please fill comment for rejection in profile section.", 0)
        }
        else if (isAdmin && $("#txtRejectComments")[0].value != "" && $("#txtRejectComments")[0].value != undefined)
        {
            $.UpdateRejectionComments(CompanyID);
        }
        else
        {
            $('#ajax_loaderDashboard').show();
            $.ajax({
                url: $.absoluteurl('/UserCompanyList/UserCompany'),
                data: { companyName: "" },
                type: "POST",
                success: function (response)
                {
                    $('#ajax_loaderDashboard').hide();
                    $('#DetailsContent').html("");
                    $('#DetailsContent').html(response);
                }

            });
        }
    });

    $('#btnAddBranch').click(function ()
    {

        var idLastBranch = 0;
        var BranchNum = 0;
        if ($("#dvLocationBranch [id*=dvLocationBranches_]:last").attr('id') != undefined)
        {
            idLastBranch = $("#dvLocationBranch [id*=dvLocationBranches_]:last").attr('id');
            BranchNum = parseInt(idLastBranch.substr(idLastBranch.length - 2).replace('_', ''));
        }
        BranchNum++;
        if (BranchNum <= 4)
        {
            var duplicate = $("#dvLocationBranches_rNum").clone(true).find('input:text').val('').end();
            $(duplicate).find('[id*=_rNum]').each(function ()
            {
                var oldID = $(this).attr('id');
                $(this).attr('id', oldID.replace('_rNum', '_' + BranchNum));
            });

            $(duplicate).find('label[for="ChkIsHeadQuarters_rNum"]').each(function ()
            {
                var checkboxid = $(this).attr('for');
                $(this).attr('for', checkboxid.replace('_rNum', '_' + BranchNum));
            });

            $(duplicate).attr("id", "dvLocationBranches_" + BranchNum).removeClass('hide');

            $(duplicate).find('#locationnumber').text('' + parseInt(BranchNum + 1));

            $("#dvLocationBranch").append(duplicate);

            $.ClearDropdown($('#ddlCountry_' + BranchNum));
            $('#ddlCountry_' + BranchNum).LoadOptions(countryList, 'CountryName', 'CountryID');
            $('#ddlCountry_' + BranchNum).bind('change', function ()
            {
                $.LoadStatesOnBranchCountry(BranchNum);
            });
        }
    });

    $(".removeBranch").click(function ()
    {
        var elementToBeDeleted = $(this).closest('[id*=dvLocationBranches_]');
        elementToBeDeleted.nextAll('[id*=dvLocationBranches_]').each(function ()
        {
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

    $.GetCompanyData = function (companyName)
    {
        isAddMode = false;
        //$('#divLoading').dialog({ modal: true, title: '', width: '115', height: '115', open: function () { $('.ui-widget-overlay').addClass('custom-overlay'); $(".ui-dialog-titlebar-close").hide(); $(".ui-widget-header").hide(); }, close: function () { $('.ui-widget-overlay').removeClass('custom-overlay'); $(".ui-dialog-titlebar-close").show(); $(".ui-widget-header").show(); } });
        $('#ajax_loaderDashboard').show();
        if (companyName !== "" && companyName != null && companyName !== undefined)
        {
            $.ajax({
                url: $.absoluteurl('/UserCompanyList/GetUserCompanyData?companyName=' + $.EncryptString(companyName)),
                datatype: 'json',
                content: "application/json; charset=utf-8",
                type: 'POST',
                success: function (data)
                {
                    if (data != null && data.companyData != null)
                    {
                        if (data.isAdminUser)
                        {
                            isAdmin = true;
                            $('#btnCompanySave').text("Approve");
                            $('.btnCompanyCancel').text("Reject");
                            $("#divRejectionComments").show();
                        }
                        else
                        {
                            isAdmin = false;
                            $('#btnCompanySave').text("Save");
                            $('.btnCompanyCancel').text("Cancel");
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
                        $("#txtCompanyDomain").val(data.companyData.CompanyDomain);
                        $("#txtRejectComments").val(data.companyData.Remarks);
                        $("#ddlEmployees").val('0');
                        $("#ddlAvgHourlyRate").val('0');
                        if ($("#ddlEmployees option[value='" + data.companyData.TotalEmployees + "']").length > 0) {
                            $("#ddlEmployees").val(data.companyData.TotalEmployees);
                        }

                        if ($("#ddlAvgHourlyRate option[value='" + data.companyData.AveragHourlyRate + "']").length > 0) {
                            $("#ddlAvgHourlyRate").val(data.companyData.AveragHourlyRate);
                        }
                        //document.getElementById('ddlEmployees').value = data.companyData.TotalEmployees;
                        //document.getElementById('ddlAvgHourlyRate').value = data.companyData.AveragHourlyRate;
                        $("#txtCompanySummary").Editor("setText", (data.companyData.Summary + '<br/>' + data.companyData.Summary1 + '<br/>' + data.companyData.Summary2 + '<br/>' + data.companyData.Summary3));
                        $("#txtKeyClients").Editor("setText", (data.companyData.KeyClients));
                        if (data.companyData.LogoName != "")
                        {
                            $('#txtLogoName').text(data.companyData.LogoName);
                            $('#imgpreview').show();
                            $('#imgpreview').attr('src', '../images/CompanyLogos/' + data.companyData.LogoName);
                        }
                        for (var i = 0; i < data.companyData.CompanyFocus.length; i++)
                        {
                            $("#hdnCompanyFocusID_" + data.companyData.CompanyFocus[i].FocusAreaID)[0].value = data.companyData.CompanyFocus[i].CompanyFocusID;
                            $("#txtFocus_" + data.companyData.CompanyFocus[i].FocusAreaID)[0].value = data.companyData.CompanyFocus[i].FocusAreaPercentage;
                            $("#txtFocus_" + data.companyData.CompanyFocus[i].FocusAreaID).blur();
                        }

                        for (var j = 0; j < data.companyData.CompanySubFocus.length; j++)
                        {
                            $("#hdnCompanySubFocusAreaID_" + data.companyData.CompanySubFocus[j].SubFocusAreaID)[0].value = data.companyData.CompanySubFocus[j].CompanyFocusID;
                            $("#txtSubFocus_" + data.companyData.CompanySubFocus[j].SubFocusAreaID)[0].value = data.companyData.CompanySubFocus[j].SubFocusAreaPercentage;
                        }

                        for (var k = 0; k < data.companyData.IndustialCompanyFocus.length; k++)
                        {
                            $("#hdnCompanySubFocusAreaID_" + data.companyData.IndustialCompanyFocus[k].FocusAreaID)[0].value = data.companyData.IndustialCompanyFocus[k].CompanyFocusID;
                            $("#txtSubFocus_" + data.companyData.IndustialCompanyFocus[k].FocusAreaID)[0].value = data.companyData.IndustialCompanyFocus[k].FocusAreaPercentage;
                        }

                        for (var l = 0; l < data.companyData.CompanyClientFocus.length; l++)
                        {
                            $("#hdnCompanySubFocusAreaID_" + data.companyData.CompanyClientFocus[l].FocusAreaID)[0].value = data.companyData.CompanyClientFocus[l].CompanyFocusID;
                            $("#txtSubFocus_" + data.companyData.CompanyClientFocus[l].FocusAreaID)[0].value = data.companyData.CompanyClientFocus[l].FocusAreaPercentage;
                        }

                        for (var m = 0; m < data.companyData.CompanyBranches.length; m++)
                        {
                            var locationsAdd = data.companyData.CompanyBranches.length - 1;
                            if (m < locationsAdd)
                            {
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
                        $('.progressChange').change();
                        $('.subFocusProgressChange').change();
                        $('.industryFocusProgressChange').change();
                        $('.clientFocusProgressChange').change();

                        //$('#divLoading').dialog('close'); $(".ui-dialog-titlebar-close").show();
                        $('#ajax_loaderDashboard').hide();
                    }
                },
                error: function (e)
                {
                    //$('#divLoading').dialog('close'); $(".ui-dialog-titlebar-close").show();
                    $('#ajax_loaderDashboard').hide();
                }
            });
        }
        else
        {
            //$('#divLoading').dialog('close'); $(".ui-dialog-titlebar-close").show();
            $('#ajax_loaderDashboard').hide();
        }
    }

    $(".ClaimApprove").click(function ()
    {
        if (confirm('Are you sure want to approve this company?'))
        {
            var claimlistingid = $(this).attr('claimlistingID');
            var companyid = $(this).attr('companyID');
            var email = $(this).attr('email');
            var compName = $(this).attr('companyName');
            var type = $(this).attr('category');
            $('#ajax_loaderDashboard').show();
            $.ajax({
                url: $.absoluteurl('/UserCompanyList/AdminClaimApprove'),
                data: { claimlistingID: claimlistingid, companyID: companyid, Rejectioncomment: "", Email: email, CompanyName: compName, Type: type },
                type: 'POST',
                success: function (response)
                {
                    if (response == "claimed")
                    {
                        $('#ajax_loaderDashboard').hide();
                        $('#trclaimListing_' + claimlistingid).remove();
                    }
                    else
                    {
                        $('#ajax_loaderDashboard').hide();
                        alert('error occured while approving company');
                    }
                },
                error: function (e)
                {
                    $('#ajax_loaderDashboard').hide();
                    alert('error occured while approving company');
                }
            });
        }
    });

    $(".claimReject").click(function ()
    {
        if (confirm('Are you sure want to reject this company?'))
        {
            var claimlistingid = $(this).attr('claimlistingID');
            var companyid = $(this).attr('companyID');
            var rejectioncomment = $('#txtRejectionComment_' + claimlistingid).val();
            var email = $(this).attr('email');
            var compName = $(this).attr('companyName');
            var type = $(this).attr('category');
            $('#txtRejectionComment_' + claimlistingid).css('border', '');
            if (rejectioncomment.trim() != "")
            {
                $('#ajax_loaderDashboard').show();
                $.ajax({
                    url: $.absoluteurl('/UserCompanyList/AdminClaimReject'),
                    data: { claimlistingID: claimlistingid, companyID: companyid, Rejectioncomment: rejectioncomment, Email: email, CompanyName: compName, Type: type },
                    type: 'POST',
                    success: function (response)
                    {
                        if (response == "Rejected")
                        {
                            $('#ajax_loaderDashboard').hide();
                            $('#trclaimListing_' + claimlistingid).remove();
                        }
                        else
                        {
                            $('#ajax_loaderDashboard').hide();
                            alert('error occured while approving company');
                        }
                    },
                    error: function (e)
                    {
                        $('#ajax_loaderDashboard').hide();
                        alert('error occured while rejecting company');
                    }
                });
            }
            else
            {
                $('#txtRejectionComment_' + claimlistingid).css('border', '1px solid red');
            }
        }
    });

    $("#addServiceProfile").click(function ()
    {
        $("#emptyServiceData").addClass("hide");
        $("#addServiceData").removeClass("hide");
    });

    $('.btnNext').click(function ()
    {
        $('.nav-tabs > .active').next('a').trigger('click');
        var scrollPos = $("#addServiceProfileSection").offset().top;
        $(window).scrollTop(scrollPos);
    });

    $("#btnCompanyList").click(function ()
    {
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/UserCompany'),
            data: { companyName: "" },
            type: "POST",
            success: function (response)
            {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });
});

function EditCompany(companyName)
{

    if (companyName != "" && companyName != null && companyName != undefined)
    {
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/GetUserCompanyData?companyName=' + $.EncryptString(companyName)),
            datatype: 'json',
            content: "application/json; charset=utf-8",
            type: 'POST',
            success: function (data)
            {
                if (data != null)
                {
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
                    //$('#txtCompanySummary').val(data.Summary);
                    //$('#txtKeyClients').val(data.KeyClients);
                    for (var i = 0; i < data.CompanyFocus.length; i++)
                    {
                        $("#hdnCompanyFocus_" + data.CompanyFocus[i].FocusAreaID)[0].value = data.CompanyFocus[i].CompanyFocusID;
                        $("#txtFocus_" + data.CompanyFocus[i].FocusAreaID)[0].value = data.CompanyFocus[i].FocusAreaPercentage;
                    }
                }
            },
            error: function (e)
            {

            }
        });
    }
}

function CheckForUploadedFile(obj)
{
    uploadedCompanyLogo = obj.files[0];
    if (obj.files && obj.files[0])
    {
        var reader = new FileReader();

        reader.onload = function (e)
        {
            $('#imgpreview').attr('src', e.target.result);
            $('#imgpreview').show();
            $('#txtLogoName').text(uploadedCompanyLogo.name);
        }

        reader.readAsDataURL(obj.files[0]);
    }
}


function ValidateUploadedFile()
{

    if ($("#UplAttachment")[0].value == "")
    {
        return 'Please select a file';
    }

    var ExtensionString = "";
    var ExtensionArray = new Array();
    ExtensionArray.push(".jpg", ".jpeg", ".jpe", ".gif", ".png");
    var Name = $("#UplAttachment")[0].value;
    var extension = (Name.substring(Name.length, Name.lastIndexOf("."))).toLowerCase();
    var SupportFlag = false;
    for (i = 0; i < ExtensionArray.length; i++)
    {
        if (extension == ExtensionArray[i])
        {
            SupportFlag = true;
            break;
        }
        ExtensionString = ExtensionString + ExtensionArray[i] + (i == 9 ? "\n" : "  ");
    }
    if (!SupportFlag)
    {
        $("#UplAttachment")[0].value = '';
        $("#UplAttachment").replaceWith($("#UplAttachment").clone(true));

        return 'Logo format is not supported. Supported formats are ' + "\n" + ExtensionString;
    }
    var UploadedFile = $("#UplAttachment")[0].value;
    var Files = $("#UplAttachment")[0].files
    var FileNames = "";

    for (var i = 0; i < Files.length; i++)
    {
        var Name = Files[i].name;
        var TempName = Name.substring(Name.indexOf("\\") == -1 ? 0 : Name.lastIndexOf("\\") + 1, Name.length);

        if (TempName.length > 256)
        {
            var FileNames = FileNames + TempName + ", ";
        }
    }
    if (FileNames != "")
    {
        FileNames = FileNames.substring(0, FileNames.lastIndexOf(","));
        $("#UplAttachment")[0].value = '';
        $("#UplAttachment").replaceWith($("#UplAttachment").clone(true));
        return 'Logo name should not exceed more than 100 characters ' + ' (' + FileNames + ')';
    }

    FileNames = "";
    var FilesSize = 0;
    for (var i = 0; i < Files.length; i++)
    {
        var Name = Files[i].name;
        FilesSize = FilesSize + Files[i].size;
    }

    if (FilesSize > 1048576)
    { //its in byte
        //Conver File Size to MB 
        FilesSize = parseFloat(FilesSize / 1048576).toFixed(2);
        $("#UplAttachment")[0].value = '';
        $("#UplAttachment").replaceWith($("#UplAttachment").clone(true));
        return 'Logo should be 1MB in size. Your file size is ' + FilesSize + 'MB';
    }

    return '';
}

function PercentageFormat(item, event)
{
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
    if (parseFloat(num) > 0)
    {
        item.value = num;
        $('#' + subfocusdivID).show();
    } else
    {
        item.value = num;
        $('#' + subfocusdivID).hide();
    }

}

function ToggleHeadQuarters(obj)
{
    var ChkBoxes = document.getElementsByName('ChkHeadQuarters');
    for (var i = 0; i < ChkBoxes.length; i++)
    {
        if ($(ChkBoxes[i])[0].id == obj.id)
        {
            $(ChkBoxes[i])[0].checked = obj.checked;
        }
        else
        {
            $(ChkBoxes[i])[0].checked = false;
        }
    }
}