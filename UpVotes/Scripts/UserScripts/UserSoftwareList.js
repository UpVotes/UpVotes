

$(document).ready(function ()
{
    
    if ($("#txtSummary").length > 0) {
        $("#txtSummary").Editor();        
    }
   



    $('#addAdminSoftwareProfile').click(function () {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/CreateNewSoftwareAdmin'),
            type: "POST",
            success: function (response) {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });

    
    $("#addServiceProfile").click(function () {
        $("#emptySoftwareData").addClass("hide");
        $("#addSoftwareData").removeClass("hide");
    });

    
    $("#btnCompanyList").click(function () {
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/UserSoftware'),
            data: { companyName: "" },
            type: "POST",
            success: function (response) {
                $('#ajax_loaderDashboard').hide();
                $('#DetailsContent').html("");
                $('#DetailsContent').html(response);
            }

        });
    });
});

