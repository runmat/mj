<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SucheHaendler.ascx.vb"
    Inherits="CKG.Components.ComCommon.SucheHaendler" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../../PageElements/Styles.ascx" %>
<input id="hidden_Haendlernummer" type="hidden" name="hidden_Haendlernummer" runat="server">
<input id="hidden_Ort" type="hidden" name="jSON_Ort" runat="server">
<input id="hidden_Name1" type="hidden" name="hidden_Name1" runat="server">
<input id="hidden_Name2" type="hidden" name="hidden_Name2" runat="server">
<input id="hidden_Strasse" type="hidden" name="hidden_Strasse" runat="server">
<input id="hidden_PLZ" type="hidden" name="hidden_PLZ" runat="server">

<script type="text/javascript" language="javascript">
<!--
var jSON_HaendlerNR;
var jSON_PLZ;
var jSON_Name1;
var jSON_Name2;
var jSON_Ort;
var jSON_Strasse;
var haendlerLB;
var aLBOptions= new Array();
var aInputFields=new Array(4);
var aLBaktuelleOptions= new Array();
var aLBBlackList=new Array();
var aPerfomanceForward=new Array();
var lblprocessing;
var lblwait;
var absatz;
var initalDatenbestand;
var aktuellerDatenbestand;
var lblErgebnissAnzahl;
var direktInput=false;
//detail anzeigen der Händler informationen
//---------------------------------------
var lblHaendlerDetailsNR;
var lblHaendlerDetailsName1;
var lblHaendlerDetailsName2;
var lblHaendlerDetailsOrt;
var lblHaendlerDetailsPLZ;
var lblHaendlerDetailsStrasse;
//---------------------------------------




function checkInputs()
{
	//JJU2008.3.18
	//die listbox wird der globalen variable haendlerLB  zugewiesen, es wird überprüft ob direkt input an oder aus sein soll, 
	//der eventhandler für selektion changed wird gesetzt und der initiale datenbestand wird gesichert. 
	//-------------------------------------------------------------------------
	var lbldirektInput;
	var tmphiddenHaendlernummer;//wird verwendet um direktInput an oder auszuschalten, wird serverseitig entschieden
	try
	{
	haendlerLB=window.document.getElementById("SucheHaendler1_lbHaendler");
	lbldirektInput=window.document.getElementById("SucheHaendler1_lbldirektInput");
	tmphiddenHaendlernummer=window.document.getElementById("SucheHaendler1_hidden_Haendlernummer");
	
		
		if  (tmphiddenHaendlernummer.value.length<1)// wenn hidden field befüllt ist dann direkt input gestattet, serverseitige steuerung
		{
			direktInput=false;
			lbldirektInput.style.display="none";
			return true;//verlässt funktion aber da von onLoad ereignis aufgerufen, muss true zurückgegeben werden sonst kein load
		}
		
		lbldirektInput.style.display="";
		direktInput=true;
		
		haendlerLB.onchange=lbSelectionChanged;//funktion für selection changed festlegen bei lb
		
	//	alert("Anzahl einträge in lb: " + haendlerLB.options.length);
		if (haendlerLB.options.length>0)
		{
		
		for (var i = 0; i < haendlerLB.options.length; i++)
		{
			aLBOptions[i]=haendlerLB.options[i];//initalen Datenbestand sichern
		}
		
		initalDatenbestand=aLBOptions.length;
		
	//	alert("Anzahl einträge in OptionSaveArray: " + aLBOptions.length);
		generateJSONArrays();
		}
		}//end try
			
	catch(ex)
	{
		alert("fehler  in checkInputs");
		lbldirektInput.style.display="none";
		direktInput=false;
		return true
	}	
	//-------------------------------------------------------------------------
	
}//end function 



