/*
	Name: Advanced Slider
	Version: 3.0
*/
(function($) {
	
	function AdvancedSlider(instance, options) {
		
		// holds all the slider's settings
		this.settings = $.extend({}, $.fn.advancedSlider.defaults, options);
		
		var VERSION = '3.0',
			
			// reference to the main DIV that contains the slider
			slider = $(instance),
			
			// reference to the current object
			self = this,			
			
			// index of the current slide
			currentIndex = -1,
			
			// index of the previous slide
			previousIndex = -1,
			
			// the index of the slide for which the caption is currently displayed
			activeCaptionIndex = -1,
			
			// array of objects, each object containing data about the slide
			slidesData = [],
			
			// array of jQuery slide objects
			slideDivs = [],
			
			// total number of slides
			numSlides = 0,
			
			// index of the initially opened slide
			slideStart = 0,
			
			// reference to the DIV element of the current slide
			sliceContainer = null,
			
			// reference to the DIV element of the previous slide
			previousSlideDiv = null,
			
			// indicates whether the slider is in the transition phase
			isTransition = false,
			
			// indicates if the mouse is over the slider
			isHover = false,
			
			// indicates whether a lightbox was set for the currently visible slide
			isLightbox = false,
			
			// indicates whether the lightbox is currently displayed
			isLightboxOpen = false,
			
			// indicates whether a caption exists for the current slide
			isCaption = false,
			
			// indicates whether there is a video playing inside the slide
			isVideoPlaying = false,
			
			// indicates whther the slideshow was manually paused by clicking on the timer animation
			slideshowManuallyPaused = false,
			
			// will be used as the timer for the slideshow mode
			slideshowTimer = 0,		
			
			// will be used for the timer animation
			timerAnimationTimer = 0,
			
			// indicated whether the slideshow is playing or is paused
			slideshowState,
			
			// indicates for how much time has the slideshow been running
			slideshowTimerPosition = 0,
			
			// indicates what was the time when the slideshow has started
			slideshowStartTime = 0,
			
			// the total number of thumbnails
			numThumbnails = 0,
			
			// the number of visible thumbnails
			visibleThumbnails = 0,
			
			// the number of previous visible thumbnails
			previousVisibleThumbnails = 0,
			
			// the total width/height of the thumbnails
			thumbnailsTotalSize,
			
			// reference to the main container, which contains the thumbnails, the arrows, buttons, scrollbar etc.
			thumbnailScroller,
			
			// reference to the container which masks the actual thumbnails
			thumbnailsVisibleContainer,
			
			// reference to the container which contains the thumbnails
			thumbnailsContainer,
			
			// the width of an individual thumbnail
			thumbnailWidth,
			
			// the height of an individual thumbnail
			thumbnailHeight,
			
			// reference to the thumbnail scroller arrows
			thumbnailScrollerArrows,
			
			// reference to the thumbnail scroller buttons
			thumbnailScrollerButtons,
			
			// reference to the thumbnail scroller scrollbar
			thumbnailScrollerScrollbar,
			
			// reference to the thumbnail scroller scrollbar track
			thumbnailScrollerScrollbarTrack,
			
			// reference to the thumbnail scroller middle area
			thumbnailScrollerScrollbarMiddle,
			
			// the total size of the scrollbar's edges
			thumbnailScrollerScrollbarEdgesSize,
			
			// the thumbnail page that is currently selected
			currentThumbnailPage = 0,
			
			// total number of thumbnail pages
			totalThumbnailPages = 0,
			
			// indicates the number of thumbnail layers
			thumbnailLayers = 0,
			
			// reference to the timer used for mouse scrolling
			thumbnailMouseScrollTimer,
			
			// reference to the timer used for mouse wheel scrolling
			thumbnailMouseWheelTimer,
			
			// indicates whether the carousel is currently scrolled using the mouse wheel method
			isThumbnailMouseWheelScrolling = false,
			
			// reference to the timer used for scrollbar scrolling
			thumbnailScrollbarTimer,
			
			// indicates whether the scrollbar's thumb is being dragged
			isThumbnailScrollbarDragging = false,
			
			// indicates whether the scrollbar is currently moving
			isThumbnailScrollbarMoving = false,			
			
			// indicates the horizotal position of the mouse pointer
			mouseX,
			
			// indicates the vertical position of the mouse pointer
			mouseY,
			
			// indicates whether the current transition is the first one
			firstTransition = true,
			
			// indicates what object requested a new slide
			triggerType = '',
			
			// reference to the timer used for the caption
			captionDelayTimer = 0,
			
			// reference to the current video
			currentVideo = null,
			
			// the top container of the slides (doesn't contain the thumbnail scroller)
			sliderMain,
			
			// will wrap the slides
			slideWrapper,
			
			// container for hte slides
			slides,
			
			// holds the wrapper's width (used when the scale type is set to proportionalFit)
			slideWrapperWidth,
			
			// holds the wrapper's height (used when the scale type is set to proportionalFit)
			slideWrapperHeight,
			
			// holds the actual height of the main slide (used when the scale type is set to proportionalFit)
			sliderMainHeight,
			
			// the extra height induced by the presence of the 3D shadow and navigation buttons
			extraHeight = 0,
			
			// used to indicate how many slices were animated when the slice effect is used
			sliceCounter,
			
			// indicates the current transition effect (used when a transition is overridden)
			currentEffectType,
			
			// indicates whether IE8 or an older version of IE is used
			isOldIE = ($.browser.msie && parseInt($.browser.version) < 9),
			
			// indicates whether the slider has focus
			isFocus = false,
			
			// these properties can be assign to individual slides in the XML file
			slideProps = ['alignType', 'effectType', 'sliceEffectType', 'sliceDelay', 'sliceDuration', 'sliceEasing', 'horizontalSlices', 'verticalSlices', 
						  'slicePattern', 'slicePoint', 'sliceStartPosition', 'sliceStartRatio', 'sliceFade', 'captionSize', 'captionPosition', 
						  'captionShowEffectDuration', 'captionShowEffectEasing', 'captionHideEffectDuration', 'captionHideEffectEasing', 'captionShowEffect', 'captionHideEffect', 
						  'captionLeft', 'captionTop', 'captionWidth', 'captionHeight', 'captionShowSlideDirection', 'captionHideSlideDirection', 'captionBackgroundColor', 
						  'captionBackgroundOpacity', 'slideshowDelay', 'slideMask', 'slideDirection', 'slideDuration', 'slideEasing', 'slideLoop', 'fadeInDuration', 
						  'fadeOutDuration', 'fadeInEasing', 'fadeOutEasing','fadePreviousSlide', 'fadePreviousSlideDuration', 'thumbnailType', 'captionDelay', 'htmlDuringTransition'];
		
		// START
		parseData();
		

		/**
		* Initializes the slider
		*/
		function parseData() {
			// delete the content of the selected DIV and initialize it
			if (self.settings.xmlSource) {				
				// delete the content of the selected DIV
				slider.empty();
				
				//parse the XML file
				$.ajax({type: 'GET', 
						url: self.settings.xmlSource, 
						dataType:  $.browser.msie ? 'text' : 'xml', 
						success: function(data) {
							var xml,
								xmlSlider;
							
							if ($.browser.msie) {
								xml = new ActiveXObject('Microsoft.XMLDOM');
								xml.async = false;
								xml.loadXML(data);
							} else {
								xml = data;
							}
							
							// create the HTML markup based on data from the XML file
							slides = $('<ul class="slides"></ul>').appendTo(slider);
							xmlSlider = $(xml).find('slider');
							
							// find all the <slide> nodes
							$(xml).find('slide').each(function(index) {
								
								var slide = $('<li class="slide"></li>').appendTo(slides);								
								
								var xmlSlide = $(this),
									xmlImage = xmlSlide.find('image'),
									xmlThumbnail = xmlSlide.find('thumbnail'),
									xmlCaption = xmlSlide.find('caption'),
									xmlHtml = xmlSlide.find('html'),
									xmlLink = xmlSlide.find('link'),
									xmlThumbnailLink = xmlSlide.find('thumbnailLink'),
									isImage = xmlImage.length || xmlSlide.attr('data-image'),
									link,
									thumbnailLink;
								
								
								if (xmlSlide.attr('data-image'))
									slide.attr('data-image', xmlSlide.attr('data-image'));
								
								
								if (isImage && xmlLink.length)
									link = $('<a></a>').attr('href', xmlLink.text())
													   .attr('title', xmlLink.attr('title') ? xmlLink.attr('title') : '')
													   .attr('target', xmlLink.attr('target') ? xmlLink.attr('target') : '_self')
													   .attr('rel', (xmlLink.attr('type') == 'slider-lightbox' ? xmlLink.attr('type') : '') + 
													   				(xmlLink.attr('group') ? '[' + xmlLink.attr('group') + ']' : ''))
													   .appendTo(slide);
								
								
								if (xmlImage.length)
									var image = $('<img/>').attr('src', xmlImage.text())
														   .attr('class', 'image')
														   .attr('alt', xmlImage.attr('alt') ? xmlImage.attr('alt') : '')
														   .attr('title', xmlImage.attr('title') ? xmlImage.attr('title') : '')
														   .appendTo(link ? link : slide);
								
								
								if (xmlThumbnail.length && xmlThumbnailLink.length)
									var thumbnailLink = $('<a></a>').attr('href', xmlThumbnailLink.text())
														   			.attr('title', xmlThumbnailLink.attr('title') ? xmlThumbnailLink.attr('title') : '')
														   			.attr('target', xmlThumbnailLink.attr('target') ? xmlThumbnailLink.attr('target') : '_self')
														   			.attr('rel', (xmlThumbnailLink.attr('type') == 'slider-lightbox' ? xmlThumbnailLink.attr('type') : '') + 
																				 (xmlThumbnailLink.attr('group') ? '[' + xmlThumbnailLink.attr('group') + ']' : ''))
														   			.appendTo(slide);
														   
														   
								if (xmlThumbnail.length)
									var thumbnail = $('<img/>').attr('src', xmlThumbnail.text())
														   	   .attr('class', 'thumbnail')
														   	   .attr('alt', xmlThumbnail.attr('alt') ? xmlThumbnail.attr('alt') : '')
														   	   .attr('title', xmlThumbnail.attr('title') ? xmlThumbnail.attr('title') : '')
														  	   .appendTo(thumbnailLink ? thumbnailLink : slide);

								
								if (xmlCaption.length)
									var caption = $('<div></div>').html(xmlCaption.text())
														   	   	  .attr('class', 'caption')
														  	   	  .appendTo((xmlCaption.attr('parent') == 'link' && link) ? link : slide);
								
								
								if (xmlHtml.length)
									var html = $('<div></div>').html(xmlHtml.text())
														   	   .attr('class', 'html')
														  	   .appendTo(slide);
								
								slide.properties = {};
								
								// reads all the attributes that were specified for a slide in the XML file
								for (var i = 0; i < slideProps.length; i++) {
									var name = slideProps[i];
																		
									if (options.slideProperties && options.slideProperties[index] && options.slideProperties[index][name])
										slide.properties[name] = options.slideProperties[index][name];
									else if ($(this).attr(name))
										slide.properties[name] = $(this).attr(name);
									else if (options[name])
										slide.properties[name] = options[name];
									else if (xmlSlider.attr(name))
										slide.properties[name] = xmlSlider.attr(name);
									else
										slide.properties[name] = $.fn.advancedSlider.defaults[name];
								}
								
								slidesData.push(slide);
							});
							
							parseHTML();
							
							// fire the 'xmlLoaded' event
							var eventObject = {type: 'xmlLoaded', data: xml};
							$.isFunction(self.settings.xmlLoaded) && self.settings.xmlLoaded.call(this, eventObject);
						}
				});
			} else {
				parseHTML();
			}
		}
		
		
		function parseHTML() {
			slides = slider.find('.slides');
							
			// shuffle the slides
			if (self.settings.shuffle) {
				var currentSlides = slides.children();
				
				var shuffledSlides = currentSlides.sort(function() {
					return 0.5 - Math.random();
				});				
				
				slides.empty()
					  .append(shuffledSlides);
			}
			
			
			// if an XML file was not specified, read the content of the selected div
			slides.children().each(function(index) {					  
				// will contain data such as path, thumbnail, caption, or link
				var slide = {};
				
				// will contain data such as effectType, sliceDelay, sliceDuration etc.
				slide.properties = {};
				
				slideDivs.push($(this));
				
				if ($(this).data('image')) {
					slide['lazyImage'] = $(this).data('image');
					slide['lazyImageLoaded'] = false;
				}
				
				// loops through all the sub-children of child
				for (var i = 0; i < $(this).children().length; i++) {
					var data = $(this).children()[i];
					
					if (($(data).hasClass('thumbnail')) || ($(data).is('a') && $(data).children('img.thumbnail').length)) {
						slide.thumbnail = $(data).clone();
					} else {
						slide[$(data).attr('class')] = $(data);
					}
				}
				
				// the caption and HTML might be wrapped within the anchor
				// so check for this possibility 
				if (!slide.caption && $(this).find('.caption').length)
					slide.caption = $(this).find('.caption');
					
				if (!slide.html && $(this).find('.html').length)
					slide.html = $(this).find('.html');
				
				// reads all the settings that were specified for each slide
				if (self.settings.xmlSource) {
					slide.properties = slidesData[index].properties
					slidesData[index] = slide;
				} else {
					for (var i = 0; i < slideProps.length; i++) {
						var name = slideProps[i],
							value;
						
						if (self.settings.slideProperties)
							if (self.settings.slideProperties[index])
								value = self.settings.slideProperties[index][name];
								
						// if a property was not specified for the slide, take the default value
						if (!slide.properties[name])
							if (value == undefined)
								slide.properties[name] = self.settings[name];
							else
								slide.properties[name] = value;
					}
					
					slidesData.push(slide);
				}
			});
			
			//remove the thumbnails
			slides.find('img.thumbnail').parent('a').remove();			
			slides.find('img.thumbnail').remove();
			
			numSlides = slidesData.length;
			create();
		}
		
		
		/**
		* Creates all the assets, preloads the slides and opens the first slide
		*/
		function create() {
			slider.addClass(self.settings.skin)
				  .css({width: self.settings.width, height: self.settings.height})
				  .focus(function() {
					  isFocus = true;
					  
					  slider.addClass('focus');
				  })
				  .blur(function() {
					  isFocus = false;
					  
					  if (slider.hasClass('focus'))
					      slider.removeClass('focus');
				  });
				  
				  
			sliderMain = $('<div class="slider-main"></div>').appendTo(slider)					
				
			if ($.browser.mozilla)
				slider.addClass('mozilla');
			else if ($.browser.webkit)
				slider.addClass('webkit');
			else if ($.browser.opera)
				slider.addClass('opera');
			else if ($.browser.msie)
				if (parseInt($.browser.version) == 6)
					slider.addClass('ie6');
				else if (parseInt($.browser.version) == 7)
					slider.addClass('ie7');
				else if (parseInt($.browser.version) == 8)
					slider.addClass('ie8');
				else if (parseInt($.browser.version) == 9)
					slider.addClass('ie9');
			
			
			slideWrapper = $('<div class="slide-wrapper"></div>')
				.append(slides)
				.appendTo(sliderMain)
				.hover(function() {
						isHover = true;
						
						
						// show the timer animation on mouse over
						if (self.settings.timerAnimation && self.settings.timerToggle && slideshowState != 'stopped' && !isTransition) {
							var timerCanvas = slider.find('.timer-animation');
							
							if (isOldIE)
								timerCanvas.css('filter', '');
							else
								timerCanvas.stop().animate({'opacity': 1}, self.settings.timerFadeDuration);									
						}
						
						
						// show the lightbox icon on mouse over
						if (isLightbox && self.settings.lightboxIconToggle) {
							var lightboxIcon = slideWrapper.find('.lightbox-icon');
							
							if (isOldIE)
								lightboxIcon.css('filter', '');
							else
								lightboxIcon.stop().animate({'opacity': 1}, self.settings.lightboxIconFadeDuration);									
						}
						
						
						// pause the slideshow on mouse over
						if (self.settings.pauseSlideshowOnHover && !slideshowManuallyPaused)
							pauseSlideshow();
							
							
						// show the caption on mouse over
						if (isCaption && self.settings.captionToggle)
							showCaption();
							
							
						// show the navigation arrows on mouse over	
						if (self.settings.slideArrows && self.settings.slideArrowsToggle)
							if (isOldIE)
								slider.find('.slide-arrows a').stop().show();
							else
								slider.find('.slide-arrows a').stop().animate({opacity: 1}, self.settings.slideArrowsShowDuration);
								
								
						// show the slideshow button on mouse over
						if (self.settings.slideshowControls && self.settings.slideshowControlsToggle)
							if (isOldIE)
								slider.find('.slideshow-controls').stop().show();
							else
								slider.find('.slideshow-controls').stop().animate({opacity: 1}, self.settings.slideshowControlsShowDuration);
						},
					
					function() {
						isHover = false;
						
						
						// hide the timer animation on mouse out 
						if (self.settings.timerAnimation && self.settings.timerToggle && slideshowState != 'stopped' && !isTransition) {
							var timerCanvas = slider.find('.timer-animation');
									
							if (isOldIE)
								timerCanvas.css('opacity', 0);
							else
								timerCanvas.stop().animate({'opacity': 0}, self.settings.timerFadeDuration);
						}
						
						
						// hide the lightbox icon on mouse out
						if (isLightbox && self.settings.lightboxIconToggle) {
							var lightboxIcon = slideWrapper.find('.lightbox-icon');
							
							if (isOldIE)
								lightboxIcon.css('opacity', 0);
							else
								lightboxIcon.stop().animate({'opacity': 0}, self.settings.lightboxIconFadeDuration);									
						}
						
						
						// resume the slideshow on mouse out
						if (self.settings.pauseSlideshowOnHover && !isLightboxOpen && !isVideoPlaying && !slideshowManuallyPaused)
							resumeSlideshow();
						
						
						// hide the caption on mouse out	
						if (isCaption && self.settings.captionToggle)
							hideCaption();
							
							
						// hide the navigation arrows on mouse out	
						if (self.settings.slideArrows && self.settings.slideArrowsToggle)
							if (isOldIE)
								slider.find('.slide-arrows a').stop().hide();
							else
								slider.find('.slide-arrows a').stop().animate({opacity: 0}, self.settings.slideArrowsHideDuration);
						
						
						// hide the slideshow button on mouse out		
						if (self.settings.slideshowControls && self.settings.slideshowControlsToggle)
							if (isOldIE)
								slider.find('.slideshow-controls').stop().hide();
							else
								slider.find('.slideshow-controls').stop().animate({opacity: 0}, self.settings.slideshowControlsHideDuration);
					});
			
			
			if (self.settings.effectType == 'swipe')
				$('<div class="swipeable-container"></div>').appendTo(slideWrapper)
															.append(slides);
															
															
			// assign the imagePath and create the captions
			slides.find('.slide').each(function(index) {
				var slide = $(this),
					image = slide.find('img.image'),
					caption = slide.find('.caption'),
					slideData = slidesData[index],
					properties = slideData.properties;
				
				
				// give the image and index and get the path of the image
				if (image.length) {
					image.data('index', index);
					slideData.imagePath = image.attr('src');
				}
				
				
				// create the caption
				if (caption.length) {
					var	captionBackgroundOpacity = parseFloat(properties.captionBackgroundOpacity),
						captionBackgroundColor = properties.captionBackgroundColor;
				
					var captionContainer = $('<div class="caption-container"></div>').appendTo(caption.parent()),
						captionWrapper = $('<div class="wrapper"></div>').appendTo(captionContainer),
						captionBackground = $('<div class="background"></div>').css({'opacity': captionBackgroundOpacity, 
																					 'background-color': captionBackgroundColor})
																			   .appendTo(captionWrapper);
					
					caption.appendTo(captionWrapper);
				}
			});
			
			
			// loop through all HTML5 videos and if the device uses iOS, recreate the video (workaround for webkit bug)
			// instantiate the VideoJS objects if the video has the 'video-js' class
			slides.find('.html video.video').each(function() {
				var video = $(this),
					videoParent = video.parent();
				
				var isIOS = (navigator.userAgent.match(/iPad/i) != null) || 
							(navigator.userAgent.match(/iPod/i) != null) || 
							(navigator.userAgent.match(/iPhone/i) != null);
							  
				
				if (isIOS) {					
					var videoString = video[0].outerHTML;
						
					video.remove();
					
					videoParent.html(videoString);
					video = videoParent.find('video');
					video[0].load();
				}
				
				
				if (video.hasClass('video-js'))
					VideoJS(video.attr('id'), video.data('video'));
			});
			
			
			if (slides.find('.html video.sublime-video').length && window.sublimevideo) {
				sublimevideo.ready(function() {
					slides.find('.html video.sublime-video').each(function() {
						sublimevideo.prepare($(this).attr('id'));
					});
				});
			}
			
			
			slideStart = self.settings.slideStart;
			
			var hash = window.location.hash;
			
			if (hash) {
				hash = hash.substring(1);
				
				var values = hash.split('-'),
					slideIndex = parseInt(values.pop()),
					sliderId = hash.replace('-' + slideIndex, '');
				
				if (slider.attr('id') == sliderId && slideIndex >= 0 && slideIndex <= numSlides - 1)
					slideStart = slideIndex;
			}
			
			
			if (self.settings.slideArrows)
				createSlideArrows();
				
			if (self.settings.slideButtons)
				createSlideButtons();
				
			if (self.settings.shadow)
				createShadow();
				
			if (self.settings.thumbnailType == 'scroller' || self.settings.thumbnailType == 'tooltipAndScroller')
				createThumbnailScroller();
						
			if (self.settings.slideshowControls)
				createSlideshowControls();
				
			if (self.settings.keyboardNavigation)
				createKeyboardNavigation();
			
			if (self.settings.lightbox) {
				slider.find("a[rel^='slider-lightbox']").prettyPhoto({social_tools: '', 
																      deeplinking: false,
																      overlay_gallery: false,
																      theme: self.settings.lightboxTheme,
																      opacity: self.settings.lightboxOpacity,
																	  openLightbox: function() {
																		  if (!isLightboxOpen) {
																			  isLightboxOpen = true;
																			  slider.blur();
																			  
																			  if (!slideshowManuallyPaused)
																			  	  pauseSlideshow();																			  
																		  }
																	  },
																	  callback: function() {
																		  if (isLightboxOpen) {
																			  isLightboxOpen = false;
																			  slider.focus();
																			  
																			  if (!slideshowManuallyPaused)
																			  	  resumeSlideshow();
																		  }
																	  }
														});
			}
			
			triggerType = 'none';
			
			doSliderLayout();
			
			gotoSlide(slideStart);
			
			$(window).resize(doSliderLayout);
		}
		
		
		/**
		* Resize the slider on load and everytime the window is resized
		*/
		function doSliderLayout() {
			var heightToExtract = 0,
				widthToExtract = 0;
			
			
			// if the thumbnail scroller is used, calculate the slides size so that it excludes the size of the slider
			if (self.settings.thumbnailType == 'scroller' || self.settings.thumbnailType == 'tooltipAndScroller') {
				if (self.settings.thumbnailOrientation == 'horizontal')
					sliderMain.css('height', slider.height() - thumbnailScroller.outerHeight(true));
				else if (self.settings.thumbnailOrientation == 'vertical')
					sliderMain.css('width', slider.width() - thumbnailScroller.outerWidth(true));
			}
			
			
			// extract the size of the shadow and hide it if the slides have less than 400px width
			if (self.settings.shadow) {
				var shadow = sliderMain.find('.shadow');
				
				if (sliderMain.width() >= 400) {
					shadow.css('display', 'block');
					
					var	shadowMiddle = shadow.find('.shadow-middle'),
						shadowMarginsWidth = shadow.find('.shadow-left').width() + shadow.find('.shadow-right').width(),					
						shadowExtra = parseInt(shadow.css('height')) + parseInt(shadow.css('margin-top'));				
					
					shadow.css('width', sliderMain.width());			
					shadowMiddle.css('width', shadow.width() - shadowMarginsWidth);
					heightToExtract = shadowExtra > heightToExtract ? shadowExtra : heightToExtract;
				} else {
					shadow.css('display', 'none');
				}
			}
			
			
			// center the slide buttons and extract their size
			if (self.settings.slideButtons) {
				var slideButtons = sliderMain.find('.slide-buttons'),
					buttonsMiddle = slideButtons.find('buttons-middle'),
					buttonsInner = buttonsMiddle.find('buttons-inner'),
					buttonsExtra = parseInt(slideButtons.css('height')) + parseInt(slideButtons.css('margin-top'));
				
				// if true, the buttons will be horizontally positioned in the middle of the slide
				// if false, the container will be posistioned based on the value of the 'left' property specified in the CSS
				if (self.settings.slideButtonsContainerCenter) {
					var leftPos = (sliderMain.width() - slideButtons.width()) / 2;
					slideButtons.css('left', leftPos);
				}				
				
				// if true, the inner buttons will be horizontally positioned in the middle of the container
				// if false, the inner buttons will be posistioned based on the value of the 'left' property specified in the CSS
				if (self.settings.slideButtonsCenter) {
					var leftPos = (buttonsMiddle.width() - buttonsInner.width()) / 2;
					buttonsInner.css('left', leftPos);
				}				
					
				heightToExtract = buttonsExtra > heightToExtract ? buttonsExtra : heightToExtract;
			}
			
			
			extraHeight = heightToExtract;
			
			
			// extract the width of the border
			heightToExtract += parseInt(slideWrapper.css('border-top-width')) + parseInt(slideWrapper.css('border-bottom-width'));
			widthToExtract += parseInt(slideWrapper.css('border-left-width')) + parseInt(slideWrapper.css('border-right-width'));
			
			
			// assign the resulted width and height to the slider
			slideWrapper.css({'height': sliderMain.height() - heightToExtract, 'width': sliderMain.width() - widthToExtract});
			
			
			// keeps a reference to the slide size 
			// these are used when scale type is proportional fit
			slideWrapperWidth = slideWrapper.width();
			slideWrapperHeight = slideWrapper.height();
			
			
			// loop through all images and modify their size and position
			// based on the align and scale types
			slides.find('.slide').each(function(index) {
				var slide = $(this),
					image = slide.find('img.image'),
					alignType = slidesData[index].properties.alignType;
				
				if (image.length) {
					if (image[0].complete || image[0].readyState == 'complete') {
						scaleAndAlign(image, alignType);
					} else {
						image.css('visibility', 'hidden');
						image.load(function() {
							image.css('visibility', '');
							scaleAndAlign(image, alignType);
						});
					}
				}
				
				
				// if the swipe effect is used, rearrange the slides	
				if (self.settings.effectType == 'swipe') {
					var slideIndex = currentIndex != -1 ? currentIndex : slideStart;
				  
					if (self.settings.swipeOrientation == 'horizontal') {
						slides.css('left', - slideIndex * (slideWrapper.width() + self.settings.swipeSlideDistance));
						slide.css('left', index * (slideWrapper.width()  + self.settings.swipeSlideDistance));
					} else if (self.settings.swipeOrientation == 'vertical') {
						slides.css('top', - slideIndex * (slideWrapper.height() + self.settings.swipeSlideDistance));
						slide.css('top', index * (slideWrapper.height() + self.settings.swipeSlideDistance));
					}
				}
			});
			
			
			// reset the thumbnail scrolling
			if ((self.settings.thumbnailType == 'tooltipAndScroller' || self.settings.thumbnailType == 'scroller') && self.settings.scaleType != 'proportionalFit' ) {
				doThumbnailScrollerLayout();
				
				clearThumbnailScrollingTimers();
			
				// restart the mouse scrolling
				if (self.settings.thumbnailMouseScroll)
					startThumbnailMouseScroll();
			}
			
			
			// fire the 'doSliderLayout' event
			var eventObject = {type: 'doSliderLayout'};
			$.isFunction(self.settings.doSliderLayout) && self.settings.doSliderLayout.call(this, eventObject);
		}
		
		
		/**
		* scale and align the specified image
		*/
		function scaleAndAlign(image, alignType) {			
			if (!image.data('init')) {
				image.data('init', true);
				image.data('width', image.width());
				image.data('height', image.height());
				
				image.data('state', 'loaded');
				
				if (!self.settings.allowScaleUp)
					image.css({maxWidth: image.data('width'), maxHeight: image.data('height')});
			}			
			
			
			switch (self.settings.scaleType) {
				case 'exactFit':
					image.css({width: '100%', height: '100%'});
					break;
					
				case 'insideFit':
					if (image.width() / image.height() >= slideWrapper.width() / slideWrapper.height())
						image.css({width: '100%', height: 'auto'});
					else
						image.css({width: 'auto', height: '100%'});	
					break;
					
				case 'outsideFit':
					if (image.width() / image.height() <= slideWrapper.width() / slideWrapper.height())
						image.css({width: '100%', height: 'auto'});
					else
						image.css({width: 'auto', height: '100%'});
					break;
					
				case 'proportionalFit':
					if (image.data('index') == currentIndex && (image[0].complete || image[0].readyState == 'complete')) {
						slideWrapper.css('width', slideWrapperWidth);
						slideWrapper.css('height', slideWrapperHeight);
						
						
						image.css({width: '100%', height: 'auto'});
						slideWrapper.css('height', image.height());
						sliderMain.css('height', slideWrapper.outerHeight(true) + extraHeight);
						
						
						if (self.settings.thumbnailType == 'scroller' || self.settings.thumbnailType == 'tooltipAndScroller') {
							if (self.settings.thumbnailOrientation == 'horizontal')
								slider.css('height', sliderMain.height() + thumbnailScroller.outerHeight(true));
							else
								slider.css('height', sliderMain.height());
									
							doThumbnailScrollerLayout();
							
							clearThumbnailScrollingTimers();
			
							// restart the mouse scrolling
							if (self.settings.thumbnailMouseScroll)
								startThumbnailMouseScroll();
						} else {
							slider.css('height', sliderMain.height());
						}
						
						
						// align the previous image, after the container has changed the size, in order to look aligned during transition
						if (previousIndex != -1) {
							var previousImage = slideDivs[previousIndex].find('img.image'),
								previousAlignType = slidesData[previousIndex].properties.alignType;
							
							if (previousImage.length)
								align(previousImage, previousAlignType);
						}
					}
					break;
			}
			
			// align the current image
			align(image, alignType);
		}
		
		
		/**
		* align the specified image based on the specified align type
		*/
		function align(image, alignType) {
			var imageWidth = image.width(),
				imageHeight = image.height();
				
			switch (alignType) {
				case 'leftTop':
					image.css({'left': 0, 'top': 0});
					break;
					
				case 'leftCenter':
					image.css({'left': 0, 'top': '50%', 'margin-top': - imageHeight * 0.5});
					break;
				
				case 'leftBottom':							
					image.css({'left': 0, 'bottom': 0});
					break;
				
				case 'centerTop':							
					image.css({'left': '50%', 'top': 0, 'margin-left': - imageWidth * 0.5});
					break;
				
				case 'centerCenter':							
					image.css({'left': '50%', 'top': '50%', 'margin-left': - imageWidth * 0.5, 'margin-top': - imageHeight * 0.5});
					break;
				
				case 'centerBottom':							
					image.css({'left': '50%', 'bottom': 0, 'margin-left': - imageWidth * 0.5});
					break;
					
				case 'rightTop':							
					image.css({'right': 0, 'top': 0});
					break;
					
				case 'rightCenter':							
					image.css({'right': 0, 'top': '50%', 'margin-top': - imageHeight * 0.5});
					break;
					
				case 'rightBottom':							
					image.css({'right': 0, 'bottom': 0});
					break;
					
				case 'default':							
					image.css({'left': '50%', 'top': '50%', 'margin-left': - imageWidth * 0.5, 'margin-top': - imageHeight * 0.5});
			}
		}
		
		
		/**
		* Creates the left and right arrows
		*/
		function createSlideArrows() {
			var	slideArrows = $('<div class="slide-arrows"></div>').appendTo(slideWrapper),
			
				previousArrow = $('<a class="previous"></a>')
					.appendTo(slideArrows)
					.click(function() {
						triggerType = 'previousButton'; 
						previousSlide(); 
					}),
			
				nextArrow = $('<a class="next"></a>')
					.appendTo(slideArrows)
					.click(function() {
						triggerType = 'nextButton'; 
						nextSlide(); 
					});
													   
			
			if (self.settings.slideArrowsToggle) {
				if (isOldIE) {
					previousArrow.hide();
					nextArrow.hide();
				} else {
					previousArrow.css('opacity', 0);
					nextArrow.css('opacity', 0);
				}
			}
		}
		
		
		/**
		* Creates the slideshow control (play and pause) buttons
		*/
		function createSlideshowControls() {			
			var slideshowControls = $('<div class="slideshow-controls"></div>').appendTo(slideWrapper),
				currentClass = self.settings.slideshow ? 'pause' : 'play';
			
			slideshowState = self.settings.slideshow ? 'playing' : 'stopped';
				
			slideshowControls
				.addClass(currentClass)
				.click(function() {					
					toggleSlideshow();
				});
			
			
			if (self.settings.slideshowControlsToggle)
				if (isOldIE)
					slideshowControls.hide();
				else
					slideshowControls.css('opacity', 0);
		}
		
		
		/**
		* Automatically creates the navigation buttons based on how many slides are in the slideshow
		*/
		function createSlideButtons() {
			var numButtons = numSlides,
				slideButtons = $('<div class="slide-buttons"></div>').appendTo(sliderMain),
				buttonsLeft = $('<div class="left"></div>').appendTo(slideButtons),
				buttonsMiddle = $('<div class="middle"></div>').appendTo(slideButtons),
				buttonsRight = $('<div class="right"></div>').appendTo(slideButtons),
				buttonsInner = $('<div class="buttons-inner"></div>').appendTo(buttonsMiddle);
			
			for (var i = 0; i < numButtons; i++) {
				var button = $('<a rel="' + i + '"></a>').appendTo(buttonsInner);
				
				if (self.settings.slideButtonsNumber)
					var number = $('<div class="number">' + (i + 1) + '</div>').appendTo(button);
				
				
				button.hover(function() {
					var index = parseInt($(this).attr('rel'));
					
					if (!$(this).hasClass('select'))
						$(this).addClass('over');						
					
					if (slidesData[index].properties.thumbnailType == 'tooltip' || slidesData[index].properties.thumbnailType == 'tooltipAndScroller')
						showThumbnail(index);
				},
				function() {
					var index = parseInt($(this).attr('rel'));
					
					if (!$(this).hasClass('select'))
						$(this).removeClass('over');
						
					if (slidesData[index].properties.thumbnailType == 'tooltip' || slidesData[index].properties.thumbnailType == 'tooltipAndScroller')
						hideThumbnail();
				});
				
				
				button.click(function() {
					triggerType = 'button';
					gotoSlide(parseInt($(this).attr('rel')));
				});
			}
			
			
			if (self.settings.slideButtonsToggle) {
				if (isOldIE)
					slideButtons.hide();
				else
					slideButtons.css('opacity', 0);
				
				
				slider.hover(function() {
					if (isOldIE) 
						slideButtons.show();
					else 	
						slideButtons.stop().animate({'opacity': 1}, self.settings.slideButtonsShowDuration);
				 },
										
				 function() {
					if (isOldIE) 
						slideButtons.hide();
					else	
						slideButtons.stop().animate({'opacity': 0}, self.settings.slideButtonsHideDuration);
				 });
			}
		}
		
		
		/**
		* Creates the keyboard navigation
		*/
		function createKeyboardNavigation() {
			$(document).keydown(function(event) {
				if (event.which == 37 && ((self.settings.keyboardNavigationOnFocusOnly && isFocus) || !self.settings.keyboardNavigationOnFocusOnly)) {
					previousSlide();						
				} else if (event.which == 39 && ((self.settings.keyboardNavigationOnFocusOnly && isFocus) || !self.settings.keyboardNavigationOnFocusOnly)) {
					nextSlide();
				} else if (event.which == 13 && isFocus && slideDivs[currentIndex].children('a').length) {
					(slideDivs[currentIndex].children('a').not('.thumbnail'))[0].click();
				}
			});
		}
		
		
		/**
		* Creates the shadow
		*/
		function createShadow() {			
			var shadow = $('<div class="shadow"></div>').appendTo(sliderMain),
				shadowLeft = $('<div class="shadow-left"></div>').appendTo(shadow),
				shadowMiddle = $('<div class="shadow-middle"></div>').appendTo(shadow),
				shadowRight = $('<div class="shadow-right"></div>').appendTo(shadow);
		}
		
		
		/**
		* Shows the thumbnail for the specified index
		*/
		function showThumbnail(index) {
			// check if a thumbnail image was specified
			if (!slidesData[index].thumbnail)
				return;
			
			// the path to the thumbnail image
			var hasLink = (slidesData[index].thumbnail).is('a'),
				thumbnailSource = hasLink ? (slidesData[index].thumbnail).find('img') : slidesData[index].thumbnail,
				path = thumbnailSource.attr('src'),
				caption = thumbnailSource.attr('alt'),
				thumbnailWrapper = $('<div class="thumbnail-wrapper"></div>').appendTo(slider.find('.buttons-inner')),
				thumbnail = $('<div class="thumbnail"></div>').css({'width': self.settings.thumbnailWidth, 'height': self.settings.thumbnailHeight})
															  .appendTo(thumbnailWrapper);			
			
			thumbnailWrapper.css({'width': thumbnail.outerWidth(), 'height': thumbnail.outerHeight()});
			
			// add caption to the thumbnail				  
			if (caption) {
				var	thumbnailCaption = $('<div class="caption"></div>').appendTo(thumbnail),																					 
					thumbnailCaptionBackground = $('<div class="background"></div>').appendTo(thumbnailCaption),
					thumbnailCaptionContent = $('<div class="content">' + caption + '</div>').appendTo(thumbnailCaption),					
					opacity = thumbnailCaptionBackground.css('opacity');
					
				thumbnailCaption.css('height', thumbnailCaptionContent.outerHeight(true));
					
				// set the initial position of the caption	
				if (self.settings.thumbnailCaptionPosition == 'top')
					thumbnailCaption.css('top', 0);
				else
					thumbnailCaption.css('bottom', 0);
			}
			
			// holds a reference to the navigation button that is rolled over
			var button = slider.find('.buttons-inner a').eq(index),
				
				// calculate the position of the thumbnail image
				leftPos = button.position().left - (thumbnailWrapper.outerWidth(true) - button.width()) / 2,
				topPos = button.position().top - thumbnailWrapper.outerHeight(true);
			
			
			if (isOldIE)
				thumbnailWrapper.find('*').not('.background').css('filter', 'inherit');
				
			thumbnailWrapper.css({'left': leftPos, 
								  'top': topPos - self.settings.thumbnailSlideAmount, 
								  'opacity': 0});
			
			// load the image using the <img> tag and when it's completely assign it as a background to the thumbnail DIV
			$('<img/>').load(function() {
								 thumbnail.css('background-image','url('+ path +')');								 
							 })
					   .attr('src', path);
			
			if (isOldIE) {						
				thumbnailWrapper.animate({'top': topPos, 'opacity': 1}, 
										  self.settings.thumbnailSlideDuration, 
										  self.settings.thumbnailSlideEasing);
				
				thumbnailWrapper.find('.caption .background').animate({'opacity': opacity}, self.settings.thumbnailSlideDuration);
			} else {
				thumbnailWrapper.animate({'top': topPos, 'opacity': 1}, 
										  self.settings.thumbnailSlideDuration, 
										  self.settings.thumbnailSlideEasing);

			}
			
		}
		
		
		/**
		* Hides the visible thumbnail
		*/
		function hideThumbnail() {
			// check if there is a visible thumbnail
			var thumbnailWrapper = slider.find('.buttons-inner .thumbnail-wrapper');
			
			if (!thumbnailWrapper.length)
				return;
			
			var thumbnailCaptionBackground = thumbnailWrapper.find('.background');
			
			if (isOldIE)
				thumbnailCaptionBackground.animate({'opacity': 0}, self.settings.thumbnailSlideDuration);
					
			thumbnailWrapper.animate({'top': thumbnailWrapper.position().top - self.settings.thumbnailSlideAmount, 'opacity': 0}, 
									  self.settings.thumbnailSlideDuration, 
									  self.settings.thumbnailSlideEasing, 
									  function(){											  
										  thumbnailWrapper.remove();
									  })
		}		
		
		
		/**
		* Opens the slide with the specified index
		*/
		function gotoSlide(index) {
			if (index == currentIndex)
				return;
			
			// if the slider is already in the transition phase
			if (isTransition) {
				if (self.settings.overrideTransition) {
					switch(currentEffectType) {
						case 'slide':
							slideDivs[currentIndex].stop(false, true);							
							break;
							
						case 'fade':
							slideDivs[currentIndex].stop(false, true);
						
							if (previousIndex != -1)
								slideDivs[previousIndex].stop(false, true);								
							break;
							
						case 'slice':
							sliceContainer.find('.slice').each(function() {
								clearTimeout($(this).data('delay'));													
								$(this).stop().remove();
							});
							
							sliceContainer.remove();
							
							if (previousIndex != -1)
								slideDivs[previousIndex].css({'visibility': ''});
							
							slideDivs[currentIndex].css('visibility', 'visible');
							
							completeTransition();
							break;
					}
				} else if (self.settings.effectType != 'swipe') {
					return;
				}
			}
			
			
			// if the slideshow mode is on, stop the slideshow timer
			if (self.settings.slideshow)
				clearSlideshow();
			
			
			if (isLightbox) {
				isLightbox = false;
				
				var lightboxIcon = slideWrapper.find('.lightbox-icon');
				
				if (!(isOldIE))
					lightboxIcon.stop().animate({'opacity': 0}, self.settings.lightboxIconFadeDuration, function(){lightboxIcon.remove();});
				else
					lightboxIcon.remove();
			}
			
			
			if (currentVideo) {
				currentVideo.leaveVideo();
				isVideoPlaying = false;
			}
			
			
			isTransition = true;
			previousIndex = currentIndex;
			currentIndex = index;
			
			
			if (currentIndex != activeCaptionIndex) {
				// remove the previous caption
				// when the swipe effect is not used, remove the previous caption before the transition begins
				if (isCaption && self.settings.effectType != 'swipe') {
					isCaption = false;
					hideCaption();
					
					if (captionDelayTimer)
						clearTimeout(captionDelayTimer);
				}
			}
						
			
			var slideData = slidesData[index],
				image = slideDivs[index].find('img.image');
			
			// load the lazy image if there is one specified
			if (slideData.lazyImage && !slideData.lazyImageLoaded) {
				var newImage = $('<img class="image"/>').attr('src', slidesData[index].lazyImage);
				
				if (image.attr('alt'))
					newImage.attr('alt', image.attr('alt'));
				
				if (image.attr('title'))
					newImage.attr('title', image.attr('title'));
				
				newImage.data('index', index);
				
				if (image.length) {
					newImage.prependTo(image.parent());
					image.remove();
				} else {
					newImage.prependTo(slideDivs[index]);
				}
				
				image = newImage;
				
				slideData.imagePath = image.attr('src');
				slideData.lazyImageLoaded = true;
			}
			
			
			// if an image was specified, make sure it's fully loaded, scaled and aligned
			// before the transitions begins
			if (image.length && (image.data('state') != 'loaded' || self.settings.scaleType == 'proportionalFit')) {
				var alignType = slideData.properties.alignType;
				
				if (self.settings.effectType == 'swipe') {
					image.css('visibility', 'hidden');
					
					if (image[0].complete || image[0].readyState == 'complete') {
						image.css('visibility', '');
						
						scaleAndAlign(image, alignType);	
					} else {
						showPreloader(slideDivs[image.data('index')]);
						
						image.load(function() {
										image.css('visibility', '');										
										scaleAndAlign(image, alignType);										
										hidePreloader(slideDivs[image.data('index')]);
							 })
							 .error(function() {
										hidePreloader(slideDivs[image.data('index')]);
										isTransition = false;
							 });
					}
					
					startTransition();
				} else {				
					if (image[0].complete || image[0].readyState == 'complete') {
						scaleAndAlign(image, alignType);					
						startTransition();
					} else {
						showPreloader(slideWrapper);
						
						image.load(function() {
										scaleAndAlign(image, alignType);
										hidePreloader(slideWrapper);
										startTransition();
							  })
							 .error(function() {
										hidePreloader(slideWrapper);
										isTransition = false;
							 });
					}
				}
			} else {
				startTransition();
			}
			
			
			// highlight the button that corresponds to the current slide
			// and remove the highlight from the previously selected button
			if (self.settings.slideButtons) {
				var buttons = slider.find('.buttons-inner');
				
				buttons.find('.select').removeClass('select');
				buttons.find('a').eq(index).attr('class', 'select');	
			}
			
			
			// highlight the thumbnail that corresponds to the current slide
			// and remove the highlight from the previously selected thumbnail
			if (self.settings.thumbnailType == 'scroller' || self.settings.thumbnailType == 'tooltipAndScroller') {
				thumbnailScroller.find('.thumbnail.select').removeClass('select');				
				thumbnailScroller.find('.thumbnail[data-index=' + index + ']').removeClass('over').addClass('select');
				
				// scroll the thumbnails to the page that containes the thumbnail 
				// which corresponds to the current slide
				if (self.settings.thumbnailSync) {
					var thumbnailPage = Math.floor(index / (visibleThumbnails * thumbnailLayers));
					
					if (thumbnailPage != currentThumbnailPage)
						scrollToThumbnailPage(thumbnailPage);
				}
			}
			
			
			// fire the 'slideOpen' event
			var eventObject = {type: 'slideOpen', index: currentIndex, data: slideData};
			$.isFunction(self.settings.slideOpen) && self.settings.slideOpen.call(this, eventObject);
		}
		
		
		/**
		* Opens the next slide
		*/
		function nextSlide() {				
			var index = (currentIndex == numSlides - 1) ? 0 : (currentIndex + 1);
			gotoSlide(index);
		}
		
		
		/**
		* Opens the previous slide
		*/
		function previousSlide() {
			var index = currentIndex == 0 ? (numSlides - 1) : (currentIndex - 1);
			gotoSlide(index);
		}
		
		
		/**
		* Preload the images that are nearby the image located at the specified index
		*/
		function preloadNearbyImages(index) {
			var imagesToPreload = [];
			
			// check how many slides there are
			if(numSlides <= 1) {
				return;
			} else if (numSlides == 2) {
				if (index == 0)
					imagesToPreload.push(1);
				else if (index == 1)
					imagesToPreload.push(0);
			} else if (numSlides >= 3) {
				var previous = (index == 0) ? numSlides - 1 : index - 1,
					next = (index == numSlides - 1) ? 0 : index + 1;
					
				imagesToPreload.push(previous, next);
			}
			
			// preload the lazy images added to the array
			for (var i = 0; i < imagesToPreload.length; i++) {
				var idx = imagesToPreload[i],
					slideData = slidesData[idx];
				
				if (slideData.lazyImage && !slideData.lazyImageLoaded) {
					var image = slideDivs[idx].find('img.image'),
						newImage = $('<img class="image"/>').attr('src', slideData.lazyImage),
						alignType = slideData.properties.alignType;
						
					if (image.attr('alt'))
						newImage.attr('alt', image.attr('alt'));
					
					if (image.attr('title'))
						newImage.attr('title', image.attr('title'));
					
					newImage.data('index', idx);
					
					if (image.length) {
						newImage.prependTo(image.parent());
						image.remove();
					} else {
						newImage.prependTo(slideDivs[idx]);
					}
					
					image = newImage;
					slideData.lazyImageLoaded = true;
					
					slideData.imagePath = image.attr('src');
					
					
					if (image[0].complete || image[0].readyState == 'complete') {
						scaleAndAlign(image, alignType);
					} else {				
						image.load(function() {
							scaleAndAlign(image, alignType);
						});
					}
				}
			}
		}
		
		
		/**
		* Shows the main preloader
		*/
		function showPreloader(target) {
			if (target.find('.preloader').length)
				return;
				
			var preloader = $('<div class="preloader"></div>')
					.hide()
					.appendTo(target),
				
				// calculate the preloader's position
				preloaderLeft = ((target.outerWidth(true) -preloader.width()) * 0.5),
				preloaderTop = ((target.outerHeight(true) - preloader.height()) * 0.5);
			
			preloader.fadeIn(300);
			preloader.css({'left': preloaderLeft, 'top': preloaderTop});
		}
		
		
		/**
		* Hides the main preloader
		*/
		function hidePreloader(target) {
			target.find('.preloader').stop().remove();
		}
		
		
		/**
		* Prevent the default behaviour of the specified event pbject
		*/
		function disableEvent(event) {
			event.preventDefault();	
		}		
		
		
		/**
		* Starts the transition
		*/
		function startTransition() {
			var effectType = slidesData[currentIndex].properties.effectType;
			
			// check if the first slide is set to appear without a transition effect
			if (!self.settings.initialEffect && firstTransition) {
				effectType = 'none';
				firstTransition = false;
			}
			
			
			if (self.settings.effectType == 'swipe') {
				effectType = 'swipe';
			} else if (effectType == 'random') {
				var effects = ['slice', 'slide', 'fade'];
				effectType = getRandom(effects);
			}
			
			currentEffectType = effectType;
			
			// fire the 'transitionStart' event
			var eventObject = {type: 'transitionStart', index: currentIndex, data: slidesData[currentIndex]};
			$.isFunction(self.settings.transitionStart) && self.settings.transitionStart.call(this, eventObject);
			
			switch(effectType) {
				case 'none':
					startNoneTransition();
					break;
					
				case 'fade':
					startFadeTransition();
					break;
					
				case 'slide':
					startSlideTransition();
					break;
					
				case 'swipe':
					startSwipeTransition();
					break;
					
				case 'slice':
					startSliceTransition();
					break;
					
				default:
					startFadeTransition();
			}
		}
		
		
		/**
		* Starts the 'none' transition
		*/
		function startNoneTransition() {
			var currentSlideElement = slideDivs[currentIndex],
				previousSlideElement;
				
			if (previousIndex != -1)
				slideDivs[previousIndex].css('visibility', '');
								
			slideDivs[currentIndex].css('visibility', 'visible');
			
			completeTransition();
		}
		
		
		/**
		* Starts the 'fade' transition
		*/
		function startFadeTransition() {
			var properties = slidesData[currentIndex].properties,
				currentSlideElement = slideDivs[currentIndex],
				previousSlideElement,
				fadeInDuration = parseInt(properties.fadeInDuration),
				fadeInEasing = properties.fadeInEasing,
				fadeOutDuration = parseInt(properties.fadeOutDuration),
				fadeOutEasing = properties.fadeOutEasing;
			
			
			if (isOldIE)
				currentSlideElement.find('*').each(function() {
					if (!($(this).hasClass('.caption-container') || $(this).parents('.caption-container').length == 1)) {
						$(this).css('filter', 'inherit');
					}
				});
						 
						   
			currentSlideElement.css({'visibility': 'visible', 'opacity': 0})
							   .animate({'opacity': 1}, fadeInDuration, fadeInEasing, function() {
								   currentSlideElement.css({'opacity': ''});
								   
								   if (isOldIE)
									   currentSlideElement.find('*').each(function() {
											if (!($(this).hasClass('.caption-container') || $(this).parents('.caption-container').length == 1)) {
												$(this).css('filter', '');
											}
										});
								   
								   completeTransition();
							   });
			
			
			if (previousIndex != -1) {
				previousSlideElement = slideDivs[previousIndex];
				
				if (isOldIE)
					previousSlideElement.find('*').each(function() {
						if (!($(this).hasClass('.caption-container') || $(this).parents('.caption-container').length == 1)) {
							$(this).css('filter', 'inherit');
						}
					});
					
				previousSlideElement.animate({'opacity': 0}, fadeOutDuration, fadeOutEasing, function() {
					previousSlideElement.css({'visibility': '', 'opacity': ''});
					
					if (isOldIE)
						previousSlideElement.find('*').each(function() {
							if (!($(this).hasClass('.caption-container') || $(this).parents('.caption-container').length == 1)) {
								$(this).css('filter', '');
							}
						});
				});
			}
		}
		
		
		/**
		* Starts the 'simpleSlide' transition
		*/
		function startSlideTransition() {
			var properties = slidesData[currentIndex].properties,
				currentSlideElement = slideDivs[currentIndex],
				previousSlideElement,
				slideDirection = properties.slideDirection,
				slideDuration = parseInt(properties.slideDuration),
				slideEasing = properties.slideEasing,
				slideLoop = properties.slideLoop;
			
			if (previousIndex != -1)
				previousSlideElement = slideDivs[previousIndex];
			
			if (slideDirection == 'random') {
				var directions = ['autoHorizontal', 'autoVertical', 'rightToLeft', 'leftToRight', 'topToBottom', 'bottomToTop'];				
				slideDirection = getRandom(directions);
			}
			
			currentSlideElement.css('visibility', 'visible');
			
			var endPosition = {},
				prop,
				value;
				
			switch (slideDirection) {
				case 'autoHorizontal':
					prop = 'left';					
					
					if (currentIndex == 0 && previousIndex == numSlides - 1 && slideLoop)
						value = slideWrapper.width();
					else if (currentIndex == numSlides - 1 && previousIndex == 0 && slideLoop)
						value = - slideWrapper.width();
					else
						value = (currentIndex > previousIndex) ? slideWrapper.width() : - slideWrapper.width();
					
					break;
					
				case 'autoVertical':
					prop = 'top';					
					
					if (currentIndex == 0 && previousIndex == numSlides - 1 && slideLoop)
						value = slideWrapper.height();
					else if (currentIndex == numSlides - 1 && previousIndex == 0 && slideLoop)
						value = - slideWrapper.height();
					else
						value = (currentIndex > previousIndex) ? slideWrapper.height() : - slideWrapper.height();					
					
					break;
					
				case 'rightToLeft':
					prop = 'left';
					value = slideWrapper.width();
					break;
				
				case 'leftToRight':
					prop = 'left';
					value = - slideWrapper.width();
					break;
				
				case 'topToBottom':
					prop = 'top';
					value = - slideWrapper.height();
					break;
				
				case 'bottomToTop':
					prop = 'top';
					value = slideWrapper.height();
					break;
					
				default:
					prop = 'left';
					value = (currentIndex > previousIndex) ? slideWrapper.width() : - slideWrapper.width();
			}
			
			currentSlideElement.css(prop, value);
			
			endPosition[prop] = 0;
			
			currentSlideElement.animate(endPosition, {duration: slideDuration, easing: slideEasing,
				complete: function() {
					currentSlideElement.css({'left': '', 'top': ''});
					
					if (previousIndex != -1)
						previousSlideElement.css({'visibility': '', 'left': '', 'top': ''});
					
					completeTransition();
				},
				step: function(currentValue) {
					if (previousSlideElement)
						previousSlideElement.css(prop, currentValue - value);
				}
			});
		}		
		
		
		/**
		* Starts the 'swipe' transition
		*/
		function startSwipeTransition() {				
			if (previousIndex == -1) {
				// arrange the slides					
				slides.css('overflow', 'visible')
					  .bind('dragstart', disableEvent)
					  .find('.slide').each(function(index) {
							var slide = $(this);
							
							if (self.settings.swipeOrientation == 'horizontal')
								slide.css({'top': 0, 'left': index * (slideWrapper.width()  + self.settings.swipeSlideDistance), 'visibility': 'visible'});
							else if (self.settings.swipeOrientation == 'vertical')
								slide.css({'top': index * (slideWrapper.height() + self.settings.swipeSlideDistance), 'left': 0, 'visibility': 'visible'});
					  });
				
				
				if (self.settings.swipeGrabCursor)
					slides.addClass('grab');
					
										
				var slidesPosition,
					dragRatio,
					allowDrag = false,
					isTouch = false;
				
				if (self.settings.swipeTouchDrag && 'ontouchstart' in window) {
					allowDrag = true;
					isTouch = true;
				}
				
				if (!isTouch && self.settings.swipeMouseDrag) {
					allowDrag = true;
				}
				
				
				if (self.settings.swipeOrientation == 'horizontal') {
					slides.css('left', - slideStart * (slideWrapper.width() + self.settings.swipeSlideDistance));
					
					if (allowDrag)
						slides.swipe({
							threshold: self.settings.swipeThreshold,
							allowPageScroll: 'vertical',
							swipeStatus: function(event, phase, direction, distance) {
								if (phase == 'start') {
									slidesPosition = parseInt(slides.css('left'));
									
									if (slides.is(':animated'))
										slides.stop();
										
									// disable clicks during swipe
									var target = $(event.target).closest('a');
									
									if (target.length) {
										target.unbind('click', disableEvent).bind('click', disableEvent);
										
										if (target.has('rel'))
											target.unbind('click.prettyphoto');
									}
									
									if (self.settings.swipeGrabCursor)
										slides.removeClass('grab').addClass('grabbing');
										
								} else if (phase == 'move') {									
									dragRatio = ((currentIndex == numSlides - 1 && direction == 'left') || (currentIndex == 0 && direction == 'right')) ? 0.2 : 1;
										
									if (direction == 'left')
										slides.css('left', slidesPosition - distance * dragRatio);
									else if (direction == 'right')
										slides.css('left', slidesPosition + distance * dragRatio);									
								} else if (phase == 'cancel') {
									slides.stop().animate({'left': - currentIndex * (slideWrapper.width() + self.settings.swipeSlideDistance)}, 
														   self.settings.swipeBackDuration, 
														   self.settings.swipeEasing, 
														   function() {
																if (isTransition)
																	completeTransition();
														   });
														   
									if (self.settings.swipeGrabCursor)
										slides.removeClass('grabbing').addClass('grab');
								}
							},
							click: function(event) {
								// enable clicks									
								var target = $(event.target).closest('a');
									
								if (target.length) {
									target.unbind('click', disableEvent);
									
									if (target.attr('rel') && $.prettyPhoto)
										target.bind('click.prettyphoto', $.prettyPhoto.initialize);
								}
							},
							swipeLeft: function() {
								if (currentIndex < numSlides - 1)
									nextSlide();
								else
									slides.stop().animate({'left': - currentIndex * (slideWrapper.width() + self.settings.swipeSlideDistance)}, 
														   self.settings.swipeBackDuration, 
														   self.settings.swipeEasing, 
														   function() {
																if (isTransition)
																	completeTransition();
														   });
														   
								if (self.settings.swipeGrabCursor)
									slides.removeClass('grabbing').addClass('grab');
							},							
							swipeRight: function() {
								if (currentIndex > 0)
									previousSlide();
								else
									slides.stop().animate({'left': - currentIndex * (slideWrapper.width() + self.settings.swipeSlideDistance)}, 
														   self.settings.swipeBackDuration, 
														   self.settings.swipeEasing, 
														   function() {
																if (isTransition)
																	completeTransition();
														   });
														   
								if (self.settings.swipeGrabCursor)
									slides.removeClass('grabbing').addClass('grab');
							}
						});
				} else if (self.settings.swipeOrientation == 'vertical') {
					slides.css('top', - slideStart * (slideWrapper.height() + self.settings.swipeSlideDistance));
					
					if (allowDrag)
						slides.swipe({
							threshold: self.settings.swipeThreshold,
							allowPageScroll: 'horizontal',
							swipeStatus: function(event, phase, direction, distance) {								
								if (phase == 'start') {
									slidesPosition = parseInt(slides.css('top'));	
									
									if (slides.is(':animated'))
										slides.stop();
									
									// disable clicks during swipe
									var target = $(event.target).closest('a');
									
									if (target.length) {
										target.unbind('click', disableEvent).bind('click', disableEvent);
										
										if (target.has('rel'))
											target.unbind('click.prettyphoto');
									}
									
									if (self.settings.swipeGrabCursor)
										slides.removeClass('grab').addClass('grabbing');
										
								} else if (phase == 'move') {
									dragRatio = ((currentIndex == numSlides - 1 && direction == 'up') || (currentIndex == 0 && direction == 'down')) ? 0.2 : 1;
									
									if (direction == 'up')
										slides.css('top', slidesPosition - distance * dragRatio);
									else if (direction == 'down')
										slides.css('top', slidesPosition + distance * dragRatio);																			
								} else if (phase == 'cancel') {									
									slides.stop().animate({'top': - currentIndex * (slideWrapper.height() + self.settings.swipeSlideDistance)}, 
														   self.settings.swipeBackDuration, 
														   self.settings.swipeEasing, 
														   function() {
																if (isTransition)
																	completeTransition();
														   });
														   
									if (self.settings.swipeGrabCursor)
										slides.removeClass('grabbing').addClass('grab');
								}						
							},
							click: function(event) {
								// enable clicks									
								var target = $(event.target).closest('a');
									
								if (target.length) {
									target.unbind('click', disableEvent);
									
									if (target.attr('rel') && $.prettyPhoto)
										target.bind('click.prettyphoto', $.prettyPhoto.initialize);
								}
							},
							swipeUp: function() {
								if (currentIndex < numSlides - 1)
									nextSlide();
								else
									slides.stop().animate({'top': - currentIndex * (slideWrapper.height() + self.settings.swipeSlideDistance)}, 
														   self.settings.swipeBackDuration, 
														   self.settings.swipeEasing, 
														   function() {
																if (isTransition)
																	completeTransition();
														   });
														   
								if (self.settings.swipeGrabCursor)
									slides.removeClass('grabbing').addClass('grab');
							},							
							swipeDown: function() {
								if (currentIndex > 0)
									previousSlide();
								else
									slides.stop().animate({'top': - currentIndex * (slideWrapper.height() + self.settings.swipeSlideDistance)}, 
														   self.settings.swipeBackDuration, 
														   self.settings.swipeEasing, 
														   function() {
																if (isTransition)
																	completeTransition();
														   });
														   
								if (self.settings.swipeGrabCursor)
									slides.removeClass('grabbing').addClass('grab');
							}
						});
				}
				
				completeTransition();
			} else {					
				if (self.settings.swipeOrientation == 'horizontal') {
					slides.stop().animate({'left': - currentIndex * (slideWrapper.width() + self.settings.swipeSlideDistance)}, 
										   self.settings.swipeDuration, 
										   self.settings.swipeEasing, 
										   function() {
												// workaround for 'stop' not functioning properly when the 'transition' library is used
												if($.support.transition && !slides.is(':animated')) 
													completeTransition();
												else if (!$.support.transition)
													completeTransition();
										   });
				} else if (self.settings.swipeOrientation == 'vertical') {
					slides.stop().animate({'top': - currentIndex * (slideWrapper.height() + self.settings.swipeSlideDistance)}, 
										   self.settings.swipeDuration, 
										   self.settings.swipeEasing, 
										   function() {
											   // workaround for 'stop' not functioning properly when the 'transition' library is used
											   if($.support.transition && !slides.is(':animated')) 
													completeTransition();
												else if (!$.support.transition)
													completeTransition();
										   });
				}
			}
		}
		
		
		/**
		* Starts the slice transition (this will handle any transition that can have multiple slices: fade, scale, width, height and slide)
		*/
		function startSliceTransition() {
			var slideData = slidesData[currentIndex],
				properties = slideData.properties,
				alignType = properties.alignType,
				sliceEffectType = properties.sliceEffectType,
				htmlDuringTransition = properties.htmlDuringTransition,
				horizontalSlices = parseInt(properties.horizontalSlices),
				verticalSlices = parseInt(properties.verticalSlices),
				slicePattern = properties.slicePattern,
				slicePoint = properties.slicePoint,
				sliceStartPosition = properties.sliceStartPosition,
				sliceStartRatio = parseFloat(properties.sliceStartRatio),
				sliceDuration = parseInt(properties.sliceDuration),
				sliceEasing = properties.sliceEasing,
				sliceDelay = parseInt(properties.sliceDelay),
				sliceFade = (properties.sliceFade == true || effectType == 'fade') ? 0 : 1,
				
				fadePreviousSlide = properties.fadePreviousSlide,
				fadePreviousSlideDuration = parseInt(properties.fadePreviousSlideDuration),
				
				//calculate the width and height of the slices
				sliceWidth = Math.floor(slideWrapper.width() / horizontalSlices),
				sliceHeight = Math.floor(slideWrapper.height() / verticalSlices),
				
				// clone the image and the slide itself
				imageClone = slideDivs[currentIndex].find('img.image').clone(),
				slideClone = slideDivs[currentIndex].clone(),
				
				initialSlices = [];
				
			
			if (previousIndex != -1 && fadePreviousSlide == true) {
				var previousSlideElement = slideDivs[previousIndex];
						
				if (isOldIE)
					previousSlideElement.find('*').each(function() {
						if (!($(this).hasClass('.caption-container') || $(this).parents('.caption-container').length == 1)) {
							$(this).css('filter', 'inherit');
						}
					});
					
				previousSlideElement.animate({'opacity': 0}, fadePreviousSlideDuration, function() {
					previousSlideElement.css({'visibility': '', 'opacity': ''});
					
					if (isOldIE)
						previousSlideElement.find('*').each(function() {
							if (!($(this).hasClass('.caption-container') || $(this).parents('.caption-container').length == 1)) {
								$(this).css('filter', '');
							}
						});
				});
			}
			
				
			// make the cloned content visible (because the original is set to hidden) and remove the caption	
			slideClone.css('visibility', 'visible').find('.caption-container').remove();
			
			if (!htmlDuringTransition)
				slideClone.find('.html').remove();
			
			// create the container for the slices	
			sliceContainer = $('<div class="slice-container"></div>').appendTo(slideWrapper);
				
			if (properties.slideMask)
				sliceContainer.css('overflow', 'hidden');
			
			// create the slices
			for (var i = 0; i < horizontalSlices; i++) {
				for (var j = 0; j < verticalSlices; j++) {
					
					var slice = $('<div class="slice"></div>').css({'left': i * sliceWidth, 'top': j * sliceHeight,
																	'width': sliceWidth, 'height': sliceHeight,
																	'opacity': sliceFade,
																	'display': 'none'})
															  .data({'hPos': i, 'vPos': j})
															  .appendTo(sliceContainer);
					
					var sliceContent;
					
					if (slideData.html || slideData.imagePath)
						sliceContent = slideClone.clone();
						
					if (sliceContent) {
						sliceContent.css({'left': - (i * sliceWidth), 'top': - (j * sliceHeight),
										  'width': slideWrapper.width(), 'height': slideWrapper.height(),
										  'overflow': 'hidden',
										  'position': 'absolute'});
						
						slice.html(sliceContent)
							 .css('overflow', 'hidden');
					}
					
					if (isOldIE)
						slice.find('*').each(function() {
							$(this).css('filter', 'inherit');
						});
					
					initialSlices.push(slice);
				}
			}	
			
			
			// if the 'random' value was specified for some of the properties, randomly select a new value from the list of available values
			
			if (sliceEffectType == 'random') {
				var types = ['scale', 'fade', 'width', 'height', 'slide'];				
				sliceEffectType = getRandom(types);
			}
			
			if (slicePattern == 'random') {
				var patterns = ['randomPattern', 'topToBottom', 'bottomToTop', 'leftToRight', 'rightToLeft', 'topLeftToBottomRight', 'topRightToBottomLeft', 'bottomLeftToTopRight',
								'bottomRightToTopLeft', 'horizontalMarginToCenter', 'horizontalCenterToMargin', 'marginToCenter', 'verticalCenterToMargin', 'skipOneTopToBottom',
								'skipOneBottomToTop', 'skipOneLeftToRight', 'skipOneRightToLeft', 'skipOneHorizontal', 'skipOneVertical', 'spiralMarginToCenterCW', 
								'spiralMarginToCenterCCW', 'spiralCenterToMarginCW', 'spiralCenterToMarginCCW'];				
				slicePattern = getRandom(patterns);
			}
			
			if ((horizontalSlices == 1 && slicePattern == 'skipOneHorizontal') || (verticalSlices == 1 && slicePattern == 'skipOneVertical'))
				slicePattern = 'randomPattern';
			
			if (slicePoint == 'random') {
				var points = ['leftTop', 'leftCenter', 'leftBottom', 'centerTop', 'centerCenter', 'centerBottom', 'rightTop', 'rightCenter', 'rightBottom'];				
				slicePoint = getRandom(points);
			}
			
			if (sliceStartPosition == 'random') {
				var positions = ['left', 'right', 'top', 'bottom', 'leftTop', 'rightTop', 'leftBottom', 'rightBottom', 'horizontalAlternate', 'verticalAlternate'];
				sliceStartPosition = getRandom(positions);
			}				
			
		
			// get the slices in a specific order, based on the slicePattern property
			var orderedSlices = getOrderedSlices(initialSlices, slicePattern, horizontalSlices, verticalSlices);			
			
			sliceCounter = orderedSlices.length;
			
			// animate all the slices
			for (var i = 0; i < sliceCounter; i++) {
				animateSlice(orderedSlices[i], i, sliceEffectType, slicePoint, sliceStartPosition, sliceStartRatio, sliceDuration, sliceEasing, sliceDelay);
			}
		}
		
		
		/**
		* Animates the individual slice
		*/
		function animateSlice(slice, i, sliceEffectType, slicePoint, sliceStartPosition, sliceStartRatio, sliceDuration, sliceEasing, sliceDelay) {
			// contains the starting values for the slice's properties
			var startState = new Object(),
			
				// contains the ending values for the slice's properties
				endState = new Object(),
				
				// assign values to the ending properties
				endWidth = parseInt(slice.css('width')),
				endHeight = parseInt(slice.css('height')),				
				endLeft = parseInt(slice.css('left')),
				endTop = parseInt(slice.css('top')),
			
				startLeft, startTop, startWidth = 0, startHeight = 0;
			
			// assign values to the starting left and top position based on the set effect type
			if (sliceEffectType == 'scale' || sliceEffectType == 'width' || sliceEffectType == 'height') {
				switch (slicePoint) {
					case 'centerCenter':
						startTop = endTop + endHeight * 0.5;
						startLeft = endLeft + endWidth * 0.5;
						break;	
						
					case 'rightCenter':
						startTop = endTop + endHeight * 0.5;
						startLeft = endLeft + endWidth;
						break;	
						
					case 'leftCenter':
						startTop = endTop + endHeight * 0.5;
						startLeft = endLeft;
						break;	
						
					case 'centerTop':
						startTop = endTop;
						startLeft = endLeft + endWidth * 0.5;
						break;	
						
					case 'rightTop':
						startTop = endTop;
						startLeft = endLeft + endWidth;
						break;	
						
					case 'leftTop':
						startTop = endTop;
						startLeft = endLeft;
						break;	
						
					case 'centerBottom':
						startTop = endTop + endHeight;
						startLeft = endLeft + endWidth * 0.5;
						break;	
						
					case 'rightBottom':
						startTop = endTop + endHeight;
						startLeft = endLeft + endWidth;
						break;	
						
					case 'leftBottom':
						startTop = endTop + endHeight;
						startLeft = endLeft;
						break;	
						
					default:
						startTop = endTop + endHeight * 0.5;
						startLeft = endLeft + endWidth * 0.5;
				}
			} else if (sliceEffectType == 'slide') {
				switch (sliceStartPosition) {
					case 'left':
						startTop = endTop;
						startLeft = endLeft - endWidth * sliceStartRatio;
						break;	
						
					case 'right':
						startTop = endTop;
						startLeft = endLeft + endWidth * sliceStartRatio;
						break;	
						
					case 'top':
						startTop = endTop - endHeight * sliceStartRatio;
						startLeft = endLeft;
						break;	
						
					case 'bottom':
						startTop = endTop + endHeight * sliceStartRatio;
						startLeft = endLeft;
						break;	
						
					case 'leftTop':
						startTop = endTop - endHeight * sliceStartRatio;
						startLeft = endLeft - endWidth * sliceStartRatio;
						break;	
						
					case 'rightTop':
						startTop = endTop - endHeight * sliceStartRatio;
						startLeft = endLeft + endWidth * sliceStartRatio;
						break;	
						
					case 'leftBottom':
						startTop = endTop + endHeight * sliceStartRatio;
						startLeft = endLeft - endWidth * sliceStartRatio;
						break;	
						
					case 'rightBottom':
						startTop = endTop + endHeight * sliceStartRatio;
						startLeft = endLeft + endWidth * sliceStartRatio;
						break;	
						
					case 'horizontalAlternate':
						startTop = endTop;
						startLeft = endLeft + endWidth * sliceStartRatio * (i % 2 == 0 ? 1 : -1);
						break;	
						
					case 'verticalAlternate':
						startTop = endTop + endHeight * sliceStartRatio * (i % 2 == 0 ? 1 : -1);
						startLeft = endLeft;
						break;	
						
					default:
						startTop = endTop;
						startLeft = endLeft - endWidth * sliceStartRatio;
				}
			}
			
			
			// assign values to the starting and ending states based on the set effect type
			switch (sliceEffectType) {
				case 'fade':
					endState = {'opacity': 1};
					break;
					
				case 'scale':
					startState = {'width': startWidth, 'height': startHeight, 'left': startLeft, 'top': startTop};
					endState = {'width': endWidth, 'height': endHeight, 'left': endLeft, 'top': endTop, 'opacity': 1};
					break;
					
				case 'width':
					startState = {'width': startWidth, 'left': startLeft};
					endState = {'width': endWidth, 'left': endLeft, 'opacity': 1};
					break;
					
				case 'height':
					startState = {'height': startHeight, 'top': startTop};
					endState = {'height': endHeight, 'top': endTop, 'opacity': 1};
					break;
					
				case 'slide':
					startState = {'left': startLeft, 'top': startTop};
					endState = {'left': endLeft, 'top': endTop, 'opacity': 1};
					break;
					
				default:
					endState = {'opacity': 1};					
			}
			
			// animate the slice
			var delay = setTimeout(function() {
				slice.css(startState)
					 .css('display', 'block')
					 .data('delay', delay)
					 .animate(endState, sliceDuration, sliceEasing, function(){ //if the last slice was animated, call the enTransition function
						sliceCounter--;
						
						if (sliceCounter == 0) {
							// remove all the slices
							sliceContainer.find('.slice').each(function() {					
								$(this).remove();
								
								if (isOldIE)
									$(this).find('*').each(function() {
										$(this).css('filter', 'inherit');
									});
							});
							
							sliceContainer.remove();
							
							if (previousIndex != -1)
								slideDivs[previousIndex].css({'visibility': ''});
							
							slideDivs[currentIndex].css('visibility', 'visible');
							
							completeTransition();
						}
					  });
			}, i * sliceDelay);
		}
		
		
		/**
		* This is called when the transition is complete
		*/
		function completeTransition() {
			var slideData = slidesData[currentIndex],
				effectType = slideData.properties.effectType,
				htmlDuringTransition = slideData.properties.htmlDuringTransition,
				captionDelay = parseInt(slideData.properties.captionDelay);
				
			isTransition = false;					
				
			// handle the slideshow behaviour
			if (self.settings.slideshow && slideshowState != 'stopped') {
				if (!self.settings.slideshowLoop && currentIndex == numSlides - 1) {
					stopSlideshow();
				} else {
					resetSlideshow();
					
					if ((self.settings.pauseSlideshowOnHover && isHover) || slideshowManuallyPaused)
						pauseSlideshow();
				}
			}
			
			
			if (currentIndex != activeCaptionIndex) {
				// remove the previous caption
				// when the swipe effect is used, remove the previous caption only when the transition is over
				if (isCaption && self.settings.effectType == 'swipe') {
					isCaption = false;
					hideCaption();
					
					if (captionDelayTimer)
						clearTimeout(captionDelayTimer);
				}
				
				// if a caption was specified for this slide, create it
				if (slideData.caption) {
					isCaption = true;
					
					captionDelayTimer = setTimeout(function() {
						if (!self.settings.captionToggle || (self.settings.captionToggle && isHover))
							showCaption();
					}, captionDelay); 
				} else {
					activeCaptionIndex = -1;
				}
			}
			
			
			if (self.settings.preloadNearbyImages)
				preloadNearbyImages(currentIndex);
			
			
			if (previousIndex != -1)
				slideDivs[previousIndex].removeClass('active');
			
			slideDivs[currentIndex].addClass('active');
			
			
			// get the lightbox link
			var lightboxLink = slideDivs[currentIndex].children('a[rel^="slider-lightbox"]');
			
			if (lightboxLink.length && self.settings.lightboxIcon) {
				isLightbox = true;
				
				var lightboxContent = lightboxLink.attr('href'),
					lightboxType,
					lightboxIcon;
				
				
				// check the type of lightbox content
				if (lightboxContent.indexOf('iframe=true') != -1 || lightboxContent.indexOf('ajax=true') != -1 || lightboxContent.indexOf('custom=true') != -1)
					lightboxType = 'link';
				else if (lightboxContent.indexOf('youtube.com') != -1 || lightboxContent.indexOf('youtu.be') != -1 || lightboxContent.indexOf('vimeo.com') != -1)
					lightboxType = 'video';
				else
					lightboxType = 'image';
					
				
				// create the lightbox icon
				lightboxIcon = $('<div class="lightbox-icon"></div>').appendTo(slideWrapper);
				
				
				// add the appropriate image to the lightbox icon
				if (lightboxType == 'video')
					lightboxIcon.addClass('play');
				else if (lightboxType == 'link')
					lightboxIcon.addClass('hyperlink');
				else if (lightboxType == 'image')
					lightboxIcon.addClass('magnifying');
					
					
				// display the icon
				if (self.settings.lightboxIconToggle && !isHover) {
					lightboxIcon.css('opacity', 0);
				} else {
					// fade in the canvas
					if (!isOldIE)
						lightboxIcon.css({'opacity': 0})
									.stop().animate({'opacity': 1}, self.settings.lightboxIconFadeDuration);	
				}
				
				
				// when the icon is clicked, trigger a click on the lightbox link
				lightboxIcon.click(function() {
					lightboxLink.trigger('click');
				});				
			}
			
			
			// check if there is a video inside the slide
			if ($.fn.videoController && slideDivs[currentIndex].find('.video').length) {
				
				// initialize the video controller
				currentVideo = slideDivs[currentIndex].find('.video').videoController({
					reachVideoAction: self.settings.reachVideoAction,
					leaveVideoAction: self.settings.leaveVideoAction,
					start: function() {						
						if (self.settings.videoPlayAction == 'stopSlideshow' && !slideshowManuallyPaused)
							clearSlideshow();
						else if (self.settings.videoPlayAction == 'pauseSlideshow' && !slideshowManuallyPaused)
							pauseSlideshow();
					},
					play: function() {						
						isVideoPlaying = true;
						
						if (self.settings.videoPlayAction == 'stopSlideshow' && !slideshowManuallyPaused)
							clearSlideshow();
						else if (self.settings.videoPlayAction == 'pauseSlideshow' && !slideshowManuallyPaused)
							pauseSlideshow();
					},
					pause: function() {						
						if (self.settings.videoPauseAction == 'startSlideshow' && !slideshowManuallyPaused)
							resetSlideshow();
						else if (self.settings.videoPauseAction == 'resumeSlideshow' && !slideshowManuallyPaused)
							resumeSlideshow();
					},
					end: function() {
						if (self.settings.videoEndAction == 'resetVideo')
							currentVideo.stopVideo();
						else if (self.settings.videoEndAction == 'startSlideshow' && !slideshowManuallyPaused)
							resetSlideshow();
						else if (self.settings.videoEndAction == 'resumeSlideshow' && !slideshowManuallyPaused)
							resumeSlideshow();
						else if (self.settings.videoEndAction == 'nextSlide')
							nextSlide();
					},
					fullscreenchange: function() {
						if ($.browser.msie) {							
							if ($('body').hasClass('vjs-full-window')) {
								if (self.settings.timerAnimation)
									slider.find('.timer-animation').css('visibility', 'hidden');
									
								if (self.settings.slideArrows)
									slideWrapper.find('.slide-arrows').css('visibility', 'hidden');
									
								if (self.settings.slideButtons)
									slider.find('.slide-buttons').css('visibility', 'hidden');
									
								if (self.settings.slideshowControls)
									slideWrapper.find('.slideshow-controls').css('visibility', 'hidden');
									
								if (self.settings.lightboxIcon)
									slideWrapper.find('.lightbox-icon').css('visibility', 'hidden');
									
								if (self.settings.thumbnailType == 'scroller' || self.settings.thumbnailType == 'tooltipAndScroller')
									slider.find('.thumbnail-scroller').css('visibility', 'hidden');
							} else {
								if (self.settings.timerAnimation)
									slider.find('.timer-animation').css('visibility', 'visible');
									
								if (self.settings.slideArrows)
									slideWrapper.find('.slide-arrows').css('visibility', 'visible');
									
								if (self.settings.slideButtons)
									slider.find('.slide-buttons').css('visibility', 'visible');
									
								if (self.settings.slideshowControls)
									slideWrapper.find('.slideshow-controls').css('visibility', 'visible');
									
								if (self.settings.lightboxIcon)
									slideWrapper.find('.lightbox-icon').css('visibility', 'visible');
									
								if (self.settings.thumbnailType == 'scroller' || self.settings.thumbnailType == 'tooltipAndScroller')
									slider.find('.thumbnail-scroller').css('visibility', 'visible');
							}
						}
					}
				});
				
				currentVideo.reachVideo();
			}
			
						
			// fire the 'transitionComplete' event
			var eventObject = {type: 'transitionComplete', index: currentIndex, data: slideData};
			$.isFunction(self.settings.transitionComplete) && self.settings.transitionComplete.call(this, eventObject);
		}		
		
		
		/**
		* Shows the caption
		*/
		function showCaption() {
			activeCaptionIndex = currentIndex;
			
			// get the specified values for the current caption
			var slideData = slidesData[activeCaptionIndex],
				properties = slideData.properties,
				
				captionPosition = properties.captionPosition,
				captionSize = getNumeric(properties.captionSize),
				captionWidth = getNumeric(properties.captionWidth),
				captionHeight = getNumeric(properties.captionHeight),
				captionLeft = getNumeric(properties.captionLeft),
				captionTop = getNumeric(properties.captionTop),
				
				captionShowEffect = properties.captionShowEffect,
				captionShowEffectDuration = parseInt(properties.captionShowEffectDuration),				
				captionShowEffectEasing = properties.captionShowEffectEasing,
				captionShowSlideDirection = properties.captionShowSlideDirection,
			
				containerWidth = (captionPosition == "custom") ? captionWidth : '100%',
				containerHeight = (captionPosition == "custom") ? captionHeight : '100%',
				containerLeft = (captionPosition == "custom") ? captionLeft : 0,
				containerTop = (captionPosition == "custom") ? captionTop : 0,			
			
				captionContainer = slideDivs[currentIndex].find('.caption-container'),
				captionWrapper = captionContainer.find('.wrapper'),
				captionBackground = captionWrapper.find('.background'),
				captionContent = captionWrapper.find('.caption'),
				
				captionBackgroundOpacity = parseFloat(properties.captionBackgroundOpacity),
				captionBackgroundColor = properties.captionBackgroundColor,
				
				initialPosition = captionShowEffect == "fade" ? 0 : captionSize,
				endState = {};			
			
			
			switch (captionPosition) {
				case 'left':
					captionContainer.css({'width': captionSize, 'height': containerHeight, 'left': containerLeft, 'top': containerTop});
					if (captionShowSlideDirection == 'auto')
						captionWrapper.css({'width': captionSize, 'height': containerHeight, 'left': -initialPosition, 'top': 0});
					break;
					
				case 'right':
					captionContainer.css({'width': captionSize, 'height': containerHeight, 'right': containerLeft, 'top': containerTop});
					if (captionShowSlideDirection == 'auto')
						captionWrapper.css({'width': captionSize, 'height': containerHeight, 'left': initialPosition, 'top': 0});
					break;
					
				case 'top':
					captionContainer.css({'width':containerWidth, 'height': captionSize, 'left': containerLeft, 'top': containerTop});
					if (captionShowSlideDirection == 'auto')
						captionWrapper.css({'width':containerWidth, 'height': captionSize, 'left': 0, 'top': -initialPosition});
					break;
					
				case 'bottom':
					captionContainer.css({'width': containerWidth, 'height': captionSize, 'left': containerLeft, 'bottom': containerTop});
					if (captionShowSlideDirection == 'auto')
						captionWrapper.css({'width': containerWidth, 'height': captionSize, 'left': 0, 'top': initialPosition});
					break;
					
				case 'custom':
					captionContainer.css({'width': containerWidth, 'height': containerHeight, 'left': containerLeft, 'top': containerTop});
					captionWrapper.css({'width': containerWidth, 'height': containerHeight, 'left': 0, 'top': 0});
					break;
					
				case 'default':
					captionContainer.css({'width': containerWidth, 'height': captionSize, 'left': containerLeft, 'bottom': containerTop});
					if (captionShowSlideDirection == 'auto')
						captionWrapper.css({'width': containerWidth, 'height': captionSize, 'left': 0, 'top': initialPosition});
					break;
			}
			
					
			if (captionShowEffect == "fade") {				
				if (isOldIE) {
					if ($.browser.version != '6.0') {
						captionContent.stop()
									  .css({'opacity': 0})
									  .animate({'opacity': 1}, captionShowEffectDuration, captionShowEffectEasing, function(){captionContent.css('filter', '')});
						
						captionBackground.stop()
									  	 .css({'opacity': 0})
										 .animate({'opacity': captionBackgroundOpacity}, captionShowEffectDuration, captionShowEffectEasing);
					} else {
						captionWrapper.css('opacity', 1);
						captionContent.css('opacity', 1);
						captionBackground.css('opacity', 1);
						captionWrapper.css('background-color', captionBackgroundColor);
					}
				} else {
					captionWrapper.css({'opacity': 0})
								  .animate({'opacity': 1}, captionShowEffectDuration, captionShowEffectEasing);
				}				
			} else {
				captionWrapper.stop().css({'opacity': 1});
				captionContent.stop().css('opacity', 1);
				captionBackground.stop().css('opacity', captionBackgroundOpacity);
				
				if (captionPosition == 'custom')
					captionContainer.css({'width': containerWidth, 'height': containerHeight, 'left': containerLeft, 'top': containerTop});
					
				if (captionShowSlideDirection == 'topToBottom')
					captionWrapper.css({'width': containerWidth, 'height': containerHeight, 'left': 0, 'top': -containerHeight});
				else if (captionShowSlideDirection == 'bottomToTop')
					captionWrapper.css({'width': containerWidth, 'height': containerHeight, 'left': 0, 'top': containerHeight});
				else if (captionShowSlideDirection == 'leftToRight')
					captionWrapper.css({'width': containerWidth, 'height': containerHeight, 'left': -containerWidth, 'top': 0});
				else if (captionShowSlideDirection == 'rightToLeft')
					captionWrapper.css({'width': containerWidth, 'height': containerHeight, 'left': containerWidth, 'top': 0});
				
				if ($.browser.msie && $.browser.version == '6.0')
					captionWrapper.css('background-color', captionBackgroundColor);
							
				captionWrapper.stop().animate({'top': 0, 'left': 0}, captionShowEffectDuration, captionShowEffectEasing);
			}
		}
		
		
		/**
		* Hides the caption
		*/
		function hideCaption() {
			if (activeCaptionIndex == -1)
				return;
				
			var captionContainer = slideDivs[activeCaptionIndex].find('.caption-container'),
				captionWrapper = captionContainer.find('.wrapper'),
				captionBackground = captionWrapper.find('.background'),
				captionContent = captionWrapper.find('.caption'),
				properties = slidesData[activeCaptionIndex].properties,
				
				captionPosition = properties.captionPosition,
				captionHideEffect = properties.captionHideEffect,
				captionHideEffectDuration = parseInt(properties.captionHideEffectDuration),
				captionHideEffectEasing = properties.captionHideEffectEasing,
				captionHideSlideDirection = properties.captionHideSlideDirection;
				
			if (captionHideEffect == "fade") {
				if (isOldIE) {
					if ($.browser.version != '6.0') {
						captionContent.stop().animate({'opacity': 0}, captionHideEffectDuration, captionHideEffectEasing);						
						captionBackground.stop().animate({'opacity': 0}, captionHideEffectDuration, captionHideEffectEasing);
					} else {
						captionContent.css('opacity', 0);
						captionBackground.css('opacity', 0);
						captionWrapper.css('opacity', 0);
					}
				} else {
					captionWrapper.stop().animate({'opacity': 0}, captionHideEffectDuration, captionHideEffectEasing);
				}
				
			} else {
				
				if (captionHideSlideDirection == 'topToBottom')
					captionWrapper.animate({'top': captionWrapper.height()}, captionHideEffectDuration);
				else if (captionHideSlideDirection == 'bottomToTop')
					captionWrapper.animate({'top': - captionWrapper.height()}, captionHideEffectDuration);
				else if (captionHideSlideDirection == 'leftToRight')
					captionWrapper.animate({'left': captionWrapper.width()}, captionHideEffectDuration);
				else if (captionHideSlideDirection == 'rightToLeft')
					captionWrapper.animate({'left': - captionWrapper.width()}, captionHideEffectDuration);
				else if (captionHideSlideDirection == 'auto')
					switch (captionPosition) {
						case 'left':
							captionWrapper.stop().animate({'left': - captionWrapper.width()}, captionHideEffectDuration);
							break;
							
						case 'right':
							captionWrapper.stop().animate({'left': captionWrapper.width()}, captionHideEffectDuration);
							break;
							
						case 'top':
							captionWrapper.stop().animate({'top': - captionWrapper.height()}, captionHideEffectDuration);
							break;
							
						case 'bottom':
							captionWrapper.stop().animate({'top': captionWrapper.height()}, captionHideEffectDuration);
							break;
							
						case 'custom':
							captionWrapper.stop().animate({'top': captionWrapper.height()}, captionHideEffectDuration);
							break;
							
						case 'default':
							captionWrapper.stop().animate({'top': captionWrapper.height()}, captionHideEffectDuration);
							break;
					}
			}
		}
		
		
		/**
		* Resets the slideshow
		*/
		function resetSlideshow() {
			var delay = slidesData[currentIndex].properties.slideshowDelay;
			
			slideshowState = 'playing';
			
			if (self.settings.timerAnimation)
				startTimerAnimation(delay);
				
			slideshowTimerPosition = 0;
			slideshowStartTime = (new Date()).getTime();
			
			if (slideshowTimer)
				clearTimeout(slideshowTimer);
				
			slideshowTimer = setTimeout(function() {
				triggerType = 'slideshow'; 
				
				if (self.settings.slideshowDirection == 'next')
					nextSlide();
				else if (self.settings.slideshowDirection == 'previous')
					previousSlide();
			}, delay);
		}
		
		
		/**
		* Start the slideshow
		*/
		function startSlideshow() {
			if (!self.settings.slideshow)
				self.settings.slideshow = true;
					
			slider.find('.slideshow-controls').removeClass('play').addClass('pause');
			
			resetSlideshow();
		}
		
		
		/**
		* Stop the slideshow
		*/
		function stopSlideshow() {
			slider.find('.slideshow-controls').removeClass('pause').addClass('play');
			slideshowState = 'stopped';
			clearSlideshow();
		}
		
		
		/**
		* Toggle the slideshow
		*/
		function toggleSlideshow() {
			if (slideshowState == 'playing' || slideshowState == 'paused') {
				stopSlideshow();
			} else if (slideshowState == 'stopped') {
				startSlideshow();
			}
		}
		
		
		/**
		* Clear the slideshow
		*/
		function clearSlideshow() {
			if (slideshowTimer)
				clearTimeout(slideshowTimer);
				
			if (self.settings.timerAnimation)
				stopTimerAnimation();
		}
		
		
		/**
		* Pauses the slideshow
		*/
		function pauseSlideshow() {
			if (self.settings.slideshow && slideshowState == 'playing' && !isTransition) {
				if (slideshowTimer)
					clearTimeout(slideshowTimer);
				
				slideshowState = 'paused';
				slideshowTimerPosition += (new Date()).getTime() - slideshowStartTime;
			}
		}
		
		
		/**
		* Resumes the slideshow
		*/
		function resumeSlideshow() {
			if (self.settings.slideshow && slideshowState == 'paused' && !isTransition) {
				var delay = slidesData[currentIndex].properties.slideshowDelay;
				
				slideshowState = 'playing';
				slideshowStartTime = (new Date()).getTime();
				
				if (slideshowTimer)
					clearTimeout(slideshowTimer);
					
				slideshowTimer = setTimeout(function() {
					if (self.settings.slideshowDirection == 'next')
						nextSlide();
					else if (self.settings.slideshowDirection == 'previous')
						previousSlide();
				}, delay - slideshowTimerPosition);
			}
		}
		
		
		/**
		* Creates the timer animation
		*/
		function startTimerAnimation(delay) {
			// create a canvas element
			var timerCanvas = document.createElement('canvas'),
			
				// calculate the diagonal of the timer based on the strokes's width and the specified radius
				timerSize = Math.max(self.settings.timerStrokeWidth1, self.settings.timerStrokeWidth2) + self.settings.timerRadius * 2,
				
				// calculate the center of the timer
				timerPosition = timerSize / 2,
				
				// used to transform degrees in radians
				radians = Math.PI / 180,
				
				// the current angle of the animated circle
				angle = 0,
				
				// will be used how much time has passed since the animation started
				currentTime,
				timePassed,
				
				// values for the color and opacity of the timer
				strokeOpacity1 = self.settings.timerStrokeOpacity1,
				strokeOpacity2 = self.settings.timerStrokeOpacity2,
				strokeRed1 = hexToRGB(self.settings.timerStrokeColor1).red,
				strokeGreen1 = hexToRGB(self.settings.timerStrokeColor1).green,					
				strokeBlue1 = hexToRGB(self.settings.timerStrokeColor1).blue,
				strokeRed2 = hexToRGB(self.settings.timerStrokeColor2).red,
				strokeGreen2 = hexToRGB(self.settings.timerStrokeColor2).green,					
				strokeBlue2 = hexToRGB(self.settings.timerStrokeColor2).blue;
				
				
			timerCanvas.width = timerCanvas.height = timerSize;
			
			// add the canvas to the slider
			$(timerCanvas).attr('class', 'timer-animation')
						  .appendTo(slideWrapper);
						  
			
			if (self.settings.timerAnimationControls) {
				if (slideshowManuallyPaused)
					$(timerCanvas).addClass('play');
				else
					$(timerCanvas).addClass('pause');
				
				
				$(timerCanvas).click(function(event) {
								  if (!slideshowManuallyPaused) {
									  slideshowManuallyPaused = true;	
									  
									  $(timerCanvas).removeClass('pause').addClass('play');
									  
									  if (!(self.settings.pauseSlideshowOnHover && isHover))
										  pauseSlideshow();
								  } else if (slideshowManuallyPaused) {
									  slideshowManuallyPaused = false;	
										
									  $(timerCanvas).removeClass('play').addClass('pause');
																  
									  if (!(self.settings.pauseSlideshowOnHover && isHover))
										  resumeSlideshow();
								  }
							  });
			}
			
			
			if (self.settings.timerToggle && !isHover) {
				$(timerCanvas).css('opacity', 0);
			} else {
				// fade in the canvas
				if (!isOldIE)
					$(timerCanvas).css({'opacity': 0})
								  .stop().animate({'opacity': 1}, self.settings.timerFadeDuration);	
			}
			
			
			var	ctx;
			
			if (timerCanvas.getContext)
				ctx = timerCanvas.getContext("2d");
			else if (typeof G_vmlCanvasManager != 'undefined' && isOldIE)
				ctx = G_vmlCanvasManager.initElement(timerCanvas).getContext("2d");
			else
				return;
			
			
			// draw the underlying circle
			ctx.beginPath();
			ctx.lineWidth = self.settings.timerStrokeWidth1;
			ctx.strokeStyle = 'rgba(' + strokeRed1 + ', ' + strokeGreen1 + ', ' + strokeBlue1 + ', ' + strokeOpacity1 + ')';	
			ctx.arc(timerPosition, timerPosition, self.settings.timerRadius, 0, 2 * Math.PI, false);
			ctx.stroke();
				
				
			timerAnimationTimer = setInterval(function() {
				if (angle <= 360 && !((self.settings.pauseSlideshowOnHover && isHover) || isLightboxOpen || slideshowManuallyPaused || slideshowState == 'paused')) {
					currentTime = (new Date()).getTime();
					timePassed = (currentTime - slideshowStartTime);
					
					// calculate the angle on the circle based on how much time has passed
					angle = ((slideshowTimerPosition + timePassed) / delay) * 360 + 1;
					if (angle > 360) 
						angle = 360;
					
					// clear the canvas
					timerCanvas.width = timerCanvas.width;
					
					// draw the underlying circle
					ctx.beginPath();
					ctx.lineWidth = self.settings.timerStrokeWidth1;
					ctx.strokeStyle = 'rgba(' + strokeRed1 + ', ' + strokeGreen1 + ', ' + strokeBlue1 + ', ' + strokeOpacity1 + ')';	
					ctx.arc(timerPosition, timerPosition, self.settings.timerRadius, 0, 2 * Math.PI, false);
					ctx.stroke();
					
					// draw the animated circle
					ctx.beginPath();
					ctx.lineWidth = self.settings.timerStrokeWidth2;
					ctx.strokeStyle = 'rgba(' + strokeRed2 + ', ' + strokeGreen2 + ', ' + strokeBlue2 + ', ' + strokeOpacity2 + ')';				
					ctx.arc(timerPosition, timerPosition, self.settings.timerRadius, 0, angle * radians, false);
					ctx.stroke();
				}
			}, 20);
		}
		
		
		/**
		* Stops the timer animation and removes the canvas
		*/
		function stopTimerAnimation() {
			if (timerAnimationTimer)
				clearInterval(timerAnimationTimer);
			
			var timerCanvas = slider.find('.timer-animation');
			
			if (timerCanvas) {
				if (!(isOldIE))
					timerCanvas.stop().animate({'opacity': 0}, self.settings.timerFadeDuration, function(){timerCanvas.remove();});
				else
					timerCanvas.remove();
			}
		}
		
		
		/**
		* Returns the left offset of the slide based on the specified align type, and the difference between the slider's specified width and the slide's actual width
		*/
		function getLeftOffset(image, alignType) {
			var left = 0;
			
			if (alignType == 'centerTop' || alignType == 'centerCenter' || alignType == 'centerBottom')
				left = slideWrapper.width() / 2 + parseInt(image.css('margin-left'));
			else if (alignType == 'rightTop' || alignType == 'rightCenter' || alignType == 'rightBottom')
				left = slideWrapper.width() - parseInt(image.css('width'));
				
			return left;
		}
		
		
		
		/**
		* Returns the top offset of the slide based on the specified align type, and the difference between the slider's specified height and the slide's actual height
		*/
		function getTopOffset(image, alignType) {
			var top = 0;
			
			if (alignType == 'leftCenter' || alignType == 'centerCenter' || alignType == 'rightCenter')
				top = slideWrapper.height() / 2 + parseInt(image.css('margin-top'));
			else if (alignType == 'leftBottom' || alignType == 'centerBottom' || alignType == 'rightBottom')
				top = slideWrapper.height() - parseInt(image.css('height'));
			
			return top;
		}
		
		
		
		/**
		* Creates the thumbnail scroller
		*/
		function createThumbnailScroller() {
			thumbnailScroller = $('<div class="thumbnail-scroller"></div>').appendTo(slider);
			thumbnailsVisibleContainer = $('<div class="visible-container"></div>').appendTo(thumbnailScroller);
			thumbnailsContainer = $('<ul class="container"></ul>').appendTo(thumbnailsVisibleContainer);			
						
			if (self.settings.thumbnailOrientation == "horizontal")
				thumbnailScroller.addClass('horizontal');
			else
				thumbnailScroller.addClass('vertical');
			
			
			for (var i = 0; i < numSlides; i++) {
				if (slidesData[i].thumbnail && (slidesData[i].properties.thumbnailType == 'scroller' || slidesData[i].properties.thumbnailType == 'tooltipAndScroller')) {
					
					var hasLink = (slidesData[i].thumbnail).is('a'),
						thumbnailSource = hasLink ? (slidesData[i].thumbnail).find('img').removeClass('thumbnail') : slidesData[i].thumbnail,
						path = thumbnailSource.attr('src'),
						caption = thumbnailSource.attr('alt'),
						tooltip = thumbnailSource.attr('title');
					
					var	thumbnailWrapper = $('<li class="thumbnail-wrapper"></li>').appendTo(thumbnailsContainer),
						thumbnail;
					
					
					if (hasLink) {
						// if the thumbnail has a link, align the image from inside the 
						// anchor tag, but only after the image is loaded						
						thumbnail = slidesData[i].thumbnail;
						
						thumbnail.addClass('thumbnail')
								 .css({'width': self.settings.thumbnailWidth,
								 	   'height': self.settings.thumbnailHeight})
								 .attr('data-index', i)
								 .appendTo(thumbnailWrapper);
						
						if (thumbnailSource[0].complete || thumbnailSource[0].readyState == 'complete') {
							thumbnailSource.css('margin-left', (self.settings.thumbnailWidth - thumbnailSource.width()) * 0.5);
							thumbnailSource.css('margin-top', (self.settings.thumbnailHeight - thumbnailSource.height()) * 0.5);
							
							thumbnail.css('background-image', 'none');
						} else {							
							thumbnailSource.load(function() {
								$(this).css('margin-left', (self.settings.thumbnailWidth - $(this).width()) * 0.5);
								$(this).css('margin-top', (self.settings.thumbnailHeight - $(this).height()) * 0.5);
								
								$(this).parent().css('background-image', 'none');
							});
						}
					} else {
						thumbnail = $('<a class="thumbnail"></a>')
							.css({'background-image': 'url('+ path +')',
								  'width': self.settings.thumbnailWidth, 
								  'height': self.settings.thumbnailHeight})
							.attr('data-index', i)
							.appendTo(thumbnailWrapper);
						
						if (slidesData[i].thumbnail.attr('title'))
							thumbnail.attr('title', slidesData[i].thumbnail.attr('title'));
					}
					
					
					thumbnailWrapper.css({'width': thumbnail.outerWidth(), 'height': thumbnail.outerHeight()});
					
					if (self.settings.thumbnailLayers == -1 || self.settings.thumbnailLayers * self.settings.maximumVisibleThumbnails > numSlides)
						thumbnailLayers = Math.ceil(numSlides / self.settings.maximumVisibleThumbnails);
					else
						thumbnailLayers = self.settings.thumbnailLayers;
					
					
					// position the thumbnail
					if (self.settings.thumbnailOrientation == 'horizontal') {
						thumbnailWrapper.css('left', Math.floor(numThumbnails / thumbnailLayers) * thumbnailWrapper.outerWidth(true));
						thumbnailWrapper.css('top', (numThumbnails % thumbnailLayers) * thumbnailWrapper.outerHeight(true));
					} else {
						thumbnailWrapper.css('left', (numThumbnails % thumbnailLayers) * thumbnailWrapper.outerWidth(true));
						thumbnailWrapper.css('top', Math.floor(numThumbnails / thumbnailLayers) * thumbnailWrapper.outerHeight(true));
					}
					
					
					// create the thumbnail's caption
					if (caption) {
						var	thumbnailCaption = $('<div class="caption"></div>').appendTo(thumbnail),																					 
							thumbnailCaptionBackground = $('<div class="background"></div>').appendTo(thumbnailCaption),
							thumbnailCaptionContent = $('<div class="content">' + caption + '</div>').appendTo(thumbnailCaption);						
							
						thumbnailCaption.css('height', thumbnailCaptionContent.outerHeight(true));
							
						// set the initial position of the caption	
						if (self.settings.thumbnailCaptionPosition == 'top')
							thumbnailCaption.css({'top': 0});
						else
							thumbnailCaption.css({'bottom': 0});
							
						// change the caption's position, if the caption will be animated
						if (self.settings.thumbnailCaptionToggle)
							if (self.settings.thumbnailCaptionEffect == 'slide') {
								if (self.settings.thumbnailCaptionPosition == 'top')
									thumbnailCaption.css({'top': -thumbnailCaption.outerHeight(true)});
								else
									thumbnailCaption.css({'bottom': -thumbnailCaption.outerHeight(true)});
							} else {
								if (isOldIE) {
									thumbnailCaptionBackground.data('opacity', thumbnailCaptionBackground.css('opacity'));
									thumbnailCaptionBackground.css('opacity', 0);
									thumbnailCaptionContent.css('opacity', 0);
								} else {
									thumbnailCaption.css('opacity', 0);
								}
							}
					}
					
					
					// get the lightbox link
					var thumbnailLightboxLink = thumbnailWrapper.children('a[rel^="slider-lightbox"]'),
						slideLightboxLink = slideDivs[i].children('a[rel^="slider-lightbox"]'),
						useThumbnailLightbox = thumbnailLightboxLink.length && self.settings.lightboxIcon,
						useSlideLightbox = slideLightboxLink.length && self.settings.thumbnailLightboxIcon;
						
					
					if (useThumbnailLightbox || useSlideLightbox) {				
						var lightboxContent = useThumbnailLightbox ? thumbnailLightboxLink.attr('href') : slideLightboxLink.attr('href'),
							lightboxType,
							lightboxIcon;
						
						
						// check the type of lightbox content
						if (lightboxContent.indexOf('iframe=true') != -1 || lightboxContent.indexOf('ajax=true') != -1 || lightboxContent.indexOf('custom=true') != -1)
							lightboxType = 'link';
						else if (lightboxContent.indexOf('youtube.com') != -1 || lightboxContent.indexOf('youtu.be') != -1 || lightboxContent.indexOf('vimeo.com') != -1)
							lightboxType = 'video';
						else
							lightboxType = 'image';
							
						
						// create the lightbox icon
						lightboxIcon = $('<div class="lightbox-icon"></div>').appendTo(thumbnail);
						
						
						// add the appropriate image to the lightbox icon
						if (lightboxType == 'video')
							lightboxIcon.addClass('play');
						else if (lightboxType == 'link')
							lightboxIcon.addClass('hyperlink');
						else if (lightboxType == 'image')
							lightboxIcon.addClass('magnifying');
							
							
						// display the icon
						if (self.settings.thumbnailLightboxIconToggle)
							lightboxIcon.css('opacity', 0);
					}
					
					
					
					// change the thumbnail's style
					// hide/show the thumbnail's caption
					thumbnail.hover(
						function() {									
							if (!$(this).hasClass('select'))
								$(this).addClass('over');
							
							
							var index = $(this).data('index');
							
							
							if (self.settings.thumbnailCaptionToggle) {
								var thumbnailCaption = $(this).children('.caption'),
									thumbnailCaptionBackground = thumbnailCaption.children('.background'),
									thumbnailCaptionContent = thumbnailCaption.children('.content'),
									oldIEOpacity = thumbnailCaptionBackground.data('opacity');
								
								if (self.settings.thumbnailCaptionEffect == 'slide') {
									if (self.settings.thumbnailCaptionPosition == 'top')
										thumbnailCaption.stop().animate({'top': 0}, 
																		self.settings.thumbnailCaptionShowDuration,
																		self.settings.thumbnailCaptionEasing);
									else
										thumbnailCaption.stop().animate({'bottom': 0}, 
																		self.settings.thumbnailCaptionShowDuration,
																		self.settings.thumbnailCaptionEasing);
								} else {
									if (isOldIE) {
										thumbnailCaptionBackground.stop().animate({'opacity': oldIEOpacity}, 
																				  self.settings.thumbnailCaptionShowDuration, 
																				  self.settings.thumbnailCaptionEasing);
										thumbnailCaptionContent.stop().animate({'opacity': 1}, 
																			   self.settings.thumbnailCaptionShowDuration, 
																			   self.settings.thumbnailCaptionEasing);
									} else {
										thumbnailCaption.stop().animate({'opacity': 1}, self.settings.thumbnailCaptionShowDuration, self.settings.thumbnailCaptionEasing);
									}
								}
							}
							
							
							// show the lightbox icon on mouse over
							if (self.settings.thumbnailLightboxIconToggle) {
								var lightboxIcon = $(this).children('.lightbox-icon');
								
								if (isOldIE)
									lightboxIcon.css('filter', '');
								else
									lightboxIcon.stop().animate({'opacity': 1}, self.settings.lightboxIconFadeDuration);									
							}
						
						
							if (self.settings.thumbnailTooltip)
								showTooltip($(this).attr('title'));
								
								
							// fire the 'thumbnailMouseOver' event	
							var eventObject = {type: 'thumbnailMouseOver', index: index};
							$.isFunction(self.settings.thumbnailMouseOver) && self.settings.thumbnailMouseOver.call(this, eventObject);
						},
			
						function() {
							if (!$(this).hasClass('select'))
								$(this).removeClass('over');
							
							
							var index = $(this).data('index');
								
								
							if (self.settings.thumbnailCaptionToggle) {
								var thumbnailCaption = $(this).children('.caption'),
									thumbnailCaptionBackground = thumbnailCaption.children('.background'),
									thumbnailCaptionContent = thumbnailCaption.children('.content');
								
								if (self.settings.thumbnailCaptionEffect == 'slide') {
									if (self.settings.thumbnailCaptionPosition == 'top')
										thumbnailCaption.stop().animate({'top': -thumbnailCaption.outerHeight(true)}, 
																		 self.settings.thumbnailCaptionHideDuration, 
																		 self.settings.thumbnailCaptionEasing);
									else
										thumbnailCaption.stop().animate({'bottom': -thumbnailCaption.outerHeight(true)}, 
																		 self.settings.thumbnailCaptionHideDuration, 
																		 self.settings.thumbnailCaptionEasing);
								} else {
									if (isOldIE) {
										thumbnailCaptionBackground.stop().animate({'opacity': 0}, 
																				   self.settings.thumbnailCaptionHideDuration, 
																				   self.settings.thumbnailCaptionEasing);
										thumbnailCaptionContent.stop().animate({'opacity': 0}, 
																				self.settings.thumbnailCaptionHideDuration, 
																				self.settings.thumbnailCaptionEasing);
									} else {
										thumbnailCaption.stop().animate({'opacity': 0}, self.settings.thumbnailCaptionHideDuration, self.settings.thumbnailCaptionEasing);
									}
								}
							}
							
							
							// hide the lightbox icon on mouse out
							if (self.settings.thumbnailLightboxIconToggle) {
								var lightboxIcon = $(this).children('.lightbox-icon');
								
								if (isOldIE)
									lightboxIcon.css('opacity', 0);
								else
									lightboxIcon.stop().animate({'opacity': 0}, self.settings.lightboxIconFadeDuration);									
							}
						
						
							if (self.settings.thumbnailTooltip)
								hideTooltip();
								
								
							// fire the 'thumbnailMouseOut' event	
							var eventObject = {type: 'thumbnailMouseOut', index: index};
							$.isFunction(self.settings.thumbnailMouseOut) && self.settings.thumbnailMouseOut.call(this, eventObject);
					});
						
					thumbnail.click(function() {
						var index = $(this).data('index');
						triggerType = 'thumbnail'; 
						
						// fire the 'thumbnailClick' event	
						var eventObject = {type: 'thumbnailClick', index: index};
						$.isFunction(self.settings.thumbnailClick) && self.settings.thumbnailClick.call(this, eventObject);
						
						if(!$(this).attr('href'))						
							gotoSlide(index);
					});
					
					
					// ensures an accurate count of the existing thumbnails
					numThumbnails++;
				}
			}
			
			
			// get the width and height of an individual thumbnail
			thumbnailWidth = thumbnailScroller.find('.thumbnail-wrapper').outerWidth(true);
			thumbnailHeight = thumbnailScroller.find('.thumbnail-wrapper').outerHeight(true);
			
			// calculate the initial thumbnail pages
			totalThumbnailPages = Math.ceil(numThumbnails / (self.settings.maximumVisibleThumbnails * thumbnailLayers));
			
			
			// set the initial width and height of the thumbnail scroller and thumbnail container
			if (self.settings.thumbnailOrientation == 'horizontal') {				
				thumbnailScroller.css({'height': thumbnailHeight * thumbnailLayers, 'width': thumbnailWidth * self.settings.maximumVisibleThumbnails});
				thumbnailsVisibleContainer.css({'height': thumbnailHeight * thumbnailLayers, 'width': thumbnailWidth * self.settings.maximumVisibleThumbnails});				
			} else {
				thumbnailScroller.css({'width': thumbnailWidth * thumbnailLayers, 'height': thumbnailHeight * self.settings.maximumVisibleThumbnails});				
				thumbnailsVisibleContainer.css({'width': thumbnailWidth * thumbnailLayers, 'height': thumbnailHeight * self.settings.maximumVisibleThumbnails});				
			}
			
				
			// create the next&previous arrows for the thumbnails
			if (self.settings.thumbnailArrows)
				createThumbnailArrows();
				
			
			// create/modify the thumbnail buttons
			if (self.settings.thumbnailButtons)
				createThumbnailButtons();
				
					
			// create the scrollbar
			if (self.settings.thumbnailScrollbar)
				createScrollbar();
			
			
			// activate the mouse scrolling
			if (self.settings.thumbnailMouseScroll)
				startThumbnailMouseScroll();
			
				
			// activate the mouse wheel scrolling
			if (self.settings.thumbnailMouseWheel)
				startThumbnailMouseWheel();			
			
			
			// create the tooltip and make it invisible
			if (self.settings.thumbnailTooltip)
				$('<div class="tooltip"><div class="content"></div></div>').hide().appendTo(thumbnailScroller);
			
			
			// track the mouse position	
			if (self.settings.thumbnailMouseScroll || self.settings.thumbnailTooltip || self.settings.thumbnailScrollbar) {
				$(document).bind('mousemove',function(event) {
					mouseX = event.pageX;
					mouseY = event.pageY;
				});
			}
			
			
			doThumbnailScrollerLayout();
			
			
			// show/hide the entire thumbnail scroller
			if (self.settings.thumbnailScrollerToggle) {
				thumbnailScroller.css({opacity: 0});
				
				thumbnailScroller.hover(
					function() {
						  if (isOldIE) 
							  thumbnailScroller.css('filter', '');
						  else	
							  thumbnailScroller.stop().animate({'opacity': 1}, self.settings.thumbnailScrollerShowDuration);
					},
		  
					function() {
						  if (isOldIE) 
							  thumbnailScroller.css('opacity', 0);
						  else
							  thumbnailScroller.stop().animate({'opacity': 0}, self.settings.thumbnailScrollerHideDuration);
					});
			}
		}
		
		
		/*
		* Set the thumbnail scroller's size and position, and the size and position of the thumbnail scroller's controls
		*/
		function doThumbnailScrollerLayout() {
			// set the size and position of the thumbnail's container and the main thumbnail scroller
			// besed on the orientation of the thumbnails, the thumbnail's size and the amount of visible thumbnails
			if (self.settings.thumbnailOrientation == 'horizontal') {
				var arrowsSize = self.settings.thumbnailArrows ? (parseInt(thumbnailScroller.css('paddingLeft')) + parseInt(thumbnailScroller.css('paddingRight'))) : 0;
				
				thumbnailsTotalSize = Math.ceil(numThumbnails / thumbnailLayers) * thumbnailWidth;
				
				visibleThumbnails = self.settings.thumbnailScrollerResponsive ? 
									Math.max(self.settings.minimumVisibleThumbnails, 
											 Math.min(Math.ceil(numThumbnails / thumbnailLayers), 
											 		  self.settings.maximumVisibleThumbnails, 
													  Math.floor((slideWrapper.outerWidth() - arrowsSize) / thumbnailWidth))) : 
									Math.min(numThumbnails, self.settings.maximumVisibleThumbnails); 
				
				thumbnailScroller.css('width', thumbnailWidth * visibleThumbnails);																			 
				thumbnailsVisibleContainer.css('width', thumbnailWidth * visibleThumbnails);				
			} else {
				var arrowsSize = self.settings.thumbnailArrows ? (parseInt(thumbnailScroller.css('paddingTop')) + parseInt(thumbnailScroller.css('paddingBottom'))) : 0;
				
				thumbnailsTotalSize = Math.ceil(numThumbnails / thumbnailLayers) * thumbnailHeight;
				
				visibleThumbnails = self.settings.thumbnailScrollerResponsive ?
									Math.max(self.settings.minimumVisibleThumbnails,  
											 Math.min(Math.ceil(numThumbnails / thumbnailLayers), 
											 		  self.settings.maximumVisibleThumbnails, 
													  Math.floor((slideWrapper.outerHeight() - arrowsSize) / thumbnailHeight))) : 
									Math.min(numThumbnails, self.settings.maximumVisibleThumbnails); 
				
				thumbnailScroller.css('height', visibleThumbnails * thumbnailHeight);				
				thumbnailsVisibleContainer.css('height', visibleThumbnails * thumbnailHeight);
			}
			
			
			// center the thumbnail scroller
			if (self.settings.thumbnailScrollerCenter) {
				if (self.settings.thumbnailOrientation == 'horizontal')
					thumbnailScroller.css('left', (sliderMain.outerWidth() - thumbnailScroller.outerWidth()) / 2);
				else if (self.settings.thumbnailOrientation == 'vertical')
					thumbnailScroller.css('top', (slideWrapper.outerHeight() - thumbnailScroller.outerHeight()) / 2);
			}
			
			
			// check if the number of visible thumbnails has changed
			if (previousVisibleThumbnails != visibleThumbnails) {
				previousVisibleThumbnails = visibleThumbnails;
				
				totalThumbnailPages = Math.ceil(numThumbnails / (visibleThumbnails * thumbnailLayers));
				
				// modify the thumbnail buttons
				if (self.settings.thumbnailButtons && self.settings.thumbnailScrollerResponsive)
					createThumbnailButtons();
				
				// modify the thumbnail scrollbasr	
				if (self.settings.thumbnailScrollbar && self.settings.thumbnailScrollerResponsive) {
					if (self.settings.thumbnailOrientation == 'horizontal') {
						thumbnailScrollerScrollbar.css('width', thumbnailScroller.width());
						thumbnailScrollerScrollbarMiddle.css('width', thumbnailScrollerScrollbar.width() - thumbnailScrollerScrollbarEdgeSize);
						thumbnailScrollerScrollbar.css('left', (thumbnailScroller.outerWidth() - thumbnailScrollerScrollbar.width()) / 2);
					} else {
						thumbnailScrollerScrollbar.css('height', thumbnailScroller.height());
						thumbnailScrollerScrollbarMiddle.css('height', thumbnailScrollerScrollbar.height() - thumbnailScrollerScrollbarEdgeSize);
						thumbnailScrollerScrollbar.css('top', (thumbnailScroller.outerHeight() - thumbnailScrollerScrollbar.height()) / 2);
					}
				}
				
				scrollToThumbnailPage(currentThumbnailPage);
			}
		}
		
		
		/*
		* Create the thumbnail arrows
		*/
		function createThumbnailArrows() {
			thumbnailScrollerArrows = $('<div class="arrows"></div>').appendTo(thumbnailScroller);
			
			var type = self.settings.thumbnailOrientation == 'horizontal' ? '' : 'vertical ',
			
				previousArrow = $('<a class="previous"></a>')
					.click(function() {
						scrollToPreviousThumbnailPage();
					})
					.appendTo(thumbnailScrollerArrows),
			
			
				nextArrow = $('<a class="next"></a>')
					.click(function() {
						scrollToNextThumbnailPage();
					})
					.appendTo(thumbnailScrollerArrows);
			
			
			// set the position of the arrows
			// and reset the size and position of the thumbnail containers
			if (self.settings.thumbnailOrientation == 'horizontal') {						
				previousArrow.css({'top': Math.round((thumbnailHeight * thumbnailLayers - previousArrow.height()) / 2)});
				nextArrow.css({'top': Math.round((thumbnailHeight * thumbnailLayers - nextArrow.height()) / 2)});
			} else {
				previousArrow.css({'left': Math.round((thumbnailWidth * thumbnailLayers - previousArrow.width()) / 2)});
				nextArrow.css({'left': Math.round((thumbnailWidth * thumbnailLayers - nextArrow.width()) / 2)});
			}
			
			
			// show/hide the arrows
			if (self.settings.thumbnailArrowsToggle) {
				if ($.browser.msie) 
					thumbnailScrollerArrows.hide();
				else	
					thumbnailScrollerArrows.css('opacity', 0);
				
				
				thumbnailScroller.hover(
					function() {
						if ($.browser.msie) 
							thumbnailScrollerArrows.show();
						else
							thumbnailScrollerArrows.stop().animate({'opacity': 1}, self.settings.thumbnailArrowsShowDuration);
					},
					
					function() {
						if ($.browser.msie) 
							thumbnailScrollerArrows.hide();
						else
							thumbnailScrollerArrows.stop().animate({'opacity': 0}, self.settings.thumbnailArrowsHideDuration);
					});
			}
		}
		
		
		/*
		* Create the thumbnail buttons
		*/
		function createThumbnailButtons() {
			if (!thumbnailScrollerButtons) {
				thumbnailScrollerButtons = $('<div class="buttons"></div>').appendTo(thumbnailScroller);
				
				// show/hide the buttons
				if (self.settings.thumbnailButtonsToggle) {
					if (isOldIE)
						thumbnailScrollerButtons.hide();
					else
						thumbnailScrollerButtons.css('opacity', 0);
					
					
					thumbnailScroller.hover(function() {
						if (isOldIE)
							thumbnailScrollerButtons.show();
						else
							thumbnailScrollerButtons.stop().animate({opacity: 1}, self.settings.thumbnailButtonsShowDuration);														
					},
					
					function() {
						if (isOldIE)
							thumbnailScrollerButtons.hide();
						else
							thumbnailScrollerButtons.stop().animate({opacity: 0}, self.settings.thumbnailButtonsShowDuration);
					});
				}
			} 
			
			
			if (totalThumbnailPages == 1) {
				thumbnailScrollerButtons.empty();
			} else if (thumbnailScrollerButtons.find('.buttons a').length != totalThumbnailPages && totalThumbnailPages > 1) {
				thumbnailScrollerButtons.empty();
				
				var	buttonSize;
				
				for (var i = 0; i < totalThumbnailPages; i++) {
					var button = $('<a rel="' + i + '"></a>').appendTo(thumbnailScrollerButtons);
					
					// position the buttons
					if (self.settings.thumbnailOrientation == 'horizontal') {
						buttonSize = button.outerWidth(true);
						button.css('left', i * buttonSize);
					} else {
						buttonSize = button.outerHeight(true);
						button.css('top', i * buttonSize);
					}
					
					// handle mouse interaction
					button.bind({mouseover: function() {									
									if (!$(this).hasClass('select'))
										$(this).addClass('over');
								},
					
								mouseout: function() {
									if (!$(this).hasClass('select'))
										$(this).removeClass('over');
								},
					
								click: function() {
									var index = parseInt($(this).attr('rel'));
									scrollToThumbnailPage(index);
								}
					});
				}		
				
				// set the size and position of the thumbnail buttons
				// reset the height/width of the thumbnail scroller
				if (self.settings.thumbnailOrientation == 'horizontal') {	
					thumbnailScrollerButtons.css('top', thumbnailHeight * thumbnailLayers);					
					thumbnailScrollerButtons.css('width', buttonSize * totalThumbnailPages);
					thumbnailScrollerButtons.css('height', buttonSize);
					thumbnailScroller.css('height', Math.max(thumbnailScroller.height(), thumbnailScrollerButtons.position().top + thumbnailScrollerButtons.outerHeight(true)));
				} else {
					thumbnailScrollerButtons.css('left', thumbnailWidth * thumbnailLayers);
					thumbnailScrollerButtons.css('height', buttonSize * totalThumbnailPages);
					thumbnailScrollerButtons.css('width', buttonSize);
					thumbnailScroller.css('width', Math.max(thumbnailScroller.width(), thumbnailScrollerButtons.position().left + thumbnailScrollerButtons.outerWidth(true)));
				}
				
				
				if (currentThumbnailPage > totalThumbnailPages - 1)
					currentThumbnailPage = totalThumbnailPages - 1
					
					
				thumbnailScrollerButtons.find('a').eq(currentThumbnailPage).addClass('select');
			}
			
			
			// set the postion of the thumbnail buttons's container
			if (self.settings.thumbnailOrientation == 'horizontal')
				thumbnailScrollerButtons.css('left', (thumbnailScroller.outerWidth() - thumbnailScrollerButtons.outerWidth()) / 2);
			else
				thumbnailScrollerButtons.css('top', (thumbnailScroller.outerHeight() - thumbnailScrollerButtons.outerHeight()) / 2);
		}
		
		
		/**
		* Scrolls the thumbnails to the next page
		*/
		function scrollToNextThumbnailPage() {			
			if (currentThumbnailPage < totalThumbnailPages - 1) {
				currentThumbnailPage++;
				scrollToThumbnailPage(currentThumbnailPage);
			} else {
				scrollToThumbnailPage(0);
			}
		}
		
		
		/**
		* Scrolls the thumbnails to the previous page
		*/
		function scrollToPreviousThumbnailPage() {			
			if (currentThumbnailPage > 0) {
				currentThumbnailPage--;
				scrollToThumbnailPage(currentThumbnailPage);
			} else {
				scrollToThumbnailPage(totalThumbnailPages - 1);	
			}
		}
		
		
		/**
		* Scrolls the thumbnails to a specified page
		*/
		function scrollToThumbnailPage(index) {
			var	pageSize = self.settings.thumbnailOrientation == 'horizontal' ? thumbnailsVisibleContainer.width() : thumbnailsVisibleContainer.height(),
				targetPosition = index * pageSize;
				
			currentThumbnailPage = index;
			
			if (currentThumbnailPage == totalThumbnailPages - 1)
				targetPosition = thumbnailsTotalSize - pageSize;
			
			onThumbnailScrollStart();
			
			var animObj = self.settings.thumbnailOrientation == 'horizontal' ? {left: -targetPosition} : {top: -targetPosition};
			
			thumbnailsContainer.animate(animObj, {duration: self.settings.thumbnailScrollDuration, 
							   			 		  easing: self.settings.thumbnailScrollEasing, 
										 		  step: function() {onThumbnailScrollProgress();},
										 		  complete: function() {onThumbnailScrollComplete();}});
			
			if (self.settings.thumbnailButtons) {
				var thumbnailScrollerButtons = thumbnailScroller.find('.buttons');
					
				thumbnailScrollerButtons.find('.select').removeClass('select');
				thumbnailScrollerButtons.find('a').eq(index).removeClass('over').addClass('select');
			}
		}
		
		
		/**
		* Enables the mouse scrolling
		*/
		function startThumbnailMouseScroll() {
			self.settings.thumbnailMouseScroll = true;
			
			var increment = 0,
				ratio,
				targetPosition,
				init = false, 
				visibleContainerTop = thumbnailsVisibleContainer.offset().top,
				visibleContainerLeft = thumbnailsVisibleContainer.offset().left,
				visibleContainerWidth = thumbnailsVisibleContainer.width(),
				visibleContainerHeight = thumbnailsVisibleContainer.height(),
				visibleContainerSize = self.settings.thumbnailOrientation == 'horizontal' ? visibleContainerWidth : visibleContainerHeight,
				visibleContainerPosition = self.settings.thumbnailOrientation == 'horizontal' ? visibleContainerLeft : visibleContainerTop,
				prop = self.settings.thumbnailOrientation == 'horizontal' ? 'left' : 'top',
				thumbnailsPosition = parseInt(thumbnailsContainer.css(prop));
				
			// start moving the thumbnails
			thumbnailMouseScrollTimer = setInterval(function() {
				if (!init) {
					init = true;
					visibleContainerTop = thumbnailsVisibleContainer.offset().top;
					visibleContainerLeft = thumbnailsVisibleContainer.offset().left;
					visibleContainerWidth = thumbnailsVisibleContainer.width();
					visibleContainerHeight = thumbnailsVisibleContainer.height();
				}
				
				if (mouseX > visibleContainerLeft && 
					mouseX < (visibleContainerLeft + visibleContainerWidth) && 
					mouseY > visibleContainerTop && 
					mouseY < (visibleContainerTop + visibleContainerHeight)) {
						
					var mousePosition = self.settings.thumbnailOrientation == 'horizontal' ? mouseX : mouseY,
						ratio = (mousePosition - visibleContainerPosition) / visibleContainerSize,
						targetPosition = -((thumbnailsTotalSize - visibleContainerSize + 200) * ratio) + 100;
				
					increment = Math.round((targetPosition - thumbnailsPosition) * self.settings.thumbnailMouseScrollSpeed / 100);
					thumbnailsPosition += increment;					
				} else {							
					if (Math.abs(increment) > 0.1) {
						increment *= (self.settings.thumbnailMouseScrollEase / 100);
						thumbnailsPosition += increment;
					} else {
						increment = 0;
					}
				}
				
				if (Math.abs(increment) > 0) {
					if (thumbnailsPosition >= 0)
						thumbnailsPosition = 0;					
					else if (thumbnailsPosition <= visibleContainerSize - thumbnailsTotalSize)
						thumbnailsPosition = visibleContainerSize - thumbnailsTotalSize;
					
					onThumbnailScrollProgress();
					thumbnailsContainer.css(prop, thumbnailsPosition); 
				}
				
			}, 30);
			
		}
		
		
		/**
		* Disables the mouse scrolling
		*/
		function stopThumbnailMouseScroll() {
			self.settings.thumbnailMouseScroll = false;
			clearInterval(thumbnailMouseScrollTimer);
		}
		
		
		/**
		* Enables the mouse wheel scrolling
		*/
		function startThumbnailMouseWheel() {
			self.settings.thumbnailMouseWheel = true;
			
			var targetPosition = 0,
				directionSign = self.settings.thumbnailMouseWheelReverse ? -1 : 1,
				currentPosition,
				prop = self.settings.thumbnailOrientation == 'horizontal' ? 'left' : 'top',
				visibleContainerSize = self.settings.thumbnailOrientation == 'horizontal' ? thumbnailsVisibleContainer.width() : thumbnailsVisibleContainer.height();
				
				
			thumbnailsVisibleContainer.bind('mousewheel', function(event, delta) {
				// disable page scrolling
				event.preventDefault();
				
				// if the mouse wheel scrolling is not already started, start it
				if (!isThumbnailMouseWheelScrolling) {
					onThumbnailScrollStart();
					isThumbnailMouseWheelScrolling = true;
					currentPosition = parseInt(thumbnailsContainer.css(prop));
					targetPosition = currentPosition;
					thumbnailMouseWheelTimer = setInterval(function() {					
						if (Math.abs(targetPosition - currentPosition) > 0.5) {
							var increment = (targetPosition - currentPosition) * (self.settings.thumbnailMouseWheelSpeed / 100);
							currentPosition += increment;
							onThumbnailScrollProgress();
							thumbnailsContainer.css(prop, currentPosition);
						} else {
							onThumbnailScrollComplete();
						}
					}, 30);
				}
				
				targetPosition += directionSign * delta * 10;
				
				if (targetPosition >= 0)
					targetPosition = 0;					
				else if (targetPosition <= visibleContainerSize - thumbnailsTotalSize)
					targetPosition = visibleContainerSize - thumbnailsTotalSize;
				
			});
		}
		
		
		/**
		* Disables the mouse wheel scrolling
		*/
		function stopThumbnailMouseWheel() {
			self.settings.thumbnailMouseWheel = false;
			isThumbnailMouseWheelScrolling = false;
			clearInterval(thumbnailMouseWheelTimer);
		}
		
		
		/**
		* Creates the scrollbar
		*/
		function createScrollbar() {
			// add the scrollbar to the carousel area
			thumbnailScrollerScrollbar = $('<div class="scrollbar"></div>').appendTo(thumbnailScroller);
			
			var	bck = $('<div class="back"></div>').appendTo(thumbnailScrollerScrollbar),
				track = $('<div class="track"></div>').appendTo(thumbnailScrollerScrollbar),
				fwd = $('<div class="forward"></div>').appendTo(thumbnailScrollerScrollbar),
				trackMiddle = $('<div class="track-middle"></div>').appendTo(track),
				trackBck = $('<div class="track-back"></div>').appendTo(track),
				trackFwd = $('<div class="track-forward"></div>').appendTo(track),
				thumb = $('<div class="thumb"></div>').appendTo(track),
				thumbMiddle = $('<div class="thumb-middle"></div>').appendTo(thumb),
				thumbBck = $('<div class="thumb-back"></div>').appendTo(thumb),
				thumbFwd = $('<div class="thumb-forward"></div>').appendTo(thumb),
				thumbPosition = 0,
				thumbOffset,
				scrollbarPosition = 0,
				currentPosition = 0,
				sizeProp = self.settings.thumbnailOrientation == 'horizontal' ? 'width' : 'height',
				positionProp = self.settings.thumbnailOrientation == 'horizontal' ? 'left' : 'top';
			
			thumbnailScrollerScrollbar.addClass(self.settings.scrollbarSkin);
			
			thumbnailScrollerScrollbarTrack = track;
			
			
			// position the scrollbar
			if (self.settings.thumbnailOrientation == 'horizontal') {
				thumbnailScrollerScrollbar.addClass('horizontal');				
				thumbnailScrollerScrollbar.css({'top': thumbnailHeight * thumbnailLayers, 'left': (thumbnailScroller.outerWidth() - thumbnailScrollerScrollbar.width()) / 2});
				thumbnailScroller.css('height', Math.max(thumbnailScroller.height(), thumbnailScrollerScrollbar.position().top + thumbnailScrollerScrollbar.outerHeight(true)));
			} else {
				thumbnailScrollerScrollbar.addClass('vertical');				
				thumbnailScrollerScrollbar.css({'left': thumbnailWidth * thumbnailLayers, 'top': (thumbnailScroller.outerHeight() - thumbnailScrollerScrollbar.height()) / 2});
				thumbnailScroller.css('width', Math.max(thumbnailScroller.width(), thumbnailScrollerScrollbar.position().left + thumbnailScrollerScrollbar.outerWidth(true)));
			}
			
			
			if (bck.width() == 0 && fwd.width() == 0) {
				thumbnailScrollerScrollbarMiddle = trackMiddle;
				
				if (self.settings.thumbnailOrientation == 'horizontal')
					thumbnailScrollerScrollbarEdgeSize = trackBck.width() + trackFwd.width();
				else
					thumbnailScrollerScrollbarEdgeSize = trackBck.height() + trackFwd.height();
			} else {
				thumbnailScrollerScrollbarMiddle = track;
				
				if (self.settings.thumbnailOrientation == 'horizontal')
					thumbnailScrollerScrollbarEdgeSize = bck.width() + fwd.width();
				else
					thumbnailScrollerScrollbarEdgeSize = bck.height() + fwd.height();
			}
			
			
			// show/hide the scrollbar
			if (self.settings.thumbnailScrollbarToggle) {											
											
				if (isOldIE)
					thumbnailScrollerScrollbar.hide();
				else
					thumbnailScrollerScrollbar.css('opacity', 0);
					
						
				thumbnailScroller.hover(
					function() {
						if (isOldIE)
							thumbnailScrollerScrollbar.show();
						else
							thumbnailScrollerScrollbar.stop().animate({opacity: 1}, self.settings.thumbnailScrollbarShowDuration);
					},
					function() {
						if (isOldIE)
							thumbnailScrollerScrollbar.hide();
						else
							thumbnailScrollerScrollbar.stop().animate({opacity: 0}, self.settings.thumbnailScrollbarShowDuration);
					});
			}
					
			
			// when the thumb is pressed, start registering its movement	
			thumb.bind('mousedown', function(event) {
				event.preventDefault();
				var mousePosition = self.settings.thumbnailOrientation == 'horizontal' ? mouseX : mouseY;
				thumbOffset = mousePosition - thumb.offset()[positionProp];
				isThumbnailScrollbarDragging = true;
				$(document).bind('mousemove', mouseMoveHandler);
			});
			
			
			// stop registering the movement when the mouse is released
			$(document).bind('mouseup', function() {
				if (isThumbnailScrollbarDragging) {
					isThumbnailScrollbarDragging = false;
					$(document).unbind('mousemove', mouseMoveHandler);
				}
			});
			
			
			// move the thumb
			function mouseMoveHandler() {
				var mousePosition = self.settings.thumbnailOrientation == 'horizontal' ? mouseX : mouseY;
				thumbPosition = mousePosition - thumbnailScrollerScrollbarTrack.offset()[positionProp] - thumbOffset;		
				move();
			}
			
			// move the thumb on left arrow click
			bck.bind('click', function() {
				thumbPosition = parseInt(thumb.css(positionProp)) - self.settings.scrollbarArrowScrollAmount;
				move();
			});
			
			// move the thumb or right arrow click
			fwd.bind('click', function() {
				thumbPosition = parseInt(thumb.css(positionProp)) + self.settings.scrollbarArrowScrollAmount;
				move();
			});
			
			
			function move() {
				// keep the thumb within bounderies
				if (thumbPosition < 0)
					thumbPosition = 0;
				else if (thumbPosition > parseInt(thumbnailScrollerScrollbarTrack.css(sizeProp)) - parseInt(thumb.css(sizeProp)))
					thumbPosition =  parseInt(thumbnailScrollerScrollbarTrack.css(sizeProp)) - parseInt(thumb.css(sizeProp));
				
				// move the thumb
				if (isThumbnailScrollbarDragging)
					thumb.css(positionProp, thumbPosition);
				
				// calculate the scrollbar position (a number between 0 and 1)
				scrollbarPosition = thumbPosition / (parseInt(thumbnailScrollerScrollbarTrack.css(sizeProp)) - parseInt(thumb.css(sizeProp)));
				
				// update the position of the thumbnails based on the thumb's position
				if (!isThumbnailScrollbarMoving) {
					onThumbnailScrollStart();
					isThumbnailScrollbarMoving = true;
					currentPosition = parseInt(thumbnailsContainer.css(positionProp));
					
					thumbnailScrollbarTimer = setInterval(function() {
						if (Math.abs(getThumbnailsPosition() + scrollbarPosition) > 0.001) {
							var newPos = - scrollbarPosition * (thumbnailsTotalSize - parseInt(thumbnailsVisibleContainer.css(sizeProp))),
				 				increment = (newPos - currentPosition) * self.settings.thumbnailScrollbarEase / 100;
							currentPosition += increment;
							onThumbnailScrollProgress();
							
				
							thumbnailsContainer.css(positionProp, currentPosition);
						} else if (isThumbnailScrollbarMoving) {
							onThumbnailScrollComplete();
						}
					}, 30);
				}
			}
			
		}
		
		
		/**
		* A number from 0 to 1, representing the current position of the thumbnails
		*/
		function getThumbnailsPosition() {
			var positionProp = self.settings.thumbnailOrientation == 'horizontal' ? 'left' : 'top',
				sizeProp = self.settings.thumbnailOrientation == 'horizontal' ? 'width' : 'height',
				position = parseFloat(thumbnailsContainer.css(positionProp)) / (thumbnailsTotalSize - parseInt(thumbnailsVisibleContainer.css(sizeProp)));
			
			return position;
		}
		
		
		/**
		* Clears all the timers
		* This has to be done before any type of scrolling in order to prevent conflicts
		*/
		function clearThumbnailScrollingTimers() {			
			if (thumbnailMouseScrollTimer) {
				clearInterval(thumbnailMouseScrollTimer);
			}
			
			if (thumbnailMouseWheelTimer) {
				isThumbnailMouseWheelScrolling = false;
				clearInterval(thumbnailMouseWheelTimer);
			}
			
			if (thumbnailScrollbarTimer) {
				isThumbnailScrollbarMoving = false;
				clearInterval(thumbnailScrollbarTimer);	
			}
		}
		
		
		/**
		* This is called before starting any type of scrolling
		*/
		function onThumbnailScrollStart() {
			clearThumbnailScrollingTimers();
			
			if (thumbnailsContainer)
				if (thumbnailsContainer.is(':animated'))
					thumbnailsContainer.stop();
		}
		
		
		/**
		* This is called after the scrolling is complete
		*/
		function onThumbnailScrollComplete() {
			clearThumbnailScrollingTimers();
			
			// restart the mouse scrolling
			if (self.settings.thumbnailMouseScroll)
				startThumbnailMouseScroll();
		}
		
		
		/**
		* This is called during the scrolling process
		*/
		function onThumbnailScrollProgress() {
			if (self.settings.thumbnailScrollbar && !isThumbnailScrollbarDragging) {
				var thumb = thumbnailScrollerScrollbarTrack.find('.thumb'),
					positionProp = self.settings.thumbnailOrientation == 'horizontal' ? 'left' : 'top',
					sizeProp = self.settings.thumbnailOrientation == 'horizontal' ? 'width' : 'height',
					maxValue = parseInt(thumbnailScrollerScrollbarTrack.css(sizeProp)) - parseInt(thumb.css(sizeProp)),
					value = - getThumbnailsPosition() * maxValue;
				
				if (value >= 0 && value <= maxValue)
					thumb.css(positionProp, value);
			}
		}
		
		
		/**
		* Shows the tooltip
		*/
		function showTooltip(content) {
			if (!content)
				return;
			
			var tooltip = slider.find('.tooltip');
			
			// add the text
			tooltip.find('.content').html(content);
			
			// fade in
			tooltip.stop(false, true).fadeIn(self.settings.tooltipShowDuration);
			
			// calculate the position based on the size of the tooltip
			var tooltipLeft = - tooltip.outerWidth() / 2,
				tooltipTop = 0 - tooltip.outerHeight() - parseInt(tooltip.css('marginBottom'));
			
			// assign the values at start
			tooltip.css({'left': mouseX - thumbnailScroller.offset().left + tooltipLeft, 'top': mouseY - thumbnailScroller.offset().top + tooltipTop});
			
			// update the position as the mouse moves
			$(document).bind('mousemove.tooltip', function() {
				tooltip.css({'left': mouseX - thumbnailScroller.offset().left + tooltipLeft, 'top': mouseY - thumbnailScroller.offset().top + tooltipTop});
			});
		}
		
		
		/**
		* Hides the tooltip
		*/
		function hideTooltip() {
				
			var tooltip = slider.find('.tooltip');
			
			if (tooltip) {
				tooltip.stop(false, true)
					   .fadeOut(self.settings.tooltipHideDuration, 
							function() {
								$(document).unbind('mousemove.tooltip');
								// position the tooltip outside of any visible area
								tooltip.css('left', -9999);
							});
			}
		}	
		
		
		/**
		* Returns a new array, with the slices ordered based on the specified pattern
		*/
		function getOrderedSlices(initialArray, pattern, horizontalSlices, verticalSlices) {
			var orderedArray = new Array(),
				i, j, k, l = 0;			
			
			switch(pattern) {
				case 'randomPattern':
					var randomArray = new Array();
										
					while(initialArray.length) {
						l = Math.floor(Math.random() * initialArray.length);
						randomArray.push(initialArray[l]);
						initialArray.splice(l, 1);
					}					
					var n = randomArray.length;					
					for(k = 0; k < n; k++) {
						orderedArray[k] = randomArray[k];
					}					
					break;					
				
				case 'topToBottom':
					for(j = 0; j < verticalSlices; j++)
						for(i = 0; i < horizontalSlices; i++) {
							orderedArray.push(getSliceByPosition(initialArray, i, j));
						}					
					break;					
				
				case 'bottomToTop':
					for (j = verticalSlices - 1; j >= 0; j--)
						for (i = horizontalSlices - 1; i >= 0; i--) {
							orderedArray.push(getSliceByPosition(initialArray, i, j));
						}					
					break;
									
				case 'rightToLeft':
					for (i = horizontalSlices - 1; i >= 0; i--)
						for (j = verticalSlices - 1; j >= 0; j--) {
							orderedArray.push(getSliceByPosition(initialArray, i, j));
						}					
					break;	
				
				case 'leftToRight':
					for (i = 0; i < horizontalSlices; i++)
						for( j = 0; j < verticalSlices; j++) {
							orderedArray.push(getSliceByPosition(initialArray, i, j));
						}					
					break;
				
				case 'topLeftToBottomRight':
					for (k = 0; k < horizontalSlices + verticalSlices - 1; k++) {
						j = 0;
						for (i = k; i >= 0; i--){							
							if (getSliceByPosition(initialArray, i, j) != undefined) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
							}
							j++;
						}
					}					
					break;	
				
				case 'bottomLeftToTopRight':
					l = horizontalSlices > verticalSlices ? horizontalSlices : verticalSlices;
					
					for (k = horizontalSlices - 1; k >= 1 - l; k--) {
						i = 0;
						for (j = k; j <= horizontalSlices - 1; j++) {
							if (getSliceByPosition(initialArray, i, j) != undefined) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
							}
							i++;
						}
					}					
					break;	
				
				case 'topRightToBottomLeft':
					l = horizontalSlices > verticalSlices ? horizontalSlices : verticalSlices;
					
					for (k = horizontalSlices - 1; k >= 1 - l; k--) {
						i = k;
						for (j = 0; j <= verticalSlices - 1; j++) {
							if (getSliceByPosition(initialArray, i, j) != undefined) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
							}
							i++;
						}
					}					
					break;	
				
				case 'bottomRightToTopLeft':
					for (k = verticalSlices + horizontalSlices - 2; k >= 0; k--) {
						j = 0;
						for (i = k; i >= 0; i--) {
							if (getSliceByPosition(initialArray, i, j) != undefined) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
							}
							j++;
						}
					}					
					break;	
			
				case 'horizontalMarginToCenter':
					if (horizontalSlices % 2) {
						for (i = 0; i < Math.floor(horizontalSlices / 2); i++)
							for (j = 0; j < verticalSlices; j++) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
								orderedArray.push(getSliceByPosition(initialArray, horizontalSlices - 1 - i, j));
							}
						for (k = 0; k < verticalSlices; k++) {
							orderedArray.push(getSliceByPosition(initialArray, Math.floor(horizontalSlices / 2), k));
						}
					} else {
						for (i = 0; i < Math.floor(horizontalSlices / 2); i++)
							for (j = 0; j < verticalSlices; j++) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
								orderedArray.push(getSliceByPosition(initialArray, horizontalSlices - 1 - i, j));
							}
					}					
					break;	
				
				case 'horizontalCenterToMargin':
					if (horizontalSlices % 2) {
						for (k = 0; k < verticalSlices; k++) {
							orderedArray.push(getSliceByPosition(initialArray, Math.floor(horizontalSlices / 2), k));
						}
						for (i = Math.floor(horizontalSlices / 2) - 1; i >= 0; i--)
							for (j = 0; j < verticalSlices; j++) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
								orderedArray.push(getSliceByPosition(initialArray, horizontalSlices - 1 - i, j));
							}
					} else {
						for (i = Math.floor(horizontalSlices / 2) - 1; i >= 0; i--)
							for (j = 0; j < verticalSlices; j++) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
								orderedArray.push(getSliceByPosition(initialArray, horizontalSlices - 1 - i, j));
							}
					}					
					break;	
				
				case 'verticalMarginToCenter':
					if (verticalSlices % 2) {
						for (j = 0; j < Math.floor(verticalSlices / 2); j++)
							for (i = 0; i < horizontalSlices; i++) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
								orderedArray.push(getSliceByPosition(initialArray, i, verticalSlices - 1 - j));
							}
						for (k = 0; k < horizontalSlices; k++) {
							orderedArray.push(getSliceByPosition(initialArray, k, Math.floor(verticalSlices / 2)));
						}
					} else {
						for (j = 0; j < Math.floor(verticalSlices / 2); j++)
							for (i = 0; i < horizontalSlices; i++) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
								orderedArray.push(getSliceByPosition(initialArray, i, verticalSlices - 1 - j));
							}
					}					
					break;	
				
				case 'verticalCenterToMargin':
					if (verticalSlices % 2) {
						for (k = 0; k < horizontalSlices; k++) {
							orderedArray.push(getSliceByPosition(initialArray, k, Math.floor(verticalSlices / 2)));
						}
						for (j = Math.floor(verticalSlices / 2) - 1; j >= 0; j--)
							for(i = 0; i < horizontalSlices; i++) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
								orderedArray.push(getSliceByPosition(initialArray, i, verticalSlices - 1 - j));
							}
					} else {
						for (j = Math.floor(verticalSlices / 2) - 1; j >= 0; j--)
							for (i = 0; i < horizontalSlices; i++) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
								orderedArray.push(getSliceByPosition(initialArray, i, verticalSlices - 1 - j));
							}
					}					
					break;
				
				case 'skipOneTopToBottom':
					for (j = 0; j < verticalSlices; j++) {
						for (i = l; i < horizontalSlices; i += 2) {
							orderedArray.push(getSliceByPosition(initialArray, i, j));
						}
						l==0 ? l = 1 : l = 0;
					}
					l = 1;
					for (j = 0; j < verticalSlices; j++) {
						for (i = l; i < horizontalSlices; i += 2) {
							orderedArray.push(getSliceByPosition(initialArray, i, j));
						}
						l == 0 ? l = 1 : l = 0;
					}					
					break;
				
				case 'skipOneBottomToTop':
					for (j = verticalSlices-1; j >= 0; j--) {
						for (i = l; i < horizontalSlices; i += 2) {
							orderedArray.push(getSliceByPosition(initialArray, i, j));
						}
						l == 0 ? l = 1 : l = 0;
					}
					l = 1;
					for (j = verticalSlices - 1; j >= 0; j--) {
						for (i = l; i < horizontalSlices; i += 2) {
							orderedArray.push(getSliceByPosition(initialArray, i, j));
						}
						l == 0 ? l = 1 : l = 0;
					}					
					break;	
				
				case 'skipOneLeftToRight':
					for (i = 0; i < horizontalSlices; i++) {
						for (j = l; j < verticalSlices; j += 2) {
							orderedArray.push(getSliceByPosition(initialArray, i, j));
						}
						l == 0 ? l = 1 : l = 0;
					}
					l = 1;
					for (i = 0; i < horizontalSlices; i++) {
						for (j = l; j < verticalSlices; j += 2) {
							orderedArray.push(getSliceByPosition(initialArray, i, j));
						}
						l == 0 ? l = 1 : l = 0;
					}					
					break;	
				
				case 'skipOneRightToLeft':
					for (i = horizontalSlices - 1; i >= 0; i--) {
						for (j = l; j < verticalSlices; j += 2) {
							orderedArray.push(getSliceByPosition(initialArray, i, j));
						}
						l == 0 ? l = 1 : l = 0;
					}
					l = 1;
					for (i = horizontalSlices - 1; i >= 0; i--) {
						for (j = l; j < verticalSlices; j += 2) {
							orderedArray.push(getSliceByPosition(initialArray, i, j));
						}
						l == 0 ? l = 1 : l = 0;
					}					
					break;	
				
				case 'skipOneVertical':
					if (verticalSlices % 2) {
						for (j = 0; j < verticalSlices; j++) {
							for (i = l; i < horizontalSlices; i += 2) {
								if (j == Math.floor(verticalSlices / 2)) {
									j++;
									for (k = 1 - (horizontalSlices % 2); k < horizontalSlices; k += 2) {
										orderedArray.push(getSliceByPosition(initialArray, k, Math.floor(verticalSlices / 2)));
										if (getSliceByPosition(initialArray, k - 1, Math.floor(verticalSlices / 2)) != undefined) {
											orderedArray.push(getSliceByPosition(initialArray, k - 1, Math.floor(verticalSlices / 2)));
										}
									}
								}
								orderedArray.push(getSliceByPosition(initialArray, i, j));
								orderedArray.push(getSliceByPosition(initialArray, i, verticalSlices - j - 1));
							}
							l == 0 ? l = 1 : l = 0;
						}
					} 
					else {
						for (j = 0; j < verticalSlices; j++) {
							for (i = l; i < horizontalSlices; i += 2) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
								orderedArray.push(getSliceByPosition(initialArray, i, verticalSlices - j - 1));
							}
							l == 0 ? l = 1 : l = 0;
						}
					}					
					break;	
				
				case 'skipOneHorizontal':
					if (horizontalSlices % 2) {
						for (i = 0; i < horizontalSlices; i++) {
							for (j = l; j < verticalSlices; j += 2) {
								if (i == Math.floor(horizontalSlices / 2)) {
									i++;
									for (k = 1 - (verticalSlices % 2); k < verticalSlices; k += 2) {
										orderedArray.push(getSliceByPosition(initialArray, Math.floor(horizontalSlices / 2), k));
										if (getSliceByPosition(initialArray, Math.floor(horizontalSlices / 2), k-1) != undefined) {
											orderedArray.push(getSliceByPosition(initialArray, Math.floor(horizontalSlices / 2), k-1));
										}
									}
								}
								
								orderedArray.push(getSliceByPosition(initialArray, i, j));
								orderedArray.push(getSliceByPosition(initialArray, horizontalSlices - 1 - i, j));
							}
							l == 0 ? l = 1 : l = 0;
						}
					}
					else {
						for (i = 0; i < horizontalSlices; i++) {
							for (j = l; j < verticalSlices; j += 2) {
								orderedArray.push(getSliceByPosition(initialArray, i, j));
								orderedArray.push(getSliceByPosition(initialArray, horizontalSlices - 1 - i, j));
							}
							l == 0 ? l = 1 : l = 0;

						}
					}					
					break;	
				
				case 'spiralMarginToCenterCW':
					var h  = horizontalSlices,
						v = verticalSlices,					
						r, a = 0,				
						m = verticalSlices < horizontalSlices ? verticalSlices : horizontalSlices,
						n = Math.floor(m / 2);
				
					for (r = 0; r < n; r++) {
						for (i = a++; i < h; i++) {
							orderedArray.push(getSliceByPosition(initialArray, i, a - 1));
						}
						h--;
						for (j = a; j < v; j++) {
							orderedArray.push(getSliceByPosition(initialArray, horizontalSlices - a, j));
						}
						v--;
						for (k = h; k >= horizontalSlices - h; k--) {
							orderedArray.push(getSliceByPosition(initialArray, k - 1, v));
						}
						for (l = v - 1; l>=  verticalSlices - v; l--) {
							orderedArray.push(getSliceByPosition(initialArray, a - 1, l));
						}
					}
					if (m % 2) {
						if (m == verticalSlices) {
							for (i = a++; i < h; i++) {
								orderedArray.push(getSliceByPosition(initialArray, i, a - 1));

							}
						}
						if (m == horizontalSlices) {
							for (j = a++; j < v; j++) {
								orderedArray.push(getSliceByPosition(initialArray, horizontalSlices - a, j));
							}
						}
					}					
					break;	
				
				case 'spiralMarginToCenterCCW':
					var h  = horizontalSlices,
						v = verticalSlices,				
						r, a = 0,
						m = verticalSlices < horizontalSlices ? verticalSlices : horizontalSlices,
						n = Math.floor(m / 2);
				
					for (r = 0; r < n; r++) {
						for (j = a++; j < v; j++) {
							orderedArray.push(getSliceByPosition(initialArray, a - 1, j));
						}
						v--;
						for (i = a; i < h; i++) {
							orderedArray.push(getSliceByPosition(initialArray, i, verticalSlices - a));
						}
						h--;
						for (k = v; k >= verticalSlices - v; k--) {
							orderedArray.push(getSliceByPosition(initialArray, h, k - 1));
						}
						for (l = h - 1; l >= horizontalSlices - h; l--) {
							orderedArray.push(getSliceByPosition(initialArray, l, a - 1));
						}
					}
					if (m % 2) {
						if (m == verticalSlices) {
							for (i = a++; i < h; i++) {
								orderedArray.push(getSliceByPosition(initialArray, i, verticalSlices - a));
							}
						}
						if (m == horizontalSlices) {
							for (j = a++; j < v; j++) {
								orderedArray.push(getSliceByPosition(initialArray, a - 1, j));
							}
						}
					}					
					break;
				
				case 'spiralCenterToMarginCCW':
					var h  = horizontalSlices,
						v = verticalSlices,
						r, a = 0,
						m = verticalSlices < horizontalSlices ? verticalSlices : horizontalSlices,
						n = Math.floor(m / 2);
				
					for (r = 0; r < n; r++) {
						for (i = a++; i < h; i++) {
							orderedArray.push(getSliceByPosition(initialArray, i, a - 1));
						}
						h--;
						for (j = a; j < v; j++) {
							orderedArray.push(getSliceByPosition(initialArray, horizontalSlices - a, j));
						}
						v--;
						for (k = h; k >= horizontalSlices - h; k--) {
							orderedArray.push(getSliceByPosition(initialArray, k - 1, v));
						}
						for (l = v - 1; l>=  verticalSlices - v; l--) {
							orderedArray.push(getSliceByPosition(initialArray, a - 1, l));
						}
					}
					if (m % 2) {
						if (m == verticalSlices) {
							for (i = a++; i < h; i++) {
								orderedArray.push(getSliceByPosition(initialArray, i, a - 1));
							}
						}
						if (m == horizontalSlices) {
							for (j = a++; j < v; j++) {
								orderedArray.push(getSliceByPosition(initialArray, horizontalSlices - a, j));
							}
						}
					}
					
					orderedArray.reverse();					
					break;
				
				case 'spiralCenterToMarginCW':
					var h  = horizontalSlices,
						v = verticalSlices,
						r, a = 0,
						m = verticalSlices < horizontalSlices ? verticalSlices : horizontalSlices,
						n = Math.floor(m / 2);
				
					for (r = 0; r < n; r++) {
						for (j = a++; j < v; j++) {
							orderedArray.push(getSliceByPosition(initialArray, a - 1, j));
						}
						v--;
						for (i = a; i < h; i++) {
							orderedArray.push(getSliceByPosition(initialArray, i, verticalSlices - a));
						}
						h--;
						for (k = v; k >= verticalSlices - v; k--) {
							orderedArray.push(getSliceByPosition(initialArray, h, k - 1));
						}
						for (l = h - 1; l >= horizontalSlices - h; l--) {
							orderedArray.push(getSliceByPosition(initialArray, l, a - 1));
						}
					}
					if (m % 2) {
						if (m == verticalSlices) {
							for (i = a++; i < h; i++) {
								orderedArray.push(getSliceByPosition(initialArray, i, verticalSlices - a));
							}
						}
						if (m == horizontalSlices) {
							for (j = a++; j < v; j++) {
								orderedArray.push(getSliceByPosition(initialArray, a - 1, j));
							}
						}
					}
					
					orderedArray.reverse();					
					break;
				
				default:
					var randomArray = new Array();
										
					while(initialArray.length) {
						l = Math.floor(Math.random() * initialArray.length);
						randomArray.push(initialArray[l]);
						initialArray.splice(l, 1);
					}
					
					var n = randomArray.length;
					
					for(k = 0; k < n; k++) {
						orderedArray[k] = randomArray[k];
					}				
			}
			
			return orderedArray;
		}
		
		/**
		* Returns an element from the array, at the specified horizontal and vertical position
		*/
		function getSliceByPosition(array, hPos, vPos) {
			return $.grep(array, function(el){return (el.data('hPos') == hPos && el.data('vPos') == vPos)})[0];
		}
		
		
		/**
		* Returns a random element from an array
		*/
		function getRandom(array) {
			return array[Math.floor((Math.random() * array.length))];
		}
		
		
		/**
		* Returns a number if the specified value is a numeric string
		*/
		function getNumeric(value) {
			return isNaN(value) ? value : parseInt(value);
		}
		
		
		/**
		* Converts an hex string to RGB
		*/
		function hexToRGB(value) {
			var red = parseInt(value.substring(1, 3), 16),
				green = parseInt(value.substring(3, 5), 16),
				blue = parseInt(value.substring(5, 7), 16);
				
			return {red: red, green: green, blue: blue};
		}
		
		
		// PUBLIC METHODS
		
		this.nextSlide = nextSlide;
		
		
		this.previousSlide = previousSlide;
		
		
		this.gotoSlide = gotoSlide;
		
		
		this.startSlideshow = startSlideshow;
		
		
		this.stopSlideshow = stopSlideshow;
		
		
		this.pauseSlideshow = pauseSlideshow;
		
		
		this.resumeSlideshow = resumeSlideshow;
		
		
		this.scrollToThumbnailPage = scrollToThumbnailPage;
		
		
		this.scrollToNextThumbnailPage = scrollToNextThumbnailPage;
		
		
		this.scrollToPreviousThumbnailPage = scrollToPreviousThumbnailPage;
		
		
		this.startThumbnailMouseScroll = startThumbnailMouseScroll;
		
		
		this.stopThumbnailMouseScroll = stopThumbnailMouseScroll;
		
		
		this.startThumbnailMouseWheel = startThumbnailMouseWheel;
		
		
		this.stopThumbnailMouseWheel = stopThumbnailMouseWheel;
		
		
		this.doSliderLayout = doSliderLayout;
		
		
		this.getSlideshowState = function() {
			return slideshowState;
		}
		
		
		this.getCurrentIndex = function() {
			return currentIndex;	
		}
		
		
		this.getSlideAt = function(index) {
			return slidesData[index];	
		}
		
		
		this.getTriggerType = function() {
			return triggerType;	
		}
		
		
		this.isTransition = function() {
			return isTransition;	
		}
		
		
		this.totalSlides = function() {
			return numSlides;	
		}
		
		
		this.getSize = function() {
			return {sliderWidth: slider.width(), sliderHeight: slider.height(), slideWidth: slideWrapper.width(), slideHeight: slideWrapper.height()};
		}
		
		
		this.destroy = function() {
			stopSlideshow();
			clearThumbnailScrollingTimers();
			$(document).unbind('mousemove');
			$(document).unbind('mousemove.tooltip');
		}		
	}
	
	
	$.fn.advancedSlider = function(options) {
		var collection = [];
		
		for (var i = 0; i < this.length; i++) {
			if (!this[i].advancedslider)
				this[i].advancedslider = new AdvancedSlider(this[i], options);
			
			collection.push(this[i].advancedslider);
		}
		
		// if there are more slider instances, return the array of sliders
		// it there is only one, return just the slide instance
		return collection.length > 1 ? collection : collection[0];
	}
	
	
	// default settings
	$.fn.advancedSlider.defaults =  {
		xmlSource: null,
		width: 500,
		height: 300,
		skin: 'pixel',
		scrollbarSkin: 'scrollbar-1',
		shadow: true,
		alignType: 'centerCenter', // all 9 possible variations
		scaleType: 'outsideFit', // noScale, exactFit, insideFit, outsideFit, proportionalFit
		allowScaleUp: false,
		preloadNearbyImages: false,
		slideStart: 0,
		shuffle: false,
		
		slideshow: true,
		slideshowDelay: 5000,
		slideshowDirection: 'next', // next, previous
		slideshowLoop: true,
		slideshowControls: false,
		slideshowControlsToggle: true,
		slideshowControlsShowDuration: 500,
		slideshowControlsHideDuration: 500,
		pauseSlideshowOnHover: false,
		
		slideArrows: true,
		slideArrowsToggle: true,
		slideArrowsShowDuration: 500,
		slideArrowsHideDuration: 500,
		slideButtons: true,
		slideButtonsNumber: false,
		slideButtonsToggle: false,
		slideButtonsShowDuration: 500,
		slideButtonsHideDuration: 500,
		slideButtonsCenter: true,
		slideButtonsContainerCenter: true,
		keyboardNavigation: false,
		keyboardNavigationOnFocusOnly: false,	
		
		timerAnimation: true,
		timerAnimationControls: true,
		timerFadeDuration: 500,
		timerToggle: false,
		timerRadius: 18,
		timerStrokeColor1: '#000000',
		timerStrokeColor2: '#FFFFFF',
		timerStrokeOpacity1: 0.5,
		timerStrokeOpacity2: 0.7,
		timerStrokeWidth1: 8,
		timerStrokeWidth2: 4,
		
		lightbox: false,
		lightboxTheme: 'pp_default', // pp_default, light_rounded, dark_rounded, light_square, dark_square, facebook
		lightboxOpacity: 0.8,
		lightboxIcon: true,
		lightboxIconToggle: false,
		lightboxIconFadeDuration: 500,
		thumbnailLightboxIcon: true,
		thumbnailLightboxIconToggle: true,
		
		htmlDuringTransition: true,
		overrideTransition: false,
		initialEffect: true,
		effectType: 'random', // slice, slide, fade, swipe, random
		
		sliceEffectType: 'random', // fade, scale, width, height, slide, random
		sliceDelay: 50,
		sliceDuration: 1000,
		sliceEasing: 'swing', // see jquery.easing.js file
		horizontalSlices: 5,
		verticalSlices: 3,
		slicePattern: 'random',
		slicePoint: 'centerCenter', // all 9 possible variations
		sliceStartPosition: 'left', // left, right, top, bottom, leftTop, rightTop, leftBottom, horizontalAlternate, verticalAlternate, random
		sliceStartRatio: 1,
		slideMask: false,
		sliceFade: true,
		fadePreviousSlide: false,
		fadePreviousSlideDuration: 300,
		
		slideDirection: 'autoHorizontal', // autoHorizontal, autoVertical, rightToLeft, leftToRight, topToBottom, bottomToTop
		slideLoop: false,
		slideDuration: 700,
		slideEasing: 'easeInOutExpo', // see jquery.easing.js file
		
		fadeInDuration: 700,
		fadeOutDuration: 700,
		fadeInEasing: 'swing', // see jquery.easing.js file
		fadeOutEasing: 'swing', // see jquery.easing.js file
		
		swipeOrientation: 'horizontal', // horizontal, vertical
		swipeDuration: 700,
		swipeBackDuration: 300,
		swipeSlideDistance: 10,
		swipeEasing: 'swing', // see jquery.easing.js file
		swipeMouseDrag: true,
		swipeTouchDrag: true,
		swipeGrabCursor: true,
		swipeThreshold: 50,		
		
		thumbnailType: 'tooltip', // tooltip, scroller, tooltipAndScroller, none
		thumbnailWidth: 80,
		thumbnailHeight: 50,
		thumbnailSlideAmount: 10,
		thumbnailSlideDuration: 300,
		thumbnailSlideEasing: 'swing', // see jquery.easing.js file
		
		thumbnailScrollerToggle: false,
		thumbnailScrollerResponsive: false,
		thumbnailScrollerCenter: true,
		thumbnailScrollDuration: 1000,
		thumbnailScrollEasing: 'swing', // see jquery.easing.js file
		maximumVisibleThumbnails: 5,
		minimumVisibleThumbnails: 1,
		thumbnailOrientation: 'horizontal', // horizontal, vertical
		thumbnailLayers: 1,
		thumbnailTooltip: false,
		tooltipShowDuration: 300,
		tooltipHideDuration: 300,
		
		thumbnailCaptionPosition: 'bottom', // top, bottom, left, right, custom
		thumbnailCaptionToggle: true,
		thumbnailCaptionEffect: 'slide', // slide, fade
		thumbnailCaptionShowDuration: 500,
		thumbnailCaptionHideDuration: 500,
		thumbnailCaptionEasing: 'swing', // see jquery.easing.js file
		thumbnailScrollbar: false,
		thumbnailButtons: false,
		thumbnailArrows: true,
		thumbnailButtonsToggle: false,
		thumbnailArrowsToggle: false,
		thumbnailScrollbarToggle: false,
		scrollbarArrowScrollAmount: 100,
		thumbnailScrollerHideDuration: 500,
		thumbnailScrollerShowDuration: 500,
		thumbnailArrowsHideDuration: 500,
		thumbnailArrowsShowDuration: 500,
		thumbnailButtonsHideDuration: 500,
		thumbnailButtonsShowDuration: 500,
		thumbnailScrollbarHideDuration: 500,
		thumbnailScrollbarShowDuration: 500,
		thumbnailSync: true,
		thumbnailMouseScroll: false,
		thumbnailMouseScrollEase: 90,
		thumbnailMouseScrollSpeed: 10,
		thumbnailMouseWheel: false,
		thumbnailMouseWheelSpeed: 20,
		thumbnailMouseWheelReverse: false,
		thumbnailScrollbarEase: 10,
		
		captionToggle: false,
		captionDelay: 0,
		captionSize: 70,
		captionBackgroundOpacity: 0.5,
		captionBackgroundColor: '#000000',
		captionShowEffect: 'slide', // slide, fade
		captionShowEffectDuration: 500,
		captionShowEffectEasing: 'swing', // see jquery.easing.js file
		captionShowSlideDirection: 'auto', // topToBottom, bottomToTop, leftToRight, rightToLeft, auto
		captionHideEffect: 'fade', // slide, fade
		captionHideEffectDuration: 300,
		captionHideEffectEasing: 'swing', // see jquery.easing.js file
		captionHideSlideDirection: 'auto', // topToBottom, bottomToTop, leftToRight, rightToLeft, auto
		captionPosition: 'bottom', // top, bottom, left, right, custom
		captionLeft: 50,
		captionTop: 50,
		captionWidth: 300,
		captionHeight: 100,
		
		videoPlayAction: 'stopSlideshow', // stopSlideshow, pauseSlideshow, none
		videoPauseAction: 'none', // startSlideshow, resumeSlideshow, none
		videoEndAction: 'startSlideshow', // resetVideo, startSlideshow, resumeSlideshow, nextSlide, none
		reachVideoAction: 'none', // startVideo, none
		leaveVideoAction: 'pauseVideo', // pauseVideo, stopVideo, pauseVideoAndBuffering
		
		slideProperties: null,
		
		slideOpen: null,
		slideClick: null,
		slideMouseOver: null,
		slideMouseOut: null,
		thumbnailClick: null,
		thumbnailMouseOver: null,
		thumbnailMouseOut: null,
		transitionStart: null,
		transitionComplete: null,
		xmlLoaded: null,
		doSliderLayout: null
	};
	
})(jQuery)