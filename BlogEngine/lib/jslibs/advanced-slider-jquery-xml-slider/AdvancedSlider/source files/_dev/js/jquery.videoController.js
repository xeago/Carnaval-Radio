/*
	Name: Video Controller
*/
(function($) {
	
	function VideoController(instance, options) {		
		
		// contains all the settings for the video
		this.settings = $.extend({}, $.fn.videoController.defaults, options);
		
		
		var video = $(instance),
			
			// reference to the current object
			self = this,
			
			videoType,
			
			videoPlayer,
			
			youtubeAPILoaded = window.YT && window.YT.Player,
			
			sublimeVideoAPILoaded = window.sublimevideo && window.sublimevideo.prepare,
			
			videoReady = false,
			
			reachVideoExecuted = false,
			
			videoStarted = false,
			
			videoPlaying = false;
			
		
		init();
		
		
		function init() {			
			if (video.is('iframe') && video.attr('src').indexOf('youtube') != -1)
				videoType = 'youtube-video';
			else if (video.is('iframe') && video.attr('src').indexOf('vimeo') != -1)
				videoType = 'vimeo-video';
			else if (video.hasClass('video video-js'))
				videoType = 'html5-video-js';
			else if (video.hasClass('video sublime-video') && window.sublimevideo)
				videoType = 'html5-sublime-video';
			else if (video.is('video') && video.hasClass('video') && !($.browser.msie && parseInt($.browser.version) < 9))
				videoType = 'html5-video';
				
			switch (videoType) {
				case 'youtube-video':
					initYoutubeVideo();
					break;
				
				case 'vimeo-video':
					initVimeoPlayer();
					break;
				
				case 'html5-video-js':
					initVideoJS();
					break;
				
				case 'html5-sublime-video':
					initSublimeVideo();
					break;
					
				case 'html5-video':
					initHTML5Video();
					break;
			}
		}
		
		
		function initYoutubeVideo() {
			if (youtubeAPILoaded) {
				addYoutubePlayerEvent();
			} else {
				var tag = document.createElement('script');
				tag.src = "http://www.youtube.com/player_api";
				var firstScriptTag = document.getElementsByTagName('script')[0];
				firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
				
				window.onYouTubePlayerAPIReady = function() {
					addYoutubePlayerEvent();	
				}
			}			
		}
		
		
		function addYoutubePlayerEvent() {
			videoPlayer = new YT.Player(video[0], {
				events: {
					'onReady': function() {
						videoReady = true;
			
						if (self.settings.reachVideoAction == 'playVideo' && !reachVideoExecuted) {
							reachVideoExecuted = true;
							playVideo();
						}
					},
					
					'onStateChange': function(event) {
						switch (event.data) {
							case YT.PlayerState.BUFFERING:
								if (!videoStarted) {
									videoStarted = true;
									dispatch('start');
								}
								
								break;
								
							case YT.PlayerState.PLAYING:
								if (!videoStarted) {
									videoStarted = true;
									dispatch('start');
								}
								
								if (!videoPlaying) {
									videoPlaying = true;
									dispatch('play');
								}
								
								break;
							
							case YT.PlayerState.PAUSED:
								if (videoPlaying) {
									videoPlaying = false;
									dispatch('pause');
								}
								
								break;
							
							case YT.PlayerState.ENDED:
								videoPlaying = false;
								dispatch('end');
								break;
						}
					}
				}
			});
		}
		
		
		function initVimeoPlayer() {
			videoPlayer = Froogaloop(video[0]);
			
			videoPlayer.addEvent('ready', function(data) {
				videoReady = true;
			
				if (self.settings.reachVideoAction == 'playVideo' && !reachVideoExecuted) {
					reachVideoExecuted = true;
					playVideo();
				}
				
				videoPlayer.addEvent('loadProgress', function(data) {
					if (!videoStarted) {
						videoStarted = true;
						dispatch('start');
					}
				});			
				
				videoPlayer.addEvent('play', function(data) {
					if (!videoStarted) {
						videoStarted = true;
						dispatch('start');
					}
								
					if (!videoPlaying) {
						videoPlaying = true;
						dispatch('play');
					}
				});			
				
				videoPlayer.addEvent('pause', function(data) {
					if (videoPlaying) {
						videoPlaying = false;
						dispatch('pause');
					}
				});			
				
				videoPlayer.addEvent('finish', function(data) {
					videoPlaying = false;
					dispatch('end');
				});
			});
		}
		
		
		
		function initVideoJS() {
			VideoJS(video.attr('id')).ready(function() {
				videoPlayer = this;
				videoReady = true;
				
				if (self.settings.reachVideoAction == 'playVideo' && !reachVideoExecuted) {
					reachVideoExecuted = true;
					playVideo();
				}
				
				videoPlayer.addEvent('play', function() {
					if (!videoStarted) {
						videoStarted = true;
						dispatch('start');
					}
					
					if (!videoPlaying) {
						videoPlaying = true;
						dispatch('play');
					}
				});						
				
				videoPlayer.addEvent('pause', function() {
					if (videoPlaying) {
						videoPlaying = false;
						dispatch('pause');
					}
				});			
				
				videoPlayer.addEvent('ended', function() {
					videoPlaying = false;
					dispatch('end');
				});
				
				videoPlayer.addEvent('enterFullWindow', function() {
					dispatch('fullscreenchange');
				});
				
				videoPlayer.addEvent('exitFullWindow', function() {
					dispatch('fullscreenchange');
				});
			});
		}
				
		
		
		function initSublimeVideo() {			
			if (sublimeVideoAPILoaded) {
				handleSublimeVideo();
			} else {
				var sublimeVideoInterval = setInterval(function() {
					if (sublimevideo.prepare) {
						clearInterval(sublimeVideoInterval);
						handleSublimeVideo();	
					}
				}, 100);
			}			
		}
		
		
		function handleSublimeVideo() {			
			if (self.settings.reachVideoAction == 'playVideo' && !reachVideoExecuted) {
				reachVideoExecuted = true;
				playVideo();
			}
			
			
			sublimevideo.onStart(function() {
				dispatch('start');
				dispatch('play');
			});	
				
			
			sublimevideo.onEnd(function() {
				dispatch('end');
			});
		}
		
		
		
		function initHTML5Video() {
			videoPlayer = video[0];			
			videoReady = true;
			
			if (self.settings.reachVideoAction == 'playVideo' && !reachVideoExecuted) {
				reachVideoExecuted = true;
				playVideo();
			}
			
			videoPlayer.addEventListener('play', function() {
				if (!videoStarted) {
					videoStarted = true;
					dispatch('start');
				}
				
				if (!videoPlaying) {
					videoPlaying = true;
					dispatch('play');
				}
			});			
			
			videoPlayer.addEventListener('pause', function() {
				if (videoPlaying) {
					videoPlaying = false;
					dispatch('pause');
				}
			});			
			
			videoPlayer.addEventListener('ended', function() {
				videoPlaying = false;
				dispatch('end');
			});
		}
		
		
		
		function reachVideo() {
			if (self.settings.reachVideoAction == 'playVideo' && videoReady && !reachVideoExecuted) {
				reachVideoExecuted = true;
				playVideo();
			}
		}
		
		
		function leaveVideo() {
			reachVideoExecuted = false;
					
			if ((videoStarted && videoPlayer ) || videoType == 'html5-sublime-video') {
				if (self.settings.leaveVideoAction == 'stopVideo')
					stopVideo();
				else if (self.settings.leaveVideoAction == 'pauseVideoAndBuffering')
					pauseVideoAndBuffering();
				else if (self.settings.leaveVideoAction == 'pauseVideo')
					pauseVideo();
			}
		}		
		
		
		function playVideo() {
			if (videoType == 'youtube-video')
				videoPlayer.playVideo();
			else if(videoType == 'vimeo-video')
				videoPlayer.api('play');
			else if(videoType == 'html5-video-js')
				videoPlayer.play();
			else if(videoType == 'html5-sublime-video')
				sublimevideo.play();
			else if(videoType == 'html5-video')
				videoPlayer.play();
		}
		
		
		function stopVideo() {
			if (videoType == 'youtube-video') {
				videoPlayer.seekTo(1);					
				videoPlayer.stopVideo();
			} else if(videoType == 'vimeo-video') {
				videoPlayer.api('pause');
				videoPlayer.api('unload');
			} else if(videoType == 'html5-video-js') {
				videoPlayer.currentTime(0);
				videoPlayer.pause();
			} else if(videoType == 'html5-sublime-video') {
				sublimevideo.stop();
			} else if(videoType == 'html5-video') {
				videoPlayer.currentTime = 0;
				videoPlayer.pause();
			}
		}
		
		
		function pauseVideo() {
			if (videoType == 'youtube-video') {
				videoPlayer.pauseVideo();
			} else if(videoType == 'vimeo-video') {
				videoPlayer.api('pause');
			} else if(videoType == 'html5-video-js') {
				videoPlayer.pause();
			} else if(videoType == 'html5-sublime-video') {
				sublimevideo.stop();
			} else if(videoType == 'html5-video') {
				videoPlayer.pause();
			}
		}
		
		
		function pauseVideoAndBuffering() {
			if (videoType == 'youtube-video') {
				videoPlayer.stopVideo();
			} else if(videoType == 'vimeo-video') {
				videoPlayer.api('pause');
			} else if(videoType == 'html5-video-js') {
				videoPlayer.pause();
			} else if(videoType == 'html5-sublime-video') {
				sublimevideo.stop();
			} else if(videoType == 'html5-video') {
				videoPlayer.pause();
			}
		}
		
		
		function dispatch(message) {
			var eventObject = {type: message};
			
			switch (message) {
				case 'start':
					$.isFunction(self.settings.start) && self.settings.start.call(this, eventObject);
					break;
				
				case 'play':
					$.isFunction(self.settings.play) && self.settings.play.call(this, eventObject);
					break;
					
				case 'pause':
					$.isFunction(self.settings.pause) && self.settings.pause.call(this, eventObject);
					break;
				
				case 'end':
					$.isFunction(self.settings.end) && self.settings.end.call(this, eventObject);
					break;
					
				case 'fullscreenchange':
					$.isFunction(self.settings.fullscreenchange) && self.settings.fullscreenchange.call(this, eventObject);
					break;
			}
		}
		
		
		this.reachVideo = reachVideo;
		
		this.leaveVideo = leaveVideo;
		
		this.stopVideo = stopVideo;
		
		this.pauseVideo = pauseVideo;
		
		this.pauseVideoAndBuffering = pauseVideoAndBuffering;
		
	}
	
	
	$.fn.videoController = function(options) {
		var collection = [];
		
		for (var i = 0; i < this.length; i++) {
			if (!this[i].videoControllerInstance)
				this[i].videoControllerInstance = new VideoController(this[i], options);
			
			collection.push(this[i].videoControllerInstance);
		}
		
		// if there are more video instances, return the array of videos
		// it there is only one, return just the video instance
		return collection.length > 1 ? collection : collection[0];
	}
	
	
	// default settings
	$.fn.videoController.defaults =  {
		start: null,
		play: null,
		pause: null,
		end: null,
		fullscreenchange: null,
		reachVideoAction: 'none',
		leaveVideoAction: 'pauseVideo'  
	};
	
})(jQuery)