function generateJSONArrays()
{
	//JJU2008.3.18
	//es werden die JSON Arrays aus den Hidden Fields generiert und auf richtigkeit geprüft
	//------------------------------------------------------------------------	
	var hiddenHNR;
	var hiddenName1;
	var hiddenName2;
	var hiddenOrt;
	var hiddenPLZ;
	var hiddenStrasse;
	try
	{
	//werte in temp variablen schreiben
	hiddenHNR=eval(window.document.getElementById("SucheHaendler1_hidden_Haendlernummer").value);
	hiddenName1=eval(window.document.getElementById("SucheHaendler1_hidden_Name1").value);
	hiddenName2=eval(window.document.getElementById("SucheHaendler1_hidden_Name2").value);
	hiddenOrt=eval(window.document.getElementById("SucheHaendler1_hidden_Ort").value);
	hiddenPLZ=eval(window.document.getElementById("SucheHaendler1_hidden_PLZ").value);
	hiddenStrasse=eval(window.document.getElementById("SucheHaendler1_hidden_Strasse").value);
	
	//alert("hidden fieldsSuche Beendet");
	
	if (!hiddenHNR || !hiddenName1 || !hiddenName2 || !hiddenOrt || !hiddenPLZ || !hiddenStrasse)
	{
		alert("FEHLER: eines der HiddenFields konnte auf dem Formular nicht gefunden werden");
	}
	//alert("hidden fieldsPrüfung Beendet");
	
	
	/*alert(""+hiddenHNR);
	alert(""+hiddenPLZ);
	alert(""+hiddenName1);
	alert(""+hiddenName2);
	alert(""+hiddenOrt);*/
	
	//JSON arrays generieren
	jSON_HaendlerNR=eval(" ("+ hiddenHNR + ")");
	jSON_PLZ=eval(" ("+ hiddenPLZ + ")");
	jSON_Name1=eval(" ("+ hiddenName1 + ")");
	jSON_Name2=eval(" ("+ hiddenName2 + ")");
	jSON_Ort=eval(" ("+ hiddenOrt + ")");
	jSON_Strasse=eval(" ("+ hiddenStrasse + ")");
	
	//alert("Umwandlung in jSON Arrays ist erfolgt");
	
		
	/*alert ("PLZ= "+jSON_PLZ.length);
	alert ("name1= "+jSON_Name1.length);
	alert ("name2= "+jSON_Name2.length);
	alert ("ORT= "+jSON_Ort.length);
	alert ("haendlerNR= "+jSON_HaendlerNR.length);*/

	var laenge=jSON_HaendlerNR.length;

	if (jSON_PLZ.length != laenge || jSON_Name1.length != laenge || jSON_Name2.length != laenge || jSON_Ort.length !=laenge || jSON_Strasse.length!=laenge)
	{
	alert("FEHLER: die Arrays sind nicht gleich lang!");
	}
	//alert("ArrayLängenPrüfung Beendet");	
	getTheInputFields();		
	}
	catch(ex)
	{
		alert("Fehler in 'generateJSONArrays': " +ex);
	}
	//------------------------------------------------------------------------
}//end function 

