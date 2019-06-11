$(document).ready(function () {

    $.getData = function (request, response) {
        if ($.trim(request.term) != "" && $.trim(request.term).length > 0) {
            $.ajax({
                url: $.absoluteurl('/SoftwareList/GetDataForAutoComplete'),
                dataType: "json",
                async: false,
                data:
                {
                    featureClass: "leftalign",
                    style: "full",
                    maxRows: 5,
                    starts_with: request.term
                },
                success: function (data) {
                    response(
                        $.map(data,
                            function (item) {
                                return {
                                    label: item.Name,
                                    value: item.Name,
                                }
                            }))
                }
            });
        }
    }

    $("#txtSoftwareSearch").autocomplete({
        source: function (request, response) {
            $.getData(request, response)
        },
        delay: 0,
        focus: function () {
            return false;
        },
        minLength: 3,
        select: function (event, ui) {
            $("#txtSoftwareSearch")[0].value = ui.item.value;
            $("#txtSoftwareSearch").change();
        }
    });

    $("#txtSoftwareReviewSearch").autocomplete({
        source: function (request, response) {
            $.getData(request, response)
        },
        delay: 0,
        focus: function () {
            return false;
        },
        minLength: 3,
        select: function (event, ui) {
            $("#txtSoftwareReviewSearch")[0].value = ui.item.value;
            $("#txtSoftwareReviewSearch").change();
        }
    });



    $("#txtSoftwareSearch").change(function () {
        $.GetSoftwareListBasedOnCriteria(this);
    });

    $("#txtSoftwareReviewSearch").change(function () {
        $.GetSoftwareUserReviewListBasedOnCriteria(this);
    });

    $('#btnLoadMoreReviews').click(function () {
        var pagesize = $('#btnLoadMoreReviews').attr('pagesize');
        $('#btnLoadMoreReviews').attr('pagesize', parseInt(pagesize) + 10);
        $.GetSoftwareUserReviewListBasedOnCriteria(this);
    });

    $.GetSoftwareUserReviewListBasedOnCriteria = function (e) {
        try {
            var softwarename = $("#txtSoftwareReviewSearch").val();
            var pagesize = $('#btnLoadMoreReviews').attr('pagesize');

            $.ajax({
                type: "POST",
                url: $.absoluteurl('/SoftwareList/GetSoftwareUserReviews'),
                cache: false,
                async: false,
                data: { softwarename: softwarename, PageSize: pagesize },
                success: function (json) {
                    $('#divSoftwareUserReviews').html(json);
                    if (softwarename != "") {
                        $('#btnLoadMoreReviews').attr('pagesize', 10);
                    }
                },
                error: function (a, b, c) {
                    debugger;
                }
            });
        }
        catch (e) { debugger; }
    }

    $.GetSoftwareListBasedOnCriteria = function (e) {
        try {            
            
            var sortby = 'Asc';
            var SoftwareCategoryID = parseInt($('#hdnSoftwareCategoryID').val());
            var PageNo = e.className.indexOf('Pagenumber') == -1 ? 1 : parseInt($(e).attr('page'));       
            var PageSize = 10;
            var FirstPage = isNaN(parseInt($('.FirstPageindex').attr('page'))) ? 1 : parseInt($('.FirstPageindex').attr('page'));
            var LastPage = isNaN(parseInt($('.LastPageindex').attr('page'))) ? 1 : parseInt($('.LastPageindex').attr('page'));
            var softwarename = $("#txtSoftwareSearch")[0].value;

            $.ajax({
                type: "POST",
                url: $.absoluteurl('/SoftwareList/SoftwareList'),
                cache: false,
                async: false,
                data: { SoftwareName: softwarename, SoftwareCategoryID: SoftwareCategoryID, sortby: sortby, PageNo: PageNo, PageSize: PageSize, FirstPage: FirstPage, LastPage: LastPage },
                success: function (json) {
                    $('#softwarelist').html(json);
                    $('.lazy').lazy();
                },
                error: function (a, b, c) {
                    debugger;
                }
            });
        }
        catch (e) { debugger; }
    }
});

function GetNews(title) {
    var baseAddress = $.absoluteurl(window.location.origin + '/news/' + encodeURI(title));
    window.open(baseAddress, '_blank')
}
function getAllNews(softwarename) {
    var baseAddress = $.absoluteurl(window.location.origin + '/software/' + encodeURI(softwarename) + '/news');
    window.open(baseAddress, '_blank')
}