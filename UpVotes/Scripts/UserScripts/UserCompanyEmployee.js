var uploadPortFolioImg;
$(document).ready(function ()
{
    $('.btnaddCompanyEmployee').click(function ()
    {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/CreateNewCompanyEmployeeForm'),
            type: "POST",
            data: {companyEmployeeID : 0},
            success: function (response)
            {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);                
            }

        });
    });

    $('.btnEditCompanyEmployee').click(function ()
    {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/CreateNewCompanyEmployeeForm'),
            type: "POST",
            data: {companyEmployeeID : 1},
            success: function (response)
            {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);                
            }

        });
    });

});