function getTheInputFields()
{
	//JJU2008.3.18
	//alle felder/objekte die im javascript code global verwendet werden, werden hier gesetzt
	//------------------------------------------------------------------------
	try
	{
	//eingabefelder in array schreiben
	//-------------------------------------
	aInputFields[0]=window.document.getElementById("SucheHaendler1_txtNummer");
	aInputFields[1]=window.document.getElementById("SucheHaendler1_txtName1");
	aInputFields[2]=window.document.getElementById("SucheHaendler1_txtName2");
	aInputFields[3]=window.document.getElementById("SucheHaendler1_txtOrt");
	aInputFields[4]=window.document.getElementById("SucheHaendler1_txtPLZ");
	//-------------------------------------
	
	
	
	//warte Anzeige zuweisen/erstellen
	//-------------------------------------
	//warte label zuweisen
	lblwait=window.document.getElementById("SucheHaendler1_lblwait");
	//ergebnisanzahl label zuweisen
	lblErgebnissAnzahl=window.document.getElementById("SucheHaendler1_lblErgebnissAnzahl");
	//knoten generieren label "processing"
	 absatz=document.createElement("br");
	 lblprocessing=document.createElement("span");
		 
	 lblprocessing.setAttribute("id", "lblprocessing");
	//geht leider irgendwie nicht ka 
	// lblprocessing.setAttribute("style", "color:Red;font-weight:bold;");
	 lblprocessing.appendChild(document.createTextNode(" processing..."));
	 //-------------------------------------
	 
	 //Händler Detail anzeige zuweisen
	 //-------------------------------------
	 lblHaendlerDetailsName1=window.document.getElementById("SucheHaendler1_lblHaendlerDetailsName1");
	 lblHaendlerDetailsName2=window.document.getElementById("SucheHaendler1_lblHaendlerDetailsName2");
	 lblHaendlerDetailsNR=window.document.getElementById("SucheHaendler1_lblHaendlerDetailsNR");
	 lblHaendlerDetailsPLZ=window.document.getElementById("SucheHaendler1_lblHaendlerDetailsPLZ");
	 lblHaendlerDetailsOrt=window.document.getElementById("SucheHaendler1_lblHaendlerDetailsOrt");
	 lblHaendlerDetailsStrasse=window.document.getElementById("SucheHaendler1_lblHaendlerDetailsStrasse");
	 //-------------------------------------
	 
	 
	 //bei einem Postback ist alles javascript verloren, dh ursprungszustand wiederherstellen
	 //alle selektionen nochmal durchlaufen in denen sich eine eingabe befindet
	 for (var y=0; y<aInputFields.length; y++)
	 {
		//focus auf das erste feld setzen das eine eingabe enthält
		var boolGesetzt=false;
		if (aInputFields[y].value.length !=0)
		{
			if (boolGesetzt==false)
			{
				aInputFields[y].focus();
				//getrickst um focus hinter letztes zeichen zu setzen
				aInputFields[y].value += '';
				boolGesetzt=true;
			}
			
			search(aInputFields[y]);
		}
	 }
	 
	 //------------------------------------------------------------------------
	 	 
	
	}
	catch(ex)
	{
		alert("Fehler in getTheInputFields: " +ex);
	}
	
}//end  getTheInputFields()



