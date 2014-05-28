/*
* jQuery selectfilter
* By: Trent Richardson [http://trentrichardson.com]
* Version 0.1
* Last Modified: 06/09/2011
* 
* Copyright 2011 Trent Richardson
* Dual licensed under the MIT and GPL licenses.
* http://trentrichardson.com/Impromptu/GPL-LICENSE.txt
* http://trentrichardson.com/Impromptu/MIT-LICENSE.txt
*/
(function($){
	
	$.fn.extend({
		selectfilter: function(options){
				options = options || {};

				if (typeof(options) == 'string'){
					if(options == 'enable'){
						return $(this).focus().keydown();
					}
				}
				else{
					options = $.extend({},{ 
							timeout: 500, // how long between searches (fast typers doesn't search each key?)
							elapse: 6000, // how long to wait before clearing query
							min_char: 3,  // min chars to search with
							disable_options: true, // disable non-matches, will be re-enabled after elapse
							filter: function(q,re,r,o){ // custom filter function(query, query_regex, previous_results, all_options){} return true/false on match
									var $opt=$(this);
									return (re.test($opt.attr('value')) || re.test($opt.text())); 
								} 
						},options);
			
					return this.each(function() {
						var $t = $(this),
							$o = $('option',$t), // the full set
							$results = $o.filter('option:enabled'), // the search results
							$notresults = $o.not($results),
							query = '',
							prev_query = '',
							$query_display = $('<span class="selectfilter-query">Type..</span>').css({ padding: 5, border:'solid #aaa 1px', background: '#333', color: '#fff', display:'none', height: 20, fontSize: 12 }).appendTo($('body')),
							elapse_timeout = null,
							search_timeout = null,
							elapsed = function(){
								query = '';
								elapse_timeout = null;
								clearInterval(search_timeout);
								search_timeout = null;
								if(options.disable_options)
									$notresults.removeAttr('disabled');
								$results = $o;
								$query_display.fadeOut('fast', function(){  });
							},
							do_search = function(){
								var ql = query.length,
									pql = prev_query.length,
									re = new RegExp(query,"i");
							
								if(query != prev_query && ql >= options.min_char){

									// letters removed, re-enable items
									if(ql <= pql){
										if(options.disable_options)
											$notresults.removeAttr('disabled');
										$results = $o;
									}

									// ok, filter results
									$results.each(function(i){
										var $i = $(this);
										if(!options.filter.apply(this, [query, re, $results, $o]))
											$results = $results.not($i);
									});
									
									if(options.disable_options)
										$notresults = $o.not($results).attr('disabled',true);

									// if we have a multi-select
									if($t.attr('multiple') == true)
										$results.attr('selected',true);
									else $results.first().attr('selected',true);
								}
							};
					
						$t.keydown(function(event){
							
							var keys = {
											'8':{l:'backspace',u:'backspace'},
											'9':{l:'tab',u:'tab'},
											'27':{l:'escape',u:'escape'},
											'46':{l:'delete',u:'delete'}, 
											'48':{l:'0',u:')'},
											'49':{l:'1',u:'!'},
											'50':{l:'2',u:'@'},
											'51':{l:'3',u:'#'},
											'52':{l:'4',u:'$'},
											'53':{l:'5',u:'%'},
											'54':{l:'6',u:'^'},
											'55':{l:'7',u:'&'},
											'56':{l:'8',u:'*'},
											'57':{l:'9',u:'('},
											'187':{l:'=',u:'+'},
											'189':{l:'-',u:'_'},
											'192':{l:'`',u:'~'},
											'186':{l:';',u:':'},
											'222':{l:"'",u:'"'},
											'188':{l:',',u:'<'},
											'190':{l:'.',u:'>'},
											'191':{l:'/',u:'?'},
											'219':{l:'[',u:'{'},
											'220':{l:'\\',u:'|'},
											'221':{l:']',u:'}'}
										};
										
							// figure out which key		
							var chr = '',
								code = event.which || event.keyCode;
							
							if(keys[code] !== undefined){
								chr = event.shiftKey? keys[code].u : keys[code].l;
							}
							else{
								chr = event.shiftKey? String.fromCharCode(event.keyCode).toUpperCase() : String.fromCharCode(event.keyCode).toLowerCase();
							}
							
							if(code !== undefined && chr !== ''){
							
								// allow the user to tab away
								if(chr == 'tab'){
									elapsed();
									return true;
								}
								
								// ok they didn't tab, so hang on..
								event.preventDefault();

								// handle the key stroke
								prev_query = query;
								if(chr == 'escape')
									elapsed();
								else if(chr == 'backspace')
									query = query.substring(0, query.length-1);
								else if(keys[code] !== undefined || /\w/.test(chr) || chr == ' ') 
									query += chr;
								

								// start the timed search
								if(!search_timeout){
									search_timeout = setInterval(do_search, options.timeout);
									var pos = $t.offset();
									
									$query_display.css({ position: 'absolute', left: pos.left, top: pos.top-40 }).fadeTo('slow',0.8);
									
								}
								$query_display.text(query);
							
								// restart elapse timeout
								if(elapse_timeout) 
									clearTimeout(elapse_timeout);
								elapse_timeout = setTimeout(elapsed, options.elapse);
							}
						});
						
					});
				}
			}
		});

})(jQuery);
