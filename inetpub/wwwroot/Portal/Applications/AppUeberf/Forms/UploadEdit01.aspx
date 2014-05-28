<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploadEdit01.aspx.vb" Inherits="AppUeberf.UploadEdit01" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Ueberf01</title>
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
<body leftmargin="0" topmargin="0">
    <form id="Form1" method="post" runat="server">
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
        <table id="Table4" width="100%" align="center">
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td class="PageNavigation" colspan="2">
                    <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;(
                    <asp:Label ID="lblPageTitle" runat="server">Adressdaten</asp:Label>)
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table id="Table5" style="width: 1032px; height: 27px" cellspacing="1" cellpadding="1"
                        width="1032" border="0">
                        <tr>
                            <td style="width: 431px">
                            </td>
                            <td style="width: 360px">
                                <strong>Schritt 1 von 4</strong>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                        <tr>
                            <td width="120">
                            </td>
                            <td style="width: 150px" width="150">
                                <strong>Rechnungszahler</strong>
                            </td>
                            <td style="width: 255px">
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td width="120">
                            </td>
                            <td style="width: 150px" width="150">
                                Auswahl
                            </td>
                            <td style="width: 255px">
                                <asp:DropDownList ID="drpRegulierer" runat="server" Width="217px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td width="120">
                            </td>
                            <td style="width: 150px" width="150">
                                Firma / Name
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAbName" runat="server" Width="220px" Enabled="False"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 150px">
                                Strasse
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAbStrasse" runat="server" Width="218px" Enabled="False"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtAbNr" runat="server" Width="73px" Enabled="False" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 150px">
                                PLZ
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAbPLZ" runat="server" Width="88px" Enabled="False" MaxLength="5"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                Ort
                            </td>
                            <td>
                                <asp:TextBox ID="txtAbOrt" runat="server" Width="299px" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 150px">
                                Ansprechpartner
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAbAnsprechpartner" runat="server" Width="223px" Enabled="False"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                Tel.:
                            </td>
                            <td>
                                <asp:TextBox ID="txtAbTelefon" runat="server" Width="299px" Enabled="False" MaxLength="16"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 150px">
                                &nbsp;
                            </td>
                            <td style="width: 255px">
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 150px">
                                <strong>Postalischer Rechnungsempfänger</strong>
                            </td>
                            <td style="width: 255px">
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 150px">
                                Auswahl
                            </td>
                            <td style="width: 255px">
                                <asp:DropDownList ID="drpRechnungsempf" runat="server" Width="220px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 150px">
                                Firma / Name
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAnName" runat="server" Width="219px" Enabled="False"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 150px">
                                Strasse
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAnStrasse" runat="server" Width="214px" Enabled="False"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtAnNr" runat="server" Width="73px" Enabled="False" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 150px">
                                PLZ
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAnPLZ" runat="server" Width="88px" Enabled="False" MaxLength="5"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                Ort
                            </td>
                            <td>
                                <asp:TextBox ID="txtAnOrt" runat="server" Width="299px" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 150px">
                                Ansprechpartner
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAnAnsprechpartner" runat="server" Width="220px" Enabled="False"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                Tel.:
                            </td>
                            <td>
                                <asp:TextBox ID="txtAnTelefon" runat="server" Width="299px" Enabled="False" MaxLength="16"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 150px">
                                &nbsp;
                            </td>
                            <td style="width: 255px">
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 150px">
                            </td>
                            <td style="width: 255px">
                                <p align="right">
                                    <asp:ImageButton ID="cmdBack" runat="server" Width="73px" ImageUrl="/Portal/Images/arrowUeberfLeft.gif"
                                        Height="34px"></asp:ImageButton></p>
                            </td>
                            <td style="width: 110px">
                            </td>
                            <td>
                                <asp:ImageButton ID="cmdRight1" runat="server" Width="73px" ImageUrl="/Portal/Images/arrowUeberfRight.gif"
                                    Height="34px"></asp:ImageButton>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 150px">
                                &nbsp;
                            </td>
                            <td style="width: 255px">
                            </td>
                            <td style="width: 110px" align="center">
                                &nbsp;
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 150px">
                            </td>
                            <td style="width: 255px">
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                                <strong><font color="red"></font></strong>
                            </td>
                        </tr>
                    </table>
                    <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                        <tr>
                            <td style="width: 119px" align="center">
                            </td>
                            <td align="center">
                                <p align="left">
                                    <asp:Label ID="lblError" runat="server" Width="325px" Height="19px" CssClass="TextError"
                                        EnableViewState="False"></asp:Label></p>
                            </td>
                        </tr>
                    </table>
                    <p>
                        &nbsp;</p>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</HTML>
