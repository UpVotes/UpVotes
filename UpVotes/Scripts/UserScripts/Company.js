$(document).ready(function () {

    //Method to display focus areas in horizontal bar.
    $.getCompanyFocus = function (companyFocusData) {
        var companyFocusAreaArray = [];
        for (var i = 0; i < companyFocusData.split(',').length; i++) {
            var a = companyFocusData.split(',')[i].split(':');
            var companyFocusObj = {};
            companyFocusObj["name"] = companyFocusData.split(',')[i].split(':')[0];
            companyFocusObj["data"] = [parseFloat(companyFocusData.split(',')[i].split(':')[1])];
            companyFocusAreaArray.push(companyFocusObj);
        }

        $('#container').highcharts({
            chart: {
                type: 'bar'
            },
            title: {
                text: ''
            },

            legend: {
                reversed: false
            },

            plotOptions: {
                series: {
                    stacking: 'normal'
                },
                bar: {
                    dataLabels: {
                        enabled: true,
                        distance: -50,
                        formatter: function () {
                            var dlabel = Math.round(this.percentage * 100) / 100 + ' %';
                            return dlabel
                        },
                        style: {
                            color: 'white',
                        },
                    },

                },
            },
            series: companyFocusAreaArray
        });
    };

    $('#btnThankNote').click(function () {
        var companyReviewID = $(this).attr('CompanyReviewID');
        var companyID = $('#hdnCompanyID')[0].value;

        $.ajax({
            url: $.absoluteurl('/Company/ThanksNoteForReview'),
            cache: false,
            async: false,
            datatype: 'json',
            data: { companyID: companyID, companyReviewID: companyReviewID },
            type: 'POST',
            success: function (response) {
                response = eval(response);
                alert(response);
                window.location.reload();
            }
        });
    });

});