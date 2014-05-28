<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change07.aspx.vb" Inherits="AppF1.Change07" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SucheHaendler" Src="../controls/SucheHaendler.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BusyIndicator" Src="../../../PageElements/BusyIndicator.ascx" %>



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
    
    <uc1:BusyIndicator runat="server" />

    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center">
        <tr>
            <td colspan="2">
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td colspan="2">
             
                    
                    
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="3">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" runat="server"> (Fahrzeugsuche)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TaskTitle" colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                                       <tr>
                        <td width="100">
                            <asp:LinkButton ID="cmdSearch"  OnClientClick="Show_BusyBox1();" runat="server" CssClass="StandardButton">Weiter</asp:LinkButton>
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td width="100">
                            <asp:LinkButton ID="lbSelektionZurueckSetzen" runat="server" Visible="false" CssClass="StandardButton">Neue Suche</asp:LinkButton>
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                    <td width="100"> &nbsp;</td>
                    
                    
                        <td colspan="2" align="left">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                 <tr>
                                
                                    <td >
                                       &nbsp;
                                      </td>                                      
                                                                     </tr>
                                <tr id="tr1" runat="server">
                                   
                                    <td colspan="1" align="left" class="TaskTitle">
                                        <asp:Label runat="server" ID="lbl_AnzeigeHaendlerSuche" Font-Underline="true" Font-Bold="true"
                                            ForeColor="Black" Font-Size="Larger" Text="lbl_AnzeigeHaendlerSuche"></asp:Label>
                                    </td>
                                </tr>
                               
                                <tr id="tr_HaendlerNr1" runat="server">
                                    
                                    <td colspan="2" style="border-color: #f5f5f5; border-style: solid; border-width: 3;">
                                        <uc1:SucheHaendler ID="SucheHaendler1" runat="server"></uc1:SucheHaendler>
                                    </td>
                                    
                                </tr>
                              
                                       
                                   
                                
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table99" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td width="100">
                            &nbsp;
                        </td>
                        <td colspan="3" align="left">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
       </tr>
        <tr>
           <td>
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    </form>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
</body>
</html>
