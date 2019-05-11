$(document).ready(function () {

    $.LoginWithTwitterAndLinkedIn = function (companyID, calledPage, url) {        
        $.ajax({
            type: "POST",   //GET or POST or PUT or DELETE verb
            url: $.absoluteurl(url),//?companyid='+compid,
            data: { companyid: companyID, calledPage: calledPage },// Location of the service
            success: function (json) {                
                 //On Successful service call
                window.location.href = json;
            }

        });
    }

    $.LoginWithRegisteredUser = function (companyID, WorkEmail, Password, url) {
        $("#ajax_loader").show();
        $("#dvforgotpwd").hide();
        $.ajax({
            type: "POST",   //GET or POST or PUT or DELETE verb
            url: $.absoluteurl(url),//?companyid='+compid,
            data: { companyid: companyID, workemail: WorkEmail, password: Password },// Location of the service
            success: function (json) {
                if (json != "") {
                    location.reload();
                    //$('#myModal').modal('hide');                    
                    //$("#divLogin").html(json);
                    //$("#ajax_loader").hide();                    
                    //var $dummy = $("<div>");
                    //$dummy.load("/Login/GetFooterSection", function (response, status, xhr) {
                    //    if (status == "success") {
                    //        $("#divfooter").html('');
                    //        $("#divfooter").html($dummy.html());
                    //    }
                    //    else
                    //        $dummy.remove();
                    //});
                }
                else {
                    $('#errValidWorkEmail').show();
                    $("#ajax_loader").hide();
                }

            }

        });
    }

    $.ForgotPassword = function (WorkEmail) {
        $("#ajax_loader").show();
        $.ajax({
            type: "POST", 
            url: $.absoluteurl('/Login/ForgotPassword'),
            data: { workemail: WorkEmail },
            success: function (json) {
                if (json != "") {
                    $("#dvforgotpwd").show();
                    $('#errValidWorkEmail').hide();
                    $('#errValidWorkEmail1').hide();
                    $("#ajax_loader").hide();
                }
                else {
                    $("#dvforgotpwd").hide();
                    $('#errValidWorkEmail1').show();
                    $("#ajax_loader").hide();
                }

            }

        });
    }

    $.checkPassword = function (pwd)
    {
        // at least one number, one special character
        var re = /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,}$/;
        return re.test(pwd);
    }

    $.ValidateChangePassword = function()
    {
        $('#errValidChangepwd').hide();
        $('#errValidChangepwd').text('');
        if ($('#txtCurrPassword').val() == "") {
            $('#errValidChangepwd').text('Error: Current Password cannot be blank!');
            $('#errValidChangepwd').show();
            return false;
        }
        if ($('#txtNewPassword').val() != "" && $('#txtNewPassword').val() == $('#txtConNewPassword').val()) {
            if (!$.checkPassword($('#txtNewPassword').val())) {
                $('#errValidChangepwd').text('Error: The password should contain atleast 6 character with one number, one special character');
                $('#errValidChangepwd').show();
                return false;
            }
        } else {
            $('#errValidChangepwd').text("Error: Please check that you've entered and confirmed your password!");
            $('#errValidChangepwd').show();
            return false;
        }
        return true;
    }

    $.ChangePassword = function(currpwd, newpwd)
    {
        $('#dvchangepwd').hide();
        $('#errValidChangepwd').hide();
        $("#ajax_loaderchng").show();
        $.ajax({
            type: "POST",
            url: $.absoluteurl('/Login/ChangePassword'),
            data: { CurrentPassword: currpwd,NewPassword: newpwd },
            success: function (json) {
                if (json != "") {
                    $('#MsgChangePwd').text(json);
                    $('#dvchangepwd').show();
                    $("#ajax_loaderchng").hide();
                    $('#txtCurrPassword').val('');
                    $('#txtNewPassword').val('');
                    $('#txtConNewPassword').val('');
                }
                else {
                    $('#dvchangepwd').hide();
                    $('#errValidChangepwd').show();
                    $('#errValidChangepwd').text("Error: Current Password is incorrect!");
                    $("#ajax_loaderchng").hide();
                }

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
});