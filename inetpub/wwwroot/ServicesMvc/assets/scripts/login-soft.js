var Login = function () {

    return {
        //main function to InitParametersiate the module
        init: function () {
            if (typeof($.backstretch) === 'undefined')
                return;
            
            $.backstretch([
		        "/servicesmvc/assets/img/bg/1.jpg",
		        "/servicesmvc/assets/img/bg/2.jpg"
		        ], {
		            fade: 1500,
		            duration: 5000
		        });
        }
    };

} ();