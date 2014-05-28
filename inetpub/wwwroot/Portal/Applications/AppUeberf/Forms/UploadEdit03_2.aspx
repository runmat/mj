<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploadEdit03_2.aspx.vb" Inherits="AppUeberf.UploadEdit03_2" %>
<%@ Register TagPrefix="uc1" TagName="AddressSearchInputControl" Src="../Controls/AddressSearchInputControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
	    <script language="javascript" src="../Controls/CustomDialog.js"></script>
		
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
    <style type="text/css">
    div.transbox
 {
	width: 400px;
    height: 180px;
    background-color: #ffffff;
    filter:alpha(opacity=60);
    opacity:0.6;
 }
    </style>

    <script type="text/javascript">

     function ShowMessage() {
         SetText('Ja', 'Nein');

         DisplayConfirmMessage('Eventuelle Änderungen speichern?', 225, 75)

         SetDefaultButton('btnConfOK');

         var element = document.getElementById('FormDiv');
         element.className = 'transbox'
         return false;

     }

</script>
        
	</HEAD>
	<body leftMargin="0" topMargin="0"  >

		<FORM id="Form1" method="post" runat="server">
        <div id="divConfMessage" runat="server" style="background-image: url(/Portal/Images/divBody.jpg);
            display: none; z-index: 200;">
            <div style="background-image: url(/Portal/Images/headbox.jpg); text-align: center"
                id="confirmText">
            </div>
            <div style="z-index: 105; height: 30%; background-image: url(/Portal/Images/divBody.jpg);
                text-align: center">
            </div>
            <div style="z-index: 105; height: 70%; background-image: url(/Portal/Images/divBody.jpg);
                text-align: center">
                <asp:Button ID="btnConfOK" Width="75px" runat="server" Text="OK"></asp:Button>
                <asp:Button ID="btnConfCancel" Width="75px" runat="server" Text="Cancel"></asp:Button>
            </div>
        </div>
        <div id="FormDiv">
            <table width="100%" align="center">
                <tr>
                    <td>
                        <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left" width="100%" colspan="3">
                        <table id="Table10" cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td class="PageNavigation" colspan="2">
                                    <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;(
                                    <asp:Label ID="lblPageTitle" runat="server">Anschlussfahrt</asp:Label>)
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 103px" valign="top" width="103">
                                    <table id="Table12" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                        border="0">
                                        <tr>
                                            <td width="150">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" width="150">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" width="150">
                                                <asp:ImageButton ID="cmdBack0" runat="server" Width="73px" Height="40px" ImageUrl="/Portal/Images/BackToMap.jpg"
                                                    ToolTip="Zurück zur Übersicht"></asp:ImageButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                <p align="right">
                                                    <strong>Schritt&nbsp;4 von 5</strong></p>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                &nbsp;
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                <asp:Label ID="lblFahrzeugdaten" runat="server" Width="186px" Font-Bold="True">Rücklieferung 2. Fahrzeug</asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405" colspan="2">
                                                <uc1:AddressSearchInputControl ID="ctrlAddressSearchRueckliefer" runat="server">
                                                </uc1:AddressSearchInputControl>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                <asp:Label ID="Label17" runat="server" Width="110px">Auswahl:</asp:Label><asp:DropDownList
                                                    ID="drpRetour" runat="server" Width="216px" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                <asp:Label ID="Label6" runat="server" Width="110px">Firma / Name*</asp:Label><asp:TextBox
                                                    ID="txtAbName" runat="server" Width="220px" Wrap="False"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405" height="21">
                                                <asp:Label ID="Label7" runat="server" Width="110px">Strasse 
                                                / Nr.*</asp:Label><asp:TextBox ID="txtAbStrasse" runat="server" Width="220px" Wrap="False"></asp:TextBox>
                                            </td>
                                            <td height="21">
                                                <asp:Label ID="Label13" runat="server" Width="50px" Visible="False">Nr.*</asp:Label>
                                                <asp:TextBox ID="txtAbNr" runat="server" Width="73px" Wrap="False" Visible="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                <asp:Label ID="Label8" runat="server" Width="110px">PLZ*</asp:Label><asp:TextBox
                                                    ID="txtAbPLZ" runat="server" Width="102px" Wrap="False" MaxLength="5"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label15" runat="server" Width="50px">Ort*</asp:Label><asp:TextBox
                                                    ID="txtAbOrt" runat="server" Width="299px" Wrap="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                <asp:Label ID="Label9" runat="server" Width="110px">Ansprechpartner*</asp:Label><asp:TextBox
                                                    ID="txtAbAnsprechpartner" runat="server" Width="223px" Wrap="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label16" runat="server" Width="50px">1. Tel.:*</asp:Label><asp:TextBox
                                                    ID="txtAbTelefon" runat="server" Width="299px" Wrap="False" MaxLength="16"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                <asp:Label ID="Label18" runat="server" Width="110px">Fax</asp:Label><asp:TextBox
                                                    ID="txtAbFax" runat="server" Width="223px" Wrap="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label19" runat="server" Width="50px">2. Tel.:</asp:Label><asp:TextBox
                                                    ID="txtAbTelefon2" runat="server" Width="299px" Wrap="False" MaxLength="16"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                &nbsp;
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                <asp:Label ID="Label5" runat="server" Width="293px" Font-Bold="True">Fahrzeugdaten 2. Fahrzeug</asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                <asp:Label ID="Label1" runat="server" Width="110px">Hersteller / Typ*</asp:Label><asp:TextBox
                                                    ID="txtHerstTyp" runat="server" Width="200px" Wrap="False" MaxLength="25"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label11" runat="server" Width="286px" Font-Bold="True">Fahrzeug zugelassen und betriebsbereit?*</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                <asp:Label ID="Label2" runat="server" Width="110px">Kennzeichen*</asp:Label><asp:TextBox
                                                    ID="txtKennzeichen1" runat="server" Width="39px" Wrap="False"></asp:TextBox><asp:Label
                                                        ID="Label10" runat="server" Width="11px" Font-Bold="True"> -</asp:Label><asp:TextBox
                                                            ID="txtKennzeichen2" runat="server" Width="102px" Wrap="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rdbZugelassen" runat="server" Width="109px" Height="19px"
                                                    RepeatDirection="Horizontal" TextAlign="Left">
                                                    <asp:ListItem>Ja</asp:ListItem>
                                                    <asp:ListItem>Nein</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr id="TRllbAnZulKCL" runat="server" visible="False">
                                            <td style="width: 405px; height: 25px" width="405">
                                            </td>
                                            <td style="height: 25px">
                                                <asp:Label ID="Label21" runat="server" Font-Bold="True" Width="286px" Visible="False">Zulassung durch KCL?*</asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="TRrdbAnZulKCL" runat="server" visible="False">
                                            <td style="width: 405px; height: 25px" width="405">
                                            </td>
                                            <td style="height: 25px">
                                                <asp:RadioButtonList ID="rdbAnZulKCL" runat="server" Width="109px" TextAlign="Left"
                                                    RepeatDirection="Horizontal" Height="19px" Visible="False">
                                                    <asp:ListItem Value="Ja">Ja</asp:ListItem>
                                                    <asp:ListItem Value="Nein">Nein</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px; height: 25px" width="405">
                                            </td>
                                            <td style="height: 25px">
                                                <asp:Label ID="Label20" runat="server" Width="192px" Font-Bold="True" Height="19px">Fahrzeugklasse in Tonnen*</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                <asp:Label ID="Label3" runat="server" Width="110px">Fgst.-Nummer</asp:Label><asp:TextBox
                                                    ID="txtVin" runat="server" Width="200px" Wrap="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rdbFahrzeugklasse" runat="server" Width="196px" Height="22px"
                                                    RepeatDirection="Horizontal" TextAlign="Left">
                                                    <asp:ListItem Value="P">&lt; 3,5</asp:ListItem>
                                                    <asp:ListItem Value="G">3,5 - 7,5</asp:ListItem>
                                                    <asp:ListItem Value="L">&gt; 7,5</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                                <asp:Label ID="Label4" runat="server" Width="110px">Referenz-Nr</asp:Label><asp:TextBox
                                                    ID="txtRef" runat="server" Width="200px" Wrap="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label12" runat="server" Width="251px" Font-Bold="True">Bereifung*</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 405px" width="405">
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rdbBereifung" runat="server" Width="137px" Height="19px"
                                                    RepeatDirection="Horizontal" TextAlign="Left">
                                                    <asp:ListItem Value="Sommer">Sommer</asp:ListItem>
                                                    <asp:ListItem Value="Winter">Winter</asp:ListItem>
                                                    <asp:ListItem Value="Ganzjahresreifen">Ganzjahresreifen</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 103px" valign="top" width="103">
                                </td>
                                <td valign="top">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 103px" valign="top" width="103">
                                </td>
                                <td valign="top">
                                    <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                        <tr>
                                            <td style="width: 330px">
                                                <p align="right">
                                                    <asp:ImageButton ID="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfLeft.gif">
                                                    </asp:ImageButton></p>
                                            </td>
                                            <td style="width: 74px">
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="cmdRight1" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfRight.gif">
                                                </asp:ImageButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 330px">
                                            </td>
                                            <td style="width: 74px">
                                                <asp:Label ID="Label14" runat="server" Width="80px" Font-Bold="True" ForeColor="Red">*=Pflichtfeld</asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 330px">
                                            </td>
                                            <td style="width: 74px">
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Label ID="lblError" runat="server" Width="289px" EnableViewState="False" CssClass="TextError"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 103px" valign="top" width="103">
                                </td>
                                <td valign="top">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
		</form>
	</body>
</HTML>
