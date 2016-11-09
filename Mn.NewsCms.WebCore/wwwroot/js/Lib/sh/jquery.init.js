$(function(){
	//To get the random tabs label with variable length for testing the calculations			
	keywords = ["Just a tab label","Long string","Short","Very very long string","tab","New tab","This is a new tab"]
		
	$('#switcher').themeswitcher();//Theme switcher
	
	sh_highlightDocument();
	
	
	
	//example 2
	var $tabs_example_1 = $('#example_1').tabs().scrollabletab();
	//Add new tab
	$('#addTab_2').click(function(){
		label = keywords[Math.floor(Math.random()*keywords.length)]
		content = 'This is the content for the '+label+'<br>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque hendrerit vulputate porttitor. Fusce purus leo, faucibus a sagittis congue, molestie tempus felis. Donec convallis semper enim, varius sagittis eros imperdiet in. Vivamus semper sem at metus mattis a aliquam neque ornare. Proin sed semper lacus.';
		$tabs_example_2.trigger('addTab',[label,content]);
		return false;
	});
	
	//Add new tab
	$('#addTab_1').click(function(){
		var label = keywords[Math.floor(Math.random()*keywords.length)]
		content = 'This is the content for the '+label+'<br>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque hendrerit vulputate porttitor. Fusce purus leo, faucibus a sagittis congue, molestie tempus felis. Donec convallis semper enim, varius sagittis eros imperdiet in. Vivamus semper sem at metus mattis a aliquam neque ornare. Proin sed semper lacus.';
		rnd = Math.floor(Math.random()*10000);
		$('body').append('<div id="'+rnd+'">'+content+'</div>');
		$tabs_example_1.tabs('add','#'+rnd,label);
		return false;
	});
	
	//Add new tab using jQuery ui tabs method
	$('#addUiTab').click(function(){
		var label = keywords[Math.floor(Math.random()*keywords.length)]
		content = 'This is the content for the '+label+'<br>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque hendrerit vulputate porttitor. Fusce purus leo, faucibus a sagittis congue, molestie tempus felis. Donec convallis semper enim, varius sagittis eros imperdiet in. Vivamus semper sem at metus mattis a aliquam neque ornare. Proin sed semper lacus.';
		rnd = Math.floor(Math.random()*10000);
		$('body').append('<div id="'+rnd+'">'+content+'</div>');
		$tabs_example_1.tabs('add','#'+rnd,label);
		return false;
	});
	
	$('#removeTab').click(function(){
		$tabs_example_1.tabs('select',$tabs_example_1.tabs('length')-1);
		$tabs_example_1.tabs('remove',$tabs_example_1.tabs('length')-1);
		return false;
	});
	
	
	//example 3
	var $tabs_example_2 = $('#example_2')
			.tabs()
			.scrollabletab({
					'closable':true, //Default false
					'animationSpeed':50, //Default 100
					'loadLastTab':true, //Default false
					'resizable':true, //Default false
					'resizeHandles':'e,s,se', //Default 'e,s,se'
					'easing':'easeInOutExpo'
			});
	//Add new tab
	$('#addTab_2').click(function(){
		var label = keywords[Math.floor(Math.random()*keywords.length)]
		content = 'This is the content for the '+label+'<br>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque hendrerit vulputate porttitor. Fusce purus leo, faucibus a sagittis congue, molestie tempus felis. Donec convallis semper enim, varius sagittis eros imperdiet in. Vivamus semper sem at metus mattis a aliquam neque ornare. Proin sed semper lacus.';
		rnd = Math.floor(Math.random()*10000);
		$('body').append('<div id="'+rnd+'">'+content+'</div>');
		$tabs_example_3.tabs('add','#'+rnd,label);
		return false;
	});
	
	//Add new tab using jQuery ui tabs method
	$('#addUiTab_2').click(function(){
		var label = keywords[Math.floor(Math.random()*keywords.length)]
		content = 'This is the content for the '+label+'<br>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque hendrerit vulputate porttitor. Fusce purus leo, faucibus a sagittis congue, molestie tempus felis. Donec convallis semper enim, varius sagittis eros imperdiet in. Vivamus semper sem at metus mattis a aliquam neque ornare. Proin sed semper lacus.';
		rnd = Math.floor(Math.random()*10000);
		$('body').append('<div id="'+rnd+'">'+content+'</div>');
		$tabs_example_2.tabs('add','#'+rnd,label);
		return false;
	});
	
	$('#removeTab_2').click(function(){
		$tabs_example_2.tabs('select',$tabs_example_2.tabs('length')-1);
		$tabs_example_2.tabs('remove',$tabs_example_2.tabs('length')-1);
		return false;
	});
});

jQuery.extend(jQuery.easing, {
    def: 'easeOutQuad',
	easeInOutExpo: function (x, t, b, c, d) {
        if (t == 0) return b;
        if (t == d) return b + c;
        if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b;
        return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b;
    }
});