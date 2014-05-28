<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ServiceCenter1.aspx.vb"
    Inherits="StatusmonitorDAD.ServiceCenter1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<script language="javascript" type="text/javascript">

var lauftext = new  String;
var lauftextKomplett= new String;
var laufschriftElement;
var counter=0;
var LaengeLaufschrift=0;

//die wollen nur eine einfache Laufschrift JJU2008.06.19
/* 
var LaufschriftElement2={
    htmlLaufschriftElemente : null,
    aLaufschriften: null,
    positionInLaufschrift : 0,
    aktuelleLaufschrift: 0,
    laufschriftSpeed: 0,
    initializeHTML : function(parentID)
    {
       var tmpParent=document.getElementById(parentID);
       if(!tmpParent)
       {
         alert("Elternerelment nicht gefunden");
       }
       else
       {
       LaufschriftElement2.htmlLaufschriftElemente=tmpParent.children;
       }//end else
       if( LaufschriftElement2.htmlLaufschriftElemente.length==0)
       {
         alert("Elternerelment enthält keine Kindelemente");
       }//end if
    },//end function initialzeHTML 
    getLaufschriftPart : function()
    {
      var tmp=""+LaufschriftElement2.aLaufschriften[LaufschriftElement2.aktuelleLaufschrift].substring(LaufschriftElement2.positionInLaufschrift,LaufschriftElement2.positionInLaufschrift + LaufschriftElement2.htmlLaufschriftElemente.length);
      return LaufschriftElement2.aLaufschriften[LaufschriftElement2.aktuelleLaufschrift].substring(LaufschriftElement2.positionInLaufschrift,LaufschriftElement2.positionInLaufschrift + LaufschriftElement2.htmlLaufschriftElemente.length);
    },//end getLaufschriftPart
    setLaufschriften : function (aLaufschriftenX)
    {
    var x=0;
    var y=0;
    var emptyVorlauf="";
    var emptynachlauf="";
    
    while (y<LaufschriftElement2.htmlLaufschriftElemente.length)
    {
    emptyVorlauf+=" ";
    emptynachlauf+=" ";//nur bei animation2 weil es nicht am bildschirmrand endet
    y+=1;
    }
    
    while (x<aLaufschriftenX.length)
    {
        // der Nachlauf wird dringend benötigt weil sonst auf der rechten seite die laufschrift nicht "geschoben wird" JJU2008.06.04
       aLaufschriftenX[x]=emptyVorlauf+aLaufschriftenX[x]+emptynachlauf;
     
     
    x+=1;
    }
    
    LaufschriftElement2.aLaufschriften=aLaufschriftenX;
    },//end setLaufschriften
    startLaufschriftAnimation : function()
    {
        animateLaufschrift2.Timer(LaufschriftElement2.laufschriftSpeed,Infinity,getNewLaufschriften2);   
     }   
    
    
   };//end laufschrift element2
   */
   
 //die wollen nur eine einfache Laufschrift JJU2008.06.19
/*   
   function animateLaufschrift2()
{

if (LaufschriftElement2.positionInLaufschrift>LaufschriftElement2.aLaufschriften[LaufschriftElement2.aktuelleLaufschrift].length-1 )
{
    if (LaufschriftElement2.aktuelleLaufschrift>=LaufschriftElement2.aLaufschriften.length-1)
    {
     //keine laufschriten mehr vorhanden,zähler auf 0 setzen funktion beenden das neue Laufschriften nachgeladen werden. 
     LaufschriftElement2.aktuelleLaufschrift=0; 
     LaufschriftElement2.positionInLaufschrift=0;
     return false;
    }
    else//nächste laufschrift
    {
       LaufschriftElement2.aktuelleLaufschrift+=1; 
       LaufschriftElement2.positionInLaufschrift=0;
    }//end else

}//end if


LaufschriftElement2.positionInLaufschrift+=1;
var text=""+LaufschriftElement2.getLaufschriftPart();
var t=LaufschriftElement2.htmlLaufschriftElemente.length;
var b=0;
var X=0;
  var tmpText="";
       
   while(b<t)
   {
   var tmpOBJ=LaufschriftElement2.htmlLaufschriftElemente[b];
   
   X=b+1;
   tmpText=text.substring(b,X)
    
   if (tmpText==" ")//alle leeren strings müssen gegen html-codierung ausgetauscht werden, da die einzelnen spans sonst "verschwinden" und die laufschrift rechts nicht "geschoben wird" JJU2008.06.04
   {
    tmpText="&nbsp;";
   }
   tmpOBJ.innerHTML=tmpText;
     
   b+=1;
   }//end while
        
}//end animateLaufschrift2
*/










