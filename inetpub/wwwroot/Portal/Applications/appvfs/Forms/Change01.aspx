<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01.aspx.vb" Inherits="AppVFS.Change01" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <script type="text/javascript">
        function showhide() {
            o = document.getElementById('trAdresse').style;

            if (o.display != 'none') {
                o.display = 'none';
            } else {
                o.display = 'block';
            }
        }
    
    </script>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>
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
                                    <td valign="middle" width="150">
                                        <asp:LinkButton ID="cmdContinue" runat="server" CssClass="StandardButton">Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" width="150">
                                        <asp:LinkButton ID="cmdConfirm" runat="server" CssClass="StandardButton" Visible="False">Absenden</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:LinkButton ID="lbExcel" Visible="False" runat="server">Excelformat</asp:LinkButton>&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td width="100%" align="Left">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td class="TextLarge" width="215" align="left" height="28">
                                                        <span lang="de">Auftraggeber</span>
                                                    </td>
                                                    <td class="TextLarge" width="340" height="28">
                                                        &nbsp;
                                                    </td>
                                                    <td class="TextLarge" align="right" width="133" height="28">
                                                    </td>
                                                    <td class="TextLarge" width="328" height="28">
                                                    </td>
                                                    <td class="TextLarge" width="328">
                                                        &nbsp;&nbsp;
                                                    </td>
                                                    <td class="TextLarge" width="328">
                                                        &nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TextLarge" width="215" align="left">
                                                        <asp:Label ID="lbl_Anrede" runat="server">lbl_Anrede</asp:Label>
                                                    </td>
                                                    <td class="TextLarge" width="340">
                                                        <asp:RadioButtonList RepeatDirection="Horizontal" ID="rblAnrede" runat="server">
                                                            <asp:ListItem Text="Firma" Value="Firma"></asp:ListItem>
                                                            <asp:ListItem Text="Herr" Value="Herr"></asp:ListItem>
                                                            <asp:ListItem Text="Frau" Value="Frau"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    <td class="TextLarge" width="39">
                                                    </td>
                                                    <td class="TextLarge" align="right" width="133">
                                                        &nbsp; &nbsp;&nbsp;
                                                    </td>
                                                    <td class="TextLarge" width="328">
                                                        &nbsp;&nbsp;
                                                    </td>
                                                    <td class="TextLarge">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="StandardTableAlternate" nowrap="nowrap" width="215" align="left">
                                                        <asp:Label ID="lbl_Vorname" runat="server">lbl_Vorname</asp:Label>
                                                        &nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td class="StandardTableAlternate" width="340">
                                                        <asp:TextBox ID="txtVorname" runat="server" MaxLength="40" Width="285px"></asp:TextBox>
                                                    </td>
                                                    <td class="StandardTableAlternate" colspan="4">
                                                        (z.B. Volksfürsorge, Agentur etc.&nbsp;oder Erika Mustermann)
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TextLarge" width="215" align="left">
                                                        <asp:Label ID="lbl_Name" runat="server">lbl_Name</asp:Label>
                                                        <span lang="de">&nbsp;</span>
                                                    </td>
                                                    <td class="TextLarge" width="340">
                                                        <asp:TextBox ID="txtName" runat="server" MaxLength="40" Width="285px"></asp:TextBox>
                                                    </td>
                                                    <td class="TextLarge" colspan="4">
                                                        (z.B. Erika Mustermann)
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="StandardTableAlternate" align="left" width="215" height="24">
                                                        <asp:Label ID="lbl_Strasse" runat="server">lbl_Strasse</asp:Label>&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td class="StandardTableAlternate" width="340" height="24">
                                                        <asp:TextBox ID="txtStrasse" runat="server" MaxLength="60" Width="285px"></asp:TextBox>
                                                    </td>
                                                    <td class="StandardTableAlternate" align="left" width="49" height="24">
                                                        <asp:Label ID="lbl_Hausnummer" runat="server">lbl_Hausnummer</asp:Label>
                                                    </td>
                                                    <td class="StandardTableAlternate" align="left" width="238" height="24">
                                                        <asp:TextBox ID="txtHausnummer" runat="server" MaxLength="10" Width="90px"></asp:TextBox>
                                                    </td>
                                                    <td class="StandardTableAlternate" width="92" height="24">
                                                    </td>
                                                    <td class="StandardTableAlternate" height="24">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TextLarge" width="215" align="left">
                                                        <asp:Label ID="lbl_Postleitzahl" runat="server">lbl_Postleitzahl</asp:Label>&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td class="TextLarge" width="340">
                                                        <asp:TextBox ID="txtPostleitzahl" runat="server" MaxLength="5" Width="45px"></asp:TextBox>
                                                    </td>
                                                    <td class="TextLarge" align="left" width="49">
                                                        <asp:Label ID="lbl_Ort" runat="server">lbl_Ort</asp:Label>
                                                    </td>
                                                    <td class="TextLarge" align="left" width="238">
                                                        <asp:TextBox ID="txtOrt" runat="server" MaxLength="40" Width="129px"></asp:TextBox>
                                                    </td>
                                                    <td class="TextLarge" width="92">
                                                        &nbsp;
                                                    </td>
                                                    <td class="TextLarge">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="StandardTableAlternate" width="215" align="left">
                                                        <asp:Label ID="lbl_Tel" runat="server">lbl_Tel</asp:Label>&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td class="StandardTableAlternate" width="340">
                                                        <asp:TextBox ID="txt_Tel" runat="server" Width="129px"></asp:TextBox>&nbsp;&nbsp;
                                                    </td>
                                                    <td class="StandardTableAlternate" colspan="4">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                        </table>
                                        <table cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td class="TextLarge" height="22px"  align="left" valign="middle" bgcolor="Silver" width="100%">
                                                    abweichende Versandadresse &nbsp;<asp:CheckBox ID="chkAdresse" runat="server" />
                                                </td>
                                                
                                            </tr>
                                            <tr id="trAdresse" style="display: none">
                                                <td class="TextLarge"  bgcolor="Silver">
                                                    <table>
                                                        <tr>
                                                            <td class="TextLarge" width="215" align="left" bgcolor="Silver">
                                                                <asp:Label ID="lbl_WEAnrede" runat="server">lbl_WEAnrede</asp:Label>
                                                            </td>
                                                            <td class="TextLarge" width="340" bgcolor="Silver">
                                                                <asp:RadioButtonList RepeatDirection="Horizontal" ID="rblWE_Anrede" runat="server">
                                                                    <asp:ListItem Text="Firma" Value="Firma"></asp:ListItem>
                                                                    <asp:ListItem Text="Herr" Value="Herr"></asp:ListItem>
                                                                    <asp:ListItem Text="Frau" Value="Frau"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                            <td class="TextLarge" width="39" bgcolor="Silver">
                                                            </td>
                                                            <td class="TextLarge" align="right" width="133" bgcolor="Silver">
                                                                &nbsp; &nbsp;&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge" nowrap="nowrap" width="215" align="left" bgcolor="Silver">
                                                                <asp:Label ID="lbl_WEVorname" runat="server">lbl_WEVorname</asp:Label>
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" width="340" bgcolor="Silver">
                                                                <asp:TextBox ID="txtWE_Vorname" runat="server" MaxLength="40" Width="285px"></asp:TextBox>
                                                            </td>
                                                            <td class="TextLarge" colspan="4" bgcolor="Silver">
                                                                (z.B. Volksfürsorge, Agentur etc.&nbsp;oder Erika Mustermann)
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge" width="215" align="left" bgcolor="Silver">
                                                                <asp:Label ID="lbl_WEName" runat="server">lbl_WEName</asp:Label>
                                                                <span lang="de">&nbsp;</span>
                                                            </td>
                                                            <td class="TextLarge" width="340" bgcolor="Silver">
                                                                <asp:TextBox ID="txtWE_Name" runat="server" MaxLength="40" Width="285px"></asp:TextBox>
                                                            </td>
                                                            <td class="TextLarge" colspan="4" bgcolor="Silver">
                                                                (z.B. Erika Mustermann)
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge" align="left" width="215" height="24" bgcolor="Silver">
                                                                <asp:Label ID="lbl_WEStrasse" runat="server">lbl_WEStrasse</asp:Label>&nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" width="340" height="24" bgcolor="Silver">
                                                                <asp:TextBox ID="txtWE_Strasse" runat="server" MaxLength="60" Width="285px"></asp:TextBox>
                                                            </td>
                                                            <td class="TextLarge" align="left" width="49" height="24" bgcolor="Silver">
                                                                <asp:Label ID="lbl_WEHausnummer" runat="server">lbl_WEHausnummer</asp:Label>
                                                            </td>
                                                            <td class="TextLarge" align="left" width="238" height="24" bgcolor="Silver">
                                                                <asp:TextBox ID="txtWE_Hausnummer" runat="server" MaxLength="10" Width="90px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge" width="215" align="left" bgcolor="Silver">
                                                                <asp:Label ID="lbl_WEPostleitzahl" runat="server">lbl_WEPostleitzahl</asp:Label>&nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" width="340" bgcolor="Silver">
                                                                <asp:TextBox ID="txtWE_Postleitzahl" runat="server" MaxLength="5" Width="45px"></asp:TextBox>
                                                            </td>
                                                            <td class="TextLarge" align="left" width="49" bgcolor="Silver">
                                                                <asp:Label ID="lbl_WEOrt" runat="server">lbl_WEOrt</asp:Label>
                                                            </td>
                                                            <td class="TextLarge" align="left" width="238" bgcolor="Silver">
                                                                <asp:TextBox ID="txtWE_Ort" runat="server" MaxLength="40" Width="129px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge" width="215" align="left" bgcolor="Silver">
                                                                <asp:Label ID="lbl_WETel" runat="server">lbl_WETel</asp:Label>&nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" width="340" bgcolor="Silver">
                                                                <asp:TextBox ID="txtWE_Tel" runat="server" Width="129px"></asp:TextBox>&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" colspan="4" bgcolor="Silver">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td colspan="6">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" width="215" align="left">
                                                    <asp:Label ID="lbl_Vermittlernummer" runat="server">lbl_Vermittlernummer</asp:Label>&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td class="TextLarge" width="340">
                                                    <asp:TextBox ID="txtVermittlernummer1" runat="server" MaxLength="3" Width="30px"></asp:TextBox>&nbsp;<asp:Label
                                                        ID="lbl_Bezirk" runat="server">lbl_Bezirk</asp:Label>
                                                    <span lang="de">&nbsp;</span>&nbsp;<asp:TextBox ID="txtVermittlernummer2" runat="server"
                                                        MaxLength="5" Width="53px"></asp:TextBox>&nbsp;
                                                </td>
                                                <td colspan="4">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" width="215" align="left">
                                                    <asp:Label ID="lbl_EmailAdresse" runat="server">lbl_EmailAdresse</asp:Label>&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td class="StandardTableAlternate" width="655" colspan="4">
                                                    <asp:TextBox ID="txtEmailAdresse" runat="server" MaxLength="241" Width="450px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" width="215" align="left">
                                                    <asp:Label ID="lbl_KeineEmailVorhanden" runat="server">lbl_KeineEmailVorhanden</asp:Label>&nbsp;&nbsp;
                                                </td>
                                                <td class="TextLarge" width="340">
                                                    <asp:CheckBox ID="chkKeineEmailVorhanden" runat="server"></asp:CheckBox>
                                                </td>
                                                <td class="TextLarge" width="49">
                                                </td>
                                                <td class="TextLarge" align="right" width="238">
                                                </td>
                                                <td class="TextLarge" width="92">
                                                </td>
                                                <td class="TextLarge">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="StandardTableAlternate" width="215" align="left">
                                                    <asp:Label ID="lbl_AnzahlKennzeichen" runat="server">lbl_AnzahlKennzeichen</asp:Label>&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td class="StandardTableAlternate" width="340">
                                                    <asp:DropDownList ID="ddlAnzahlKennzeichen" runat="server" Width="129px">
                                                        <asp:ListItem>1</asp:ListItem>
                                                        <asp:ListItem>2</asp:ListItem>
                                                        <asp:ListItem>3</asp:ListItem>
                                                        <asp:ListItem>4</asp:ListItem>
                                                        <asp:ListItem>5</asp:ListItem>
                                                        <asp:ListItem>6</asp:ListItem>
                                                        <asp:ListItem>7</asp:ListItem>
                                                        <asp:ListItem>8</asp:ListItem>
                                                        <asp:ListItem>9</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="StandardTableAlternate" width="49">
                                                    &nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td class="StandardTableAlternate" align="right" width="238">
                                                    &nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td class="StandardTableAlternate" width="92">
                                                </td>
                                                <td class="StandardTableAlternate">
                                                    &nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" width="215" align="left">
                                                    <asp:Label ID="lbl_Versandart" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                    <td class="TextLarge" width="215" align="right">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td class="TextLarge" colspan="5">
                                        <asp:RadioButton ID="rbNormal" runat="server" GroupName="grpVersandart" Text="Standard"
                                            Checked="True"></asp:RadioButton>
                                        <span lang="de">&nbsp;<i><font color="black">(Versandkosten trägt Versicherungsgesellschaft)</font></i></span>
                                    </td>
                                    <td class="TextLarge">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TextLarge" width="215" align="right">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td class="TextLarge" colspan="5">
                                        <asp:RadioButton ID="rbExpress" runat="server" GroupName="grpVersandart" Text="Express">
                                        </asp:RadioButton>
                                        &nbsp;<i><font color="black">(Versandkosten <b>in Höhe von 38,68 Euro</b> trägt der
                                            Vermittler)</font></i>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <table class="InfoText" id="Table4" cellspacing="1" cellpadding="1" border="0">
                    <tr>
                        <td>
                            <strong><u>Hinweis:<br>
                            </u></strong>Die Deutsche Post AG garantiert für&nbsp;<b><i>Standardsendungen</i></b>
                            keine Zustellzeiten<br>
                            und gibt die Zustellwahrscheinlichkeit wie folgt an:&nbsp;
                            <br>
                            <br>
                            &nbsp;&nbsp;&nbsp;- 95% aller Sendungen werden dem Empfänger innerhalb von 24 Stunden
                            zugestellt,<br>
                            &nbsp;&nbsp;&nbsp;- 3% aller Sendungen benötigen zwischen 24 und 48 Stunden bis
                            zur Zustellung.<br>
                            <br />
                            <span lang="de"><b><i>Expresssendungen</i></b> erfolgen bis 12:00 Uhr des nächsten Tages.<br />
                                Vorraussetzung für eine zeitgerechte Lieferung ist der Bestelleingang bis 16:00
                                Uhr. </span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td valign="top" align="left">
                <!--#include File="../../../PageElements/Footer.html" -->
                &nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    &nbsp;&nbsp;&nbsp;&nbsp; </TD></TR></TBODY></TABLE></form>
</body>
</html>