function search(obj)
{
//JJU2008.3.18
//Hauptfunktion, Performance Prüfung und weiterleitung selektion laut benutzereingaben, abschließende tätigkeit, wird  bei einem keyup in einer Such-textbox aufgerufen
//------------------------------------------------------------------------
	if (direktInput)
	{
	
	if (obj)
	{
	
	//ersetze fälschlichen * durch ""
	obj.value=obj.value.replace("*","");
	
	
	//perfomance array erstellen/auswerten/erweitern, dieses dient zur prüfung ob der schon selektierte datenbestand weiterverwendet werden kann oder aus dem initialdatenbestand
	//selektiert werden muss
	//------------------------------------------------------------------------
	 var boolFound=false;
	 for (var i = 0; i < aPerfomanceForward.length; i++)
	 {
			if (obj==aPerfomanceForward[i][0])
			{
				boolFound=true;
			//	alert("objekt in aPerfomanceForward bekannt: "+ obj.id );
			//	alert("vergleich beider objekte in der länge obj= "+obj.value.length +"  im array= " + aPerfomanceForward[i][1]);
				if (obj.value.length < aPerfomanceForward[i][1])
				{
					//alert("selektionsarray aLBOptions muss zurückgesetzt werden da selektion rückläufig");
					//dh es wurde ein Zeichen entfernt somit muss die selektion wieder von grund auf durchgeführt werden
					for (var z = 0; z < aLBOptions.length; z++)
					{
						var newOption = new Option(aLBOptions[z].text, aLBOptions[z].value);
						aLBaktuelleOptions[z]=newOption;
					}//end for 				
				}
				else
				{
					//nichts machen da altes selektionsarray weiterverwendet werden kann
					//alert("selektionsarray aLBOptions kann weiterverwendet werden, selektion vorläufig");
				}
				//arrayWert Länge neu setzen, egal welcher vorgang vorgenommen worden ist
				//alert("wert von i bei neu setzen der länge eines objekt values= "+i);
				aPerfomanceForward[i][1]=obj.value.length;
				break;
			}	 
	 }
	 if (!boolFound)
	 {		
		//objekt dem aperfomance array hinzufügen, gleichbedeutend mit altes array kann weiterverwendet werden, weil vorläufe selektion
		if (aPerfomanceForward.length==0)//ist es die ersteingabe muss  selektionsarray befüllt werden
		{
					for (var i = 0; i < aLBOptions.length; i++)
					{
						var newOption = new Option(aLBOptions[i].text, aLBOptions[i].value);
						aLBaktuelleOptions[i]=newOption;
					}//end for 		
				//	alert("selektionsarray inital befüllt");	
		}
		//alert("objekt in aPerfomanceForward NICHTbekannt->hinzufügen");
		//hinzufügen eines arrays in das array, 1. position ist das objekt, die zweite die länge des objektes 
		//dies ist nötig weil das objekt auf die texbox referenziert und somit nie alte länge behält, hier muss alte mit neuer länge verglichen werden
		aPerfomanceForward.unshift(new Array(obj,obj.value.length));
	 }				

	//------------------------------------------------------------------------

	/*alert("items in aktuelleOption Array nach prüfung des performance arrays");
	for (var i=0; i< aLBaktuelleOptions.length; i++)
	{
		alert(""+i + " " + aLBaktuelleOptions[i].text);
	}*/
	
		
		
	//alle eingaben prüfen und wenn sie nicht leer sind zur bei selektion für den neuen LB inhalt beachten
	//------------------------------------------
	for (var i=0; i<aInputFields.length; i++)
	{	
	//alert("input field nummer " + i + " Eingabewert=" + aInputFields[i].value);
	if (aInputFields[i].value!="")
	{
		switch (aInputFields[i].id)
		{
		case "SucheHaendler1_txtNummer": checkUserInput(jSON_HaendlerNR,aInputFields[i]);
		break;
		case "SucheHaendler1_txtName1": checkUserInput(jSON_Name1,aInputFields[i]); 
		break;
		case "SucheHaendler1_txtName2": checkUserInput(jSON_Name2,aInputFields[i]);
		break;
		case "SucheHaendler1_txtOrt": checkUserInput(jSON_Ort,aInputFields[i]);
		break;
		case "SucheHaendler1_txtPLZ": checkUserInput(jSON_PLZ,aInputFields[i]);
		break;
		default: alert("Fehler: Unbekanntes input feld in inputfelder-array");
		}//end switch
	}//end if 
	}//end for
	//------------------------------------------
	
	
	
	//neu befüllen der LB
	//------------------------------------------
	haendlerLB.options.length=0;
	for (var i=0; i<aLBaktuelleOptions.length;i++)
	{
	haendlerLB.options[i]=aLBaktuelleOptions[i];
	}//end for
	aktuellerDatenbestand=aLBaktuelleOptions.length;
	//------------------------------------------
	
	
	
	//ende des programms, finale aktionen durchführen
	//------------------------------------------
	//warte anzeige wieder ausblenden
	 lblwait.style.display = "none";
	 absatz.removeNode(true);
	 lblprocessing.removeNode(true);
	 //ergebnisAnzeige aktuallisieren
	 lblErgebnissAnzahl.innerHTML=""+ aLBaktuelleOptions.length;
	 //wenn ergebnisse <20 dann selektion auf 1. option 
	 if (aLBaktuelleOptions.length<20 )
	 {
		//wenn kleiner 20 ergebnisse
		lblErgebnissAnzahl.style.color="orange";
		lblErgebnissAnzahl.style.fontSize="14";
		lblErgebnissAnzahl.style.fontWeight="bold";
	 
		//alert("selected Index auf 0 gesetzt");
		haendlerLB.selectedIndex = 0;
		
		//alert(" selectedIndex= " + haendlerLB.selectedIndex + " Value= " +haendlerLB.value); 
		if (aLBaktuelleOptions.length<4 && aLBaktuelleOptions.length>0 )
		{
		lblErgebnissAnzahl.style.color="green";
		lblErgebnissAnzahl.style.fontSize="16";
		lblErgebnissAnzahl.style.fontWeight="bolder";
		}
	
		
		//spätere Implementierung JJU2008.04.07
		//---------------------------------------------------------
		//wenn ein einzelnes ergbniss vorliegt, finde ich sollte die eingabe abgebrochen werden, aber 
		//ist vll schwer für den benutzer zu verstehen bzw bin ich jetzt zu faul,
		//lösung könnte ein versetzen des focuses sein oder das austauschen der eingabe gegen
		//leere strings, wichtig ist das der focus am ende wieder in der selektionsbox bleibt!
		//if (aLBaktuelleOptions.length==1)
		//{
		//}	
		//---------------------------------------------------------
		
				
		if (aLBaktuelleOptions.length==0)
		{
		//wenn keine ergebnisse
		lblErgebnissAnzahl.style.color="red";
		lblErgebnissAnzahl.style.fontSize="14";
		lblErgebnissAnzahl.style.fontWeight="bold";
		}	
		
	 }
	 else
	 {
		//wenn mehr als 20 normales label
		lblErgebnissAnzahl.style.color="black";
		lblErgebnissAnzahl.style.fontSize="12";
		lblErgebnissAnzahl.style.fontWeight="bold";
	 }
	 
	 	  //------------------------------------------
	 //aufrufen da programmatisch gesetzte selektionen kein changedEvent feuern, bzw wenn kein selektion dann noselection anzeigen
	 lbSelectionChanged();
	
	
	 
	}//end if 
	else
	{
		alert("obj nicht übergeben!");
	}//end else obj
	}//end if direktinput
	//------------------------------------------------------------------------
}//end search

