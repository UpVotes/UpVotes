$(document).ready(function () {

    //Method to display focus areas in horizontal bar.
    $.getCompanyFocus = function (companyFocusData) {
        // Radialize the colors
        Highcharts.setOptions({
            colors: Highcharts.map(Highcharts.getOptions().colors, function (color) {
                return {
                    radialGradient: {
                        cx: 0.5,
                        cy: 0.3,
                        r: 0.7
                    },
                    stops: [
                        [0, color],
                        [1, Highcharts.Color(color).brighten(-0.3).get('rgb')] // darken
                    ]
                };
            })
        });

        // Build the chart
        Highcharts.chart('container', {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: ''
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        },
                        connectorColor: 'silver'
                    }
                }
            },            
            series: [{
                name: 'Focus',
                data: eval(companyFocusData)
            }]
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