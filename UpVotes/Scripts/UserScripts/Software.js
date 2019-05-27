$(document).ready(function () {

    $.VoteForSoftware = function (softwareID, softwareName) {
        $.ajax({
            url: $.absoluteurl('/Softwares/VoteForSoftware'),
            cache: false,
            async: false,
            datatype: 'json',
            data: { softwareID: softwareID },
            type: 'POST',
            success: function (response) {
                response = eval(response);
                if (response == '0') {
                    $('#myModal').modal();
                }
                else {
                    alert(response);
                    window.location.reload();
                }
            }
        });
    }

    $('#btnVote').click(function () {
        var softwareID = $('#hdnSoftwareID')[0].value;
        var softwareName = $('#hdnSoftwareName')[0].value;
        $.VoteForSoftware(softwareID, softwareName);
    });

    $('.btnThankNote').click(function () {
        var softwareReviewID = $(this).attr('SoftwareReviewID');
        var softwareID = $('#hdnSoftwareID')[0].value;

        $.ajax({
            url: $.absoluteurl('/Softwares/ThanksNoteForReview'),            
            async: false,
            datatype: 'json',
            data: { softwareID: softwareID, softwareReviewID: softwareReviewID },
            type: 'POST',
            success: function (response) {               
                alert(response);
                window.location.reload();
            }
        });
    });
    $('#btnClaimListing').click(function () {
        $('#spnerrordomain').text('');
        $('#spnerrordomain').hide();
        $('#txtClaimListing').val('');
    });

    $('#btnclaimSubmit').click(function () {        
        if ($('#txtClaimListing').val() != "") {
            var email = $('#txtClaimListing').val()
            if (email.indexOf("@") == -1) {
                $('#ajax_loaderClaim').show();
                $('#spnerrordomain').text('');
                $('#spnerrordomain').hide();
                var SoftwareID = $('#hdnSoftwareID')[0].value;
                var domain = $('#hdnSoftwareDomain')[0].value;
                $.ajax({
                    url: $.absoluteurl('/Softwares/ClaimSoftwareListing'),                    
                    data: { SoftwareID: SoftwareID, Email: email, Domain: domain },
                    type: 'POST',
                    success: function (response) {
                        $('#spnerrordomain').text(response);
                        $('#spnerrordomain').show();
                        $('#ajax_loaderClaim').hide();
                    }
                });
            }
            else {
                $('#spnerrordomain').text('Domain is not required');
                $('#spnerrordomain').show();
            }
        }
        else {
            $('#spnerrordomain').text('Work email is required without domain');
            $('#spnerrordomain').show();
        }


    });

    $('#btnSubmitReview').click(function () {
        var softwareID = $('#hdnSoftwareID')[0].value;
        var softwareName = $('#hdnSoftwareName')[0].value;
        $.ajax({
            url: $.absoluteurl('/Softwares/SubmitSoftwareReview'),
            cache: false,
            async: false,
            datatype: 'json',
            data: { softwareID: softwareID, softwareName: softwareName },
            type: 'POST',
            success: function (response) {
                if (response != "Please login to submit the review.") {
                    window.open(window.location.origin + response, "_blank");
                }
                else {
                    alert(response);
                }
            }
        });
    });

    $('#btnAddNews').click(function () {
        $.ajax({
            url: $.absoluteurl('/Softwares/AddNews'),
            cache: false,
            async: false,
            datatype: 'json',
            type: 'POST',
            success: function (response) {
                if (response != "Please login to add the news.") {
                    window.open(window.location.origin + response, "_self");
                }
                else {
                    alert(response);
                }
            }
        });
    });
    
    //Get Container width
    var conWidth = $('.content-wrap .container').width();

    //Sticky header on scroll
    var stickyOffset = $('.sticky-title').offset().top;

    $(window).scroll(function () {
        var sticky = $('.sticky-title'),
            scroll = $(window).scrollTop();

        if (scroll >= stickyOffset) {
            sticky.addClass('fixed');
            $('.list-head').css('width', conWidth);
        }
        else {
            sticky.removeClass('fixed');
        }
    });
    //Scroll to top
    $(window).scroll(function () {
        if ($(this).scrollTop()) {
            $('.scroll-top').show();
        } else {
            $('.scroll-top').hide();
        }
    });

    $(document).ready(function () {
        $(".scroll-top").click(function () {
            $("html, body").scrollTop(0);
        });
    });
});

function GetNews(title) {
    var baseAddress = $.absoluteurl(window.location.origin + '/news/' + encodeURI(title));
    window.open(baseAddress, '_blank')
}
function getAllNews(softwarename) {
    var baseAddress = $.absoluteurl(window.location.origin + '/software/' + encodeURI(softwarename) + '/news');
    window.open(baseAddress, '_blank')
}
function getAllReviews(softwarename) {
    var baseAddress = $.absoluteurl(window.location.origin + '/software/' + encodeURI(softwarename) + '/reviews');
    window.open(baseAddress, '_blank')
}
function getAllCompanyEmployees(name) {
    var baseAddress = '';
    baseAddress = $.absoluteurl(window.location.origin + '/software/' + encodeURI(name) + '/team-members');
    window.open(baseAddress, '_blank')
}