function showWaitInfo(obj)
{
//JJU2008.3.18
//------------------------------------------------------------------------
//warte anzeige kann nicht im gleichen event wie berechnung ausgeführt werden,
//da innerhalb einer javaskript berechnung nicht angezeigt und dann wieder ausgeblendet werden kann
//wird von einem keydown in der jeweiligen such-textbox aufgerufen
//------------------------
	//prüfen ob es sich lohnt eine warteanzeige zu setzen, sonst bei jedem tastendruck diese kurz aufblenden würde, sieht doof aus->
	//ja wenn eingabe suche rückwärts und initalDatenbestand >waitAnzeigeLvl
	//ja wenn eingabe suche vorwärts und aktuelle DS >waitAnzeigeLvl oder initalDatenbestand>800
	//keine anzeige wenn tabulator gedrückt wurde!
	//------------------------
	//hier muss beachtet werden das die funktion aus onkeydown aufgerufen wird, somit steht 
	//in der aufrufenden textbox noch nicht das zeichen, es muss der keycode der taste abgefragt werden
	//um festzustellen ob vorläufige oder rückläufige selektion, nicht so genau wie das arbeiten
	//mit dem aPerfomanceForward-array, da es ja vorkommen kann das mehrere (markierte) Zeichen durch eines ersetzt werden
	//aber ich sehe grad keine andere möglichkeit 
	//------------------------------------------------------------------------
	if(direktInput)
	{
	if (obj)
	{
	
	var anzeige=false;
	var waitAnzeigeLvl=250;
	try
	{
	//var txtAnzeigeInfo=window.document.getElementById("SucheHaendler1_txtAnzeigeInfo");
	
	//txtAnzeigeInfo.value="länge von aPerfomanceForward= " +aPerfomanceForward.length;

	
	if (aPerfomanceForward.length!=0 )
	{
			if (window.event.keyCode==8 || window.event.keyCode==46 )	
			{
				//46=entf, 8=backspace
				//rückläufige suche
				if (initalDatenbestand > waitAnzeigeLvl)
				{
			//	txtAnzeigeInfo.value="rückläufeigeSuche";
					anzeige=true;
				}						
			}//end if	
			else
			{
			if (window.event.keyCode!=9)
			{
			//9=tabulator, dann überhaupt keine anzeige
			
			//vorläufige suche
			if (aktuellerDatenbestand > waitAnzeigeLvl || initalDatenbestand > 800)
			{
		//	txtAnzeigeInfo.value="Vorläufige Suche";
				anzeige=true;
			}//end if	
			}//end if
			}//end else
	 }//end if
	 else
	 {
		//erste suche es sei denn tabulator keycoe=9
		if (initalDatenbestand > waitAnzeigeLvl && window.event.keyCode!=9)
		{
		//txtAnzeigeInfo.value="ersteSuche";
			anzeige=true;
		}//end if
	 }//end else
	}//end try
	catch(ex)
	{
		alert("fehler in showWaitInfo");
		anzeige=true;
	}
	if (anzeige)
	{
		//wartelabel anzeigen
		lblwait.style.display ="";
		var tempParent= obj.parentNode;
       
		if(tempParent)
		{
		//unter "sender objekt" process info anzeigen 
		tempParent.appendChild(absatz);
			tempParent.appendChild(lblprocessing);
		}//end if
	}//end if
	}//end if obj
	}//end if direkt input
	else//wenn keine clientselection aktiv ist, dann auf enter taste reagieren
	{
		//eine form wird mit drücken des enterbuttons automatisch verschickt, 
		//es wird aber nicht der button des Händlersuche Controls ausgeführt, sondern der (oder ein) button des
		//parent-reports, es soll nun so sein das wenn der benutzer in einer selektionstextbox
		//des HändlerSuche Controls die enter taste drückt und noch keine Clientselektino vorliegt,
		//also noch keine Direkteingabe, die SAP-seitige suche durch den Button "lb_SucheHaendler" 
		//angestoßen wird. 
		//dafür cancel ich alle SubmitEvents der form und den führe postback des "lb_SucheHaendler" Button
		//manuell aus
		//keyCode 13=return taste
		//JJU2008.04.07
		
		if (window.event.keyCode==13)
		{
		//alert("werde ein Postback machen! und alle anderen submit-events abbrechen");
		window.event.returnValue=false;
		event.cancel = true;
		__doPostBack('SucheHaendler1$lb_SucheHaendler','')//von asp.net generierte Postback Funktion;
		}
	}
}//end function 

