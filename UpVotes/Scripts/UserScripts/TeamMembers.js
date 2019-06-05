var uploadedProfilePicture;
$(document).ready(function ()
{
    $('.btnaddTeamMember').click(function ()
    {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/TeamMemberForm'),
            type: "GET",
            data: { memberId: 0 },
            success: function (response)
            {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });

    $('.btnEditTeamMember').click(function () {        
        var teamMemberId = $(this).attr("memberID");
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/TeamMemberForm'),
            type: "GET",
            data: { memberID: teamMemberId },
            success: function (response)
            {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);                
            }

        });
    });

    $.DisplayMessage = function (isError, displayText)
    {
        if (isError)
        {
            $("#divFailureMessage").removeClass('hide');
            $('#spnMessage').css('display', 'block');
            $('#spnMessage').html(displayText);
            $('html, body').animate({ scrollTop: 0 }, 'slow');
        }
        else
        {
            $("#divFailureMessage").addClass('hide');
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
            if ($('#txtMemberName').val() === "" || $('#txtMemberName').val() === undefined)
            {
                status = 1;
            }

            if ($('#txtDesignation').val() === "" || $('#txtDesignation').val() === undefined)
            {
                status = 1;
            }

            if ($('#txtStartDate').val() === "" || $('#txtStartDate').val() === undefined)
            {
                status = 1;
            } else
            {
                if (!isDate($('#txtStartDate').val()))
                {
                    status = 2;
                }
            }

            if ($('#txtEndDate').val() !== "" && $('#txtEndDate').val() !== undefined && !isDate($('#txtEndDate').val()))
            {
                status = 2;
            }

            if ($("#UplAttachment")[0].value !== "" && $("#UplAttachment")[0].value !== undefined)
            {
                fileMessage = ValidateUploadedFile();
            }

            if (parseInt($('#hdnMemberId').val()) === 0)
            {
                fileMessage = ValidateUploadedFile();
            }

            if (status === 1)
            {
                $.DisplayMessage(true, "Fields marked with * are mandatory.", 0);
                return false;
            }
            else if (status === 2)
            {
                $.DisplayMessage(true, "Invalid date.", 0);
                return false;
            }
            else if (fileMessage !== "")
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

        }
    }

    $('#btnSaveTeamMember').click(function() {        
        $.DisplayMessage(false, "", -1);
        $('#ajax_loaderDashboard').show();

        if ($.SaveModeValidations()) {
            var teamMemberData = new Object();
            teamMemberData.MemberId = (($("#hdnMemberId")[0].value === "" || $("#hdnMemberId")[0].value === undefined)
                ? 0
                : $("#hdnMemberId")[0].value);
            teamMemberData.MemberName = $("#txtMemberName")[0].value;
            teamMemberData.PictureName = $("#txtLogoName")[0].value;
            teamMemberData.Designation = $("#txtDesignation")[0].value;
            teamMemberData.LinkedInProfile = $("#txtLinkedInProfile")[0].value;
            teamMemberData.StartDate = $("#txtStartDate")[0].value;
            teamMemberData.EndDate = $("#txtEndDate")[0].value;

            var form = $('#myForm')[0];
            var data = new FormData(form);
            data.append('UploadedFile', uploadedProfilePicture);

            var myTeamMemberData = JSON.stringify(teamMemberData);
            data.append('TeamMemberData', myTeamMemberData);

            $.ajax({
                url: $.absoluteurl('/UserCompanyList/SaveTeamMembers'),
                data: data,
                cache: false,
                datatype: 'json',
                type: 'POST',
                contentType: false,
                processData: false,
                success: function(response) {                    
                    $('#ajax_loaderDashboard').hide();
                    if (response.IsSuccess) {
                        uploadedProfilePicture = null;
                        alert("Successfully Saved.");
                        $('#showAddTeamMembersSection').click();
                    } else {
                        $.DisplayMessage(true, "Failed to save.");
                    }
                },
                error: function(e) {
                    $('#spnMessage').css('display', 'block');
                    $.DisplayMessage(true, "Some error has occured. Failed to save. Please contact admin.", 0);
                    $('#ajax_loaderDashboard').hide();
                }
            });
        } else {
            $('#ajax_loaderDashboard').hide();
        }
    });

    $('.btnDeleteTeamMember').click(function () {        
        $('#hdnTeamMemberId').val($(this).attr('memberID'));
        $('#hdnPictureUrl').val($(this).attr('image'));
    });

    $('.btnDeleteNo').click(function ()
    {
        $('#hdnTeamMemberId').val('0');
        $('#hdnPictureUrl').val('');
    });

    $('#btnDeleteYes').click(function () {        
        var teamMemberId = $('#hdnTeamMemberId').val();
        var ImageUrl = $('#hdnPictureUrl').val();
        $('.btnDeleteNo').click();
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/DeleteTeamMember'),
            data: { teamMemberId: teamMemberId, imageUrl: ImageUrl },
            type: "POST",
            success: function (response)
            {
                $('#ajax_loaderDashboard').hide();
                if (response.IsSuccess)
                {
                    alert('Successfully Deleted');
                    $('#hdnTeamMemberId').val('0');
                    $('#hdnPictureUrl').val('');
                    $('#showAddTeamMembersSection').click();
                }

            },
            error: function (e)
            {
                alert('Some error has occured. Failed to save. Please contact admin.');
                $('#ajax_loaderDashboard').hide();
            }

        });
    });

});
function CheckForUploadedFile(obj)
{
    uploadedProfilePicture = obj.files[0];
    if (obj.files && obj.files[0])
    {
        var reader = new FileReader();

        reader.onload = function (e)
        {
            $('#imgpreview').attr('src', e.target.result);
            $('#imgpreview').show();
            $('#txtLogoName').text(uploadedProfilePicture.name);
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