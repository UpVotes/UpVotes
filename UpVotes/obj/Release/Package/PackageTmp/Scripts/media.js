$( ".menu-breadcrumbs >li" ).click(function() {
   $(this).addClass('activeuphold').siblings().removeClass('activeuphold');
});

$('.progress-bar[data-toggle="tooltip"]').tooltip({
    animated: 'fade',
    placement: 'top'
});