function checkUserInput(array,obj)
{
	//JJU2008.3.18
	//je nach eingabe des benutzers wird hier das ergebniss der LB zusammengestellt
	//-------------------------------------------------------------------------
	
	//alert("name des objektes=" +obj.id);
	//alert("hab ich das array?länge= " +array.length);
			
	var objRegAusdruck =new RegExp(obj.value+ "{1}", "i");
	//alert("der Reguläre vergleichsausdruck = " + stringRegAusdruck);
	
	//alert("count des der eingabe= " +eingabe.length);
	
	var eingabeLaenge=obj.value.length;
	for (var i=0; i<array.length; i++)
	{
		//alert("das ist die eingabe= " +obj.value);
		//alert("wert zum vergleich= " +array[i].substring(0,obj.value.length))
		if (objRegAusdruck.test(array[i].substring(0,eingabeLaenge)))
		{			
			//alert("FOUND"+ i + ", " + array[i]);
		}
		else
		{
			//alert("NOT FOUND "+ i + ", " + array[i]);
			var y;
			for (y=0; y<aLBaktuelleOptions.length;y++)
			{
				//if (parseInt(jSON_HaendlerNR[i])=parseInt(aLBaktuelleOptions[y].value))
				if (jSON_HaendlerNR[i]==aLBaktuelleOptions[y].value)
				{
					//alert("es wird ein element aus OptionArray entfernt");
					//alert("HändlerNr aus JSON= " +jSON_HaendlerNR[i] + " Händler nummer aus LB-Option= " +aLBaktuelleOptions[y].value);
					//element aus array entfernen
					aLBaktuelleOptions.splice(y,1);
					break;
					//alert("Länge des aktuelleOptionArrays nach entfernung= "+aLBaktuelleOptions.length);
				}//end if
				
			}//end for 	
			
		}//end else
	}//end for 
}//end function 

