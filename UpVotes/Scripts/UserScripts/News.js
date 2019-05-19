
var uploadedNewsImage;
$(document).ready(function () {

    var type = 2;

    if ($("#txtDescription").length > 0) {
        $("#txtDescription").Editor();
    }

    $('#ddlServiceCategory').change(function () {
        var focusAreaID = $('#ddlServiceCategory').val();
        if (focusAreaID != 0) {
            $.ajax({
                url: $.absoluteurl('/UserCompanyList/GetSubFocusAreaByFocusID?focusAreaID=' + focusAreaID),
                cache: false,
                async: false,
                datatype: 'json',
                type: 'GET',
                success: function (response) {
                    $.ClearDropdown($('#ddlSubCategory'));
                    $('#ddlSubCategory').LoadOptions(response.subfocusList, 'SubFocusAreaName', 'SubFocusAreaID');
                },
                error: function (e) {
                    alert("Some error has occured. Unable to get the states. Please contact admin.");
                }
            });
        }
    });
    $('#ddlCountry').change(function () {
        var countryID = $('#ddlCountry').val();
        if (countryID != 0) {
            $.ajax({
                url: $.absoluteurl('/UserCompanyList/GetStates?countryID=' + countryID),
                cache: false,
                async: false,
                datatype: 'json',
                type: 'GET',
                success: function (response) {
                    $.ClearDropdown($('#ddlState'));
                    $('#ddlState').LoadOptions(response.statesList, 'StateName', 'StateID');
                },
                error: function (e) {
                    alert("Some error has occured. Unable to get the states. Please contact admin.");
                }
            });
        }
    });

    $('#ddlProvider').change(function () {
        var provider = $(this).val();
        if(provider == "1")
        {
            $('#ddlServiceCategory').removeClass('hide');
            $('#ddlSubCategory').removeClass('hide');
            $('#divLocation').removeClass('hide');
            $('#ddlSoftwareCategory').addClass('hide');
            type = 2;
        }
        else {
            $('#ddlServiceCategory').addClass('hide');
            $('#ddlSubCategory').addClass('hide');
            $('#divLocation').addClass('hide');
            $('#ddlSoftwareCategory').removeClass('hide');
            type = 1;
        }
    });

    $.IsExistsNews = function (websiteurl, title) {
        $.ajax({
            url: $.absoluteurl('/OverViewAndNews/IsNewsExists'),
            cache: false,
            async: false,
            datatype: 'json',
            data: { Title: title, WebsiteUrl: websiteurl },
            type: 'GET',
            success: function (response) {
                if (websiteurl != '') {
                    if(!response.IsSuccess){
                        $('#spnAvailURL').removeClass('hide');
                        $('#spnNotAvailURL').addClass('hide');
                    }
                    else {
                        $('#spnAvailURL').addClass('hide');
                        $('#spnNotAvailURL').removeClass('hide');
                    }
                }
                else if (title != '') {
                    if (!response.IsSuccess) {
                        $('#spnAvailTitle').removeClass('hide');
                        $('#spnNotAvailTitle').addClass('hide');
                    }
                    else {
                        $('#spnAvailTitle').addClass('hide');
                        $('#spnNotAvailTitle').removeClass('hide');
                    }
                }
            },
            error: function (e) {
                //alert("Some error has occured. Unable to get the countries. Please contact admin.");
            }
        });
    }

    $.LoadCountry = function () {
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/GetCountry'),
            cache: false,
            async: false,
            datatype: 'json',
            type: 'GET',
            success: function (response) {
                $.ClearDropdown($('#ddlCountry'));
                $('#ddlCountry').LoadOptions(response.countryList, 'CountryName', 'CountryID');
                countryList = response.countryList;
            },
            error: function (e) {
                alert("Some error has occured. Unable to get the countries. Please contact admin.");
            }
        });
    }

    $("#txtCompanySoftware").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: $.absoluteurl('/SubmitReview/GetCompanySoftwareAutoComplete'), type: "POST",
                dataType: "json",
                async: false,
                data: {
                    search: request.term,
                    type: type
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

    $('#txtWebsiteURL').change(function () {
        var websiteurl = $(this).val();
        $.IsExistsNews(websiteurl, '');
    });
    $('#txtTitle').change(function () {
        var Title = $(this).val();
        Title = handleSpecialChar(Title)
        $.IsExistsNews('', Title);
    });

    $.SaveModeUserNewsValidations = function () {
        var IsValid = true;
        if ($('#txtWebsiteURL').val().trim() == '') {
            IsValid = false;
        }
        else if ($('#txtTitle').val().trim() == '') {
            IsValid = false;
        }
        else if (($("#txtDescription").Editor("getText") == "<br>" || ($("#txtDescription").Editor("getText") == "") || $("#txtDescription").Editor("getText") == undefined)) {
            IsValid = false;
        }
        else if ($('#hdnIsCompany').val() == "Yes" && $('#ddlUserServiceCategory').length > 0 && $('#ddlUserServiceCategory').val() == "0") {
            IsValid = false;
        }
        else if ($('#hdnIsCompany').val() == "No" && $('#ddlSoftwareCategory').length > 0 && $('#ddlSoftwareCategory').val() == "0") {
            IsValid = false;
        }

        $('#divErrorMsg').addClass('hide');
        if (!IsValid) {
            $('html, body').animate({ scrollTop: 0 }, 'slow');
            $('#divErrorMsg').removeClass('hide');
        }
        return IsValid;
    }

    $.SaveModeValidations = function ()
    {
        var IsValid = true;
        if($('#txtWebsiteURL').val().trim() == '')
        {
            IsValid = false;
        }
        else if($('#txtTitle').val().trim() == '')
        {
            IsValid = false;
        }
        else if (($("#txtDescription").Editor("getText") == "<br>" || ($("#txtDescription").Editor("getText") == "") || $("#txtDescription").Editor("getText") == undefined)) {
            IsValid = false;
        }
        else if ($('#ddlProvider').val() == "1" && $('#ddlServiceCategory').val() == "0")
        {
            IsValid = false;
        }
        else if ($('#ddlProvider').val() == "2" && $('#ddlSoftwareCategory').val() == "0") {
            IsValid = false;
        }

        $('#divErrorMsg').addClass('hide');
        if (!IsValid)
        {
            $('html, body').animate({ scrollTop: 0 }, 'slow');
            $('#divErrorMsg').removeClass('hide');
        }
        return IsValid;
    }

    $('#btnSavePost').click(function () {        
        if($.SaveModeValidations())
        {            
            var NewsRequestObj = new Object();
            NewsRequestObj.IsCompanySoftware = $('#ddlProvider').val();            
            if(NewsRequestObj.IsCompanySoftware == "1")
            {
                NewsRequestObj.CategoryID = $('#ddlServiceCategory').val();
                NewsRequestObj.Subcategory = $('#ddlSubCategory').val();
                NewsRequestObj.CountryID = $('#ddlCountry').val();
                NewsRequestObj.StateID = $('#ddlState').val();
                NewsRequestObj.City = $('#txtCity').val();
            }
            else
            {
                NewsRequestObj.CategoryID = $('#ddlSoftwareCategory').val();
                NewsRequestObj.Subcategory = 0;
                NewsRequestObj.CountryID = 0;
                NewsRequestObj.StateID = 0;
                NewsRequestObj.City = '';
            }            
            NewsRequestObj.CompanySoftwareID = $('#hdnCompanySoftwareID').val();
            NewsRequestObj.CompanySoftwareName = $('#txtCompanySoftware').val();
            NewsRequestObj.WebsiteURL = $('#txtWebsiteURL').val(); 
            NewsRequestObj.Title = $('#txtTitle').val();
            NewsRequestObj.UrlTitle = handleSpecialChar($('#txtTitle').val());
            NewsRequestObj.Description = ($("#txtDescription").Editor("getText"));
            NewsRequestObj.YoutubeURL = $('#txtYoutubeUrl').val();
            var form = $('#myForm')[0];
            var data = new FormData(form);
            data.append('UploadedFile', uploadedNewsImage)

            var myNewsData = JSON.stringify(NewsRequestObj);
            data.append('NewsData', myNewsData);

            $('#ajax_loaderDashboard').show();
            $.ajax({
                url: $.absoluteurl('/OverViewAndNews/SaveNews'),
                data: data,
                cache: false,
                datatype: 'json',
                type: 'POST',
                contentType: false,
                processData: false,
                success: function (response) {                    
                    $('#ajax_loaderDashboard').hide();
                    if (response.IsSuccess) {
                        uploadedNewsImage = null;                        
                        alert("Successfully Saved.");
                        $('#showAddNewsSection').click();
                    } else {                        
                        alert("Failed to save.");
                    }
                },
                error: function (e) {
                    $('#ajax_loaderDashboard').hide();
                    alert("Some error has occured. Failed to save. Please contact admin.");                    
                }
            });
        }
    });

    $('#btnSaveUserNews').click(function () {
        if ($.SaveModeUserNewsValidations()) {
            var NewsRequestObj = new Object();
            if ($('#ddlUserServiceCategory').length > 0) {
                NewsRequestObj.IsCompanySoftware = "1";
            }
            else
            {
                NewsRequestObj.IsCompanySoftware = "2";
            }

            if (NewsRequestObj.IsCompanySoftware == "1") {
                NewsRequestObj.CategoryID = $('#ddlUserServiceCategory').val();
                NewsRequestObj.Subcategory = 0;
                NewsRequestObj.CountryID = 0;
                NewsRequestObj.StateID = 0;
                NewsRequestObj.City = '';
            }
            else {
                NewsRequestObj.CategoryID = $('#ddlSoftwareCategory').val();
                NewsRequestObj.Subcategory = 0;
                NewsRequestObj.CountryID = 0;
                NewsRequestObj.StateID = 0;
                NewsRequestObj.City = '';
            }
            NewsRequestObj.CompanySoftwareID = $('#hdnCompanySoftwareID').val();
            NewsRequestObj.CompanySoftwareName = $('#txtCompanySoftwareName').val();
            NewsRequestObj.WebsiteURL = $('#txtWebsiteURL').val();
            NewsRequestObj.Title = $('#txtTitle').val();
            NewsRequestObj.UrlTitle = handleSpecialChar($('#txtTitle').val());
            NewsRequestObj.Description = ($("#txtDescription").Editor("getText"));
            NewsRequestObj.YoutubeURL = $('#txtYoutubeUrl').val();
            var form = $('#myForm')[0];
            var data = new FormData(form);
            data.append('UploadedFile', uploadedNewsImage)

            var myNewsData = JSON.stringify(NewsRequestObj);
            data.append('NewsData', myNewsData);

            $('#ajax_loaderDashboard').show();
            $.ajax({
                url: $.absoluteurl('/OverViewAndNews/SaveUserNews'),
                data: data,
                cache: false,
                datatype: 'json',
                type: 'POST',
                contentType: false,
                processData: false,
                success: function (response) {
                    $('#ajax_loaderDashboard').hide();
                    if (response.IsSuccess) {
                        uploadedNewsImage = null;                        
                        alert("Successfully Saved.");
                        $('#showAddUserNewsSection').click();
                    } else {
                        alert("Failed to save.");
                    }
                },
                error: function (e) {
                    $('#ajax_loaderDashboard').hide();
                    alert("Some error has occured. Failed to save. Please contact admin.");
                }
            });
        }
    });

});
function CheckForUploadedFile(obj) {
    uploadedNewsImage = obj.files[0];    
    if (obj.files && obj.files[0]) {
        var ext = uploadedNewsImage.name.split('.').pop().toLowerCase();
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
            uploadedNewsImage = null;
            $('#txtImageName').text('Select Image');
            alert('Select Image!');
            uploadedNewsImage = Object();
        }
        else {            
            $('#txtImageName').text(uploadedNewsImage.name);            
        }
    }
}

function handleSpecialChar(value)
{
    value = value.replace(/[^a-z0-9\s]/gi, '').replace(/[_\s]/g, '-');
    return value;
}