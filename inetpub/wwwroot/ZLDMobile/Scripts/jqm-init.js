// Anpassungen, die durchgeführt werden sollen, wenn JQuery Mobile anschließend eingebunden wird

$(document).bind('mobileinit', function () {
    $.mobile.textinput.prototype.options.clearSearchButtonText = "Zur\u00fccksetzen";
});
