<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%--<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>--%>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change205.aspx.vb" Inherits="AppARVAL.Change205" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" enctype="multipart/form-data" runat="server">
    <table width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr>
                            <td class="PageNavigation" colspan="2">
                                <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"
                                    Visible="False"> (Fahrzeugsuche)</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="120" height="192">
                                <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                    border="0">
                                    <tr>
                                        <td class="TaskTitle" width="120">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr id="trcmdUpload" runat="server">
                                        <td valign="center" width="120">
                                            <asp:LinkButton ID="cmdSearch" runat="server" CssClass="StandardButton"> &#149;&nbsp;Suchen</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr id="trcmdSearch" runat="server">
                                        <td valign="center" width="120">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top" align="right">
                                <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td class="TaskTitle" valign="top" align="right">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblSelection" cellspacing="0" cellpadding="0" width="100%" border="0"
                                    runat="server">
                                    <tr>
                                        <td valign="top" align="left">
                                            <table id="Table1" cellspacing="0" cellpadding="5" width="100%" border="0">
                                                <tr>
                                                    <td class="TextLarge" valign="top" nowrap align="left">
                                                        <table id="Table8" cellspacing="1" cellpadding="1" border="0">
                                                            <tr>
                                                                <td nowrap>
                                                                    <asp:Label ID="Label1" runat="server">Leasingvertrags-Nr.</asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td class="TextLarge">
                                                        <font size="1">
                                                            <table id="Table7" cellspacing="1" cellpadding="1" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOrdernummer" runat="server" MaxLength="10" Width="250px"></asp:TextBox>&nbsp;<font
                                                                            size="1">(1234567)</font>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </font>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TextLarge" valign="top" nowrap align="left">
                                                        <table id="Table9" cellspacing="1" cellpadding="1" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label2" runat="server">Kfz-Kennzeichen*</asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td class="" height="37">
                                                        <font size="1">
                                                            <table id="Table5" cellspacing="1" cellpadding="1" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAmtlKennzeichen" runat="server" MaxLength="9" Width="250px"></asp:TextBox>&nbsp;<font
                                                                            size="1">(XX-Y1234)</font>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </font>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TextLarge" nowrap align="left">
                                                    </td>
                                                    <td>
                                                        *<font size="1">Eingabe von nachgestelltem Platzhalter möglich. Mindestens Kreis und
                                                            ein Buchstabe (z.B. XX-Y*)</font>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="" nowrap align="left" colspan="2">
                                                        <input id="cbxDatei" style="display: none" type="checkbox" name="cbxDatei" runat="server">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="" valign="top" nowrap align="left">
                                                        <a class="StandardButtonTable" href="javascript:showhide()">•&nbsp;Dateiauswahl</a>&nbsp;
                                                        </STRONG><a href="javascript:openinfo('Info01.htm');"><img src="/Portal/Images/fragezeichen.gif"
                                                            border="0"></a>
                                                    </td>
                                                    </STRONG>
                                                    <td class="" valign="top" align="left" width="100%">
                                                        <table id="Table3" cellspacing="1" cellpadding="1" border="0">
                                                            <tr id="trDateiauswahl" style="display: none">
                                                                <td>
                                                                    <input class="InfoBoxFlat" id="upFile" type="file" size="40" name="File1" runat="server">
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="cmdContinue" runat="server" CssClass="StandardButtonTable"> &#149;&nbsp;Weiter&nbsp;&#187;</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Label ID="lblExcelfile" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="" nowrap align="left">
                                                    </td>
                                                    <td class="" width="100%">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <tr>
                                <td valign="top" width="120">
                                    &nbsp;
                                </td>
                                <td valign="top">
                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" width="120">
                                    &nbsp;
                                </td>
                                <td align="right">
                                    <!--#include File="../../../PageElements/Footer.html" -->
                                </td>
                            </tr>
                            <script language="JavaScript">										
						<!--
                                function showhide() {
                                    o = document.getElementById("trDateiauswahl").style;
                                    if (o.display != "none") {
                                        o.display = "none";
                                        document.forms[0].txtOrdernummer.disabled = false;
                                        document.forms[0].txtAmtlKennzeichen.disabled = false;
                                        document.forms[0].cbxDatei.checked = false;
                                        window.document.Form1.txtOrdernummer.focus();
                                    } else {
                                        o.display = "";
                                        document.forms[0].txtOrdernummer.disabled = true;
                                        document.forms[0].txtAmtlKennzeichen.disabled = true;
                                        document.forms[0].cbxDatei.checked = true;
                                        document.forms[0].upFile.focus();
                                    }
                                }
                                function openinfo(url) {
                                    fenster = window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=650,height=250");
                                    fenster.focus();
                                }
						-->
                            </script>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>
    </form>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
</body>
</html>
