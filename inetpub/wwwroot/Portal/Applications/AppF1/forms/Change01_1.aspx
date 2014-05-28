<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../controls/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01_1.aspx.vb" Inherits="AppF1.Change01_1" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>


<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>

 <script language="JavaScript" type="text/javascript">
        function openinfo(url) {
            fenster = window.open(url, "GMAC", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=500,height=200");
            fenster.focus();
        }
</script>

<body topmargin="0" leftmargin="0" ms_positioning="FlowLayout">
    
    <uc1:BusyIndicator runat="server" />

    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center">
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
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" runat="server"> (Fahrzeugsuche)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="100">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle" width="150">
                                        <asp:LinkButton ID="cmdSearch" OnClientClick="Show_BusyBox1();" runat="server" CssClass="StandardButton">Suchen</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <uc1:Kopfdaten ID="Kopfdaten1" runat="server"></uc1:Kopfdaten>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        <table id="Table1" cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr id="tr_Fahrgestellnummer" runat="server">
                                                <td class="TextLarge" width="150">
                                                    <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label><br>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td class="TextLarge">
                                                    <asp:TextBox ID="txtFahrgestellNr" runat="server" Width="250px" MaxLength="35"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="tr1" runat="server">
                                                <td class="TextLarge" width="150">
                                                    <asp:Label ID="lbl_ZBII" runat="server">lbl_ZBII</asp:Label><br>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td class="TextLarge">
                                                    <asp:TextBox ID="txt_ZBII" runat="server" Width="250px" MaxLength="35"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    (Eingabe von Platzhaltern möglich, z.B. *12345 oder 12345*)
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table id="Table4" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" border="0">
                                            <tr>
                                                <td colspan="1" align="left" class="TaskTitle">
                                                    <asp:Label ID="lbl_Info" Text="Dokumentenanforderung-Excel-Datei-Auswahl" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="border-color: #f5f5f5; border-style: solid; border-width: 3;">
                                                    <table id="tbl0001" cellspacing="0" cellpadding="5" width="100%" border="0">
                                                        <tr>
                                                            <td class="TextLarge" nowrap="nowrap" align="right">
                                                                Dateiauswahl <a href="javascript:openinfo('Info01.htm');">
                                                                    <img src="../../../images/fragezeichen.gif" border="0" /></a>:&nbsp;&nbsp;
                                                            </td>
                                                            <td class="TextLarge" width="100%">
                                                                <input id="upFile" type="file" size="49" name="File1" runat="server" />&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TextLarge" nowrap="nowrap" align="right">
                                                                &nbsp;
                                                            </td>
                                                            <td class="TextLarge">
                                                                &nbsp;
                                                                <asp:Label ID="lblExcelfile" runat="server" EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
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
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
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
    </table>
    </form>
</body>
</html>