function lbSelectionChanged()
{
	//JJU2008.3.18
	//wenn dient zur anzeige der händler details wenn eine selektion in der LB vorhanden	
	//-------------------------------------------------------------------------
	//alert("Selected Index LB = "+haendlerLB.selectedIndex +  " länge des JSON Arrays=" +jSON_HaendlerNR.length );
 
	if (haendlerLB.selectedIndex!=-1)//keine selektion
	{
	for(var i=0; i< jSON_HaendlerNR.length; i++)
	{
		//alert("vergleiche json " +jSON_HaendlerNR[i]+ " lb wert= "+haendlerLB.value+" index="+i);
		if (jSON_HaendlerNR[i]==haendlerLB.value)
		{
			//alert("option gefunden; wert= "+haendlerLB.value+" index="+i);
			lblHaendlerDetailsNR.innerHTML=jSON_HaendlerNR[i];
			lblHaendlerDetailsName1.innerHTML=jSON_Name1[i];
			lblHaendlerDetailsName2.innerHTML=jSON_Name2[i];
			lblHaendlerDetailsPLZ.innerHTML=jSON_PLZ[i];
			lblHaendlerDetailsOrt.innerHTML=jSON_Ort[i];
			lblHaendlerDetailsStrasse.innerHTML=jSON_Strasse[i];
			break;	
		}//end if
	}//end for
	}//end if
	else
	{
	lblHaendlerDetailsNR.innerHTML="no selection";
	lblHaendlerDetailsName1.innerHTML="";
	lblHaendlerDetailsName2.innerHTML="";
	lblHaendlerDetailsPLZ.innerHTML="";
	lblHaendlerDetailsOrt.innerHTML="";
	lblHaendlerDetailsStrasse.innerHTML="";	
	}
	//-------------------------------------------------------------------------
}//end function 

window.onload=checkInputs;

-->
</script>

