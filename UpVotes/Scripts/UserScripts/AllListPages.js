$(document).ready(function () {

    $('.btnCompanyThankNote').click(function () {
        var companyReviewID = $(this).attr('CompanyReviewID');
        var companyID = $(this).attr('CompanyID');
        var CompanyName = $(this).attr('ReviewCompanyName');
        var TotThanks = $(this).attr('TotalThanks');
        $.ajax({
            url: $.absoluteurl('/CompanyList/ThanksNoteForReview'),
            cache: false,
            async: false,
            datatype: 'json',
            data: { companyID: companyID, companyReviewID: companyReviewID, compname: CompanyName },
            type: 'POST',
            success: function (response) {
                if (response == "Please login to provide thanks note." || response == "Duplicate thanks note.") {
                    alert(response);
                }
                else {
                    alert(response);
                    $('#spn' + companyReviewID).html(parseInt(TotThanks) + 1);
                }
            }
        });
    });

    $('.btnSoftwareThankNote').click(function () {
        var softwareReviewID = $(this).attr('SoftwareReviewID');
        var softwareID = $(this).attr('SoftwareID');
        var softwareName = $(this).attr('ReviewSoftwareName');
        var TotThanks = $(this).attr('TotalThanks');
        $.ajax({
            url: $.absoluteurl('/SoftwareList/ThanksNoteForReview'),
            cache: false,
            async: false,
            datatype: 'json',
            data: { softwareID: softwareID, softwareReviewID: softwareReviewID, SoftwareName: softwareName },
            type: 'POST',
            success: function (response) {
                if (response == "Please login to provide thanks note." || response == "Duplicate thanks note.") {
                    alert(response);
                }
                else {
                    alert(response);
                    $('#spn' + softwareReviewID).html(parseInt(TotThanks) + 1);
                }
            }
        });
    });

    $('#btnListAddNews').click(function () {
        $.ajax({
            url: $.absoluteurl('/Company/AddNews'),
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

    $('#btnListAddPortfolio').click(function () {
        $.ajax({
            url: $.absoluteurl('/Company/AddPortfolio'),
            cache: false,
            async: false,
            datatype: 'json',
            type: 'POST',
            success: function (response) {
                if (response != "Please login to add the portfolio.") {
                    window.open(window.location.origin +response, "_self");
                }
                else {
                    alert(response);
                }
            }
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

function getSoftwareCompany(name, SoftwareOrCompany) {
    if (SoftwareOrCompany == 1) {
        window.open(window.location.origin + '/profile/' + encodeURI(name), '_blank');
    }
    else {
        window.open(window.location.origin + '/software/' + encodeURI(name), '_blank');
    }
}
function GetNews(title) {
    var baseAddress = $.absoluteurl(window.location.origin + '/news/' + encodeURI(title));
    window.open(baseAddress, '_blank')
}

function getAllReviews(name, SoftwareOrCompany) {
    var baseAddress ='';
    if (SoftwareOrCompany == 1) 
        baseAddress=  $.absoluteurl(window.location.origin + '/profile/' + encodeURI(name) + '/reviews');
    else
        baseAddress=  $.absoluteurl(window.location.origin + '/software/' + encodeURI(name) + '/reviews');
    window.open(baseAddress, '_blank')
}