//die wollen nur eine einfache Laufschrift JJU2008.06.19
/* 
var LaufschriftElement={
    htmlLaufschriftElemente : null,
    aLaufschriften: null,
    positionInLaufschrift : 0,
    aktuelleLaufschrift: 0,
    laufschriftSpeed: 0,
    initializeHTML : function(parentID)
    {
       var tmpParent=document.getElementById(parentID);
       if(!tmpParent)
       {
         alert("Elternerelment nicht gefunden");
       }
       else
       {
       LaufschriftElement.htmlLaufschriftElemente=tmpParent.children;
       }//end else
       if( LaufschriftElement.htmlLaufschriftElemente.length==0)
       {
         alert("Elternerelment enthält keine Kindelemente");
       }//end if
    },//end function initialzeHTML 
    getLaufschriftPart : function()
    {
      var tmp=""+LaufschriftElement.aLaufschriften[LaufschriftElement.aktuelleLaufschrift].substring(LaufschriftElement.positionInLaufschrift,LaufschriftElement.positionInLaufschrift + LaufschriftElement.htmlLaufschriftElemente.length);
      return LaufschriftElement.aLaufschriften[LaufschriftElement.aktuelleLaufschrift].substring(LaufschriftElement.positionInLaufschrift,LaufschriftElement.positionInLaufschrift + LaufschriftElement.htmlLaufschriftElemente.length);
    },//end getLaufschriftPart
    setLaufschriften : function (aLaufschriftenX)
    {
    var x=0;
    var y=0;
    var emptyVorlauf="";
    var emptynachlauf="";
    
     while (y<LaufschriftElement.htmlLaufschriftElemente.length)
    {
    emptyVorlauf+=" ";
    emptynachlauf+=" ";//bei animation2 weil es nicht am bildschirmrand endet aber da alle gleichschnell laufen sollen auch hier 
    y+=1;
    }
    
   while (x<aLaufschriftenX.length)
    {
        // der Nachlauf wird dringend benötigt weil sonst auf der rechten seite die laufschrift nicht "geschoben wird" JJU2008.06.04
       aLaufschriftenX[x]=emptyVorlauf+aLaufschriftenX[x]+emptynachlauf;
          
    x+=1;
    }
    
    LaufschriftElement.aLaufschriften=aLaufschriftenX;
    },//end setLaufschriften
    startLaufschriftAnimation : function()
    {
        animateLaufschrift.Timer(LaufschriftElement.laufschriftSpeed,Infinity,getNewLaufschriften);   
     }   
    
    
   };//end laufschrift element*/
   
      
   

//die wollen nur eine einfache Laufschrift JJU2008.06.19
/* 
  function animateLaufschrift()
{

if (LaufschriftElement.positionInLaufschrift>LaufschriftElement.aLaufschriften[LaufschriftElement.aktuelleLaufschrift].length-1 )
{
    if (LaufschriftElement.aktuelleLaufschrift>=LaufschriftElement.aLaufschriften.length-1)
    {
    //keine laufschriten mehr vorhanden,zähler auf 0 setzen funktion beenden das neue Laufschriften nachgeladen werden. 
     LaufschriftElement.aktuelleLaufschrift=0; 
     LaufschriftElement.positionInLaufschrift=0;
     return false;
    }
    else//nächste laufschrift
    {
       LaufschriftElement.aktuelleLaufschrift+=1; 
       LaufschriftElement.positionInLaufschrift=0;
    }//end else

}//end if


LaufschriftElement.positionInLaufschrift+=1;
var text=""+LaufschriftElement.getLaufschriftPart();
var t=LaufschriftElement.htmlLaufschriftElemente.length;
var b=0;
  
      
   while(b<t)
   {
  var tmpOBJ=LaufschriftElement.htmlLaufschriftElemente[b];
   
   X=b+1;
   tmpText=text.substring(b,X)
    
   if (tmpText==" ")//alle leeren strings müssen gegen html-codierung ausgetauscht werden, da die einzelnen spans sonst "verschwinden" und die laufschrift rechts nicht "geschoben wird" JJU2008.06.04
   {
    tmpText="&nbsp;";
   }
   tmpOBJ.innerHTML=tmpText;
     
   b+=1;
   }//end while
        
}//end animateLaufschrift
*/




