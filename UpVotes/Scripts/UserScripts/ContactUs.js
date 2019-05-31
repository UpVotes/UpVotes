
/* Mega Menu */
$(document).ready(function () {    

    $('#btnContactSubmit').click(function () {
        if ($.SaveModeValidation())
        {
            var name = $('#txtContactName').val();
            var email = $('#txtContactEmail').val();
            var phone = $('#txtContactPhone').val();
            var companyname = $('#txtContactCompany').val();
            var message = $('#txtContactMessage').val();

            $('#ajax_loaderContact').show();
            $.ajax({
                url: $.absoluteurl('/ContactUs/SaveContactUsInfo'),
                data: { Name: name.trim(), Email: email.trim(), Phone: phone.trim(), CompanyName: companyname.trim(), ContactMessage: message.trim() },
                type: "POST",
                success: function (response) {
                    $('#ajax_loaderContact').hide();
                    if (response.IsSuccess) {
                        $('#contactModal').modal();
                        $.ClearControls();
                    }

                },
                error: function (e) {
                    alert('Some error has occured. Failed to save. Please contact admin.');
                    $('#ajax_loaderContact').hide();
                }

            });
        }
    });

    $.SaveModeValidation = function()
    {
        var name= $('#txtContactName').val();
        var email = $('#txtContactEmail').val();
        var phone = $('#txtContactPhone').val();
        var companyname = $('#txtContactCompany').val();
        var message = $('#txtContactMessage').val();
        $('#spnErrorMessage').text('');
        $('#spnErrorMessage').hide();
        
        if (name.trim() == '' || email.trim() == '' || phone.trim() == '' || companyname.trim() == '' || message.trim() == '')
        {
            $('#spnErrorMessage').show();
            $('#spnErrorMessage').text('All * are mandatory');
            return false;
        }
        else if(!$.ValidateEmail(email.trim()))
        {
            $('#spnErrorMessage').show();
            $('#spnErrorMessage').text('Enter valid email!');
            return false;
        }
        return true;
    }

    $.ClearControls = function () {
        $('#txtContactName').val('');
        $('#txtContactEmail').val('');
        $('#txtContactPhone').val('');
        $('#txtContactCompany').val('');
        $('#txtContactMessage').val('');
        $('#spnErrorMessage').text('');
        $('#spnErrorMessage').hide();        
    }

});


