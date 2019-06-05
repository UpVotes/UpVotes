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
    $('#btnSubmitReview').click(function () {
        var companyID = $('#hdnCompanyID')[0].value;
        var companyName = $('#hdnCompanyName')[0].value;
        $.ajax({
            url: $.absoluteurl('/Company/SubmitReview'),
            cache: false,
            async: false,
            datatype: 'json',
            data: { companyID: companyID, companyName: companyName },
            type: 'POST',
            success: function (response) {
                if (response != "Please login to submit the review.") {
                    window.open(window.location.origin+response, "_blank");
                }
                else {
                    alert(response);
                }
            }
        });
    });

    $('#btnAddNews').click(function () {        
        $.ajax({
            url: $.absoluteurl('/Company/AddNews'),
            cache: false,
            async: false,
            datatype: 'json',            
            type: 'POST',
            success: function (response) {
                if (response != "Please login to add the news.") {
                    window.open(window.location.origin+response, "_self");
                }
                else {
                    alert(response);
                }
            }
        });
    });

    $('#btnAddTeamMember').click(function ()
    {
        $.ajax({
            url: $.absoluteurl('/Company/AddTeamMember'),
            cache: false,
            async: false,
            datatype: 'json',
            type: 'POST',
            success: function (response)
            {
                if (response != "Please login to add the team members.")
                {
                    window.open(window.location.origin + response, "_self");
                }
                else
                {
                    alert(response);
                }
            }
        });
    });

    $('#btnAddPortfolio').click(function () {
        $.ajax({
            url: $.absoluteurl('/Company/AddPortfolio'),
            cache: false,
            async: false,
            datatype: 'json',
            type: 'POST',
            success: function (response) {
                if (response != "Please login to add the portfolio.") {
                    window.open(window.location.origin + response, "_self");
                }
                else {
                    alert(response);
                }
            }
        });
    });

    $('.btnThankNote').click(function () {
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
                var companyID = $('#hdnCompanyID')[0].value;
                var domain = $('#hdnCompanyDomain')[0].value;
                $.ajax({
                    url: $.absoluteurl('/Company/ClaimListing'),                    
                    data: { companyID: companyID, Email: email, Domain: domain },
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

let modalId = $('#image-gallery');

$(document)
  .ready(function () {

      loadGallery(true, 'a.thumbnail');

      //This function disables buttons when needed
      function disableButtons(counter_max, counter_current) {
          $('#show-previous-image, #show-next-image')
            .show();
          if (counter_max === counter_current) {
              $('#show-next-image')
                .hide();
          } else if (counter_current === 1) {
              $('#show-previous-image')
                .hide();
          }
      }

      function loadGallery(setIDs, setClickAttr) {
          let current_image,
            selector,
            counter = 0;

          $('#show-next-image, #show-previous-image')
            .click(function () {
                if ($(this)
                  .attr('id') === 'show-previous-image') {
                    current_image--;
                } else {
                    current_image++;
                }

                selector = $('[data-image-id="' + current_image + '"]');
                updateGallery(selector);
            });

          function updateGallery(selector) {
              let $sel = selector;
              current_image = $sel.data('image-id');
              $('#image-gallery-title')
                .text($sel.data('title'));
              $('#image-gallery-caption')
                .text($sel.data('caption'));
              $('#image-gallery-image')
                .attr('src', '/images/CompanyPortfolio/'+$sel.data('image'));
              disableButtons(counter, $sel.data('image-id'));
          }

          if (setIDs == true) {
              $('[data-image-id]')
                .each(function () {
                    counter++;
                    $(this)
                      .attr('data-image-id', counter);
                });
          }
          $(setClickAttr)
            .on('click', function () {
                updateGallery($(this));
            });
      }
  });

  // build key actions
  $(document)
    .keydown(function (e) {
        switch (e.which) {
            case 37: // left
                if ((modalId.data('bs.modal') || {})._isShown && $('#show-previous-image').is(":visible")) {
                    $('#show-previous-image')
                      .click();
                }
                break;

            case 39: // right
                if ((modalId.data('bs.modal') || {})._isShown && $('#show-next-image').is(":visible")) {
                    $('#show-next-image')
                      .click();
                }
                break;

            default:
                return; // exit this handler for other keys
        }
        e.preventDefault(); // prevent the default action (scroll / move caret)
    });

  function GetNews(title) {
      var baseAddress = $.absoluteurl(window.location.origin + '/news/' + encodeURI(title));
      window.open(baseAddress, '_blank')
  }
  function getAllNews(companyname) {
      var baseAddress = $.absoluteurl(window.location.origin + '/profile/' + encodeURI(companyname)+'/news');
      window.open(baseAddress, '_blank')
  }
  function getAllReviews(companyname) {
      var baseAddress = $.absoluteurl(window.location.origin + '/profile/' + encodeURI(companyname) + '/reviews');
      window.open(baseAddress, '_blank')
  }
  function getAllPortFolio(companyname) {
      var baseAddress = $.absoluteurl(window.location.origin + '/profile/' + encodeURI(companyname) + '/portfolio');
      window.open(baseAddress, '_blank');
  }
  function getAllCompanyEmployees(name) {
      var baseAddress = '';      
      baseAddress = $.absoluteurl(window.location.origin + '/profile/' + encodeURI(name) + '/team-members');        
      window.open(baseAddress, '_blank')
  }