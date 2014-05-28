<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploadEdit02.aspx.vb"
    Inherits="AppUeberf.UploadEdit02" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AddressSearchInputControl" Src="../Controls/AddressSearchInputControl.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Ueberfg_01</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

    <script language="javascript" src="../Controls/CustomDialog.js"></script>

    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        div.transbox
        {
            width: 400px;
            height: 180px;
            background-color: #ffffff;
            filter: alpha(opacity=60);
            opacity: 0.6;
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

</head>
<body leftmargin="0" topmargin="0">
    <form id="Form1" method="post" runat="server">
    <div id="divConfMessage" style="background-image: url(/Portal/Images/divBody.jpg);
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
    <div id="FormDiv" runat="server">
        <table id="Table4" width="100%" align="center">
            <tr>
                <td>
                    <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
                </td>
            </tr>
            <tr>
                <td class="PageNavigation">
                    <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;(
                    <asp:Label ID="lblPageTitle" runat="server">Adressdaten</asp:Label>)
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table5" style="width: 1032px; height: 27px" cellspacing="1" cellpadding="1"
                        width="1032" border="0">
                        <tr>
                            <td style="width: 431px">
                            </td>
                            <td style="width: 360px">
                                <strong>Schritt&nbsp;2 von 4</strong>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 113px">
                                <asp:ImageButton ID="cmdBack0" runat="server" Width="73px" Height="40px" ImageUrl="/Portal/Images/BackToMap.jpg"
                                    ToolTip="Zurück zur Übersicht"></asp:ImageButton>
                            </td>
                            <td>
                                <strong>
                                    <asp:Label ID="Label1" runat="server" Width="122px" Height="17px">Abholung</asp:Label></strong>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 113px" valign="top">
                                &nbsp;
                            </td>
                            <td>
                                    <strong>
                                        <uc1:AddressSearchInputControl ID="ctrlAddressSearchAbholung" runat="server"></uc1:AddressSearchInputControl>
                                    </strong>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                        <tr>
                            <td width="121" style="width: 121px">
                            </td>
                            <td width="120">
                                Auswahl
                            </td>
                            <td style="width: 255px">
                                <asp:DropDownList ID="drpAbholung" runat="server" Width="217px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td width="121" style="width: 121px">
                            </td>
                            <td width="120">
                                Firma / Name*
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAbName" runat="server" Width="220px"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 108px">
                                Strasse*
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAbStrasse" runat="server" Width="218px"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtAbNr" runat="server" Width="73px" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 108px">
                                PLZ*
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAbPLZ" runat="server" Width="88px" MaxLength="5"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                Ort*
                            </td>
                            <td>
                                <asp:TextBox ID="txtAbOrt" runat="server" Width="299px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 108px">
                                Ansprechpartner*
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAbAnsprechpartner" runat="server" Width="223px"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                1. Tel.:*
                            </td>
                            <td>
                                <asp:TextBox ID="txtAbTelefon" runat="server" Width="299px" MaxLength="16"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 108px">
                                &nbsp;Fax
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAbFax" runat="server" Width="223px" MaxLength="16"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                2. Tel.:
                            </td>
                            <td>
                                <asp:TextBox ID="txtAbTelefon2" runat="server" Width="299px" MaxLength="16"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 108px">
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
                            <td style="width: 108px">
                                <strong>Anlieferung</strong>
                            </td>
                            <td style="width: 255px">
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td width="121" style="width: 121px">
                            </td>
                            <td colspan="3">
                                <div>
                                    <uc1:AddressSearchInputControl ID="ctrlAddressSearchAnlieferung" runat="server">
                                    </uc1:AddressSearchInputControl>
                                </div>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 108px">
                                Auswahl
                            </td>
                            <td style="width: 255px">
                                <asp:DropDownList ID="drpAnlieferung" runat="server" Width="220px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 108px">
                                Firma / Name*
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAnName" runat="server" Width="219px"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 108px">
                                Strasse*
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAnStrasse" runat="server" Width="214px"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtAnNr" runat="server" Width="73px" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 108px">
                                PLZ*
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAnPLZ" runat="server" Width="88px" MaxLength="5"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                Ort*
                            </td>
                            <td>
                                <asp:TextBox ID="txtAnOrt" runat="server" Width="299px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 108px">
                                Ansprechpartner*
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAnAnsprechpartner" runat="server" Width="220px"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                1. Tel.:*
                            </td>
                            <td>
                                <asp:TextBox ID="txtAnTelefon" runat="server" Width="299px" MaxLength="16"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 108px">
                                &nbsp;Fax
                            </td>
                            <td style="width: 255px">
                                <asp:TextBox ID="txtAnFax" runat="server" Width="220px" MaxLength="16"></asp:TextBox>
                            </td>
                            <td style="width: 68px">
                                2. Tel.:
                            </td>
                            <td>
                                <asp:TextBox ID="txtAnTelefon2" runat="server" Width="299px" MaxLength="16"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 108px">
                                &nbsp;
                            </td>
                            <td style="width: 255px">
                            </td>
                            <td style="width: 80px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 121px">
                            </td>
                            <td style="width: 108px">
                            </td>
                            <td style="width: 255px">
                                <p align="right">
                                    <asp:ImageButton ID="cmdBack" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfLeft.gif">
                                    </asp:ImageButton></p>
                            </td>
                            <td style="width: 68px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:ImageButton ID="cmdRight1" runat="server" Width="73px" Height="34px" ImageUrl="/Portal/Images/arrowUeberfRight.gif">
                                </asp:ImageButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Width="80px" Font-Bold="True" ForeColor="DarkRed">*=Pflichtfeld</asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                        <tr>
                            <td style="width: 119px" align="center">
                            </td>
                            <td align="center">
                                <p align="left">
                                    <asp:Label ID="lblError" runat="server" Width="325px" Height="19px" EnableViewState="False"
                                        CssClass="TextError"></asp:Label></p>
                            </td>
                        </tr>
                    </table>
                    <p>
                        &nbsp;</p>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" id="ScrollPosition">

        function sstchur_SmartScroller_GetCoords() {



            var scrollX, scrollY;



            if (document.all) {

                if (!document.documentElement.scrollLeft)

                    scrollX = document.body.scrollLeft;

                else

                    scrollX = document.documentElement.scrollLeft;



                if (!document.documentElement.scrollTop)

                    scrollY = document.body.scrollTop;

                else

                    scrollY = document.documentElement.scrollTop;

            }

            else {

                scrollX = window.pageXOffset;

                scrollY = window.pageYOffset;

            }

            document.forms["Form1"].xCoordHolder.value = scrollX;

            document.forms["Form1"].yCoordHolder.value = scrollY;

        }
        function sstchur_SmartScroller_Scroll() {
            var x = document.forms["Form1"].xCoordHolder.value;

            var y = document.forms["Form1"].yCoordHolder.value;

            window.scrollTo(x, y);

        }
        window.onload = sstchur_SmartScroller_Scroll;

        window.onscroll = sstchur_SmartScroller_GetCoords;

        window.onkeypress = sstchur_SmartScroller_GetCoords;

        window.onclick = sstchur_SmartScroller_GetCoords;

    </script>

    <input type="hidden" id="xCoordHolder" runat="server" name="xCoordHolder"><input
        type="hidden" id="yCoordHolder" runat="server" name="yCoordHolder"></form>
</body>
</html>
