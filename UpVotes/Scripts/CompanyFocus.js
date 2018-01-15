﻿$(function () {
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
                        var dlabel = '';
                        return dlabel
                    },
                    style: {
                        color: 'white',
                    },
                },

            },
        },
        series: [{
            name: 'pear',
            data: [30]
        }, {
            name: 'apple',
            data: [10]
        }, {
            name: 'mango',
            data: [25]
        },
        {
            name: 'sappota',
            data: [5]
        },
        {
            name: 'roseberry',
            data: [8]
        },
        {
            name: 'strawberry',
            data: [12]
        },
        {
            name: 'orange',
            data: [15]
        }, ]
    });
});



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
        text: 'Browser market shares. January, 2015 to May, 2015'
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
        name: 'Brands',
        data: [
            { name: 'IE', y: 35.33 },
            {
                name: 'Chrome',
                y: 24.03,
                sliced: true,
                selected: true
            },
            { name: 'Firefox', y: 10.38 },
            { name: 'Safari', y: 4.77 },
            { name: 'Opera', y: 0.91 },
            { name: 'Other', y: 0.2 },
            { name: 'Other', y: 3.2 },
            { name: 'Other', y: 3.2 },
            { name: 'Other', y: 2.2 },
            { name: 'Other', y: 1.2 },
            { name: 'Other', y: 0.8 },
            { name: 'Other', y: 0.5 },
            { name: 'Other', y: 0.2 }
        ]
    }]
});