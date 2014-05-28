function toggleQuery() {
    var TableQuery = document.getElementById('TableQuery');
    var img = document.getElementById('navDisplayArrow');
    var data = document.getElementById('data');

    if (TableQuery.style.display == 'none') {
        TableQuery.style.display = 'block';
        img.src = '../../../Images/navDisplayArrowLeft.gif';
    }
    else {
        TableQuery.style.display = 'none';
        img.src = '../../../Images/navDisplayArrowRight.gif';
    }
}

var divWidth = '';
var divHeight = '';
var txtFirstButton = 'OK';
var txtSecondButton = 'Cancel'

function DisplayCalender(objname) {
    // Set default dialogbox width if null

    if (document.getElementById(objname).style.display == 'block') {
        document.getElementById(objname).style.display = 'none'
        document.getElementById('Text2').value = ''

    }
    else {
        divWidth = 180

        // Set default dialogBox height if null

        divHeight = 90

        // Ge the dialogbox object
        var divLayer = document.getElementById(objname);
        // Set dialogbox height and width
        SetHeightWidth(divLayer)
        // Set dialogbox top and left
        SetTopLeft(divLayer);

        // Show the div layer
        divLayer.style.display = 'block';
        // Change the location and reset the width and height if window is resized
        window.onresize = function() { if (divLayer.style.display == 'block') { SetTopLeft(divLayer); SetHeightWidth(divLayer) } }
        
        if (objname == 'CalendarDiv') {
            document.getElementById('Text2').value = 'O'
        }
          else {
              document.getElementById('Text2').value = 'O2'
        }
        
        // Set the dialogbox display message
        //document.getElementById('confirmText').innerText = msg;
        //divLayer.className = 'transbox'
    }
}
function SetTopLeft(divLayer) {
    // Get the dialogbox height
    var divHeightPer = divLayer.style.height.split('px')[0];

    // Set the top variable
    var top = (parseInt(document.body.offsetHeight) / 2) - (divHeightPer / 2);
    // Get the dialog box width
    var divWidthPix = divLayer.style.width.split('px')[0];

    // Get the left variable
    var left = (parseInt(document.body.offsetWidth) / 2) - (parseInt(divWidthPix) / 2);
    // set the dialogbox position to abosulute
    divLayer.style.position = 'absolute';


//    var CalButton = document.getElementById('txtDateVon');

    // Set the div top to the height
    divLayer.style.top = top;

    // Set the div Left to the height
    divLayer.style.left = left;
}
function SetHeightWidth(divLayer) {
    // Set the dialogbox width
    divLayer.style.width = divWidth + 'px';
    // Set the dialogbox Height
    divLayer.style.height = divHeight + 'px'
}

function ShowHide(objname) {

    if (document.getElementById(objname).style.display == 'block') {
        document.getElementById(objname).style.display = 'none'
    }
    else {
        document.getElementById(objname).style.display = 'block'

    }
}

function changein(object) {

    object.cells[0].className = "bgstyleleft";

    count = object.cells.length;

    for (i = 1; i <= count - 1; i++) {

        object.cells[i].className = "bgstyle";

    }

    object.cells[count - 1].className = "bgstyleright";

}
function changeout(object) {

    object.cells[0].className = "";

    count = object.cells.length;

    for (i = 1; i <= count - 1; i++) {

        object.cells[i].className = "";

    }

    object.cells[count - 1].className = "";

}
