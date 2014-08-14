var interval
var currentMenue
var currentImg
var currentImgNormalSrc
var currentImgHoverSrc
function showMenue(StateWhat, MenueWhat, imgObj) {
    try {
        //1=Mousein 
        //2=MouseOut 


        if (typeof (MenueWhat) == 'string') {
            setCurrentMenue(MenueWhat);
            if (typeof (imgObj) == 'object') {
                if (currentImg != imgObj) {
                    currentImg = imgObj;
                    if (imgObj.src.indexOf('Normal', 0) != -1) {
                        currentImgNormalSrc = imgObj.src;

                        currentImgHoverSrc = imgObj.src.replace('Normal', 'Hover');

                    }
                    else {
                        currentImgHoverSrc = '';
                        currentImgNormalSrc = '';
                    }
                }


            }
        }

        if (StateWhat == '1') {
            changeInterval(2);
            changeVisible(1);
        }
        if (StateWhat == '2') {
            changeInterval(1);
        }
    }


    catch (ex) {
        alert("fehler" + ex);
    } //end Catch
} //end showMenue()


function setCurrentMenue(Menue) {
    if (currentMenue != null) {

        if (currentMenue.id != Menue) {
            changeVisible(2);
            currentMenue = document.getElementById(Menue);
        }

    }
    else {
        currentMenue = document.getElementById(Menue);
    }

}

function changeInterval(State) {

    if (State == 1) {
        if (interval == null) {
            interval = setInterval('changeVisible(2)', 800);
        }
    }
    else {
        if (interval != null) {
            clearInterval(interval);
            interval = null;
        }
    }
} //end changeInterval

function changeVisible(State) {
    //1=true,2=false

    //var Container = document.getElementById('menueContainer');
    if (currentMenue != null) {

        if (State == 1)
        // if (document.getElementById('MenueState').value!='')
        {
            currentMenue.style.display = 'block';
            if (currentImg != null) {
                currentImg.src = currentImgHoverSrc;
            }

        }
        else {
            currentMenue.style.display = 'none';
            if (currentImg != null) {
                currentImg.src = currentImgNormalSrc;
            }
            changeInterval(2);
        }
    }
} //end changeVisible

function toggleNavigation() {
    var navigation = document.getElementById('innerContentLeft');
    var img = document.getElementById('navDisplayArrow');
    var content = document.getElementById('innerContentRight');

    if (navigation.style.display == 'none') {
        navigation.style.display = 'block';
        img.src = '/PortalZLD/Images/navDisplayArrowLeft.gif';
        content.style.width = '78%';
    }
    else {
        navigation.style.display = 'none';
        img.src = '/PortalZLD/Images/navDisplayArrowRight.gif';
        content.style.width = '100%';
    }
}

function toggleQuery() {
    var TableQuery = document.getElementById('TableQuery');
    var img = document.getElementById('navDisplayArrow');
    var data = document.getElementById('data');

    if (TableQuery.style.display == 'none') {
        TableQuery.style.display = 'block';
        img.src = '/PortalZLD/Images/navDisplayArrowLeft.gif';
    }
    else {
        TableQuery.style.display = 'none';
        img.src = '/PortalZLD/Images/navDisplayArrowRight.gif';
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
        //        document.Form1.SelOpen2.value = ''
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
        //        document.Form1.SelOpen2.value = "O";
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
