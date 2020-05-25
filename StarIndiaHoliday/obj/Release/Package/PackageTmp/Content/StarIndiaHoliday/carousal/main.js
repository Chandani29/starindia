// JavaScript Document

(function($) {
	
	"use strict";
	
	
	
	
	//Hidden Bar Menu Config
	function hiddenBarMenuConfig() {
		var menuWrap = $('.hidden-bar .side-menu');
		// appending expander button
		menuWrap.find('.dropdown').children('a').append(function () {
			return '<button type="button" class="btn expander"><i class="icon fa fa-bars"></i></button>';
		});
		// hidding submenu 
		menuWrap.find('.dropdown').children('ul').hide();
		// toggling child ul
		menuWrap.find('.btn.expander').each(function () {
			$(this).on('click', function () {
				$(this).parent() // return parent of .btn.expander (a) 
					.parent() // return parent of a (li)
						.children('ul').slideToggle();
	
				// adding class to expander container
				$(this).parent().toggleClass('current');
				// toggling arrow of expander
				$(this).find('i').toggleClass('fa-minus fa-bars');
	
				return false;
	
			});
		});
	}

	
	 
	
	
	/*---------------------------------------
	featured  product, bestseller, carousel
----------------------------------------- */	
	$('.feartured-carousel, .bestseller-carousel').owlCarousel({
		items : 4,
		itemsDesktop : [1199,4],
		itemsDesktopSmall : [991,2],
		itemsTablet: [767,1],
		itemsMobile : [480,1],
		autoPlay :  true,
		stopOnHover: true,		
		navigation: true,
		pagination: false,
		navigationText: ['<img src="/Content/StarIndiaHoliday/images/prevarrow.png">', '<img src="/Content/StarIndiaHoliday/images/nextarrow.png">']
	});	
		
	

	
	/*---------------------------------------
	featured  product, bestseller, carousel
----------------------------------------- */	
	$('.feartured-carouse2').owlCarousel({
	    items: 4,
	    itemsDesktop: [1199, 4],
	    itemsDesktopSmall: [991, 2],
	    itemsTablet: [767, 1],
	    itemsMobile: [480, 1],
	    autoPlay: true,
	    stopOnHover: true,
	    navigation: true,
	    pagination: false,
	    navigationText: ['<img src="/Content/StarIndiaHoliday/images/prevarrow.png">', '<img src="/Content/StarIndiaHoliday/images/nextarrow.png">']
	});

	

	
	/*---------------------------------------
	featured  product, bestseller, carousel
----------------------------------------- */	
	$('.hot-tour-package').owlCarousel({
		items : 2,
		itemsDesktop : [1199,2],
		itemsDesktopSmall : [991,2],
		itemsTablet: [767,1],
		itemsMobile : [480,1],
		autoPlay :  true,
		stopOnHover: false,		
		navigation: true,
		pagination: false,
		navigationText: ['<img src="/Content/StarIndiaHoliday/images/prevarrow2.png">', '<img src="/Content/StarIndiaHoliday/images/nextarrow2.png">']
	});	
		
		
		/*---------------------------------------
	featured  product, bestseller, carousel
----------------------------------------- */	
	$('.testimonial-crousal').owlCarousel({
		items : 3,
		itemsDesktop : [1199,3],
		itemsDesktopSmall : [991,2],
		itemsTablet: [767,1],
		itemsMobile : [480,1],
		autoPlay :  true,
		stopOnHover: false,		
		navigation: false,
		pagination: true,
		navigationText: ['<img src="/Content/StarIndiaHoliday/images/prevarrow2.png">', '<img src="/Content/StarIndiaHoliday/images/nextarrow2.png">']
	});	
		
	

	
/*------------------------------------------------------------------------------*/
/* TopSearch
/*------------------------------------------------------------------------------*/
jQuery( ".ttm-header-search-link a" ).addClass('sclose');   
    
    jQuery( ".ttm-header-search-link a" ).on('click',function(event ) {  
  
                
        if (jQuery('.ttm-header-search-link a').hasClass('sclose')) {   
            jQuery(this).removeClass('sclose').addClass('open');    
            jQuery(".ttm-search-overlay").addClass('st-show'); 
            jQuery(".ttm-search-overlay").slideDown( 400, function() {          
                jQuery(".field.searchform-s").focus();  
            });   
        } else {
            jQuery(this).removeClass('open').addClass('sclose');
            jQuery(".ttm-search-overlay").slideUp( 400, function() {                                
            });
        }   

    jQuery(".ttm-search-close").on('click',function(){
        jQuery('.ttm-header-search-link a').removeClass('st-show').addClass('sclose');
        jQuery(".ttm-search-overlay").slideUp(400,function(){});
    });
});


	
	
	
	
	
	
	

})(window.jQuery);