//die wollen nur eine einfache Laufschrift JJU2008.06.19
/* 
Function.prototype.Timer = function (interval, calls, onend) {
  var count = 0;
  var payloadFunction = this;
  var startTime = new Date();
  var callbackFunction = function () {
    return payloadFunction(startTime, count);
  };
  var endFunction = function () {
    if (onend) {
      onend(startTime, count, calls);
    }
  };
  var timerFunction = function () {
    count++;
    if (count < calls && callbackFunction() != false) {
      window.setTimeout(timerFunction, interval);
    } else {
      endFunction();
    }
  };
  timerFunction();
};*/

//die wollen nur eine einfache Laufschrift JJU2008.06.19
/* 
function getNewLaufschriften()
{
var aLaufschriften=document.getElementById("hiddenLaufschriftLeft").value.split('|');
//letzter teile zu viel, entfernen
aLaufschriften.pop();
LaufschriftElement.setLaufschriften(aLaufschriften);
if (aLaufschriften.length >0)
{
LaufschriftElement.startLaufschriftAnimation(); 
}
else
{
   setTimeout(getNewLaufschriften,30000);//wenn keine Laufschriften vorhanden, nochmal nach 30 sekunden prüfen 
}

}//end getNewLaufschriften
*/

//die wollen nur eine einfache Laufschrift JJU2008.06.19
/* 
function getNewLaufschriften2()
{
var aLaufschriften=document.getElementById("hiddenLaufschriftRight").value.split('|');
//letzter teile zu viel, entfernen
aLaufschriften.pop();
LaufschriftElement2.setLaufschriften(aLaufschriften);
if (aLaufschriften.length >0)
{
LaufschriftElement2.startLaufschriftAnimation(); 
}
else
{
   setTimeout(getNewLaufschriften2,30000);//wenn keine Laufschriften vorhanden, nochmal nach 30 sekunden prüfen 
}

}//end getNewLaufschriften2
*/


function laufschrift() {
 
  if (lauftextKomplett.length!=0)
  {
  if (counter==lauftextKomplett.length-LaengeLaufschrift) //wenn einmal durchgelaufen
  {
   getNewLaufschrift();
   counter=0;
  }
  else
  {
  getLaufschriftPart(counter);
  
  counter+=1;
  //lauftext = lauftext.substring(1, lauftext.length)+ lauftext.substring(0,1);
  
  
  var TmpLauftext=lauftext.replace(/ /g,"&nbsp;"); //richtige leerzeichen einfügen sonst läufts net
      laufschriftElement.innerHTML=TmpLauftext;
  }
  }
  else //wenn kein Lauftext vorhanden
  {
   setTimeout(getNewLaufschrift,30000);
  }
  
}

function getNewLaufschrift()
{
lauftextKomplett="";
var i=0;
while (i<LaengeLaufschrift)
{
lauftextKomplett+=" ";
i+=1;
}
lauftextKomplett+=document.getElementById("hiddenLaufschrift").value;

var i=0;
while (i<LaengeLaufschrift)
{
lauftextKomplett+=" ";
i+=1;
}
//lauftext=lauftextKomplett.substring(0,100);
}//end getNewLaufschrift

function getLaufschriftPart(counter)
{
lauftext=lauftextKomplett.substring(counter,counter+LaengeLaufschrift);
 
}


function callFunctions()
{
LaengeLaufschrift=70;
laufschriftElement=document.getElementById("lblLaufschrift");
getNewLaufschrift();
//setTimeout(getNewLaufschrift,30000);
setInterval("laufschrift()", 150);


//die wollen nur eine einfache Laufschrift JJU2008.06.19
/* 
LaufschriftElement.initializeHTML("laufschriftLeft");

var aLaufschriften=document.getElementById("hiddenLaufschriftLeft").value.split('|');
//letzter teile zu viel, entfernen
aLaufschriften.pop();
LaufschriftElement.setLaufschriften(aLaufschriften);
//setColors wird noch implementiert.
LaufschriftElement.laufschriftSpeed=300; //weniger ist schneller
if (aLaufschriften.length >0)
{
LaufschriftElement.startLaufschriftAnimation(); 
}
else
{
getNewLaufschriften();
}
*/

//die wollen nur eine einfache Laufschrift JJU2008.06.19
/* 
LaufschriftElement2.initializeHTML("laufschriftRight");
var aLaufschriften=document.getElementById("hiddenLaufschriftRight").value.split('|');
//letzter teile zu viel, entfernen
aLaufschriften.pop();
LaufschriftElement2.setLaufschriften(aLaufschriften);
//setColors wird noch implementiert.
LaufschriftElement2.laufschriftSpeed=300; //weniger ist schneller
if (aLaufschriften.length >0)
{
LaufschriftElement2.startLaufschriftAnimation(); 
}
else
{
getNewLaufschriften2();
}
*/


}//end callFunctions
window.onload=callFunctions;

   

