var uploadedSoftwareLogo;
$(document).ready(function ()
{
    $(".numericOnly").keypress(function (e)
    {
        return $(this).IsValidNumber(e);
    });

    if ($("#txtSummary").length > 0)
    {
        $("#txtSummary").Editor();
    }

    $('#addAdminSoftwareProfile').click(function ()
    {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/CreateNewSoftwareAdmin'),
            type: "POST",
            success: function (response)
            {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
                $("#addServiceProfile").click();
            }

        });
    });


    $("#addServiceProfile").click(function ()
    {
        $("#emptySoftwareData").addClass("hide");
        $("#addSoftwareData").removeClass("hide");
    });

    $('.btnEditSoftware').click(function ()
    {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/GetUserSoftwareDetail'),
            data: { softwareName: $(this).attr('softwarename') },
            type: "POST",
            success: function (response)
            {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
                $("#addServiceProfile").click();
            }

        });
    });

    $('#btnSoftwareList').click(function ()
    {
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/UserSoftware'),
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

    $.BindSoftwareData = function (isAdmin, softwareId, logoName)
    {        
        if (isAdmin && softwareId !== "0")
        {
            $("#btnSoftwraeCancel").show();
            $('#btnSaveSoftwareProfile').text("Approve");
            $('#btnSoftwraeCancel').text("Reject");
            $("#divRejectionComments").show();
        } else
        {
            $("#btnSoftwraeCancel").hide();
            $('#btnSaveSoftwareProfile').text("Save");
            $('#btnSoftwraeCancel').text("Cancel");
            $("#divRejectionComments").hide();
        }

        if (logoName !== "")
        {
            $('#txtLogoName').text(logoName);
            $('#imgpreview').show();
            $('#imgpreview').attr('src', '../images/SoftwareLogos/' + logoName);
        }

        $('#hdnIsAdmin')[0].value = isAdmin;
        //var categoryIds = softwareViewModelObj.SoftwareList[0].SoftwareCatagoryIds.split("_");                     
        //for (var i = 0; i < categoryIds.length; i++) {
        //    $('#ddlSoftwareCategory')[0][categoryIds[i]].selected = true;
        //    $('.select-category').SumoSelect();
        //}        
    };

    $.DisplayMessage = function (isError, displayText)
    {        
        if (isError)
        {
            $("#divFailureMessage").removeClass('hide');
            $('#spnMessage').css('display', 'block');
            $('#spnMessage').html(displayText);

            $("#successServiceData").addClass('hide');
            $('#spnSuccessMessage').css('display', 'none');
            $('#spnSuccessMessage').html("");
            $('html, body').animate({ scrollTop: 0 }, 'slow');
        }
        else
        {
            $("#divFailureMessage").addClass('hide');
            $('#spnMessage').css('display', 'none');
            $('#spnMessage').html("");
        }
    }

    $.handleSpecialChar = function (value)
    {
        var reg = /^[a-zA-Z0-9\s]*$/;
        if (reg.test(value))
        {
            return true;
        } else
        {
            return false;
        }
    };

    $.SaveModeValidations = function ()
    {
        try
        {
            var status = 0;
            var fileMessage = '';
            if ($("#txtSoftwareName")[0].value === "" || $("#txtSoftwareName")[0].value === undefined) {
                status = 1;
            } else {
                if (!$.handleSpecialChar($("#txtSoftwareName")[0].value))
                {
                    status = 3;
                }
            }

            if ($("#UplAttachment")[0].value !== "" && $("#UplAttachment")[0].value !== undefined)
            {
                fileMessage = ValidateUploadedFile();
            }

            var softwareCatagoriesArray = [];
            $('.select-category option:selected').each(function (i)
            {
                softwareCatagoriesArray.push($(this).val());
            });

            if (softwareCatagoriesArray.length === 0)
            {
                status = 1;
            }

            if ($("#txtPriceRange")[0].value === "" || $("#txtPriceRange")[0].value === undefined)
            {
                status = 1;
            }

            if ($("#txtSoftwareTrail")[0].value === "" || $("#txtSoftwareTrail")[0].value === undefined)
            {
                status = 1;
            }

            if ($("#txtSoftwareWebSiteUrl")[0].value === "" || $("#txtSoftwareWebSiteUrl")[0].value === undefined)
            {
                status = 1;
            }

            if ($("#txtFoundedYear")[0].value === "" || $("#txtFoundedYear")[0].value === undefined)
            {
                status = 1;
            }

            if ($("#txtDemoUrl")[0].value === "" || $("#txtDemoUrl")[0].value === undefined)
            {
                status = 1;
            }

            if (($("#txtSummary").Editor("getText") === "<br>" || $("#txtSummary").Editor("getText") === undefined) && status === 0)            
            {
                status = 1;
            }

            if ($("#hdnIsAdmin")[0].value === "true" && $("#hdnCreatedBy")[0].value === $("#hdnLoggedInUserID")[0].value)
            {
                if ($("#txtSoftwareDomain")[0].value === "" || $("#txtSoftwareDomain")[0].value === undefined)
                {
                    status = 1;
                }
            }
            else
            {
                if ($("#txtWorkEmailId")[0].value === "" || $("#txtWorkEmailId")[0].value === undefined)
                {
                    status = 1;
                }
                else
                {
                    var isValidWorkEmail = $.ValidateEmail($("#txtWorkEmailId")[0].value);
                    if (isValidWorkEmail)
                    {
                        var isBusinessEmail = $.ValidateWorkEmailID($("#txtWorkEmailId")[0].value);
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

            if (status === 1)
            {
                $.DisplayMessage(true, "Fields marked with * are mandatory.", 0);
                return false;
            }
            else if (status === 3)
            {
                $.DisplayMessage(true, "Software Name must contain only alphanumeric and spaces.", 0);
                return false;                
            }
            else if (fileMessage !== '')
            {
                $.DisplayMessage(true, fileMessage, 0);
                return false;
            }
            else
            {
                $.DisplayMessage(false, "");
                return true;
            }

        } catch (e)
        {
            debugger;
        }
    }


    $('#btnSaveSoftwareProfile').click(function ()
    {
        $.DisplayMessage(false, "", -1);
        $('#ajax_loaderDashboard').show();

        if ($.SaveModeValidations())
        {
            var softwareprofiledata = new Object();
            softwareprofiledata.SoftwareID = (($("#hdnUserSoftwareID")[0].value == "" || $("#hdnUserSoftwareID")[0].value == undefined) ? 0 : $("#hdnUserSoftwareID")[0].value);
            softwareprofiledata.SoftwareName = $("#txtSoftwareName")[0].value;
            softwareprofiledata.LogoName = '';
            softwareprofiledata.TagLine = $("#txtSoftwareTagLine")[0].value;
            softwareprofiledata.PriceRange = $("#txtPriceRange")[0].value;
            softwareprofiledata.SoftwareTrail = $("#txtSoftwareTrail")[0].value;
            softwareprofiledata.DemoURL = $("#txtDemoUrl")[0].value;
            softwareprofiledata.WebSiteURL = $("#txtSoftwareWebSiteUrl")[0].value;
            softwareprofiledata.LinkedInURL = $("#txtLinkedInProfile")[0].value;
            softwareprofiledata.FaceBookURL = $("#txtFacebookProfile")[0].value;
            softwareprofiledata.TwitterURL = $("#txtTwitterProfile")[0].value;
            softwareprofiledata.InstagramURL = $("#txtInstagramProfile")[0].value;
            softwareprofiledata.FoundedYear = $("#txtFoundedYear")[0].value;
            softwareprofiledata.SoftwareDescription = ($("#txtSummary").Editor("getText"));
            softwareprofiledata.CreatedBy = (($("#hdnUserSoftwareID")[0].value === "" || $("#hdnUserSoftwareID")[0].value == undefined) ? $("#hdnLoggedInUserID")[0].value : $("#hdnCreatedBy")[0].value);
            softwareprofiledata.ModifiedBy = $("#hdnLoggedInUserID")[0].value;
            softwareprofiledata.Remarks = $("#txtRejectComments")[0].value;
            var categoryIds = '';
            $('.select-category option:selected').each(function (i)
            {
                categoryIds = categoryIds + $(this).val() + '_';
            });

            softwareprofiledata.SoftwareCatagoryIds = categoryIds.slice(0, categoryIds.length - 1);
            
            if ($("#hdnIsAdmin")[0].value === "true" && $("#hdnCreatedBy")[0].value === $("#hdnLoggedInUserID")[0].value)
            {
                softwareprofiledata.DomainID = $("#txtSoftwareDomain")[0].value;
            } else
            {
                softwareprofiledata.WorkEmail = $("#txtWorkEmailId")[0].value;
            }

            var form = $('#myForm')[0];
            var data = new FormData(form);
            data.append('UploadedFile', uploadedSoftwareLogo);

            var myProfileData = JSON.stringify(softwareprofiledata);
            data.append('ProfileData', myProfileData);

            $.ajax({
                url: $.absoluteurl('/UserCompanyList/SaveSoftwareDetails'),
                data: data,
                cache: false,
                datatype: 'json',
                type: 'POST',
                contentType: false,
                processData: false,
                success: function (response)
                {                    
                    $('#ajax_loaderDashboard').hide();
                    if (response.IsSuccess) {
                        uploadedSoftwareLogo = null;
                        $("#successServiceData").removeClass('hide');
                        $('#spnSuccessMessage').css('display', 'block');
                        $("#addSoftwareData").addClass('hide');
                        if ($("#hdnIsAdmin")[0].value === "true")
                        {
                            $.ajax({
                                url: $.absoluteurl('/UserCompanyList/UserSoftware'),
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
                            var softwareId = ($("#hdnUserSoftwareID")[0].value === "" || $("#hdnUserSoftwareID")[0].value === undefined) ? 0 : $("#hdnUserSoftwareID")[0].value;
                            if (softwareId === "0")
                            {
                                $('#spnSuccessMessage').html("Thanks for creating a software at upvotes.co. Please verify your software by clicking on the link provided in your work email received from upvotes.co!!");
                                $('html, body').animate({ scrollTop: 0 }, 'slow');
                            }
                            else
                            {
                                $('#spnSuccessMessage').html("Thank you for submitting your software in Upvotes, we get back you shortly.");
                                $('html, body').animate({ scrollTop: 0 }, 'slow');
                            }
                        }
                    } else
                    {
                        $.DisplayMessage(true, "Failed to save.");
                    }
                },
                error: function (e)
                {
                    $('#spnMessage').css('display', 'block');
                    $.DisplayMessage(true, "Some error has occured. Failed to save. Please contact admin.", 0);
                    $('#ajax_loaderDashboard').hide();
                }
            });

        } else
        {
            $('#ajax_loaderDashboard').hide();
        }
    });

    $.UpdateSoftwareRejectionComments = function (softwareId)
    {        
        var softwareRejectComments = new Object();
        softwareRejectComments.SoftwareId = $("#hdnUserSoftwareID")[0].value;
        softwareRejectComments.SoftwareName = $("#txtSoftwareName")[0].value;
        softwareRejectComments.RejectComments = $("#txtRejectComments")[0].value;
        softwareRejectComments.WorkEmail = $("#txtWorkEmailId")[0].value;

        softwareRejectComments = JSON.stringify(softwareRejectComments);
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/Softwares/UpdateSoftwareRejectionComments'),
            data: { softwareRejectComments: softwareRejectComments },
            cache: false,
            datatype: 'json',
            type: 'POST',
            success: function (response)
            {                
                $.ajax({
                    url: $.absoluteurl('/UserCompanyList/UserSoftware'),
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
                debugger;
                $('#ajax_loaderDashboard').hide();
                $.DisplayMessage(true, "Some error occured", 0);
            }
        });
    }

    $("#btnSoftwraeCancel").click(function ()
    {        
        var isAdmin = $("#hdnIsAdmin")[0].value;
        if (isAdmin && ($("#txtRejectComments")[0].value == "" || $("#txtRejectComments")[0].value == undefined))
        {
            $.DisplayMessage(true, "Please fill comment for rejection in profile section.", 0)
        }
        else if (isAdmin && $("#txtRejectComments")[0].value != "" && $("#txtRejectComments")[0].value != undefined)
        {
            $.UpdateSoftwareRejectionComments();
        }
        else
        {
            $('#ajax_loaderDashboard').show();
            $.ajax({
                url: $.absoluteurl('/UserCompanyList/UserSoftware'),
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
});

function CheckForUploadedFile(obj)
{
    uploadedSoftwareLogo = obj.files[0];
    if (obj.files && obj.files[0])
    {
        var reader = new FileReader();

        reader.onload = function (e)
        {
            $('#imgpreview').attr('src', e.target.result);
            $('#imgpreview').show();
            $('#txtLogoName').text(uploadedSoftwareLogo.name);
        };

        reader.readAsDataURL(obj.files[0]);
    }
}


function ValidateUploadedFile()
{

    if ($("#UplAttachment")[0].value === "")
    {
        return 'Please select a file';
    }

    var extensionString = "";
    var extensionArray = new Array();
    extensionArray.push(".jpg", ".jpeg", ".jpe", ".gif", ".png");
    var name = $("#UplAttachment")[0].value;
    var extension = (name.substring(name.length, name.lastIndexOf("."))).toLowerCase();
    var supportFlag = false;
    var i;
    for (i = 0; i < extensionArray.length; i++)
    {
        if (extension == extensionArray[i])
        {
            supportFlag = true;
            break;
        }
        extensionString = extensionString + extensionArray[i] + (i === 9 ? "\n" : "  ");
    }
    if (!supportFlag)
    {
        $("#UplAttachment")[0].value = '';
        $("#UplAttachment").replaceWith($("#UplAttachment").clone(true));

        return 'Logo format is not supported. Supported formats are ' + "\n" + extensionString;
    }
    var uploadedFile = $("#UplAttachment")[0].value;
    var files = $("#UplAttachment")[0].files;
    var fileNames = "";

    for (i = 0; i < files.length; i++)
    {
        name = files[i].name;
        var tempName = name.substring(name.indexOf("\\") === -1 ? 0 : name.lastIndexOf("\\") + 1, name.length);

        if (tempName.length > 256)
        {
            fileNames = fileNames + tempName + ", ";
        }
    }
    if (fileNames !== "")
    {
        fileNames = fileNames.substring(0, fileNames.lastIndexOf(","));
        $("#UplAttachment")[0].value = '';
        $("#UplAttachment").replaceWith($("#UplAttachment").clone(true));
        return 'Logo name should not exceed more than 100 characters ' + ' (' + fileNames + ')';
    }

    fileNames = "";
    var filesSize = 0;
    for (i = 0; i < files.length; i++)
    {
        name = files[i].name;
        filesSize = filesSize + files[i].size;
    }

    if (filesSize > 1048576)
    { //its in byte
        //Conver File Size to MB 
        filesSize = parseFloat(filesSize / 1048576).toFixed(2);
        $("#UplAttachment")[0].value = '';
        $("#UplAttachment").replaceWith($("#UplAttachment").clone(true));
        return 'Logo should be 1MB in size. Your file size is ' + filesSize + 'MB';
    }

    return '';
}