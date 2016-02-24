var Login = function () {

    return {
        //main function to InitParametersiate the module
        init: function () {
            if (typeof($.backstretch) === 'undefined')
                return;
            
            $.backstretch([
		        "/servicesmvc/assets/img/bg/11.jpg",
		        "/servicesmvc/assets/img/bg/12.jpg"
		        ], {
		            fade: 1500,
		            duration: 5000
		        });
        }
    };

} ();