$(function () {
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