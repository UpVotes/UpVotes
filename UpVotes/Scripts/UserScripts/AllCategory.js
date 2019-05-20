$(document).ready(function () {

    $(".all-cat .sidebar .choose").click(function () {
        $(".all-cat .sidebar ul").addClass("open");
    });

    $(".tabselect").click(function () {
        $('#titleCategory').text($(this).attr('heading'));
    });

    $(".serviceClick").click(function () {
        $('#UlserviceCategory li').removeClass('active');
        $(this).addClass('active');
        $.ajax({
            url: $.absoluteurl('/Blogs/AllCategories'),
            cache: false,
            async: false,
            datatype: 'json',
            data: { focusAreaID: $(this).attr('focusid') },
            type: 'POST',
            success: function (response) {
                $('#divServiceCategory').html('');
                $('#divServiceCategory').html(response);
            }
        });
    });


    

});
