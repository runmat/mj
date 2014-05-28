var Lock = function () {

    return {
        //main function to initiate the module
        init: function () {

             $.backstretch([
		        "assets_v2/img/bg/1.jpg",
		        "assets_v2/img/bg/2.jpg",
		        "assets_v2/img/bg/3.jpg",
		        "assets_v2/img/bg/4.jpg"
		        ], {
		          fade: 1000,
		          duration: 8000
		      });
        }

    };

}();