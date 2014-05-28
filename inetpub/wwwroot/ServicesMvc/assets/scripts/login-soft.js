var Login = function () {

    return {
        //main function to InitParametersiate the module
        init: function () {
            if (typeof($.backstretch) === 'undefined')
                return;
            
            $.backstretch([
		        "/servicesmvc/assets/img/bg/1.jpg",
            //"/servicesmvc/assets/img/bg/2.jpg",
		        "/servicesmvc/assets/img/bg/3.jpg",
		        "/servicesmvc/assets/img/bg/4.jpg"
		        ], {
		            fade: 1000,
		            duration: 8000
		        });
        }
    };

} ();