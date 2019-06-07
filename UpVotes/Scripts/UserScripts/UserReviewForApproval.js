
$(document).ready(function ()
{
    $('.btnViewReview').click(function ()
    {
        $('#ajax_loaderDashboard').show();
        $.ajax({
            url: $.absoluteurl('/UserCompanyList/ViewReviewByID'),
            type: "POST",
            data: { ReviewID: $(this).attr('ReviewID') },
            success: function (response)
            {
                $('#hdnReviewID').val(response.ReviewID);
                $('#spnCategoryName').text(response.CategoryName);
                $('#spnProjectName').text(response.ProjectName);
                $('#spnSummary').text(response.Summary);
                $('#ulRating').attr('class', '')
                var className='star-rate rating-'+response.Rating;
                $('#ulRating').attr('class', className);
                $('#spnReviewerName').text(response.ReviewerUserName);
                $('#spnDesignation').text(response.ReviewerDesignation);
                $('#spnReviewerCompanyName').text(response.ReviewerCompanyName);
                $('#spnEmail').text(response.Email);
                $('#spnPhone').text(response.Phone);
                $('#divApproveRejectButton').removeClass('hide');
                if (response.IsApproved)
                {
                    $('#divApproveRejectButton').addClass('hide');
                }
                $('#divViewReviewDetails').modal();
                $('#ajax_loaderDashboard').hide();
                              
            },
            error: function (e) {
                $('#ajax_loaderDashboard').hide();
            }

        });
    }); 

    $('#btnReject').click(function () {        
        if(confirm('Are you sure you want to reject this user review?'))
        {
            $('#ajax_loaderDashboard').show();
            $.ajax({
                url: $.absoluteurl('/UserCompanyList/ApproveRejectUserReview'),
                type: "POST",
                data: { ReviewID: $('#hdnReviewID').val(), IsApprove: false },
                success: function (response) {
                    $('#ajax_loaderDashboard').hide();
                    if (response) {
                        alert('Successfully Rejected!');
                        $('.close').click();
                        $('#showUserReviewSection').click();
                    }
                    else
                    {
                        alert("Some error has occured. Unable to get the states. Please contact admin.");
                    }

                },
                error: function (e) {
                    $('#ajax_loaderDashboard').hide();
                    alert("Some error has occured. Unable to get the states. Please contact admin.");
                }

            });
        }
    });

    $('#btnApprove').click(function () {
        if (confirm('Are you sure you want to approve this user review?')) {
            $.ajax({
                url: $.absoluteurl('/UserCompanyList/ApproveRejectUserReview'),
                type: "POST",
                data: { ReviewID: $('#hdnReviewID').val(), IsApprove: true },
                success: function (response) {
                    $('#ajax_loaderDashboard').hide();
                    if (response) {
                        alert('Successfully Approved!');
                        $('.close').click();
                        $('#showUserReviewSection').click();
                    }
                    else {
                        alert("Some error has occured. Unable to get the states. Please contact admin.");
                    }
                },
                error: function (e) {
                    $('#ajax_loaderDashboard').hide();
                    alert("Some error has occured. Unable to get the states. Please contact admin.");
                }

            });
        }
    });

    
});