<table id="Table3" cellspacing="0" cellpadding="1" width="100%" bgcolor="white">
    <tr id="trHaendlernummer" runat="server" width="100%">
        <td class="TextLarge" height="29" width="150">
            <asp:Label ID="lbl_HaendlerNummer" runat="server"></asp:Label>
        </td>
        <td class="TextLarge" width="350" height="29">
            <asp:TextBox ID="txtNummer" runat="server" MaxLength="10" Width="200px"></asp:TextBox>&nbsp;<asp:Label
                ID="lblSHistoryNR" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
        </td>
        <td class="TextLarge" height="29">
            &nbsp;
        </td>
        <td class="TextLarge" height="29">
            &nbsp;
        </td>
    </tr>
    <tr id="tr_Name1" runat="server">
        <td class="StandardTableAlternate">
            <asp:Label runat="server" ID="lbl_Name1">Name1:</asp:Label>
        </td>
        <td class="StandardTableAlternate">
            <asp:TextBox ID="txtName1" runat="server" MaxLength="35" Width="200px"></asp:TextBox>&nbsp;<asp:Label
                ID="lblSHistoryName1" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
        </td>
        <td class="StandardTableAlternate">
           </td>
        <td class="StandardTableAlternate">
            &nbsp;
        </td>
    </tr>
    <tr id="Tr_Name2" runat="server">
        <td class="TextLarge">
            Name2:
        </td>
        <td class="TextLarge">
            <asp:TextBox ID="txtName2" runat="server" Enabled="False" BackColor=" #c1ccd9" MaxLength="35"
                Width="200px"></asp:TextBox>&nbsp;<asp:Label ID="lblSHistoryName2" runat="server"
                    Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
        </td>
        <td class="TextLarge">
        </td>
        <td class="TextLarge">
            &nbsp;
        </td>
    </tr>
    <tr id="tr_PLz" runat="server">
        <td class="StandardTableAlternate">
            PLZ:
        </td>
        <td class="StandardTableAlternate">
            <asp:TextBox ID="txtPLZ" runat="server" MaxLength="35" Width="200px"></asp:TextBox>&nbsp;<asp:Label
                ID="lblSHistoryPLZ" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
        </td>
        <td class="StandardTableAlternate">
        </td>
        <td class="StandardTableAlternate">
            &nbsp;
        </td>
    </tr>
    <tr id="Tr_Ort" runat="server">
        <td class="TextLarge">
            Ort:
        </td>
        <td class="TextLarge">
            <asp:TextBox ID="txtOrt" runat="server" MaxLength="35" Width="200px"></asp:TextBox>&nbsp;<asp:Label
                ID="lblSHistoryOrt" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
        </td>
        <td class="TextLarge">
        </td>
        <td class="TextLarge">
            &nbsp;
        </td>
    </tr>
    <tr id="Tr_SelectionButton" runat="server">
        <td class="TextLarge">
            <asp:Label ID="lbldirektInput" Style="display: none" runat="server" Width="40" ForeColor="green">
				<u>Direkteingabe</u>
            </asp:Label><br>
            Anzahl Treffer:
            <asp:Label ID="lblErgebnissAnzahl" runat="server" Width="40"></asp:Label><br>
            <asp:Label ID="lblwait" Style="display: none" runat="server" ForeColor="red" Font-Bold="True">bitte warten</asp:Label>
        </td>
        <td>
            <p>
                &nbsp;</p>
        </td>
        <td class="TextLarge">
        </td>
        <td class="TextLarge">
            &nbsp;
        </td>
    </tr>
    <tr id="tr_HaendlerAuswahl" runat="server" visible="false">
        <td class="TextLarge" colspan="2">
            <asp:ListBox ID="lbHaendler" runat="server" Width="500px" Height="150px"></asp:ListBox>
        </td>
        <td align="left">
            <p align="left" colspan="2">
                <b>&nbsp;&nbsp;
                    <asp:Label ID="lblHaendlerDetailsNR" runat="server" Font-Size="13"></asp:Label></b><br>
                &nbsp;&nbsp;
                <asp:Label ID="lblHaendlerDetailsName1" runat="server" Font-Size="12"></asp:Label><br>
                &nbsp;&nbsp;
                <asp:Label ID="lblHaendlerDetailsName2" runat="server" Font-Size="12"></asp:Label><br>
                &nbsp;&nbsp;
                <asp:Label ID="lblHaendlerDetailsStrasse" runat="server" Font-Size="12"></asp:Label><br>
                <br>
                <b>&nbsp;&nbsp;<asp:Label ID="lblHaendlerDetailsPLZ" runat="server" Font-Size="12"></asp:Label>
                    <br>
                    &nbsp;&nbsp;<asp:Label ID="lblHaendlerDetailsOrt" runat="server" Font-Size="12"></asp:Label>
                </b>
            </p>
        </td>
        <td align="left">
            &nbsp;
        </td>
    </tr>
    <tr id="tr_Message" runat="server">
        <td class="TextLarge" colspan="3">
            <p>
                <asp:Label ID="lbl_Message" runat="server" ForeColor="Black" Font-Bold="True"></asp:Label><br>
                <asp:Label ID="lbl_Info" runat="server">einfache / mehrfache Platzhaltersuche möglich z.B. 'PLZ= 9*', 'Name1=*Musterma*' </asp:Label><br>
                <asp:Label ID="lbl_error" runat="server" CssClass="TextError"></asp:Label></p>
            <p>
                &nbsp;&nbsp;
                <asp:TextBox ID="txtAnzeigeInfo" runat="server" Width="450px" Visible="False"></asp:TextBox></p>
        </td>
        <td class="TextLarge">
            &nbsp;
        </td>
    </tr>
</table>
