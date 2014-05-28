<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_1.aspx.vb" Inherits="AppAvis.Change02_1" %>
<%@ Register TagPrefix="DBWC" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.HierarGrid" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">

    <script language="JavaScript" type="text/javascript" src="../Javascript/Slideup_Down.js"></script>

    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        div.transbox
        {
            width: 100%;
            height: 100%;
            background-color: #ffffff;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .style8
        {
            width: 176px;
        }
        .styleInput
        {
            border-style: none;
            border-color: inherit;
            border-width: medium;
            width: 25px;
            height: 19px;
            text-align: center;
            bottom: 0px;
            background-color: #FFFFCC;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" name="Form1" method="post" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
<%--    <input type="hidden" value="" name="Zuldat" />--%>
    <input type="hidden" value="empty" name="PDINummer" />
    <input type="hidden" value="empty" name="NummerInGrid" />
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Auswahl von Modellen bzw. Fahrzeugen)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdWeiter" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdAbsenden" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Absenden</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdVerwerfen" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Verwerfen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdWeitereAuswahl" runat="server" CssClass="StandardButton" Visible="False">•&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="cmdZurueck" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Zurück</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:HyperLink ID="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change02.aspx">Carport - Suche</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td class="BorderLeftBottom" width="100px">
                                                                <asp:Label ID="Label1" runat="server" Text="Zulassungsdatum:"></asp:Label>
                                                            </td>
                                                            <td class="BorderLeftBottom" align="center" width="10px">
                                                                &nbsp;</td>
                                                            <td width="75px" class="BorderLeftBottom" valign="middle">
                                                                <asp:Label ID="lblZulDat" runat="server"  EnableViewState="False" Width="75px"></asp:Label>
                                                                <input type="hidden"  id="HiddenZuldat" value="" runat="server"  />
                                                            </td>
                                                            <td width="38px" class="BorderLeftBottom" valign="baseline">
                                                                <input type="button" id="Cal1" onclick="DisplayCalender()" style="border: none; background-image: url(../../../Images/calendar_red.jpg);
                                                                    width: 16px; height: 16px; cursor: hand; position: static;" runat="server" />&nbsp;
  
                                                                <asp:ImageButton ID="ibtnDelZulDat" runat="server" 
                                                                    ImageUrl="../../../Images/loesch.gif" Height="13px" Width="13px" 
                                                                    ToolTip="Datum entfernen!" ImageAlign="AbsMiddle" />
                                                            </td>
                                                            <td class="BorderLeftBottom" width="120px">
                                                                <asp:Label ID="Label2" runat="server" Text="Zulassungen ges."></asp:Label>
                                                            </td>
                                                            <td class="BorderLeftBottom" width="25px" align="center">
                                                                <input type="text" class="styleInput" name="ZulassungGesamt" runat="server" id="GesamtAnzahl"
                                                                    readonly="readonly" />
                                                            </td>
                                                            <td align="center" class="style8">
                                                            </td>
                                                            <td width="100px">
                                                            </td>
                                                            <td width="100px" />
                                                            <td width="15px" align="right" />
                                                        </tr>
                                                        <tr>
                                                            <td width="100px">
                                                                &nbsp;
                                                            </td>
                                                            <td width="10px">
                                                            </td>
                                                            <td width="75px">
                                                                <div id="DivCalendar"  style="position:absolute; display:none">
                                                                    <asp:Calendar ID="calVon" runat="server" BackColor="White" BorderColor="#999999"
                                                                        CellPadding="1" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black">
                                                                        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                                                        <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                                        <SelectorStyle BackColor="#CCCCCC" />
                                                                        <WeekendDayStyle BackColor="#FFFFCC" />
                                                                        <OtherMonthDayStyle ForeColor="#808080" />
                                                                        <NextPrevStyle VerticalAlign="Bottom" />
                                                                        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                                                        <TitleStyle BackColor="#999999" Font-Size="7pt" BorderColor="Black" Font-Bold="True" />
                                                                    </asp:Calendar>
                                                                </div>
                                                            </td>
                                                            <td width="38px">
                                                                &nbsp;</td>
                                                            <td class="BorderLeftBottom" width="120px">
                                                                <asp:Label ID="Label3" runat="server" Text="aktuelle Auswahl:"></asp:Label>
                                                            </td>
                                                            <td class="BorderLeftBottom" width="25px" align="center">
                                                                <input type="text" class="styleInput" name="Aktuell" runat="server" id="AktuelleAnzahl"
                                                                    readonly="readonly" />
                                                            </td>
                                                            <td align="center" class="style8">
                                                            </td>
                                                            <td width="100px">
                                                                &nbsp;
                                                            </td>
                                                            <td width="100px" />
                                                            <td width="15px" align="right" />
                                                        </tr>
                                                        <tr>
                                                            <td width="100px">
                                                                &nbsp;
                                                            </td>
                                                            <td width="10px">
                                                            </td>
                                                            <td width="75px">
                                                                &nbsp;
                                                            </td>
                                                            <td width="38px">
                                                                &nbsp;</td>
                                                            <td class="BorderLeftBottom" width="120px">
                                                                <asp:Label ID="Label4" runat="server" Text="aktuell Gesamt:"></asp:Label>
                                                                
                                                            </td>
                                                            <td class="BorderLeftBottom" width="25px" align="center">
                                                                <input type="text" class="styleInput" name="AktuellGesamt" readonly="readonly" runat="server"
                                                                    id="AktuelleSumme" />
                                                            </td>
                                                            <td align="center" class="style8">
                                                                &nbsp;
                                                            </td>
                                                            <td width="100px">
                                                                &nbsp;
                                                            </td>
                                                            <td width="100px" />
                                                            &nbsp;<td width="15px" align="right" />
                                                        </tr>
                                                    </table>
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr id="rowExcelLink" runat="server">
                                    <td class="TextLarge" valign="center">
                                        <asp:ImageButton ID="lnkCreateExcel" ImageUrl="~/Images/excel.gif" runat="server"
                                            Height="16px" Width="16px" ImageAlign="Top" Visible="False"></asp:ImageButton>
                                        &nbsp;<asp:LinkButton ID="lnkCreateExcel2" class="TextLarge" Visible="False" runat="server">Excelformat</asp:LinkButton>
                                    </td>
                                </tr>
                                 <tr id="rowSelectionExcel" runat="server">
                                    <td class="TextLarge" valign="center">
                                        <asp:ImageButton ID="imgbExcel" ImageUrl="~/Images/excel.gif" runat="server"
                                            Height="16px" Width="16px" ImageAlign="Top" Visible="true"></asp:ImageButton>
                                        &nbsp;<asp:LinkButton ID="lnkExcel" class="TextLarge" Visible="true" runat="server">Excelformat</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        <DBWC:HierarGrid ID="HG1" Style="z-index: 101" runat="server" TemplateDataMode="Table"
                                            LoadControlMode="UserControl" TemplateCachingBase="Tablename" CellPadding="0"
                                            BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#999999"
                                            Width="100%" AutoGenerateColumns="False">
                                            <AlternatingItemStyle CssClass="GridTableAlternate" />
                                            <ItemStyle CssClass="GridTableItem" />
                                            <HeaderStyle CssClass="GridTableHead" />
                                            <Columns>
                                                <asp:TemplateColumn Visible="False" SortExpression="Details" HeaderText="Details">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDetails" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Details") %>'>
                                                        </asp:CheckBox>
                                                        <asp:CheckBox ID="chkLoaded" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Loaded") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                    <FooterStyle Wrap="False"></FooterStyle>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Carportnr" HeaderText="Carportnr." SortExpression="Carportnr">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Carport" HeaderText="Carport" SortExpression="Carport">
                                                    <HeaderStyle Width="70%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Zulassungsf_Fahrzeuge" HeaderText="Zulassungs-&lt;br&gt;fähige&lt;br&gt;Fahrzeuge"
                                                    SortExpression="Zulassungsf_Fahrzeuge">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Gesperrte_Fahrzeuge" HeaderText="Gesperrte&lt;br&gt;Fahrzeuge"
                                                    SortExpression="Gesperrte_Fahrzeuge">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundColumn>
                                            </Columns>
                                        </DBWC:HierarGrid>
                                      
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td class="LabelExtraLarge">
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <input id="SelOpen2" type="hidden" size="1" runat="server" />
    </table>


    <script language="JavaScript" type="text/javascript">
		<!--        //
        function DisableControls() {

            var j;
            for (var i = 0; i < window.document.Form1.length; ++i) {
                if (window.document.Form1.elements[i].type == "checkbox") {
                    document.getElementById(window.document.Form1.elements[i].name).disabled = true;
                }
                if (window.document.Form1.elements[i].type == "text") {
                    document.getElementById(window.document.Form1.elements[i].name).disabled = true;
                }
                if (window.document.Form1.elements[i].name == "ImageButton1") {
                    document.getElementById(window.document.Form1.elements[i].name).disabled = true;
                }
            }
        }

        function DetailsSuchen(PDI, Zeile, Task) {
            window.document.Form1.PDINummer.value = PDI;
            window.document.Form1.NummerInGrid.value = Zeile;
        }

        //        function SetValues(ModellID, Task) {
        //            var j;
        //            for (var i = 0; i < window.document.Form1.length; ++i)	//Haken suchen, der markiert wurde (Auswahl rechts). j = Spaltenindex.
        //            {
        //                if (window.document.Form1.elements[i].name == "Anzahl_alt_" + ModellID) {
        //                    j = i;
        //                    break;
        //                }
        //            }
        //            var k = j;
        //            var Bemerkung = "";
        //            var BemerkungDatum = ""; 										//Bemerkung Datum
        //            var DatumErstZulassung = "";
        //            var ZielPDI = "";
        //            var Anzahl_alt = Number(window.document.Form1.elements[j].value);
        //            var Anzahl_neu = Number(window.document.Form1.elements[j + 1].value);
        //            var count3;
        //            switch (Task) {
        //                case "Zulassen":
        //                    //Zuldatum aus Kalenderauswahl Carportebene oder
        //                    //Werte aus der Modell - Ebene holen...

        //                    if (window.document.Form1.Zuldat.value != '') {
        //                        window.document.Form1.elements[j - 1].value = window.document.Form1.Zuldat.value;
        //                        DatumErstZulassung = window.document.Form1.Zuldat.value;
        //                    } else {
        //                    DatumErstZulassung = window.document.Form1.elements[j - 1].value;
        //                    }
        //                    //window.document.Form1.elements[j - 1].value;-->DatumErstZulassung aus der Modellebene 
        //                    break;
        //                default:
        //                    Bemerkung = window.document.Form1.elements[j - 1].value;
        //                    break;
        //            }
        //            if (isNaN(Anzahl_neu) == true) {
        //                alert("Bitte geben Sie einen Zahlenwert für die Anzahl ein.");
        //            }
        //            else {
        //                if (Anzahl_alt < Anzahl_neu) {
        //                    alert("Der Zahlenwert für die Anzahl ist zu groß.");
        //                }
        //                else {
        //                    if (Anzahl_neu < 0) {
        //                        alert("Der Zahlenwert für die Anzahl ist zu klein.");
        //                    }
        //                    else {
        //                        j = 0;

        //                        for (i = k; i < window.document.Form1.length; ++i)			//Werte in die Fahrzeugebene übertragen...
        //                        {
        //                            if (window.document.Form1.elements[i].value == "Modell_ID_" + ModellID) {
        //                                if (j < Anzahl_alt) {
        //                                    if (j < Anzahl_neu) {
        //                                        if (window.document.Form1.elements[i - 4].value == "") {

        //                                            if (window.document.Form1.elements[i - 1].checked == false) {

        //                                                count3 = Number(document.getElementById("AktuelleAnzahl").value);
        //                                                count3 = count3 + 1;
        //                                                document.getElementById("AktuelleAnzahl").value = count3;
        //                                                count3 = Number(document.getElementById("AktuelleSumme").value);
        //                                                count3 = count3 + 1;
        //                                                document.getElementById("AktuelleSumme").value = count3;
        //                                            }
        //                                            window.document.Form1.elements[i - 1].checked = true;
        //                                            switch (Task) {
        //                                                case "Zulassen":
        //                                                    window.document.Form1.elements[i - 3].value = DatumErstZulassung;
        //                  //window.document.Form1.elements[i - 3].value;-->DatumErstZulassung aus der Fahrzeugebene
        //                                                    break;
        //                                                default:
        //                                                    window.document.Form1.elements[i - 3].value = Bemerkung;
        //                                                    break;
        //                                            }
        //                                        }
        //                                        else {
        //                                            j = j - 1;
        //                                        }
        //                                    }
        //                                    else {
        //                                        if (window.document.Form1.elements[i - 1].checked == true) {

        //                                            count3 = Number(document.getElementById("AktuelleAnzahl").value);
        //                                            count3 = count3 - 1;
        //                                            document.getElementById("AktuelleAnzahl").value = count3;
        //                                            count3 = Number(document.getElementById("AktuelleSumme").value);
        //                                            count3 = count3 - 1;
        //                                            document.getElementById("AktuelleSumme").value = count3;
        //                                        }
        //                                        window.document.Form1.elements[i - 1].checked = false;
        //                                        switch (Task) {
        //                                            case "Zulassen":
        //                                                window.document.Form1.elements[i - 3].value = "";
        //                                                window.document.Form1.elements[i - 3].disabled = false;
        //                                            default:
        //                                                window.document.Form1.elements[i - 3].value = "";
        //                                        }
        //                                    }
        //                                    j = j + 1;
        //                                }
        //                                else {
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        document.getElementById("Picture_" + ModellID).src = "/Portal/Images/Confirm_Mini.GIF"
        //                    }
        //                }
        //            }
        //        }



        function CountChecked(obj, ID) {
            var count;
            var count2;


            if (obj.checked) {
                count = Number(document.getElementById("AktuelleAnzahl").value);
                count = count + 1;
                document.getElementById("AktuelleAnzahl").value = count;
                count2 = Number(document.getElementById("AktuelleSumme").value);
                count2 = count2 + 1;
                document.getElementById("AktuelleSumme").value = count2;
                if (window.document.Form1.HiddenZuldat.value != '') {
                    ID.value = window.document.Form1.HiddenZuldat.value;
                }

            }
            else {
                count = Number(document.getElementById("AktuelleAnzahl").value);
                count = count - 1;
                document.getElementById("AktuelleAnzahl").value = count;
                count2 = Number(document.getElementById("AktuelleSumme").value);
                count2 = count2 - 1;
                document.getElementById("AktuelleSumme").value = count2;
                ID.value = '';
            }

        }
 
		//-->
    </script>

    <asp:Literal ID="litAddScript" runat="server"></asp:Literal>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </form>
</body>
</html>
