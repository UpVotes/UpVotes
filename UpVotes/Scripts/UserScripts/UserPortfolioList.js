var uploadPortFolioImg;
$(document).ready(function ()
{


    $('.btnaddPortFolio').click(function ()
    {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/CreateNewPortFolioForm'),
            type: "POST",
            data: {portfolioID : 0},
            success: function (response)
            {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);                
            }

        });
    });
    
    $('.btnEditPortfolio').click(function ()
    {
        var PortFolioID=$(this).attr('portid');
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/CreateNewPortFolioForm'),
            data: { portfolioID: PortFolioID },
            type: "POST",
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
            if ($('#txtProjectName').val() === "" || $('#txtProjectName').val() === undefined)
            {
                status = 1;
            }

            if ($('#txtProjectDescription').val() === "" || $('#txtProjectDescription').val() === undefined)
            {
                status = 1;
            }

            if (($("#UplAttachment")[0].value !== "" && $("#UplAttachment")[0].value !== undefined) || (parseInt($('#hdnPortfolioID').val()) == 0))
            {
                fileMessage = ValidateUploadedFile();
            }

            //if (parseInt($('#hdnPortfolioID').val()) == 0)
            //{
            //    fileMessage = ValidateUploadedFile();
            //}

            if (status === 1)
            {
                $.DisplayMessage(true, "Fields marked with * are mandatory.", 0);
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
           
        }
    }

    $('.btnDeletePortFolio').click(function () {
        $('#hdnPortfolioID').val($(this).attr('portid'));
        $('#hdnImageUrl').val($(this).attr('image'));       
    });

    $('.btnDeleteNo').click(function () {
        $('#hdnPortfolioID').val('0');
        $('#hdnImageUrl').val('');        
    });

    $('#btnDeleteYes').click(function () {
        var PortFolioID = $('#hdnPortfolioID').val();
        var ImageUrl = $('#hdnImageUrl').val();
        $('.btnDeleteNo').click();
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/DeletePortFolio'),
            data: { portfolioID: PortFolioID, ImageUrl: ImageUrl },
            type: "POST",
            success: function (response) {
                $('#ajax_loaderDashboard').hide();
                if (response.IsSuccess) {
                    alert('Successfully Deleted');                    
                    $('#hdnPortfolioID').val('0');
                    $('#hdnImageUrl').val('');
                    $('#showAddUserPortfolioSection').click();
                }
                
            },
            error: function (e)
            {
                alert('Some error has occured. Failed to save. Please contact admin.');
                $('#ajax_loaderDashboard').hide();
            }

        });
    });

    $('#btnsavePortFolio').click(function ()
    {        
        $.DisplayMessage(false, "", -1);
        $('#ajax_loaderDashboard').show();

        if ($.SaveModeValidations())
        {
            var Portfoliodata = new Object();
            Portfoliodata.CompanyID = $('#hdnCompanyID').val();
            Portfoliodata.CompanyPortFolioID = $('#hdnPortfolioID').val();
            Portfoliodata.Description = $('#txtProjectDescription').val();
            Portfoliodata.Title = $('#txtProjectName').val();

            var form = $('#myForm')[0];
            var data = new FormData(form);
            data.append('UploadedFile', uploadPortFolioImg);

            var myPortfolioData = JSON.stringify(Portfoliodata);
            data.append('PortfolioData', myPortfolioData);

            $.ajax({
                url: $.absoluteurl('/UserCompanyList/SaveCompanyPortfolio'),
                data: data,
                cache: false,
                datatype: 'json',
                type: 'POST',
                contentType: false,
                processData: false,
                success: function (response)
                {                    
                    $('#ajax_loaderDashboard').hide();
                    if (response.IsSuccess)
                    {
                        uploadPortFolioImg = null;
                        alert("Successfully Saved.");
                        $('#showAddUserPortfolioSection').click();                        
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

    

    
});

function CheckForUploadedFile(obj)
{
    uploadPortFolioImg = obj.files[0];
    if (obj.files && obj.files[0])
    {
        var reader = new FileReader();

        reader.onload = function (e)
        {
            $('#imgpreview').attr('src', e.target.result);
            $('#imgpreview').show();
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
        $('#imgpreview').attr('src', '');
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
        $('#imgpreview').attr('src', '')
        return 'Image should not exceed more than 100 characters ' + ' (' + fileNames + ')';
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
        $('#imgpreview').attr('src', '')
        return 'Image should be 1MB in size. Your file size is ' + filesSize + 'MB';
    }

    return '';
}