</script>

<head runat="server">
    <title>DAD-Statusmonitor</title>
</head>
<body>
    <form style="font-family: Arial" id="form1" runat="server">
    <asp:scriptmanager id="ScriptManager1" runat="server">
    </asp:scriptmanager>
    <asp:timer id="timerRefresh" interval="11000" runat="server">
    </asp:timer>
    <asp:updatepanel id="upHiddenFields" runat="server">
        <contenttemplate>
       
            <input id="hiddenLaufschriftLeft" runat="server" value="DIES IST EIN TEST !DIES IST EIN TEST !DIES IST EIN TEST !DIES IST EIN TEST !DIES IST EIN TEST !DIES IST EIN TEST !DIES IST EIN TEST !DIES IST EIN TEST !"
                type="hidden" />
            <input id="hiddenLaufschriftRight" runat="server" type="hidden" />
             <input id="hiddenLaufschrift" runat="server" type="hidden" value=" +++ Read my lips: no tax on beer "  /> 
        </contenttemplate>
    </asp:updatepanel>
    <div>
        <table runat="server" id="TableMain" width="100%">
            <tr id="trHead">
                <td>
                    <table runat="server" cellpadding="0" cellspacing="0" id="TableHead">
                        <tr style="width: 100%">
                            <td colspan="3">
                                <asp:image imagealign="AbsBottom" id="Image3" height="20" width="100%" runat="server"
                                    imageurl="~/Images/HeaderBalkentop10.jpg" />
                            </td>
                        </tr>
                        <tr style="width: 100%" nowrap="noWrap">
                            <td style="border-bottom: solid 1px black; border-top: solid 1px black;">
                                <asp:image id="imgLogoHeaderRight" runat="server" imageurl="~/Images/dadlogo.jpg" />
                            </td>
                            <td style="width: 100%; border-bottom: solid 1px black; border-top: solid 1px black;"
                                align="center" valign="middle">
                                <asp:updatepanel id="upStand" runat="server">
                                    <contenttemplate>
                                        <asp:Label runat="server" Text="Stand:" ID="lblStand"></asp:Label></contenttemplate>
                                </asp:updatepanel>
                                <asp:label runat="server" id="lblHeaderText" font-size="XX-Large" font-bold="True"
                                    text="Online Status Report DAD Service Center 1"></asp:label>
                            </td>
                            <td style="border-bottom: solid 1px black; border-top: solid 1px black;">
                                <asp:image imagealign="Right" id="imgLogoHeaderLeft" runat="server" imageurl="~/Images/dadlogo.jpg" />
                            </td>
                        </tr>
                        <tr style="width: 100%">
                            <td colspan="3">
                                <asp:image id="Image4" height="20" width="100%" runat="server" imageurl="~/Images/HeaderBalkenBottom10.jpg" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </td>
            </tr>
            <tr id="trBody" runat="server">
                <td>
                    <table runat="server" cellpadding="4" style="border: solid 1 black" id="TableBody"
                        width="100%">
                        <tr>
                            <td valign="top" style="width: 50%" align="center">
                                <asp:updatepanel runat="server" id="upGvLeft">
                                    <triggers>
                                        <asp:AsyncPostBackTrigger ControlID="timerRefresh" EventName="Tick" />
                                    </triggers>
                                    <contenttemplate>
                                        <asp:GridView AutoGenerateColumns="False" BorderWidth="0" BorderStyle="None" runat="server"
                                            Width="100%" ID="gvLeft">
                                            <AlternatingRowStyle BorderWidth="1" BorderStyle="solid" VerticalAlign="Middle" HorizontalAlign="Left"
                                                Font-Bold="true" Font-Size="XX-Large" BackColor="WhiteSmoke" />
                                            <RowStyle Height="60px" Width="120px" BorderWidth="0" BorderStyle="none" VerticalAlign="Middle"
                                                HorizontalAlign="Left" Font-Bold="true" Font-Size="XX-Large" />
                                              <Columns>
                                                <asp:TemplateField>
                                                   
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgKundenLogo" runat="server" Height="55px" ImageUrl='<%# DataBinder.Eval(Container, "DataItem.Logo") %>'
                                                            Visible='<%# not DataBinder.Eval(Container, "DataItem.Logo") is System.DBNull.Value  AND  not DataBinder.Eval(Container, "DataItem.logo")=""  %>' />
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abteilung") %>'
                                                            ID="lblAnzeigeKundenName" Visible='<%# DataBinder.Eval(Container, "DataItem.logo") is System.DBNull.value OR DataBinder.Eval(Container, "DataItem.logo")=""  %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="ImgGut" runat="server" ImageUrl="~/Images/AllesOK2.JPG" Visible='<%# DataBinder.Eval(Container, "DataItem.StatusNeu")=0 %>' />
                                                        <asp:Image ID="ImgSchlecht" runat="server" ImageUrl="~/Images/Problem.jpg" Visible='<%# DataBinder.Eval(Container, "DataItem.StatusNeu")=1 %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </contenttemplate>
                                </asp:updatepanel>
                            </td>
                            <td valign="top" style="width: 50%" align="center">
                                <asp:updatepanel runat="server" id="upGvRight">
                                    <contenttemplate>
                                        <asp:GridView AutoGenerateColumns="False" BorderWidth="0" BorderStyle="None" runat="server"
                                            Width="100%" ID="gvRight">
                                            <AlternatingRowStyle BorderWidth="1" BorderStyle="solid" VerticalAlign="Middle" HorizontalAlign="Left"
                                                Font-Bold="true" Font-Size="XX-Large" BackColor="WhiteSmoke" />
                                            <RowStyle Height="60px" Width="120px" BorderWidth="0" BorderStyle="none" VerticalAlign="Middle"
                                                HorizontalAlign="Left" Font-Bold="true" Font-Size="XX-Large" />
                                              <Columns>
                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                        <asp:Image ID="imgKundenLogo" runat="server" Height="55px" ImageUrl='<%# DataBinder.Eval(Container, "DataItem.Logo") %>'
                                                            Visible='<%# not DataBinder.Eval(Container, "DataItem.Logo") is System.DBNull.Value  AND  not DataBinder.Eval(Container, "DataItem.logo")=""  %>' />
                                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Abteilung") %>'
                                                            ID="lblAnzeigeKundenName" Visible='<%# DataBinder.Eval(Container, "DataItem.logo") is System.DBNull.value OR DataBinder.Eval(Container, "DataItem.logo")=""  %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    
                                                    <ItemTemplate>
                                                        <asp:Image ID="ImgGut" runat="server" ImageUrl="~/Images/AllesOK2.jpg" Visible='<%# DataBinder.Eval(Container, "DataItem.StatusNeu")=0 %>' />
                                                        <asp:Image ID="ImgSchlecht" runat="server" ImageUrl="~/Images/Problem.jpg" Visible='<%# DataBinder.Eval(Container, "DataItem.StatusNeu")=1 %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </contenttemplate>
                                </asp:updatepanel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                </td>
            </tr>
            <tr id="trFooter" runat="server">
                <td>
                    <table cellpadding="0" cellspacing="0" width="100%" id="TableFooter">
                        <tr>
                            <td>
                            </td>
                            <td>
                                
                                &nbsp;</td>
                            <td>
                            </td>
                        </tr>
                        <tr style="width: 100%" valign="bottom">
                            <td style="width: 100%;" colspan="3">
                                <asp:image id="Image1" imagealign="Right" width="50%" height="10" runat="server"
                                    imageurl="~/Images/FooterRightTop.jpg" />
                                <asp:image id="Image2" imagealign="Left" height="10" width="50%" runat="server" imageurl="~/Images/FooterLeftTop.jpg" />
                            </td>
                        </tr>
                        <tr style="width: 100%">
                            <td colspan="3" id="laufschriftLeft" style="width: 100%;" align="right" valign="middle">
                                <asp:label runat="server" id="lblLaufschrift" style="width:100%" 
                                    Font-Bold="True" Font-Names="Courier New" Font-Size="20pt"></asp:label>
                            </td>
                        </tr>
                        <tr style="width: 100%" valign="top">
                            <td style="width: 100%" colspan="3">
                                <asp:image id="Image5" imagealign="Left" width="50%" height="10" runat="server" imageurl="~/Images/FooterLeftBottom.jpg" />
                                <asp:image id="Image6" imagealign="Right" height="10" width="50%" runat="server"
                                    imageurl="~/Images/FooterRightBottom.jpg" />
                            </td>
                        </tr>
                        <tr style="width: 100%; height:22px" align="center">
                            <td>
                             &nbsp;
                            </td>
                            <td>
                             &nbsp;
                            </td>  
                        </tr>                      
                        <tr style="width: 100%" align="center">
                            <td>
                                </td>
                            <td>                                <asp:label runat="server" id="Label1" font-size="XX-Large" font-bold="True"
                                    text="Status: Service Level Agreement - Vorgaben"></asp:label>
                                
                             </td>
                            <td>
                            </td>
                        </tr>                        
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
