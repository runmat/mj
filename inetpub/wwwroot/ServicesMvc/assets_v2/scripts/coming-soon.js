var ComingSoon = function () {

    return {
        //main function to InitParametersiate the module
        init: function () {

            $.backstretch([
    		        "assets_v2/img/bg/1.jpg",
    		        "assets_v2/img/bg/2.jpg",
    		        "assets_v2/img/bg/3.jpg",
    		        "assets_v2/img/bg/4.jpg"
    		        ], {
    		          fade: 1000,
    		          duration: 10000
    		    });

            var austDay = new Date();
            austDay = new Date(austDay.getFullYear() + 1, 1 - 1, 26);
            $('#defaultCountdown').countdown({until: austDay});
            $('#year').text(austDay.getFullYear());
        }

    };

}();