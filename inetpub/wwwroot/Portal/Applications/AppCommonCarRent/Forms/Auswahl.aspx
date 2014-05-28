<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Auswahl.aspx.vb" Inherits="AppCommonCarRent.Auswahl" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    
    <uc1:BusyIndicator runat="server" />

    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" align="center">
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
                                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TaskTitle" valign="top" colspan="2">
                                &nbsp;
                            </td>
            </td>
        </tr>
        <tr>
            <td valign="top" width="120">
                <table id="Table2" cellspacing="0" cellpadding="0" bgcolor="white" border="0">
                    <tr id="trCreate" runat="server">
                        <td valign="center">
                            <asp:LinkButton ID="lb_Weiter" OnClientClick="Show_BusyBox1();" Text="Weiter" runat="server"
                                CssClass="StandardButton" Height="16px" Width="120px"></asp:LinkButton>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="center">
                            &nbsp;</td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td width="100">
            </td>
            <td valign="top">
                <table id="Table1" cellspacing="0" cellpadding="5" width="100%" bgcolor="white" 
                    >
                    <tr id="tr_Leasingvertragsnummer" class="TextLarge">
                      
                           <td align="center" width="25%">
                               &nbsp;</td>
                      
                           <td align="left" width="50%" style="border: 1px solid #DFDFDF">
                               <asp:RadioButtonList ID="rbl_Auswahl" runat="server">
                                   <asp:ListItem>Bezahltkennzeichen setzen</asp:ListItem>
                                   <asp:ListItem>Adresszuweisung</asp:ListItem>
                                   <asp:ListItem>Setzen und Aufhebung von Sperren</asp:ListItem>
                                   <asp:ListItem>Versandfreigabe</asp:ListItem>
                               </asp:RadioButtonList>
                           </td>
                      
                           <td align="left" width="25%">
                               &nbsp;</td>
                    </tr>
                    
                    </table>
            </td>
        </tr>
        <tr>
            <td valign="top" width="100">
                &nbsp;
            </td>
            <td align="left" colspan="2">
                &nbsp;</td>
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
    </form>
</body>
</html>
