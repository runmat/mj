<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change45.aspx.vb" Inherits="CKG.Components.ComCommon.Change45" %>

<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SucheHaendler" Src="PageElements/SucheHaendler.ascx" %>
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
    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" border="0">
        <tr>
            <td colspan="2">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td class="PageNavigation" colspan="2">
                <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
            </td>
        </tr>
        <tr>
            <td class="TaskTitle" valign="top" colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="TABLEX" width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td valign="top" width="100">
                <asp:LinkButton ID="lbSuche" runat="server" CssClass="StandardButton">&#149;&nbsp;Weiter</asp:LinkButton>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td width="100">
                <asp:LinkButton ID="lbSelektionZurueckSetzen" runat="server" Visible="false" CssClass="StandardButton">&#149;&nbsp;Neue Suche</asp:LinkButton>
            </td>
            <td>
            </td>
            <tr>
                <td width="100">
                    &nbsp;
                </td>
                <td align="left">
                    <table id="Table1" cellspacing="0" width="100%" bgcolor="white" border="0">
                        <tr id="tr1" runat="server">
                            <td colspan="1" align="left" class="TaskTitle">
                                <asp:Label runat="server" ID="lbl_AnzeigeHaendlerSuche" Font-Underline="true" Font-Bold="true"
                                    ForeColor="Black" Font-Size="Larger" Text="lbl_AnzeigeHaendlerSuche"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="border-color: #f5f5f5; border-style: solid; border-width: 3;">
                                <uc1:SucheHaendler runat="server" ID="SucheHaendler1" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
               <tr>
                                 <td width="100">
                                        &nbsp;
                                    </td>
                                    <td  align="left" class="TaskTitle">                                    
                                  <asp:Label runat="server" ID="lbl_AnzeigeFahrzeugSuche" Font-Underline="true" Font-Bold="true" ForeColor="Black" Font-Size="Larger" Text="lbl_AnzeigeFahrzeugSuche"></asp:Label>
                                    </td>
                                                                     
                                </tr>         
            <tr>
                <td width="100">
                    &nbsp;
                </td>
                <td align="left"  style="border-color:#f5f5f5; border-style:solid; border-width:3;">
                    <table id="Table3" cellspacing="0" width="100%" bgcolor="white" border="0">
                        <tr class="TextLarge">
                            <td width="147" class="TextLarge">
                                <asp:Label runat="server" ID="lbl_FIN"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFIN" Width="200" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="80">
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="80">
                    &nbsp;
                </td>
                <td>
                    <!--#include File="../../../PageElements/Footer.html" -->
                </td>
            </tr>
    </table>
    </form>
</body>
</html>
