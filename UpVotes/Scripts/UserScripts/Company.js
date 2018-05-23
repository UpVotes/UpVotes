$(document).ready(function () {

    //Method to display focus areas in horizontal bar.
    $.getCompanyFocus = function (companyFocusData, Focusdivid) {
        var focusareas = new Array();
        for (i = 0; i < companyFocusData.length; i++) { focusareas.push({ 'name': companyFocusData[i].FocusAreaName, 'data': [companyFocusData[i].FocusAreaPercentage] }); }
        
        Highcharts.chart(Focusdivid, {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'bar'
            },
            title: {
                text: ''//'<span style="font-weight: bold;font-family: OpenSans-Bold;">' + FocusName + '</span>',
                //align:'left'
            },
            xAxis: {
                alignTicks:true,
                categories: ['']
            },
            yAxis: {

                gridLineColor: 'transparent',
                //height:50,
                min: 0,
                max: 100,
                title: {
                    text: ''
                }
            },
            legend: {
                layout: 'horizontal',//change to horizontal
                align: 'center',//removed alignment
                //itemDistance:40,
                itemWidth:250,
                alignColumns:false,
                verticalAlign: 'bottom',
                maxHeight: 60,//this was the key property to make my legend paginated
                //y: 5,//remove position
                navigation: {
                    activeColor: '#3E576F',
                    animation: true,
                    arrowSize: 12,
                    inactiveColor: '#CCC',
                    
                    style: {
                        fontWeight: 'bold',
                        color: '#333',
                        fontSize: '12px'
                    }
                }
            },
            plotOptions: {
                series: {
                    stacking: 'normal',
                    pointWidth: 40
                }
            },
            tooltip: {
                pointFormat: '{series.name}<br>Focus: <b>{point.percentage:.1f}%</b>'
            },
            series: focusareas
        });
    };

    $.getSubCompanyFocus = function (subfocusareas, subfocusdivid) {
        Highcharts.chart(subfocusdivid, {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'bar'
            },
            title: {
                text: ''//'<span style="font-weight: bold;font-family: OpenSans-Bold;">' + FocusName + '</span>',
                //align:'left'
            },
            xAxis: {
                alignTicks: true,
                categories: ['']
            },
            yAxis: {

                gridLineColor: 'transparent',
                //height:50,
                min: 0,
                max: 100,
                title: {
                    text: ''
                }
            },
            legend: {
                layout: 'horizontal',//change to horizontal
                align: 'center',//removed alignment
                //itemDistance:40,
                itemWidth: 250,
                alignColumns: false,
                verticalAlign: 'bottom',
                maxHeight: 60,//this was the key property to make my legend paginated
                //y: 5,//remove position
                navigation: {
                    activeColor: '#3E576F',
                    animation: true,
                    arrowSize: 12,
                    inactiveColor: '#CCC',

                    style: {
                        fontWeight: 'bold',
                        color: '#333',
                        fontSize: '12px'
                    }
                }
            },
            plotOptions: {
                series: {
                    stacking: 'normal',
                    pointWidth: 40
                }
            },
            tooltip: {
                pointFormat: '{series.name}<br>Focus: <b>{point.percentage:.1f}%</b>'
            },
            series: subfocusareas
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